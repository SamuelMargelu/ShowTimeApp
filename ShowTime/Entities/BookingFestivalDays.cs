namespace ShowTime.Entities
{
    public class BookingFestivalDays
    {
        public int BookingId { get; set; }
        public Booking? Booking { get; set; }
        public int FestivalDayId { get; set; }
        public FestivalDay? FestivalDay { get; set; }
    }
}
