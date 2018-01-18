﻿using NLTD.EmploeePortal.LMS.Dac.DbModel;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmploeePortal.LMS.Dac.Dac
{
    public class EmployeeLeaveBalanceDac : IDisposable
    {
        public void Dispose()
        {
            //Nothing to implement...
        }
        //Added by Tamil
        public IList<LeaveBalanceEmpProfile> GetLeaveBalanceEmpProfile(Int64 userId)
        {
            IList<LeaveBalanceEmpProfile> retModel = new List<LeaveBalanceEmpProfile>();

            using (var context = new NLTDDbContext())
            {

                if (userId > 0)
                {
                    var employeeLeaveBalanceProfile = (from employee in context.Employee
                                                       where employee.UserId == userId
                                                       select new EmployeeLeaveBalanceProfile
                                                       {
                                                           LogonId = employee.LoginId,
                                                           EmployeeId = employee.EmployeeId,
                                                           UserId = employee.UserId,
                                                           FirstName = employee.FirstName,
                                                           LastName = employee.LastName,
                                                           ReportedToId = employee.ReportingToId
                                                       }).FirstOrDefault();

                    var repName = context.Employee.Where(x => x.UserId == employeeLeaveBalanceProfile.ReportedToId).FirstOrDefault();
                    if (repName != null)
                    {
                        employeeLeaveBalanceProfile.ReportedToName = repName.FirstName + " " + repName.LastName;
                    }

                    var employeeLeaveBalance = (from l in context.LeaveType
                                                from elb in context.EmployeeLeaveBalance.Where(x => x.LeaveTypeId == l.LeaveTypeId && x.UserId == userId && x.Year == DateTime.Now.Year).DefaultIfEmpty()
                                                where l.AdjustLeaveBalance == true
                                                orderby l.LeaveTypeId
                                                select new EmployeeLeaveBalanceDetails
                                                {
                                                    LeaveTypeId = l.LeaveTypeId,
                                                    OfficeId = l.OfficeId,
                                                    Type = l.Type,
                                                    AdjustLeaveBalance = l.AdjustLeaveBalance,
                                                    LeaveBalanceId = elb.LeaveBalanceId,
                                                    ExistingTotalDays = elb.TotalDays ?? 0,
                                                    BalanceDays = elb.BalanceDays,
                                                    UserId = elb.UserId,
                                                    Year = elb.Year
                                                }).ToList();

                    retModel.Add(new LeaveBalanceEmpProfile
                    {
                        employeeLeaveBalanceProfile = employeeLeaveBalanceProfile,
                        lstEmployeeLeaveBalance = employeeLeaveBalance,
                        Name = employeeLeaveBalanceProfile.FirstName + " " + employeeLeaveBalanceProfile.LastName
                    });
                }
            }

            return retModel;
        }

        public string UpdateLeaveBalance(List<EmployeeLeaveBalanceDetails> empLeaveBalanceDetails, Int64 UserId, Int64 LoginUserId)
        {
            try
            {
                int isSaved = 0; bool isAuthorizedRole = false;
                using (var context = new NLTDDbContext())
                {
                    var isAuthorized = (from e in context.Employee
                                        join r in context.EmployeeRole on e.EmployeeRoleId equals r.RoleId
                                        where e.UserId == LoginUserId
                                        select new { r.Role }
                                  ).FirstOrDefault();

                    if (isAuthorized != null)
                    {
                        if (isAuthorized.Role.ToUpper() == "HR")
                            isAuthorizedRole = true;
                    }

                    if (isAuthorizedRole)
                    {
                         foreach (var item in empLeaveBalanceDetails)
                        {
                            if (item.NoOfDays > 0)
                            {
                                EmployeeLeaveBalance leaveBalance = context.EmployeeLeaveBalance.Where(x => x.UserId == UserId && x.LeaveTypeId == item.LeaveTypeId 
                                && x.LeaveBalanceId == item.LeaveBalanceId && x.Year == DateTime.Now.Year).FirstOrDefault();

                                if (leaveBalance != null)
                                {
                                    leaveBalance.TotalDays = item.CreditOrDebit == "C" ? (leaveBalance.TotalDays + item.NoOfDays) : (leaveBalance.TotalDays - item.NoOfDays);
                                    leaveBalance.ModifiedBy = LoginUserId;
                                    leaveBalance.ModifiedOn = DateTime.Now;
                                    leaveBalance.BalanceDays = item.TotalDays;
                                    isSaved = context.SaveChanges();
                                }
                                else
                                {
                                    leaveBalance = new EmployeeLeaveBalance();
                                    leaveBalance.UserId = UserId;
                                    leaveBalance.Year = DateTime.Now.Year;
                                    leaveBalance.LeaveTypeId = Convert.ToInt64(item.LeaveTypeId);
                                    leaveBalance.TotalDays = item.TotalDays;
                                    leaveBalance.LeaveTakenDays = 0;
                                    leaveBalance.PendingApprovalDays = 0;
                                    leaveBalance.BalanceDays = item.TotalDays;
                                    leaveBalance.CreatedBy = LoginUserId;
                                    leaveBalance.CreatedOn = DateTime.Now;
                                    leaveBalance.ModifiedBy = LoginUserId;
                                    leaveBalance.ModifiedOn = DateTime.Now;
                                    context.EmployeeLeaveBalance.Add(leaveBalance);
                                    isSaved = context.SaveChanges();
                                }

                                LeaveTransactiontHistory leaveTransactiontHistory = new LeaveTransactiontHistory();
                                leaveTransactiontHistory.UserId = UserId;
                                leaveTransactiontHistory.LeaveTypeId = Convert.ToInt64(item.LeaveTypeId);
                                leaveTransactiontHistory.LeaveId = -1;
                                leaveTransactiontHistory.TransactionDate = DateTime.Now;
                                leaveTransactiontHistory.TransactionType = item.CreditOrDebit;
                                leaveTransactiontHistory.NumberOfDays = item.NoOfDays;
                                leaveTransactiontHistory.TransactionBy = LoginUserId;
                                leaveTransactiontHistory.Remarks = item.Remarks;
                                context.LeaveTransactionHistory.Add(leaveTransactiontHistory);
                                isSaved = context.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        return "Need Role";
                    }

                }
                if (isSaved > 0)
                    return "Saved";
                else
                    return "Failed";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
