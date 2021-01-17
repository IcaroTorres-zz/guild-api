using Business.Behaviors;
using Business.Usecases.Guilds.CreateGuild;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Extensions
{
    public static class BusinessExtension
    {
        public static IServiceCollection BootstrapPipelinesServices(this IServiceCollection services)
        {
            return services
                // mediatR dependency injection
                .AddMediatR(typeof(CreateGuildHandler).GetTypeInfo().Assembly)

                // mediatR pre-request open-type pipeline behaviors
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(IncludeHateoasBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(NotFoundResponseBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        }
    }
}