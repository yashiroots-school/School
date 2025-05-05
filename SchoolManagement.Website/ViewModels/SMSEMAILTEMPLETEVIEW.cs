using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class SMSEMAILTEMPLETEVIEW
    {

    }
    public class SMSEMAILTEMPLETESUMMARY
    {
        public long TempleteID { get; set; }
        public string NOTIFICATIONTYPE { get; set; }
        public string SUBJECT { get; set; }
    }
    public class studentlist
    {
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public string MailId { get; set; }
        public string ClassName { get; set; }
        public string StudentName { get; set; }
        public string BatchName { get; set; }
        
    }
    public class Userlist
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class AddSMSEmailTemplete
    {
        public string SMSEmailtype { get; set; }
        public string SMSSubject { get; set; }
        public string SMSText { get; set; }
        public string Eamilmessage { get; set; }
        public string Attachement { get; set; }
    }

    public class AddSMSEmailTempleteview
    {
        public string NOTIFICATIONTYPE { get; set; }
        public string SUBJECT { get; set; }
        public string SMS { get; set; }
        public string EMAIL { get; set; }
        public string ATTACHEDFILE { get; set; }
        public string ATTACHEDFILETYPE { get; set; }
        public string ATTACHEDFILENAME { get; set; }
        public DateTime CREATEDDATE { get; set; }
    }
}