using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmployeePortal.LMS.Repository
{
    public interface IEmployeeLeaveBalanceHelper : IDisposable
    {
        IList<LeaveBalanceEmpProfile> GetLeaveBalanceEmpProfile(string name);

        string UpdateLeaveBalance(List<EmployeeLeaveBalanceDetails> empLeaveBalanceDetails, Int64 UserId, Int64 LoginUserId);
    }
}
