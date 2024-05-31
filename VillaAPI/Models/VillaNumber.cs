using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VillaAPI.Models
{
    public class VillaNumber
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int NumberOfVilla { get; set; }
        public string SpecialDetails { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }

        [ForeignKey("Villa")]
        public int Villa_ID { get; set; }
        public Villa Villa { get; set; }
    }
}
