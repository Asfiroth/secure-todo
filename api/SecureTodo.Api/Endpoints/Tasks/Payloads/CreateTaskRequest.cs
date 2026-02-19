using SecureTodo.Domain.Task.Enums;

namespace SecureTodo.Api.Endpoints.Tasks.Payloads;

public sealed record CreateTaskRequest(string Title, string? Description, TaskPriority Priority, DateOnly DueDate);