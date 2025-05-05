using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class viewstaffattendanceviewmodel
    {
        public string Staff_Name { get; set; }

        public int Staff_Id { get; set; }

        public float Total { get; set; }

        public string MarkPresent { get; set; }
    }
}