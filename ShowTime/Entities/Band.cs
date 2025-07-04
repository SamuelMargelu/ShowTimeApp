using ShowTime.Enum;

namespace ShowTime.Entities
{
    public class Band
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Genre Genre { get; set; }
        public ICollection<BandFestival> BandFestivals { get; set; } = [];
        public byte[]? Photo { get; set; }
    }
}
