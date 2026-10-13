using Microsoft.AspNetCore.Mvc;
using TaskManager.Identity.Models.Authentication;
using TaskManager.Identity.Services;

namespace TaskManager.Identity.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _authenticationService.RegisterAsync(
                request,
                cancellationToken);

            return Created("/api/auth/login", response);
        }
        catch(InvalidOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _authenticationService.LoginAsync(
            request,
            cancellationToken);

        if (response is null)
        {
            return Unauthorized(
                new { message = "Usuario o contraseña incorrectos." });
        }

        return Ok(response);
    }
}
