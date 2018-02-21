using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Repository;
using NLTD.EmployeePortal.LMS.Dac.DbModel;

namespace NLTD.EmployeePortal.LMS.Dac.Dac
{
    public class EmployeeAttendanceDac : IEmployeeAttendanceHelper
    {
        public void Dispose()
        {

        }

        public List<EmployeeAttendanceModel> GetAttendence(Int64 UserID)
        {
            List<EmployeeAttendanceModel> employeeAttendanceModelList = new List<EmployeeAttendanceModel>();
            try
            {
                using (var context = new NLTDDbContext())
                {
                    employeeAttendanceModelList = (from ea in context.EmployeeAttendance
                                                   where ea.UserID == UserID
                                                   select new EmployeeAttendanceModel
                                                   {
                                                       UserID = ea.UserID,
                                                       InOutDate = ea.InOutDate,
                                                       InOut = (ea.InOut ? "OUT" : "IN")
                                                   }).OrderByDescending(e => e.InOutDate).ToList();

                }
            }
            catch (Exception)
            {
                throw;
            }
            return employeeAttendanceModelList;
        }


        public List<EmployeeAttendanceModel> GetAttendenceForRange(Int64 UserID, DateTime FromDateTime, DateTime ToDateTime, string requestLevelPerson,bool isDirectEmployees)
        {
            List<EmployeeAttendanceModel> employeeAttendanceModelList = new List<EmployeeAttendanceModel>();
            try
            {
                if (requestLevelPerson.ToUpper() == "MY")
                {
                    using (var context = new NLTDDbContext())
                    {
                        employeeAttendanceModelList = (from ea in context.EmployeeAttendance
                                                       join e in context.Employee on ea.UserID equals e.UserId
                                                       where ea.UserID == UserID && ea.InOutDate >= FromDateTime && ea.InOutDate <= ToDateTime
                                                       select new EmployeeAttendanceModel
                                                       {
                                                           UserID = ea.UserID,
                                                           InOutDate = ea.InOutDate,
                                                           //InOut = GetInOut(ea.InOut)
                                                           InOut = (ea.InOut ? "Out" : "In"),
                                                           Name = (e.FirstName + " " +e.LastName)
                                                       }).OrderByDescending(e => e.InOutDate).ToList();
                    }
                }

                else
                {
                    using (var context = new NLTDDbContext())
                    {
                        var leadinfo = (from emp in context.Employee
                                        join role in context.EmployeeRole on emp.EmployeeRoleId equals role.RoleId
                                        where emp.UserId == UserID
                                        select new { RoleName = role.Role }).FirstOrDefault();
                        if (leadinfo != null)
                        {


                            if (leadinfo.RoleName.ToUpper() == "ADMIN" || leadinfo.RoleName.ToUpper() == "HR")
                            {
                                // We need to get all the employee List
                                employeeAttendanceModelList = (from ea in context.EmployeeAttendance join e in context.Employee on ea.UserID equals e.UserId
                                                               where ea.InOutDate >= FromDateTime && ea.InOutDate <= ToDateTime
                                                               select new EmployeeAttendanceModel
                                                               {
                                                                   UserID = ea.UserID,
                                                                   InOutDate = ea.InOutDate,
                                                                   //InOut = GetInOut(ea.InOut)
                                                                   InOut = (ea.InOut ? "Out" : "In"),
                                                                   Name = (e.FirstName+ " " + e.LastName)
                                                               }).OrderBy(e => e.Name).ThenByDescending(e => e.InOutDate).ToList();
                            }
                            else
                            {
                                IList<Int64> employeeIDs = null;
                                EmployeeDac EmployeeHelperObj = new EmployeeDac();
                                if(isDirectEmployees)
                                {
                                    employeeIDs = EmployeeHelperObj.GetDirectEmployees(UserID);
                                }
                                else
                                {
                                    employeeIDs = EmployeeHelperObj.GetEmployeesReporting(UserID); // to get the employee Under the manager
                                }
                                
                                
                                employeeAttendanceModelList = (from ea in context.EmployeeAttendance
                                                               join e in context.Employee on ea.UserID equals e.UserId
                                                               where employeeIDs.Contains(ea.UserID??0) && ea.InOutDate >= FromDateTime && ea.InOutDate <= ToDateTime
                                                               select new EmployeeAttendanceModel
                                                               {
                                                                   UserID = ea.UserID,
                                                                   InOutDate = ea.InOutDate,
                                                                   //InOut = GetInOut(ea.InOut)
                                                                   InOut = (ea.InOut ? "Out" : "In"),
                                                                   Name = (e.FirstName + " " + e.LastName)
                                                               }).OrderBy(e => e.Name).ThenByDescending(e => e.InOutDate).ToList();
                                // Get all the employee time sheet under the manager
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return employeeAttendanceModelList;
        }

       
        //private string GetInOut(bool InOut)
        //{
        //    return InOut ? "IN" : "OUT";
        //}
    }
}
