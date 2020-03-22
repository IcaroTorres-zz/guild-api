using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Application.Middlewares
{
    [ExcludeFromCodeCoverage]
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var status = (int) HttpStatusCode.InternalServerError;
            var errors = GenerateExceptionErrorMessages(ex);
            var result = JsonConvert.SerializeObject(new { status, errors }, Formatting.Indented);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) status;
            return context.Response.WriteAsync(result);
        }

        private static List<string> GenerateExceptionErrorMessages(Exception ex)
        {
            if (ex is null) return new List<string>();

            var errors = GenerateExceptionErrorMessages(ex.InnerException);
            errors.Add(ex.Message);

            return errors;
        }
    }
}