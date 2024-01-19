using Ballast.Todo.Domain.Entities;
using FluentValidation;

namespace Ballast.Todo.Application.Validators
{
    public class TodoItemValidator : AbstractValidator<TodoItem>
    {
        public TodoItemValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
