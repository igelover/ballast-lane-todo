using Ballast.Todo.Domain.Models;

namespace Ballast.Todo.Application.Contracts.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginRequest request);
    }
}
