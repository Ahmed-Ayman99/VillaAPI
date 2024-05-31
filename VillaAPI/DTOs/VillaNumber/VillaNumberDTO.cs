using VillaAPI.DTOs.Villa;

namespace VillaAPI.DTOs.VillaNumber
{
    public class VillaNumberDTO
    {
        public int id { get; set; }
        public int NumberOfVilla { get; set; }
        public string SpecialDetails { get; set; }
        public int Villa_ID { get; set; }
        public DateTime CreatedAt { get; }
        public DateTime UpdateAt { get; }
        public VillaDTO Villa { get; set; }
    }
}
