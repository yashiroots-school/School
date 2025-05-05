using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_ArchieveChangeStaffAccounttype
    {
        [Key]
        public int ChangeAccounType_ID { get; set; }

        public int StafID { get; set; }

        public string EmpId { get; set; }

        public string Staf_Name { get; set; }

        public string Employee_Designation { get; set; }

        public int Employee_AccountId { get; set; }

        public string Employee_AccountName { get; set; }

        public int Category_Id { get; set; }

        public string Staff_CategoryName { get; set; }

        public string Employee_Code { get; set; }
    }
}