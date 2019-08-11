using api.Repositories;
using Guild.Context;
using Guild.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;

namespace api
{
    public static class ApiGlobalExceptionHandlerExtension
    {
        public static IApplicationBuilder UseWebApiExceptionHandler(this IApplicationBuilder app, IService<ApiContext> service)
        {
            return app.UseExceptionHandler(HandleApiException(service));
        }

        public static Action<IApplicationBuilder> HandleApiException(IService<ApiContext> service)
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
        private static string ErrorMessageBuilder(HttpContext context, string Message = "") =>
            $"Fails on {context.Request.Method} " +
            $"to '{context.Request.Path.ToUriComponent()}'. " +
            $"Exception found: {Message}.";
    }
}