using Microsoft.EntityFrameworkCore;
using ShowTime.Context;
using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;

namespace ShowTime.Repositories.Implementations
{
    public class BookingRepository(ShowTimeDbContext context) : Repository<Booking>(context), IBookingRepository
    {
        private readonly DbSet<Booking> _bookings = context.Bookings;

        public async Task<IEnumerable<Booking>?> GetBookingsByUserIdAsync(int userId)
        {
            return await _bookings
                         .Include(b => b.Festival)
                             .ThenInclude(f => f.FestivalDays)
                         .Include(b => b.BookingFestivalDays)
                             .ThenInclude(bfd => bfd.FestivalDay)
                         .Where(b => b.ApplicationUserId == userId)
                         .ToListAsync();
        }
    }
}
