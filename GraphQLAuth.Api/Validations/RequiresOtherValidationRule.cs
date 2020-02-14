using GraphQL.Language.AST;
using GraphQL.Validation;

namespace GraphQLAuth.Api.Validations
{
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
}
