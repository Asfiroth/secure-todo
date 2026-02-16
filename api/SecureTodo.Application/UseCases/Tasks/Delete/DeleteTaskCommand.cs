using Ardalis.Result;
using Mediator;

namespace SecureTodo.Application.UseCases.Tasks.Delete;

public record DeleteTaskCommand(Guid Id) : ICommand<Result>;
