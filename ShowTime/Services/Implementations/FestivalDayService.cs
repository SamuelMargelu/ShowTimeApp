using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;
using ShowTime.Services.Interfaces;

namespace ShowTime.Services.Implementations
{
    public class FestivalDayService(IFestivalDayRepository festivalDayRepository) 
        : Service<FestivalDay>(festivalDayRepository), IFestivalDayService
    {
        public async Task AddRangeAsync(IEnumerable<FestivalDay> festivalDays)
        {
            if (festivalDays == null || !festivalDays.Any())
            {
                throw new ArgumentException("The collection of festival days cannot be null or empty.", nameof(festivalDays));
            }
            await festivalDayRepository.AddRangeAsync(festivalDays);
            await festivalDayRepository.SaveChangesAsync();
        }
    }
}
