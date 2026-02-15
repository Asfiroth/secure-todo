using Mediator;
using SecureTodo.Application.QueryServices;

namespace SecureTodo.Application.Tasks.List;

public class ListTasksQueryHandler : IQueryHandler<ListTasksQuery, List<TaskItemDto>>
{
    public ValueTask<List<TaskItemDto>> Handle(ListTasksQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}