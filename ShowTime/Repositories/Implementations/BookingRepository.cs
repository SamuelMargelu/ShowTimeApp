using Microsoft.EntityFrameworkCore;
using ShowTime.Context;
using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;

namespace ShowTime.Repositories.Implementations
{
    public class BookingRepository(ShowTimeDbContext context) : Repository<Booking>(context), IBookingRepository
    {
        private readonly DbSet<Booking> _bookings = context.Bookings;
    }
}
