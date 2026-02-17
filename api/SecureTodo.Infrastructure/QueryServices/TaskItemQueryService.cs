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

        if (!string.IsNullOrWhiteSpace(filter.Cursor))
        {
            query = query.Where(item => item.Id.Value < Guid.Parse(filter.Cursor));
        }
        
        query = query
            .Where(item => item.IsCompleted == filter.IsCompleted)
            .Where(item => EF.Property<Guid>(item, "CreatedBy") == filter.UserId)
            .OrderBy(item => item.Priority)
            .Take(filter.PageSize + 1);
        
        var result = await query.Select(item => 
            new TaskItemDto(
                item.Id.Value, 
                item.Title, 
                item.Description, 
                item.Priority, 
                item.DueDate,
                item.IsCompleted)
        ).ToListAsync(cancellationToken);

        var cursor = "";
        if (result.Count > filter.PageSize)
        {
            result.RemoveAt(result.Count - 1);
            cursor = result.Last().Id.ToString();
        }

        return new PagedList<TaskItemDto>(result, cursor);
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
                    item.DueDate,
                    item.IsCompleted)
            )
            .FirstOrDefaultAsync(cancellationToken);
        
        return result;
    }
}