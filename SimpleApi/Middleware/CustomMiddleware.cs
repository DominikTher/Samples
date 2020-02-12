using Microsoft.AspNetCore.Http;
using SimpleApi.Response;
using System.Threading.Tasks;

namespace SimpleApi
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Value != "/")
            {
                await _next(httpContext);
                return;
            }

            var cutomResponse = new CustomResponse();
            await cutomResponse.Write(httpContext);
        }
    }
}