using AuthServer.Core.Configiration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using CommonLibrary.Dtos;
using Microsoft.AspNetCore.Identity;
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
        //Lientlar için
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

        public Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ClientLoginDto>> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
