using GraphQL.Authorization;
using GraphQL.Language.AST;
using GraphQL.Server;
using GraphQL.Types;
using GraphQL.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLAuth.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton(s =>
            {
                var definitions = @"
                  type User {
                    id: ID
                    name: String
                  }
                  type Query {
                    viewer: User
                    users: [User]
                  }
                ";
                var schema = Schema.For(
                    definitions,
                    _ =>
                    {
                        _.Types.Include<Query>();
                    });

                //schema.FindType("User").AuthorizeWith("AdminPolicy");

                return schema;
            });

            // extension method defined in this project
            services.AddGraphQLAuth((_, s) =>
            {
                _.AddPolicy("MinimumAge", p => p.AddRequirement(new MinimumAgeRequirement(200)));
                _.AddPolicy("App1", p => p.RequireClaim("app", "1"));
                _.AddPolicy("App2", p => p.RequireClaim("app", "2"));
            });

            services
                .AddGraphQL(options =>
                {
                    options.ExposeExceptions = true;
                })
                .AddUserContextBuilder(context => new GraphQLUserContext { User = context.User });

            // Kestrel workaround
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // IIS workaround
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer("SampleApp1", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuer = true,
                        ValidIssuer = "Dominik",
                        ValidateAudience = true,
                        ValidAudience = "SampleApp",
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("zXyi0iWkUcJ2XlCdZ5NscHJBwioXQl")),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromDays(300)
                    };
                })
                .AddJwtBearer("SampleApp2", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuer = true,
                        ValidIssuer = "Dominik",
                        ValidateAudience = true,
                        ValidAudience = "SampleApp",
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("zXyi0iWkUcJ2XlCdZ5NscHJBwioXQl")),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromDays(300)
                    };
                });

            // Add another IValidationRule, there will be always one because AddGraphQLAuth
            services.AddTransient<IValidationRule, RequiresAuthenticationValidationRule>();
            services.AddTransient<IValidationRule, RequiresOtherValidationRule>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCustomMiddleware();
            app.UseAuthentication();
            //app.UseAuthorization();

            var validationRules = app.ApplicationServices.GetServices<IValidationRule>();

            app.UseGraphQL<ISchema>("/graphql");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class CustomMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddleware>();
        }
    }

    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var service = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            var handler = await service.GetHandlerAsync(httpContext, "SampleApp1");

            var authenticateResult = await handler.AuthenticateAsync();

            if (authenticateResult.Succeeded)
                await _next(httpContext);
            else
            {
                httpContext.Response.StatusCode = 401;
            }
        }
    }

    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get; }

        public MinimumAgeRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }

        public Task Authorize(AuthorizationContext context)
        {
            var minimumAge = int.Parse(context.User.FindFirst(c => c.Type == "age")?.Value ?? "0");
            if (minimumAge < 21)
                context.ReportError("MinimumAge");

            return Task.CompletedTask;
        }
    }

    public class RequiresOtherValidationRule : IValidationRule
    {
        public INodeVisitor Validate(ValidationContext context)
        {
            var userContext = context.UserContext as GraphQLUserContext;
            var authenticated = userContext.User.Identity.IsAuthenticated;

            return new EnterLeaveListener(_ =>
            {
                // Just example
                _.Match<Field>(op =>
                {
                    // Some logic
                });
            });
        }
    }

    public class RequiresAuthenticationValidationRule : IValidationRule
    {
        public INodeVisitor Validate(ValidationContext context)
        {
            var userContext = context.UserContext as GraphQLUserContext;
            var authenticated = userContext.User.Identity.IsAuthenticated;

            return new EnterLeaveListener(_ =>
            {
                if (authenticated == false)
                    context.ReportError(new ValidationError(context.OriginalQuery, "401", "Unauthorized"));
            });
        }
    }
}
