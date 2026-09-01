using ABP_test_task.Data;
using ABP_test_task.Middleware;
using ABP_test_task.Services.Pricing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddProblemDetails()
	.AddExceptionHandler<GlobalExceptionHandler>()
	.AddEndpointsApiExplorer()
	.AddSwaggerGen()
	.AddDbContext<AppDbContext>(options => {
		options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
		options.UseSnakeCaseNamingConvention();
	});

builder.Services
	.AddSingleton<IRentalPriceCalculator, RentalPriceCalculator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
	var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	await DataSeeder.SeedAsync(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseExceptionHandler();


app.Run();