using AuthServer.Core.Configiration;
using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Data;
using AuthServer.Data.Repositories;
using AuthServer.Data.UnitOfWork;
using AuthServer.Service.Services;
using CommonLibrary.Configurations;
using CommonLibrary.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using FluentValidation.AspNetCore;

namespace Auth.Server.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*
             * DI nesneleri DI conteyn�r a eklenir. Yani Core,Data ve Service katman�nda register edilmesi gerekir
             * Bunun sebebi Fremwork kar��la�t��� interfacenin, hangi class a denk geldi�ini ve
             * hangi clastan nesne �rne�i alaca�� belirlenir.
             */

            //Tekbir istekte 1 tane nesne �rne�i olu�turur.
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Generic olanlar i�in ekleme
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IServiceGeneric<,>), typeof(ServiceGeneric<,>));


            //DbContext Eklenir.
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlServer"), sqlOptions =>
                {
                    //Migration �n ger�ekle�ece�i Assembly
                    sqlOptions.MigrationsAssembly("AuthServer.Data");
                });
            });


            services.AddIdentity<UserApp, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();



            /*
             * CustomTokenOption u git appsettingjson i�erisinden TakenOption olan� al
             */
            services.Configure<CustomTokenOption>(Configuration.GetSection("TokenOption"));

            //TokenOptionstan nesne �retilir.
            var tokenOptions = Configuration.GetSection("TokenOption").Get<CustomTokenOption>();

            /*
             * Client � git appsettingjson i�erisindenClient olan� al
             */
            services.Configure<List<Client>>(Configuration.GetSection("Clients"));


            /*
             * Token geldi�i taktirde do�rulamak i�in yap�lan i�lemler.
             * 
             */
            services.AddAuthentication(options =>
            {
                //�yelik sisteminde birden fazla login olabilir. Kullan�c� ve Admin gibi. 1 tane login oldu�u i�in bu �ekilde belirtildi.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //A�a��daki kurallar� JwtBearerDefaults.AuthenticationScheme ile e�le�tirmek i�in
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            {
                //Gelen token�n neleri kontrol edilir
                opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    //Gelen token�n verildi�i IdentityServer appsettings dosyas�nda bulunan Issuer de�eriyle ayn� m�?.
                    ValidIssuer = tokenOptions.Issuer,
                    //Gelen token�n Payload�nda bulunan istek at�labilecek api lerde appsettings dosyas�nda bulunan Audiencenin ilk de�eri olmal�d�r.
                    ValidAudience = tokenOptions.Audience[0],
                    //�mza
                    IssuerSigningKey = SignService.GetSymmetricSecurtyKey(tokenOptions.SecurityKey),

                    //�ifre Do�rulama olucak
                    ValidateIssuerSigningKey = true,
                    
                    //�stek att��� url e yetkisi var m� kontrol�n� yap
                    ValidateAudience = true,

                    //Token�n al�nd��� IdentityServer kontrol�
                    ValidateIssuer = true,

                    //Token�n �mr� kontrol� yap
                    ValidateLifetime = true,

                    //Default olarak verilen 5 dakikal�k sarkmay� s�f�r olarak belirtildi (Token �n �mr�)
                    ClockSkew = TimeSpan.Zero

                };
                
            });

            //services.AddControllers().(optipons =>
            //{
            //    optipons.RegisterValidatorsFromAssemblyContaining<Startup>();
            //});
            //services.UseCustomValidationResponse();

            services.AddControllers().AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Startup>();
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth.Server.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth.Server.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
