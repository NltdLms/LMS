using NLTD.EmploeePortal.LMS.Dac.DbHelper;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmployeePortal.LMS.Client
{
    public class EmployeeLeaveBalanceClient : IEmployeeLeaveBalanceHelper
    {
        public void Dispose()
        {
            //Nothing to implement...
        }

        public IList<LeaveBalanceEmpProfile> GetLeaveBalanceEmpProfile(string name)
        {
            using (IEmployeeLeaveBalanceHelper helper = new EmployeeLeaveBalanceHelper())
            {
                return helper.GetLeaveBalanceEmpProfile(name);
            }
        }

        public string UpdateLeaveBalance(List<EmployeeLeaveBalanceDetails> empLeaveBalanceDetails, Int64 UserId, Int64 LoginUserId)
        {
            using (IEmployeeLeaveBalanceHelper helper = new EmployeeLeaveBalanceHelper())
            {
                return helper.UpdateLeaveBalance(empLeaveBalanceDetails, UserId, LoginUserId);
            }
        }
    }
}
