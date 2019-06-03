using System;
using api.Context;
using api.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace api
{
  public static class ApiGlobalExceptionHandlerExtension
  {
    public static IApplicationBuilder UseWebApiExceptionHandler(this IApplicationBuilder app, IUnitOfWork _unitOfWork)
    {
      return app.UseExceptionHandler(HandleApiException(_unitOfWork));
    }

    public static Action<IApplicationBuilder> HandleApiException(IUnitOfWork _unitOfWork)
    {
      return appBuilder =>
      {
        appBuilder.Run(async context =>
        {
          _unitOfWork.Rollback();
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