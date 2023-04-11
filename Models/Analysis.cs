using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByuEgyptSite.Models
{
    [Table("analysis")]
    public class Analysis
    {
        [Required]
        [Key]
        public long MyProperty { get; set; }
        public int? analysistype { get; set; }
        public string? doneby { get; set; }
        public int? analysisid { get; set; }
        public DateTime date { get; set; }
        public ICollection<AnalysisTextile> analysisTextiles { get; set; }
    }
}
