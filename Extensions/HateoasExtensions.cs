using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hateoas
{
    public static class HateoasExtensions
    {
        public static IMvcCoreBuilder AddHateoasResources(this IMvcCoreBuilder builder, Action<HateoasOptions> options = null)
        {
            if (options != null)
            {
                builder.Services.Configure(options);
            }
            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            builder.AddMvcOptions(o => o.OutputFormatters.Add(new JsonHateoasFormatter()));
            return builder;
        }
    }
}
