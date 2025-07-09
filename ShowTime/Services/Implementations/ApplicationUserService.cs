using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;
using ShowTime.Services.Interfaces;

namespace ShowTime.Services.Implementations
{
    public class ApplicationUserService(IApplicationUserRepository applicationUserRepository)
        : Service<ApplicationUser>(applicationUserRepository), IApplicationUserService
    {
    }
}
