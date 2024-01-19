using Ballast.Todo.Application.Contracts.Services;
using Ballast.Todo.Domain.Entities;
using Ballast.Todo.Domain.Models;
using Moq;

namespace Ballast.Todo.Tests.Mocks.Services
{
    public class MockUserService : Mock<IUserService>
    {
        public MockUserService MockRegisterUserAsync(User user)
        {
            Setup(x => x.RegisterUserAsync(It.IsAny<RegistrationRequest>())).ReturnsAsync(user);
            return this;
        }
        public MockUserService VerifyRegisterUserAsync(Times times)
        {
            Verify(x => x.RegisterUserAsync(It.IsAny<RegistrationRequest>()), times);
            return this;
        }
    }
}
