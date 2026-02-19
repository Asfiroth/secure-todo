namespace SecureTodo.Api.Endpoints.Tasks.Payloads;

public sealed record ListTasksRequest(
    string? SearchTerm,
    bool IsCompleted = false,
    int PageSize = 10,
    string Cursor = "");