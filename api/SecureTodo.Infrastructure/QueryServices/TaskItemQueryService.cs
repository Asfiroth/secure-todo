using Microsoft.EntityFrameworkCore;
using SecureTodo.Application.QueryServices;
using SecureTodo.Domain.Task.ValueObjects;
using SecureTodo.Infrastructure.Data;

namespace SecureTodo.Infrastructure.QueryServices;

internal sealed class TaskItemQueryService : ITaskItemQueryService
{
    private readonly SecureTodoDbContext _dbContext;
    
    public TaskItemQueryService(SecureTodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<PagedList<TaskItemDto>> GetPaged(PagingFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Tasks.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Filter))
        {
            query = query.Where(item => item.Title.Contains(filter.Filter) || (item.Description != null && item.Description.Contains(filter.Filter)));
        }

        if (filter.Priority is not null)
        {
            query = query.Where(item => item.Priority == filter.Priority);
        }

        if (string.IsNullOrWhiteSpace(filter.Cursor))
        {
            query = query.Where(item => item.Id.Value < Guid.Parse(filter.Cursor));
        }
        
        query = query
            .OrderByDescending(item => item.DueDate)
            .Take(filter.PageSize);
        
        var result = await query.Select(item => 
            new TaskItemDto(
                item.Id.Value, 
                item.Title, 
                item.Description, 
                item.Priority, 
                item.DueDate)
        ).ToListAsync(cancellationToken);

        return new PagedList<TaskItemDto>(result, result.Last().Id.ToString());
    }

    public async ValueTask<TaskItemDto?> GetById(TaskItemId taskId, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Tasks
            .AsNoTracking()
            .Where(item => item.Id == taskId)
            .Select(item => 
                new TaskItemDto(
                    item.Id.Value, 
                    item.Title, 
                    item.Description, 
                    item.Priority, 
                    item.DueDate)
            )
            .FirstOrDefaultAsync(cancellationToken);
        
        return result;
    }
}