namespace TaskManager.Contracts.Authentication;

public sealed record AuthenticationResponse(
    Guid UserId,
    string UserName,
    string Email,
    string AccessToken,
    DateTime ExpiresAt);