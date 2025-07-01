using Microsoft.EntityFrameworkCore;
using ShowTime.Context;
using ShowTime.Entities;
using ShowTime.Repositories.Interfaces;

namespace ShowTime.Repositories.Implementations
{
    public class BandRepository(ShowTimeDbContext context) : Repository<Band>(context), IBandRepository
    {
        private readonly DbSet<Band> _bands = context.Bands;
    }
}
