using NLTD.EmploeePortal.LMS.Dac.DbModel;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Common.QueryModel;
using NLTD.EmployeePortal.LMS.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Text;
namespace NLTD.EmploeePortal.LMS.Dac
{
    public class ShiftDac : IShiftHelper
    {
        public List<ShiftAllocation> GetShiftAllocation(Int64 UserId, string RequestMenuUser)
        {
            List<ShiftAllocation> lstShiftAllocation = new List<ShiftAllocation>();

            using (var context = new NLTDDbContext())
            {
                if (RequestMenuUser == "Admin")
                {
                    var leadinfo = (from emp in context.Employee
                                    join role in context.EmployeeRole on emp.EmployeeRoleId equals role.RoleId
                                    where emp.UserId == UserId
                                    select new { RoleName = role.Role }).FirstOrDefault();

                    if (leadinfo.RoleName.ToUpper() == "ADMIN" || leadinfo.RoleName.ToUpper() == "HR")
                    {
                        lstShiftAllocation = (from sm in context.ShiftMapping
                                              join s in context.ShiftMaster on sm.ShiftID equals s.ShiftID
                                              join e in context.Employee on sm.UserID equals e.UserId
                                              where e.IsActive == true
                                              select new ShiftAllocation
                                              {
                                                  Name = e.FirstName + " " + e.LastName,
                                                  EmpId = e.EmployeeId,
                                                  UserId = sm.UserID,
                                                  ShiftID = s.ShiftID,
                                                   ShiftDate= sm.ShiftDate,
                                                  ToTime = s.ToTime,
                                                  ShiftName = s.ShiftDescription,
                                                  IsActive = e.IsActive,
                                                  ShiftMappingID = sm.ShiftMappingID
                                              }).ToList();
                    }
                }
                else
                {
                    lstShiftAllocation = GetShiftEmployees(context, UserId);
                }
            }
            return lstShiftAllocation;
        }




        public List<Shifts> GetShiftMaster()
        {
            List<Shifts> lstShift = new List<Shifts>();

            using (var context = new NLTDDbContext())
            {
                lstShift = (from s in context.ShiftMaster
                            select new Shifts
                            {
                                ShiftId = s.ShiftID,
                                ShiftName = s.ShiftDescription,
                                FromTime = s.FromTime,
                                ToTime = s.ToTime,
                            }).ToList();

            }
            return lstShift;
        }


        public AddShiftEmployee GetShiftDetailsForUsers(Int64 UserId, Int64 ShiftMappingId, string RequestMenuUser)
        {
            AddShiftEmployee lstAddShiftEmployee = new AddShiftEmployee();

            using (var context = new NLTDDbContext())
            {
                var lstShiftEmployees = new List<ShiftEmployees>();

                if (RequestMenuUser == "Admin")
                {
                    if (ShiftMappingId > 0)
                    {
                        lstShiftEmployees = (from sm in context.ShiftMapping
                                             join e in context.Employee on sm.UserID equals e.UserId
                                             where sm.ShiftMappingID == ShiftMappingId
                                             select new ShiftEmployees
                                             {
                                                 Name = e.FirstName + " " + e.LastName,
                                                 EmpId = e.EmployeeId,
                                                 UserId = e.UserId,
                                                 ShiftId = sm.ShiftID,
                                                 ShiftDate = sm.ShiftDate,
                                                 ShiftMappingID = sm.ShiftMappingID
                                             }).ToList();
                    }
                    else
                    {
                        lstShiftEmployees = (from e in context.Employee
                                             where e.IsActive == true
                                             select new ShiftEmployees
                                             {
                                                 Name = e.FirstName + " " + e.LastName,
                                                 EmpId = e.EmployeeId,
                                                 UserId = e.UserId
                                             }).ToList();
                    }
                }
                else
                {
                    if (ShiftMappingId > 0)
                    {
                        lstShiftEmployees = (from sm in context.ShiftMapping
                                             join e in context.Employee on sm.UserID equals e.UserId
                                             where sm.ShiftMappingID == ShiftMappingId
                                             select new ShiftEmployees
                                             {
                                                 Name = e.FirstName + " " + e.LastName,
                                                 EmpId = e.EmployeeId,
                                                 UserId = e.UserId,
                                                 ShiftId = sm.ShiftID,
                                                 ShiftDate = sm.ShiftDate,
                                                 ShiftMappingID = sm.ShiftMappingID
                                             }).ToList();
                    }
                    else
                    {
                        lstShiftEmployees = GetEmployees(context, UserId);
                    }
                }
                var lstShift = context.ShiftMaster.AsEnumerable().Select(s => new Shifts
                {
                    ShiftId = s.ShiftID,
                    ShiftName = s.ShiftDescription + "( " + string.Format("{0:hh\\:mm}", s.FromTime) + " - " + string.Format("{0:hh\\:mm}", s.ToTime) + " )",
                    FromTime = s.FromTime,
                    ToTime = s.ToTime
                }).ToList();

                lstAddShiftEmployee.ShiftEmployees = lstShiftEmployees;
                lstAddShiftEmployee.Shifts = lstShift;
                lstAddShiftEmployee.IsEdit = (ShiftMappingId > 0);
            }
            return lstAddShiftEmployee;
        }


