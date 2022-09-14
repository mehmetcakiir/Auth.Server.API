using AuthServer.Core.DTOs;
using CommonLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IAuthenticationService
    {
        /*
         * Login gerektiren AccessToken almak
         */

        //Kullanıcının Login ile Token alması
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);

        //RefreskToken ile AccessToken almak
        Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);

        //Kullanıcıya ait olan refresh tokenı geçersiz kılmak 
        Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);


        /*
         * Login gerektirmeyen AccessToken almak
         */

        Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
    }
}
