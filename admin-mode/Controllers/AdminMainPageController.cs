using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using admin_mode.Models;
using admin_mode.My_custom;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace admin_mode.Controllers
{
    public class AdminMainPageController : Controller
    {
        // GET: AdminMainPage
        public ActionResult Index()
        {
            AdminMainPageModels model = new AdminMainPageModels();
            Debug.WriteLine(model.TotalUsers);
            return View(model);
        }

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

            return View(dictionary);
        }

        // GET: AdminMainPage/Create
        public ActionResult Create()
        {
            return View();
        }


        // GET: AdminMainPage/UsersToXml
        [Route("UsersToXml")] 
        public ActionResult UsersToXml( )
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

        // POST: AdminMainPage/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

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

        [Authorize(Roles = "Global Admin")]
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



        // GET: AdminMainPage/UserEdit2/5
        [Authorize(Roles = "Global Admin")]
        public async Task<ActionResult> UserEdit2(string id)
        {
            if (id.IsNullOrWhiteSpace())
            {
                return HttpNotFound("id IsNullOrWhiteSpace!");
            }
            MyIdentityManager myIdentityManager = new MyIdentityManager();
            ApplicationUser user = null;
            try
            {
                user = myIdentityManager.GetUserByIdentityUserId(id);
            }
            catch (Exception e)
            {
                return HttpNotFound("user not found!!"+e);
            }
            
            
            return PartialView("UserEdit2", user);
        }

        // POST: AdminMainPage/UserEdit2/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Global Admin")]
        public async Task<ActionResult> UserEdit2([Bind(Include = "EnrollmentDate,Email,Id,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                MyIdentityManager myIdentityManager = new MyIdentityManager();
                //get the user based on the id
                var userEdited = myIdentityManager.SearchUserById(applicationUser.Id);
                var userEdited2 = myIdentityManager.GetUserByIdentityUserId(applicationUser.Id);
                 


                userEdited.EnrollmentDate = applicationUser.EnrollmentDate;
                userEdited.Email = applicationUser.Email;
                userEdited.EmailConfirmed = applicationUser.EmailConfirmed;
                userEdited.PhoneNumber = applicationUser.PhoneNumber;
                userEdited.PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed;
                userEdited.TwoFactorEnabled = applicationUser.TwoFactorEnabled;
                userEdited.LockoutEnabled = applicationUser.LockoutEnabled;
                userEdited.AccessFailedCount = applicationUser.AccessFailedCount;
                userEdited.UserName = applicationUser.UserName;
                var userEditResult = myIdentityManager.UpdateUser(userEdited);

                if (!userEditResult.Succeeded)
                {
                    return HttpNotFound("not updated");
                }


                return Json(new { success = true });
            }
            return PartialView("Users2");
        }


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
        public ActionResult AddNewRoleToUser2(string id, string[] role, FormCollection collection)
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
                return RedirectToAction("UserDetails", "AdminMainPage", new {id = id});
            }
            return HttpNotFound("AddNewRoleToUser: Error addint role: " + role + " to user with id " + id);

        }
    }
}
