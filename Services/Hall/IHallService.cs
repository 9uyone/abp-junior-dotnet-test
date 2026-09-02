using ABP_test_task.DTOs.Halls;

namespace ABP_test_task.Services.Hall;

public interface IHallService {
	Task<IReadOnlyList<AvailableHallDto>> GetAllAsync(CancellationToken cancellationToken);
	Task<int> CreateHallAsync(CreateHallRequest request, CancellationToken cancellationToken);
	Task<bool> UpdateHallAsync(int id, UpdateHallRequest request, CancellationToken cancellationToken);
	Task<bool> DeleteHallAsync(int id, CancellationToken cancellationToken);
	Task<IReadOnlyList<AvailableHallDto>> FindAvailableHallsAsync(FindAvailableHallsQuery query, CancellationToken cancellationToken);
}
