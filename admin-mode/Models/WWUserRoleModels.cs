using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using admin_mode.My_custom;
using Microsoft.AspNet.Identity.EntityFramework;

namespace admin_mode.Models
{
    public class WWUserRoleModels
    {
        public List<IdentityRole> HAS { get; set; }

        private List<string> getHas(string id)
        {
            MyIdentityManager myIdentityManager = new MyIdentityManager();
            var roles = myIdentityManager.GetUserRoles(id);
            var Rroles = roles as List< string >;
            return Rroles;
        }

        public List<string> setHas(string id)
        {
            MyIdentityManager myIdentityManager = new MyIdentityManager();
            List<string> AllRolesInDb = myIdentityManager.GetAllRolesToList();
            var AllRolesInUser = myIdentityManager.GetUserRoles(id);
            foreach (var roles in AllRolesInDb)
            {
                Debug.WriteLine(roles);
            }
            return null;
             
        }
    }
}