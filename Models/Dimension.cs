using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByuEgyptSite.Models
{
    /// <summary>
    /// Model for textile dimensions
    /// </summary>
    [Table("dimension")]
    public class Dimension
    {
        [Key]
        [Required]
        public long id { get; set; }
        public string? dimensiontype { get; set; }
        public string? value { get; set; }
        public int? dimensionid { get; set; }
        public ICollection<DimensionTextile> dimensionTextiles { get; set; } // association class to textiles
    }
}
