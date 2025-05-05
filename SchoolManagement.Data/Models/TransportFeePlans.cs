using SchoolManagement.Entities;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public class TransportFeePlans : BaseEntity
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
        public float FeeValue { get; set; }
        public string Opt1 { get; set; }
        public string Opt2 { get; set; }
        public string Opt3 { get; set; }
        public string Opt4 { get; set; }
    }
}
