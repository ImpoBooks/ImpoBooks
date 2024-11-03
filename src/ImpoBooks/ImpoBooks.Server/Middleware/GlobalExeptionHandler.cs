using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ImpoBooks.Server.Middleware;

public class GlobalExeptionHandler(ILogger<GlobalExeptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExeptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exeption has occured: {Message}", exception.Message);

        ProblemDetails problemDetails = new()
        {
            Title = "Internal Server Error",
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://developer.mozilla.org/ru/docs/Web/HTTP/Status/500"
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
}