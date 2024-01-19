using Ballast.Todo.Domain.Entities;
using Ballast.Todo.Domain.Models;

namespace Ballast.Todo.Application.Contracts.Services
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(RegistrationRequest request);
    }
}
