using ShowTime.Entities;

namespace ShowTime.Services.Interfaces
{
    public interface IBandFestivalsService : IService<BandFestival>
    {
        Task DeleteByBandAndFestivalIdAsync(BandFestival bandFestival);
    }
}
