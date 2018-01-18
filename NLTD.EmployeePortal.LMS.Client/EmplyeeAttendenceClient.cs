using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLTD.EmploeePortal.LMS.Dac.Dac;
using NLTD.EmploeePortal.LMS.Dac.DbHelper;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Repository;

namespace NLTD.EmployeePortal.LMS.Client
{
    public class EmplyeeAttendenceClient : IEmployeeAttendanceHelper
    {
        private IEmployeeAttendanceHelper employeeAttendanceHelper;

        public EmplyeeAttendenceClient()
        {
            employeeAttendanceHelper = new EmployeeAttendanceHelper();
        }
        public void Dispose()
        {
          
        }
        public List<EmployeeAttendanceModel> GetAttendence(Int64 EmployeeID)
        {
            return employeeAttendanceHelper.GetAttendence(EmployeeID);
        }

        public List<EmployeeAttendanceModel> GetAttendenceForRange(Int64 EmployeeID, DateTime FromDateTime, DateTime ToDateTime,string requestLevelPerson,bool IsDirectEmployees)
        {
            return employeeAttendanceHelper.GetAttendenceForRange(EmployeeID, FromDateTime,ToDateTime, requestLevelPerson, IsDirectEmployees);
        }
    }
}
