using Ballast.Todo.API.Controllers;
using Ballast.Todo.Domain.Models;
using Ballast.Todo.Tests.Mocks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Ballast.Todo.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly ILogger<AuthController> _logger;

        public AuthControllerTests()
        {
            _logger = new LoggerFactory().CreateLogger<AuthController>();
        }

        [Fact]
        public async Task AuthController_LoginAsync_ValidRequest_Success()
        {
            var token = "";
            var request = new LoginRequest
            {
                Email = "mail@example.com",
                Password = "password"
            };

            var mockAuthService = new MockAuthService().MockLoginAsync(token);
            var authController = new AuthController(mockAuthService.Object, _logger);

            var actionResult = await authController.LoginAsync(request);

            var result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result);
            Assert.NotNull(result.Value);

            mockAuthService.VerifyLoginAsync(Times.Once());
        }
    }
}
