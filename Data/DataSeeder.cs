namespace ABP_test_task.Data;

using ABP_test_task.Entities;
using Microsoft.EntityFrameworkCore;
/*
public static class DataSeeder {
	public static async Task SeedAsync(AppDbContext context) {
		// Автоматично накатує міграції, якщо база щойно створена
		await context.Database.MigrateAsync();

		// Якщо дані вже є — виходимо
		if (await context.Halls.AnyAsync() || await context.Services.AnyAsync()) {
			return;
		}

		// Сідування початкових залів з ТЗ
		var halls = new List<ConferenceHall>
		{
			new() { Id = Guid.NewGuid(), Name = "Зал A", Capacity = 50, BasePricePerHour = 2000m },
			new() { Id = Guid.NewGuid(), Name = "Зал B", Capacity = 100, BasePricePerHour = 3500m },
			new() { Id = Guid.NewGuid(), Name = "Зал C", Capacity = 30, BasePricePerHour = 1500m }
		};

		// Сідування послуг з ТЗ
		var services = new List<Service>
		{
			new() { Id = Guid.NewGuid(), Name = "Проєктор", Price = 500m },
			new() { Id = Guid.NewGuid(), Name = "Wi-Fi", Price = 300m },
			new() { Id = Guid.NewGuid(), Name = "Звук", Price = 700m }
		};

		await context.Halls.AddRangeAsync(halls);
		await context.Services.AddRangeAsync(services);
		await context.SaveChangesAsync();
	}
}
*/