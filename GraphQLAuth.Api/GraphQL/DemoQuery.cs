using GraphQL.Authorization;
using GraphQL.Types;
using System;
using System.Collections.Generic;

namespace GraphQLAuth.Api.GraphQL
{
    public class DemoQuery : ObjectGraphType
    {
        public DemoQuery()
        {
            var users = Field<ListGraphType<UserType>>(
                name: "Users",
                resolve: context =>
                {
                    return new List<User> {
                        new User { Id = Guid.NewGuid().ToString(), Name = "Harlem" },
                        new User { Id = Guid.NewGuid().ToString(), Name = "Grande" }
                    };
                }
            );

            users.AuthorizeWith("Reg1");
            users.AuthorizeWith("MinimumAge");

            Field<ListGraphType<UserType>>(
                name: "Viewers",
                resolve: context =>
                {
                    return new List<User> {
                        new User { Id = Guid.NewGuid().ToString(), Name = "Bronx" },
                        new User { Id = Guid.NewGuid().ToString(), Name = "Gatsby" }
                    };
                }
            ).AuthorizeWith("Reg2");
        }
    }
}