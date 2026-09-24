using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities;

public sealed class User : Entity
{
    public string UserName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string? DisplayName { get; private set; }
    public string? AvatarUrl { get; private set; }
    public string Language { get; private set; } = "es";
    public string Theme { get; private set; } = "system";

    private User()
    {

    }

    public User(string userName, string email, string passwordHash)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
    }

    public void UpdateProfile(string displayName, string? avatarUrl)
    {
        DisplayName = displayName;
        AvatarUrl = avatarUrl;
    }

    public void UpdatePreferences(string language, string theme)
    {
        Language = language;
        Theme = theme;
    }
}
