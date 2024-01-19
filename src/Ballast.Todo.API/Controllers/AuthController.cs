using Ballast.Todo.Application.Contracts.Services;
using Ballast.Todo.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ballast.Todo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("ApiUser")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Default constructor receiving an <see cref="IAuthService"/>
        /// </summary>
        /// <param name="authService">The <see cref="IAuthService"/> to use</param>
        /// <param name="logger">The logger</param>
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint to login
        /// </summary>
        /// <param name="request">The user email and password</param>
        /// <returns>The access token</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> LoginAsync([FromBody] LoginRequest request)
        {
            _logger.LogDebug("Received request to login user {user}", request.Email);
            var token = await _authService.LoginAsync(request);
            return Ok(token);
        }
    }
}
