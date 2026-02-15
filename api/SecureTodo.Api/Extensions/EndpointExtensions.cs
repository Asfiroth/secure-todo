using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SecureTodo.Api.Endpoints;

namespace SecureTodo.Api.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] endpointServices = 
        assembly.DefinedTypes.Where(type =>
                type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();
        
        services.TryAddEnumerable(endpointServices);
        
        return services;
    }

    public static IApplicationBuilder UseEndpoints(this WebApplication app, RouteGroupBuilder? routeBuilder = null)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>().ToList();
        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        IEndpointRouteBuilder builder = routeBuilder is null ? app : routeBuilder;

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }
        
        logger.LogInformation("Total of {TotalEndpoints} Endpoints registered.", endpoints.Count);
        
        return app;
    }
    
    
}