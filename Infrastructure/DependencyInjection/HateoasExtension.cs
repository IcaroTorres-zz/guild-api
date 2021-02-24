using Application.Common.Abstractions;
using HateoasNet.DependencyInjection.Core;
using Infrastructure.Hateoas;
using Infrastructure.Hateoas.Members;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
    public static class HateoasExtension
    {
        public static IServiceCollection BootstrapHateoas(this IServiceCollection services)
        {
            services.AddScoped<IApiHateoasFactory, ApiHateoasFactory>();
            return services.ConfigureHateoas(context => context.ConfigureFromAssembly(typeof(MemberHateoasBuilder).Assembly));
        }
    }
}