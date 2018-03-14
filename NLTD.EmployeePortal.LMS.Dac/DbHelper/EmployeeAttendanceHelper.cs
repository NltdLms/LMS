using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Dac.Dac;
using NLTD.EmployeePortal.LMS.Repository;
using System;
using System.Collections.Generic;

namespace NLTD.EmployeePortal.LMS.Dac.DbHelper
{
    public class EmployeeAttendanceHelper : IEmployeeAttendanceHelper
    {
        private IEmployeeAttendanceHelper employeeAttendanceHelper;

        public EmployeeAttendanceHelper()
        {
            employeeAttendanceHelper =new EmployeeAttendanceDac();
        }
        public void Dispose()
        {
          //  throw new NotImplementedException();
        }

        public List<EmployeeAttendanceModel> GetAttendence(Int64 UserID)
        {
            return employeeAttendanceHelper.GetAttendence(UserID);
        }


        public List<EmployeeAttendanceModel> GetAttendenceForRange(Int64 UserID, DateTime FromDateTime,DateTime ToDateTime,string requestLevelUser,bool IsDirectEmployees)
        {
            return employeeAttendanceHelper.GetAttendenceForRange(UserID, FromDateTime, ToDateTime, requestLevelUser, IsDirectEmployees);
        }

    }
}
