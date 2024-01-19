using Ballast.Todo.Application.Contracts.Services;
using Ballast.Todo.Domain.Entities;
using Ballast.Todo.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ballast.Todo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("ApiUser")]
    public class RegistrationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<RegistrationController> _logger;

        /// <summary>
        /// Default constructor receiving an <see cref="IUserService"/>
        /// </summary>
        /// <param name="userService">The <see cref="IUserService"/> to use</param>
        /// <param name="logger">The logger</param>
        public RegistrationController(IUserService userService, ILogger<RegistrationController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint to register a new user
        /// </summary>
        /// <param name="request">The user to be created</param>
        /// <returns>The newly created <see cref="User"/></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> RegisterUserAsync([FromBody] RegistrationRequest request)
        {
            _logger.LogDebug("Received request to register user {user}", request.Email);
            var result = await _userService.RegisterUserAsync(request);
            return Ok(result);
        }
    }
}
