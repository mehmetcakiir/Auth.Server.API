using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using CommonLibrary.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class UserService : IUserService
    {
        //Kullanıcı ile ilgili işlem olduğu için
        private readonly UserManager<UserApp> _userManager;

        public UserService(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        } 


        //Yeni bir kullanıcı oluşturulacak
        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new UserApp
            {
                Email = createUserDto.Email,
                UserName = createUserDto.UserName
            };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            //Username daha önce alındı gibi hatalar döner
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return Response<UserAppDto>.Fail(new ErrorDto(errors,true), 400);
            }
            return Response<UserAppDto>.Success(ObjectMapper.mapper.Map<UserAppDto>(user), 200);
        }


        //Kullanıcı dönülür.
        public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return Response<UserAppDto>.Fail("User not found", 404, true);
            }
            return Response<UserAppDto>.Success(ObjectMapper.mapper.Map<UserAppDto>(user), 404);
        }
    }
}
