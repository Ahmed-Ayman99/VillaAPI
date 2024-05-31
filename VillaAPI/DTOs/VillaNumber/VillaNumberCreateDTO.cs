namespace VillaAPI.DTOs.VillaNumber
{
    public class VillaNumberCreateDTO
    {
        public int NumberOfVilla { get; set; }
        public int Villa_ID { get; set; }
        public string SpecialDetails { get; set; }
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
    }
}
