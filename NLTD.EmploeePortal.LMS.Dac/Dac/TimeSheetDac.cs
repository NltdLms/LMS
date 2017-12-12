using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Common.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLTD.EmployeePortal.LMS.Repository;
using NLTD.EmploeePortal.LMS.Dac.DbModel;

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
        public List<TimeSheetModel> GetTimeSheet(Int64 UserID, DateTime FromDate, DateTime ToDate, string requestLevelPerson)
        {
            List<TimeSheetModel> timeSheetModelList = new List<TimeSheetModel>();
            // To retrive the Shift details for the employee
            List<ShiftQueryModel> shiftQueryModelList = GetShiftDetails(UserID, FromDate, ToDate);

            IEmployeeAttendanceHelper EmployeeAttendanceDacObj = new EmployeeAttendanceDac();
            //To Retrive the Employee Attendence for the given date.
            List<EmployeeAttendanceModel> EmployeeAttendenceList = EmployeeAttendanceDacObj.GetAttendenceForRange(UserID, FromDate, ToDate, requestLevelPerson);

            DateTime currentDate = FromDate;
            // To get the employee profile
            EmployeeProfile EmployeeProfileObj = new EmployeeDac().GetEmployeeProfile(UserID);
            string employeeName = EmployeeProfileObj.FirstName + " " + EmployeeProfileObj.LastName;

            // To get the employee weekoff Days
            OfficeWeekOffDac officeWeekOffDacObj = new OfficeWeekOffDac();
            List<string> officeWeekOffDayList = officeWeekOffDacObj.GetEmployeeWeekOffDay(UserID);
            // To get the employee Holiday List 
            OfficeHolidayDac officeHolidayDacObj = new OfficeHolidayDac();
            List<OfficeHoliday> officeHolidayList = officeHolidayDacObj.GetOfficeHoliday(UserID);
            // To get the employee Leave Details
            LeaveTransactionHistoryDac leaveTransactionHistoryDacObj = new LeaveTransactionHistoryDac();
            List<Leave> employeeLeaveList = leaveTransactionHistoryDacObj.GetLeaveForEmployee(UserID);
            bool found = false;
            TimeSpan shiftFromTime = new TimeSpan(09, 30, 0);
            TimeSpan shiftToTime = new TimeSpan(18, 30, 0);
            TimeSpan empFromTime = new TimeSpan(09, 30, 0);
            TimeSpan empToTime = new TimeSpan(18, 30, 0);
            if (EmployeeAttendenceList.Count > 0)
            {

                DateTime shiftFromDateTime = currentDate.Add(shiftFromTime);
                DateTime shiftToDateTime = currentDate.Add(shiftToTime);
                TimeSheetModel timeSheetModelObj = null;

                while (currentDate != ToDate.AddDays(1))
                {
                    found = false;
                    timeSheetModelObj = new TimeSheetModel();
                    timeSheetModelObj.WorkingDate = currentDate;
                    var shift = GetEmployeeShift(shiftQueryModelList, currentDate);
                    if (shift.Count() > 0)
                    {
                        shiftFromTime = shift.First().ShiftFromtime;
                        shiftToTime = shift.First().ShiftTotime;
                    }
                    shiftFromDateTime = currentDate.Add(shiftFromTime);
                    shiftToDateTime = currentDate.Add(shiftToTime);
                    if (shiftToDateTime < shiftFromDateTime)
                    {
                        shiftToDateTime = shiftToDateTime.AddDays(1);
                    }
                    timeSheetModelObj.userID = UserID;
                    timeSheetModelObj.Shift = shiftFromTime + " - " + shiftToTime;
                    var startandendtime = from ea in EmployeeAttendenceList
                                          where ea.InOutDate >= shiftFromDateTime.AddHours(-3)
                                            && ea.InOutDate <= shiftToDateTime.AddHours(5)
                                          group ea.InOutDate
                                            by ea.UserID into grp
                                          select new { startTime = grp.Min(), toTime = grp.Max() };
                    if (startandendtime.Count() > 0)
                    {
                        timeSheetModelObj.InTime = startandendtime.First().startTime;
                        timeSheetModelObj.OutTime = startandendtime.First().toTime;
                        timeSheetModelObj.WorkingHours = timeSheetModelObj.OutTime - timeSheetModelObj.InTime;

                        timeSheetModelObj.Status = TimeSheetStatus.Present;
                    }
                    else
                    {

                        timeSheetModelObj.Status = GetAbsentStatus(timeSheetModelObj.WorkingDate,
                            officeWeekOffDayList, officeHolidayList, employeeLeaveList);
                    }
                    timeSheetModelObj.Name = employeeName;
                    timeSheetModelList.Add(timeSheetModelObj);
                    currentDate = currentDate.AddDays(1);
                }
            }
            else
            {
                TimeSheetModel timeSheetModelObj = null;
                while (currentDate != ToDate.AddDays(1))
                {
                    timeSheetModelObj = new TimeSheetModel();
                    timeSheetModelObj.Status = GetAbsentStatus(currentDate, officeWeekOffDayList,
            officeHolidayList, employeeLeaveList);
                    timeSheetModelObj.WorkingDate = currentDate;
                    timeSheetModelList.Add(timeSheetModelObj);
                    //timeSheetModelList.add
                    currentDate = currentDate.AddDays(1);
                }
            }

            return timeSheetModelList;

        }

        private string GetAbsentStatus(DateTime CurrentDate, List<string> officeWeekOffDayList,
            List<OfficeHoliday> officeHolidayList, List<Leave> employeeLeaveList)
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
            if (!found)
            {
                if (employeeLeaveList.Count > 0)
                {
                    int count = (from el in employeeLeaveList
                                 where
    CurrentDate >= el.StartDate
    && CurrentDate <= el.EndDate
                                 select el).Count();
                    if (count > 0)
                    {
                        status = TimeSheetStatus.Leave;
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
            List<Leave> employeeLeaveList = leaveTransactionHistoryDacObj.GetLeaveForEmployee(UserID);

            for (int i = 0; i < ShiftQueryModelList.Count(); i++)
            {
                TimeSheetModel TimeSheetModelObj = new TimeSheetModel();
                DateTime shiftFromDateTime = ShiftQueryModelList[i].ShiftDate.Add(ShiftQueryModelList[i].ShiftFromtime.Add(new TimeSpan(-3, 0, 0)));
                DateTime shiftEndDateTime = ShiftQueryModelList[i].ShiftDate.Add(ShiftQueryModelList[i].ShiftTotime.Add(new TimeSpan(5, 0, 0)));

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
    }
}
