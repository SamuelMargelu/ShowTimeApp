using ShowTime.Entities;

namespace ShowTime.Repositories.Interfaces
{
    public interface IBandRepository : IRepository<Band>
    {
        Task<IEnumerable<Band>> GetBandsWithFestivalsAsync();
    }
}
