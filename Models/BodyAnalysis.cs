using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByuEgyptSite.Models
{
    [Table("bodyanalysischart")]
    public class BodyAnalysis
    {
        [Required]
        [Key]
        public long bodyanalysisid { get; set; }
        public string? burialmainid { get; set; }
        public int? squarenorthsouth { get; set; }
        public string? northsouth { get; set; }
        public int? squareeastwest { get; set; }
        public string? eastwest { get; set; }
        public string? area { get; set; }
        public int? burialnumber { get; set; }
        public string? dateofexamination { get; set; }
        public int? preservationindex { get; set; }
        public string? haircolor { get; set; }
        public string? observations { get; set; }
        public string? robust { get; set; }
        public string? supraorbitalridges { get; set; }
        public string? orbitedge { get; set; }
        public string? parietalbossing { get; set; }
        public string? gonion { get; set; }
        public string? nuchalcrest { get; set; }
        public string? zygomaticcrest { get; set; }
        public string? sphenooccipitalsynchrondrosis { get; set; }
        public string? lamboidsuture { get; set; }
        public string? squamossuture { get; set; }
        public string? toothattrition { get; set; }
        public string? tootheruption { get; set; }
        public string? tootheruptionageestimate { get; set; }
        public string? ventralarc { get; set; }
        public string? subpubicangle { get; set; }
        public string? sciaticnotch { get; set; }
        public string? pubicbone { get; set; }
        public string? preauricularsulcus { get; set; }
        public string? medial_ip_ramus{ get; set; }
        public string? dorsalpitting { get; set; }
        public string? femur { get; set; }
        public string? humerus { get; set; }
        public string? femurheaddiameter { get; set; }
        public string? humerusheaddiameter { get; set; }
        public string? femurlength { get; set; }
        public string? humeruslength { get; set; }
        public string? estimatestature { get; set; }
        public string? osteophytosis { get; set; }
        public string? caries_periodontal_disease { get; set; }
        public string? notes { get; set; }
        public Burial Burial { get; set; }
    }
}
