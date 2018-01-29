﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmployeePortal.LMS.Common.DisplayModel
{
    public class TimeSheetConsolidate
    {
        public string name { get; set; }
        public TimeSpan hours { get; set; }
        
        public int permissionCount { get; set; }
        public int leaveCount { get; set; }
        public int lateCount { get; set; }
        public int earlyCount { get; set; }
        
       public DateTime fromDatetime { get; set; }
        public DateTime toDatetime { get; set; }
        public int WFHCount { get; set; }
    }
}