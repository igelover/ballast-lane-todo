using Ballast.Todo.Application.Contracts.Persistence;
using Ballast.Todo.Domain.Entities;
using Ballast.Todo.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Ballast.Todo.Infrastructure.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IDataContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var users = await _context.Users.FindAsync(x => x.Email.Equals(email));
            return await users.FirstOrDefaultAsync();
        }

        public async Task RegisterUserAsync(User user)
        {
            await _context.Users.InsertOneAsync(user);
            _logger.LogDebug("Created user {user}", user.Email);
        }

        public async Task<string> ValidateCredentialsAsync(string email, string password)
        {
            _logger.LogDebug("Validating credentials for user {user}", email);
            var users = await _context.Users.FindAsync(x => x.Email.Equals(email) && x.Password.Equals(password));
            var user = await users.FirstOrDefaultAsync();
            return user is null ? throw new UnauthorizedException() : user.Id;
        }
    }
}
