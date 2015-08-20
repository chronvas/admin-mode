using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace admin_mode.Models
{
    public class AdminMainPageModels
    {
        public   int GetTotalUsers()
        {
            var db = new ApplicationDbContext();
            var tot = db.Users.Count();
            Debug.WriteLine("tot"+tot);
            return  tot;
        }

        public int TotalUsers {get { return GetTotalUsers(); }}
    }
    
}