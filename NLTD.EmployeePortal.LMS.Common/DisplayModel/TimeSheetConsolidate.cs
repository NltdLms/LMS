using System;

namespace NLTD.EmployeePortal.LMS.Common.DisplayModel
{
    public class TimeSheetConsolidate
    {
        public string name { get; set; }
        public TimeSpan hours { get; set; }
        
        public int permissionCount { get; set; }
        public decimal leaveCount { get; set; }
        public int lateCount { get; set; }
        public int earlyCount { get; set; }
        
       public DateTime fromDatetime { get; set; }
        public DateTime toDatetime { get; set; }
        public decimal WFHCount { get; set; }
    }
}
