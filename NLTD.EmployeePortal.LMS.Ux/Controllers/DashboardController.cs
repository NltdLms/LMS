using NLTD.EmployeePortal.LMS.Client;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Ux.AppHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NLTD.EmployeePortal.LMS.Ux.Controllers
{
    public class DashboardController : BaseController
    {
        public ActionResult Index()
        {
            DashBoardModel dbMdl = new DashBoardModel();             
            
            using (var client = new LeaveClient())
            {
                dbMdl= client.GetDashboardData(UserId);
            }
            dbMdl.IsLMSApprover = IsLMSApprover;          
          
            return View(dbMdl);
        }
        
        public ActionResult LoadPendingCount()
        {
            int count = 0;
            using (var client = new LeaveClient())
            {
                count = client.GetPendingApprovalCount(UserId);
            }
            return PartialView("PendingApprovalCountPartial", count);
        }


    }
}