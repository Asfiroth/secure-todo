using Ardalis.Result;
using Mediator;

namespace SecureTodo.Application.Tasks.Delete;

public record DeleteTaskCommand(Guid Id) : ICommand<Result>;
