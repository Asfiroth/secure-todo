using Ardalis.Result;
using Mediator;
using SecureTodo.Domain.Task.Enums;

namespace SecureTodo.Application.UseCases.Tasks.Create;

public sealed record CreateTaskCommand(
    string Title,
    string? Description,
    TaskPriority Priority,
    DateOnly DueDate
    ) : ICommand<Result<CreateTaskResult>>;
    
public sealed record CreateTaskResult(
    Guid Id,
    string Title,
    string? Description,
    TaskPriority Priority,
    DateOnly DueDate);