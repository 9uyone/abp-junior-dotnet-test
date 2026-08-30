using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ABP_test_task.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler {
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken) {
		logger.LogError(exception, "Error occured: {Message}", exception.Message);

		var (statusCode, title) = exception switch {
			BadHttpRequestException => (StatusCodes.Status400BadRequest, "Bad request to API"),
			UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized access to API"),
			_ => (StatusCodes.Status500InternalServerError, "Internal server error")
		};

		var problemDetails = new ProblemDetails {
			Status = statusCode,
			Title = title,
			Detail = exception.Message
		};

		httpContext.Response.StatusCode = problemDetails.Status.Value;
		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

		return true;
	}
}
