using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public class tbl_skillset
    {
        [Key]
        public long SkillsetId { get; set; }
        public string ScholarNumber { get; set; }
        public string Problemsolving { get; set; }
        public string Initiative { get; set; }
        public string Adaptabilitytochange { get; set; }
        public string Interpersonalskills { get; set; }
        public string Strategicthinking { get; set; }
        public string Timemanagement { get; set; }
        public string Communication { get; set; }
        public string Leadership { get; set; }
        public string Teamwork { get; set; }
        public string Dancing { get; set; }
        public string Singing { get; set; }
        public string Compering { get; set; }
        public string Creative { get; set; }
        public string Contentwriting { get; set; }
        public string CoralDraw { get; set; }
        public string Photoshop { get; set; }
        public string Drawing { get; set; }
        public string Spare1 { get; set; }
        public string Spare2 { get; set; }
        public string Spare3 { get; set; }

        [StringLength(20)]
        public string Addedon { get; set; }

        [StringLength(20)]
        public string Addedby { get; set; }

        [StringLength(20)]
        public string Updatedon { get; set; }

        [StringLength(20)]
        public string Updatedby { get; set; }
    }
}
