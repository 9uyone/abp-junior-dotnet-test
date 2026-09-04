using ABP_test_task.Data;
using ABP_test_task.DTOs.Bookings;
using ABP_test_task.DTOs.Halls;
using ABP_test_task.Endpoints;
using ABP_test_task.Middleware;
using ABP_test_task.Services.Analytics;
using ABP_test_task.Services.Booking;
using ABP_test_task.Services.Hall;
using ABP_test_task.Services.Pricing;
using ABP_test_task.Validators.Bookings;
using ABP_test_task.Validators.Halls;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddProblemDetails()
	.AddExceptionHandler<GlobalExceptionHandler>()
	.AddEndpointsApiExplorer()
	.AddSwaggerGen(c => {
		c.MapType<TimeOnly>(() => new Microsoft.OpenApi.OpenApiSchema {
			Type = Microsoft.OpenApi.JsonSchemaType.String,
			Format = "time",
			Example = "14:00"
		});
		c.MapType<DateOnly>(() => new Microsoft.OpenApi.OpenApiSchema {
			Type = Microsoft.OpenApi.JsonSchemaType.String,
			Format = "date",
			Example = "2026-09-04"
		});
	})
	.AddDbContext<AppDbContext>(options => {
		options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
		options.UseSnakeCaseNamingConvention();
	});

builder.Services
	.AddSingleton<IRentalPriceCalculator, RentalPriceCalculator>()
	.AddSingleton<IBookingTimePolicy, BookingTimePolicy>()
	.AddScoped<IHallService, HallService>()
	.AddScoped<IBookingService, BookingService>()
	.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services
	.AddScoped<IValidator<CreateHallRequest>, CreateHallRequestValidator>()
	.AddScoped<IValidator<UpdateHallRequest>, UpdateHallRequestValidator>()
	.AddScoped<IValidator<FindAvailableHallsQuery>, FindAvailableHallsQueryValidator>()
	.AddScoped<IValidator<CreateBookingRequest>, CreateBookingRequestValidator>();

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

app.MapGroup("/api/halls")
   .MapHallEndpoints()
   .WithTags("Conference halls");

app.MapGroup("/api/bookings")
   .MapBookingEndpoints()
   .WithTags("Bookings");

app.MapReportsEndpoints();

app.Run();
