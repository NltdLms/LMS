using NLTD.EmploeePortal.LMS.Dac.DbModel;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NLTD.EmploeePortal.LMS.Dac.Dac
{
    public class EmployeeDac : IEmployeeHelper
    {
        public EmployeeProfile GetEmployeeProfile(Int64 userId)
        {
            
            EmployeeProfile profile = null;
            try
            {
                using (var context = new NLTDDbContext())
                {
                    profile = (from employee in context.Employee
                               join rt in context.EmployeeRole on employee.EmployeeRoleId equals rt.RoleId                               
                               where employee.UserId== userId
                               select new EmployeeProfile
                               {
                                   EmailAddress = employee.EmailAddress,
                                   EmployeeId = employee.EmployeeId,
                                   FirstName = employee.FirstName,
                                   LastName = employee.LastName,
                                   Gender = employee.Gender,
                                   OfficeHolodayId = employee.OfficeHolodayId,
                                   OfficeId=employee.OfficeId,
                                   LocationText = "",
                                   MobileNumber = employee.MobileNumber,
                                   ReportedToId = employee.ReportingToId,                                   
                                   RoleId = employee.EmployeeRoleId,
                                   RoleText = rt.Role,
                                   UserId = employee.UserId,
                                   Avatar = employee.AvatarUrl,                                  
                                   //IsHandleMembers = employee.IsHandleMembers,                                   
                                   LogonId = employee.LoginId,
                                   IsActive=employee.IsActive

                               }).FirstOrDefault();
                    if (profile != null)
                    {
                        if (profile.ReportedToId != null)
                        {
                            var repName = context.Employee.Where(x => x.UserId == profile.ReportedToId).FirstOrDefault();
                            if (repName != null)
                            {
                                profile.ReportedToName = repName.FirstName + " " + repName.LastName;
                            }
                        }
                        var weekOffs = (from e in context.Employee
                                        join wo in context.EmployeeWeekOff on e.UserId equals wo.UserId
                                        join w in context.DayOfWeek on wo.DaysOfWeekId equals w.DaysOfWeekId
                                        where e.UserId == profile.UserId
                                        select new { Day = w.Day }
                                      ).ToList();


                        if(weekOffs.Count>0)
                        {
                            foreach (var item in weekOffs)
                            {
                                switch (item.Day.ToUpper())
                                {
                                    case "SUNDAY":
                                        profile.Sunday = true;
                                        break;
                                    case "MONDAY":
                                        profile.Monday = true;
                                        break;
                                    case "TUESDAY":
                                        profile.Tuesday = true;
                                        break;
                                    case "WEDNESDAY":
                                        profile.Wednesday = true;
                                        break;
                                    case "THURSDAY":
                                        profile.Thursday = true;
                                        break;
                                    case "FRIDAY":
                                        profile.Friday = true;
                                        break;
                                    case "SATURDAY":
                                        profile.Saturday = true;
                                        break;
                                    default:
                                        break;
                                }
                            }

                        }

                    }
                }
            }
            catch(Exception ex) { throw; }
            return profile;
        }
        IList<Int64> GetEmployeesReporting(long leadId)
        {
            var result = new List<Int64>();
            using (var context = new NLTDDbContext())
            {
                var employees = context.Employee
                                       .Where(e => e.ReportingToId == leadId && e.IsActive == true)
                                       .ToList();
                foreach (var employee in employees)
                {
                    result.Add(employee.UserId);
                    result.AddRange(GetEmployeesReporting(employee.UserId));
                }
            }
            return result;
        }
        public IList<ViewEmployeeProfileModel> GetTeamProfiles(Int64 userId, bool onlyReportedToMe, string name, string requestMenuUser, bool hideInactiveEmp)
        {
            IList<ViewEmployeeProfileModel> retModel = new List<ViewEmployeeProfileModel>();            
            IList<Int64> empList = GetEmployeesReporting(userId);
            ViewEmployeeProfileModel profile = null;
            using (var context = new NLTDDbContext())
            {
                var ids = (from e in context.Employee
                           //where e.ReportingToId == userId
                           orderby e.FirstName
                           select new { userId = e.UserId,iactive=e.IsActive,reportingToId=e.ReportingToId,name=e.FirstName+" "+e.LastName }
                         ).ToList();


                if (requestMenuUser == "My")
                {
                    ids = ids.Where(x => x.userId == userId).ToList();
                }
                else
                {
                    var leadinfo = (from emp in context.Employee
                                    join role in context.EmployeeRole on emp.EmployeeRoleId equals role.RoleId
                                    where emp.UserId == userId
                                    select new { RoleName = role.Role }).FirstOrDefault();
                    if (leadinfo != null)
                    {
                        if (requestMenuUser == "Admin")
                        {
                            if (leadinfo.RoleName.ToUpper() == "ADMIN" || leadinfo.RoleName.ToUpper() == "HR")
                            {
                                if (hideInactiveEmp == true)
                                {
                                    ids = ids.Where(x => x.iactive == true).ToList();
                                }
                                else
                                {
                                    ids = ids.ToList();
                                }
                            }
                        }
                        else if (requestMenuUser == "Team")
                        {
                            if (onlyReportedToMe)
                            {
                                ids = ids.Where(t => t.reportingToId == userId && t.iactive==true).ToList();
                            }
                            else
                            {
                                ids = ids.Where(t => empList.Contains(t.userId) && t.iactive==true).ToList();
                            }
                        }
                    }
                }
                
                if (name != null)
                {
                    if (name.Trim() != "")
                    {
                        if (ids.Count > 0)
                        {
                            ids = ids.Where(x => x.name.ToUpper().Trim() == name.ToUpper().Trim()).ToList();
                        }
                    }
                }

                foreach (var memId in ids)
                {

                    profile = (from employee in context.Employee
                               join rt in context.EmployeeRole on employee.EmployeeRoleId equals rt.RoleId
                               join o in context.OfficeLocation on employee.OfficeId equals o.OfficeId
                               join h in context.OfficeHoliday on employee.OfficeHolodayId equals h.OfficeHolodayId
                               where employee.UserId == memId.userId
                               select new ViewEmployeeProfileModel
                               {
                                   EmailAddress = employee.EmailAddress,
                                   EmployeeId = employee.EmployeeId,
                                   FirstName = employee.FirstName,
                                   LastName = employee.LastName,
                                   Name=employee.FirstName+" "+employee.LastName,
                                   Gender = employee.Gender == "M" ? "Male" : "Female",
                                   HolidayOfficeId = employee.OfficeHolodayId,
                                   OfficeName = o.OfficeName,
                                   MobileNumber = employee.MobileNumber,
                                   ReportedToId = employee.ReportingToId,
                                   RoleText = rt.Role,
                                   UserId = employee.UserId,
                                   LogonId = employee.LoginId,
                                   IsActive = employee.IsActive

                               }).FirstOrDefault();
                    if (profile != null)
                    {
                        var repName = context.Employee.Where(x => x.UserId == profile.ReportedToId).FirstOrDefault();
                        if (repName != null)
                        {
                            profile.ReportedToName = repName.FirstName + " " + repName.LastName;
                        }
                        profile.HolidayOfficeName = context.OfficeLocation.Where(x => x.OfficeId == profile.HolidayOfficeId).FirstOrDefault().OfficeName;
                        var weekOffs = (from e in context.Employee
                                        join wo in context.EmployeeWeekOff on e.UserId equals wo.UserId
                                        join w in context.DayOfWeek on wo.DaysOfWeekId equals w.DaysOfWeekId
                                        where e.UserId == profile.UserId
                                        select new { Day = w.Day }
                                      ).ToList();


                        if (weekOffs.Count > 0)
                        {
                            foreach (var item in weekOffs)
                            {
                                switch (item.Day.ToUpper())
                                {
                                    case "SUNDAY":
                                        profile.Sunday = true;
                                        break;
                                    case "MONDAY":
                                        profile.Monday = true;
                                        break;
                                    case "TUESDAY":
                                        profile.Tuesday = true;
                                        break;
                                    case "WEDNESDAY":
                                        profile.Wednesday = true;
                                        break;
                                    case "THURSDAY":
                                        profile.Thursday = true;
                                        break;
                                    case "FRIDAY":
                                        profile.Friday = true;
                                        break;
                                    case "SATURDAY":
                                        profile.Saturday = true;
                                        break;
                                    default:
                                        break;
                                }
                            }

                        }

                    }

                    retModel.Add(profile);
                }
            }

            return retModel;
        }

        public ViewEmployeeProfileModel ViewEmployeeProfile(Int64 userId)
        {
            
            ViewEmployeeProfileModel profile = null;
            try
            {
                using (var context = new NLTDDbContext())
                {
                    profile = (from employee in context.Employee
                               join rt in context.EmployeeRole on employee.EmployeeRoleId equals rt.RoleId
                               join o in context.OfficeLocation on employee.OfficeId equals o.OfficeId
                               join h in context.OfficeHoliday on employee.OfficeHolodayId equals h.OfficeHolodayId
                               where employee.UserId == userId
                               select new ViewEmployeeProfileModel
                               {
                                   EmailAddress = employee.EmailAddress,
                                   EmployeeId = employee.EmployeeId,
                                   FirstName = employee.FirstName,
                                   LastName = employee.LastName,
                                   Gender = employee.Gender == "M" ? "Male" : "Female",
                                   HolidayOfficeId = employee.OfficeHolodayId,
                                   OfficeName = o.OfficeName,                                   
                                   MobileNumber = employee.MobileNumber,
                                   ReportedToId = employee.ReportingToId,                                   
                                   RoleText = rt.Role,
                                   UserId = employee.UserId, 
                                   LogonId = employee.LoginId,
                                   IsActive = employee.IsActive

                               }).FirstOrDefault();
                    if (profile != null)
                    {
                        var repName = context.Employee.Where(x => x.UserId == profile.ReportedToId).FirstOrDefault();
                        if (repName != null)
                        {
                            profile.ReportedToName = repName.FirstName + " " + repName.LastName;
                        }
                        profile.HolidayOfficeName = context.OfficeLocation.Where(x => x.OfficeId == profile.HolidayOfficeId).FirstOrDefault().OfficeName;
                        var weekOffs = (from e in context.Employee
                                        join wo in context.EmployeeWeekOff on e.UserId equals wo.UserId
                                        join w in context.DayOfWeek on wo.DaysOfWeekId equals w.DaysOfWeekId
                                        where e.UserId == profile.UserId
                                        select new { Day = w.Day }
                                      ).ToList();


                        if (weekOffs.Count > 0)
                        {
                            foreach (var item in weekOffs)
                            {
                                switch (item.Day.ToUpper())
                                {
                                    case "SUNDAY":
                                        profile.Sunday = true;
                                        break;
                                    case "MONDAY":
                                        profile.Monday = true;
                                        break;
                                    case "TUESDAY":
                                        profile.Tuesday = true;
                                        break;
                                    case "WEDNESDAY":
                                        profile.Wednesday = true;
                                        break;
                                    case "THURSDAY":
                                        profile.Thursday = true;
                                        break;
                                    case "FRIDAY":
                                        profile.Friday = true;
                                        break;
                                    case "SATURDAY":
                                        profile.Saturday = true;
                                        break;
                                    default:
                                        break;
                                }
                            }

                        }

                    }
                }
            }
            catch (Exception ex) { throw; }
            return profile;
        }

        public EmployeeProfile GetEmployeeLoginProfile(string LogonId)
        {
            EmployeeProfile profile = null;
            try
            {
                using (var context = new NLTDDbContext())
                {
                    profile = (from employee in context.Employee
                               join rt in context.EmployeeRole on employee.EmployeeRoleId equals rt.RoleId
                               where employee.LoginId == LogonId && employee.IsActive==true
                               select new EmployeeProfile
                               {
                                   EmailAddress = employee.EmailAddress,
                                   EmployeeId = employee.EmployeeId,
                                   FirstName = employee.FirstName,
                                   LastName = employee.LastName,
                                   OfficeId = employee.OfficeId,
                                   ReportedToId = employee.ReportingToId,
                                   RoleId=employee.EmployeeRoleId,
                                   RoleText = rt.Role,
                                   UserId = employee.UserId,                                                                   
                                   LogonId = employee.LoginId,
                                   IsActive = employee.IsActive
                               }).FirstOrDefault();
                    if (profile != null)
                    {
                        if (profile.ReportedToId != null)
                        {
                            var repName = context.Employee.Where(x => x.UserId == profile.ReportedToId).FirstOrDefault();
                            if (repName != null)
                            {
                                profile.ReportedToName = repName.FirstName + " " + repName.LastName;
                            }
                        }
                        var teamReporting = context.Employee.Where(x => x.ReportingToId == profile.UserId && x.IsActive==true).FirstOrDefault();
                        if (teamReporting != null)
                        {
                            profile.IsHandleMembers = true;
                        }
                        else
                        {
                            profile.IsHandleMembers = false;
                        }
                    }
                }
            }
            catch  { throw; }
            return profile;
        }
        public List<DropDownItem> GetReportToList(Int64 OfficeId)
        {
            using (var context = new NLTDDbContext())
            {
                List<DropDownItem> ReportToPersons = (from employee in context.Employee
                                                      join lead in context.Employee on employee.ReportingToId equals lead.UserId
                                                      where employee.OfficeId == OfficeId
                                                      select new DropDownItem
                                                      {
                                                          Key = lead.UserId.ToString(),
                                                          Value = lead.FirstName + " " + lead.LastName
                                                      }).ToList();

                ReportToPersons = ReportToPersons.GroupBy(x => x.Key)
                   .Select(grp => grp.First())
                   .ToList();
                
                return ReportToPersons;
            }
        }
        public void Dispose()
        {
            //Nothing to dispose...
        }
        public string UpdateEmployeeProfile(EmployeeProfile profile, Int64 ModifiedBy)
        {
            //check if ModifiedBy user is HR


            Employee employee = null;
            int isSaved = 0;
            string retMsg = string.Empty;
            bool noChanges = false;
            bool isAuthorizedRole = false;
            string remarks = string.Empty;
            using (var context = new NLTDDbContext())
            {
                var isAuthorized = (from e in context.Employee
                                    join r in context.EmployeeRole on e.EmployeeRoleId equals r.RoleId
                                    where e.UserId==ModifiedBy
                                    select new { r.Role }
                                  ).FirstOrDefault();

                if (isAuthorized != null)
                {
                    if (isAuthorized.Role.ToUpper() == "HR")
                        isAuthorizedRole = true;
                }
                if (isAuthorizedRole)
                {
                    employee = context.Employee.Where(e => e.EmployeeId.ToUpper() == profile.EmployeeId).FirstOrDefault();
                    if (profile.Mode == "Add")
                    {
                        if (employee != null)
                        {
                            return "Duplicate";
                        }
                    }



                    if (profile.Mode == "Add")
                    {
                        var isCorpIdExits = context.Employee.Where(e => e.LoginId == profile.LogonId.ToUpper()).FirstOrDefault();
                        if (isCorpIdExits != null)
                        {                            
                            return "DupCorp";
                        }
                    }
                    else
                    {
                        var isCorpIdExits = context.Employee.Where(e => e.LoginId == profile.LogonId.ToUpper() && e.EmployeeId != employee.EmployeeId).FirstOrDefault();
                        if (isCorpIdExits != null)
                        {
                           
                                return "DupCorp";
                            
                        }
                    }

                    if (employee == null)
                    {
                        remarks = "#LogonId" + "^" + profile.LogonId;
                        remarks = remarks + "#EmployeeId" + "^" + profile.EmployeeId;
                        remarks = remarks + "#OfficeId" + "^" + profile.OfficeId;
                        remarks = remarks + "#IsActive" + "^" + profile.IsActive;
                        remarks = remarks + "#FirstName" + "^" + profile.FirstName;
                        remarks = remarks + "#LastName" + "^" + profile.LastName;
                        remarks = remarks + "#Gender" + "^" + profile.Gender;
                        remarks = remarks + "#EmailAddress" + "^" + profile.EmailAddress;
                        remarks = remarks + "#MobileNumber" + "^" + profile.MobileNumber;
                        remarks = remarks + "#RoleId" + "^" + profile.RoleId;
                        remarks = remarks + "#ReportedToId" + "^" + profile.ReportedToId;
                        remarks = remarks + "#OfficeHolodayId" + "^" + profile.OfficeHolodayId;

                        employee = new Employee();
                        employee.LoginId = profile.LogonId.ToUpper();
                        employee.EmployeeId = profile.EmployeeId;
                        employee.OfficeId = profile.OfficeId;
                        employee.IsActive = profile.IsActive;
                        employee.FirstName = profile.FirstName;
                        employee.LastName = profile.LastName;
                        employee.Gender = profile.Gender;
                        employee.MobileNumber = profile.MobileNumber;
                        employee.EmailAddress = profile.EmailAddress;
                        employee.EmployeeRoleId = profile.RoleId;
                        employee.ReportingToId = profile.ReportedToId;
                        //employee.IsHandleMembers = profile.IsHandleMembers;
                        employee.OfficeHolodayId = profile.OfficeHolodayId;
                        employee.ModifiedBy = -1;
                        employee.CreatedBy = ModifiedBy;
                        employee.CreatedOn = System.DateTime.Now;
                        employee.ModifiedOn = System.DateTime.Now;

                        context.Employee.Add(employee);
                        isSaved = context.SaveChanges();
                        if (isSaved > 0)
                        {
                            List<String> lstNewWeekOffs = new List<string>();
                            if (profile.Sunday)
                                lstNewWeekOffs.Add("Sunday");
                            if (profile.Monday)
                                lstNewWeekOffs.Add("Monday");
                            if (profile.Tuesday)
                                lstNewWeekOffs.Add("Tuesday");
                            if (profile.Wednesday)
                                lstNewWeekOffs.Add("Wednesday");
                            if (profile.Thursday)
                                lstNewWeekOffs.Add("Thursday");
                            if (profile.Friday)
                                lstNewWeekOffs.Add("Friday");
                            if (profile.Saturday)
                                lstNewWeekOffs.Add("Saturday");
                            Int64 dayOfWeek = 0;
                            EmployeeWeekOff ew;
                            foreach (var item in lstNewWeekOffs)
                            {
                                dayOfWeek = context.DayOfWeek.Where(x => x.Day.ToUpper() == item.ToUpper()).FirstOrDefault().DaysOfWeekId;
                                ew = new EmployeeWeekOff();
                                ew.UserId = employee.UserId;
                                ew.DaysOfWeekId = dayOfWeek;
                                ew.ModifiedBy = -1;
                                ew.ModifiedOn = System.DateTime.Now;
                                ew.CreatedBy = ModifiedBy;
                                ew.CreatedOn = System.DateTime.Now;
                                context.EmployeeWeekOff.Add(ew);
                                isSaved = context.SaveChanges();
                            }
                        }
                        if (isSaved > 0)
                        {
                            EmployeeTransactionHistory hist = new EmployeeTransactionHistory();
                            hist.UserId = employee.UserId;
                            hist.TransactionDate = System.DateTime.Now;
                            hist.TransactionType = "Insert";
                            hist.TransactionBy = ModifiedBy;
                            hist.Remarks = remarks;
                            context.EmployeeTransactiontHistory.Add(hist);
                            isSaved = context.SaveChanges();

                        }
                        if (isSaved > 0)
                            retMsg = "Saved";
                        else
                        {
                            if (retMsg == "")
                                retMsg = "Failed";
                        }
                        return retMsg;
                    }
                    else
                    {

                        Employee oldEmpData = new Employee();
                        oldEmpData = context.Employee.Where(x => x.UserId == profile.UserId).FirstOrDefault();

                        if (profile.LogonId != oldEmpData.LoginId)
                            remarks = "#LogonId" + "^" + profile.LogonId;

                        if (profile.EmployeeId != oldEmpData.EmployeeId)
                            remarks = remarks + "#EmployeeId" + "^" + profile.EmployeeId;

                        if (profile.IsActive != oldEmpData.IsActive)
                            remarks = remarks + "#IsActive" + "^" + profile.IsActive;

                        if (profile.ReportedToId != oldEmpData.ReportingToId)
                            remarks = remarks + "#ReportedToId" + "^" + profile.ReportedToId;

                        if (profile.FirstName != oldEmpData.FirstName)
                            remarks = remarks + "#FirstName" + "^" + profile.FirstName;

                        if (profile.LastName != oldEmpData.LastName)
                            remarks = remarks + "#LastName" + "^" + profile.LastName;

                        if (profile.Gender != oldEmpData.Gender)
                            remarks = remarks + "#Gender" + "^" + profile.Gender;

                        if (profile.MobileNumber != oldEmpData.MobileNumber)
                            remarks = remarks + "#MobileNumber" + "^" + profile.MobileNumber;

                        if (profile.EmailAddress != oldEmpData.EmailAddress)
                            remarks = remarks + "#EmailAddress" + "^" + profile.EmailAddress;

                        if (profile.OfficeHolodayId != oldEmpData.OfficeHolodayId)
                            remarks = remarks + "#OfficeHolodayId" + "^" + profile.OfficeHolodayId;

                        if (profile.RoleId != oldEmpData.EmployeeRoleId)
                            remarks = remarks + "#RoleId" + "^" + profile.RoleId;


                        employee.OfficeId = profile.OfficeId;
                        employee.LoginId = profile.LogonId;
                        employee.EmployeeId = profile.EmployeeId;
                        employee.IsActive = profile.IsActive;
                        employee.ReportingToId = profile.ReportedToId;
                        employee.FirstName = profile.FirstName;
                        employee.LastName = profile.LastName;
                        employee.Gender = profile.Gender;
                        employee.MobileNumber = profile.MobileNumber;
                        employee.EmailAddress = profile.EmailAddress;
                        // employee.IsHandleMembers = profile.IsHandleMembers;
                        employee.OfficeHolodayId = profile.OfficeHolodayId;
                        employee.EmployeeRoleId = profile.RoleId;
                        if (remarks == "") { noChanges = true; }
                        else
                        {
                            noChanges = false;
                            employee.ModifiedOn = System.DateTime.Now;
                            employee.ModifiedBy = ModifiedBy;
                            isSaved = context.SaveChanges();
                        }

                        string isSameWeekoff = "";

                        var existingWeekOffs = (from wo in context.EmployeeWeekOff
                                                join w in context.DayOfWeek on wo.DaysOfWeekId equals w.DaysOfWeekId
                                                where wo.UserId == employee.UserId
                                                select new { Day = w.Day, EmpWeekOffId = wo.EmployeeWeekOffId }
                                              ).ToList();

                        List<String> lstNewWeekOffs = new List<string>();
                        if (profile.Sunday)
                            lstNewWeekOffs.Add("Sunday");
                        if (profile.Monday)
                            lstNewWeekOffs.Add("Monday");
                        if (profile.Tuesday)
                            lstNewWeekOffs.Add("Tuesday");
                        if (profile.Wednesday)
                            lstNewWeekOffs.Add("Wednesday");
                        if (profile.Thursday)
                            lstNewWeekOffs.Add("Thursday");
                        if (profile.Friday)
                            lstNewWeekOffs.Add("Friday");
                        if (profile.Saturday)
                            lstNewWeekOffs.Add("Saturday");
                        Int64 dayOfWeek = 0;
                        Dictionary<Int64, string> dicttUpdateExisting = new Dictionary<long, string>();

                        bool deleteAndAdd = false;
                        var prepareList = lstNewWeekOffs;
                        if (existingWeekOffs.Count == lstNewWeekOffs.Count)
                        {
                            if (existingWeekOffs.Count == 0 && lstNewWeekOffs.Count == 0) { }
                            else
                            {
                                //verify if they are same
                                foreach (var item in existingWeekOffs)
                                {
                                    if (lstNewWeekOffs.Where(x => x == item.Day).Any())
                                    {
                                        if (isSameWeekoff == "")
                                        {
                                            isSameWeekoff = "Yes";
                                            
                                        }


                                    }
                                    else
                                    {
                                        isSameWeekoff = "No";
                                        dicttUpdateExisting.Add(item.EmpWeekOffId, prepareList.Where(x => x != item.Day).FirstOrDefault());
                                        prepareList.Remove(dicttUpdateExisting[item.EmpWeekOffId]);

                                    }

                                }
                            }
                        }
                        else if (lstNewWeekOffs.Count > existingWeekOffs.Count)
                        {
                            deleteAndAdd = true;
                        }
                        else if (lstNewWeekOffs.Count < existingWeekOffs.Count)
                        {
                            deleteAndAdd = true;
                        }
                        if (isSameWeekoff != "Yes")
                        {
                            noChanges = false;
                            remarks = remarks + "#WeeklyOff" + "^" + "Changed";
                            if (deleteAndAdd)
                            {
                                var deleteOldOffs = context.EmployeeWeekOff.Where(x => x.UserId == employee.UserId).ToList();
                                if (deleteOldOffs.Any())
                                {
                                    context.EmployeeWeekOff.RemoveRange(deleteOldOffs);
                                    context.SaveChanges();
                                }
                                EmployeeWeekOff ew;
                                foreach (var item in lstNewWeekOffs)
                                {
                                    dayOfWeek = context.DayOfWeek.Where(x => x.Day.ToUpper() == item.ToUpper()).FirstOrDefault().DaysOfWeekId;
                                    ew = new EmployeeWeekOff();
                                    ew.UserId = employee.UserId;
                                    ew.DaysOfWeekId = dayOfWeek;
                                    ew.ModifiedBy = -1;
                                    ew.ModifiedOn = System.DateTime.Now;
                                    ew.CreatedBy = ModifiedBy;
                                    ew.CreatedOn = System.DateTime.Now;
                                    context.EmployeeWeekOff.Add(ew);
                                    isSaved = context.SaveChanges();
                                }
                            }
                            else
                            {
                                foreach (var item in dicttUpdateExisting)
                                {
                                    var updtExist = context.EmployeeWeekOff.Where(x => x.EmployeeWeekOffId == item.Key).FirstOrDefault();
                                    dayOfWeek = context.DayOfWeek.Where(x => x.Day.ToUpper() == item.Value.ToUpper()).FirstOrDefault().DaysOfWeekId;
                                    if (updtExist != null)
                                    {
                                        updtExist.DaysOfWeekId = dayOfWeek;
                                        updtExist.ModifiedBy = ModifiedBy;
                                        updtExist.ModifiedOn = System.DateTime.Now;
                                        isSaved = context.SaveChanges();
                                    }

                                }
                            }
                        }
                        
                        if (isSaved > 0)
                        {
                            EmployeeTransactionHistory hist = new EmployeeTransactionHistory();
                            hist.UserId = profile.UserId;
                            hist.TransactionDate = System.DateTime.Now;
                            hist.TransactionType = "Update";
                            hist.TransactionBy = ModifiedBy;
                            hist.Remarks = remarks;
                            context.EmployeeTransactiontHistory.Add(hist);
                            isSaved = context.SaveChanges();
                           
                        }
                        if (isSaved > 0)
                            retMsg = "Saved";
                        else if (noChanges) {
                            retMsg = "noChanges";
                        }
                        else 
                        {
                            if (retMsg == "")
                                retMsg = "Failed";
                        }

                        return retMsg;
                    }

                }
                else
                {
                    return "NeedRole";
                }
            }
        }
        public long GetEmployeeId(string LogonId)
        {
            using (var context = new NLTDDbContext())
            {
                var user = context.Employee.Where(e => e.LoginId.ToUpper() == LogonId.ToUpper()).FirstOrDefault();
                if (user == null)
                    return 0;
                else
                    return user.UserId;
            }
        }
        public long GetUserId(string name)
        {
            using (var context = new NLTDDbContext())
            {
                var empPrf = context.Employee.Where(x => string.Concat(x.FirstName, " ", x.LastName).ToUpper() == name.ToUpper()).FirstOrDefault();
                if (empPrf == null)
                    return 0;
                else
                    return empPrf.UserId;
            }
        }
        public string ReportingToName(Int64 userId)
        {
            using (var context = new NLTDDbContext())
            {
                var user = (from e in context.Employee
                            join r in context.Employee on e.ReportingToId equals r.UserId
                            where e.UserId == userId
                            select new { FirstName = r.FirstName, LastName = r.LastName }
                            ).FirstOrDefault();
                if (user == null)
                    return "";
                else
                    return user.FirstName + " " + user.LastName;
            }
        }


    }
}
