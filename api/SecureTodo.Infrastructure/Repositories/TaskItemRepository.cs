using Microsoft.EntityFrameworkCore;
using SecureTodo.Application.Repositories;
using SecureTodo.Domain.Task;
using SecureTodo.Domain.Task.ValueObjects;
using SecureTodo.Infrastructure.Data;

namespace SecureTodo.Infrastructure.Repositories;

internal sealed class TaskItemRepository : ITaskItemRepository
{
    private readonly SecureTodoDbContext _dbContext;
    
    public TaskItemRepository(SecureTodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async ValueTask<TaskItem?> GetByIdAsync(TaskItemId taskId, CancellationToken cancellationToken = default)
    {
        var taskItem = await _dbContext.Tasks.FirstOrDefaultAsync(item => item.Id == taskId, cancellationToken);
        return taskItem;
    }

    public async ValueTask AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        await _dbContext.Tasks.AddAsync(taskItem, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        _dbContext.Tasks.Update(taskItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        _dbContext.Tasks.Remove(taskItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}