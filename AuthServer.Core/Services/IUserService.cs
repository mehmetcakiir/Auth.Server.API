using AuthServer.Core.DTOs;
using CommonLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    /*
     * Kullanıcıların kayıt işlemleri için gerekli interface
     */

    /*
     * Kullanıcı kayıt işlemi veri tabanı ile ilgili bir işlem olamasına rağmen bu interfacenin bir benzeri
     * Repositories içerisine oluşturulmadı. Bunun sebebi "Identity" kütüphanesi ile birlikte hazır methodlar gelmektedir.
     */

    public interface IUserService
    {
        //Kullanıcı kayıt
        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);

        //Verilen kullanıcı adı ile veri trabanında kayırlı olan kullanıcı bilgileri getirilir.
        Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
    }
}
