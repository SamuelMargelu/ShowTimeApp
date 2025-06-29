using Microsoft.EntityFrameworkCore;
using ShowTime.Context;
using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;

namespace ShowTime.Repositories.Implementations
{
    public class BandRepository : Repository<Band>, IBandRepository
    {
        private readonly ShowTimeDbContext _context;
        private readonly DbSet<Band> _dbSet;

        public BandRepository(ShowTimeDbContext context) : base(context) 
        {
            _context = context;
            _dbSet = context.Bands;
        }

        public async Task<IEnumerable<Band>> GetBandsWithFestivalsAsync()
        {
            return await _dbSet.Include(b => b.Festivals).ToListAsync();
        }
    }
}
