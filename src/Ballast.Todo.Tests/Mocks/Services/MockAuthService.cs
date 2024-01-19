using Ballast.Todo.Application.Contracts.Services;
using Ballast.Todo.Domain.Models;
using Moq;

namespace Ballast.Todo.Tests.Mocks.Services
{
    public class MockAuthService : Mock<IAuthService>
    {
        public MockAuthService MockLoginAsync(string token)
        {
            Setup(x => x.LoginAsync(It.IsAny<LoginRequest>())).ReturnsAsync(token);
            return this;
        }
        public MockAuthService VerifyLoginAsync(Times times)
        {
            Verify(x => x.LoginAsync(It.IsAny<LoginRequest>()), times);
            return this;
        }
    }
}
