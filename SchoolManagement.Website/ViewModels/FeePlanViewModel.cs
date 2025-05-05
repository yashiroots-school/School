using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class FeePlanViewModel
    {
        public int feeHeadingId { get; set; }
        public string feeHeadingName { get; set; }
        public float feeValue { get; set; }
        public int[] classesId { get; set; }
        public int[] categoryId { get; set; }
        public string BatchName { get; set; }
        public string Medium { get; set; }
        public int Transportopt_Id { get; set; }
        public int KmDistance_Id { get; set; }

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