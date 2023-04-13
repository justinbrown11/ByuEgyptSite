using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByuEgyptSite.Models
{
    /// <summary>
    /// Model for burial textiles
    /// </summary>
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
        public ICollection<ColorTextile> colorTextiles { get; set; } // association class to colors
        public ICollection<DimensionTextile> dimensionTextiles { get; set; } // association class to dimensions
        public ICollection<BurialTextile> burialTextiles { get; set; } // association class to burials
        public ICollection<StructureTextile> structureTextiles { get; set; } // association class to structures
        public ICollection<TextileFunctionTextile> textileFunctionTextiles { get; set; } // association class to textile functions
        public ICollection<YarnManipulationTextile> yarnManipulationTextiles { get; set; } // association class to yarn manipulations
        public ICollection<DecorationTextile> decorationTextiles { get; set; } // association class to decorations
        public ICollection<AnalysisTextile> analysisTextiles { get; set; } // association class to analyses
        public ICollection<PhotoDataTextile> photoDataTextiles { get; set; } // association class to photo data
    }
}
