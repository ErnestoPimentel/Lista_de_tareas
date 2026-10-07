using TaskManager.Identity.Models.Authentication;

namespace TaskManager.Identity.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

    Task<AuthenticationResponse?> LoginAsync(LoginRequest request, CancellationToken cancellation = default);
}
