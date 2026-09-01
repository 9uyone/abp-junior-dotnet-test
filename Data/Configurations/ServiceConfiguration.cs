using ABP_test_task.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABP_test_task.Data.Configurations;

internal class ServiceConfiguration : IEntityTypeConfiguration<Service> {
	public void Configure(EntityTypeBuilder<Service> builder) {
		builder.Property(s => s.Name)
			.IsRequired()
			.HasMaxLength(100);
	}
}