using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Common.QueryModel;
using NLTD.EmployeePortal.LMS.Repository;
using NLTD.EmployeePortal.LMS.DbHelper;

namespace NLTD.EmployeePortal.LMS.Client
{
    public class TimesheetClient : ITimesheetHelper
    {
        ITimesheetHelper TimesheetHelper ;

        public TimesheetClient()
        {
            TimesheetHelper = new TimesheetHelper();
        }
        public List<TimeSheetModel> GetMyTimeSheet(Int64 UserID, DateTime FromDate, DateTime ToDate)
        {
            List<TimeSheetModel> timeSheetModelList=  TimesheetHelper.GetMyTimeSheet(UserID, FromDate, ToDate);
            return timeSheetModelList;
        }
        public List<TimeSheetModel> GetMyTeamTimeSheet(Int64 UserID, DateTime FromDate, DateTime ToDate)
        {
            List<TimeSheetModel> timeSheetModelList = TimesheetHelper.GetMyTeamTimeSheet(UserID, FromDate, ToDate);
            return timeSheetModelList;
        }
    }
}
