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
        public DateTime? sampledate { get; set; }
        public DateTime? photographeddate { get; set; }
        public string? direction { get; set; }
        public ICollection<ColorTextile> colorTextiles { get; set; }
        public ICollection<DimensionTextile> dimensionTextiles { get; set; }
        public ICollection<BurialTextile> burialTextiles { get; set; }
        public ICollection<StructureTextile> structureTextiles { get; set; }
        public ICollection<TextileFunctionTextile> textileFunctionTextiles { get; set; }
        public ICollection<YarnManipulationTextile> yarnManipulationTextiles { get; set; }
        public ICollection<DecorationTextile> decorationTextiles { get; set; }
        public ICollection<AnalysisTextile> analysisTextiles { get; set; }
        public ICollection<PhotoDataTextile> photoDataTextiles { get; set; }
    }
}
