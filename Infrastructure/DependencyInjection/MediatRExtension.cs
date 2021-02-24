using Application.Common.PipelineBehaviors;
using Application.Guilds.Commands.CreateGuild;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.DependencyInjection
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
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(MapperBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(NotFoundResponseBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        }
    }
}