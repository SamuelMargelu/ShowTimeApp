using Microsoft.AspNetCore.Identity;

namespace ShowTime.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
