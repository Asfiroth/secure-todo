using Ardalis.Result;
using Mediator;
using SecureTodo.Domain.Task.Enums;

namespace SecureTodo.Application.UseCases.Tasks.Update;

public record UpdateTaskCommand(
    Guid Id,
    string Title,
    string? Description,
    TaskPriority Priority,
    DateOnly DueDate
    ) : ICommand<Result>;
    