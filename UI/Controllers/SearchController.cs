using DataLogic.Models;
using Microsoft.AspNet.Identity;
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
                var users  = context.Users.Where(x => x.FirstName.Contains(input) || x.LastName.Contains(input)).ToList();
                return View(users.ToList());
            }
        }
    }
}