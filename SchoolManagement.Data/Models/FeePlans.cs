using SchoolManagement.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public class FeePlans:BaseEntity
    {
        [Key]
        public int FeePlanId { get; set; }
        public string FeePlanName { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int FeeId { get; set; }
        public string FeeName { get; set; }
       
        public string Medium { get; set; }
        
        public float FeeValue { get; set; }
        public string Opt1 { get; set; }
        public string Opt2 { get; set; }
        public string Opt3 { get; set; }
        public string Opt4 { get; set; }
        public int FeeType_Id { get; set; }
        public int TransportOpt_Id { get; set; }
        public int? KmDistance_Id { get; set; }
        public string Amount { get; set; }


        [DefaultValue(0)]
        public byte Jan { get; set; }
        [DefaultValue(0)]
        public byte Feb { get; set; }
        [DefaultValue(0)]
        public byte Mar { get; set; }
        [DefaultValue(0)]
        public byte Apr { get; set; }
        [DefaultValue(0)]
        public byte May { get; set; }
        [DefaultValue(0)]
        public byte Jun { get; set; }
        [DefaultValue(0)]
        public byte Jul { get; set; }
        [DefaultValue(0)]
        public byte Aug { get; set; }
        [DefaultValue(0)]
        public byte Sep { get; set; }
        [DefaultValue(0)]
        public byte Oct { get; set; }
        [DefaultValue(0)]
        public byte Nov { get; set; }
        [DefaultValue(0)]
        public byte Dec { get; set; }

        public string Fee_configurationid { get; set; }
        public string FeeConfigurationname { get; set; }

        public int Batch_Id { get; set; }

        public string Batch_Name { get; set; }


    }
}
