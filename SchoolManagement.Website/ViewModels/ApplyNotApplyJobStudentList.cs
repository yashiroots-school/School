using System.Collections.Generic;

namespace SFIMAR.ViewModels
{
    public class ApplyNotApplyJobStudentList
    {
        public List<ApplyNotApplyJobStudent> ApplyNotApplyJobStudents { get; set; }
        public List<CompanyDetail1> CompanyDetails { get; set; }
    }

    public class ApplyNotApplyJobStudent
    {
        public string Scholarnumber { get; set; }
        public string CompanyCode { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Specialization { get; set; }
        public string Status { get; set; }
        public string ApplyDate { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CommunicationId { get; set; }

    }

    public class CompanyDetail1
    {
        public long CompanyId { get; set; }
        public string CompanyCode { get; set; }
    }

}