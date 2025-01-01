using AuthServer.Core.Configiration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using CommonLibrary.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        //CLientlar için
        private readonly List<Client> _clients;

        //Token oluşturabilmek için
        private readonly ITokenService _tokenService;

        //Üyelikle ilgili işlemler için (Kullanıcı var mı yok mu)
        private readonly UserManager<UserApp> _userManager;

        //Veri tabanı kayıt için
        private readonly IUnitOfWork _unitOfWork;

        //UserRefreshToken için
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenService;


        /*
         * Clientlar apsetting dosyasından okunacağı için IOptions tan alınır.
         * Token üretebilmek için ITOkenService kullanılır
         * Kullanıcı işlemleri için UserManeger<UserApp> kullanılır.
         * Veri tabanı işlemleri için IUnitOfWork kullanılır
         */
        public AuthenticationService(IOptions<List<Client>> clients, ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork,
            IGenericRepository<UserRefreshToken> userRefreshTokenService)
        {
            _clients = clients.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshTokenService = userRefreshTokenService;
        }

        //Üyelik gerektiren token
        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {

            //LoginDto kontrolü
            if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            //Kullanıcı kontrol edilir
            if (user == null) return Response<TokenDto>.Fail("Email or Password is wrong",400,true);

            //Şifre kontrol
            if(!await _userManager.CheckPasswordAsync(user, loginDto.Password)) 
                return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);

            //Token Üretilit
            var token = _tokenService.CreateToken(user);

            //Daha öncesinde ResfreshToken olup olmadığı kontrol edilir.
            //(SingleOrDefault metodu koleksiyonda, listede bulunan değerlerden şartımıza
            //uyan değeri tek kayıt olarak bize geri döner. Birden fazla ise hata verir.)
            var userRefreshToken = await _userRefreshTokenService
                .Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

            //RefreshToken yok ise
            if (userRefreshToken == null)
            {
                await _userRefreshTokenService.AddAsync(new UserRefreshToken {
                    UserId = user.Id,
                    RefreshTokenCode = token.RefreshToken,
                    Expretion = token.RefreshTokenExpiration 
                });
            }
            else
            {
                userRefreshToken.RefreshTokenCode = token.RefreshToken;
                userRefreshToken.Expretion = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(token, 200);
        }

        //Üyelik gerektirmeyen token
        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            //Client alınır.
            var userClient = _clients.FirstOrDefault(x => x.ClientId == clientLoginDto.ClientId && x.ClientSecret == clientLoginDto.ClientSecret);

            //Client kontrol edilir
            if (userClient == null) return Response<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 404, true);

            //Token Üretilir
            var token = _tokenService.CreateClientToken(userClient);

            return Response<ClientTokenDto>.Success(token, 200);
            
        }

        public Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        //RefreshToken ı geçersiz kılmak
        public Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
