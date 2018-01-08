using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLogic.Models
{
    public class HomeViewModel
    {
        public List<ApplicationUser> RndUsers { get; set; }
        
        public bool IsEmpty { get; set; }

        public HomeViewModel()
        {
            IsEmpty = false;
            GetRandomUsers();
        }

        public void GetRandomUsers()
        {
            List<ApplicationUser> tempList = new List<ApplicationUser>();

            using (var context = new ApplicationDbContext())
            {
                int count = context.Users.Count();
                if (count == 0)
                {
                    IsEmpty = true;
                    return;
                }



                var users = context.Users.ToList();

                for (int i = 0; i < 5; i++)
                {
                    if (i == count - 1)
                        break;
                    
                    tempList.Add(users[i]);
                }
                RndUsers = tempList;
            }
        }
    }
}