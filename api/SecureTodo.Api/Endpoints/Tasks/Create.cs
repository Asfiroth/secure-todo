using Ardalis.Result.AspNetCore;
using Mediator;
using SecureTodo.Api.Constants;
using SecureTodo.Api.Endpoints.Tasks.Payloads;
using SecureTodo.Application.UseCases.Tasks.Create;

namespace SecureTodo.Api.Endpoints.Tasks;

public sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(RouteNames.Tasks.Create, HandleAsync)
            .WithTags(RouteNames.Tasks.Tag);
    }
    
    private async Task<IResult> HandleAsync(CreateTaskRequest request, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateTaskCommand(
            Title: request.Title,
            Description: request.Description,
            Priority: request.Priority,
            DueDate: request.DueDate
        ), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.CreatedAtRoute(
                result.Value,
                routeName: nameof(GetById),
                routeValues: new { id = result.Value.Id }
                );
        }

        return result.ToMinimalApiResult();
    }
}