using ABP_test_task.DTOs.Bookings;

namespace ABP_test_task.Services.Booking;

public interface IBookingService {
	Task<BookingConfirmationDto> CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken);
}
