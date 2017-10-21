using NLTD.EmployeePortal.LMS.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmploeePortal.LMS.Dac.DbHelper;

namespace NLTD.EmployeePortal.LMS.Client
{
    public class EmployeeClient : IEmployeeHelper
    {
        public void Dispose()
        {
            //Nothing to dispose...
        }

        public long GetEmployeeId(string LogonId)
        {
            using (IEmployeeHelper helper = new EmployeeHelper())
            {
                return helper.GetEmployeeId(LogonId);
            }
        }
        public string ReportingToName(Int64 userId)
        {
            using (IEmployeeHelper helper = new EmployeeHelper())
            {
                return helper.ReportingToName(userId);
            }
        }
        public long GetUserId(string name)
        {
            using (IEmployeeHelper helper = new EmployeeHelper())
            {
                return helper.GetUserId(name);
            }
        }
        public EmployeeProfile GetEmployeeProfile(Int64 userId)
        {
            using (IEmployeeHelper helper = new EmployeeHelper())
            {
                return helper.GetEmployeeProfile(userId);
            }

        }
        public ViewEmployeeProfileModel ViewEmployeeProfile(Int64 userId)
        {
            using (IEmployeeHelper helper = new EmployeeHelper())
            {
                return helper.ViewEmployeeProfile(userId);
            }

        }
        public EmployeeProfile GetEmployeeLoginProfile(string LogonId)
        {
            using (IEmployeeHelper helper = new EmployeeHelper())
            {
                return helper.GetEmployeeLoginProfile(LogonId);
            }

        }
        public List<DropDownItem> GetReportToList(long OfficeId)
        {
            using (IEmployeeHelper helper = new EmployeeHelper())
            {
                return helper.GetReportToList(OfficeId);
            }
        }

        public string UpdateEmployeeProfile(EmployeeProfile profile, Int64 ModifiedBy)
        {
            using (IEmployeeHelper helper = new EmployeeHelper())
            {
                return helper.UpdateEmployeeProfile(profile, ModifiedBy);
            }
        }

        public IList<ViewEmployeeProfileModel> GetTeamProfiles(Int64 userId, bool onlyReportedToMe, string name, string requestMenuUser,bool hideInactiveEmp)
        {
            using (IEmployeeHelper helper = new EmployeeHelper())
            {
                return helper.GetTeamProfiles(userId, onlyReportedToMe, name,  requestMenuUser, hideInactiveEmp);
            }
        }


    }
}