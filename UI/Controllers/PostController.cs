using DataLogic.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Create(string id)
        {
            var model = new Post();
            using (var context = new ApplicationDbContext())
            {
                model.Writer = context.Users.Find(User.Identity.GetUserId());
                model.User = context.Users.Find(id);
            }
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Post post, string id)
        {
            using (var context = new ApplicationDbContext())
            {
                post.User = context.Users.Find(id);
                post.Writer = context.Users.Find(User.Identity.GetUserId());
                context.Posts.Add(post);
                context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}