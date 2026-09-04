using ABP_test_task.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABP_test_task.Data.Configurations;

internal class BookingConfiguration : IEntityTypeConfiguration<Booking> {
	public void Configure(EntityTypeBuilder<Booking> builder) {
		builder.HasOne(b => b.Hall)
			.WithMany(h => h.Bookings)
			.HasForeignKey(b => b.HallId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.Property(b => b.StartTime)
			.IsRequired();

		builder.Property(b => b.DurationHours)
			.IsRequired();

		builder.Property(b => b.TotalPrice)
			.HasColumnType("decimal(10,2)")
			.IsRequired();

		builder.HasMany(b => b.BookingServices)
			.WithOne(bs => bs.Booking)
			.HasForeignKey(bs => bs.BookingId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}