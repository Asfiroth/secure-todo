namespace SecureTodo.Application.Services;

public interface IAuthService
{
    Guid? GetLoggedUserId();
}