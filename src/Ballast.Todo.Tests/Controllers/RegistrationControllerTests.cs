using Ballast.Todo.API.Controllers;
using Ballast.Todo.Domain.Entities;
using Ballast.Todo.Domain.Models;
using Ballast.Todo.Tests.Mocks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Ballast.Todo.Tests.Controllers
{
    public class RegistrationControllerTests
    {
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationControllerTests()
        {
            _logger = new LoggerFactory().CreateLogger<RegistrationController>();
        }

        [Fact]
        public async Task RegistrationController_RegisterUserAsync_ValidRequest_Success()
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

            var mockUserService = new MockUserService().MockRegisterUserAsync(user);
            var registrationController = new RegistrationController(mockUserService.Object, _logger);

            var actionResult = await registrationController.RegisterUserAsync(request);

            var result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result);
            Assert.NotNull(result.Value);

            mockUserService.VerifyRegisterUserAsync(Times.Once());
        }
    }
}