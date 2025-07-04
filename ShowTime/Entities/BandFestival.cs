namespace ShowTime.Entities
{
    public class BandFestival
    {
        public int BandsId { get; set; }
        public int FestivalsId { get; set; }
        public Band? Band { get; set; }
        public Festival? Festival { get; set; }
        public int BandOrder { get; set; }
    }
}
