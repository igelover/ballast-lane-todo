using Ballast.Todo.Domain.Entities;
using FluentValidation;

namespace Ballast.Todo.Application.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
