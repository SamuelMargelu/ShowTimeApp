using Microsoft.EntityFrameworkCore;
using ShowTime.Context;
using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;

namespace ShowTime.Repositories.Implementations
{
    public class FestivalRepository : Repository<Festival>, IFestivalRepository
    {
        private readonly ShowTimeDbContext _context;
        private readonly DbSet<Festival> _dbSet;

        public FestivalRepository(ShowTimeDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Festivals;
        }

        public async Task<IEnumerable<Festival>> GetFestivalsWithBandsAsync()
        {
            return await _dbSet.Include(f => f.Bands).ToListAsync();
        }
    }
}
