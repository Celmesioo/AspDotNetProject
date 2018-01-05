using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataLogic.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Bio { get; set; }
        public byte[] ProfileImage { get; set; }

        public ICollection<Friendship> FriendRequestsMade { get; set; }

        public ICollection<Friendship> FriendRequestsAccepted { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class Friendship
    {
        public string User1Id { get; set; }
        public virtual ApplicationUser User1 { get; set; }

        public string User2Id { get; set; }
        public virtual ApplicationUser User2 { get; set; }

        public int? Status { get; set; }
    }
}