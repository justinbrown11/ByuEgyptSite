using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByuEgyptSite.Models
{
    /// <summary>
    /// Model for textile colors
    /// </summary>
    [Table("color")]
    public class Color
    {
        [Key]
        [Required]
        public long id { get; set; }
        public string? value { get; set; }
        public int? colorid { get; set; }
        public ICollection<ColorTextile> colorTextiles { get; set; } // association class to textiles
    }
}
