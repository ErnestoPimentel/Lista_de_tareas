using TaskManager.Identity.Services;

namespace TaskManager.Identity.UnitTests.Services;

public sealed class CredentialValidatorTests
{
    [Theory]
    [InlineData("SecurePassword123", true)]
    [InlineData("short1A", false)]
    [InlineData("alllowercase123", false)]
    [InlineData("ALLUPPERCASE123", false)]
    [InlineData("NoNumbersHere", false)]
    public void IsValidPassword_ReturnsExpectedResult(string password, bool expected)
    {
        var result = CredentialValidator.IsValidPassword(password);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void NormalizeUserName_TrimsWhitespace()
    {
        var result = CredentialValidator.NormalizeUserName("   neto   ");
        Assert.Equal("neto", result);
    }

    [Fact]
    public void NormalizeEmail_TrimsWhitespaceAndLowercases()
    {
        var result = CredentialValidator.NormalizeEmail("  NETO@EXAMPLE.COM  ");
        Assert.Equal("neto@example.com", result);
    }
}
