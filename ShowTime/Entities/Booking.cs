namespace ShowTime.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public int FestivalId { get; set; }
        public Festival? Festival { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
