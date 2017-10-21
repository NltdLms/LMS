using System.Web.Mvc;

namespace NLTD.EmployeePortal.LMS.Ux.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Dashboard");
        }
    }
}