using Application.Common.Abstractions;
using Application.Identity;
using FluentValidation;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DependencyInjection
{
    public static class IdentityExtension
    {
        public static IServiceCollection BootstrapIdentity(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
        {
            services
                .AddDbContext<IdentityContext>(options =>
                 {
                     var identityPath = Path.Combine(env.ContentRootPath, "../Infrastructure/Identity", configuration["SQLiteSettings:Identity"]);
                     options.UseSqlite($"Data Source={identityPath}");
                 });

            // users request validators 
            services.AddScoped<IValidator<AuthenticateUserCommand>, AuthenticateModelValidator>()
                    .AddScoped<IValidator<RegisterUserCommand>, RegisterModelValidator>();

            // configure jwt authentication
            var jwtSecret = Encoding.ASCII.GetBytes(configuration["JwtSecret"]);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userName = context.Principal.Identity.Name;
                        var authResponse = userService.GetByName(userName);
                        if (!authResponse.Success)
                        {
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtSecret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
