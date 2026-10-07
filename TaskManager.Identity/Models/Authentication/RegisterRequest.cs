namespace TaskManager.Identity.Models.Authentication;

public sealed record RegisterRequest(
    string UserName,
    string Email,
    string Password);
