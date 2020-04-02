using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

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

        public async Task Invoke(HttpContext context, IMediator mediator /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (DbUpdateException dbex)
            {
                await HandleExceptionAsync(context, dbex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, DbUpdateException ex)
        {
            return WriteResponseAsync(context, (int)HttpStatusCode.Conflict, new[] { ex.Message });
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            return WriteResponseAsync(context, (int)HttpStatusCode.InternalServerError, GenerateExceptionErrorMessages(ex));
        }

        private static Task WriteResponseAsync(HttpContext context, int status, IEnumerable<object> errors)
        {
            var stringResult = JsonConvert.SerializeObject(new { status, errors }, Formatting.Indented);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = status;
            return context.Response.WriteAsync(stringResult);
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