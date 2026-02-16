using Ardalis.Result.AspNetCore;
using Mediator;
using SecureTodo.Application.Services;
using SecureTodo.Application.UseCases.Tasks.List;
using SecureTodo.Domain.Task.Enums;

namespace SecureTodo.Api.Endpoints.Tasks;

public sealed record ListTasksRequest(
    string? SearchTerm,
    TaskPriority? Priority,
    int PageSize = 10,
    string Cursor = "");

public class List : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(RouteNames.Tasks.GetAll, HandleAsync)
            .WithTags(RouteNames.Tasks.Tag);
    }
    
    private async Task<IResult> HandleAsync([AsParameters]ListTasksRequest request, IMediator mediator, IAuthService authService, CancellationToken cancellationToken)
    {
        var userId = authService.GetLoggedUserId();
        
        if (!userId.HasValue)
            return TypedResults.Unauthorized();

        var result = await mediator.Send(new ListTasksQuery(
            SearchTerm: request.SearchTerm,
            Priority: request.Priority,
            PageSize: request.PageSize,
            Cursor: request.Cursor,
            UserId: userId.Value
        ), cancellationToken);
        
        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }
        
        return result.ToMinimalApiResult();
    }
}