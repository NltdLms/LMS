using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Repository;
using NLTD.EmploeePortal.LMS.Dac;
using NLTD.EmploeePortal.LMS.Dac.Dac;

namespace NLTD.EmploeePortal.LMS.Dac.DbHelper
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
