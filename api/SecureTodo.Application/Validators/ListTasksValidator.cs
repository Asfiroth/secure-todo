using FluentValidation;
using SecureTodo.Application.UseCases.Tasks.List;

namespace SecureTodo.Application.Validators;

public class ListTasksValidator : AbstractValidator<ListTasksQuery>
{
    public ListTasksValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(0);
    }
}