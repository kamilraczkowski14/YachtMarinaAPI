using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using YachtMarinaAPI.Models;

namespace YachtMarinaAPI.Authorization
{
    public class BasketAuthorization : AuthorizationHandler<ResourceOperationRequirement, Basket>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Basket resource)
        {
            var LoggedUserId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (resource.UserId == int.Parse(LoggedUserId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}
