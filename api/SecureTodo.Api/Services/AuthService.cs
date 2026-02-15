using System.Security.Claims;
using SecureTodo.Application.Services;

namespace SecureTodo.Api.Services;

public sealed class AuthService: IAuthService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<AuthService> _logger;
    
    private Guid? _userId = null;
    
    public AuthService(IHttpContextAccessor contextAccessor, ILogger<AuthService> logger)
    {
        _contextAccessor = contextAccessor;
        _logger = logger;
    }
    
    public Guid? GetLoggedUserId()
    {
        if (_userId is not null)
            return _userId;
        
        _logger.LogInformation("Attempting to retrieve user id from context");
        
        var employeeId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(employeeId, out var parsedUserId)) return null;
        
        _userId = parsedUserId;
        return parsedUserId;
    }
}