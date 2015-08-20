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
                 
            }catch (Exception e){}
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

        // GET: AdminMainPage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminMainPage/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
