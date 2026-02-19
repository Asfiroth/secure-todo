using Ardalis.Result;
using Mediator;

namespace SecureTodo.Application.UseCases.Tasks.Delete;

public sealed record DeleteTaskCommand(Guid Id) : ICommand<Result>;
