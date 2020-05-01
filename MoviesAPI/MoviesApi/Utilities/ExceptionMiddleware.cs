using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MoviesApi.CustomExceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MoviesApi.Utilities
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (NotFoundException ex)
            {
                await HandleNotFoundExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext);
            }
        }

        private Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error from the custom middleware."
            }.ToString());
        }

        private Task HandleNotFoundExceptionAsync(HttpContext context, NotFoundException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            }.ToString());
        }
    }
}
