using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SecureTodo.Application.Services;
using SecureTodo.Domain.Base;

namespace SecureTodo.Infrastructure.Data.Interceptors;

public sealed class UpdateAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    private readonly IAuthService _authService;
    
    public UpdateAuditableEntitiesInterceptor(IAuthService authService)
    {
        _authService = authService;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var userId = _authService.GetLoggedUserId();

        if (userId is null)
        {
            //throw new UnauthorizedAccessException("User is not authenticated.");
            userId = Guid.Empty;
        }
        
        var entries = dbContext.ChangeTracker.Entries<Auditable>();
        
        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property("CreatedBy").CurrentValue = userId.Value;
                    break;
                case EntityState.Modified:
                    entry.Property("UpdatedBy").CurrentValue = userId.Value;
                    entry.Property("UpdatedAt").CurrentValue = DateTimeOffset.UtcNow;
                    break;
                case EntityState.Deleted:
                    entry.Property("IsDeleted").CurrentValue = true;
                    entry.Property("DeletedBy").CurrentValue = userId.Value;
                    entry.Property("DeletedAt").CurrentValue = DateTimeOffset.UtcNow;
                    entry.State = EntityState.Modified;
                    break;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}