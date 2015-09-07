using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using admin_mode.Models;
using admin_mode.My_custom;

namespace admin_mode.Controllers
{
    public class ComboItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ComboItems
        public async Task<ActionResult> Index()
        {
            return View(await db.ComboItem.ToListAsync());
        }

        // GET: ComboItems/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComboItem comboItem = await db.ComboItem.FindAsync(id);
            if (comboItem == null)
            {
                return HttpNotFound();
            }
            return View(comboItem);
        }

        // GET: ComboItems/Create
        public ActionResult Create()
        {
            //in the get, a list of users to select is presented
            MyIdentityManager myIdentityManager = new MyIdentityManager();
            var allUsers = myIdentityManager.GetAllUsers();
            
            
            return View();
        }

        // POST: ComboItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CombooItemId,Name")] ComboItem comboItem)
        {
            MyComboItemManager myComboItemManager = new MyComboItemManager();
            if (ModelState.IsValid)
            {
                await myComboItemManager.CreateNewComboItem(comboItem.Name);
                return RedirectToAction("Index");
            }
            return View(comboItem);

//
//            var r = myComboItemManager.AddComboItemToUser("07dfa8b7-2e00-4453-95fd-f2892b2bf655", "1234");
//            await db.SaveChangesAsync();
//            if(r)
//              return View(comboItem);
//            else return HttpNotFound("no");
        }

        // GET: ComboItems/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComboItem comboItem = await db.ComboItem.FindAsync(id);
            if (comboItem == null)
            {
                return HttpNotFound();
            }
            return View(comboItem);
        }

        // POST: ComboItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CombooItemId,Name")] ComboItem comboItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comboItem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(comboItem);
        }

        // GET: ComboItems/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null){return new HttpStatusCodeResult(HttpStatusCode.BadRequest);}

            ComboItem comboItem = await db.ComboItem.FindAsync(id);
            if (comboItem == null){return HttpNotFound();} 

            return View(comboItem);
        }

        // POST: ComboItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ComboItem comboItem = await db.ComboItem.FindAsync(id);
            db.ComboItem.Remove(comboItem);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
