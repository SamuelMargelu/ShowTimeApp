using ShowTime.Entities;

namespace ShowTime.Repositories.Interfaces
{
    public interface IFestivalDayRepository : IRepository<FestivalDay>
    {
        Task AddRangeAsync(IEnumerable<FestivalDay> festivalDays);
    }
}
