using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmployeePortal.LMS.Common.QueryModel
{
    public class ShiftQueryModel
    {
        public Int64 UserID { get; set; }
        public string Employeename { get; set; }
        public TimeSpan ShiftFromtime { get; set; }
        public TimeSpan ShiftTotime { get; set; }
        public DateTime ShiftDate { get; set; }
        
    }
}
