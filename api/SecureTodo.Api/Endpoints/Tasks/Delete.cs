using Ardalis.Result.AspNetCore;
using Mediator;
using SecureTodo.Application.Tasks.Delete;

namespace SecureTodo.Api.Endpoints.Tasks;

public sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapDelete(RouteNames.Tasks.Delete, Handler)
            .WithTags(RouteNames.Tasks.Tag);
    }
    
    private async Task<IResult> Handler(Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteTaskCommand(id), cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok();
        }

        return result.ToMinimalApiResult();
    }
}