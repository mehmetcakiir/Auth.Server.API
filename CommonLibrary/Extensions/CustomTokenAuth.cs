using CommonLibrary.Configurations;
using CommonLibrary.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Extensions
{
    public static class CustomTokenAuth
    {
        public static void AddCustomTokenAuth(this IServiceCollection services, CustomTokenOption tokenOptions)
        {
            /*
             * Token geldiği taktirde doğrulamak için yapılan işlemler.
             * 
            */
            services.AddAuthentication(options =>
            {
                //Üyelik sisteminde birden fazla login olabilir. Kullanıcı ve Admin gibi. 1 tane login olduğu için bu şekilde belirtildi.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //Aşağıdaki kuralları JwtBearerDefaults.AuthenticationScheme ile eşleştirmek için
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            {
                //Gelen tokenın neleri kontrol edilir
                opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    //Gelen tokenın verildiği IdentityServer appsettings dosyasında bulunan Issuer değeriyle aynı mı?.
                    ValidIssuer = tokenOptions.Issuer,
                    //Gelen tokenın Payloadında bulunan istek atılabilecek api lerde appsettings dosyasında bulunan Audiencenin ilk değeri olmalıdır.
                    ValidAudience = tokenOptions.Audience[0],
                    //İmza
                    IssuerSigningKey = SignService.GetSymmetricSecurtyKey(tokenOptions.SecurityKey),

                    //Şifre Doğrulama olucak
                    ValidateIssuerSigningKey = true,

                    //İstek attığı url e yetkisi var mı kontrolünü yap
                    ValidateAudience = true,

                    //Tokenın alındığı IdentityServer kontrolü
                    ValidateIssuer = true,

                    //Tokenın ömrü kontrolü yap
                    ValidateLifetime = true,

                    //Default olarak verilen 5 dakikalık sarkmayı sıfır olarak belirtildi (Token ın ömrü)
                    ClockSkew = TimeSpan.Zero

                };

            });
        }
    }
}
