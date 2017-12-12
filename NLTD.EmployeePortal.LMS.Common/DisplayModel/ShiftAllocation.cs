using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NLTD.EmployeePortal.LMS.Common.DisplayModel
{
    public class ShiftAllocation
    {
        public string EmpId { get; set; }
        public string Name { get; set; }
        public Int64 UserId { get; set; }
        public Int64? ShiftID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ShiftDate { get; set; }
        
        public TimeSpan ToTime { get; set; }
        public string ShiftName { get; set; }
        public bool IsActive { get; set; }
        public Int64 ShiftMappingID { get; set; }
    }

    public class ShiftEmployees
    {
        public string EmpId { get; set; }
        public string Name { get; set; }
        public Int64 UserId { get; set; }
        public Int64 ShiftId { get; set; }
        public DateTime ShiftDate { get; set; }
        
        public Int64? ShiftMappingID { get; set; }
    }

    public class Shifts
    {
        public Int64 ShiftId { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public string ShiftName { get; set; }
    }

    public class AddShiftEmployee
    {
        public List<ShiftEmployees> ShiftEmployees { get; set; }
        public List<Shifts> Shifts { get; set; }
        public bool IsEdit { get; set; }
    }

    public class SaveShiftEmployee
    {
        public List<int> UserId { get; set; }
        public Int64 Shift { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
