using ShowTime.Entities;

namespace ShowTime.Repositories.Interfaces
{
    public interface IFestivalRepository : IRepository<Festival>
    {
        Task<IEnumerable<Festival>> GetFestivalsWithBandsAsync();
    }
}
