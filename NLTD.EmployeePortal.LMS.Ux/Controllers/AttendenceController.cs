using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLTD.EmployeePortal.LMS.Client;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Repository;
using NLTD.EmployeePortal.LMS.Ux.AppHelpers;

namespace NLTD.EmployeePortal.LMS.Ux.Controllers
{
    public class AttendenceController : BaseController
    {
        private IEmployeeAttendanceHelper EmployeeAttendanceHelper;
        // GET: Attentance
        public AttendenceController()
        {
            EmployeeAttendanceHelper = new EmplyeeAttendenceClient();
        }

        public ActionResult MyAttendence()
        {
            EmployeeProfile employeeProfileObj = (EmployeeProfile) Session["Profile"];
            ViewBag.RequestLevelPerson = "My";
            List<EmployeeAttendanceModel> employeeAttendanceModelObj =
                EmployeeAttendanceHelper.GetAttendence(employeeProfileObj.UserId);
            return View();
        }
        public ActionResult MyTeamAttendence()
        {
            ViewBag.RequestLevelPerson = "Team";
            return View("MyAttendence");
        }
        public ActionResult MyTimeSheet()
        {
            ViewBag.RequestLevelPerson = "My";
            return View("TimeSheet");
        }
        public ActionResult MyTeamTimeSheet()
        {
            ViewBag.RequestLevelPerson = "Team";
            return View("TimeSheet");
        }
        public ActionResult EmployeeTimeSheet()
        {
            ViewBag.RequestLevelPerson = "Admin";
            return View("TimeSheet");
        }
        public ActionResult EmployeeAttendence()
        {
            ViewBag.RequestLevelPerson = "Admin";
            return View("MyAttendence");
        }
    }
}