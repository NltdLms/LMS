using NLTD.EmployeePortal.LMS.Client;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
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
            return View("ShiftAllocation");
        }

        public ActionResult AdminShiftAllocation()
        {
            ViewBag.RequestLevelPerson = "Admin";
            return View("ShiftAllocation");
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

        public ActionResult GetShiftDetailsForUsers(Int64 ShiftMappingId, string RequestMenuUser)
        {
            AddShiftEmployee addShiftEmployee = null;

            using (var client = new ShiftClient())
            {
                addShiftEmployee = client.GetShiftDetailsForUsers(this.UserId, ShiftMappingId, RequestMenuUser);
            }

            return PartialView("AddOrEditShiftAllocation", addShiftEmployee);
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





        public ActionResult SaveEmployeeShift(List<Int64> UserId, int Shift, DateTime FromDate, DateTime ToDate, Int64? ShiftMappingID)
        {
            string result = "";
            if (ModelState.IsValid)
            {
                using (var client = new ShiftClient())
                {
                    long LoginUserId = this.UserId;
                    result = client.SaveEmployeeShift(UserId, Shift, FromDate, ToDate, this.UserId, ShiftMappingID);
                }
            }

            return Json(result);
        }

        public ActionResult SaveShiftMaster(int shiftId, string shiftName, TimeSpan fromTime, TimeSpan toTime)
        {
            string result = "";
            TimeSpan tempToTime= toTime;
            if (toTime < fromTime) 
                tempToTime = toTime.Add(TimeSpan.FromHours(24));

            TimeSpan duration = tempToTime - fromTime ;
            int workinghours = Math.Abs(Convert.ToInt32(duration.TotalHours));
            if (workinghours == 9)
            {
                if (ModelState.IsValid)
                {
                    using (var client = new ShiftClient())
                    {
                        result = client.SaveShiftMaster(shiftId, shiftName, fromTime, toTime, this.UserId);
                    }
                }
            }
            else
            {
                result = "Woking hours should be 9 hours only ";
            }
        return Json(result);
        }
    }
}