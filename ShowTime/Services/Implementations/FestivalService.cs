using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;
using ShowTime.Services.Interfaces;

namespace ShowTime.Services.Implementations
{
    public class FestivalService(IFestivalRepository festivalRepository, IRepository<Band> bandRepository) : Service<Festival>(festivalRepository), IFestivalService
    {
        private readonly IFestivalRepository _festivalRepository = festivalRepository;
        private readonly IRepository<Band> _bandRepository = bandRepository;
        public async Task<IEnumerable<Festival>> GetFestivalsWithBandsAsync()
        {
            return await _festivalRepository.GetFestivalsWithBandsAsync();
        }

        public async Task AddBandtoFestival(int bandId, int festivalId)
        {
            var festival = await _festivalRepository.GetByIdAsync(festivalId)
                ?? throw new ArgumentException($"Festival with ID {festivalId} not found.");

            var band = await _bandRepository.GetByIdAsync(bandId)
                ?? throw new ArgumentException($"Band with ID {bandId} not found.");

            if (festival.Bands.Any(b => b.Id == bandId))
            {
                throw new InvalidOperationException($"Band with ID {bandId} is already added to the festival with ID {festivalId}.");
            }

            festival.Bands.Add(band);
            await _festivalRepository.UpdateAsync(festival);
            await _festivalRepository.SaveChangesAsync();
        }
    }
}
