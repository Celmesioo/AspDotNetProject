using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Web;

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

        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friendship>()
        .HasKey(x => new { x.User1Id, x.User2Id });

            modelBuilder.Entity<Friendship>().HasRequired(x => x.User1)
               .WithMany(y => y.FriendRequestsMade)
               .HasForeignKey(x => x.User1Id).WillCascadeOnDelete(false);

            modelBuilder.Entity<Friendship>()
                .HasRequired(x => x.User2)
                .WithMany(y => y.FriendRequestsAccepted)
                .HasForeignKey(x => x.User2Id);

            modelBuilder.Entity<ApplicationUser>().HasMany(x => x.posts).WithRequired(x => x.Writer);

            base.OnModelCreating(modelBuilder);
        }
    }

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
                    SecurityStamp = "ndaphn3naisjd903rmpAID03dn83nan09Niuhaklnf9"
                    
                };
                context.Users.Add(user);
            }
            context.SaveChanges();
            base.Seed(context);
        }

        private byte[] GetDefaultImage()
        {
            byte[] imageData = null;
            string path = HttpContext.Current.Server.MapPath("~/Content/img/defaultImage.png");
            imageData = File.ReadAllBytes(path);

            return imageData;
        }
    }
}