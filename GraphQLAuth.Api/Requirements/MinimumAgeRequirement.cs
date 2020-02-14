using GraphQL.Authorization;
using System.Threading.Tasks;

namespace GraphQLAuth.Api.Requirements
{
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get; }

        public MinimumAgeRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }

        public Task Authorize(AuthorizationContext context)
        {
            var minimumAge = int.Parse(context.User.FindFirst(c => c.Type == "age")?.Value ?? "0");
            if (minimumAge < 21)
                context.ReportError("MinimumAge");

            return Task.CompletedTask;
        }
    }
}
