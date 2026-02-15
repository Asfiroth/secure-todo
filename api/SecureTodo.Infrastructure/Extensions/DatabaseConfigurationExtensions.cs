using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureTodo.Application.QueryServices;
using SecureTodo.Application.Repositories;
using SecureTodo.Infrastructure.Data;
using SecureTodo.Infrastructure.Data.Interceptors;
using SecureTodo.Infrastructure.QueryServices;
using SecureTodo.Infrastructure.Repositories;

namespace SecureTodo.Infrastructure.Extensions;

public static class DatabaseConfigurationExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

        services.AddDbContext<SecureTodoDbContext>((provider, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("TodoDb"));
            var auditableInterceptor = provider.GetService<UpdateAuditableEntitiesInterceptor>();
            IInterceptor[] interceptors = [auditableInterceptor!];
            options.AddInterceptors(interceptors);
        });
        
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<ITaskItemRepository, TaskItemRepository>();
        services.AddTransient<ITaskItemQueryService, TaskItemQueryService>();
        return services;
    }
}