using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShowTime.Entities;

namespace ShowTime.Context
{
    public class ShowTimeDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ShowTimeDbContext(DbContextOptions<ShowTimeDbContext> options) : base(options)
        {
        }
        public DbSet<Band> Bands { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<Festival> Festivals { get; set; } = null!;
        public DbSet<BandFestival> BandFestivals { get; set; } = null!;
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
        public DbSet<FestivalDay> FestivalDays { get; set; } = null!;
        public DbSet<BookingFestivalDays> BookingFestivalDays { get; set; } = null!;

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

            modelBuilder.Entity<BookingFestivalDays>()
                .HasKey(bfd => new { bfd.BookingId, bfd.FestivalDayId });

            modelBuilder.Entity<BookingFestivalDays>()
                .HasOne(bfd => bfd.Booking)
                .WithMany(b => b.BookingFestivalDays)
                .HasForeignKey(bfd => bfd.BookingId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookingFestivalDays>()
                .HasOne(bfd => bfd.FestivalDay)
                .WithMany(fd => fd.BookingFestivalDays)
                .HasForeignKey(bfd => bfd.FestivalDayId)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);
        }
    }
}
