using AuthServer.Core.Configiration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using CommonLibrary.Configurations;
using CommonLibrary.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class TokenService : ITokenService
    {
        //Kullanıcı işlemleri için (UserApp kullanıcısı için)
        private readonly UserManager<UserApp> _userManager;

        //Token oluşturabilmek için
        private readonly CustomTokenOption _customTokenOption;

        //conturaction
        public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOption> customTokenOption)
        {
            _userManager = userManager;
            _customTokenOption = customTokenOption.Value;
        }

        //RefreshToken için string değer  üreten method
        private string CreateRefreshToken()
        {
            //Kaç byte lık bir string refreshToken olacağına karar verilir
            var numberByte = new Byte[32];

            //Random bir değer üretilir.
            using var rnd = RandomNumberGenerator.Create();

            //Üretilen random değerin byte ları alınarak numberByte değişkenine atandı.
            rnd.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);
        }




        /* 
         * Claim ifadesi OAuth 2.0 ile gelen bir ifadedir
         *
         * Bu ifade 2 anlama gelmektedir
         * 
         * 1-) Tokenın payload ı üzerinde, kullanıcı hakkında tutulan datalardır
         * 
         * 2-) Token üzerinde tokenın hangi apilere istek yapabileceği veya token ömri gibi bilgilere de Claim denilmektedir.
         * 
         * Token için oluşturulan tüm Claim ler Tokenın payload içerisinde birer keyValue şeklinde eklenecek.
         */

        //Kullanıcı hakkında bilgiler alınacağı için UserApp sınıfı parametre olarak alınır.
        //Token ın hangi api lere istek yapabileceklerini öğrenebilmek için audences parametre olarak alınır.

        //Üyelik sistemi gerektiren token oluşturma methodu
        private IEnumerable<Claim> GetClaiem(UserApp userApp, List<String> audiences)
        {
            //Kullanıcı ile ilgili bilgiler eklenir.
            var userList = new List<Claim>
            {
                //Payload da olacak kullanıcı Id si
                new Claim(ClaimTypes.NameIdentifier,userApp.Id),

                //Email ? ("email", userApp.Email) olarakta yazılabilir.
                new Claim(JwtRegisteredClaimNames.Email,userApp.Email),

                //UserName
                new Claim(ClaimTypes.Name, userApp.UserName),

                //Her token için random üretilen bir ıd (JwtRegisteredClaimNames.Jti ifadesi tokenıı kimliklendirecek olan propertidir)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            //Token ile ilgili bilgiler eklenir

            //Her oudences i Token ın Payload kısmının Aud kısmına eklenir
            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            return userList;
        }

        //Public Apilere istek yapacak olan client (Üyelik sistemi gerektirmeyen token oluşturma methodu)
        private IEnumerable<Claim> GetCliemByClient(Client client)
        {
            var userClientList = new List<Claim>
            {
                 //Payload da olacak olan token kimin için oluşturuldu
                new Claim(JwtRegisteredClaimNames.Sub, client.ClientId),

                 //Her token için random üretilen bir ıd (JwtRegisteredClaimNames.Jti ifadesi tokenıı kimliklendirecek olan propertidir)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Her oudences i Token ın Payload kısmının Aud kısmına eklenir
            userClientList.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            return userClientList;

        }


        public ClientTokenDto CreateClientToken(Client client)
        {
            //Tokenın ömrü belirtilir. Token oluşturma saatine appsetting içerisinde belirtilen dakikayı ekle
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration);

            //Tokenın imzalanabilmesi için publicKey
            var securityKey = SignService.GetSymmetricSecurtyKey(_customTokenOption.SecurityKey);

            //securty key ile "HmacSha256Signature" algoritması ile token imzalanır.
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //Tokenın Payload kısmı oluşturuldu
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _customTokenOption.Issuer,
                expires: accessTokenExpiration,
                //iki saat aralığında geçerli
                notBefore: DateTime.Now,
                claims: GetCliemByClient(client),
                signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            //Token oluşturuldu
            var token = handler.WriteToken(jwtSecurityToken);

            //Oluşturulan Token TokenDto ya dönüştürülür.
            var clientTokenDto = new ClientTokenDto
            {
                AccessToken = token,
                AccessTokenExpiration = accessTokenExpiration
            };

            return clientTokenDto;
        }


        //Token gerektirmeyen public api leri için Token alınır (Hava durumu api sine istek atabilmek için)
        public TokenDto CreateToken(UserApp userApp)
        {
            //Tokenın ömrü belirtilir. Token oluşturma saatine appsetting içerisinde belirtilen dakikayı ekle
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration);

            //ResfreshToken ın ömrünü velirtir. ResfreshToken oluşturma saatine appsetting içerisinde belirtilen dakikayı ekle
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.ResrefhTokenExpiration);

            //Tokenın imzalanabilmesi için publicKey
            var securityKey = SignService.GetSymmetricSecurtyKey(_customTokenOption.SecurityKey);

            //securty key ile "HmacSha256Signature" algoritması ile token imzalanır.
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //Tokenın Payload kısmı oluşturuldu
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _customTokenOption.Issuer,
                expires: accessTokenExpiration,
                //iki saat aralığında geçerli
                notBefore: DateTime.Now,
                claims: GetClaiem(userApp,_customTokenOption.Audience),
                signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            //Token oluşturuldu
            var token = handler.WriteToken(jwtSecurityToken);

            //Oluşturulan Token TokenDto ya dönüştürülür.
            var tokenDto = new TokenDto
            {
                AccessToken = token,
                AccessTokenExpiration = accessTokenExpiration,
                RefreshToken = CreateRefreshToken(),
                RefreshTokenExpiration = refreshTokenExpiration,
            };

            return tokenDto;
        }
    }
}
