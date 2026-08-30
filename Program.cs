using ABP_test_task.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi()
	.AddProblemDetails()
	.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.MapOpenApi();
}

//app.UseHttpsRedirection();
app.UseExceptionHandler();

app.MapGet("/{id}", (int id) => {
	return id switch {
		< 0 => throw new BadHttpRequestException("ID cannot be negative"),
		> 100 => throw new UnauthorizedAccessException("ID cannot be greater than 100"),
		_ => Results.Ok(new { Id = id, Message = "Request successful" })
	};
});

app.Run();