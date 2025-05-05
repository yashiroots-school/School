using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class ClassTimeTableEntry
    {
        public long Id { get; set; }
        public int ClassId { get; set; }
        public int StaffId { get; set; }  // "Mon", "Tue", etc.
        public string SubjectId { get; set; }  // Foreign key from Subject table
        public bool IsClassTeacher { get; set; }
        // Optional: Navigation properties
        // public virtual Class Class { get; set; }
        // public virtual Subject Subject { get; set; }
    }

    public class ClassTimeTableViewModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }

        public List<SubjectEntryPerDay> SubjectsPerDays { get; set; } = new List<SubjectEntryPerDay>();
    }
    public class MasterTimeTableViewModel
    {
        public List<StaffNames> Staff { get; set; } = new List<StaffNames>(); // e.g., "Mon", "Tue"
        public List<Subject> AllSubjects { get; set; } = new List<Subject>(); // From DB
        
        public int SectionId { get; set; }
        public int BatchId { get; set; }
        public List<ClassTimeTableViewModel> ClassTimeTables { get; set; } = new List<ClassTimeTableViewModel>();
    }

    public class ClassTimeTable
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }

        public List<SubjectEntryPerDay> SubjectsPerDays { get; set; } = new List<SubjectEntryPerDay>();
        public Dictionary<int, SubjectEntryPerDay> SubjectsPerDaysMap
        {
            get
            {
                return SubjectsPerDays?.ToDictionary(x => x.StaffId) ?? new Dictionary<int, SubjectEntryPerDay>();
            }
        }
    }
    public class SubjectEntryPerDay
    {
        public int StaffId { get; set; }
        public bool IsClassTeacher { get; set; }
        public List<string> SubjectIds { get; set; } = new List<string>();
    }
    public class Subject
    {
        public string SubjectId { get; set; }      // Can be int or string
        public string SubjectName { get; set; }
    }
    public class StaffNames
    {
        public int StaffId { get; set; }      // Can be int or string
        public string StaffName { get; set; }
        public bool IsClassTeacher { get; set; }
        public int StaffCategory { get; set; }
        public bool IsActive { get; set; }
    }
    public class TimeTableCell
    {
        public string SubjectName { get; set; }
        public string StaffName { get; set; }
        public int Load { get; set; }
    }

    public class TimeTableRow
    {
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public int PeriodNumber { get; set; }
        public Dictionary<int, TimeTableCell> DayWiseCells { get; set; } = new Dictionary<int, TimeTableCell>();
    }

}
