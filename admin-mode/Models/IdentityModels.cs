using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace admin_mode.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            this.ComboItems = new HashSet<ComboItem>();
            return userIdentity;
            
        }
        public virtual string Address { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public virtual ICollection<ComboItem> ComboItems { get; set; }
    }

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
           
        public DbSet<ComboItem> ComboItem { get; set; }

        /*
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserComboItem>().HasKey(k => new { k.UserId, k.CombooItemId });
        }
        */
         
        // commented out giati evgaze to error:
        //Multiple object sets per type are not supported.The object sets 'Identity Users' and 'Users' can both contain instances of type 'admin-mode.Models.ApplicationUser'.
        //public System.Data.Entity.DbSet<admin_mode.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}