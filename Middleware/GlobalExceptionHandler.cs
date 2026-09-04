using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ABP_test_task.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler {
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken) {
		logger.LogError(exception, "Error occured: {Message}", exception.Message);

		var (statusCode, title, detail, errors) = exception switch {
			ValidationException validationException => (
				StatusCodes.Status400BadRequest,
				"Validation failed",
				"One or more validation errors occurred.",
				string.Join("\n", validationException.Errors.Select(error => error.ErrorMessage))
			),
			BadHttpRequestException => (StatusCodes.Status400BadRequest, "Bad request to API", exception.Message, null),
			UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized access to API", null, null),

			// Бізнес-помилки: безпечно показуємо наше власне повідомлення
			ArgumentException => (StatusCodes.Status400BadRequest, "Invalid request data", exception.Message, null),
			InvalidOperationException => (StatusCodes.Status409Conflict, "Operation is not allowed", exception.Message, null),

			// Усі непередбачені системні падіння маскуємо повністю
			_ => (StatusCodes.Status500InternalServerError, "Internal server error", "An unexpected error occurred. Please try again later.", null)
		};

		var details = new ProblemDetails {
			Status = statusCode,
			Title = title,
			Detail = detail
		};

		if (errors is not null)
			details.Extensions["errors"] = errors;

		httpContext.Response.StatusCode = details.Status.Value;
		await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);

		return true;
	}
}