using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DataLogic.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
    }

    public class DataInitilizer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var store = new UserStore<ApplicationUser>(context);
            var manager = new ApplicationUserManager(store);

            for (int i = 0; i < 10; i++)
            {
                var user = new ApplicationUser
                {
                    UserName = $"user{i}@mail.com",
                    Email = $"user{i}@mail.com",
                    FirstName = $"User{i}",
                    LastName = $"Lastname{i}"
                };
                manager.CreateAsync(user, "123").Wait();
            }


            base.Seed(context);
        }
    }
}