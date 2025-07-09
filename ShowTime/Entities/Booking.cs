namespace ShowTime.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public int FestivalId { get; set; }
        public Festival? Festival { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
