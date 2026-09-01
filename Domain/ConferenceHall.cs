namespace ABP_test_task.Entities;

public class ConferenceHall : BaseEntity {
	public string Name { get; set; } = string.Empty;
	public int Capacity { get; set; }
	public decimal BasePricePerHour { get; set; }

	public ICollection<HallService> HallServices { get; set; } = new List<HallService>();
	public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

}
