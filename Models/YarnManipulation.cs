using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByuEgyptSite.Models
{
    [Table("yarnmanipulation")]
    public class YarnManipulation
    {
        [Key]
        [Required]
        public long id { get; set; }
        public string? thickness { get; set; }
        public string? angle { get; set; }
        public string? manipulation { get; set; }
        public string? material { get; set; }
        public string? count { get; set; }
        public string? component { get; set; }
        public string? ply { get; set; }
        public int? yarnmanipulationid { get; set; }
        public string? direction { get; set; }
        public ICollection<YarnManipulationTextile> yarnManipulationTextiles { get; set; }
    }
}
