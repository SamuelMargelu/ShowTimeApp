using ShowTime.Entities;

namespace ShowTime.Repositories.Interfaces
{
    public interface IFestivalRepository : IRepository<Festival>
    {
        Task<Festival?> GetFestivalWithDetailsAsync(int id);
    }
}
