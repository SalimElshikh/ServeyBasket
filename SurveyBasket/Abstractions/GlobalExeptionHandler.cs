﻿using Microsoft.AspNetCore.Diagnostics;

namespace SurveyBasket.Abstractions;

public class GlobalExeptionHandler(ILogger<GlobalExeptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception , "Something went wrong : {Message}" , exception.Message);
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error ",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"

        };
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}
