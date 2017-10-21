using NLTD.EmployeePortal.LMS.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmploeePortal.LMS.Dac.DbHelper;
using NLTD.EmployeePortal.LMS.Common.QueryModel;

namespace NLTD.EmployeePortal.LMS.Client
{
    public class LeaveClient : ILeaveHelper
    {
        public string ChangeStatus(LeaveStatusModel status)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.ChangeStatus(status);
            }
        }
        //public string ChangeStatusFromEmail(Int64 leaveId, Int64 userId, string action)
        //{
        //    using (ILeaveHelper helper = new LeaveHelper())
        //    {
        //        return helper.ChangeStatusFromEmail(leaveId, userId, action);
        //    }
        //}
        public EmailDataModel ViewLeaveFromEmail(Int64 leaveId, Int64 userId)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.ViewLeaveFromEmail(leaveId, userId);
            }
        }
       
       
        public EmailDataModel GetEmailData(Int64 leaveId, string actionName)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetEmailData(leaveId, actionName);
            }
        }
        public void Dispose()
        {
            //Nothing to impement...
        }

        public IList<TeamLeaves> GetTeamLeaveHistory(ManageTeamLeavesQueryModel qryMdl)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetTeamLeaveHistory(qryMdl);
            }
        }
        public IList<TeamLeaves> GetLeaveRequests(ManageTeamLeavesQueryModel qryMdl)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetLeaveRequests(qryMdl);
            }
        }
        //public List<LeaveItem> GetMyLeaveRequests(long UserId)
        //{
        //    using (ILeaveHelper helper = new LeaveHelper())
        //    {
        //        return helper.GetMyLeaveRequests(UserId);
        //    }
        //}
        public List<LeaveTypesModel> GetLeaveTypes(long OfficeId, Int64 userId)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetLeaveTypes(OfficeId, userId);
            }
        }
        public List<DropDownItem> GetYearsFromLeaveBalance()
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetYearsFromLeaveBalance();
            }
        }
        public string SaveLeaveRequest(LeaveRequestModel request)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.SaveLeaveRequest(request);
            }
        }

        public IList<LeaveSummary> GetLeaveSumary(long UserId, Int32 summaryYear)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetLeaveSumary(UserId, summaryYear);
            }
        }
        public IList<HolidayModel> GetHolidays(long UserId, Int32 holYear)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetHolidays(UserId, holYear);
            }
        }
        public IList<EmployeeWiseLeaveSummaryModel> GetEmployeeWiseLeaveSumary(Int64 UserId, int Year, string reqUsr,string Name, bool OnlyReportedToMe)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetEmployeeWiseLeaveSumary(UserId, Year,  reqUsr, Name,OnlyReportedToMe);
            }
        }
        public int GetHolidayCount(DateTime startDate, DateTime endDate, long UserId)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetHolidayCount(startDate, endDate, UserId);
            }
        }
        public IList<DaywiseLeaveDtlModel> GetDaywiseLeaveDtl(DateTime? FromDate, DateTime? ToDate, bool IsLeaveOnly, Int64 LeadId, bool OnlyReportedToMe,string Name, string reqUsr, bool DonotShowRejected)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetDaywiseLeaveDtl(FromDate, ToDate, IsLeaveOnly, LeadId, OnlyReportedToMe,Name, reqUsr,DonotShowRejected);
            }
        }
        public IList<MonthwiseLeavesCountModel> GetMonthwiseLeavesCount(Int32 year, Int64 LeadId, bool OnlyReportedToMe, string Name, string reqUsr)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetMonthwiseLeavesCount(year, LeadId, OnlyReportedToMe, Name, reqUsr);
            }
        }
        public IList<LeaveDtl> GetLeaveDetailCalculation(DateTime LeaveFrom, DateTime LeaveUpto, string LeaveFromTime, string LeaveUptoTime, Int64 UserId, Int64 LeaveTypText)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetLeaveDetailCalculation(LeaveFrom, LeaveUpto, LeaveFromTime, LeaveUptoTime, UserId, LeaveTypText);
            }
        }
        public decimal ReturnDuration(DateTime LeaveFrom, DateTime LeaveUpto, string LeaveFromTime, string LeaveUptoTime, Int64 UserId)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.ReturnDuration(LeaveFrom, LeaveUpto, LeaveFromTime, LeaveUptoTime, UserId);
            }
        }
        public string ReturnWeekOff(long UserId)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.ReturnWeekOff(UserId);
            }
        }
        public IList<PermissionDetailsModel> GetPermissionDetail(string Name, string reqUsr, DateTime? startDate, DateTime? endDate, bool OnlyReportedToMe, Int64 LeadId)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetPermissionDetail(Name, reqUsr, startDate, endDate, OnlyReportedToMe, LeadId);
            }
        }
        public IList<DropDownItem> GetWeekOffs(long UserId)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetWeekOffs(UserId);
            }

        }
        public DashBoardModel GetDashboardData(Int64 UserId)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetDashboardData(UserId);
            }
        }
        public LeaveRequestModel ApplyLeaveCommonData(Int64 UserId, Int64 OfficeId)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.ApplyLeaveCommonData(UserId, OfficeId);
            }
        }
        public IList<LeaveDetailModel> ShowLeaveDetail(Int64 LeaveId)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.ShowLeaveDetail(LeaveId);
            }
        }
        public IList<EmployeeList> GetEmployeeList(string param, Int64 userId)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetEmployeeList(param, userId);
            }
        }
        public int GetPendingApprovalCount(Int64 userId)
        {
            using (ILeaveHelper helper = new LeaveHelper())
            {
                return helper.GetPendingApprovalCount(userId);
            }
        }
    }
}
