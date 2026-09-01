namespace ABP_test_task.Entities;

public class HallService: BaseEntity {
	public int HallId { get; set; }
	public ConferenceHall Hall { get; set; } = null!;

	public int ServiceId { get; set; }
	public Service Service { get; set; } = null!;

	public decimal Price { get; set; }
}