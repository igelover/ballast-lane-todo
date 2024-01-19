using Ballast.Todo.Application.Services;
using Ballast.Todo.Application.Validators;
using Ballast.Todo.Domain.Entities;
using Ballast.Todo.Domain.Exceptions;
using Ballast.Todo.Domain.Models;
using Ballast.Todo.Tests.Mocks.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Ballast.Todo.Tests.Services
{
    public class UserServiceTests
    {
        private readonly ILogger<UserService> _logger;

        public UserServiceTests()
        {
            _logger = new LoggerFactory().CreateLogger<UserService>();
        }

        [Fact]
        public async Task UserService_RegisterUserAsync_ValidRequest_Success()
        {
            var request = new RegistrationRequest
            {
                Email = "mail@example.com",
                Password = "password"
            };
            var mockUserRepo = new MockUserRepository().MockGetByEmailAsync(null);
            var userService = new UserService(new UserValidator(), mockUserRepo.Object, _logger);

            var newUser = await userService.RegisterUserAsync(request);

            Assert.NotNull(newUser);
            mockUserRepo.VerifyGetByEmailAsync(Times.Once());
            mockUserRepo.VerifyRegisterUserAsync(Times.Once());
        }

        [Fact]
        public async Task UserService_RegisterUserAsync_InvalidRequest_Failure()
        {
            var request = new RegistrationRequest();
            var mockUserRepo = new MockUserRepository();
            var userService = new UserService(new UserValidator(), mockUserRepo.Object, _logger);

            var ex = await Assert.ThrowsAsync<ValidationException>(() => userService.RegisterUserAsync(request));

            Assert.NotEmpty(ex.Errors);
            mockUserRepo.VerifyGetByEmailAsync(Times.Never());
            mockUserRepo.VerifyRegisterUserAsync(Times.Never());
        }

        [Fact]
        public async Task UserService_RegisterUserAsync_DuplicateUser_Failure()
        {
            var user = new User
            {
                Email = "mail@example.com"
            };
            var request = new RegistrationRequest
            {
                Email = "mail@example.com",
                Password = "password"
            };
            var mockUserRepo = new MockUserRepository().MockGetByEmailAsync(user);
            var userService = new UserService(new UserValidator(), mockUserRepo.Object, _logger);

            await Assert.ThrowsAsync<ConflictException>(() => userService.RegisterUserAsync(request));

            mockUserRepo.VerifyGetByEmailAsync(Times.Once());
            mockUserRepo.VerifyRegisterUserAsync(Times.Never());
        }
    }
}
