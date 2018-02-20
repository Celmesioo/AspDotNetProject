using DataLogic.Context;
using DataLogic.Entities;
using DataLogic.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers.api
{
    public class PostApiController : Controller
    {
        public JsonResult InsertPosts(string id)
        {
            var allUserPosts = new List<string>();
            using (var context = new ApplicationDbContext())
            {
                allUserPosts = context.Posts.Where(x => x.User.Id == id).Select(x => x.Content).ToList();
            }

            return Json(allUserPosts, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void AddPost(Post post, string id)
        {
            if(post.Content == null)
            {
                return;
            }
            using (var context = new ApplicationDbContext())
            {
                post.User = context.Users.Where(x => x.Id == id).SingleOrDefault();
                post.Writer = context.Users.Find(User.Identity.GetUserId());
                context.Posts.Add(post);
                context.SaveChanges();
            }
        }
    }
}