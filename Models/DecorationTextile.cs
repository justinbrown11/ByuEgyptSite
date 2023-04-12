using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByuEgyptSite.Models
{
    [Table("decoration_textile")]
    public class DecorationTextile
    {
        public long DecorationId { get; set; }
        public long TextileId { get; set; }

        public Decoration? Decoration { get; set; }
        public Textile? Textile { get; set; }
    }
}
