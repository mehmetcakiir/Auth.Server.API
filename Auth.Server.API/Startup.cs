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
             * DI nesneleri DI conteynýr a eklenir. Yani Core,Data ve Service katmanýnda register edilmesi gerekir
             * Bunun sebebi Fremwork karþýlaþtýðý interfacenin, hangi class a denk geldiðini ve
             * hangi clastan nesne örneði alacaðý belirlenir.
             */

            //Tekbir istekte 1 tane nesne örneði oluþturur.
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Generic olanlar için ekleme
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IServiceGeneric<,>), typeof(ServiceGeneric<,>));


            //DbContext Eklenir.
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlServer"), sqlOptions =>
                {
                    //Migration ýn gerçekleþeceði Assembly
                    sqlOptions.MigrationsAssembly("AuthServer.Data");
                });
            });


            services.AddIdentity<UserApp, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();



            /*
             * CustomTokenOption u git appsettingjson içerisinden TakenOption olaný al
             */
            services.Configure<CustomTokenOption>(Configuration.GetSection("TokenOption"));

            //TokenOptionstan nesne üretilir.
            var tokenOptions = Configuration.GetSection("TokenOption").Get<CustomTokenOption>();

            /*
             * Client ý git appsettingjson içerisindenClient olaný al
             */
            services.Configure<List<Client>>(Configuration.GetSection("Clients"));


            /*
             * Token geldiði taktirde doðrulamak için yapýlan iþlemler.
             * 
             */
            services.AddAuthentication(options =>
            {
                //Üyelik sisteminde birden fazla login olabilir. Kullanýcý ve Admin gibi. 1 tane login olduðu için bu þekilde belirtildi.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //Aþaðýdaki kurallarý JwtBearerDefaults.AuthenticationScheme ile eþleþtirmek için
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            {
                //Gelen tokenýn neleri kontrol edilir
                opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    //Gelen tokenýn verildiði IdentityServer appsettings dosyasýnda bulunan Issuer deðeriyle ayný mý?.
                    ValidIssuer = tokenOptions.Issuer,
                    //Gelen tokenýn Payloadýnda bulunan istek atýlabilecek api lerde appsettings dosyasýnda bulunan Audiencenin ilk deðeri olmalýdýr.
                    ValidAudience = tokenOptions.Audience[0],
                    //Ýmza
                    IssuerSigningKey = SignService.GetSymmetricSecurtyKey(tokenOptions.SecurityKey),

                    //Þifre Doðrulama olucak
                    ValidateIssuerSigningKey = true,
                    
                    //Ýstek attýðý url e yetkisi var mý kontrolünü yap
                    ValidateAudience = true,

                    //Tokenýn alýndýðý IdentityServer kontrolü
                    ValidateIssuer = true,

                    //Tokenýn ömrü kontrolü yap
                    ValidateLifetime = true,

                    //Default olarak verilen 5 dakikalýk sarkmayý sýfýr olarak belirtildi (Token ýn ömrü)
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
