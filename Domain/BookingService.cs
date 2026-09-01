namespace ABP_test_task.Entities;

public class BookingService: BaseEntity {
	public int BookingId { get; set; }
	public Booking Booking { get; set; } = null!;

	public int ServiceId { get; set; }
	public Service Service { get; set; } = null!;

	public decimal PriceAtBooking { get; set; }
}