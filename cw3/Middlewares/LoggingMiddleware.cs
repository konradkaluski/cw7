using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace cw3.Middlewares
{
    public class LoggingMiddleware
    {

        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
            if(httpContext.Request != null)
            {
                string path = httpContext.Request.Path;
                string method = httpContext.Request.Method;
                string queryStr = httpContext.Request.QueryString.ToString();
                string bodyStr = "";
                string log;

                using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                    httpContext.Request.Body.Position = 0;
                }

                log = "Path: " + path + " \n" +
                    "Method: " + method + " \n" +
                    "QueryString: " + queryStr + " \n" +
                    "Body: " + bodyStr + " \n" +
                    " " + " \n";

                File.AppendAllText("requestsLog.txt", log);

            }

            await _next(httpContext);
        }
    }
}
