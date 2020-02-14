using GraphQL;
using GraphQL.Validation;

namespace GraphQLAuth.Api.Validations
{
    // TODO: Handle Authentication here?
    public class RequiresAuthenticationValidationRule : IValidationRule
    {
        public INodeVisitor Validate(ValidationContext context)
        {
            var userContext = context.UserContext as GraphQLUserContext;
            var authenticated = userContext.User.Identity.IsAuthenticated;

            return new EnterLeaveListener(_ =>
            {
                if (authenticated == false)
                    throw new ExecutionError("Unauthorized");
                    //context.ReportError(new ValidationError("", "401", "Unauthorized"));
            });
        }
    }
}
