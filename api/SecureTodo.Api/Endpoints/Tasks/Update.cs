using Ardalis.Result.AspNetCore;
using Mediator;
using SecureTodo.Application.Tasks.Create;
using SecureTodo.Application.Tasks.Update;
using SecureTodo.Domain.Task.Enums;

namespace SecureTodo.Api.Endpoints.Tasks;

public record UpdateTaskRequest(string Title, string? Description, TaskPriority Priority, DateOnly DueDate);

public sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapPut(RouteNames.Tasks.Update, Handler)
            .WithTags(RouteNames.Tasks.Tag);
    }
    
    private async Task<IResult> Handler(Guid id, UpdateTaskRequest request, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateTaskCommand(
            Id: id,
            Title: request.Title,
            Description: request.Description,
            Priority: request.Priority,
            DueDate: request.DueDate
        ), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.NoContent();
        }

        return result.ToMinimalApiResult();
    }
}