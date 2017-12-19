using DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DataLogic.Repository
{
    public class DataInitilizer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {

    }
}