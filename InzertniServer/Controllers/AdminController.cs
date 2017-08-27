using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InzertniServer.Models;

namespace InzertniServer.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageUsers()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var users = db.Users.Include("Role").ToList();
                return View(users);
            }
        }

        public ActionResult ManageListings(int id)
        {
            DatabaseContext db = new DatabaseContext();
            var user = db.Users.Find(id);
            return View(user);
        }

        public ActionResult CreateAdmin()
        {
            return RedirectToAction("CreateAdmin", "User");
        }

        public ActionResult DeleteUserAdmin(int id)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                ICollection<Listing> temporaryListings = new List<Listing>();
                ICollection<Photo> temporaryPhotos = new List<Photo>();
                ICollection<Comment> temporaryComments = new List<Comment>();
                ICollection<Report> temporaryReports = new List<Report>();

                var user = db.Users.FirstOrDefault(x => x.UserId.Equals(id));
                if (user.Listings.Count > 0)
                {
                    foreach (var listing in user.Listings)
                    {
                        if (listing.Photos.Count > 0)
                        {
                            foreach (var photo in listing.Photos)
                            {
                                System.IO.File.Delete(Server.MapPath("~/uploads/photos" + photo.PhotoName));
                                temporaryPhotos.Add(photo);
                            }
                        }
                        temporaryListings.Add(listing);
                    }
                }

                if (user.Comments.Count > 0)
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
                db.Listings.RemoveRange(temporaryListings);
                db.Photos.RemoveRange(temporaryPhotos);
                db.Comments.RemoveRange(temporaryComments);
                db.Reports.RemoveRange(temporaryReports);
                db.Users.Remove(user);
                db.SaveChanges();
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
        }

        public ActionResult DeleteListingAdmin(int id)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                ICollection<Photo> temporaryPhotos = new List<Photo>();
                var listing = db.Listings.Include("User").FirstOrDefault(x => x.ListingId.Equals(id));
                var userId = listing.User.UserId;
                if (listing.Photos.Count > 0)
                {
                    foreach (var photo in listing.Photos)
                    {
                        System.IO.File.Delete(Server.MapPath("~/uploads/listing" + photo.PhotoName));
                        temporaryPhotos.Add(photo);
                    }
                }
                db.Photos.RemoveRange(temporaryPhotos);
                db.Listings.Remove(listing);
                db.SaveChanges();
                return RedirectToAction("ManageListings", new {id = userId});
            }
        }

        public ActionResult ShowReports()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var reports = db.Reports.Include("User").ToList();
                return View(reports);
            }
        }

        public ActionResult DeleteReport(int id)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var report = db.Reports.Find(id);
                db.Reports.Remove(report);
                db.SaveChanges();
                return RedirectToAction("ShowReports");
            }
        }




    }



}