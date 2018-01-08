using DataLogic.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Drawing;

namespace DataLogic.Context
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
        public DbSet<Post> Posts { get; set; }

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

            modelBuilder.Entity<ApplicationUser>().HasMany(x => x.Posts).WithRequired(x => x.Writer);

            base.OnModelCreating(modelBuilder);
        }
    }
}