using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using InzertniServer.Models;

namespace InzertniServer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DatabaseContext>());
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //seed
            using (DatabaseContext db = new DatabaseContext())
            {
                if (!db.Roles.Any())
                {
                    db.Roles.AddOrUpdate(x => x.RoleId,
                        new Role() { RoleId = 1, RoleType = "admin" },
                        new Role() { RoleId = 2, RoleType = "user" });
                    db.SaveChanges();
                }
                if (!db.Categories.Any())
                {
                    db.Categories.AddOrUpdate(x => x.CategoryId,
                        new Category() { CategoryId = 1, CategoryName = "Elektro" },
                        new Category() { CategoryId = 2, CategoryName = "Dům a zahrada" },
                        new Category() { CategoryId = 3, CategoryName = "Auto-moto" },
                        new Category() { CategoryId = 4, CategoryName = "Služby" },
                        new Category() { CategoryId = 5, CategoryName = "Oblečení, doplňky" },
                        new Category() { CategoryId = 6, CategoryName = "Hobby" });
                    db.SaveChanges();
                }
                if (!db.Users.Any())
                {
                    var password = "admin";
                    var sha1 = new SHA1CryptoServiceProvider();

                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                    MemoryStream passwordStream = new MemoryStream(sha1.ComputeHash(passwordBytes));
                    StreamReader passwordReader = new StreamReader(passwordStream);
                    password = passwordReader.ReadToEnd();

                    db.Users.AddOrUpdate(x => x.UserId,
                        new User() { UserId = 1, FirstName = "admin", LastName = "admin", Login = "admin", Password = password, Connection = "admin", Role = db.Roles.FirstOrDefault(x => x.RoleId.Equals(1)) });
                    db.SaveChanges();
                }
            }
        }
    }
}

/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace InzertniServer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
*/
