using NLTD.EmployeePortal.LMS.Client;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Common.QueryModel;
using NLTD.EmployeePortal.LMS.Ux.AppHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NLTD.EmployeePortal.LMS.Ux.Controllers
{
    public class ShiftController : BaseController
    {
        // GET: TimeSheet

        public ActionResult TeamShiftAllocation()
        {
            ViewBag.RequestLevelPerson = "Team";
            IList<Shifts> shiftMaster = null;
            using (var client = new ShiftClient())
            {
                shiftMaster = client.GetShiftMaster();
            }
            return View("ShiftAllocation", shiftMaster);
        }

        public ActionResult AdminShiftAllocation()
        {
            ViewBag.RequestLevelPerson = "Admin";
            IList<Shifts> shiftMaster = null;

            using (var client = new ShiftClient())
            {
                shiftMaster = client.GetShiftMaster();
            }
            return View("ShiftAllocation", shiftMaster);
        }
        public ActionResult ShiftMaster()
        {
            ViewBag.Authorize = this.IsAuthorized;
            return View();
        }

        public ActionResult GetShiftDetail(string RequestMenuUser)
        {
            IList<ShiftAllocation> shiftAllocation = null;

            using (var client = new ShiftClient())
            {
                shiftAllocation = client.GetShiftAllocation(this.UserId, RequestMenuUser);
            }

            return PartialView("ShiftAllocationDetailPartial", shiftAllocation);
        }

        public ActionResult GetShiftDetailsForUsers(string RequestMenuUser)
        {
            List<ShiftEmployees> shiftEmployees = null;

            using (var client = new ShiftClient())
            {
                shiftEmployees = client.GetShiftDetailsForUsers(this.UserId, RequestMenuUser);
            }

            return PartialView("AddOrEditShiftAllocation", shiftEmployees);
        }

        public ActionResult GetShiftMasterDetail()
        {
            IList<Shifts> shiftMaster = null;

            using (var client = new ShiftClient())
            {
                shiftMaster = client.GetShiftMaster();
            }
            return PartialView("ShiftMasterDetailPartial", shiftMaster);
        }

        public ActionResult GetShiftMasterDetailwithId(Int64 shiftId)
        {
            // IList<Shifts> shiftMaster = null;

            Shifts objShifts = new Shifts();
            if (shiftId != 0)
            {
                using (var client = new ShiftClient())
                {
                    objShifts = client.GetShiftMasterWithId(shiftId);
                }
            }
            return PartialView("AddOrEditShift", objShifts);
        }

        public ActionResult SaveEmployeeShift(List<Int64> UserId, int Shift, DateTime FromDate, DateTime ToDate)
        {
            string result = "";
            if (ModelState.IsValid)
            {
                using (var client = new ShiftClient())
                {
                    result = client.SaveEmployeeShift(UserId, Shift, FromDate, ToDate, this.UserId);
                }
            }

            return Json(result);
        }

        public ActionResult SaveShiftMaster(int shiftId, string shiftName, TimeSpan fromTime, TimeSpan toTime)
        {
            string result = "";
            //TimeSpan tempToTime = toTime;
            //if (toTime < fromTime)
            //    tempToTime = toTime.Add(TimeSpan.FromHours(24));

            //TimeSpan duration = tempToTime - fromTime;
            //int workinghours = Math.Abs(Convert.ToInt32(duration.TotalHours));
            //if (workinghours == 9)
            //{
            if (ModelState.IsValid)
            {
                using (var client = new ShiftClient())
                {
                    result = client.SaveShiftMaster(shiftId, shiftName, fromTime, toTime, this.UserId);
                }
            }
            //}
            //else
            //{
            //    result = "Woking hours should be 9 hours only ";
            //}
            return Json(result);
        }



        public ActionResult EmployeeShiftAllocation()
        {
            ViewBag.RequestLevelPerson = "Admin";
            ManageTeamLeavesQueryModel qyMdl = new ManageTeamLeavesQueryModel();
            return View("EmployeeShiftAllocation", qyMdl);
        }

        public ActionResult TeamEmployeeShiftAllocation()
        {
            ViewBag.RequestLevelPerson = "Team";
            ManageTeamLeavesQueryModel qyMdl = new ManageTeamLeavesQueryModel();
            return View("EmployeeShiftAllocation", qyMdl);
        }

        public ActionResult GetEmployeeShiftDetails(string Name, string RequestMenuUser, string FromDate, string ToDate, string Shift)
        {
            EmpShift shiftDetail = null;

            if (Name != "")
            {
                Name = Name.Replace("|", " ");
            }

            using (var client = new ShiftClient())
            {
                long Userid = this.UserId;
                shiftDetail = client.GetEmployeeShiftDetails(Name, RequestMenuUser, Userid);
                ViewBag.FromDate = FromDate;
                ViewBag.ToDate = ToDate;
                ViewBag.Shift = Shift;
            }

            return PartialView("EmpShiftAllocationPartial", shiftDetail);
        }

        public ActionResult SaveIndividualEmployeeShift(DateTime FromDate, DateTime ToDate, int Shift, Int64 UserId)
        {
            string result = "";
            if (ModelState.IsValid)
            {
                using (var client = new ShiftClient())
                {
                    result = client.SaveIndividualEmployeeShift(UserId, Shift, FromDate, ToDate, this.UserId);
                }
            }

            return Json(result);
        }
    }
}