using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLogic.Models
{
    public class Post
    {
        public int ID { get; set; }
        public string Content { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser Writer { get; set; }
    }
}