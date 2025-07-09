namespace ShowTime.Entities
{
    public class FestivalDay
    {
        public int Id { get; set; }
        public int FestivalId { get; set; }
        public Festival? Festival { get; set; }
        public DateTime Date { get; set; }
        public ICollection<BookingFestivalDays>? BookingFestivalDays { get; set; } = new List<BookingFestivalDays>();
    }
}
