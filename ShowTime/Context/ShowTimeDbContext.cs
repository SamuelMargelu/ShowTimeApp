using Microsoft.EntityFrameworkCore;
using ShowTime.Entities;

namespace ShowTime.Context
{
    public class ShowTimeDbContext : DbContext
    {
        public ShowTimeDbContext(DbContextOptions<ShowTimeDbContext> options) : base(options)
        {
        }
        public DbSet<Band> Bands { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<Festival> Festivals { get; set; } = null!;
    }
}
