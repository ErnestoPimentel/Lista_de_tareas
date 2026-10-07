namespace TaskManager.Identity.Models.Authentication;

public sealed record AuthenticationResponse(
    Guid UserId,
    string UserName,
    string Email,
    string AccessToken,
    DateTime ExpiresAt);
