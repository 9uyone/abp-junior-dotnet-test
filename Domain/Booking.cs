namespace ABP_test_task.Entities;

public class Booking: BaseEntity {
	public int HallId { get; set; }
	public ConferenceHall Hall { get; set; } = null!;

	public DateTime StartTime { get; set; }
	public int DurationHours { get; set; }
	public DateTime EndTime => StartTime.AddHours(DurationHours);

	public decimal TotalPrice { get; set; }

	public ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();
}