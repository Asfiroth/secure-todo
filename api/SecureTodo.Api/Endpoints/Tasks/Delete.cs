using Ardalis.Result.AspNetCore;
using Mediator;
using SecureTodo.Api.Constants;
using SecureTodo.Application.UseCases.Tasks.Delete;

namespace SecureTodo.Api.Endpoints.Tasks;

public sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapDelete(RouteNames.Tasks.Delete, HandleAsync)
            .WithTags(RouteNames.Tasks.Tag)
            .RequireAuthorization();
    }
    
    private async Task<IResult> HandleAsync(Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteTaskCommand(id), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.NoContent();
        }

        return result.ToMinimalApiResult();
    }
}