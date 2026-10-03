using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TaskManager.Application.Interfaces;

namespace TaskManager.Infrastructure.Identity;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext
        .User
        .Identity?
        .IsAuthenticated == true;

    public Guid UserId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(value, out var userId)
                ? userId
                : throw new UnauthorizedAccessException(
                    "The authenticated user does not contain a valid identifier.");
        }
    }
}
