using Ardalis.Result;
using Mediator;
using Microsoft.Extensions.Logging;
using SecureTodo.Application.Repositories;
using SecureTodo.Domain.Task.ValueObjects;

namespace SecureTodo.Application.UseCases.Tasks.Update;

public sealed class UpdateTaskCommandHandler : ICommandHandler<UpdateTaskCommand, Result>
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly ILogger<UpdateTaskCommandHandler> _logger;
    
    public UpdateTaskCommandHandler(ITaskItemRepository taskItemRepository, ILogger<UpdateTaskCommandHandler> logger)
    {
        _taskItemRepository = taskItemRepository;
        _logger = logger;
    }
    
    public async ValueTask<Result> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to update task with id : {Id}", command.Id);
        var taskId = TaskItemId.FromGuid(command.Id);
        var found = await _taskItemRepository.GetByIdAsync(taskId, cancellationToken);

        if (found is null)
        {
            _logger.LogWarning("Task with Id: {Id} not found.", command.Id);
            return Result.NotFound();
        }
        
        found.UpdateDescription(command.Description);
        found.UpdateTitle(command.Title);
        found.UpdatePriority(command.Priority);
        found.UpdateDueDate(command.DueDate);
        
        await _taskItemRepository.UpdateAsync(found, cancellationToken);
        
        _logger.LogInformation("Task with Id: {Id} updated successfully.", command.Id);
        return Result.Success();
    }
}