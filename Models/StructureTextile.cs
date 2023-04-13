using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByuEgyptSite.Models
{
    /// <summary>
    /// Association class model for textile structures
    /// </summary>
    [Table("structure_textile")]
    public class StructureTextile
    {
        public long StructureId { get; set; }
        public long TextileId { get; set; }

        public Structure? Structure { get; set; }
        public Textile? Textile { get; set; }
    }
}
