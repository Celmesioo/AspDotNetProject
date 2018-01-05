using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
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

            base.OnModelCreating(modelBuilder);
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