using System.ComponentModel.DataAnnotations.Schema;

namespace VillaAPI.Models
{
    public class Villa
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        public string Amenity { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // ONE to ONE
        public VillaNumber VillaNumber { get; set; }
    }
}
