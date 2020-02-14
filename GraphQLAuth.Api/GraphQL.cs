using GraphQL.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace GraphQLAuth.Api
{
    public class GraphQLUserContext : IProvideClaimsPrincipal
    {
        public ClaimsPrincipal User { get; set; }
    }

    public class Query
    {
        [GraphQLAuthorize(Policy = "Reg1")]
        [GraphQLAuthorize(Policy = "MinimumAge")]
        public User Viewer()
        {
            return new User { Id = Guid.NewGuid().ToString(), Name = "Quinn" };
        }

        [GraphQLAuthorize(Policy = "Reg2")]
        public List<User> Users()
        {
            return new List<User> { new User { Id = Guid.NewGuid().ToString(), Name = "Quinn" } };
        }
    }

    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
