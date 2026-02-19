using FluentValidation;
using SecureTodo.Application.UseCases.Tasks.GetById;

namespace SecureTodo.Application.Validators;

public class GetTaskByIdValidator : AbstractValidator<GetTaskByIdQuery>
{
    public GetTaskByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}