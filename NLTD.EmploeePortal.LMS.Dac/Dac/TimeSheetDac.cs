﻿using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Common.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLTD.EmployeePortal.LMS.Repository;
using NLTD.EmploeePortal.LMS.Dac.DbModel;
using System.Configuration;

namespace NLTD.EmploeePortal.LMS.Dac.Dac
{
    public class TimeSheetDac : ITimesheetHelper
    {
        public List<ShiftQueryModel> GetShiftDetails(Int64 UserID, DateTime FromDate, DateTime ToDate)
        {
            List<ShiftQueryModel> shiftQueryModelList = new List<ShiftQueryModel>();
            using (var entity = new NLTDDbContext())
            {

                shiftQueryModelList = (from sm in entity.ShiftMaster
                                       join smp in entity.ShiftMapping on sm.ShiftID equals smp.ShiftID
                                       join e in entity.Employee on smp.UserID equals e.UserId
                                       where smp.UserID == UserID && smp.ShiftDate >= FromDate && smp.ShiftDate <= ToDate
                                       select new ShiftQueryModel
                                       {
                                           UserID = smp.UserID,
                                           Employeename = e.FirstName + " " + e.LastName,
                                           ShiftFromtime = sm.FromTime,
                                           ShiftTotime = sm.ToTime,
                                           ShiftDate = smp.ShiftDate,

                                       }).ToList();
            }
            return shiftQueryModelList.OrderBy(m => m.ShiftDate).ToList();
        }

        private string GetAbsentStatus(DateTime CurrentDate, List<string> officeWeekOffDayList,
            List<OfficeHoliday> officeHolidayList, List<EmployeeLeave> employeeLeaveList)
        {
            string status = string.Empty;
            bool found = false;
            // to check whether the day is weekoff
            if (officeWeekOffDayList.Count() > 0)
            {
                int count = (from wod in officeWeekOffDayList
                             where wod == CurrentDate.DayOfWeek.ToString()
                             select wod).Count();
                if (count > 0)
                {
                    status = TimeSheetStatus.WeekOff;
                    found = true;
                }
            }
            // To check whether it is public holiday
            if (!found)
            {
                if (officeHolidayList.Count > 0)
                {
                    int count = (from hl in officeHolidayList where hl.Holiday == CurrentDate select hl)
                        .Count();
                    if (count > 0)
                    {
                        status = TimeSheetStatus.Holiday;
                        found = true;
                    }
                }
            }
            
            
                return status;
        }

        private List<ShiftQueryModel> GetEmployeeShift(List<ShiftQueryModel> shiftQueryModelList, DateTime currentDate)
        {
            List<ShiftQueryModel> ShiftQueryModelObj = null;
                //(from s in shiftQueryModelList where currentDate >= s.FromDate && currentDate <= s.ToDate select s).ToList();
            if (ShiftQueryModelObj == null)
            {
                ShiftQueryModelObj = new List<ShiftQueryModel>();
            }
            return ShiftQueryModelObj;
        }

