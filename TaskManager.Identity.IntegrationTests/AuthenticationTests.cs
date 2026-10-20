using System.Net;
using System.Net.Http.Json;
using TaskManager.Identity.Models.Authentication;

namespace TaskManager.Identity.IntegrationTests;

public sealed class AuthenticationTests : IClassFixture<IdentityApiFactory>
{
    private readonly HttpClient _client;

    public AuthenticationTests(IdentityApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsAccessToken()
    {
        var request = new RegisterRequest(
            "testuser",
            "testuser@example.com",
            "SecurePassword123");

        var response = await _client.PostAsJsonAsync(
            "/api/auth/register",
            request,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<AuthenticationResponse>(TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.UserId);
        Assert.False(string.IsNullOrWhiteSpace(result.AccessToken));
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsAccessToken()
    {
        var registration = new RegisterRequest(
            "loginuser",
            "loginuser@example.com",
            "SecurePassword123");

        await _client.PostAsJsonAsync(
            "/api/auth/register",
            registration,
            TestContext.Current.CancellationToken);

        var login = new LoginRequest(
            "loginuser",
            "SecurePassword123");

        var response = await _client.PostAsJsonAsync(
            "/api/auth/login",
            login,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<AuthenticationResponse>(TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.AccessToken));
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
    {
        var registration = new RegisterRequest(
            "invaliduser",
            "invaliduser@example.com",
            "SecurePassword123");

        await _client.PostAsJsonAsync(
            "/api/auth/register",
            registration,
            TestContext.Current.CancellationToken);

        var login = new LoginRequest(
            "invaliduser",
            "WrongPassword123");

        var response = await _client.PostAsJsonAsync(
            "/api/auth/login",
            login,
            TestContext.Current.CancellationToken);

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }
}
