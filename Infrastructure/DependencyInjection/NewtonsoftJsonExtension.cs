using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Infrastructure.DependencyInjection
{
    public static class NewtonsoftJsonExtension
    {
        public static IMvcBuilder BootstrapNewtonsoftJson(this IMvcBuilder builder)
        {
            return builder.AddNewtonsoftJson(options =>
             {
                 options.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                 options.SerializerSettings.Formatting = Formatting.Indented;
                 options.SerializerSettings.Converters.Add(new StringEnumConverter());
             });
        }
    }
}
