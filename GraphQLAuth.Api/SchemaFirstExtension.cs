using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GraphQLAuth.Api
{
    public static class SchemaFirstExtension
    {
        public static IServiceCollection AddSchemaFisrt(this IServiceCollection services)
        {
            //services.TryAddSingleton(s =>
            //{
            //    var definitions = @"
            //      type User {
            //        id: ID
            //        name: String
            //      }
            //      type Query {
            //        viewer: User
            //        users: [User]
            //      }
            //    ";
            //    var schema = Schema.For(
            //        definitions,
            //        _ =>
            //        {
            //            _.Types.Include<Query>();
            //        });

            //    //schema.FindType("User").AuthorizeWith("AdminPolicy");

            //    return schema;
            //});

            return services;
        }
    }
}
