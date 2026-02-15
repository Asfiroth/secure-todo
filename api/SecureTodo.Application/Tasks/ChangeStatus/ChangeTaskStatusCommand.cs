using Ardalis.Result;
using Mediator;

namespace SecureTodo.Application.Tasks.ChangeStatus;

public sealed record ChangeTaskStatusCommand(Guid Id, bool IsCompleted) : ICommand<Result>; 
