using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;
using ShowTime.Services.Interfaces;

namespace ShowTime.Services.Implementations
{
    public class FestivalService(IFestivalRepository festivalRepository, IBandRepository bandRepository) : Service<Festival>(festivalRepository), IFestivalService
    {
        public async Task<IEnumerable<Festival>> GetFestivalsWithBandsAsync()
        {
            return await festivalRepository.GetFestivalsWithBandsAsync();
        }

        public async Task AddBandtoFestival(int bandId, int festivalId)
        {
            var festival = await festivalRepository.GetByIdAsync(festivalId)
                ?? throw new ArgumentException($"Festival with ID {festivalId} not found.");

            var band = await bandRepository.GetByIdAsync(bandId)
                ?? throw new ArgumentException($"Band with ID {bandId} not found.");

            if (festival.Bands.Any(b => b.Id == bandId))
            {
                throw new InvalidOperationException($"Band with ID {bandId} is already added to the festival with ID {festivalId}.");
            }

            festival.Bands.Add(band);
            await festivalRepository.UpdateAsync(festival);
            await festivalRepository.SaveChangesAsync();
        }
    }
}
