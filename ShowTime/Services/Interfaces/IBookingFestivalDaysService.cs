using ShowTime.Entities;

namespace ShowTime.Services.Interfaces
{
    public interface IBookingFestivalDaysService : IService<BookingFestivalDays>
    {
        Task AddRangeAsync(ICollection<BookingFestivalDays> bookingFestivalDays);
    }
}
