using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace GraphQLAuth.Api
{
    public static class AddJwtExtension
    {
        public static IServiceCollection AddJwtAuth(this IServiceCollection services)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer("api", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuer = true,
                        ValidIssuer = "Dominik",
                        ValidateAudience = true,
                        ValidAudience = "api",
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("zXyi0iWkUcJ2XlCdZ5NscHJBwioXQl")),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromDays(300)
                    };
                })
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

            return services;
        }
    }
}
