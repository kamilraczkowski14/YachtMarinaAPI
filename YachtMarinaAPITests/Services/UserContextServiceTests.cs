using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using YachtMarinaAPI.Services;
using Assert = Xunit.Assert;

namespace YachtMarinaAPI.Tests.Services
{
    public class UserContextServiceTests
    {
        [Fact]
        public void LoggedUserId_UserIsNotNull_ReturnsUserId()
        {
            var userId = 1;
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(principal);

            var userContextService = new UserContextService(httpContextAccessorMock.Object);

            var loggedUserId = userContextService.LoggedUserId;
            Assert.Equal(userId, loggedUserId);
        }

        [Fact]
        public void LoggedUserId_UserIsNull_ReturnsNull()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns((ClaimsPrincipal)null);

            var userContextService = new UserContextService(httpContextAccessorMock.Object);

            var loggedUserId = userContextService.LoggedUserId;

            Assert.Null(loggedUserId);
        }
    }
}