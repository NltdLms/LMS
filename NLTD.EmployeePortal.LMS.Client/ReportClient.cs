using NLTD.EmployeePortal.LMS.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmploeePortal.LMS.Dac.DbHelper;
using NLTD.EmployeePortal.LMS.Common.QueryModel;

namespace NLTD.EmployeePortal.LMS.Client
{
    public class ReportClient : IReportHelper
    {
        public List<LateAndEarltRpt> GetLateAndEarlyEmployees(DateTime FromDate, DateTime ToDate, Int64 UserId, bool OnlyReportedToMe)
        {
            using (IReportHelper helper = new ReportHelper())
            {
                return helper.GetLateAndEarlyEmployees(FromDate, ToDate, UserId, OnlyReportedToMe);
            }
        }

        public List<NoOfLateInMonth> GetLateReport(Int64 UserID, DateTime FromDate, DateTime ToDate, bool myDirectEmployees)
        {
            using (IReportHelper helper = new ReportHelper())
            {
                return helper.GetLateReport(UserID, FromDate, ToDate, myDirectEmployees);
            }
        }

            public void Dispose()
        {
            //Nothing to impement...
        }
    }
}
