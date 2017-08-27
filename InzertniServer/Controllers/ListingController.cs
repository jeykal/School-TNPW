using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InzertniServer.Models;

namespace InzertniServer.Controllers
{
    public class ListingController : Controller
    {
        [Authorize]
        public ActionResult CreateListing()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                ViewBag.categories = db.Categories.ToList();
                return View();
            }
           
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddListing(Listing listing, int categoryId, IEnumerable<HttpPostedFileBase> pictures)
        {
            if (ModelState.IsValid)
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    listing.User = db.Users.FirstOrDefault();
                    listing.Category = db.Categories.Find(categoryId);

                    foreach (var picture in pictures)
                    {
                        if (picture != null && picture.ContentLength != 0)
                        {
                            if (picture.ContentType == "image/png" || picture.ContentType == "image/jpg" ||
                                picture.ContentType == "image/jpeg")
                            {
                                using (Bitmap bitmap = new Bitmap(Image.FromStream(picture.InputStream)))
                                {
                                    Guid guid = Guid.NewGuid();
                                    string pictureName = guid.ToString() + ".jpg";
                                    bitmap.Save(Server.MapPath("~/uploads/photos/" + pictureName), ImageFormat.Jpeg); //png unnecessarily big

                                    Photo photo = new Photo();
                                    photo.PhotoName = pictureName;
                                    photo.Listing = listing;
                                    db.Photos.Add(photo);
                                }
                            }
                        }
                        
                    }

                    db.Listings.Add(listing);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return View("CreateListing", listing);
        }

        public ActionResult ListingDetail(int id)
        {
            DatabaseContext db = new DatabaseContext();

            var listing = db.Listings.Include("User").Include("Category").FirstOrDefault(x => x.ListingId.Equals(id));
            return View(listing);
        }

        [Authorize]
        public ActionResult EditListing(int id)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var currentUser = db.Users.FirstOrDefault(x => x.Login.Equals(User.Identity.Name));
                var listing = db.Listings.Include("User").FirstOrDefault(x => x.ListingId.Equals(id));

                if (listing.User.UserId != currentUser.UserId)
                {
                    return RedirectToAction("Login", "Login");
                }

                return View(listing);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateListing(Listing uListing)
        {
            if (ModelState.IsValid)
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    db.Entry(uListing).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                TempData["error-editListing"] = "An Error Has Occured, Try Again.";
                return View("EditListing", uListing);
            }
            TempData["success-editListing"] = "Update Occurred Successfuly.";
            return View("EditListing", uListing);
        }

        [Authorize]
        public ActionResult DeleteListing(int id)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                ICollection<Photo> temporaryPhotos = new List<Photo>();
                var currentUser = db.Users.FirstOrDefault(x => x.Login.Equals(User.Identity.Name));
                var listing = db.Listings.Include("User").FirstOrDefault(x => x.ListingId.Equals(id));

                if (listing.User.UserId != currentUser.UserId)
                {
                    return RedirectToAction("Login", "Login");
                }

                if (listing.Photos.Count > 0)
                {
                    foreach (var photo in listing.Photos)
                    {
                        System.IO.File.Delete(Server.MapPath("~/uploads/photos/" + photo.PhotoName));
                        temporaryPhotos.Add(photo);
                    }
                }
                db.Photos.RemoveRange(temporaryPhotos);
                db.Listings.Remove(listing);
                db.SaveChanges();

                return RedirectToAction("ShowProfile", "User");
            }
        }
        




    }
}