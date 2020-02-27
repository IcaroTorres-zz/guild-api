using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Services;
using System;

namespace api
{
    public static class ApiGlobalExceptionHandlerExtension
    {
        public static IApplicationBuilder UseWebApiExceptionHandler(this IApplicationBuilder app, IBaseService service)
        {
            return app.UseExceptionHandler(HandleApiException(service));
        }

        public static Action<IApplicationBuilder> HandleApiException(IBaseService service)
        {
            return appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    service.Rollback();
                    context.Response.StatusCode = 500;
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    await context.Response
                                 .WriteAsync(ErrorMessageBuilder(context, exceptionHandlerFeature != null ? exceptionHandlerFeature.Error.Message
                                                                                                          : "An unexpected fault happened."));
                });
            };
        }
        private static string ErrorMessageBuilder(HttpContext context, string Message = "")
        {
            return $"Fails on {context.Request.Method} to '{context.Request.Path.ToUriComponent()}'. Exception found: {Message}.";
        }
    }
}