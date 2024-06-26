﻿namespace VillaAPI.DTOs.Villa
{
    public class VillaCreateDTO
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        //public string ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string Amenity { get; set; }
        public DateTime CreateAt { get; } = DateTime.UtcNow;
    }
}
