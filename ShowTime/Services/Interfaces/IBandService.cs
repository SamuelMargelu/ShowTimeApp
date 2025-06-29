using ShowTime.Entities;

namespace ShowTime.Services.Interfaces
{
    public interface IBandService : IService<Band>
    {
        Task<IEnumerable<Band>> GetBandsWithFestivalsAsync();
    }
}
