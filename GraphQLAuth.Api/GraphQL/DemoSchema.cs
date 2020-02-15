using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLAuth.Api.GraphQL
{
    public class DemoSchema : Schema
    {
        public DemoSchema(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            Query = dependencyResolver.Resolve<DemoQuery>();
        }
    }
}
