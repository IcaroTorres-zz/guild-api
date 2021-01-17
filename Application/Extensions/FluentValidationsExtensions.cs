using Business.Usecases.Guilds.CreateGuild;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class FluentValidationsExtensions
    {
        public static IMvcBuilder BootstrapValidators(this IMvcBuilder builder)
        {
            return builder.AddFluentValidation(fv =>
             {
                 fv.ConfigureClientsideValidation(enabled: false);
                 fv.ImplicitlyValidateChildProperties = true;
                 fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                 fv.RegisterValidatorsFromAssemblyContaining<CreateGuildValidator>();
                 fv.ValidatorOptions.CascadeMode = FluentValidation.CascadeMode.Stop;
             });
        }
    }
}