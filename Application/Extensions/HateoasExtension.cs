using Application.Hateoas;
using Application.Hateoas.Members;
using Domain.Hateoas;
using HateoasNet.DependencyInjection.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
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