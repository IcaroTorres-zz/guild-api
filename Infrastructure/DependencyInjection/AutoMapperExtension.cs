using Application.Common.MapperProfiles;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Infrastructure.Identity.MapperProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
    public static class AutoMapperExtension
    {
        public static IServiceCollection BootstrapAutoMapper(this IServiceCollection services)
        {
            return services.AddAutoMapper(cfg => cfg.AddExpressionMapping(), typeof(DomainToApplicationProfile), typeof(IdentityProfile));
        }
    }
}
