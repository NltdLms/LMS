﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmployeePortal.LMS.Common.QueryModel
{
    public class LeaveStatusModel
    {
        public Int64 LeaveId { get; set; }
        public String Comment { get; set; }
        public String Status { get; set; }
        public Int64 UserId { get; set; }
    }
}
