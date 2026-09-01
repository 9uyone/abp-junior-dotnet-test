using ABP_test_task.Data.Configurations;
using ABP_test_task.Entities;
using Microsoft.EntityFrameworkCore;

namespace ABP_test_task.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options) {
	public DbSet<Service> Services { get; set; }
	public DbSet<HallService> HallServices { get; set; }
	public DbSet<ConferenceHall> Halls { get; set; }
	public DbSet<BookingService> BookingServices { get; set; }
	public DbSet<Booking> Bookings { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.ApplyConfiguration(new ServiceConfiguration());
		modelBuilder.ApplyConfiguration(new HallServiceConfiguration());
		modelBuilder.ApplyConfiguration(new ConferenceHallConfiguration());
		modelBuilder.ApplyConfiguration(new BookingServiceConfiguration());
		modelBuilder.ApplyConfiguration(new BookingConfiguration());

		base.OnModelCreating(modelBuilder);
	}
}
