using Microsoft.EntityFrameworkCore;
using ShowTime.Context;
using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;

namespace ShowTime.Repositories.Implementations
{
    public class FestivalRepository(ShowTimeDbContext context) : Repository<Festival>(context), IFestivalRepository
    {
        private readonly DbSet<Festival> _festivals = context.Festivals;

        public async Task<Festival?> GetFestivalWithDetailsAsync(int id)
        {
            return await _festivals
                .Include(f => f.BandFestivals)
                    .ThenInclude(bf => bf.Band)
                .Include(f => f.FestivalDays)
                .Include(f => f.Bookings)
                    .ThenInclude(b => b.ApplicationUser)
                .Include(f => f.Bookings)
                    .ThenInclude(b => b.BookingFestivalDays)
                        .ThenInclude(bfd => bfd.FestivalDay)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

    }
}
