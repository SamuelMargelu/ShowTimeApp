using Microsoft.EntityFrameworkCore;
using ShowTime.Context;
using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;

namespace ShowTime.Repositories.Implementations
{
    public class ApplicationUserRepository(ShowTimeDbContext context) : Repository<ApplicationUser>(context), IApplicationUserRepository
    {
        private readonly DbSet<ApplicationUser> _applicationUsers = context.ApplicationUsers;
    }
}
