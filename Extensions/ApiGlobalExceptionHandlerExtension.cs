using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace api
{
    public static class ApiGlobalExceptionHandlerExtension
    {
        public static IApplicationBuilder UseWebApiExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context => await context.Response.WriteAsync(ErrorMessageBuilder(context)));
            });
        }
        private static string ErrorMessageBuilder(HttpContext context)
        {
            var errors = GenerateExceptionErrorMessages(context.Features.Get<IExceptionHandlerFeature>().Error);
            return JsonConvert.SerializeObject(new
            {
                status = context.Response?.StatusCode ?? 500,
                errors
            }, Formatting.Indented);
        }

        private static List<string> GenerateExceptionErrorMessages(Exception ex)
        {
            if (ex is null)
            {
                return new List<string>();
            }

            var errors = GenerateExceptionErrorMessages(ex.InnerException);
            errors.Add(ex.Message);

            return errors;
        }
    }
}