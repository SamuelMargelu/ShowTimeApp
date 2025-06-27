namespace ShowTime.Entities
{
    public class Festival
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Band> Bands { get; set; } = [];
        public ICollection<Booking> Bookings { get; set; } = [];
    }
}
