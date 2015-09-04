using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace admin_mode.Models
{
    public class ComboItem
    {
        [Key]
        public string CombooItemId { get; set; }

        public string Name { get; set; }

        //public ComboItem() : this("")
        //{
        //}

        //public ComboItem(string comboItemName)
        //{
        //    this.CombooItemId = Guid.NewGuid().ToString();
        //    this.Name = comboItemName;
        //    this.ApplicationUsers = new HashSet<ApplicationUser>();
        //}

        //public ComboItem(string comboItemName,ApplicationUser applicationUser)
        //{
        //    this.CombooItemId = Guid.NewGuid().ToString();
        //    this.Name = comboItemName;
        //    this.ApplicationUsers = new HashSet<ApplicationUser>();
        //    this.ApplicationUsers.Add(applicationUser);
        //    var sd = 4;
        //}
        
        
        public virtual ICollection<ApplicationUser> ApplicationUsers{ get; set; }
    }
}