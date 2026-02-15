using Ardalis.Result;
using Mediator;
using Microsoft.Extensions.Logging;
using SecureTodo.Application.Repositories;
using SecureTodo.Domain.Task.ValueObjects;

namespace SecureTodo.Application.Tasks.Delete;

public sealed class DeleteTaskCommandHandler : ICommandHandler<DeleteTaskCommand, Result>
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly ILogger<DeleteTaskCommandHandler> _logger;
    
    public DeleteTaskCommandHandler(ITaskItemRepository taskItemRepository, ILogger<DeleteTaskCommandHandler> logger)
    {
        _taskItemRepository = taskItemRepository;
        _logger = logger;
    }
    
    public async ValueTask<Result> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to delete task with {Id}", command.Id);

        var taskId = TaskItemId.FromGuid(command.Id);
        var item = await _taskItemRepository.GetByIdAsync(taskId, cancellationToken);
        
        if (item is null)
        {
            _logger.LogWarning("Task with Id: {Id} not found.", command.Id);
            return Result.NotFound();
        }
        
        await _taskItemRepository.DeleteAsync(item, cancellationToken);
        
        _logger.LogInformation("Task with Id: {Id} deleted successfully.", command.Id);
        return Result.Success();
    }
}