        public List<TimeSheetModel> GetMyTimeSheet(Int64 UserID, DateTime FromDate, DateTime ToDate)
        {
            List<TimeSheetModel> timeSheetModelList = new List<TimeSheetModel>();
            List<ShiftQueryModel> ShiftQueryModelList = GetShiftDetails(UserID, FromDate, ToDate);
            IEmployeeAttendanceHelper EmployeeAttendanceDacObj = new EmployeeAttendanceDac();
            //To Retrive the Employee Attendence for the given date.
            List<EmployeeAttendanceModel> EmployeeAttendenceList = EmployeeAttendanceDacObj.GetAttendenceForRange(UserID, FromDate, ToDate, "My");

            // To Get the Employee name
            EmployeeProfile EmployeeProfileObj = new EmployeeDac().GetEmployeeProfile(UserID);
            string name = string.Empty;
            if (EmployeeProfileObj != null)
            {
                name = EmployeeProfileObj.FirstName + ' ' + EmployeeProfileObj.LastName;
            }

            // To get the employee weekoff Days
            OfficeWeekOffDac officeWeekOffDacObj = new OfficeWeekOffDac();
            List<string> officeWeekOffDayList = officeWeekOffDacObj.GetEmployeeWeekOffDay(UserID);
            // To get the employee Holiday List 
            OfficeHolidayDac officeHolidayDacObj = new OfficeHolidayDac();
            List<OfficeHoliday> officeHolidayList = officeHolidayDacObj.GetOfficeHoliday(UserID);
            // To get the employee Leave Details
            LeaveTransactionHistoryDac leaveTransactionHistoryDacObj = new LeaveTransactionHistoryDac();
            List<EmployeeLeave> employeeLeaveList = leaveTransactionHistoryDacObj.GetLeaveForEmployee(UserID);
            int BeforeShiftBuffer = Convert.ToInt32(ConfigurationManager.AppSettings["BeforeShiftBuffer"]);
            int AfterShiftBuffer = Convert.ToInt32(ConfigurationManager.AppSettings["AfterShiftBuffer"]);
            for (int i = 0; i < ShiftQueryModelList.Count(); i++)
            {
                TimeSheetModel TimeSheetModelObj = new TimeSheetModel();
                DateTime shiftFromDateTime = ShiftQueryModelList[i].ShiftDate.Add(ShiftQueryModelList[i].ShiftFromtime.Add(new TimeSpan(-BeforeShiftBuffer, 0, 0)));
                DateTime shiftEndDateTime = ShiftQueryModelList[i].ShiftDate.Add(ShiftQueryModelList[i].ShiftTotime.Add(new TimeSpan(AfterShiftBuffer, 0, 0)));

                if(shiftEndDateTime<shiftFromDateTime)
                {
                    shiftEndDateTime=shiftEndDateTime.AddDays(1);
                }
                // To add the employee basic details 
                TimeSheetModelObj.Shift = ShiftQueryModelList[i].ShiftFromtime.ToString(@"hh\:mm") + '-' + ShiftQueryModelList[i].ShiftTotime.ToString(@"hh\:mm");
                TimeSheetModelObj.userID = UserID;
                TimeSheetModelObj.Name = name;
                TimeSheetModelObj.WorkingDate = ShiftQueryModelList[i].ShiftDate;
                    // Linq query to find the min and max for the given date
                var maxmin = from s in EmployeeAttendenceList
                             where s.InOutDate >= shiftFromDateTime && s.InOutDate <= shiftEndDateTime
                             group s by true into r
                             select new
                             {
                                 min = r.Min(z => z.InOutDate),
                                 max = r.Max(z => z.InOutDate)
                             };
                if (maxmin != null && maxmin .Count()>0)
                {
                    TimeSheetModelObj.InTime = maxmin.ToList()[0].min;
                    TimeSheetModelObj.OutTime = maxmin.ToList()[0].max;
                    TimeSheetModelObj.WorkingHours = TimeSheetModelObj.OutTime - TimeSheetModelObj.InTime;
                    TimeSheetModelObj.Status = "Present";
                  
                }
                else// If no record found in the employee for the given date 
                {
                    // Get Absent Details 
                    TimeSheetModelObj.Status = GetAbsentStatus(ShiftQueryModelList[i].ShiftDate, officeWeekOffDayList,
           officeHolidayList, employeeLeaveList);
                }
                TimeSheetModelObj.LMSStatus = GetLMSStatus(employeeLeaveList, ShiftQueryModelList[i].ShiftDate);
                timeSheetModelList.Add(TimeSheetModelObj);

            }
            return timeSheetModelList.OrderByDescending(m=>m.WorkingDate).ToList();
        }

        public List<TimeSheetModel> GetMyTeamTimeSheet(Int64 UserID, DateTime FromDate, DateTime ToDate)
        {
            List<TimeSheetModel> timeSheetModelList = new List<TimeSheetModel>();
            // To Get all the employee profile under the manager or lead
            EmployeeDac EmployeeDacObj = new EmployeeDac();
            string userRole = string.Empty;
            // To get the employee role, whether he is the Team lead or HR Or admin
            using (NLTDDbContext context = new NLTDDbContext())
            {
                userRole=(from emp in context.Employee
                 join role in context.EmployeeRole on emp.EmployeeRoleId equals role.RoleId
                 where emp.UserId == UserID
                 select role.Role).FirstOrDefault();
            }
            List<EmployeeProfile> employeeProfileListUnderManager= EmployeeDacObj.GetReportingEmployeeProfile(UserID, userRole).OrderBy(m=>m.FirstName).ToList();
            for (int i = 0; i < employeeProfileListUnderManager.Count; i++)
            {
                List<TimeSheetModel> timeSheetModelListTemp = GetMyTimeSheet(employeeProfileListUnderManager[i].UserId, FromDate, ToDate);
                timeSheetModelList.AddRange(timeSheetModelListTemp);
            }
            return timeSheetModelList;
        }

        public string GetLMSStatus(List<EmployeeLeave> employeeLeaveList,DateTime statusDate)
        {
            string LMSStatus = string.Empty;
            if(employeeLeaveList.Count>0)
            {
                LMSStatus = (from e in employeeLeaveList where statusDate >= e.StartDate && statusDate <= e.EndDate select e.LeaveType).FirstOrDefault();
            }
            return LMSStatus;
        }
        
    }
    public class EmployeeLeave
    {
        public Int64 LeaveId { get; set; }
        public Int64 UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String StartDateType { get; set; }
        public String EndDateType { get; set; }
        public Int64 LeaveTypeId { get; set; }
        public Decimal Duration { get; set; }
        public String Remarks { get; set; }
        public String Comments { get; set; }
        public String Status { get; set; }
        public Int64 AppliedBy { get; set; }
        public DateTime AppliedAt { get; set; }
        public Int64? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public string LeaveType { get; set; }
    }
}
