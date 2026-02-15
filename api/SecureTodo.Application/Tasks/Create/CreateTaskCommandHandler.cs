using Ardalis.Result;
using Mediator;
using Microsoft.Extensions.Logging;
using SecureTodo.Application.Repositories;
using SecureTodo.Domain.Task;

namespace SecureTodo.Application.Tasks.Create;

public sealed class CreateTaskCommandHandler : ICommandHandler<CreateTaskCommand, Result<CreateTaskResult>>
{
    private readonly ILogger<CreateTaskCommandHandler> _logger;
    private readonly ITaskItemRepository _taskItemRepository;

    public CreateTaskCommandHandler(ILogger<CreateTaskCommandHandler> logger, ITaskItemRepository taskItemRepository)
    {
        _logger = logger;
        _taskItemRepository = taskItemRepository;
    }
    
    public async ValueTask<Result<CreateTaskResult>> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating Task");
        var newTask = TaskItem.Factory.Create(command.Title, command.Description, command.Priority, command.DueDate);
        await _taskItemRepository.AddAsync(newTask, cancellationToken);
        _logger.LogInformation("Task Created");
        
        var mapped = new CreateTaskResult(
            newTask.Id.Value, 
            newTask.Title, 
            newTask.Description, 
            newTask.Priority, 
            newTask.DueDate
            );
        
        return Result<CreateTaskResult>.Success(mapped);
    }
}