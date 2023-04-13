using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByuEgyptSite.Models
{
    /// <summary>
    /// Model for textile analyses
    /// </summary>
    [Table("analysis")]
    public class Analysis
    {
        [Required]
        [Key]
        public long id { get; set; }
        public int? analysistype { get; set; }
        public string? doneby { get; set; }
        public int? analysisid { get; set; }
        public DateTime date { get; set; }
        public ICollection<AnalysisTextile> analysisTextiles { get; set; } // Association class
    }
}
