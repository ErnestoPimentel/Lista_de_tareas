namespace TaskManager.Web.Services.Authentication;

using TaskManager.Contracts.Authentication;

public sealed class AuthenticationSessionService
{
    private readonly IdentityApiClient _identityApiClient;
    private readonly TokenSession _tokenSession;
    private readonly JwtAuthenticationStateProvider _authenticationStateProvider;

    public AuthenticationSessionService(
        IdentityApiClient identityApiClient,
        TokenSession tokenSession,
        JwtAuthenticationStateProvider authenticationStateProvider)
    {
        _identityApiClient = identityApiClient;
        _tokenSession = tokenSession;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<bool> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _identityApiClient.LoginAsync(
            request,
            cancellationToken);

        if (response is null)
            return false;

        _tokenSession.SetToken(
            response.AccessToken,
            response.ExpiresAt);

        _authenticationStateProvider.NotifyUserAuthentication();

        return true;
    }

    public async Task<bool> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _identityApiClient.RegisterAsync(
            request,
            cancellationToken);

        if (response is null)
            return false;

        _tokenSession.SetToken(
            response.AccessToken,
            response.ExpiresAt);

        _authenticationStateProvider.NotifyUserAuthentication();

        return true;
    }

    public void Logout() => _authenticationStateProvider.NotifyUserLogout();
}
