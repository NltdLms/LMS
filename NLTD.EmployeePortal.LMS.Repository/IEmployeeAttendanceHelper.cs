using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using System;
using System.Collections.Generic;

namespace NLTD.EmployeePortal.LMS.Repository
{
    public interface IEmployeeAttendanceHelper : IDisposable
    {
        List<EmployeeAttendanceModel> GetAttendence(Int64 UserID);
        List<EmployeeAttendanceModel> GetAttendenceForRange(Int64 UserID, DateTime FromDateTime, DateTime ToDateTime,string requestLevelUser,bool IsDirectEmployees);
    }
}
