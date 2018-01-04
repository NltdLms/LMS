using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Common.QueryModel;

namespace NLTD.EmployeePortal.LMS.Repository
{
    public interface ITimesheetHelper
    {
         List<TimeSheetModel> GetMyTimeSheet(Int64 UserID, DateTime FromDate, DateTime ToDate);
         List<TimeSheetModel> GetMyTeamTimeSheet(Int64 UserID, DateTime FromDate, DateTime ToDate);
    }
}
