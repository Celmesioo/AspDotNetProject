using DataLogic.Context;
using DataLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLogic.Models
{
    public class LogInPartialViewModel
    {
        public ApplicationUser User { get; set; }
        public int PendingFriends { get; set; }
        public bool PendingFriendRequest { get; set; }

        public void SetUser(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                User = context.Users.Find(id);
            }
            PendingRequest();
        }

        private void PendingRequest()
        {
            if (User == null)
                return;

            SetFriendRequests();
            if (PendingFriends > 0)
                PendingFriendRequest = true;
            else
                PendingFriendRequest = false;
        }

        private void SetFriendRequests()
        {
            using (var context = new ApplicationDbContext())
            {
                PendingFriends = context.Friendships.Where(x => x.User2Id == User.Id && x.Status == 0).Select(c => c.User1Id).ToList().Count;
            }
        }
    }
}