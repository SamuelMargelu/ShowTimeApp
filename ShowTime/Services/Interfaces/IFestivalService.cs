using ShowTime.Entities;

namespace ShowTime.Services.Interfaces
{
    public interface IFestivalService : IService<Festival>
    {
        Task AddBandtoFestival(int bandId, int festivalId);
    }
}
