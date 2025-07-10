using ShowTime.Entities;

namespace ShowTime.Repositories.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>?> GetBookingsByUserIdAsync(int userId);
    }
}
