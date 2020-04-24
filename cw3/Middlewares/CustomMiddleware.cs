using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace cw3
{
    public class CustomMiddleware
    {
        private RequestDelegate _next;
        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Add("xyz", "999");

            await _next.Invoke(context); 
        }
    }
}
