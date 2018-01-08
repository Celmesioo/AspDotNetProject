using DataLogic.Models;
using System.Linq;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        [HttpPost]
        public ActionResult Index(string input)
        {
            using (var context = new ApplicationDbContext())
            {
                var users  = context.Users.Where(x => (x.FirstName.Contains(input) || x.LastName.Contains(input)) && x.IsSearchAble == true).ToList();
                return View(users.ToList());
            }
        }
    }
}