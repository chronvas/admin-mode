using admin_mode.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace admin_mode.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<admin_mode.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "admin_mode.Models.ApplicationDbContext";
        }

        protected override void Seed(admin_mode.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //



            // get managers and stores for roles adding
            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);
            // add roles
            var UserRole = new IdentityRole { Name = "User" };
            manager.Create(UserRole);
            var GARole = new IdentityRole { Name = "Global Admin" };
            manager.Create(GARole);
            var ARole = new IdentityRole { Name = "Admin" };
            manager.Create(ARole);
            var ARole2 = new IdentityRole { Name = "Admin2" };
            manager.Create(ARole2);
            var ARole3 = new IdentityRole { Name = "Admin3" };
            manager.Create(ARole3);
            var ARole4 = new IdentityRole { Name = "Admin4" };
            manager.Create(ARole4);
            var ARole5 = new IdentityRole { Name = "Admin5" };
            manager.Create(ARole5);
            var ARole6 = new IdentityRole { Name = "Admin6" };
            manager.Create(ARole6);
            var ARole7 = new IdentityRole { Name = "Admin7" };
            manager.Create(ARole7);


            // get managers and hashers for users add
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var PasswordHash = new PasswordHasher();

            // seed the default user, and add to a role created above
            if (!context.Users.Any(u => u.Email == "qq@qq.qq"))
            {
                var user1 = new ApplicationUser
                {
                    Email = "qq@qq.qq",
                    UserName = "qq@qq.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 2, 13)
                };
                UserManager.Create(user1);
                UserManager.AddToRole(user1.Id, "User");
            }

            //user 2
            if (!context.Users.Any(u => u.Email == "Global@Admin.com"))
            {
                var user2 = new ApplicationUser
                {
                    Email = "Global@Admin.com",
                    UserName = "Global@Admin.com",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 2, 17)

                };
                UserManager.Create(user2);
                UserManager.AddToRole(user2.Id, "Global Admin");
                UserManager.AddToRole(user2.Id, "User");
                UserManager.AddToRole(user2.Id, "Admin");
                UserManager.AddToRole(user2.Id, "Admin2");
                UserManager.AddToRole(user2.Id, "Admin3");
                UserManager.AddToRole(user2.Id, "Admin4");
                UserManager.AddToRole(user2.Id, "Admin5");
                UserManager.AddToRole(user2.Id, "Admin6");
                UserManager.AddToRole(user2.Id, "Admin7");
            }

            //user 3
            if (!context.Users.Any(u => u.Email == "qq2@qq.qq"))
            {
                var user3 = new ApplicationUser
                {
                    Email = "qq2@qq.qq",
                    UserName = "qq2@qq.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 2, 3)
                };
                UserManager.Create(user3);
                UserManager.AddToRole(user3.Id, "Global Admin");
                UserManager.AddToRole(user3.Id, "User");
            }

            //user 4
            if (!context.Users.Any(u => u.Email == "qq3@qq.qq"))
            {
                var user4 = new ApplicationUser
                {
                    Email = "qq3@qq.qq",
                    UserName = "qq3@qq.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 2, 4)
                };
                UserManager.Create(user4);
                UserManager.AddToRole(user4.Id, "User");
            }

            //user 5
            if (!context.Users.Any(u => u.Email == "qqadmin@qq.qq"))
            {
                var user5 = new ApplicationUser
                {
                    Email = "qqadmin@qq.qq",
                    UserName = "qqadmin@qq.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 2, 5)
                };
                UserManager.Create(user5);
                UserManager.AddToRole(user5.Id, "Admin");
            }

            //user 6
            if (!context.Users.Any(u => u.Email == "qq6@qq.qq"))
            {
                var user6 = new ApplicationUser
                {
                    Email = "qq6@qq.qq",
                    UserName = "qq6@qq.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 2, 6)
                };
                UserManager.Create(user6);
                UserManager.AddToRole(user6.Id, "Admin");
            }

            //user 7
            if (!context.Users.Any(u => u.Email == "qq7@qq.qq"))
            {
                var user7 = new ApplicationUser
                {
                    Email = "qq7@qq.qq",
                    UserName = "qq7@qq.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 2, 6)
                };
                UserManager.Create(user7);
                UserManager.AddToRole(user7.Id, "Admin");
            }

            //user 8
            if (!context.Users.Any(u => u.Email == "qq8@qq.qq"))
            {
                var user8 = new ApplicationUser
                {
                    Email = "qq8@qq.qq",
                    UserName = "qq8@qq.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 2, 7)
                };
                UserManager.Create(user8);
                UserManager.AddToRole(user8.Id, "Admin");
            }

            //user 9
            if (!context.Users.Any(u => u.Email == "qq9@qq.qq"))
            {
                var user9 = new ApplicationUser
                {
                    Email = "qq9@qq.qq",
                    UserName = "qq9@qq.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 2, 8)
                };
                UserManager.Create(user9);
                UserManager.AddToRole(user9.Id, "Admin");
            }

            //user 10
            if (!context.Users.Any(u => u.Email == "qq10@qq.qq"))
            {
                var user10 = new ApplicationUser
                {
                    Email = "qq10@qq.qq",
                    UserName = "qq10@qq.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 2, 9)
                };
                UserManager.Create(user10);
                UserManager.AddToRole(user10.Id, "Admin");
            }

            //user 11
            if (!context.Users.Any(u => u.Email == "qq11@qq.qq"))
            {
                var user11 = new ApplicationUser
                {
                    Email = "qq11@qq.qq",
                    UserName = "qq11@qq.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 2, 10)
                };
                UserManager.Create(user11);
                UserManager.AddToRole(user11.Id, "Admin");
            }

            //user 12
            if (!context.Users.Any(u => u.Email == "qq12@qq.qq"))
            {
                var user12 = new ApplicationUser
                {
                    Email = "qq12@qq.qq",
                    UserName = "qq12@qq.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 2, 11)
                };
                UserManager.Create(user12);

            } 
            
            //user 13
            if (!context.Users.Any(u => u.Email == "elefantas@zougla.qq"))
            {
                var user13 = new ApplicationUser
                {
                    Email = "elefantas@zougla.qq",
                    UserName = "elefantas@zougla.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 4, 1)
                };
                UserManager.Create(user13);
                UserManager.AddToRole(user13.Id, "User");
            }
            
            //user 14
            if (!context.Users.Any(u => u.Email == "feidoulas@zougla.qq"))
            {
                var user14 = new ApplicationUser
                {
                    Email = "feidoulas@zougla.qq",
                    UserName = "feidoulas@zougla.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 4, 1)
                };
                UserManager.Create(user14);
                UserManager.AddToRole(user14.Id, "User");
            }
            
            //user 15
            if (!context.Users.Any(u => u.Email == "krokodeilos@zougla.qq"))
            {
                var user15 = new ApplicationUser
                {
                    Email = "krokodeilos@zougla.qq",
                    UserName = "krokodeilos@zougla.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 4, 11)
                };
                UserManager.Create(user15);
                UserManager.AddToRole(user15.Id, "User");
            }
            
            //user 16
            if (!context.Users.Any(u => u.Email == "vatraxos@zougla.qq"))
            {
                var user16 = new ApplicationUser
                {
                    Email = "vatraxos@zougla.qq",
                    UserName = "vatraxos@zougla.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 6, 13)
                };
                UserManager.Create(user16);
                UserManager.AddToRole(user16.Id, "User");
            }
            
            //user 17
            if (!context.Users.Any(u => u.Email == "ipopokampos@zougla.qq"))
            {
                var user17 = new ApplicationUser
                {
                    Email = "ipopokampos@zougla.qq",
                    UserName = "ipopokampos@zougla.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 6, 13)
                };
                UserManager.Create(user17);
                UserManager.AddToRole(user17.Id, "User");
            }
            
            //user 18
            if (!context.Users.Any(u => u.Email == "kampias@zougla.qq"))
            {
                var user18 = new ApplicationUser
                {
                    Email = "kampias@zougla.qq",
                    UserName = "kampias@zougla.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 6, 13)
                };
                UserManager.Create(user18);
                UserManager.AddToRole(user18.Id, "User");
            }
            
            //user 19
            if (!context.Users.Any(u => u.Email == "savras@zougla.qq"))
            {
                var user19 = new ApplicationUser
                {
                    Email = "savras@zougla.qq",
                    UserName = "savras@zougla.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 6, 13)
                };
                UserManager.Create(user19);
                UserManager.AddToRole(user19.Id, "User");
            }
            
            //user 20
            if (!context.Users.Any(u => u.Email == "papias@zougla.qq"))
            {
                var user20 = new ApplicationUser
                {
                    Email = "papias@zougla.qq",
                    UserName = "papias@zougla.qq",
                    PasswordHash = PasswordHash.HashPassword("qqqqqq"),
                    EnrollmentDate = new DateTime(2013, 3, 23)
                };
                UserManager.Create(user20);
                UserManager.AddToRole(user20.Id, "User");
            }


        }
    }
}
