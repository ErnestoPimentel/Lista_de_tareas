namespace TaskManager.Identity.Models.Authentication;

public sealed record LoginRequest(
    string UserName,
    string Password);