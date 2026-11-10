namespace TaskManager.Web.Services.Authentication;

public sealed class TokenSession
{
    public string? AccessToken { get; private set; }

    public DateTime? ExpiresAt { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(AccessToken)
        && ExpiresAt.HasValue
        && ExpiresAt.Value > DateTime.UtcNow;

    public void SetToken(string accessToken, DateTime expiresAt)
    {
        AccessToken = accessToken;
        ExpiresAt = expiresAt;
    }

    public void Clear()
    {
        AccessToken = null;
        ExpiresAt = null;
    }
}
