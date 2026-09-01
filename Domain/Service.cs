namespace ABP_test_task.Entities;

public class Service: BaseEntity {
	public string Name { get; set; } = string.Empty;

	public ICollection<HallService> HallServices { get; set; } = new List<HallService>();
	public ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();
}
