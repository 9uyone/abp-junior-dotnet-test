namespace ABP_test_task.Data;

using ABP_test_task.Entities;
using Microsoft.EntityFrameworkCore;

public static class DataSeeder {
	public static async Task SeedAsync(AppDbContext context) {
		// Автоматично накатує міграції, якщо база щойно створена
		await context.Database.MigrateAsync();

		// Якщо дані вже є - виходимо
		if (await context.Halls.AnyAsync() || await context.Services.AnyAsync())
			return;

		var halls = new List<ConferenceHall> {
			new() { Name = "Зал A", Capacity = 50, BasePricePerHour = 2000m },
			new() { Name = "Зал B", Capacity = 100, BasePricePerHour = 3500m },
			new() { Name = "Зал C", Capacity = 30, BasePricePerHour = 1500m }
		};

		var services = new List<Service> {
			new() { Name = "Проєктор" },
			new() { Name = "Wi-Fi" },
			new() { Name = "Звук" }
		};

		var hallServices = new List<HallService> {
			new() { Hall = halls[0], Service = services[0], Price = 500m },
			new() { Hall = halls[1], Service = services[2], Price = 300m },
			new() { Hall = halls[2], Service = services[1], Price = 700m }
		};

		await context.Halls.AddRangeAsync(halls);
		await context.Services.AddRangeAsync(services);
		await context.HallServices.AddRangeAsync(hallServices);

		await context.SaveChangesAsync();
	}
}