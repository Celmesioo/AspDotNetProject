using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using DataLogic.Models;
using DataLogic.Repository;
using DataLogic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace UI.Controllers
{
    [Authorize]
    public class AccountController : ApplicationBaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        public ActionResult Edit(string id)
        {
            ApplicationUser user = new ApplicationUser();
            using (var context = new ApplicationDbContext())
            {
                user = context.Users.Find(id);
                return View(user);
            }
        }

        public ActionResult SaveEdit(ApplicationUser model, string id)
        {

            ApplicationUser user = new ApplicationUser();
            using (var context = new ApplicationDbContext())
            {
                user = context.Users.Find(id);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Bio = model.Bio;

                context.SaveChanges();
            }

            return RedirectToAction("Details", new { id });
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Exclude = "ProfileImage")]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // To convert the user uploaded Photo as Byte Array before save to DB
                byte[] imageData = null;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase poImgFile = Request.Files["ProfileImage"];

                    using (var binary = new BinaryReader(poImgFile.InputStream))
                    {
                        imageData = binary.ReadBytes(poImgFile.ContentLength);
                    }
                }
                if (imageData.Count() == 0)
                {
                    imageData = DataInitilizer.GetDefaultImage();
                }
              

                var user = new ApplicationUser {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ProfileImage = imageData
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
            return View(model);
        }

        public ActionResult Details( string id)
        {
            UserSiteModel model = new UserSiteModel();
            using (var context = new ApplicationDbContext())
            {
                model.User = context.Users.Find(id);
                if(id != User.Identity.GetUserId())
                {
                    var userID = User.Identity.GetUserId();
                    var result = context.Friendships.Where(x => x.User1Id == userID && x.User2Id == id || x.User1Id == id && x.User2Id == userID).FirstOrDefault();
                    if (result != null)
                    {
                        model.AreFriends = true;
                    }
                }
                var friendlist1 = context.Friendships.Where(x => x.User2Id == id && x.Status == (StatusCode)1).Select(c => c.User1Id).ToList();
                var friendlist2 = context.Friendships.Where(x => x.User1Id == id && x.Status == (StatusCode)1).Select(c => c.User2Id).ToList();
                var allFriends = friendlist1.Concat(friendlist2);
                model.Friends = context.Users.Where(x => allFriends.Contains(x.Id)).ToList();

                return View(model);
            }
        }

        public ActionResult SendRequest(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                var userId = User.Identity.GetUserId();
                Friendship f = new Friendship();
                f.User1Id = userId;
                f.User2Id = id;
                f.Status = 0;
                context.Friendships.Add(f);
                context.SaveChanges();
            }
            return RedirectToAction("Details", new { id });
        }

        public ActionResult RequestAccepted(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                var loggedInUser = User.Identity.GetUserId();
                var result = context.Friendships.Where(x => x.User1Id == id && x.User2Id == loggedInUser).First();
                result.Status = (StatusCode)1;
                context.SaveChanges();
            }
            return RedirectToAction("Details", new { id});
        }

        public ActionResult RequestDeclined(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                var loggedInUser = User.Identity.GetUserId();
                var result = context.Friendships.Where(x => x.User1Id == id && x.User2Id == loggedInUser).First();
                context.Friendships.Remove(result);
                context.SaveChanges();
            }
            return RedirectToAction("Details", new { id });
        }

        public ActionResult InspectRequests()
        {
            UserSiteModel model = new UserSiteModel();
            
            using (var context = new ApplicationDbContext())
            {
                model.User = context.Users.Find(User.Identity.GetUserId());
                var requests = context.Friendships.Where(x => x.User2Id == model.User.Id && x.Status == 0).Select(c => c.User1Id).ToList();
                model.Friends = context.Users.Where(x => requests.Contains(x.Id)).ToList();
            }
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}