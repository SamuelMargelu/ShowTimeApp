using ShowTime.Enum;
// TO DO Pentru poze sa se converteasca poza in biti si se salveze in baza de date si dupa pentru incarcare si afisare sa fie convertita din biti in poza
namespace ShowTime.Entities
{
    public class Band
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Genre Genre { get; set; }
        public ICollection<Festival> Festivals { get; set; } = [];
        public byte[]? Photo { get; set; }
    }
}
