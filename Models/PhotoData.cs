using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByuEgyptSite.Models
{
    /// <summary>
    /// Model for textile photo data
    /// </summary>
    [Table("photodata")]
    public class PhotoData
    {
        [Required]
        [Key]
        public long id { get; set; }
        public string? description { get; set; }
        public string? filename { get; set; }
        public int? photodataid { get; set; }
        public DateTime? date { get; set; }
        public string? url { get; set; }
        public ICollection<PhotoDataTextile> photoDataTextiles { get; set; } // association class to textiles
    }
}
