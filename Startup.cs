using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autenticacao.Repositorie;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Autenticacao
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
               {
                   option.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = "Iterup.Security.Bearer",
                       ValidAudience = "Iterup.Security.Bearer",
                       IssuerSigningKey = ProviderJWT.JwtSecurityKey.Create("Secret_Key_Iterup")
                   };

                   option.Events = new JwtBearerEvents
                   {
                       OnAuthenticationFailed = context =>
                       {
                           Console.WriteLine($"OnAuthenticationFailed: {context.Exception.Message}");
                           return Task.CompletedTask;
                       },

                       OnTokenValidated = context =>
                       {
                           Console.WriteLine($"OnTokenValidated: {context.SecurityToken}");
                           return Task.CompletedTask;
                       }
                   };
               });

            services.AddAuthorization(options =>
           {
               options.AddPolicy("UsuarioAPI",
                   policy => policy.RequireClaim("UsuarioApiNumero")
                   );
           });

            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddSingleton<IPersonRepository, PersonRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
               {
                   await context.Response.WriteAsync("Api Desafio C#!");
                });
            });

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
    
        }
    }
}
