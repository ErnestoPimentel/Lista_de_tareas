namespace TaskManager.Identity.Services;

public static class CredentialValidator
{
    public static string NormalizeUserName(string userName) => userName.Trim();

    public static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

    public static bool IsValidPassword(string password) =>
        password.Length >= 12 &&
        password.Any(char.IsUpper) &&
        password.Any(char.IsLower) &&
        password.Any(char.IsDigit);
}
