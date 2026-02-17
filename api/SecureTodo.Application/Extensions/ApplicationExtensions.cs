using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using SecureTodo.Application.Validators;

namespace SecureTodo.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
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