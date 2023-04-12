using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByuEgyptSite.Models
{
    [Table("dimension_textile")]
    public class DimensionTextile
    {
        public long DimensionId { get; set; }
        public long TextileId { get; set; }

        public Dimension? Dimension { get; set; }
        public Textile? Textile { get; set; }
    }
}
