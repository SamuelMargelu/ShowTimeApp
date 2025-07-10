using Microsoft.EntityFrameworkCore;
using ShowTime.Context;
using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;

namespace ShowTime.Repositories.Implementations
{
    public class BookingFestivalDaysRepository(ShowTimeDbContext context) : Repository<BookingFestivalDays>(context), IBookingFestivalDaysRepository
    {
        private readonly DbSet<BookingFestivalDays> _bookingFestivalDays = context.BookingFestivalDays;
        public async Task AddRangeAsync(ICollection<BookingFestivalDays> entities)
        {
            await _bookingFestivalDays.AddRangeAsync(entities);

        }
    }
}
