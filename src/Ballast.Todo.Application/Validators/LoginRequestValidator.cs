using Ballast.Todo.Domain.Models;
using FluentValidation;

namespace Ballast.Todo.Application.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
