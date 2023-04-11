using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByuEgyptSite.Models
{
    [Table("textile")]
    public class Textile
    {
        [Required]
        [Key]
        public long id { get; set; }
        public string? locale { get; set; }
        public int? textileid { get; set; }
        public string? description { get; set; }
        public string? burialnumber { get; set; }
        public string? estimatedperiod { get; set; }
        public DateOnly? sampledate { get; set; }
        public DateOnly? photographeddate { get; set; }
        public string? direction { get; set; }
    }
}
