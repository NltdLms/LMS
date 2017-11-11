using NLTD.EmploeePortal.LMS.Dac.Dac;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmploeePortal.LMS.Dac.DbHelper
{
    public class LeaveTransactionHistoryHelper : ILeaveTransactionHisotryHelper, IDisposable
    {
        public void Dispose()
        {
            //Nothing to implement...
        }
        public IList<LeaveTransactionDetail> GetTransactionLog(string Name, string RequestMenuUser, long userId)
        {
            using (var dac = new LeaveTransactionHistoryDac())
            {
                return dac.GetTransactionLog(Name, RequestMenuUser, userId);
            }
        }
    }
}
