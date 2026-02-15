using SecureTodo.Domain.Task;
using SecureTodo.Domain.Task.ValueObjects;

namespace SecureTodo.Application.Repositories;

public interface ITaskItemRepository
{
    ValueTask<TaskItem?> GetByIdAsync(TaskItemId taskId, CancellationToken cancellationToken = default);
    ValueTask AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    ValueTask UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    ValueTask DeleteAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    
}