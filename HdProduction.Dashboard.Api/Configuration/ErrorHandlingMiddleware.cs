using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Exceptions;
using log4net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace HdProduction.Dashboard.Api.Configuration
{
  public class ErrorHandlingMiddleware
  {
    private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next.Invoke(context);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(context, ex);
      }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      string message;
      var statusCode = HttpStatusCode.InternalServerError;

      if (exception is ArgumentException)
      {
        statusCode = exception is EntityNotFoundException ? HttpStatusCode.NotFound : HttpStatusCode.BadRequest;
        message = exception.Message;
      }
      else if (exception is UnauthorizedAccessException)
      {
        statusCode = HttpStatusCode.Unauthorized;
        message = "Unauthorized";
      }
      else
      {
        Logger.Error("Unknown exception", exception);
        message = "Oops. Something went wrong.";
      }

      var result = JsonConvert.SerializeObject(new {message});
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)statusCode;
      await context.Response.WriteAsync(result);
    }
  }
}