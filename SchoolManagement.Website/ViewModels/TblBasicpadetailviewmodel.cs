using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class TblBasicpadetailviewmodel
    {
        public int BasicAmount_Id { get; set; }

        public int SchoolCategory_Id { get; set; }

        public int BasicPay_Id { get; set; }

        public string Category_Name { get; set; }

        public string Basicpay_Name { get; set; }

        public float Basic_Amount { get; set; }

        public string CreatedDate { get; set; }

        public string CreatePermission { get; set; }

        public string Editpermission { get; set; }

        public string DeletePermission { get; set; }

        public string ViewPermission { get; set; }

    }
}