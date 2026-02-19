using FluentValidation;
using SecureTodo.Application.UseCases.Tasks.Delete;

namespace SecureTodo.Application.Validators;

public class DeleteTaskValidator : AbstractValidator<DeleteTaskCommand>
{
    public DeleteTaskValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}