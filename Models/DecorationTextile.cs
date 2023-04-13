using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByuEgyptSite.Models
{
    /// <summary>
    /// Association class Model for textile decorations
    /// </summary>
    [Table("decoration_textile")]
    public class DecorationTextile
    {
        public long DecorationId { get; set; }
        public long TextileId { get; set; }

        public Decoration? Decoration { get; set; }
        public Textile? Textile { get; set; }
    }
}
