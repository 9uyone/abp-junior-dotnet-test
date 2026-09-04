using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ABP_test_task.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler {
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken) {
		logger.LogError(exception, "Error occured: {Message}", exception.Message);

		var (statusCode, title, errors) = exception switch {
			ValidationException validationException => (
				StatusCodes.Status400BadRequest,
				"Validation failed",
				/*validationException.Errors
					.GroupBy(error => error.PropertyName)
					.ToDictionary(group => group.Key, group => group.Select(error => error.ErrorMessage).ToArray())*/
				string.Join("\n", validationException.Errors.Select(error => $"{error.ErrorMessage}"))
			),
			BadHttpRequestException => (StatusCodes.Status400BadRequest, "Bad request to API", null),
			UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized access to API", null),
			ArgumentException => (StatusCodes.Status400BadRequest, "Invalid request data", null),
			InvalidOperationException => (StatusCodes.Status409Conflict, "Operation is not allowed", null),
			_ => (StatusCodes.Status500InternalServerError, "Internal server error", null)
		};

		var details = new ProblemDetails {
			Status = statusCode,
			Title = title
		};

		if (errors is not null)
			details.Extensions["errors"] = errors;

		httpContext.Response.StatusCode = details.Status.Value;
		await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);

		return true;
	}
}
