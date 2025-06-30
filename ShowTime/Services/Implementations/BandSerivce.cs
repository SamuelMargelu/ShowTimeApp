using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;
using ShowTime.Services.Interfaces;

namespace ShowTime.Services.Implementations
{
    public class BandSerivce(IBandRepository bandRepository, IFestivalRepository festivalRepository) : Service<Band>(bandRepository), IBandService
    {
        public async Task<IEnumerable<Band>> GetBandsWithFestivalsAsync()
        {
            return await bandRepository.GetBandsWithFestivalsAsync();
        }
    }
}
