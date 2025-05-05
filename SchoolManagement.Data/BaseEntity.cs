using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Entities
{
    public class BaseEntity
    {
        //public Int32 Id { get; set; }
        [DisplayName("Added Date")]
        public DateTime AddedDate { get; set; }
        [DisplayName("Modified Date")]
        public DateTime? ModifiedDate { get; set; }
        public int CurrentYear { get; set; }
        public string IP { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public int CreateBy { get; set; }
        public string InsertBy { get; set; } = string.Empty;
        public string BatchName { get; set; }=string.Empty;
        public BaseEntity()
        {
            AddedDate = DateTime.Now.Date;
            ModifiedDate = DateTime.Now.Date;
            string hostName = Dns.GetHostName();
#pragma warning disable CS0618 // Type or member is obsolete
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
#pragma warning restore CS0618 // Type or member is obsolete
            IP = myIP;
            CurrentYear = DateTime.Now.Year;
        }
    }
}
