using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Middleware;

/// <summary>
/// Middleware for handling exceptions globally across the application.
/// </summary>
public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred: {Message}", ex.Message);

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var problemDetails = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = "An internal server error occurred.",
                Detail = _env.IsDevelopment() ? ex.ToString() : "Please try again later or contact support."
            };
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}