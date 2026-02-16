using Ardalis.Result;
using Mediator;
using Microsoft.Extensions.Logging;
using SecureTodo.Application.QueryServices;
using SecureTodo.Domain.Task.ValueObjects;

namespace SecureTodo.Application.UseCases.Tasks.GetById;

public sealed class GetTaskByIdQueryHandler : IQueryHandler<GetTaskByIdQuery, Result<TaskItemDto>>
{
    private readonly ITaskItemQueryService _taskItemQueryService;
    private readonly ILogger<GetTaskByIdQueryHandler> _logger;
    
    public GetTaskByIdQueryHandler(ITaskItemQueryService taskItemQueryService, ILogger<GetTaskByIdQueryHandler> logger)
    {
        _taskItemQueryService = taskItemQueryService;
        _logger = logger;
    }
    
    public async ValueTask<Result<TaskItemDto>> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to retrieve Task with Id: {Id}", query.Id);

        var taskId = TaskItemId.FromGuid(query.Id);
        var task = await _taskItemQueryService.GetById(taskId, cancellationToken);

        if (task is null)
        {
            _logger.LogWarning("Task with Id: {Id} not found.", query.Id);
            return Result.NotFound();
        }
        _logger.LogInformation("Task with Id: {Id} retrieved successfully.", query.Id);
        return Result<TaskItemDto>.Success(task);
    }
}