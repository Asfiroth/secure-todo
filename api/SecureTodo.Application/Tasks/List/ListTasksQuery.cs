using Mediator;
using SecureTodo.Application.QueryServices;

namespace SecureTodo.Application.Tasks.List;

public sealed record ListTasksQuery(
    
    ): IQuery<List<TaskItemDto>>;
