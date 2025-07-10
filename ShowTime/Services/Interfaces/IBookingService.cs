using ShowTime.Entities;


namespace ShowTime.Services.Interfaces
{
    public interface IBookingService : IService<Booking>
    {
        Task<IEnumerable<Booking>?> GetBookingsByUserIdAsync(int userId);
        
    }
}
