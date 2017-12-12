using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmployeePortal.LMS.Common.DisplayModel
{
    public class TimeSheetModel
    {
        public string Name { get; set; }
        public Int64 userID { get; set; }
        public string Shift { get; set; }
        public DateTime WorkingDate { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public TimeSpan WorkingHours { get; set; }
        public string Status { get; set; }
    }
}
