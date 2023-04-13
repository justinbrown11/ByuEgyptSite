using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByuEgyptSite.Models
{
    /// <summary>
    /// Assocation class Model for textile colors
    /// </summary>
    [Table("color_textile")]
    public class ColorTextile
    {
        public long ColorId { get; set; }
        public long TextileId { get; set; }

        public Color? Color { get; set; }
        public Textile? Textile { get; set; }
    }
}
