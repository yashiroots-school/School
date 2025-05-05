using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class Tbl_Basicpaymasterviewmodel
    {
        public int BasicPay_MasterId { get; set; }

        public string Basicpay_Name { get; set; }

        public string Created_Date { get; set; }

        public string CreatePermission { get; set; }

        public string Editpermission { get; set; }

        public string DeletePermission { get; set; }

        public string ViewPermission { get; set; }

    }
}