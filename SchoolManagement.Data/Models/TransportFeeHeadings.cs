using SchoolManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class TransportFeeHeadings : BaseEntity
    {
        [Key]
        public int TransportFeeId { get; set; }
        public string FeeName { get; set; }
        public int FeeFrequencyId { get; set; }
        public string FeeFrequencyName { get; set; }
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
        //public int FeeHeadGroupId { get; set; }
        //public string FeeHeadGroupName { get; set; }
        //public int AccountId { get; set; }
        //public string AccountName { get; set; }

        ////Foreign Keys
        //[ForeignKey("AccountId")]
        //public Accounts Account { get; set; }

        //[ForeignKey("FeeHeadGroupId")]
        //public FeeHeadingGroups FeeHeadingGroups { get; set; }

        //[ForeignKey("FeeFrequencyId")]
        //public Frequencys Frequencys { get; set; }
    }
}
