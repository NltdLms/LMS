using NLTD.EmployeePortal.LMS.Dac.DbHelper;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmployeePortal.LMS.Client
{
    public class LeaveTransactionHistoryClient:ILeaveTransactionHisotryHelper
    {
        public void Dispose()
        {
            //Nothing to implement...
        }

        public IList<LeaveTransactionDetail> GetTransactionLog(string Name, string RequestMenuUser, long Userid)
        {
            using (ILeaveTransactionHisotryHelper helper = new LeaveTransactionHistoryHelper())
            {
                return helper.GetTransactionLog(Name, RequestMenuUser, Userid);
            }
        }
    }
}
