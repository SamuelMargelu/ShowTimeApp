using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;
using ShowTime.Services.Interfaces;

namespace ShowTime.Services.Implementations
{
    public class BandSerivce(IBandRepository bandRepository)
        : Service<Band>(bandRepository), IBandService
    {
    }
}
