using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class TransportFee
    {

    }
    public class TransportFeeConfigurationView
    {
        public string Class { get; set; }
        public string BatchName { get; set; }
        public int FromKM { get; set; }
        public int ToKM { get; set; }
        public int Amount { get; set; }       
        public int Class_Id { get; set; }
        public int Batch_Id { get; set; }
        public int Id { get;   set; }
    }
}