using GraphQL;
using GraphQL.Server;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQLAuth.Api.GraphQL;
using GraphQLAuth.Api.Middleware;
using GraphQLAuth.Api.Requirements;
using GraphQLAuth.Api.Validations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GraphQLAuth.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));

            //services.AddSchemaFisrt();
            services.AddScoped<ISchema, DemoSchema>();

            // Extension used in sample from GraphQL Authorization NuGet
            services.AddGraphQLAuth((_, s) =>
            {
                _.AddPolicy("MinimumAge", p => p.AddRequirement(new MinimumAgeRequirement(200)));
                _.AddPolicy("Reg1", p => p.RequireClaim("reg", "1"));
                _.AddPolicy("Reg2", p => p.RequireClaim("reg", "2"));
            });

            // GraphQL
            services
                .AddGraphQL(options =>
                {
                    options.ExposeExceptions = false;
                })
                .AddGraphTypes(ServiceLifetime.Scoped)
                .AddUserContextBuilder(context => new GraphQLUserContext { User = context.User });

            // Custom extension
            services.AddJwtAuth();

            // Add another IValidationRule, there will be always one because AddGraphQLAuth
            services.AddTransient<IValidationRule, RequiresAuthenticationValidationRule>();
            services.AddTransient<IValidationRule, RequiresOtherValidationRule>();

            // Just for TokenController
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
            app.UseAuthorization();

            // Just for testing purposes
            var validationRules = app.ApplicationServices.GetServices<IValidationRule>();

            app.UseGraphQL<ISchema>("/sampleapp1");
            app.UseGraphQL<ISchema>("/sampleapp2");

            // Just for TokenController
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
