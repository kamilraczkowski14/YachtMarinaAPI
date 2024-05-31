using Microsoft.AspNetCore.Authorization;

namespace YachtMarinaAPI.Authorization
{
    public enum ResourceOperation
    {
        Create,
        Read,
        Update,
        Delete
    }
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperation _resourceOperation { get; }
        public ResourceOperationRequirement(ResourceOperation resourceOperation)
        {
            _resourceOperation = resourceOperation;
        }
    }
}
