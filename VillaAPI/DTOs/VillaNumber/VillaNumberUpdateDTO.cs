namespace VillaAPI.DTOs.VillaNumber
{
    public class VillaNumberUpdateDTO
    {
        public int Id { get; set; }
        public int NumberOfVilla { get; set; }
        public string SpecialDetails { get; set; }
        public int Villa_ID { get; set; }
        public DateTime UpdateAt { get;private set; } = DateTime.UtcNow;

    }
}
