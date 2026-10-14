using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Identity.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(ArgumentException exception)
        {
            await WriteProblemAsync(
                context,
                StatusCodes.Status400BadRequest,
                "Invalid request",
                exception.Message);
        }
        catch(InvalidOperationException exception)
        {
            await WriteProblemAsync(
                context,
                StatusCodes.Status409Conflict,
                "Conflic",
                exception.Message);
        }
        catch(DbUpdateException exception)
        {
            _logger.LogError(
                exception,
                "A database operation failed.");

            await WriteProblemAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Database error",
                "The operation could not be completed.");
        }
        catch(Exception exception)
        {
            _logger.LogError(
                exception,
                "An unhandled exception occurred.");

            await WriteProblemAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Internal server error",
                "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblemAsync(
        HttpContext context,
        int statusCode,
        string title,
        string detail)
    {
        if(context.Response.HasStarted)
            throw new InvalidOperationException("The response has already started.");

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            });
    }
}
