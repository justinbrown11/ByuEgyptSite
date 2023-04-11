using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByuEgyptSite.Models
{
    [Table("textilefunction")]
    public class TextileFunction
    {
        [Key]
        [Required]
        public long id { get; set; }
        public string? value { get; set; }
        public int? textilefunctionid { get; set; }
        public ICollection<TextileFunctionTextile> textileFunctionTextiles { get; set; }
    }
}
