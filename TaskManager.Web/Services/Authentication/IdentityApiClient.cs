using TaskManager.Identity.Models.Authentication;

namespace TaskManager.Web.Services.Authentication;

public sealed class IdentityApiClient
{
    private readonly HttpClient _httpClient;

    public IdentityApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AuthenticationResponse?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            "api/auth/login",
            request,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content
            .ReadFromJsonAsync<AuthenticationResponse>(cancellationToken);
    }

    public async Task<AuthenticationResponse?> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            "api/auth/register",
            request,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content
            .ReadFromJsonAsync<AuthenticationResponse>(cancellationToken);
    }
}
