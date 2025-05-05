using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_RoomType
    {

        [Key]
        public int Room_Id { get; set; }

        public string Room_Type { get; set; }

    }
}