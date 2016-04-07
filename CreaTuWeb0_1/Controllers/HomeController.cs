using CreaTuWeb0_1.Models;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
             var msvm = new MisEntidadesViewModel();
            HashSet<MisEntidadesViewModel> hs = new HashSet<MisEntidadesViewModel>();
            hs.Add(msvm);
            IEnumerable<MisEntidadesViewModel> imsvm = hs;
            return View(imsvm);
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
