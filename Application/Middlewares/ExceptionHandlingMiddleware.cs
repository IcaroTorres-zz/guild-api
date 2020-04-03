using Business.ResponseOutputs;
using MediatR;
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

    public async Task Invoke(HttpContext context, IMediator mediator, IWebHostEnvironment env /* other dependencies */)
    {
      try
      {
        await next(context);
      }
      catch (DbUpdateException dbex)
      {
        await HandleExceptionAsync(dbex, context, env.IsDevelopment());
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(ex, context, env.IsDevelopment());
      }
    }

    private static Task HandleExceptionAsync(DbUpdateException ex, HttpContext context, bool isDevelopment = false)
    {
      var errorOutput = new ApiErrorOutput()
      {
        Title = "Database violation error.",
        Status = (int)HttpStatusCode.Conflict
      };
      var DbProductionMessage = "Some constraints were violated. Please contact system's administrators.";
      var messages = new string[] { isDevelopment ? ex.Message : DbProductionMessage };

      errorOutput.AddErrorEntry("Database", messages);

      return WriteCustomOutputAsync(errorOutput, context);
    }

    private static Task HandleExceptionAsync(Exception ex, HttpContext context, bool isDevelopment = false)
    {
      var errorOutput = new ApiErrorOutput()
      {
        Title = "Unexpected Execution error.",
        Status = (int)HttpStatusCode.InternalServerError
      };
      var executionProductionMessage = "Sorry! A failure occurred durring request. Please contact system's administrators.";
      var messages = isDevelopment
          ? ExtractExceptionMessages(ex).ToArray()
          : new string[] { executionProductionMessage };

      errorOutput.AddErrorEntry("Execution", messages);

      return WriteCustomOutputAsync(errorOutput, context);
    }

    private static Task WriteCustomOutputAsync(ApiErrorOutput errorOutput, HttpContext context)
    {
      var stringResult = JsonConvert.SerializeObject(errorOutput, new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
      });
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = errorOutput?.Status ?? 500;
      return context.Response.WriteAsync(stringResult);
    }

    private static List<string> ExtractExceptionMessages(Exception ex)
    {
      if (ex is null) return new List<string>();

      var errors = ExtractExceptionMessages(ex.InnerException);
      errors.Add(ex.Message);

      return errors;
    }
  }
}