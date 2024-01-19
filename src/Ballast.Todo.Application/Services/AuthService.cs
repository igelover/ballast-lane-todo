using Ballast.Todo.Application.Contracts.Identity;
using Ballast.Todo.Application.Contracts.Persistence;
using Ballast.Todo.Application.Contracts.Services;
using Ballast.Todo.Domain;
using Ballast.Todo.Domain.Entities;
using Ballast.Todo.Domain.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;
using ValidationException = Ballast.Todo.Domain.Exceptions.ValidationException;

namespace Ballast.Todo.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IValidator<LoginRequest> _validator;
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IValidator<LoginRequest> validator, IUserRepository userRepository, ISessionRepository sessionRepository, IJwtService jwtService, ILogger<AuthService> logger)
        {
            _validator = validator;
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var result = await _validator.ValidateAsync(request);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var userId = await _userRepository.ValidateCredentialsAsync(request.Email, request.Password);

            var (tokenId, jwtToken) = _jwtService.GenerateJwtFor(userId, Constants.JwtScopeUserRole);
            _logger.LogDebug("Created tokenId {token} for user {user}", tokenId, request.Email);

            await _sessionRepository.DeleteSessionsAsync(userId);
            await _sessionRepository.CreateSessionAsync(new Session
            {
                UserId = userId,
                CreateDate = DateTime.UtcNow,
                TokenId = tokenId,
            });

            return jwtToken;
        }
    }
}
