using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Identity.Data;
using TaskManager.Identity.Models;
using TaskManager.Identity.Models.Authentication;

namespace TaskManager.Identity.Services;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly IdentityDbContext _context;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly IConfiguration _configuration;

    public AuthenticationService(
        IdentityDbContext context,
        IPasswordHasher<ApplicationUser> passwordHasher,
        IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task<AuthenticationResponse> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var exists = await _context.User.AnyAsync(
            x => x.UserName == request.UserName
            || x.Email == request.Email,
            cancellationToken);

        if (exists)
            throw new InvalidOperationException(
                "The username or email is already registered.");

        var user = new ApplicationUser(
            request.UserName,
            request.Email);

        var passwordHash = _passwordHasher.HashPassword(
            user,
            request.Password);

        user.SetPasswordHash(passwordHash);

        await _context.User.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return CreateAuthenticationResponse(user);
    }

    public async Task<AuthenticationResponse?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await _context.User.FirstOrDefaultAsync(
            x => x.UserName == request.UserName,
            cancellationToken);

        if (user is null)
            return null;

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (result == PasswordVerificationResult.Failed)
            return null;

        return CreateAuthenticationResponse(user);
    }

    private AuthenticationResponse CreateAuthenticationResponse(ApplicationUser user)
    {
        var expirationMinutes = _configuration.GetValue<int>("Jwt:ExpirationMinutes");

        var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException(
                    "JWT signing key is not configured.")));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AuthenticationResponse(
            user.Id,
            user.UserName,
            user.Email,
            new JwtSecurityTokenHandler().WriteToken(token),
            expiresAt);
    }
}
