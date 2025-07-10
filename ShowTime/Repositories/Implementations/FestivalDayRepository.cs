using Microsoft.EntityFrameworkCore;
using ShowTime.Context;
using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;

namespace ShowTime.Repositories.Implementations
{
    public class FestivalDayRepository(ShowTimeDbContext context) : Repository<FestivalDay>(context), IFestivalDayRepository
    {
        private readonly DbSet<FestivalDay> _festivalDays = context.FestivalDays;
        public async Task AddRangeAsync(IEnumerable<FestivalDay> festivalDays)
        {
            await _festivalDays.AddRangeAsync(festivalDays);
        }
    }
}
