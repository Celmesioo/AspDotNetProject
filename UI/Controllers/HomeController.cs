using DataLogic.Context;
using DataLogic.Models;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new HomeViewModel();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

       
        //Ladda bild
        public ActionResult Image(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = context.Users.Find(id);
                return File(user.ProfileImage, "image/png");
            }
            
        }
    }
}