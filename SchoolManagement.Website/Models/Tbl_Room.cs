using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace SchoolManagement.Website.Models
{
    public class Tbl_Room
    {

        [Key]
        public int Room_Id { get; set; }

        public string Room_Name { get; set; }

        public string Room_No { get; set; }

        public string Room_Type { get; set; }

        public string Seating_Capacity { get; set; }

        public string Location { get; set; }

        public string Remarks { get; set; }

        public int RoomType_ID { get; set; }

    }
}