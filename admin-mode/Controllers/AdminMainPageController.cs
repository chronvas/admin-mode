using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
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
                dictionary.Add("userId",user.Id);
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

        // GET: AdminMainPage/Edit/5
        public ActionResult Edit(int id)
        {
            
            return View();
            
        }

        // POST: AdminMainPage/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
                return RedirectToAction("Users");
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

    }
}
