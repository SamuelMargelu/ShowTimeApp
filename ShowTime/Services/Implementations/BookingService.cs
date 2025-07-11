using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;
using ShowTime.Services.Interfaces;

namespace ShowTime.Services.Implementations
{
    public class BookingService(IBookingRepository bookingRepository)
        : Service<Booking>(bookingRepository), IBookingService
    {
        public async Task<IEnumerable<Booking>?> GetBookingsByUserIdAsync(int userId)
        {
            return await bookingRepository.GetBookingsByUserIdAsync(userId);
        }
    }
}
