using Ballast.Todo.Domain.Entities;

namespace Ballast.Todo.Application.Contracts.Persistence
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task RegisterUserAsync(User user);
        Task<string> ValidateCredentialsAsync(string email, string password);
    }
}
