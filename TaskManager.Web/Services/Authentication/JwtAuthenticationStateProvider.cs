namespace TaskManager.Web.Services.Authentication;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

public sealed class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly TokenSession _tokenSession;

    public JwtAuthenticationStateProvider(TokenSession tokenSession)
    {
        _tokenSession = tokenSession;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!_tokenSession.IsAuthenticated)
        {
            return Task.FromResult(
                new AuthenticationState(
                    new ClaimsPrincipal(
                        new ClaimsIdentity())));
        }

        var handler = new JwtSecurityTokenHandler();

        var token = handler.ReadJwtToken(_tokenSession.AccessToken);

        var claims = token.Claims.ToList();

        var identity = new ClaimsIdentity(
            claims,
            authenticationType: "jwt",
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role);

        var principal = new ClaimsPrincipal(identity);

        return Task.FromResult(
            new AuthenticationState(principal));
    }

    public void NotifyUserAuthentication()
    {
        var authenticationState = GetAuthenticationStateAsync();

        NotifyAuthenticationStateChanged(authenticationState);
    }

    public void NotifyUserLogout()
    {
        _tokenSession.Clear();

        var anonymous = new AuthenticationState(
            new ClaimsPrincipal(
                new ClaimsPrincipal()));

        NotifyAuthenticationStateChanged(Task.FromResult(anonymous));
    }
}
