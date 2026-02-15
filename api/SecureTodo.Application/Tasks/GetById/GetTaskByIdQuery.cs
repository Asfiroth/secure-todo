using Ardalis.Result;
using Mediator;
using SecureTodo.Application.QueryServices;

namespace SecureTodo.Application.Tasks.GetById;

public record GetTaskByIdQuery(Guid Id) : IQuery<Result<TaskItemDto>>;