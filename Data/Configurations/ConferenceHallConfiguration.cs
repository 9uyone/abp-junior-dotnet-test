using ABP_test_task.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABP_test_task.Data.Configurations;

internal class ConferenceHallConfiguration : IEntityTypeConfiguration<ConferenceHall> {
	public void Configure(EntityTypeBuilder<ConferenceHall> builder) {
		builder.Property(h => h.Name)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(h => h.Capacity)
			.IsRequired();

		builder.Property(h => h.BasePricePerHour)
			.IsRequired()
			.HasColumnType("decimal(18,2)");

		builder.HasMany(h => h.HallServices)
			.WithOne(hs => hs.Hall)
			.HasForeignKey(hs => hs.HallId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasMany(h => h.Bookings)
			.WithOne(b => b.Hall)
			.HasForeignKey(b => b.HallId);
	}
}