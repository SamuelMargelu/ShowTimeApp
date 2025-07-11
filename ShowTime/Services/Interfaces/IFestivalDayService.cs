using ShowTime.Entities;

namespace ShowTime.Services.Interfaces
{
    public interface IFestivalDayService : IService<FestivalDay>
    {
        Task AddRangeAsync(IEnumerable<FestivalDay> festivalDays);
    }
}
