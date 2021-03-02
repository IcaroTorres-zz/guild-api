using Application.Common.Abstractions;
using Application.Common.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

namespace Presentation.Middlewares
{
    [ExcludeFromCodeCoverage]
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUnitOfWork uow, IWebHostEnvironment env)
        {
            try
            {
                await _next(context);
            }
            catch (DbUpdateException dbUpdateException)
            {
                await HandleExceptionAsync(dbUpdateException, context, uow, env.IsProduction());
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(exception, context, uow, env.IsProduction());
            }
        }

        private static Task HandleExceptionAsync(DbUpdateException ex, HttpContext context, IUnitOfWork uow, bool isProduction = false)
        {
            return Handle("Database",
                "Some constraints were violated. Please contact system's administrators.",
                HttpStatusCode.Conflict, context, ex, uow, isProduction);
        }

        private static Task HandleExceptionAsync(Exception ex, HttpContext context, IUnitOfWork uow, bool isProduction = false)
        {
            return Handle("Execution",
                "Unexpected error ocurred. Please contact system's administrators.",
                HttpStatusCode.InternalServerError, context, ex, uow, isProduction);
        }

        private static Task Handle(string title, string message, HttpStatusCode code, HttpContext context, Exception ex, IUnitOfWork uow, bool isProduction)
        {
            if (uow.HasOpenTransaction) uow.RollbackTransactionAsync();
            var errors = new List<Error>() { new Error(title, message) };

            if (!isProduction) errors.AddRange(ex.ToApiError());

            var result = new FailExecutionResult(code, errors.ToArray());
            return WriteErrorAsync(result, context);
        }

        private static Task WriteErrorAsync(FailExecutionResult result, HttpContext context)
        {
            var stringResult = JsonConvert.SerializeObject(result.Value, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Include,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = result.StatusCode ?? 500;
            return context.Response.WriteAsync(stringResult);
        }
    }
}