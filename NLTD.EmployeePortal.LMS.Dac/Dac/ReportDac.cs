﻿using NLTD.EmployeePortal.LMS.Dac.Dac;
using NLTD.EmployeePortal.LMS.Dac.DbModel;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Common.QueryModel;
using NLTD.EmployeePortal.LMS.DbHelper;
using NLTD.EmployeePortal.LMS.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Text;
namespace NLTD.EmployeePortal.LMS.Dac
{
    public class ReportDac : IReportHelper
    {
        int BeforeShiftBuffer = Convert.ToInt32(ConfigurationManager.AppSettings["BeforeShiftBuffer"]);
        int AfterShiftBuffer = Convert.ToInt32(ConfigurationManager.AppSettings["AfterShiftBuffer"]);
        public List<LateAndEarltRpt> GetLateAndEarlyEmployees(DateTime FromDate, DateTime ToDate, Int64 UserId, bool OnlyReportedToMe)
        {
            List<LateAndEarltRpt> lateAndEarltRpt = new List<LateAndEarltRpt>();

            try
            {
                ToDate = ToDate.AddDays(1);
                using (var context = new NLTDDbContext())
                {
                    List<TimeSheetModel> timeSheetModelList = new List<TimeSheetModel>();
                    TimeSheetDac timeSheetDac = new TimeSheetDac();
                    List<TimeSheetModel> timeSheetModelListTemp = timeSheetDac.GetMyTeamTimeSheet(UserId, FromDate, ToDate, OnlyReportedToMe);
                    timeSheetModelList.AddRange(timeSheetModelListTemp);

                   var shiftQueryModelList = (from sm in context.ShiftMaster
                                           join smp in context.ShiftMapping on sm.ShiftID equals smp.ShiftID
                                           join e in context.Employee on smp.UserID equals e.UserId
                                           where smp.UserID == UserId && smp.ShiftDate >= FromDate && smp.ShiftDate <= ToDate
                                           select new ShiftQueryModel
                                           {
                                               UserID = smp.UserID,
                                               Employeename = e.FirstName + " " + e.LastName,
                                               ShiftFromtime = sm.FromTime,
                                               ShiftTotime = sm.ToTime,
                                               ShiftDate = smp.ShiftDate,

                                           }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lateAndEarltRpt;
        }

        public List<NoOfLateInMonth> GetLateReport(Int64 UserID, DateTime FromDate, DateTime ToDate, bool myDirectEmployees)
        {
            List<ReportLateMonth> reportLateMonthlst = new List<ReportLateMonth>();
            List<NoOfLateInMonth> noOfLateInMonth = new List<NoOfLateInMonth>();
            // To Get all the employee profile under the manager or lead
            EmployeeDac EmployeeDacObj = new EmployeeDac();
            string userRole = string.Empty;
            TimeSheetDac timeSheetDac = new TimeSheetDac();
            // To get the employee role, whether he is the Team lead or HR Or admin
            try
            {
                using (NLTDDbContext context = new NLTDDbContext())
                {
                    userRole = (from emp in context.Employee
                                join role in context.EmployeeRole on emp.EmployeeRoleId equals role.RoleId
                                where emp.UserId == UserID
                                select role.Role).FirstOrDefault();
                }
                List<EmployeeProfile> employeesUnderManager = EmployeeDacObj.GetReportingEmployeeProfile(UserID, userRole, myDirectEmployees).OrderBy(m => m.FirstName).ToList();
                for (int i = 0; i < employeesUnderManager.Count; i++)
                {
                    string Name = employeesUnderManager[i].FirstName + " " + employeesUnderManager[i].LastName;
                    List<ReportLateMonth> reportLateMonth = GetMyTimeSheet(employeesUnderManager[i].UserId, FromDate, ToDate, employeesUnderManager[i].ReportedToName, Name, employeesUnderManager[i].EmployeeId);
                    reportLateMonthlst.AddRange(reportLateMonth);
                }

                noOfLateInMonth = (from p in reportLateMonthlst
                                   where p.LateEntry != null
                                   group p by new { p.UserID, p.Name, p.ReportingTo, p.EmpId } into g
                                   select new NoOfLateInMonth
                                   {
                                       Name = g.Key.Name,
                                       UserID = g.Key.UserID,
                                       ReportingTo = g.Key.ReportingTo,
                                       EmpId = g.Key.EmpId,
                                       NoOfLate = g.Count()
                                   }).ToList();
                return noOfLateInMonth;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ReportLateMonth> GetMyTimeSheet(Int64 UserID, DateTime FromDate, DateTime ToDate, string ReportingTo, string Name, string EmpId)
        {
            TimeSheetDac timeSheetDac = new TimeSheetDac();
            List<ReportLateMonth> reportLateMonthlst = new List<ReportLateMonth>();
            List<ShiftQueryModel> ShiftQueryModelList = timeSheetDac.GetShiftDetails(UserID, FromDate, ToDate);
            try
            {
                var toDateShift = (from m in ShiftQueryModelList where m.ShiftDate == ToDate
                                   select new { fromTime = m.ShiftFromtime, toTime = m.ShiftTotime }).FirstOrDefault();
                TimeSpan fromTime = toDateShift.fromTime;
                TimeSpan toTime = toDateShift.toTime;
                if (fromTime > toTime)
                {
                    ToDate = ToDate.AddDays(1).Add(toTime.Add(new TimeSpan(AfterShiftBuffer, 0, 0)));
                }
                else
                {
                    ToDate = ToDate.Add(toTime.Add(new TimeSpan(AfterShiftBuffer, 0, 0)));
                }
                // TimeSpan ToDate = ToDate.Add(toDateShift);
            }
            catch (Exception)
            {
                throw;
            }
            IEmployeeAttendanceHelper EmployeeAttendanceDacObj = new EmployeeAttendanceDac();
            //To Retrive the Employee Attendence for the given date.
            List<EmployeeAttendanceModel> EmployeeAttendenceList = EmployeeAttendanceDacObj.GetAttendenceForRange(UserID, FromDate, ToDate, "My", true);

            try
            {
                for (int i = 0; i < ShiftQueryModelList.Count(); i++)
                {
                    ReportLateMonth reportLateMonth = new ReportLateMonth();
                    DateTime shiftFromDateTime = ShiftQueryModelList[i].ShiftDate.Add(ShiftQueryModelList[i].ShiftFromtime.Add(new TimeSpan(-BeforeShiftBuffer, 0, 0)));
                    DateTime shiftEndDateTime = ShiftQueryModelList[i].ShiftDate.Add(ShiftQueryModelList[i].ShiftTotime.Add(new TimeSpan(AfterShiftBuffer, 0, 0)));

                    if (shiftEndDateTime < shiftFromDateTime)
                    {
                        shiftEndDateTime = shiftEndDateTime.AddDays(1);
                    }

                    reportLateMonth.UserID = UserID;
                    reportLateMonth.ReportingTo =ReportingTo;
                    reportLateMonth.Name =Name;
                    reportLateMonth.EmpId=EmpId;
                    var maxmin = from s in EmployeeAttendenceList
                                 where s.InOutDate >= shiftFromDateTime && s.InOutDate <= shiftEndDateTime
                                 group s by true into r
                                 select new
                                 {
                                     min = r.Min(z => z.InOutDate)
                                 };

                    if (maxmin != null && maxmin.Count() > 0)
                    {   
                        if (maxmin.ToList()[0].min.TimeOfDay > ShiftQueryModelList[i].ShiftFromtime)
                        {
                            reportLateMonth.LateEntry = maxmin.ToList()[0].min.TimeOfDay - ShiftQueryModelList[i].ShiftFromtime;
                            reportLateMonthlst.Add(reportLateMonth);
                        }
                    }                       
                }
            }
            catch (Exception)
            {
                throw;
            }
            return reportLateMonthlst;
        }

        public void Dispose()
        {
            //Nothing to implement...
        }
    }
}