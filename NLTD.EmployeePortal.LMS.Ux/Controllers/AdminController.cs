using NLTD.EmployeePortal.LMS.Client;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Common.QueryModel;
using NLTD.EmployeePortal.LMS.Ux.AppHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using NLTD.EmployeePortal.LMS.Repository;

namespace NLTD.EmployeePortal.LMS.Ux.Controllers
{
    public class AdminController : BaseController
    {
        public ActionResult Employee()
        {
            return View();
        }
        public ActionResult PublicHolidays()
        {
            return View();
        }
        public ActionResult MapTeam()
        {
            return View();
        }
        public ActionResult MyYearWiseLeaveSumary()
        {
            using (var Client = new LeaveClient())
            {
                var result = Client.GetYearsFromLeaveBalance();
                ViewBag.YearsInLeaveBal = result;
            }
            ViewBag.RequestLevelPerson = "My";
            YearwiseLeaveSummaryQueryModel qryMdl = new YearwiseLeaveSummaryQueryModel();
            qryMdl.OnlyReportedToMe = true;
            return View("ViewEmployeeWiseLeaveSummary", qryMdl);
        }
        public ActionResult TeamYearWiseLeaveSumary()
        {
            using (var Client = new LeaveClient())
            {
                var result = Client.GetYearsFromLeaveBalance();
                ViewBag.YearsInLeaveBal = result;
            }
            ViewBag.RequestLevelPerson = "Team";
            YearwiseLeaveSummaryQueryModel qryMdl = new YearwiseLeaveSummaryQueryModel();
            qryMdl.OnlyReportedToMe = true;
            return View("ViewEmployeeWiseLeaveSummary", qryMdl);
        }
        public ActionResult AdminYearWiseLeaveSumary()
        {
            using (var Client = new LeaveClient())
            {
                var result = Client.GetYearsFromLeaveBalance();
                ViewBag.YearsInLeaveBal = result;
            }
            ViewBag.RequestLevelPerson = "Admin";
            YearwiseLeaveSummaryQueryModel qryMdl = new YearwiseLeaveSummaryQueryModel();
            qryMdl.OnlyReportedToMe = true;
            return View("ViewEmployeeWiseLeaveSummary", qryMdl);
        }
        public ActionResult loadYearwiseLeaveSummary(int Year, string reqUsr, string Name, bool OnlyReportedToMe)
        {
            IList<EmployeeWiseLeaveSummaryModel> LeaveRequests = null;
            if (Name != "")
            {
                Name = Name.Replace("|", " ");
            }
            using (var client = new LeaveClient())
            {
                LeaveRequests = client.GetEmployeeWiseLeaveSumary(UserId, Year, reqUsr, Name, OnlyReportedToMe);
            }
            return PartialView("YearwiseSummaryPartial", LeaveRequests);
        }
        public ActionResult ExportYearSummaryToExcel(YearwiseLeaveSummaryQueryModel data, string RequestLevelPerson)
        {
            IList<EmployeeWiseLeaveSummaryModel> LeaveRequests = null;
            using (var client = new LeaveClient())
            {
                LeaveRequests = client.GetEmployeeWiseLeaveSumary(UserId, data.Year, RequestLevelPerson, data.Name, data.OnlyReportedToMe);
            }
            List<EmployeeWiseLeaveSummaryModel> excelData = new List<EmployeeWiseLeaveSummaryModel>();
            excelData = LeaveRequests.ToList();
            if (excelData.Count > 0)
            {
                string[] columns = { "EmpID", "Name", "LeaveType", "TotalLeaves", "UsedLeaves", "PendingApproval", "BalanceLeaves" };
                byte[] filecontent = ExcelExportHelper.ExportExcelYearSummary(excelData, "", false, columns);
                return File(filecontent, ExcelExportHelper.ExcelContentType, "YearwiseSummaryLeaveReport_" + System.DateTime.Now + ".xlsx");
            }
            else
            {
                using (var Client = new LeaveClient())
                {
                    var result = Client.GetYearsFromLeaveBalance();
                    ViewBag.YearsInLeaveBal = result;
                }
                ViewBag.RequestLevelPerson = RequestLevelPerson;
                data.ErrorMsg = "Excel file is not generated as no data returned.";
                return View("ViewEmployeeWiseLeaveSummary", data);
            }
        }
        public ActionResult MyMonthWiseLeaves()
        {
            using (var Client = new LeaveClient())
            {
                var result = Client.GetYearsFromLeaveBalance();
                ViewBag.YearsInLeaveBal = result;
            }
            ViewBag.RequestLevelPerson = "My";

            YearwiseLeaveSummaryQueryModel qryMdl = new YearwiseLeaveSummaryQueryModel();
            qryMdl.OnlyReportedToMe = true;
            return View("MonthwiseLeaveCount", qryMdl);
        }
        public ActionResult TeamMonthWiseLeaves()
        {
            using (var Client = new LeaveClient())
            {
                var result = Client.GetYearsFromLeaveBalance();
                ViewBag.YearsInLeaveBal = result;
            }
            ViewBag.RequestLevelPerson = "Team";

            YearwiseLeaveSummaryQueryModel qryMdl = new YearwiseLeaveSummaryQueryModel();
            qryMdl.OnlyReportedToMe = true;
            return View("MonthwiseLeaveCount", qryMdl);
        }
        public ActionResult AdminMonthWiseLeaves()
        {
            using (var Client = new LeaveClient())
            {
                var result = Client.GetYearsFromLeaveBalance();
                ViewBag.YearsInLeaveBal = result;
            }
            ViewBag.RequestLevelPerson = "Admin";

            YearwiseLeaveSummaryQueryModel qryMdl = new YearwiseLeaveSummaryQueryModel();
            qryMdl.OnlyReportedToMe = true;
            return View("MonthwiseLeaveCount", qryMdl);
        }
        public ActionResult LoadMonthWiseLeaveCount(int Year, string reqUsr, string Name, bool OnlyReportedToMe)
        {
            IList<MonthwiseLeavesCountModel> LeaveRequests = null;
            if (Name != "")
            {
                Name = Name.Replace("|", " ");
            }
            using (var client = new LeaveClient())
            {
                LeaveRequests = client.GetMonthwiseLeavesCount(Year, UserId, OnlyReportedToMe, Name, reqUsr);
            }
            return PartialView("MonthwiseLeaveCountPartial", LeaveRequests);
        }

