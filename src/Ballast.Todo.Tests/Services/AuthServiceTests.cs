using Ballast.Todo.Application.Services;
using Ballast.Todo.Application.Validators;
using Ballast.Todo.Domain.Exceptions;
using Ballast.Todo.Domain.Models;
using Ballast.Todo.Tests.Mocks.Repositories;
using Ballast.Todo.Tests.Mocks.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Ballast.Todo.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly ILogger<AuthService> _logger;

        public AuthServiceTests()
        {
            _logger = new LoggerFactory().CreateLogger<AuthService>();
        }

        [Fact]
        public async Task AuthService_LoginAsync_ValidRequest_Success()
        {
            var userId = Guid.NewGuid().ToString();
            (string tokenId, string token) = (Guid.NewGuid().ToString(), "");
            var request = new LoginRequest
            {
                Email = "mail@example.com",
                Password = "password"
            };

            var mockUserRepo = new MockUserRepository().MockValidateCredentialsAsync(userId);
            var mockJwtService = new MockJwtService().MockGenerateJwtFor((tokenId, token));
            var mockSessionRepo = new MockSessionRepository().MockDeleteSessionsAsync(true).MockCreateSessionAsync(true);
            var authService = new AuthService(new LoginRequestValidator(), mockUserRepo.Object, mockSessionRepo.Object, mockJwtService.Object, _logger);

            var jwtToken = await authService.LoginAsync(request);

            Assert.NotNull(jwtToken);
            mockUserRepo.VerifyValidateCredentialsAsync(Times.Once());
            mockJwtService.VerifyGenerateJwtFor(Times.Once());
            mockSessionRepo.VerifyDeleteSessionsAsync(Times.Once());
            mockSessionRepo.VerifyCreateSessionAsync(Times.Once());
        }

        [Fact]
        public async Task AuthService_LoginAsync_InvalidRequest_Failure()
        {
            var request = new LoginRequest();

            var mockUserRepo = new MockUserRepository();
            var mockJwtService = new MockJwtService();
            var mockSessionRepo = new MockSessionRepository();
            var authService = new AuthService(new LoginRequestValidator(), mockUserRepo.Object, mockSessionRepo.Object, mockJwtService.Object, _logger);

            var ex = await Assert.ThrowsAsync<ValidationException>(() => authService.LoginAsync(request));

            Assert.NotEmpty(ex.Errors);
            mockUserRepo.VerifyValidateCredentialsAsync(Times.Never());
            mockJwtService.VerifyGenerateJwtFor(Times.Never());
            mockSessionRepo.VerifyDeleteSessionsAsync(Times.Never());
            mockSessionRepo.VerifyCreateSessionAsync(Times.Never());
        }

        [Fact]
        public async Task AuthService_LoginAsync_InvalidCredentials_Failure()
        {
            var userId = Guid.NewGuid().ToString();
            (string tokenId, string token) = (Guid.NewGuid().ToString(), "");
            var request = new LoginRequest
            {
                Email = "mail@example.com",
                Password = "password"
            };

            var mockUserRepo = new MockUserRepository().MockValidateCredentialsAsyncInvalid();
            var mockJwtService = new MockJwtService();
            var mockSessionRepo = new MockSessionRepository();
            var authService = new AuthService(new LoginRequestValidator(), mockUserRepo.Object, mockSessionRepo.Object, mockJwtService.Object, _logger);

            var ex = await Assert.ThrowsAsync<UnauthorizedException>(() => authService.LoginAsync(request));

            mockUserRepo.VerifyValidateCredentialsAsync(Times.Once());
            mockJwtService.VerifyGenerateJwtFor(Times.Never());
            mockSessionRepo.VerifyDeleteSessionsAsync(Times.Never());
            mockSessionRepo.VerifyCreateSessionAsync(Times.Never());
        }
    }
}
