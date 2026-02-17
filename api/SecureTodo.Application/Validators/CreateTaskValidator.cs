using FluentValidation;
using SecureTodo.Application.UseCases.Tasks.Create;

namespace SecureTodo.Application.Validators;

public class CreateTaskValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(120);
        
        RuleFor(x => x.Description)
            .MaximumLength(200);
        
        RuleFor(x => x.DueDate)
            .Must(x => x.ToDateTime(TimeOnly.MinValue)> DateTime.Now.Date)
            .WithMessage("Due date cannot be in the past.");
        
        RuleFor(x => x.Priority)
            .IsInEnum();
    }
}