        public ActionResult ExportMonthCountToExcel(YearwiseLeaveSummaryQueryModel data, string RequestLevelPerson)
        {
            IList<MonthwiseLeavesCountModel> LeaveRequests = null;
            using (var client = new LeaveClient())
            {
                LeaveRequests = client.GetMonthwiseLeavesCount(data.Year, UserId, data.OnlyReportedToMe, data.Name, RequestLevelPerson);

            }
            List<MonthwiseLeavesCountModel> excelData = new List<MonthwiseLeavesCountModel>();
            excelData = LeaveRequests.ToList();
            if (excelData.Count > 0)
            {
                string[] columns = { "EmpId", "Name", "CL1", "PL1", "LWP1", "CO1", "CL2", "PL2", "LWP2", "CO2", "CL3", "PL3", "LWP3", "CO3", "CL4", "PL4", "LWP4", "CO4", "CL5", "PL5", "LWP5", "CO5", "CL6", "PL6", "LWP6", "CO6", "CL7", "PL7", "LWP7", "CO7", "CL8", "PL8", "LWP8", "CO8", "CL9", "PL9", "LWP9", "CO9", "CL10", "PL10", "LWP10", "CO10", "CL11", "PL11", "LWP11", "CO11", "CL12", "PL12", "LWP12", "CO12" };
                byte[] filecontent = ExcelExportHelper.ExportExcelMonthSummary(excelData, "", false, columns);
                return File(filecontent, ExcelExportHelper.ExcelContentType, "MonthwiseSummaryLeaveReport_" + System.DateTime.Now + ".xlsx");
            }
            else
            {
                using (var Client = new LeaveClient())
                {
                    var result = Client.GetYearsFromLeaveBalance();
                    ViewBag.YearsInLeaveBal = result;
                }
                ViewBag.RequestLevelPerson = RequestLevelPerson;
                data.ErrorMsg = "Excel file is not generated as no data returned.";
                return View("MonthwiseLeaveCount", data);
            }
        }

