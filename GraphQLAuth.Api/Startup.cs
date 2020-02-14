using GraphQL.Server;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQLAuth.Api.Middleware;
using GraphQLAuth.Api.Requirements;
using GraphQLAuth.Api.Validations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

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
                _.AddPolicy("Reg1", p => p.RequireClaim("reg", "1"));
                _.AddPolicy("Reg2", p => p.RequireClaim("reg", "2"));
            });

            services
                .AddGraphQL(options =>
                {
                    options.ExposeExceptions = false;
                })
                .AddUserContextBuilder(context => new GraphQLUserContext { User = context.User });

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                // TODO: Put some default schema and check what's happened
                .AddJwtBearer("sampleapp1", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuer = true,
                        ValidIssuer = "Dominik",
                        ValidateAudience = true,
                        ValidAudience = "SampleApp1",
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("zXyi0iWkUcJ2XlCdZ5NscHJBwioXQl")),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromDays(300)
                    };
                })
                .AddJwtBearer("sampleapp2", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuer = true,
                        ValidIssuer = "Dominik",
                        ValidateAudience = true,
                        ValidAudience = "SampleApp2",
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("zXyi0iWkUcJ2XlCdZ5NscHJBwioXQl")),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromDays(300)
                    };
                });

            // Add another IValidationRule, there will be always one because AddGraphQLAuth
            services.AddTransient<IValidationRule, RequiresAuthenticationValidationRule>();
            services.AddTransient<IValidationRule, RequiresOtherValidationRule>();

            // Just for default
            services.AddControllers();

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Order matter !!!
            app.UseAuthenticationMiddleware();
            app.UseAuthentication();
            //app.UseAuthorization();

            var validationRules = app.ApplicationServices.GetServices<IValidationRule>();

            app.UseGraphQL<ISchema>("/sampleapp1");
            app.UseGraphQL<ISchema>("/sampleapp2");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
