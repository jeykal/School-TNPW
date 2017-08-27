using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Mvc;
using InzertniServer.Models;

namespace InzertniServer.Controllers
{
    public class PhotoController : Controller
    {
        // GET: Photo
        public void GetThumbnail(int photoId)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var photo = db.Photos.Find(photoId);

                if (photo != null)
                {
                    new WebImage(HostingEnvironment.MapPath(@"~/uploads/photos/" + photo.PhotoName))
                        .Resize(256, 256, true, true).Write();
                }
                else
                {
                    new WebImage(HostingEnvironment.MapPath(@"~/assets/not-available.jpg")).Write();
                }
            }
        }
    }
}