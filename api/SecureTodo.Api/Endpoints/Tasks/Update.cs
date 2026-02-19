using Ardalis.Result.AspNetCore;
using Mediator;
using SecureTodo.Api.Constants;
using SecureTodo.Api.Endpoints.Tasks.Payloads;
using SecureTodo.Application.UseCases.Tasks.Update;

namespace SecureTodo.Api.Endpoints.Tasks;

public sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapPut(RouteNames.Tasks.Update, HandleAsync)
            .WithTags(RouteNames.Tasks.Tag);
    }
    
    private async Task<IResult> HandleAsync(Guid id, UpdateTaskRequest request, IMediator mediator, CancellationToken cancellationToken)
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