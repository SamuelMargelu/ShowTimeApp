using ShowTime.Entities;

namespace ShowTime.Repositories.Interfaces
{
    public interface IBandFestivalRepository : IRepository<BandFestival>
    {
        Task DeleteByBandAndFestivalIdAsync(BandFestival bandFestival);
    }
}
