using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByuEgyptSite.Models
{
    [Table ("yarnmanipulation_textile")]
    public class YarnManipulationTextile
    {
        public long YarnManipulationId { get; set; }
        public long TextileId { get; set; }

        public YarnManipulation? YarnManipulation { get; set; }
        public Textile? Textile { get; set; }
    }
}
