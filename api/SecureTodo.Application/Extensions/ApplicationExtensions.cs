using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace SecureTodo.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatorBehaviours();
        return services;
    }
    
    public static IServiceCollection AddMediatorBehaviours(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehaviour<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>),typeof(ExceptionHandlingBehaviour<,>));
        return services;
    }
}