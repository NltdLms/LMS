using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmployeePortal.LMS.Common.DisplayModel
{
    public class TimeSheetConsolidate
    {
        public string name { get; set; }
        public TimeSpan weeklyHours { get; set; }
        public TimeSpan monthlyHours { get; set; }
        public int weeklyPermissionCount { get; set; }
        public int monthlyPermissionCount { get; set; }
        public int weeklyLeaveCount { get; set; }
        public int monthlyLeaveCount { get; set; }
        public int weeklyLateCount { get; set; }
        public int monthlyLateCount { get; set; }
        public int weeklyEarlyCount { get; set; }
        public int monthlyEarlyCount { get; set; }

        public bool isWeeklyHoursUpdate { get; set; }
        public bool isMonthlyHoursUpdate { get; set; }

        public string dateRange { get; set; }
    }
}
