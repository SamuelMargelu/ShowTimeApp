using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;
using ShowTime.Services.Interfaces;

namespace ShowTime.Services.Implementations
{
    public class BookingFestivalDaysService(IBookingFestivalDaysRepository festivalDaysRepository)
        : Service<BookingFestivalDays>(festivalDaysRepository), IBookingFestivalDaysService
    {
        public async Task AddRangeAsync(ICollection<BookingFestivalDays> bookingFestivalDays)
        {
            if (bookingFestivalDays == null || !bookingFestivalDays.Any())
            {
                throw new ArgumentException("The collection of booking festival days cannot be null or empty.", nameof(bookingFestivalDays));
            }
            await festivalDaysRepository.AddRangeAsync(bookingFestivalDays);
            await festivalDaysRepository.SaveChangesAsync();
        }
    }
}
