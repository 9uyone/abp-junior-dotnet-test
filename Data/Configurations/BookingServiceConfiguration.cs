using ABP_test_task.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABP_test_task.Data.Configurations;

internal class BookingServiceConfiguration : IEntityTypeConfiguration<BookingService> {
	public void Configure(EntityTypeBuilder<BookingService> builder) {
		builder.HasOne(bs => bs.Booking)
			.WithMany(b => b.BookingServices)
			.HasForeignKey(bs => bs.BookingId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(bs => bs.Service)
			.WithMany(s => s.BookingServices)
			.HasForeignKey(bs => bs.ServiceId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.Property(bs => bs.PriceAtBooking)
			.HasColumnType("decimal(18,2)")
			.IsRequired();
	}
}