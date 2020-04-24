using cw3.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace cw3.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext);
            }
            catch (Exception exc)
            {
                await HandleExceptionAsync(httpContext, exc);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception exc)
        {

            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return httpContext.Response.WriteAsync(new Err
            {

                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Blad"
            }.ToString());

            
        }
    }
}
