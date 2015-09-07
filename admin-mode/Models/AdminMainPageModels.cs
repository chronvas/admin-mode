using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using admin_mode.My_custom;
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
            db.Dispose();
            return  tot;
        }

        public int TotalUsers {get { return GetTotalUsers(); }}


        public int GetTotalComboItems()
        {
            MyComboItemManager myComboItemManager = new MyComboItemManager();
            var count = myComboItemManager.GetAllComboItems().Count;
            myComboItemManager.DisposeAll();
            return count;
        }
        public int TotalComboItems { get { return GetTotalComboItems(); } }
    }

    public class AddNewUserViewModel
    {
        [Key]
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public virtual string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public virtual string Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Username")]
        public virtual string UserName { get; set; }

        [Display(Name = "Enrollment Date")]
        public virtual DateTime? EnrollmentDate { get; set; }

        [Display(Name = "Phone Number")]
        public virtual string PhoneNumber { get; set; }

        [Display(Name = "Phone Number Confirmed")]
        public virtual bool PhoneNumberConfirmed { get; set; }

        public virtual bool TwoFactorEnabled { get; set; }

        [Display(Name = "User Lockout Enabled")]
        public virtual bool LockoutEnabled { get; set; }

        [Display(Name = "Lockout End Date :UTC")]
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        [Display(Name = "Access Failed Count")]
        public virtual int? AccessFailedCount { get; set; } 
         
        [Display(Name = "Email Confirmed")]
        public virtual bool EmailConfirmed { get; set; }
    }
}