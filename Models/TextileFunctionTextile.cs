using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByuEgyptSite.Models
{
    [Table("textilefunction_textile")]
    public class TextileFunctionTextile
    {
        public long TextileFunctionId { get; set; }
        public long TextileId { get; set; }

        public TextileFunction? TextileFunction { get; set; }
        public Textile? Textile { get; set; }
    }
}
