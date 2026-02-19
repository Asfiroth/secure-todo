using FluentValidation;
using SecureTodo.Application.UseCases.Tasks.ChangeStatus;

namespace SecureTodo.Application.Validators;

public class ChangeTaskStatusValidator : AbstractValidator<ChangeTaskStatusCommand>
{
    public ChangeTaskStatusValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}