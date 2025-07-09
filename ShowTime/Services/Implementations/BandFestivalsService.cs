using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;
using ShowTime.Services.Interfaces;

namespace ShowTime.Services.Implementations
{
    public class BandFestivalsService(IBandFestivalRepository bandFestivalRepository)
        : Service<BandFestival>(bandFestivalRepository), IBandFestivalsService
    {
        public async Task DeleteByBandAndFestivalIdAsync(BandFestival bandFestival)
        {

            await bandFestivalRepository.DeleteByBandAndFestivalIdAsync(bandFestival);
            await bandFestivalRepository.SaveChangesAsync();
        }
    }
}
