using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLogic.Models
{
    public class UserSiteModel
    {
        public ApplicationUser User { get; set; }
        public List<ApplicationUser> Friends { get; set; }
        public bool AreFriends { get; set; }
    }
}