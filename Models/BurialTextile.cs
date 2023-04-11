using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByuEgyptSite.Models
{
    [Table("burialmain_textile")]
    public class BurialTextile
    {
        public long BurialId { get; set; }
        public long TextileId { get; set; }

        public Burial? Burial { get; set; }
        public Textile? Textile { get; set; }
    }
}
