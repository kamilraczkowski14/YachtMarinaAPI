using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using YachtMarinaAPI.Models;

namespace YachtMarinaAPI.Authorization
{
    public class UserAuthorization : AuthorizationHandler<ResourceOperationRequirement, User>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ResourceOperationRequirement requirement, User resource)
        {
            var LoggedUserId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (resource.Id == int.Parse(LoggedUserId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
