using Ballast.Todo.Application.Contracts.Persistence;
using Ballast.Todo.Domain.Entities;
using Ballast.Todo.Domain.Exceptions;
using Moq;

namespace Ballast.Todo.Tests.Mocks.Repositories
{
    public class MockUserRepository : Mock<IUserRepository>
    {
        public MockUserRepository MockGetByEmailAsync(User user)
        {
            Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            return this;
        }
        public MockUserRepository VerifyGetByEmailAsync(Times times)
        {
            Verify(x => x.GetByEmailAsync(It.IsAny<string>()), times);
            return this;
        }

        public MockUserRepository MockRegisterUserAsync()
        {
            Setup(x => x.RegisterUserAsync(It.IsAny<User>()));
            return this;
        }
        public MockUserRepository VerifyRegisterUserAsync(Times times)
        {
            Verify(x => x.RegisterUserAsync(It.IsAny<User>()), times);
            return this;
        }

        public MockUserRepository MockValidateCredentialsAsync(string userId)
        {
            Setup(x => x.ValidateCredentialsAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(userId);
            return this;
        }
        public MockUserRepository MockValidateCredentialsAsyncInvalid()
        {
            Setup(x => x.ValidateCredentialsAsync(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new UnauthorizedException());
            return this;
        }
        public MockUserRepository VerifyValidateCredentialsAsync(Times times)
        {
            Verify(x => x.ValidateCredentialsAsync(It.IsAny<string>(), It.IsAny<string>()), times);
            return this;
        }
    }
}
