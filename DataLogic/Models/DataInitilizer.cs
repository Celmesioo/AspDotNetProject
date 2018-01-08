using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.IO;
using System.Web;

namespace DataLogic.Models
{
    public class DataInitilizer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword("123");

            for (int i = 0; i < 100; i++)
            {
                var user = new ApplicationUser
                {
                    UserName = $"User{i}@mail.com",
                    Email = $"User{i}@mail.com",
                    PasswordHash = password,
                    FirstName = "User" + i,
                    LastName = "Lastname" + i,
                    ProfileImage = GetDefaultImage(),
                    SecurityStamp = "ndaphn3naisjd903rmpAID03dn83nan09Niuhaklnf9",
                    IsSearchAble = true
                    
                    
                };
                context.Users.Add(user);
            }
            context.SaveChanges();
            base.Seed(context);
        }

        public static byte[] GetDefaultImage()
        {
            byte[] imageData = null;
            string path = HttpContext.Current.Server.MapPath("~/Content/img/defaultImage.png");
            imageData = File.ReadAllBytes(path);

            return imageData;
        }
    }
}