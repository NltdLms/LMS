using NLTD.EmploeePortal.LMS.Dac.DbModel;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLTD.EmploeePortal.LMS.Dac.DbModel;


namespace NLTD.EmploeePortal.LMS.Dac.Dac
{
    public class LeaveTransactionHistoryDac : IDisposable
    {
        public void Dispose()
        {
            //Nothing to dispose..
        }
        public IList<LeaveTransactionDetail> GetTransactionLog(string Name, string RequestMenuUser, long LeaduserId)
        {
            IList<LeaveTransactionDetail> retModel = new List<LeaveTransactionDetail>();
            LeaveDac lv = new LeaveDac();
            IList<Int64> empList = lv.GetEmployeesReporting(LeaduserId);
            using (var context = new NLTDDbContext())
            {
                EmployeeDac employeeDac = new EmployeeDac();
                long userId = 0;

                if (RequestMenuUser != "My")
                    userId = employeeDac.GetUserId(Name);


                if (userId > 0 || (RequestMenuUser == "My" && LeaduserId > 0))
                {
                    string ReportingTo = (RequestMenuUser == "My" && LeaduserId > 0) ? employeeDac.ReportingToName(LeaduserId) : employeeDac.ReportingToName(userId);

                    List<LeaveTransactiontHistoryModel> transactionDetails = new List<LeaveTransactiontHistoryModel>();
                    if (RequestMenuUser == "My")
                    {
                        transactionDetails = getTransactionDetails(context, LeaduserId);
                    }
                    else if (RequestMenuUser == "Admin")
                    {
                        var leadinfo = (from emp in context.Employee
                                        join role in context.EmployeeRole on emp.EmployeeRoleId equals role.RoleId
                                        where emp.UserId == LeaduserId
                                        select new { RoleName = role.Role }).FirstOrDefault();

                        if (leadinfo.RoleName.ToUpper() == "ADMIN" || leadinfo.RoleName.ToUpper() == "HR")
                        {
                            transactionDetails = getTransactionDetails(context, userId);
                        }
                    }
                    else if (RequestMenuUser == "Team")
                    {
                        //var user = (from e in context.Employee
                        //            where e.ReportingToId == LeaduserId
                        //            select e).ToList();

                        var user = empList.Where(x => x == userId).FirstOrDefault();

                        //var found = FindControlRecursively(user, userId);

                        if (user != null)
                        {
                            transactionDetails = getTransactionDetails(context, userId);
                        }
                    }

                    IList<LeaveTransactionDetail> retList = new List<LeaveTransactionDetail>();
                    var groupedLeaveList = transactionDetails.GroupBy(u => u.LeaveTypeId)
                                                          .Select(grp => new { LeaveTypeId = grp.Key, LeaveTransactiontHistoryModel = grp.ToList() })
                                                          .ToList();

                    retModel = (from gv in groupedLeaveList
                                select new LeaveTransactionDetail
                                {
                                    ReportingTo = ReportingTo,
                                    LeaveTypeId = gv.LeaveTypeId,
                                    LeaveType = gv.LeaveTransactiontHistoryModel[0].Type,
                                    LeaveTransactiontHistoryModel = gv.LeaveTransactiontHistoryModel
                                }).ToList();
                }
            }

            return retModel;
        }
		
		public static Employee FindControlRecursively(List<Employee> root, Int64 id)
        {
            foreach (Employee emp in root)
            {
                if (emp.UserId == id)
                    return emp;
                using (var context = new NLTDDbContext())
                {
                    var child = (from e in context.Employee
                                 where e.ReportingToId == emp.UserId
                                 select e).ToList();
                    Employee found = FindControlRecursively(child, id);
                    if (found != null)
                        return found;
                }
            }
            return null;
        }
        public List<LeaveTransactiontHistoryModel> getTransactionDetails(NLTDDbContext context, long userId)
        {
            // var myInClause = new string[] { "P", "R", "C" };

            //var transactionDetails = (from lth in context.LeaveTransactionHistory
            //                          join l in context.LeaveType on lth.LeaveTypeId equals l.LeaveTypeId
            //                          join e in context.Employee on lth.UserId equals e.UserId
            //                          where lth.UserId == userId && lth.LeaveId == -1 && l.IsTimeBased == false
            //                          select new LeaveTransactiontHistoryModel
            //                          {
            //                              LeaveTypeId = lth.LeaveTypeId,
            //                              TransactionType = lth.TransactionType == "C" ? "Credit" : "Debit",
            //                              NumberOfDays = lth.NumberOfDays,
            //                              UserId = lth.UserId,
            //                              Remarks = lth.Remarks,
            //                              TransactionDate = lth.TransactionDate,
            //                              Type = l.Type,
            //                              FirstName = e.FirstName,
            //                              LastName = e.LastName,
            //                              EmployeeId = e.EmployeeId
            //                          }).ToList();
            //transactionDetails = transactionDetails.Union
            var transactionDetails = (from lth in context.LeaveTransactionHistory
                                      join lt in context.LeaveType on lth.LeaveTypeId equals lt.LeaveTypeId
                                      //join l in context.Leave on lth.LeaveId equals l.LeaveId
                                      join e in context.Employee on lth.UserId equals e.UserId
                                      where lth.UserId == userId && lt.IsTimeBased == false
                                      // myInClause.Contains(lth.Remarks) &&
                                      select new LeaveTransactiontHistoryModel
                                      {
										  LeaveId= lth.LeaveId,
                                          LeaveTypeId = lth.LeaveTypeId,
                                          TransactionType = lth.TransactionType == "C" ? "Credit" : "Debit",
                                          NumberOfDays = lth.NumberOfDays,
                                          UserId = lth.UserId,
                                          Remarks = lth.Remarks,
                                          TransactionDate = lth.TransactionDate,
                                          Type = lt.Type,
                                          FirstName = e.FirstName,
                                          LastName = e.LastName,
                                          EmployeeId = e.EmployeeId
                                      }).ToList().OrderByDescending(x => x.TransactionDate).ToList();
            return transactionDetails;
        }

        public List<EmployeeLeave> GetLeaveForEmployee(Int64 UserID)
        {
            List<EmployeeLeave> leaveList = new List<EmployeeLeave>();
            using (var context = new NLTDDbContext())
            {
                leaveList = (from l in context.Leave join
                             lt in context.LeaveType on l.LeaveTypeId equals lt.LeaveTypeId
                             where l.UserId == UserID && l.Status== "A"
                             select new EmployeeLeave
                             {
                                 UserId = l.UserId,
                                 StartDate=l.StartDate,
                                 EndDate = l.EndDate,
                                 LeaveType = lt.Type
                             }
                             ).ToList();
            }
            return leaveList;
        }
    }
}