        public string SaveEmployeeShift(List<Int64> UserId, int Shift, DateTime FromDate, DateTime ToDate, Int64 MgrId, Int64? ShiftMappingID)
        {
            try
            {
                int isSaved = 0;

                string errorMsg = "";
                using (var context = new NLTDDbContext())
                {
                    foreach (var Id in UserId)
                    {
                        int exists=0;
                        //if (ShiftMappingID != null)
                        //{
                        //    exists = context.ShiftMapping.Where(c => c.UserID == Id && c.ShiftMappingID != ShiftMappingID &&
                        //((FromDate >= c.FromDate && FromDate <= c.ToDate) || (ToDate >= c.FromDate && ToDate <= c.ToDate)
                        // || (c.FromDate >= FromDate && c.FromDate <= ToDate) || (c.ToDate >= FromDate && c.ToDate <= ToDate))).Select(p => p.ShiftMappingID).Count();
                        //}
                        //else
                        //{
                        //    exists = context.ShiftMapping.Where(c => c.UserID == Id &&
                        //((FromDate >= c.FromDate && FromDate <= c.ToDate) || (ToDate >= c.FromDate && ToDate <= c.ToDate)
                        //|| (c.FromDate >= FromDate && c.FromDate <= ToDate) || (c.ToDate >= FromDate && c.ToDate <= ToDate))).Select(p => p.ShiftMappingID).Count();
                        //}

                        if (exists > 0)
                        {
                            var emp = (from e in context.Employee
                                       where e.UserId == Id
                                       select new { Name = e.FirstName + " " + e.LastName }).FirstOrDefault();
                            errorMsg += emp.Name + Environment.NewLine;
                        }
                        else
                        {
                            if (ShiftMappingID != null)
                            {
                                var shiftMapping = context.ShiftMapping.Where(c => c.ShiftMappingID == ShiftMappingID).SingleOrDefault();
                                shiftMapping.ShiftID = Shift;
                                //shiftMapping.ShiftDate = ShiftDate,
                                shiftMapping.ModifiedBy = MgrId;
                                shiftMapping.ModifiedDate = DateTime.Now;
                            }
                            else
                            {
                                ShiftMapping shiftMapping = new ShiftMapping();
                                shiftMapping.UserID = Id;
                                shiftMapping.ShiftID = Shift;
                                //shiftMapping.FromDate = FromDate;
                                //shiftMapping.ToDate = ToDate;
                                shiftMapping.CreatedBy = MgrId;
                                shiftMapping.Createddate = DateTime.Now;
                                context.ShiftMapping.Add(shiftMapping);
                            }

                            isSaved = context.SaveChanges();

                            ShiftTransaction shiftTransaction = new ShiftTransaction();
                            shiftTransaction.ShiftID = Shift;
                            shiftTransaction.UserId = Id;
                            shiftTransaction.CreatedBy = MgrId;
                            shiftTransaction.Createddate = DateTime.Now;
                            shiftTransaction.FromDate = FromDate;
                            shiftTransaction.ToDate = ToDate;
                            context.ShiftTransaction.Add(shiftTransaction);
                            isSaved = context.SaveChanges();
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(errorMsg))
                    return "Available" + errorMsg;

                return isSaved > 0 ? "Saved" : "Failed";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        List<ShiftEmployees> GetEmployees(NLTDDbContext context, Int64 UserId)
        {
            var result = new List<ShiftEmployees>();

            var employees = (from e in context.Employee
                             where e.ReportingToId == UserId && e.IsActive == true
                             select new ShiftEmployees
                             {
                                 Name = e.FirstName + " " + e.LastName,
                                 EmpId = e.EmployeeId,
                                 UserId = e.UserId
                             }).ToList();

            foreach (var employee in employees)
            {
                result.Add(employee);
                result.AddRange(GetEmployees(context, employee.UserId));
            }

            return result;
        }

        List<ShiftAllocation> GetShiftEmployees(NLTDDbContext context, Int64 UserId)
        {
            var result = new List<ShiftAllocation>();

            var emp = (from e in context.Employee
                       where e.ReportingToId == UserId && e.IsActive == true
                       select e.UserId).ToList();

            var employees = (from sm in context.ShiftMapping
                             join s in context.ShiftMaster on sm.ShiftID equals s.ShiftID
                             join e in context.Employee on sm.UserID equals e.UserId
                             where e.ReportingToId == UserId && e.IsActive == true
                             select new ShiftAllocation
                             {
                                 Name = e.FirstName + " " + e.LastName,
                                 EmpId = e.EmployeeId,
                                 UserId = sm.UserID,
                                 ShiftID = s.ShiftID,
                                 //FromDate = sm.FromDate,
                                 //ToDate = sm.ToDate,
                                 //FromTime = s.FromTime,
                                 ToTime = s.ToTime,
                                 ShiftName = s.ShiftDescription,
                                 IsActive = e.IsActive,
                                 ShiftMappingID = sm.ShiftMappingID
                             }).ToList();

            if (employees != null && employees.Count > 0)
                result.AddRange(employees);

            foreach (Int64 id in emp)
            {
                result.AddRange(GetShiftEmployees(context, id));
            }

            return result;
        }

        public Shifts GetShiftMasterWithId(Int64 shiftId)
        {
            var objShifts = new Shifts();

            using (var context = new NLTDDbContext())
            {
                if (shiftId > 0)
                {
                    objShifts = (from s in context.ShiftMaster
                                 where s.ShiftID == shiftId
                                 select new Shifts
                                 {
                                     ShiftId = s.ShiftID,
                                     ShiftName = s.ShiftDescription,
                                     FromTime = s.FromTime,
                                     ToTime = s.ToTime
                                 }).SingleOrDefault();

                    //objShifts = (from sm in context.ShiftMaster
                    //             where sm.ShiftID == shiftId
                    //select new Shifts
                    //{
                    //    ShiftId = sm.ShiftID,
                    //    ShiftName = sm.ShiftDescription,
                    //    FromTime = sm.FromTime,
                    //    CreatedBy = sm.CreatedBy
                    //}).SingleOrDefault();
                }

            }
            return objShifts;
        }
        
        public string SaveShiftMaster(int shiftId, string shiftName, TimeSpan fromTime, TimeSpan toTime, Int64 mgrId)
        {
            try
            {
                int isSaved = 0;

                using (var context = new NLTDDbContext())
                {
                    var objShiftMaster = context.ShiftMaster.Where(c => c.ShiftID == shiftId).SingleOrDefault();
                    if (objShiftMaster != null)
                    {
                        objShiftMaster = context.ShiftMaster.Where(c => c.ShiftID == shiftId).Single();
                        objShiftMaster.ShiftID = shiftId;
                        objShiftMaster.ShiftDescription = shiftName;
                        objShiftMaster.FromTime = fromTime;
                        objShiftMaster.ToTime = toTime;
                    }
                    else
                    {
                        objShiftMaster = new ShiftMaster();
                        objShiftMaster.ShiftDescription = shiftName;
                        objShiftMaster.FromTime = fromTime;
                        objShiftMaster.ToTime = toTime;
                        objShiftMaster.CreatedBy = mgrId;
                        objShiftMaster.CreatedDate = DateTime.Now;
                    }
                    context.ShiftMaster.AddOrUpdate(objShiftMaster);
                    isSaved = context.SaveChanges();

                }

                return isSaved > 0 ? "Saved" : "Failed";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void Dispose()
        {
            //Nothing to implement...
        }
    }
}