        public ActionResult MyDaywiseLeaves()
        {
            DaywiseLeaveQueryModel mdl = new DaywiseLeaveQueryModel();
            ViewBag.RequestLevelPerson = "My";
            mdl.OnlyReportedToMe = true;
            mdl.DonotShowRejected = true;
            return View("DaywiseLeaveDateRangeView", mdl);
        }
        public ActionResult TeamDaywiseLeaves()
        {
            DaywiseLeaveQueryModel mdl = new DaywiseLeaveQueryModel();
            ViewBag.RequestLevelPerson = "Team";
            mdl.OnlyReportedToMe = true;
            mdl.DonotShowRejected = true;
            return View("DaywiseLeaveDateRangeView", mdl);
        }
        public ActionResult AdminDaywiseLeaves()
        {
            DaywiseLeaveQueryModel mdl = new DaywiseLeaveQueryModel();
            ViewBag.RequestLevelPerson = "Admin";
            mdl.OnlyReportedToMe = true;
            mdl.DonotShowRejected = true;
            return View("DaywiseLeaveDateRangeView", mdl);
        }
        public ActionResult loadDaywiseLeaves(string Name, string FromDate, string ToDate, bool IsLeaveOnly, bool OnlyReportedToMe, string reqUsr, bool DonotShowRejected)
        {
            DateTime? startDateFormatted = null;
            DateTime? endDateFormatted = null;
            if (Name != "")
            {
                Name = Name.Replace("|", " ");
            }
            if (FromDate != null)
            {
                if (FromDate.Trim() != "")
                {
                    startDateFormatted = DateTime.ParseExact(FromDate, "d-M-yyyy", CultureInfo.InvariantCulture);
                    endDateFormatted = DateTime.ParseExact(ToDate, "d-M-yyyy", CultureInfo.InvariantCulture);
                }
            }
            if (FromDate == "" || ToDate == "")
            {
                startDateFormatted = System.DateTime.Now.Date;
                endDateFormatted = System.DateTime.Now.Date;
            }
            IList<DaywiseLeaveDtlModel> LeaveRequests = null;
            using (var client = new LeaveClient())
            {
                LeaveRequests = client.GetDaywiseLeaveDtl(startDateFormatted, endDateFormatted, IsLeaveOnly, UserId, OnlyReportedToMe, Name, reqUsr, DonotShowRejected);
            }
            for (int i = 0; i < LeaveRequests.Count; i++)
            {
                if (LeaveRequests[i].LeaveReason != null)
                {
                    if (LeaveRequests[i].LeaveReason.Length > 12)
                    {
                        LeaveRequests[i].ReasonShort = LeaveRequests[i].LeaveReason.Substring(0, 12) + "...";
                    }
                    else
                    {
                        LeaveRequests[i].ReasonShort = LeaveRequests[i].LeaveReason;
                    }
                }
                if (LeaveRequests[i].ApproverComments != null)
                {
                    if (LeaveRequests[i].ApproverComments.Length > 12)
                    {
                        LeaveRequests[i].CommentsShort = LeaveRequests[i].ApproverComments.Substring(0, 12) + "...";
                    }
                    else
                    {
                        LeaveRequests[i].CommentsShort = LeaveRequests[i].ApproverComments;
                    }
                }

            }
            return PartialView("DaywiseLeaveDateRangeDtlPartial", LeaveRequests);
        }
        public ActionResult ExportToExcel(DaywiseLeaveQueryModel data, string RequestLevelPerson)
        {
            IList<DaywiseLeaveDtlModel> LeaveRequests = null;
            string startDate = data.DateRange.Substring(0, 10);
            string endDate = data.DateRange.Substring(12);
            DateTime? startDateFormatted = null;
            DateTime? endDateFormatted = null;
            if (startDate != "")
            {
                try
                {
                    startDateFormatted = DateTime.Parse(startDate, new CultureInfo("en-GB", true));
                    endDateFormatted = DateTime.Parse(endDate, new CultureInfo("en-GB", true));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            using (var client = new LeaveClient())
            {
                LeaveRequests = client.GetDaywiseLeaveDtl(startDateFormatted, endDateFormatted, data.IsLeaveOnly, UserId, data.OnlyReportedToMe, data.Name, RequestLevelPerson, data.DonotShowRejected);
            }
            List<DaywiseLeaveDtlModel> excelData = new List<DaywiseLeaveDtlModel>();
            excelData = LeaveRequests.ToList();
            if (excelData.Count > 0)
            {
                string[] columns = { "EmpId", "Name", "LeaveType", "LeaveBalance", "LeaveDate", "PartOfDay", "Duration", "LeaveStatus", "LeaveReason", "ApproverComments" };
                byte[] filecontent = ExcelExportHelper.ExportExcel(excelData, "", false, columns);
                return File(filecontent, ExcelExportHelper.ExcelContentType, "DaywiseLeaveReport_" + System.DateTime.Now + ".xlsx");
            }
            else
            {
                ViewBag.RequestLevelPerson = RequestLevelPerson;
                data.ErrorMsg = "Excel file is not generated as no data returned.";
                return View("DaywiseLeaveDateRangeView", data);
            }
        }
        public ActionResult MyPermissions()
        {
            PermissionQueryModel mdl = new PermissionQueryModel();
            using (var Client = new LeaveClient())
            {
                var result = Client.GetYearsFromLeaveBalance();
                ViewBag.YearsInLeaveBal = result;
            }
            ViewBag.RequestLevelPerson = "My";
            mdl.OnlyReportedToMe = true;
            return View("DatewisePermissions", mdl);
        }
        public ActionResult TeamPermissions()
        {
            PermissionQueryModel mdl = new PermissionQueryModel();
            using (var Client = new LeaveClient())
            {
                var result = Client.GetYearsFromLeaveBalance();
                ViewBag.YearsInLeaveBal = result;
            }
            ViewBag.RequestLevelPerson = "Team";
            mdl.OnlyReportedToMe = true;
            return View("DatewisePermissions", mdl);
        }
        public ActionResult AdminPermissions()
        {
            PermissionQueryModel mdl = new PermissionQueryModel();
            using (var Client = new LeaveClient())
            {
                var result = Client.GetYearsFromLeaveBalance();
                ViewBag.YearsInLeaveBal = result;
            }
            ViewBag.RequestLevelPerson = "Admin";
            mdl.OnlyReportedToMe = true;
            return View("DatewisePermissions", mdl);
        }
        public ActionResult GetPermissionDetail(string Name, string reqUsr, string startDate, string endDate, bool OnlyReportedToMe)
        {
            IList<PermissionDetailsModel> LeaveRequests = null;
            DateTime? startDateFormatted = null;
            DateTime? endDateFormatted = null;
            if (Name != "")
            {
                Name = Name.Replace("|", " ");
            }
            if (startDate != "")
            {
                try
                {
                    startDateFormatted = DateTime.Parse(startDate, new CultureInfo("en-GB", true));
                    endDateFormatted = DateTime.Parse(endDate, new CultureInfo("en-GB", true));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (startDate == "" || endDate == "")
            {
                startDateFormatted = System.DateTime.Now.Date;
                endDateFormatted = System.DateTime.Now.Date;
            }
            using (var client = new LeaveClient())
            {
                LeaveRequests = client.GetPermissionDetail(Name, reqUsr, startDateFormatted, endDateFormatted, OnlyReportedToMe, UserId);
            }
            for (int i = 0; i < LeaveRequests.Count; i++)
            {
                if (LeaveRequests[i].Reason != null)
                {
                    if (LeaveRequests[i].Reason.Length > 12)
                    {
                        LeaveRequests[i].ReasonShort = LeaveRequests[i].Reason.Substring(0, 12) + "...";
                    }
                    else
                    {
                        LeaveRequests[i].ReasonShort = LeaveRequests[i].Reason;
                    }
                }
                if (LeaveRequests[i].ApproverComments != null)
                {
                    if (LeaveRequests[i].ApproverComments.Length > 12)
                    {
                        LeaveRequests[i].CommentsShort = LeaveRequests[i].ApproverComments.Substring(0, 12) + "...";
                    }
                    else
                    {
                        LeaveRequests[i].CommentsShort = LeaveRequests[i].ApproverComments;
                    }
                }

            }
            return PartialView("ViewPermissionDetailPartial", LeaveRequests);
        }



        public ActionResult ExportPermissionsExcel(PermissionQueryModel qryMdl, string RequestLevelPerson)
        {
            IList<PermissionDetailsModel> LeaveRequests = null;

            DateTime? startDateFormatted = null;
            DateTime? endDateFormatted = null;
            string startDate = qryMdl.DateRange.Substring(0, 10);
            string endDate = qryMdl.DateRange.Substring(12);
            if (startDate != "")
            {
                try
                {
                    startDateFormatted = DateTime.Parse(startDate, new CultureInfo("en-GB", true));
                    endDateFormatted = DateTime.Parse(endDate, new CultureInfo("en-GB", true));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            using (var client = new LeaveClient())
            {
                LeaveRequests = client.GetPermissionDetail(qryMdl.Name, RequestLevelPerson, startDateFormatted, endDateFormatted, qryMdl.OnlyReportedToMe, UserId);
            }
            List<PermissionDetailsModel> excelData = new List<PermissionDetailsModel>();
            excelData = LeaveRequests.ToList();
            if (excelData.Count > 0)
            {
                string[] columns = { "EmpId", "Name", "Month", "PermissionType", "PermissionDate", "TimeFrom", "TimeTo", "Duration", "Status", "Reason", "ApproverComments" };
                byte[] filecontent = ExcelExportHelper.ExportPermissionsExcel(excelData, "", false, columns);
                return File(filecontent, ExcelExportHelper.ExcelContentType, "PermissionsReport_" + System.DateTime.Now + ".xlsx");
            }
            else
            {
                ViewBag.RequestLevelPerson = RequestLevelPerson;
                qryMdl.ErrorMsg = "Excel file is not generated as no data returned.";
                return View("DatewisePermissions", qryMdl);
            }
        }
        public ActionResult GetEmployeeList(string term)
        {
            IList<EmployeeList> mdl;
            using (var Client = new LeaveClient())
            {
                mdl = Client.GetEmployeeList(term, UserId);

            }
            return Json(mdl);
        }
        public ActionResult ViewTransactionLog()
        {
            ViewBag.RequestLevelPerson = "Admin";
            ManageTeamLeavesQueryModel qyMdl = new ManageTeamLeavesQueryModel();
            qyMdl.OnlyReportedToMe = true;
            //int year = DateTime.Now.Year;
            //DateTime firstDay = new DateTime(year, 1, 1);
            //DateTime lastDay = new DateTime(year, 12, 31);
            //qyMdl.FromDate = firstDay;
            //qyMdl.ToDate = lastDay;
            //return View("TeamHistory", qyMdl);
            return View("SearchTransactionLog", qyMdl);
        }

        public ActionResult MyTransactionLogs()
        {
            ViewBag.RequestLevelPerson = "My";
            ManageTeamLeavesQueryModel qyMdl = new ManageTeamLeavesQueryModel();
            qyMdl.OnlyReportedToMe = true;
            return View("SearchTransactionLog", qyMdl);
        }

        public ActionResult TeamTransactionLogs()
        {
            ViewBag.RequestLevelPerson = "Team";
            ManageTeamLeavesQueryModel qyMdl = new ManageTeamLeavesQueryModel();
            qyMdl.OnlyReportedToMe = true;
            return View("SearchTransactionLog", qyMdl);
        }

        public ActionResult GetTransactionLog(string OnlyReportedToMe, string Name, string RequestMenuUser)
        {
            IList<LeaveTransactionDetail> transactionHistory = null;

            if (Name != "")
            {
                Name = Name.Replace("|", " ");
            }

            using (var client = new LeaveTransactionHistoryClient())
            {
                long Userid = this.UserId;
                transactionHistory = client.GetTransactionLog(Name, RequestMenuUser, Userid);
            }

            return PartialView("ViewTransactionLogPartial", transactionHistory);
        }


        public ActionResult loadEmployeeAttendence(string Name, string FromDate, string ToDate,string requestLevelPerson)
        {
            string errorMessage = string.Empty;
            IList<EmployeeAttendanceModel> employeeAttendanceModelList = GetEmployeeAttendenceList(Name,FromDate, ToDate, requestLevelPerson);
            ViewBag.RequestLevelPerson = requestLevelPerson;
            return PartialView("EmployeeAttendenceDtlPartial", employeeAttendanceModelList);
        }
        public ActionResult ExportAttendenceToExcel(EmployeeAttendenceQueryModel EmployeeAttendenceQueryModelObj, string RequestLevelPerson)
        {
            IEmployeeAttendanceHelper employeeAttendanceHelper = new EmplyeeAttendenceClient();
            IList<EmployeeAttendanceModel> employeeAttendanceModelList = new List<EmployeeAttendanceModel>();
            string name = "";

            if (EmployeeAttendenceQueryModelObj.Name == null && EmployeeAttendenceQueryModelObj.FromDate == null)
            {
                EmployeeProfile profile = (EmployeeProfile)Session["Profile"];
                name = profile.FirstName + " " + profile.LastName;
                employeeAttendanceModelList = employeeAttendanceHelper.GetAttendence(profile.UserId);
            }
            else if (EmployeeAttendenceQueryModelObj.Name != null && EmployeeAttendenceQueryModelObj.FromDate == null)
            {
                IEmployeeHelper EmployeeHelper = new EmployeeClient();
                name = EmployeeAttendenceQueryModelObj.Name;
                Int64 UserID = EmployeeHelper.GetUserId(EmployeeAttendenceQueryModelObj.Name);
                employeeAttendanceModelList = employeeAttendanceHelper.GetAttendence(UserID);
            }
            else if (EmployeeAttendenceQueryModelObj.Name != null && EmployeeAttendenceQueryModelObj.FromDate != null)
            {
                IEmployeeHelper EmployeeHelper = new EmployeeClient();
                Int64 UserID = EmployeeHelper.GetUserId(EmployeeAttendenceQueryModelObj.Name);
                name = EmployeeAttendenceQueryModelObj.Name;
                EmployeeAttendenceQueryModelObj.ToDate = ChangeTime(Convert.ToDateTime(EmployeeAttendenceQueryModelObj.ToDate), 23, 59, 59, 0);
                employeeAttendanceModelList = employeeAttendanceHelper.GetAttendenceForRange(UserID, Convert.ToDateTime(EmployeeAttendenceQueryModelObj.FromDate), Convert.ToDateTime(EmployeeAttendenceQueryModelObj.ToDate),RequestLevelPerson);
            }
            else if (EmployeeAttendenceQueryModelObj.Name == null && EmployeeAttendenceQueryModelObj.FromDate != null)
            {
                EmployeeProfile profile = (EmployeeProfile)Session["Profile"];
                name = profile.FirstName + " " + profile.LastName;
                EmployeeAttendenceQueryModelObj.ToDate = ChangeTime(Convert.ToDateTime(EmployeeAttendenceQueryModelObj.ToDate), 23, 59, 59, 0);
                employeeAttendanceModelList = employeeAttendanceHelper.GetAttendenceForRange(profile.UserId, Convert.ToDateTime(EmployeeAttendenceQueryModelObj.FromDate), Convert.ToDateTime(EmployeeAttendenceQueryModelObj.ToDate),RequestLevelPerson);
            }
            List<EmployeeAttendanceModel> excelData = employeeAttendanceModelList.ToList();

            if (excelData.Count > 0)
            {
                string[] columns = { "AttendenceDate", "INOutTime", "InOut" };
                byte[] filecontent = ExcelExportHelper.ExportExcelAttendence(excelData, "", false, columns);
                return File(filecontent, ExcelExportHelper.ExcelContentType, name + "_" + System.DateTime.Now + ".xlsx");
            }
            else
            {
                using (var Client = new LeaveClient())
                {
                    var result = Client.GetYearsFromLeaveBalance();
                    ViewBag.YearsInLeaveBal = result;
                }
                ViewBag.RequestLevelPerson = RequestLevelPerson;
                EmployeeAttendenceQueryModel data = new EmployeeAttendenceQueryModel();
                data.ErrorMsg = "Excel file is not generated as no data returned.";
                return View("~/Views/Attendence/MyAttendance.cshtml", data);
            }
        }

        private IList<EmployeeAttendanceModel> GetEmployeeAttendenceList(string Name,string FromDate, string ToDate,string requestLevelPerson)
        {
            IList<EmployeeAttendanceModel> employeeAttendanceModelList = null;
            DateTime startDateFormatted, endDateFormatted;
            string tempRequestLevelPerson = requestLevelPerson;
            EmployeeProfile profile = (EmployeeProfile)Session["Profile"];
            Int64 userID = profile.UserId;
            if(!string.IsNullOrEmpty(Name) && Name.ToUpper()!="NONAME" )
            {
                IEmployeeHelper EmployeeHelperObj = new EmployeeClient();
                userID= EmployeeHelperObj.GetUserId(Name.Replace("|"," "));
                requestLevelPerson = "My";// If we are not change the requst level person, If we pass any manager ID it will return all timesheet who are all under the manager
                if(userID==0)
                {
                    return new List<EmployeeAttendanceModel>();
                }
            }
            if (string.IsNullOrEmpty(FromDate)|| FromDate== "Nodate")
            {
                startDateFormatted = DateTime.Now.AddDays(-30).Date;
                endDateFormatted = DateTime.Now;
            }
            else
            {
                 startDateFormatted = DateTime.Parse(FromDate, new CultureInfo("en-GB", true));
                 endDateFormatted = DateTime.Parse(ToDate, new CultureInfo("en-GB", true));
            }
           
            IEmployeeAttendanceHelper employeeAttendanceHelper = new EmplyeeAttendenceClient();
            employeeAttendanceModelList = employeeAttendanceHelper.GetAttendenceForRange(userID, startDateFormatted, endDateFormatted, requestLevelPerson);
            requestLevelPerson = tempRequestLevelPerson;
            return employeeAttendanceModelList;
        }
        private DateTime ChangeTime(DateTime dateTime, int hours, int minutes, int seconds, int milliseconds)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hours,
                minutes,
                seconds,
                milliseconds,
                dateTime.Kind);
        }

        public List<TimeSheetModel> GetEmployeeTimeSheet(TimeSheetQueryModel TimeSheetQueryModelObj,out string errorMessage,string requestLevelPerson)
        {
            errorMessage = string.Empty;
            List<EmployeeAttendanceModel> employeeAttendanceModelList = new List<EmployeeAttendanceModel>();
            ITimesheetHelper EmployeeAttendanceHelperObj = new TimesheetClient();
            if (string.IsNullOrEmpty(TimeSheetQueryModelObj.Name))
            {
                EmployeeProfile profile = (EmployeeProfile)Session["Profile"];
                TimeSheetQueryModelObj.UserID = profile.UserId;
                if (TimeSheetQueryModelObj.FromDate == DateTime.MinValue)
                {
                    // For Last 15 days Attendence
                    TimeSheetQueryModelObj.FromDate = DateTime.Now.Add(TimeSpan.Parse("00:00:00")).AddDays(-30);
                    TimeSheetQueryModelObj.ToDate = DateTime.Now.AddDays(-1);
                }
            }
            else if (!string.IsNullOrEmpty(TimeSheetQueryModelObj.Name))
            {
                IEmployeeHelper EmployeeHelper = new EmployeeClient();
                Int64 UserID = EmployeeHelper.GetUserId(TimeSheetQueryModelObj.Name);
                if(UserID==0)
                {
                    errorMessage = "Employee name not exists";
                }
                if (TimeSheetQueryModelObj.FromDate == DateTime.MinValue)
                {
                  
                    // For Last 15 days Attendence
                    TimeSheetQueryModelObj.FromDate = DateTime.Now.Add(TimeSpan.Parse("00:00:00")).AddDays(-30);
                    TimeSheetQueryModelObj.ToDate = DateTime.Now.AddDays(-1);
                    
                }
                requestLevelPerson = "My";
                
                TimeSheetQueryModelObj.UserID = UserID;
            }
            if(TimeSheetQueryModelObj.ToDate>DateTime.Now)
            {
                TimeSheetQueryModelObj.ToDate = DateTime.Now;
            }
            List<TimeSheetModel> timeSheetModelList = new List<TimeSheetModel>();
            if(requestLevelPerson=="My")
            {
                timeSheetModelList = EmployeeAttendanceHelperObj.GetMyTimeSheet(TimeSheetQueryModelObj.UserID, TimeSheetQueryModelObj.FromDate, TimeSheetQueryModelObj.ToDate);
            }
            else
            {
                timeSheetModelList = EmployeeAttendanceHelperObj.GetMyTeamTimeSheet(TimeSheetQueryModelObj.UserID, TimeSheetQueryModelObj.FromDate, TimeSheetQueryModelObj.ToDate);
            }
            
            return timeSheetModelList;
        }
        public ActionResult LoadMyTimesheet(TimeSheetQueryModel TimeSheetQueryModelObj)
        {
            string errorMessage = string.Empty;
            List<TimeSheetModel> timeSheetModelList = GetEmployeeTimeSheet(TimeSheetQueryModelObj,out errorMessage, "My");
            ViewBag.RequestLevelPerson = "My";
            return PartialView("TimeSheetPartial", timeSheetModelList);

        }
        public ActionResult LoadMyTeamTimesheet(TimeSheetQueryModel TimeSheetQueryModelObj)
        {
            string errorMessage = string.Empty;
            List<TimeSheetModel> timeSheetModelList = GetEmployeeTimeSheet(TimeSheetQueryModelObj, out errorMessage, "Team");
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new HttpStatusCodeResult(400, "Given Username does not exists");
            }
            ViewBag.RequestLevelPerson = "Team";
            return PartialView("TimeSheetPartial", timeSheetModelList);

        }

        public ActionResult ExportTimeSheetToExcel(TimeSheetQueryModel TimeSheetQueryModelObj, string RequestLevelPerson)
        {
            string errorMessage = string.Empty;
            List<TimeSheetModel> timeSheetModelList = GetEmployeeTimeSheet(TimeSheetQueryModelObj,out errorMessage, RequestLevelPerson);
            List<TimeSheetModel> excelData = timeSheetModelList.ToList();
            if (excelData.Count > 0)
            {
                string[] columns = {  "WorkingDate", "Shift", "InTime", "OutTime","WorkingHours","Name", "Status" };
                byte[] filecontent = ExcelExportHelper.ExportExcelTimeSheet(excelData, "", false, columns);
                if(string.IsNullOrEmpty(TimeSheetQueryModelObj.Name))
                {
                    EmployeeProfile profile = (EmployeeProfile)Session["Profile"];
                    TimeSheetQueryModelObj.Name = profile.FirstName + " " + profile.LastName;
                }
               
                return File(filecontent, ExcelExportHelper.ExcelContentType, TimeSheetQueryModelObj.Name + "_" + System.DateTime.Now + ".xlsx");
            }
            else
            {
                using (var Client = new LeaveClient())
                {
                    var result = Client.GetYearsFromLeaveBalance();
                    ViewBag.YearsInLeaveBal = result;
                }
                ViewBag.RequestLevelPerson = RequestLevelPerson;
                EmployeeAttendenceQueryModel data = new EmployeeAttendenceQueryModel();
                data.ErrorMsg = "Excel file is not generated as no data returned.";
                return View("~/Views/Attendence/MyAttendance.cshtml", data);
            }
        }
    }
}