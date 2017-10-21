﻿using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmployeePortal.LMS.Repository
{
    public interface IEmployeeHelper : IDisposable
    {
        EmployeeProfile GetEmployeeProfile(Int64 userId);
        string UpdateEmployeeProfile(EmployeeProfile profile, Int64 ModifiedBy);
        List<DropDownItem> GetReportToList(Int64 OfficeId);
        Int64 GetEmployeeId(String LogonId);
        EmployeeProfile GetEmployeeLoginProfile(string LogonId);
        ViewEmployeeProfileModel ViewEmployeeProfile(Int64 userId);
        long GetUserId(string name);

        string ReportingToName(Int64 userId);
        IList<ViewEmployeeProfileModel> GetTeamProfiles(Int64 userId, bool onlyReportedToMe, string name, string requestMenuUser, bool hideInactiveEmp);

    }
}
