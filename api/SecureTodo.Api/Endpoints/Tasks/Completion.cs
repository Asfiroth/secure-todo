using Ardalis.Result.AspNetCore;
using Mediator;
using SecureTodo.Api.Constants;
using SecureTodo.Application.UseCases.Tasks.ChangeStatus;

namespace SecureTodo.Api.Endpoints.Tasks;

public sealed class Completion : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(RouteNames.Tasks.Completion, HandleCompletionAsync)
            .WithTags(RouteNames.Tasks.Tag);
        
        builder
            .MapDelete(RouteNames.Tasks.Completion, HandleIncompletionAsync)
            .WithTags(RouteNames.Tasks.Tag);
    }
    
    private async Task<IResult> HandleCompletionAsync(Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
       var result = await mediator.Send(new ChangeTaskStatusCommand(id, IsCompleted: true), cancellationToken);

       if (result.IsSuccess)
       {
           return TypedResults.NoContent();
       }

       return result.ToMinimalApiResult();
    }
    
    private async Task<IResult> HandleIncompletionAsync(Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ChangeTaskStatusCommand(id, IsCompleted: false), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }

        return result.ToMinimalApiResult();
    }
    
    
}