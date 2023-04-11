using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByuEgyptSite.Models
{
    [Table("analysis_textile")]
    public class AnalysisTextile
    {
        public long AnalysisId { get; set; }
        public long TextileId { get; set; }

        public Analysis? Analysis { get; set; }
        public Textile? Textile { get; set; }
    }
}
