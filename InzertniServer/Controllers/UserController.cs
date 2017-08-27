using InzertniServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;

namespace InzertniServer.Controllers
{
    public class UserController : Controller
    {

        [Authorize(Roles = "admin")]
        public ActionResult CreateAdmin()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult AddAdmin(User user)
        {
            if (ModelState.IsValid)
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    user.Role = db.Roles.FirstOrDefault(x => x.RoleId.Equals(1));
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
            else
            {
                TempData["error-createUser"] = "Registration Failed, Please Try Again.";
                return View("CreateUser", user);
            }
            TempData["success-createUser"] = "Admin Registration Successful.";
            return View("CreateUser");
        }




        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        //[CaptchaValidation("CaptchaCode", "Register Captcha", "Incorret Captcha Code!")]
        public ActionResult AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                var sha1 = new SHA1CryptoServiceProvider();
                byte[] passwordBytes = Encoding.UTF8.GetBytes(user.Password);

                MemoryStream passwordStream = new MemoryStream(sha1.ComputeHash(passwordBytes));
                StreamReader passwordReader = new StreamReader(passwordStream);
                user.Password = passwordReader.ReadToEnd();
                using (DatabaseContext db = new DatabaseContext())
                {
                    user.Role = db.Roles.FirstOrDefault(x => x.RoleId.Equals(2));
                    db.Users.Add(user);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        TempData["error_loginAlreadyExists"] = "Login Already Exists, Pick Something Else.";
                        return View("CreateUser", user);
                    }
                }
            }
            else
            {
                TempData["error-createUser"] = "Registration Failed, Please Try Again.";
            }
            TempData["success-createUser"] = "Registration Successful, Now You Can Login.";
            return View("CreateUser", user);
        }

        [Authorize]
        public ActionResult EditUser()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var user = db.Users.FirstOrDefault(x => x.Login.Equals(User.Identity.Name));
                return View(user);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateUser(User updatedUser)
        {
            if (ModelState.IsValid)
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    db.Entry(updatedUser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                TempData["error-editUser"] = "Detail Change Failed, Try Again.";
                return View("EditUser", updatedUser);
            }
            TempData["success-editUser"] = "Details Changed, Relog.";
            return View("EditUser", updatedUser);
        }

        [Authorize]
        public ActionResult ShowProfile()
        {
            DatabaseContext db = new DatabaseContext();
            var user = db.Users.FirstOrDefault(x => x.Login.Equals(User.Identity.Name));
            return View(user);
        }

        public ActionResult UserDetail(int id)
        {
            DatabaseContext db = new DatabaseContext();
            var user = db.Users.Find(id);
            return View(user);
        }

        [Authorize]
        public ActionResult ReportUser(int id)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var user = db.Users.Find(id);
                return View(user);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult ReportUser(int id, Report newReport)
        {
            if (ModelState.IsValid)
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    newReport.User = db.Users.Find(id);
                    db.Reports.Add(newReport);
                    db.SaveChanges();
                    TempData["success-reportUser"] = "User Has Been Reported";
                    return RedirectToAction("ReportUser", new {id = id});
                }
            }
            TempData["error-reportUser"] = "An Error has Occured During Reporting";
            return ReportUser(id);
        }

        public ActionResult UserDetail(int id, Comment newComment)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                if (ModelState.IsValid)
                {
                    newComment.User = db.Users.Find(id);
                    newComment.Author = User.Identity.Name;
                    db.Comments.Add(newComment);
                    db.SaveChanges();
                    return RedirectToAction("UserDetail", new {id = id});
                }
                return UserDetail(id);
            }
        }

        [Authorize]
        public ActionResult DeleteUser()
        {
            return View();
        }

        [Authorize]
        public ActionResult RemoveUser()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                ICollection<Listing> temporaryListings = new List<Listing>();
                ICollection<Photo> temporaryPhotos = new List<Photo>();
                ICollection<Comment> temporaryComments = new List<Comment>();
                ICollection<Report> temporaryReports = new List<Report>();
                var user = db.Users.FirstOrDefault(x => x.Login.Equals(User.Identity.Name));
                if (user.Listings.Count > 0)
                {
                    foreach (var listing in user.Listings)
                    {
                        if (listing.Photos.Count > 0)
                        {
                            foreach (var photo in listing.Photos)
                            {
                                System.IO.File.Delete(Server.MapPath("~/uploads/photos/" + photo.PhotoName));
                                temporaryPhotos.Add(photo);
                            }
                        }
                        temporaryListings.Add(listing);
                    }
                }
                if(user.Comments.Count > 0)
                {
                    foreach (var comment in user.Comments)
                        {
                            temporaryComments.Add(comment);
                        }
                    }
                if (user.Reports.Count > 0)
                {
                        foreach (var report in user.Reports)
                        {
                            temporaryReports.Add(report);
                        }
                }
                    db.Listings.RemoveRange(user.Listings);
                    db.Photos.RemoveRange(temporaryPhotos);
                    db.Comments.RemoveRange(temporaryComments);
                    db.Reports.RemoveRange(temporaryReports);
                    db.Users.Remove(user);
                    db.SaveChanges();

                    return RedirectToAction("Logout", "Login");
            }
        }



    }
}