﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmployeePortal.LMS.Common.QueryModel
{
    public class TimeSheetQueryModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Int64 UserID { get; set; }
        public string ErrorMsg { get; set; }
        public string Name { get; set; }
        public string DateRange { get; set; }

        public bool MyDirectEmployees { get; set; }
    }
}
