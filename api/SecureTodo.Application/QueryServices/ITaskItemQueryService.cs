using SecureTodo.Domain.Task.Enums;
using SecureTodo.Domain.Task.ValueObjects;

namespace SecureTodo.Application.QueryServices;

public record PagedList<T>(
    IReadOnlyCollection<T> Items, 
    string Cursor) where T : notnull;

public record TaskItemDto(Guid Id, string Title, string? Description, TaskPriority Priority, DateOnly DueDate);

public record PagingFilter(
    string? Filter, 
    TaskPriority? Priority, 
    string Cursor, 
    int PageSize,
    Guid UserId);

public interface ITaskItemQueryService
{
    ValueTask<PagedList<TaskItemDto>> GetPaged(PagingFilter filter, CancellationToken cancellationToken = default);
    ValueTask<TaskItemDto?> GetById(TaskItemId taskId, CancellationToken cancellationToken = default);
}