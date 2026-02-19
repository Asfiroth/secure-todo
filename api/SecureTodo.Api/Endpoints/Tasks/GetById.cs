using Ardalis.Result.AspNetCore;
using Mediator;
using SecureTodo.Api.Constants;
using SecureTodo.Application.UseCases.Tasks.GetById;

namespace SecureTodo.Api.Endpoints.Tasks;

public sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(RouteNames.Tasks.GetById, HandleAsync)
            .WithTags(RouteNames.Tasks.Tag)
            .WithName(nameof(GetById));
    }

    private async Task<IResult> HandleAsync(Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetTaskByIdQuery(id), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        return result.ToMinimalApiResult();
    }
}