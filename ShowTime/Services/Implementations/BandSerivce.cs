using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;
using ShowTime.Services.Interfaces;

namespace ShowTime.Services.Implementations
{
    public class BandSerivce(IBandRepository bandRepository, IFestivalRepository festivalRepository) : Service<Band>(bandRepository), IBandService
    {
        private readonly IBandRepository _bandRepository = bandRepository;
        private readonly IFestivalRepository _estivalRepository = festivalRepository;

        public async Task<IEnumerable<Band>> GetBandsWithFestivalsAsync()
        {
            return await _bandRepository.GetBandsWithFestivalsAsync();
        }
    }
}
