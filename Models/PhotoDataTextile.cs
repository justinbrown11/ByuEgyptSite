using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByuEgyptSite.Models
{
    /// <summary>
    /// Association class model for textile photo data
    /// </summary>
    [Table("photodata_textile")]
    public class PhotoDataTextile
    {
        public long PhotoDataId { get; set; }
        public long TextileId { get; set; }

        public PhotoData? PhotoData { get; set; }
        public Textile? Textile { get; set; }
    }
}
