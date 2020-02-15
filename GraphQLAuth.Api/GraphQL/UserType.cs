using GraphQL.Types;

namespace GraphQLAuth.Api.GraphQL
{
    public class UserType : ObjectGraphType<User>
    {
        public UserType()
        {
            Field(user => user.Id);
            Field(user => user.Name);
        }
    }
}
