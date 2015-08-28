using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using admin_mode.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace admin_mode.My_custom
{
    // // by
    //http://stackoverflow.com/questions/21741384/how-do-i-invalidate-claims-using-asp-net-identity/31602684#31602684
    public class MyIdentityManager
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _dbContext;
        private ApplicationSignInManager _signInManager;
        private DpapiDataProtectionProvider protectionProvider;
        private ClaimsAuthenticationManager aclaimsauthenticationmanager;

        public MyIdentityManager()
        {
            _dbContext = new ApplicationDbContext();
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_dbContext));
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dbContext));
            protectionProvider = new DpapiDataProtectionProvider("Demo");
            _userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(protectionProvider.Create("ResetTokens"));
        }
        public MyIdentityManager(ApplicationSignInManager signmanager)
        {
            _dbContext = new ApplicationDbContext();
            _signInManager = signmanager;
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_dbContext));
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dbContext));
            protectionProvider = new DpapiDataProtectionProvider("Demo");
            _userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(protectionProvider.Create("ResetTokens"));
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager;
            }
            private set { _signInManager = value; }
        }

        public bool CreateNewUserRole(string role)
        {
            if (!RoleExist(role))
            {
                var result = _roleManager.Create(new IdentityRole(role));

                return result.Succeeded;
            }
            return false;
        }

        public bool DeleteUserRole(string role)
        {
            if (!RoleExist(role))
                return true;

            var result = _roleManager.Delete(new IdentityRole(role));
            return result.Succeeded;
        }

        public IdentityResult DeleteMemberShipUser(ApplicationUser user)
        {
            // first, remove all the roles for this user
            _userManager.RemoveFromRoles(user.Id);
            //then remove the user completely



            var logins = user.Logins;

            foreach (var login in logins.ToList())
            {
                _userManager.RemoveLogin(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
            }

            var rolesForUser = _userManager.GetRoles(user.Id);

            if (rolesForUser.Any())
            {
                foreach (var item in rolesForUser.ToList())
                {
                    // item should be the name of the role
                    var result = _userManager.RemoveFromRole(user.Id, item);
                }
            }

            return _userManager.Delete(user);
        }




        public bool RoleExist(string role)
        {
            return _roleManager.RoleExists(role);
        }

        public IdentityResult ChangePassword(ApplicationUser user, string token, string newpassword)
        {
            _userManager.UserValidator = new UserValidator<ApplicationUser>(_userManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            return _userManager.ResetPassword(user.Id, token, newpassword);
        }

        public ApplicationUser GetUserByIdentityUserId(string userId)
        {
            var ret = _userManager.FindById(userId);
            return ret;
        }

        public ApplicationUser SearchUserById(string IdContains)
        {
            var usersFound = _dbContext.Users.SingleOrDefault(s => s.Id == IdContains);
            return usersFound;
        }
        public IdentityResult CreateNewUser(ApplicationUser user, string password)
        {
            _userManager.UserValidator = new UserValidator<ApplicationUser>(_userManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            _userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            _userManager.UserLockoutEnabledByDefault = false;
            _userManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            _userManager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var result = _userManager.Create(user, password);
            return result;
        }

        public IdentityResult UpdateUser(ApplicationUser user)
        {
            var ret = _userManager.Update(user);
            return ret;

        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var result = _userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        public bool AddUserToRoles(string userId, string[] roleNames)
        {
            var result = _userManager.AddToRoles(userId,roleNames);
            return result.Succeeded;
        }

        public IdentityResult RemoveUserFromRole(string userId, string role)
        {
            var result = _userManager.RemoveFromRole(userId, role);
            return result;
        }

        public List<ApplicationUser> GetAllUsers()
        {

            var users = _userManager.Users.ToList();
            return users;
        }

        public IList<string> GetUserRoles(string userid)
        {
            return _userManager.GetRoles(userid);
        }

        public string GetUserRole(string userid)
        {
            return _userManager.GetRoles(userid).FirstOrDefault();
        }

        public IdentityRole GetRoleByRoleName(string roleName)
        {
            var t = _roleManager.Roles.First(i => i.Name == roleName);

            return t;
        }

        public string GetUserRoleId(string userId)
        {
            var userRole = GetUserRole(userId);
            if (string.IsNullOrWhiteSpace(userRole)) return null;

            var role = GetRoleByRoleName(userRole);
            return role.Id;
        }

        public IdentityResult CreateNewSystemRole(IdentityRole role)
        {
            return !RoleExist(role.Name) ? _roleManager.Create(role) : new IdentityResult(new List<string> { "Role Already Exists" });
        }

        public List<IdentityRole> GetAllRoles()
        {
            return _roleManager.Roles.ToList();
        }

        public bool IsUserInRole(string role, string userName)
        {
            var user = _userManager.FindByName(userName);
            return _userManager.IsInRole(user.Id, role);
        }
        public bool IsUserInRolebyId(string role, string id)
        {
            return _userManager.IsInRole(id, role);
        }

        public ApplicationUser GetUserByUserName(string username)
        {
            return _userManager.FindByName(username);
        }

        public string GenerateResetToken(string userid)
        {
            return _userManager.GeneratePasswordResetToken(userid);
        }

        public IdentityResult SetLockStatus(string userid, bool lockstatus)
        {
            return _userManager.SetLockoutEnabled(userid, lockstatus);
        }

        public IdentityResult AddUserClaim(string userId, Claim claim)
        {
            return _userManager.AddClaim(userId, claim);
        }

        public void AddRoleClaim(string roleId, string claimType, string claimValue, int utilityid, string description)
        {
            try
            {
                _userManager.UserValidator = new UserValidator<ApplicationUser>(_userManager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true
                };


                //                
                //                var roleClaim = new AspNetRoleClaims()
                //                {
                //                    RoleId = roleId,
                //                    ClaimType = claimType,
                //                    ClaimValue = claimValue,
                //                    UtilityId = utilityid,
                //                    Description = description
                //                };
                //                aclaimsauthenticationmanager.
                //                _dbContext.AspNetRoleClaims.Add(ro'leClaim);
                //                _dbContext.adfsasd.Add(roleClaim);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new IdentityNotMappedException(ex.Message, ex);
            }
        }

        public IList<Claim> GetUserClaims(string userId)
        {
            return _userManager.GetClaims(userId);
        }

        public IdentityResult RemoveUserClaim(string userId, string claimType)
        {
            _userManager.UserValidator = new UserValidator<ApplicationUser>(_userManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            var claim = _userManager.GetClaims(userId).FirstOrDefault(t => t.Type == claimType);
            if (claim == null) return IdentityResult.Success;

            return _userManager.RemoveClaim(userId, claim);
        }

        public void DeleteRole(string id)
        {
            //var language = new LanguageCodeLookup();
            var aspNetRoles = _dbContext.Roles.FirstOrDefault(r => r.Id == id);

            if (aspNetRoles == null)
                throw new Exception("Role Does Not Exist");
            if (aspNetRoles.Name == "Utility Administrator" ||
                aspNetRoles.Name == "Content Manager" ||
                aspNetRoles.Name == "System Administrator" ||
                aspNetRoles.Name == "Customer Accounts Manager")
                throw new Exception("CannotDeleteDefaultRoles");
            if (aspNetRoles.Users.Count > 0)
                throw new Exception("Cannot Delete Roles With Users");
            _dbContext.Roles.Remove(aspNetRoles);
            _dbContext.SaveChanges();
        }

        public IdentityRole GetRole(string id)
        {
            return _dbContext.Roles.FirstOrDefault(r => r.Id == id);
        }

        public IdentityResult UpdateRole(IdentityRole roleToUpdate)
        {
            return _roleManager.Update(roleToUpdate);
        }

        public List<ApplicationUser> SearchUsersByUsername(string UsernameContains)
        {
            var usersFound = _dbContext.Users.Where(s => s.UserName.Contains(UsernameContains)).ToList();
            return usersFound;
        }



        public IEnumerable<SelectListItem> AllRolesToIenumSelectListItems()
        {
            List<SelectListItem> list = null;
            var query = (from ca in _dbContext.Roles
                         orderby ca.Name
                         select new SelectListItem { Text = ca.Name, Value = ca.Name}).Distinct();
            list = query.ToList();
            Debug.WriteLine("-- Roles nu",list.Count);
            return list;
        }

        /// <summary>
        /// //add the roles to ienum selectlistitem for a specific user, setting property disabled if the user ALREADY exists in that role
        /// </summary>
        /// <returns>IEnumerable<SelectListItem></returns>

        public IEnumerable<SelectListItem> AllRolesToIenumSelectListItemsForuser(string id)
        {
            List<SelectListItem> list = null;
            var query = (from ca in _dbContext.Roles
                         orderby ca.Name
                         select new SelectListItem { Text = ca.Name, Value = ca.Name }).Distinct();
            list = query.ToList();

            
            foreach (var item in list)
            {
                if (IsUserInRolebyId(item.Value, id))
                {
                    item.Disabled = true;
                }
            }
            Debug.WriteLine("-- Roles nu "+ list.Count);
            return list;
        }
    }
}