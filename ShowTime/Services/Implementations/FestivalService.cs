using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;
using ShowTime.Services.Interfaces;

namespace ShowTime.Services.Implementations
{
    public class FestivalService(IFestivalRepository festivalRepository, IBandRepository bandRepository)
        : Service<Festival>(festivalRepository), IFestivalService
    {
        public async Task AddBandtoFestival(int bandId, int festivalId)
        {
            var festival = await festivalRepository.GetByIdAsync(festivalId)
                ?? throw new ArgumentException($"Festival with ID {festivalId} not found.");

            if (festival.BandFestivals.Any(b => b.BandsId == bandId))
            {
                throw new InvalidOperationException($"Band with ID {bandId} is already added to the festival with ID {festivalId}.");
            }
            var band = await bandRepository.GetByIdAsync(bandId)
                ?? throw new ArgumentException($"Band with ID {bandId} not found.");

            festival.BandFestivals.Add(new BandFestival { FestivalsId = festivalId, BandsId = bandId });

            await festivalRepository.UpdateAsync(festival);
            await festivalRepository.SaveChangesAsync();
        }

        public async Task<Festival?> GetFestivalWithDetailsAsync(int id)
        {
            var festival = await festivalRepository.GetFestivalWithDetailsAsync(id);
            if (festival == null)
            {
                throw new KeyNotFoundException($"Festival with ID {id} not found.");
            }
            return festival;
        }
    }
}
