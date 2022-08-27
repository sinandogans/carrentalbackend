using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Core.Utilities.ExceptionHandling
{
    public class ExceptionHandlingMiddleware
    {
        readonly RequestDelegate _next;
        string _message;
        int _statusCode;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                _message = ex.Errors.First().ErrorMessage;
                _statusCode = (int)HttpStatusCode.InternalServerError;
                await HandleExceptionAsync(httpContext, _message, _statusCode);
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                _statusCode = (int)HttpStatusCode.InternalServerError;
                await HandleExceptionAsync(httpContext, _message, _statusCode);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, string message, int statusCode)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            var response = JsonSerializer.Serialize(new ExceptionModel()
            {
                Message = message,
                StatusCode = statusCode,
            });
            return httpContext.Response.WriteAsync(response);
        }
    }
}
