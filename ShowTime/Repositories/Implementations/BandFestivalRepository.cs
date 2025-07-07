using Microsoft.EntityFrameworkCore;
using ShowTime.Context;
using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;

namespace ShowTime.Repositories.Implementations
{
    public class BandFestivalRepository(ShowTimeDbContext context) : Repository<BandFestival>(context), IBandFestivalRepository
    {

        private readonly DbSet<BandFestival> _bandFestivals = context.BandFestivals;
        public Task DeleteByBandAndFestivalIdAsync(BandFestival bandFestival)
        {
            _bandFestivals.Remove(bandFestival);
            return Task.CompletedTask;
        }
    }
}
