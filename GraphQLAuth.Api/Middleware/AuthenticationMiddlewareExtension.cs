using Microsoft.AspNetCore.Builder;

namespace GraphQLAuth.Api.Middleware
{
    public static class AuthenticationMiddlewareExtension
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
