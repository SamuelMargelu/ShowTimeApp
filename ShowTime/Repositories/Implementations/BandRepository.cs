using Microsoft.EntityFrameworkCore;
using ShowTime.Context;
using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;

namespace ShowTime.Repositories.Implementations
{
    public class BandRepository(ShowTimeDbContext context) : Repository<Band>(context), IBandRepository
    {
        private readonly DbSet<Band> _dbSet = context.Bands;

        public async Task<IEnumerable<Band>> GetBandsWithFestivalsAsync()
        {
            return await _dbSet.Include(b => b.Festivals).ToListAsync();
        }
    }
}
