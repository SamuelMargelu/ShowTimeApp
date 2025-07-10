using ShowTime.Entities;

namespace ShowTime.Repositories.Interfaces
{
    public interface IBookingFestivalDaysRepository : IRepository<BookingFestivalDays>
    {
        Task AddRangeAsync(ICollection<BookingFestivalDays> entities);
    }
}
