using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using InzertniServer.Models;

namespace InzertniServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var lists = db.Listings.Include("User").Include("Category").ToList();
                ViewBag.categories = db.Categories.ToList();
                return View(lists);
            }
           
        }

        public ActionResult SearchFilter(int? id, string searchQuery)
        {
            DatabaseContext db = new DatabaseContext();
            ViewBag.categories = db.Categories.ToList();

            // searchQuery and Category Filter
            if ((!string.IsNullOrEmpty(searchQuery)) && (id != null))
            {
                int categoryId = Convert.ToInt32(id);
                ViewBag.categories = db.Categories.ToList();
                var currentCategory = db.Categories.FirstOrDefault(x => x.CategoryId.Equals(categoryId));
                ViewBag.currentCategoryId = currentCategory.CategoryId;
                ViewBag.currentQuery = searchQuery;
                var listings = db.Listings.Where(x => (x.ListingTitle.Contains(searchQuery) || x.ListingDescription.Contains(searchQuery) || x.ListingTags.Contains(searchQuery)) && x.Category.CategoryId.Equals(categoryId)).Include("User").Include("Category").ToList();
                return View("Index", listings);
            }
            // search Filter
            if (!string.IsNullOrEmpty(searchQuery))
            {
                ViewBag.currentQuery = searchQuery;
                var listings = db.Listings.Where(x => x.ListingTitle.Contains(searchQuery) || x.ListingDescription.Contains(searchQuery) || x.ListingTags.Contains(searchQuery)).Include("User").Include("Category").ToList();
                return View("Index", listings);
            }

            // category Filter
            if (id != null)
            {
                int categoryId = Convert.ToInt32(id);
                var currentCategory = db.Categories.FirstOrDefault(x => x.CategoryId.Equals(categoryId));
                ViewBag.currentCategoryId = currentCategory.CategoryId;
                var listings = db.Listings.Where(x => x.Category.CategoryId.Equals(categoryId)).Include("User").Include("Category").ToList();
                return View("Index", listings);
            }
        
            return View("Index");
        }

    }
}