using Ardalis.Result;
using Mediator;
using Microsoft.Extensions.Logging;
using SecureTodo.Application.QueryServices;

namespace SecureTodo.Application.UseCases.Tasks.List;

public class ListTasksQueryHandler : IQueryHandler<ListTasksQuery, Result<PagedList<TaskItemDto>>>
{
    private readonly ILogger<ListTasksQueryHandler> _logger;
    private readonly ITaskItemQueryService _taskItemQueryService;
    
    public ListTasksQueryHandler(ILogger<ListTasksQueryHandler> logger, ITaskItemQueryService taskItemQueryService)
    {
        _logger = logger;
        _taskItemQueryService = taskItemQueryService;
    }
    
    public async ValueTask<Result<PagedList<TaskItemDto>>> Handle(ListTasksQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving tasks for current user");
        var filter = new PagingFilter(
            Filter: query.SearchTerm,
            Priority: query.Priority,
            Cursor: query.Cursor,
            PageSize: query.PageSize,
            UserId: query.UserId);
        
        var found = await _taskItemQueryService.GetPaged(filter, cancellationToken);

        return Result<PagedList<TaskItemDto>>.Success(found);
    }
}