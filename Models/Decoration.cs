using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByuEgyptSite.Models
{
    /// <summary>
    /// Model for textile decorations
    /// </summary>
    [Table("decoration")]
    public class Decoration
    {
        [Required]
        [Key]
        public long id { get; set; }
        public int? decorationid { get; set; }
        public string? value { get; set; }

        public ICollection<DecorationTextile> decorationTextiles { get; set; } // assocation class to textiles
    }
}
