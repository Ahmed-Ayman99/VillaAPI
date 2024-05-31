namespace VillaAPI.DTOs.Villa
{
    public class VillaUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string Amenity { get; set; }
        public IFormFile Image { get; set; }
        public DateTime UpdatedAt { get; } = DateTime.UtcNow;
    }

}
