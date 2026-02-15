using Ardalis.Result;
using Mediator;
using Microsoft.Extensions.Logging;
using SecureTodo.Application.Repositories;
using SecureTodo.Domain.Task.ValueObjects;

namespace SecureTodo.Application.Tasks.ChangeStatus;

public sealed class ChangeTaskStatusCommandHandler : ICommandHandler<ChangeTaskStatusCommand, Result>
{
    private readonly ILogger<ChangeTaskStatusCommandHandler> _logger;
    private readonly ITaskItemRepository _taskItemRepository;
    
    public ChangeTaskStatusCommandHandler(ILogger<ChangeTaskStatusCommandHandler> logger, ITaskItemRepository taskItemRepository)
    {
        _logger = logger;
        _taskItemRepository = taskItemRepository;
    }
    
    public async ValueTask<Result> Handle(ChangeTaskStatusCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to change task to state: {Completed}", command.IsCompleted);
        var taskId = TaskItemId.FromGuid(command.Id);
        var found = await _taskItemRepository.GetByIdAsync(taskId, cancellationToken);

        if (found is null)
        {
            _logger.LogWarning("Task with Id: {Id} not found.", command.Id);
            return Result.NotFound();
        }
        
        if (command.IsCompleted) found.MarkAsCompleted();
        else found.MarkAsIncomplete();
        
        await _taskItemRepository.UpdateAsync(found, cancellationToken);
        
        _logger.LogInformation("Task with Id: {Id} updated successfully.", command.Id);
        
        return Result.Success();
    }
}