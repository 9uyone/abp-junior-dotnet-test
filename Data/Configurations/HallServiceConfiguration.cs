using ABP_test_task.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABP_test_task.Data.Configurations;

internal class HallServiceConfiguration : IEntityTypeConfiguration<HallService> {
	public void Configure(EntityTypeBuilder<HallService> builder) {
		builder.HasOne(hs => hs.Hall)
			.WithMany(h => h.HallServices)
			.HasForeignKey(hs => hs.HallId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(hs => hs.Service)
			.WithMany(s => s.HallServices)
			.HasForeignKey(hs => hs.ServiceId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.Property(hs => hs.Price)
			.HasColumnType("decimal(18,2)")
			.IsRequired();

		builder.HasIndex(hs => new { hs.HallId, hs.ServiceId })
			.IsUnique();
	}
}