using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using admin_mode.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace admin_mode.My_custom
{
    public class MyComboItemManager
    {
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _dbContext;
        private DpapiDataProtectionProvider protectionProvider;

        public MyComboItemManager()
        {
            _dbContext = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dbContext));
            //idk why they are used..
            protectionProvider = new DpapiDataProtectionProvider("Demo");
            _userManager.UserTokenProvider =
                new DataProtectorTokenProvider<ApplicationUser>(protectionProvider.Create("ResetTokens"));
            
        }

        public void DisposeAll()
        {
            _dbContext.Dispose();
            _userManager.Dispose();
        }
        public async Task<bool> CreateNewComboItem(string Name)
        {
            ComboItem Cnew = new ComboItem() {Name = Name,CombooItemId = Guid.NewGuid().ToString()};
            _dbContext.ComboItem.Add(Cnew);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public bool AddComboItemToUser(string UserId, string ComboItemName)
        {
            var userToAssociate = _dbContext.Users.FirstOrDefault(x => x.Id == UserId);

            var comboItem = _dbContext.ComboItem.FirstOrDefault(x => x.Name == ComboItemName);
            //new ComboItem(ComboItemName, userToAssociate);

//            comboItem?.ApplicationUsers.Add(userToAssociate);  
            if (comboItem != null) comboItem.ApplicationUsers.Add(userToAssociate);
            else return false;

            _dbContext.SaveChanges();
            
            return true;
        }
        public bool RemoveComboItemFromUser(string UserId, string ComboItemName)
        {
            var userToAssociate = _dbContext.Users.FirstOrDefault(x => x.Id == UserId);

            var comboItem = _dbContext.ComboItem.FirstOrDefault(x => x.Name == ComboItemName);
            //new ComboItem(ComboItemName, userToAssociate);

            comboItem.ApplicationUsers.Remove(userToAssociate);

            _dbContext.SaveChanges();

            return true;
        }
        public List<ComboItem> GetAllComboItems()
        {
            var all = _dbContext.ComboItem.ToList();
            return all;
        }

        public List<ComboItem> GetAllComboItemsForUsername(string username)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.UserName == username);
            if (user != null) return user.ComboItems.ToList();
            Debug.WriteLine("GetAllComboItemsForUsername->User null");
            return null;
        }
        public List<ComboItem> GetAllComboItemsForId(string id)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);
            if (user != null) return user.ComboItems.ToList();
            Debug.WriteLine("GetAllComboItemsForUsername->User null");
            return null;
        }
        public string[] GetAllComboItemsForUsernameStringTable(string username)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.UserName == username);
            List<string> s= new List<string>();

            foreach (var comboItem in user.ComboItems)
            {
                s.Add(comboItem.Name);
            }
            String[] stringArray = s.ToArray();
            return stringArray;
        }

        public string[] GetAllComboItemsStringTable()
        {
            List<string> s = new List<string>();
            var ds = _dbContext.ComboItem.ToArray();
            foreach (var comboItem in ds)
            {
                s.Add(comboItem.Name);
            }
            String[] stringArray = s.ToArray();
            return stringArray;
        }
        public IEnumerable<SelectListItem> AllComboItemsToIenumSelectlistItemsForUser(string id)
        {
            List<SelectListItem> list = null;
            var query = (from ca in _dbContext.ComboItem
                orderby ca.Name
                select new SelectListItem() {Text = ca.Name, Value = ca.Name}).Distinct();
            list = query.ToList();
            foreach (var item in list)
            {
                if(IsUserAssosiacedWithComboItemById(item.Value, id))
                {
                    item.Selected = true;
                }
            }
            return list;
        }

        public List<ComboItem> AllComboItemsListForUser(string id)
        {
// Searching in each item-> Searching in each user associated-> Searching if users id matching incoming id
            //var sada = _dbContext.ComboItem.SelectMany(o => o.ApplicationUsers);
            List<string> list = new List<string>();
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            List<ComboItem> cItemsForUser = user.ComboItems.OrderBy(w=>w.Name).ToList();
            

            return cItemsForUser;
        }

        public List<string> AllComboItemNamesListForUser(string id)
        {// Searching in each item-> Searching in each user associated-> Searching if users id matching incoming id
            //var sada = _dbContext.ComboItem.SelectMany(o => o.ApplicationUsers);
            List<string> list = new List<string>();
            var allComboItems = GetAllComboItems(); 
            foreach (var comboItem in allComboItems)
            {
                var usersforC = comboItem.ApplicationUsers;
                foreach (var applicationUser in usersforC)
                {
                    if (applicationUser.Id == id)
                    {
                        list.Add(comboItem.Name);
                    }
                }
                
            } 

//Method 2, needs checking
//            var query = (from ca in _dbContext.ComboItem
//                orderby ca.Name
//                select new string(ca.Name.ToCharArray())).Distinct();
//                         //select new string(ca.Name) { Text = ca.Name, Value = ca.Name }).Distinct();
//                         //select new SelectListItem() { Text = ca.Name, Value = ca.Name }).Distinct();
//            list = new List<string>(query);
//            foreach (var item in list)
//            {
//                Debug.WriteLine("459: "+item);
////                if (IsUserAssosiacedWithComboItemById(item.Value, id))
////                {
////                    item.Selected = true;
////                }
//            }
            return list;
        }
        private bool IsUserAssosiacedWithComboItemById(string comboItemName, string userid)
        {
            var comboitem  = _dbContext.ComboItem.FirstOrDefault(x => x.Name == comboItemName);
            if (comboitem == null)
            {
                Debug.WriteLine("comboitem with name "+comboItemName+" not found");
                return false;
            }
            var user = _userManager.FindById(userid);
            var adadada = (comboitem.ApplicationUsers.FirstOrDefault(x => x == user));
            if (comboitem.ApplicationUsers.FirstOrDefault(x => x == user) != null) return true;
            else return false;
        }

        public bool UpdateComboItemsforUser(string id, string[] comboItems)
        {
            //if nothing is chosen, remove all
            if (comboItems == null)
            { 
                RemoveAllComboItemsFromUser(id);
                return true;
            }

            //int count = comboItems.Length;
            ////if the list is the same, and the user just pressed Save without changing anything!
            //var allComboItemsAssignedToUserList = AllComboItemsListForUser(id);
            
            //foreach (var comboItemAssignedToUser in allComboItemsAssignedToUserList)
            //{
            //    if (comboItems.Contains(comboItemAssignedToUser.Name))
            //        count--;
            //}
            //if (count == 0) return true;

            foreach (var comboItem in comboItems)
            { //an den einai assoc me to item, prosthese ton
                if (!IsUserAssosiacedWithComboItemById(comboItem, id))
                { 
                    AddComboItemToUser(id,comboItem); 
                }
            }
            // make a list of strings, with all the names of the ComboItems
            List<string> allComboItemNamesinDb = new List<string>();
            foreach (var a in GetAllComboItems())
            {   allComboItemNamesinDb.Add(a.Name);
            }
            
            //all the comboitems in the application, except the selection given here
            // we wish the removal OR the non association of the user and the ComboItem
            IEnumerable<string> notinALLITEMslist = allComboItemNamesinDb.Except(comboItems);  
            foreach (var comboItem in notinALLITEMslist)
            {//if the user used to be associated with the combo item, remove the association, 
                // (because the new state is not selected in the list)
                if (IsUserAssosiacedWithComboItemById(comboItem, id))
                {
                    RemoveComboItemFromUser(id, comboItem);
                }
                // else the user was not assoc. and we dont want to associate him, nothing needed to be done here then
            }
            return true;
        }

        private void RemoveAllComboItemsFromUser(string id)
        {
            //the comboitem the user with id == id is associated with
            var user = _userManager.Users.FirstOrDefault(r => r.Id == id);
            var comboItems = AllComboItemsListForUser(id);
             
            foreach (var comboItem in comboItems)
            {
                user.ComboItems.Remove(comboItem);
            }
            //var s = _dbContext.ComboItem.FirstOrDefault(x => x.ApplicationUsers == _userManager.Users.FirstOrDefault(f => f.Id == id));
            
            _dbContext.SaveChanges();
        }

        public IEnumerable<SelectListItem> AllRolesToIenumSelectListItems()
        {
            List<SelectListItem> list = null;
            var query = (from ca in _dbContext.ComboItem
                         orderby ca.Name
                         select new SelectListItem { Text = ca.Name, Value = ca.Name }).Distinct();
            list = query.ToList();
            Debug.WriteLine("-- ComboItems nu", list.Count);
            return list;
        }

        public IEnumerable<SelectListItem> GetAllRolesForUserIdToIenumSelectListItem(string id)
        {
            List<SelectListItem> list = new List<SelectListItem>();
//            var query = from ca in _dbContext.Users.Where(x => x.Id == id)
//                        select new SelectListItem {Text = ca.Id, Value = ca.UserName};
//            list = query.ToList();
            var allComboItems = GetAllComboItems();
             
            foreach (var usersComboItem in allComboItems)
            {
                SelectListItem x = new SelectListItem()
                {
                    Value = usersComboItem.Name,
                    Text = usersComboItem.Name,
                    Selected = IsUserAssosiacedWithComboItemById(usersComboItem.Name,id)
                };
                list.Add(x);
            }
            
            Debug.WriteLine("-- ComboItems nu", list.Count);
            return list;
        } 

        private ComboItem GetComboItemById(string id)
        {
            return _dbContext.ComboItem.FirstOrDefault(x => x.CombooItemId == id);
        }
    }
}