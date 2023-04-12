using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByuEgyptSite.Models
{
    [Table("structure_textile")]
    public class StructureTextile
    {
        public long StructureId { get; set; }
        public long TextileId { get; set; }

        public Structure? Structure { get; set; }
        public Textile? Textile { get; set; }
    }
}
