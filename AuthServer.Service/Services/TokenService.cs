using AuthServer.Core.Configiration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using CommonLibrary.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
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
         * Claim ifadesi OAuth 2.0 ile geln bir ifadedir
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
        private IEnumerable<Claim> GetCliem(UserApp userApp, List<String> audences)
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
            userList.AddRange(audences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            return userList;
        }



        public ClientTokenDto CreateClientToken(Client client)
        {
            throw new NotImplementedException();
        }


        //Token gerektirmeyen public api leri için Token alınır (Hava durumu api sine istek atabilmek için)
        public TokenDto CreateToken(UserApp userApp)
        {
            throw new NotImplementedException();
        }
    }
}
