using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models.Master
{
    public class DataListItemModel
    {
        public int DataListItemId { get; set; }
        public string DataListItemName { get; set; }
        public string DataListName { get; set; }
        public string CreatePermission { get; set; }

        public string Editpermission { get; set; }

        public string DeletePermission { get; set; }

        public string ViewPermission { get; set; }

        public string Status { get; set; }
    }
}