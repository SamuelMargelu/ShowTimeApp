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
        public DbSet<BandFestival> BandFestivals { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BandFestival>()
                .HasKey(bf => new { bf.BandsId, bf.FestivalsId });

            modelBuilder.Entity<BandFestival>()
                .HasOne(bf => bf.Band)
                .WithMany(b => b.BandFestivals)
                .HasForeignKey(bf => bf.BandsId);

            modelBuilder.Entity<BandFestival>()
                .HasOne(bf => bf.Festival)
                .WithMany(f => f.BandFestivals)
                .HasForeignKey(bf => bf.FestivalsId);
        }
    }
}
