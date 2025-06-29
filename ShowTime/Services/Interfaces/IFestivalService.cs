using ShowTime.Entities;

namespace ShowTime.Services.Interfaces
{
    public interface IFestivalService : IService<Festival>
    {
        Task<IEnumerable<Festival>> GetFestivalsWithBandsAsync();
        Task AddBandtoFestival(int bandId, int festivalId);
    }
}
