using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByuEgyptSite.Models
{
    /// <summary>
    /// Model for textile structures
    /// </summary>
    [Table("structure")]
    public class Structure
    {
        [Key]
        [Required]
        public long id { get; set; }
        public string? value { get; set; }
        public int? structureid { get; set; }
        public ICollection<StructureTextile> structureTextiles { get; set; } //  association class to textiles
    }
}
