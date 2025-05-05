using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Constants
{
    public class FrequencyEnum
    {
        public static string GetMessageOfFrequencyEnum(string name)
        {
            string message = string.Empty;
            switch (name)
            {
                case "Annual":
                message = "Select Any One Month";
                    break;

                case "Bi Monthly":
                    message = "Select Any Six Months";
                    break;
                case "Half Yearly":
                    message = "Select Any Two Months";
                    break;
                case "Monthly":
                    message = "Select All Months";
                    break;
                case "One Time":
                    message = "Select Any One Month";
                    break;
                case "Quarterly":
                    message = "Select Any Four Months";
                    break;
                case "Four Monthly":
                    message = "Select Any Three Months";
                    break;
                default:
                    break;
            }
            return message;
        }
    }
}
