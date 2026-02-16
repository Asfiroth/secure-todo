using Ardalis.Result;
using Mediator;
using SecureTodo.Application.QueryServices;
using SecureTodo.Domain.Task.Enums;

namespace SecureTodo.Application.UseCases.Tasks.List;

public sealed record ListTasksQuery(
    string? SearchTerm,
    TaskPriority? Priority,
    int PageSize,
    string Cursor,
    Guid UserId
    ): IQuery<Result<PagedList<TaskItemDto>>>;
