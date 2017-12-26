using DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                var users  = context.Users.Where(x => x.UserName.Contains(input)).ToList();
                return View(users.ToList());
            }
        }
    }
}