using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data.Entity;
using System.Diagnostics;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Xml;
using System.Xml.Serialization;
using admin_mode.Models;
using admin_mode.My_custom;
using admin_mode.My_custom.Helpers;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using PagedList;

namespace admin_mode.Controllers
{
    public class AdminMainPageController : Controller
    {
        #region Index
        // GET: AdminMainPage
        [Authorize(Roles = "Global Admin")]
        public ActionResult Index()
        {
            if (Request.ContentType == "application/json")
            { //this will return a json (or whatever we want) if in the Header of the request the Content-type is application/jsonn
                return Json("some crazy object here", JsonRequestBehavior.AllowGet);
            }
            AdminMainPageModels model = new AdminMainPageModels();
            Debug.WriteLine(model.TotalUsers);
            return View(model);
        }
        #endregion index

        #region Users (palio)
        [Authorize(Roles = "Global Admin")]
        public ActionResult Users(string sortOrder, string currentFilter,string searchString, int? page)
        {
            //if (!User.IsInRole("Global Admin")) {  return HttpNotFound();}
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "email_desc" : "";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.UsernameSortParam = sortOrder == "username" ? "username_desc" : "username";
            //            if (searchString != null) { page = 1; } //if searchstring is something, return to page 1
            //            else { searchString = currentFilter; }  //if searchstring is null, make it equal to currentfilter

            ViewBag.CurrentFilter = searchString;
            ApplicationDbContext db = new ApplicationDbContext();
            var users = from s in db.Users
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            { //search in emails and usernames
                users= users.Where(s => s.Email.Contains(searchString)
                                       || s.UserName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "email_desc":
                    users = users.OrderByDescending(s => s.Email);
                    break;
                case "date":
                    users = users.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    users = users.OrderByDescending(s => s.EnrollmentDate);
                    break;
                case "username":
                    users = users.OrderByDescending(s => s.UserName);
                    break;
                case "username_desc":
                    users = users.OrderBy(s => s.UserName);
                    break;
                default:
                    users = users.OrderBy(s => s.Email);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1); 
            return View(users.ToPagedList(pageNumber, pageSize));
            //return View(users.ToList());
        }
        #endregion Users (palio)

        #region Users2
        [Authorize(Roles = "Global Admin")]
        public ActionResult Users2(string sortOrder, string currentFilter, string searchString, int? page)
        { 
            //if (!User.IsInRole("Global Admin")) {  return HttpNotFound();}
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "email_desc" : "";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.UsernameSortParam = sortOrder == "username" ? "username_desc" : "username";
            //            if (searchString != null) { page = 1; } //if searchstring is something, return to page 1
            //            else { searchString = currentFilter; }  //if searchstring is null, make it equal to currentfilter

            ViewBag.CurrentFilter = searchString;
            ApplicationDbContext db = new ApplicationDbContext();
            var users = from s in db.Users
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            { //search in emails and usernames
                users = users.Where(s => s.Email.Contains(searchString)
                                        || s.UserName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "email_desc":
                    users = users.OrderByDescending(s => s.Email);
                    break;
                case "date":
                    users = users.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    users = users.OrderByDescending(s => s.EnrollmentDate);
                    break;
                case "username":
                    users = users.OrderByDescending(s => s.UserName);
                    break;
                case "username_desc":
                    users = users.OrderBy(s => s.UserName);
                    break;
                default:
                    users = users.OrderBy(s => s.Email);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
            //return View(users.ToList());
        }
        #endregion Users2

        #region UserDetails 
        // GET: AdminMainPage/User/5 
        [Authorize(Roles = "Global Admin")]
        public ActionResult UserDetails(string id)
        { 
            if (id == null){Debug.WriteLine("id null");return HttpNotFound("id null");}
            MyIdentityManager myIdentityManager = new MyIdentityManager();
            List<object> listOfDetailsToDisplay = new List<object>();
            Dictionary<string,object> dictionary = new Dictionary<string, object>();

            // find what user we are talking about
            Debug.WriteLine("Details called for id: "+id);
            var  user = myIdentityManager.GetUserByIdentityUserId(id);

            //ad what we need to the dictionary
            try
            {
                dictionary.Add("user",user);
                dictionary.Add("UserRolesList",myIdentityManager.GetUserRoles(id));
                 
            }catch (Exception e){Debug.WriteLine(e);}
            if (dictionary.Count == 0) {  Debug.WriteLine("id wrong");return HttpNotFound("id wrong");}

            return PartialView(dictionary);
        }
        #endregion Users2

        #region DeleteUser
        //GET: AdminMainPage/DeleteUser/5
        [Authorize(Roles = "Global Admin")]
        [HttpGet]
        public ActionResult DeleteUser(string id)
        {
            if (id.IsNullOrWhiteSpace())
                return HttpNotFound("id IsNullOrWhiteSpace!");
            
            MyIdentityManager myIdentityManager = new MyIdentityManager();
            var user = myIdentityManager.GetUserByIdentityUserId(id);
            myIdentityManager.Dispose();
            
            return PartialView(user);
        }

        //POST: AdminMainPage/DeleteUser/5
        [Authorize(Roles = "Global Admin")]
        [HttpPost]
        public ActionResult DeleteUser(string id, FormCollection collection)
        {
            if (id.IsNullOrWhiteSpace())
                return HttpNotFound("id IsNullOrWhiteSpace!");
            
            MyIdentityManager myIdentityManager = new MyIdentityManager();
            var user = myIdentityManager.GetUserByIdentityUserId(id);
            var result = myIdentityManager.DeleteMemberShipUser(user);
            if (result.Succeeded)
                return Json(new { success = true });
            else
                return HttpNotFound("Error deleting user");
        }
#endregion
        
        #region UserEdit
        //PALIO
        // GET: AdminMainPage/UserEdit/5
        [Authorize(Roles = "Global Admin")]
        public ActionResult UserEdit(string id)
        {
            MyIdentityManager myIdentityManager = new MyIdentityManager();
            var user = myIdentityManager.SearchUserById(id);
            return View(user);
        }
        
        [HttpPost]
        [Authorize(Roles = "Global Admin")]
        [ValidateAntiForgeryToken]
        //Caution! singe this method is called by Global Admin, NO Checks suck as email validity or so are made before update!
        // POST: AdminMainPage/UserEdit/5
        public ActionResult UserEdit(string id, ApplicationUser applicationUser)
        {
            MyIdentityManager myIdentityManager = new MyIdentityManager();

            //var adsada = applicationUser.Email; //dinei nea timi
            //var newEmail = Request.Form["Email"]; //doulevei nea timi
            //myIdentityManager.SearchUserById(id).Email = form.GetValue("Email").AttemptedValue;  //doulevei nea timi

            //string value = Convert.ToString(form["Email"]); //doulevei nea timi
            var user = myIdentityManager.SearchUserById(id);
            
            
            user.EnrollmentDate = applicationUser.EnrollmentDate;
            user.Email = applicationUser.Email;
            user.EmailConfirmed = applicationUser.EmailConfirmed;
            user.PhoneNumber = applicationUser.PhoneNumber;
            user.PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed;
            user.TwoFactorEnabled = applicationUser.TwoFactorEnabled;
            user.LockoutEnabled = applicationUser.LockoutEnabled;
            user.AccessFailedCount = applicationUser.AccessFailedCount;
            user.UserName = applicationUser.UserName;
            myIdentityManager.UpdateUser(user);
            return RedirectToAction("Users");
        }

        #endregion UserEdit

        #region DeleteRoleFromUser
        //PALIO
        // GET: AdminMainPage/DeleteRoleFromUser/5
        [Authorize(Roles = "Global Admin")]
        public ActionResult DeleteRoleFromUser(string id, string role)
        {
            Debug.WriteLine("delete get");
            Debug.WriteLine(id+"^"+role); 
            if (id.IsNullOrWhiteSpace() || role.IsNullOrWhiteSpace()){return HttpNotFound("id and role incorrect");}
            List<string> idandrole = new List<string>() {id,role};
            return View(idandrole);
        }

        // POST: AdminMainPage/DeleteRoleFromUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Global Admin")]
        public ActionResult DeleteRoleFromUser(string id,string role, FormCollection collection)
        {
            try
            { 
                Debug.WriteLine("Delete requested"); 
//                Debug.WriteLine("model data: "+m.Role+" "+m.UserId); 
                Debug.WriteLine(id+"^"+role);
                MyIdentityManager myIdentityManager = new MyIdentityManager();
                var result = myIdentityManager.RemoveUserFromRole(id, role);
                if (result.Succeeded)
                {
                    Debug.WriteLine("sucess");
                }
                return RedirectToAction("UserDetails", "AdminMainPage", new { id = id });
            }
            catch
            {
                return View();
            }   
        }
        #endregion DeleteRoleFromUser

        #region AddNewRoleToUser (palio)
        [Authorize(Roles = "Global Admin")]
        //PALIO
        //Get: AdminMainPage/AddNewRoleToUser 
        //it will present a list of available roles for the user to be added to \
        public ActionResult AddNewRoleToUser(string id)
        {
            Debug.WriteLine("add new user to role requested, userid: "+id);

            if (id.IsNullOrWhiteSpace() ) { return HttpNotFound("id and role incorrect"); }
            
            MyIdentityManager myIdentityManager = new MyIdentityManager();

             
            List<SelectListItem> passingRolesList = new List<SelectListItem>();
            var allRoles = myIdentityManager.GetAllRoles();
            if (allRoles.Count == 0){return HttpNotFound("No roles available. Create a New Role First!");}
            foreach (var role in allRoles)
            {
                SelectListItem listItem = new SelectListItem() {Text = role.Name,Value = role.Name};
                passingRolesList.Add(listItem);
            }

            var dictionary = new Dictionary<string,object>();
            dictionary.Add("selectlist",passingRolesList);
            dictionary.Add("id",id);
            IEnumerable<SelectListItem> rolesienum = myIdentityManager.AllRolesToIenumSelectListItemsForuser(id); 
            dictionary.Add("ienum", rolesienum);
            return View(dictionary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Global Admin")]
        public ActionResult AddNewRoleToUser(string id, string[] role, FormCollection collection)
        {
            MyIdentityManager myIdentityManager = new MyIdentityManager();

            #region safety checks
            if (role ==null) { return HttpNotFound("AddNewRoleToUser: Roles table zero"); }
            foreach (var r in role){
                if (r.IsNullOrWhiteSpace()) { return HttpNotFound("AddNewRoleToUser: a role in the roles table is null or whitespace");} 
                //if role exists and we are not trying to add the user to an non existing role in the db
                if (!myIdentityManager.RoleExist(r)) { return HttpNotFound("AddNewRoleToUser: a role in the roles table not found!"); }
                //if user is already in this role
                var user = myIdentityManager.SearchUserById(id);
                if (myIdentityManager.IsUserInRole(r, user.UserName)) { return HttpNotFound("AddNewRoleToUser: user is already in a role you selected!"); }
            }
            #endregion
            //add user to every role
            var result = myIdentityManager.AddUserToRoles(id, role);
            if (result == true)
                return RedirectToAction("UserDetails","AdminMainPage",new {id=id});
            return HttpNotFound("AddNewRoleToUser: Error addint role: " + role + " to user with id " + id);

        }
        #endregion AddNewRoleToUser 

        #region UserEdit2
        // GET: AdminMainPage/UserEdit2/5
        [Authorize(Roles = "Global Admin")]
        public async Task<ActionResult> UserEdit2(string id)
        {
            if (id.IsNullOrWhiteSpace())
            {
                return HttpNotFound("id IsNullOrWhiteSpace!");
            }
            MyIdentityManager myIdentityManager = new MyIdentityManager();
            ApplicationUser user;
            try
            {
                user = myIdentityManager.GetUserByIdentityUserId(id);
            }
            catch (Exception e)
            {
                return HttpNotFound("user not found!!"+e);
            }
            
            MyComboItemManager myComboItemManager = new MyComboItemManager();
            Dictionary<object, object> dictionary = new Dictionary<object, object>();
             
            var I2 = myComboItemManager.GetAllRolesForUserIdToIenumSelectListItem(id);
            
            dictionary.Add("ienum", I2);
            dictionary.Add("applicationUser", user);

            return PartialView("UserEdit2", dictionary);
        }

        // POST: AdminMainPage/UserEdit2/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Global Admin")]
        public async Task<ActionResult> UserEdit2([Bind
            (Include = "EnrollmentDate,Email,Id,EmailConfirmed," +
                       "PhoneNumber,PhoneNumberConfirmed," +
                       "TwoFactorEnabled,LockoutEnabled," +
                       "AccessFailedCount,UserName," +
                       "LockoutEndDateUtc,ComboItems")]
                                        ApplicationUser applicationUser,string[] ComboItems )
        {
            if (ModelState.IsValid)
            {
                MyIdentityManager myIdentityManager = new MyIdentityManager();
                MyComboItemManager myComboItemManager = new MyComboItemManager();
                //get the user based on the id
                var userEdited = myIdentityManager.SearchUserById(applicationUser.Id);
                
                myComboItemManager.UpdateComboItemsforUser(applicationUser.Id, ComboItems);

                userEdited.EnrollmentDate = applicationUser.EnrollmentDate;
                userEdited.Email = applicationUser.Email;
                userEdited.EmailConfirmed = applicationUser.EmailConfirmed;
                userEdited.PhoneNumber = applicationUser.PhoneNumber;
                userEdited.PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed;
                userEdited.TwoFactorEnabled = applicationUser.TwoFactorEnabled;
                userEdited.LockoutEnabled = applicationUser.LockoutEnabled;
                userEdited.LockoutEndDateUtc = applicationUser.LockoutEndDateUtc;
                userEdited.AccessFailedCount = applicationUser.AccessFailedCount;
                userEdited.UserName = applicationUser.UserName;
                var userEditResult = myIdentityManager.UpdateUser(userEdited);

                if (!userEditResult.Succeeded)
                {
                    return HttpNotFound("not updated");
                }

                myComboItemManager.DisposeAll();
                return Json(new { success = true });
            }
            return HttpNotFound("Not Valid mod");
        }

        #endregion UserEdit2

        #region AddNewRoleToUser2
        [Authorize(Roles = "Global Admin")]
        //GET: AdminMainPage/AddNewRoleToUser2
        //it will present a list of available roles for the user to be added to \
        public  ActionResult  AddNewRoleToUser2(string id)
        {
            if (id.IsNullOrWhiteSpace()) { return HttpNotFound("id and role incorrect"); }

            MyIdentityManager myIdentityManager = new MyIdentityManager();


            List<SelectListItem> passingRolesList = new List<SelectListItem>();
            var allRoles = myIdentityManager.GetAllRoles();
            if (allRoles.Count == 0) { return HttpNotFound("No roles available. Create a New Role First!"); }
            foreach (var role in allRoles)
            {
                SelectListItem listItem = new SelectListItem() { Text = role.Name, Value = role.Name };
                passingRolesList.Add(listItem);
            }

            var dictionary = new Dictionary<string, object>();
            dictionary.Add("selectlist", passingRolesList);
            dictionary.Add("id", id);
            IEnumerable<SelectListItem> rolesienum = myIdentityManager.AllRolesToIenumSelectListItemsForuser(id);
            dictionary.Add("ienum", rolesienum);
            Debug.WriteLine(dictionary.Count);
            return PartialView("AddNewRoleToUser2", dictionary);

        }

        //POST: AdminMainPage/AddNewRoleToUser2
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Global Admin")]
        public async Task<ActionResult> AddNewRoleToUser2(string id, string[] role, FormCollection collection)
        {
            MyIdentityManager myIdentityManager = new MyIdentityManager();

            #region safety checks
            if (role == null) { return HttpNotFound("AddNewRoleToUser: Roles table zero"); }
            foreach (var r in role)
            {
                if (r.IsNullOrWhiteSpace()) { return HttpNotFound("AddNewRoleToUser: a role in the roles table is null or whitespace"); }
                //if role exists and we are not trying to add the user to an non existing role in the db
                if (!myIdentityManager.RoleExist(r)) { return HttpNotFound("AddNewRoleToUser: a role in the roles table not found!"); }
                //if user is already in this role
                var user = myIdentityManager.SearchUserById(id);
                if (myIdentityManager.IsUserInRole(r, user.UserName)) { return HttpNotFound("AddNewRoleToUser: user is already in a role you selected!"); }
            }
            #endregion
            //add user to every role
            var result = myIdentityManager.AddUserToRoles(id, role);
            if (result == true)
            {
                TempData["Success"] = true;
                
                //return RedirectToAction("Us", "AdminMainPage", new {id = id});
            }
            return HttpNotFound("AddNewRoleToUser: Error addint role: " + role + " to user with id " + id);

        }

        #endregion AddNewRoleToUser2

        #region ManageRolesForUser
        [Authorize(Roles = "Global Admin")]
        //Get: AdminMainPage/ManageRolesForUser
        //it will manage the roles. 
        //presenting the previous roles, user selects new roles, ALL previous roles dropped, and all user selected roles added
        public ActionResult ManageRolesForUser(string id)
        {
            if (id.IsNullOrWhiteSpace()) { return HttpNotFound("id null or whitespace"); }
            MyIdentityManager myIdentityManager = new MyIdentityManager();

            List<SelectListItem> passingRolesList = new List<SelectListItem>();
            var allRoles = myIdentityManager.GetAllRoles();
            if (allRoles.Count == 0) { return HttpNotFound("No roles available in the system. Create a New Role First!"); }
            foreach (var role in allRoles)
            {
                SelectListItem listItem = new SelectListItem() { Text = role.Name, Value = role.Name };
                passingRolesList.Add(listItem);
            }
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("selectlist", passingRolesList);
            dictionary.Add("username", myIdentityManager.GetUserByIdentityUserId(id).UserName);

            IEnumerable<SelectListItem> rolesienum = myIdentityManager.AllRolesToIenumSelectListItemsForuser(id);
            dictionary.Add("ienum", rolesienum);
            
            return PartialView(dictionary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Global Admin")]
        public async Task<ActionResult> ManageRolesForUser(string id, string[] role, FormCollection collection)
        {
            MyIdentityManager myIdentityManager = new MyIdentityManager();
            var user = myIdentityManager.SearchUserById(id);
            #region safety checks
            if (user == null){return HttpNotFound("User with id " + id + "not found!");}
            if (role == null) //list emptied out
            { // all roles must be removed
                myIdentityManager.RemoveUserFromRoles(id);
                return Json(new { success = true });
            }
            //delete all roles form user, and add the new roles in the table 
            if (!myIdentityManager.RemoveUserFromRoles(id).Succeeded){return HttpNotFound("removeRolesError");}
            var result = false;
            foreach (var r in role)
            {
                if (r.IsNullOrWhiteSpace()) { return HttpNotFound("AddNewRoleToUser: a role in the roles table is null or whitespace!"); }
                //if role exists and we are not trying to add the user to an non existing role in the db
                if (!myIdentityManager.RoleExist(r)) { return HttpNotFound("AddNewRoleToUser: system roles not representing roles table"); }
                //if user is already in this role
                result = myIdentityManager.AddUserToRole(id, r);
            }
            #endregion
            //add user to every role
            //var result = myIdentityManager.AddUserToRoles(id, role);
         
            if (result == true)
            {
                return Json(new { success = true });
            }
            return HttpNotFound("AddNewRoleToUser: Error adding role: " + role + " to user with id " + id);
 
        }

        #endregion ManageRolesForUser

        #region AddUser
        //GET: AdminMainPage/AddUser
        public async Task<ActionResult> AddUser()
        { 

            return PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(
            [Bind(
                Include =
                    "EnrollmentDate,Email,Id,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnabled,AccessFailedCount,UserName"
                )] ApplicationUser applicationUser)
        {
            // id system
            //
            MyIdentityManager myIdentityManager = new MyIdentityManager();
            
            ApplicationUser newUser = new ApplicationUser();

            newUser.EnrollmentDate = applicationUser.EnrollmentDate;
            newUser.Email = applicationUser.Email;
            newUser.EmailConfirmed = applicationUser.EmailConfirmed;
            //newUser.PhoneNumber = applicationUser.PhoneNumber;
            //newUser.PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed;
            //newUser.TwoFactorEnabled = applicationUser.TwoFactorEnabled;
            newUser.LockoutEnabled = false;
            //newUser.AccessFailedCount = applicationUser.AccessFailedCount;
            newUser.UserName = applicationUser.UserName;
            
            //password
            var PasswordHash = new PasswordHasher();
            var hp = PasswordHash.HashPassword("qqqqqq");

            myIdentityManager.CreateNewUser(newUser, hp);
            return Json(new { success = true });
        }

        #endregion AddUser

        #region AddNewUser
        //GET: AdminMainPage/AddUser
        public async Task<ActionResult> AddNewUser()
        {

            AddNewUserViewModel addNewUserViewModel = new AddNewUserViewModel();
            return PartialView(addNewUserViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AddNewUser(
            [Bind(
                Include =
                    "Email,Password,UserName,EnrollmentDate,PhoneNumber,PhoneNumberConfirmed" +
                    ",TwoFactorEnabled,LockoutEnabled,LockoutEndDateUtc,AccessFailedCount,EmailConfirmed,ComboItemsStrTable"
                )] AddNewUserViewModel addNewUserViewModel  )
        {
            MyIdentityManager myIdentityManager = new MyIdentityManager();

            if (!ModelState.IsValid)
            {//if the modelstate is not valid, pass the errors to a string, and display via httpnotfount. Not the best way, but works
                #region error reporting
                var modelStateerrors = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors }).ToArray();
                string errorList = "";
                

                if (modelStateerrors !=null)
                {
                    foreach (var modelerr in modelStateerrors)
                    {
                        foreach (var modelerro in modelerr.Errors)
                        {
                            errorList = errorList + " | " + modelerro.ErrorMessage;
                        } 
                    }
                }
                //return PartialView("CustomError", errorList);
                #endregion
            }
            //our user, to ASPNET user. Relying to ASPNET's input error checks etc
            DateTime temp = DateTime.Now;
            var user = new ApplicationUser {    Email = addNewUserViewModel.Email, 
                                                UserName = addNewUserViewModel.Email, 
                                                EnrollmentDate = addNewUserViewModel.EnrollmentDate ?? temp,  //if the user leaves that blank, fill it with datetime.now
                                                PhoneNumber = addNewUserViewModel.PhoneNumber,
                                                PhoneNumberConfirmed = addNewUserViewModel.PhoneNumberConfirmed,
                                                TwoFactorEnabled = addNewUserViewModel.TwoFactorEnabled,
                                                LockoutEnabled = addNewUserViewModel.LockoutEnabled,
                                                LockoutEndDateUtc = addNewUserViewModel.LockoutEndDateUtc,
                                                AccessFailedCount = addNewUserViewModel.AccessFailedCount ?? 0, //if the user leaves that blank, make it 0
                                                EmailConfirmed = addNewUserViewModel.EmailConfirmed
                                                };
            var createUserResult =   myIdentityManager.CreateNewUser( user, addNewUserViewModel.Password);
            bool addComboItemToUserResult = false;
            if (createUserResult.Succeeded)
            {
                //foreach (var selectListItem in addNewUserViewModel.ComboItems)
                //{
                //    MyComboItemManager myComboItemManager = new MyComboItemManager();
                //    if (selectListItem.Selected)
                //    {
                //        addComboItemToUserResult = myComboItemManager.AddComboItemToUser(user.Id, selectListItem.Value);
                //    }
                //}
                
                //if (addComboItemToUserResult)
                // the above is the proper way. DropDownList returns a string[] instead a fking IEnumerable<SelectListItem>. FK that

                MyComboItemManager myComboItemManager = new MyComboItemManager();
                addComboItemToUserResult  = myComboItemManager.UpdateComboItemsforUser(user.Id, addNewUserViewModel.ComboItemsStrTable);
                myComboItemManager.DisposeAll();

                    return Json(new { success = true });
                //else
                //    return HttpNotFound("Could not add Comboitems to User");
            }
            else
            { 
                return HttpNotFound("User data not valid, please try again");
            }
        }
        #endregion AddNewUser

        #region ManageComboItemsforUser
        public async Task<ActionResult> ManageComboItemsforUser(string id)
        {
            if (id.IsNullOrWhiteSpace()) { return HttpNotFound("id null or whitespace"); }
            MyComboItemManager myComboItemManager = new MyComboItemManager();
            List<SelectListItem> passingComboItemsList = new List<SelectListItem>();
            var allComboItems = myComboItemManager.GetAllComboItems();
            if (allComboItems.Count == 0) { return HttpNotFound("No comboitems available in the system. Create a New Role First!"); }
            foreach (var comboitem in allComboItems)
            {
                SelectListItem listItem = new SelectListItem() { Text = comboitem.Name, Value = comboitem.Name };
                passingComboItemsList.Add(listItem);
            }
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("selectlist", passingComboItemsList);
            dictionary.Add("id", id); 

            IEnumerable<SelectListItem> comboItemsIenum =
                myComboItemManager.AllComboItemsToIenumSelectlistItemsForUser(id);
            dictionary.Add("ienum", comboItemsIenum);
            return PartialView(dictionary);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageComboItemsforUser(string id, string[] ComboItems, FormCollection collection)
        {
            MyIdentityManager myIdentityManager = new MyIdentityManager();
            MyComboItemManager myComboItemManager = new MyComboItemManager();
            if (ComboItems != null)
            {
                foreach (var comboItem in ComboItems)
                {
                    if (comboItem.IsNullOrWhiteSpace())
                    {
                        return HttpNotFound("AddNewRoleToUser: a role in the roles table is null or whitespace!");
                    }
                    //if role exists and we are not trying to add the user to an non existing role in the db
                }
            }// if ComboItems==null is checked by UpdateComboItemsforUser, and there is an action for that

            if (id.IsNullOrWhiteSpace()) return HttpNotFound("User Id Is null! 4956");

            //find user
            var user = myIdentityManager.SearchUserById(id);
            if (user == null) { return HttpNotFound("User with id " + id + "not found!"); }
            //if (ComboItems == null) { return HttpNotFound("ManageRolesForUser: ComboItems table zero"); }

            //Logic to update with new selection of ComboItems
            bool result = myComboItemManager.UpdateComboItemsforUser(id, ComboItems);

            //myComboItemManager.DisposeAll();

            if (result == true)
            {
                return Json(new { success = true });
            } 
            return HttpNotFound("AddNewRoleToUser: Error adding role: " + " to user with id " + id); 
        }

        #endregion ManageComboItemsforUser

#region APIs
        // GET: AdminMainPage/UsersToXml
        [Route("UsersToXml")]
        public ActionResult UsersToXml()
        {
            string xml = "<?xml version = \"1.0\" encoding= \"utf-8\"?>";
            MyIdentityManager myIdentityManager = new MyIdentityManager();
            var users = myIdentityManager.GetAllUsers();
            xml = xml + "<Users>";
            foreach (var user in users)
            {
                xml = xml + "<Name>";
                xml = xml + user.Id;
                xml = xml + "</Name>";
            }
            xml = xml + "</Users>";

            return Content(xml, "text/xml");
        }
#endregion APIs

        //mockup. Not for use.
        public ActionResult ChooseRolePartial(string username)
        {
            MyComboItemManager myComboItemManager = new MyComboItemManager();
            var allComboItemsForUser = myComboItemManager.GetAllComboItemsForUsernameStringTable(username);
            var userRoles = Roles.GetRolesForUser(username); 
            var comboItems = myComboItemManager.GetAllComboItemsStringTable().Select(y => new SelectListItem
            {
                Value = y,
                Text = y,
                Selected = allComboItemsForUser.Contains(y)
            }).ToArray();
            return null;
        }


        public async Task<ActionResult> ResetPassword(string id)
        {
            return PartialView("UserDetails");
        }
    }
}
