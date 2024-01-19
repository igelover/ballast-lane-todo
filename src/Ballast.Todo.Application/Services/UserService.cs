using Ballast.Todo.Application.Contracts.Persistence;
using Ballast.Todo.Application.Contracts.Services;
using Ballast.Todo.Domain.Entities;
using Ballast.Todo.Domain.Exceptions;
using Ballast.Todo.Domain.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;
using ValidationException = Ballast.Todo.Domain.Exceptions.ValidationException;

namespace Ballast.Todo.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IValidator<User> _validator;
        private readonly IUserRepository _repository;
        private readonly ILogger<UserService> _logger;

        public UserService(IValidator<User> validator, IUserRepository repository, ILogger<UserService> logger)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<User> RegisterUserAsync(RegistrationRequest request)
        {
            var user = new User
            {
                Email = request.Email,
                Password = request.Password
            };

            var result = await _validator.ValidateAsync(user);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var duplicate = await _repository.GetByEmailAsync(user.Email);
            if (duplicate != null) {
                throw new ConflictException("User email already registered!");
            }

            await _repository.RegisterUserAsync(user);
            _logger.LogDebug("Registered new user {user}", user.Email);
            return user;
        }
    }
}
