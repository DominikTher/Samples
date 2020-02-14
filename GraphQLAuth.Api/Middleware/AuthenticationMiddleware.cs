using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace GraphQLAuth.Api.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var app = httpContext.Request.Path.Value.Remove(0, 1);

            var authenticationService = httpContext.RequestServices.GetRequiredService<IAuthenticationService>() as AuthenticationService;
            authenticationService.Options.DefaultAuthenticateScheme = app;
            authenticationService.Options.DefaultChallengeScheme = app;

            // TODO: Below validation cause, that whole app is validated adn we are not able to use [Authorize] or [AllowAnonymouse]

            //var service = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            //var handler = await service.GetHandlerAsync(httpContext, app);

            //var authenticateResult = await handler.AuthenticateAsync();

            //if (authenticateResult.Succeeded)
            //{
            //    httpContext.User = authenticateResult.Principal;
            //    await next(httpContext);
            //}
            //else
            //{
            //    httpContext.Response.StatusCode = 401;
            //}

            await next(httpContext);
        }
    }
}
