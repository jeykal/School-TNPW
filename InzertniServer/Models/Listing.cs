using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace InzertniServer.Models
{

    public class Listing
    {
        public User User { get; set; }
        public Category Category { get; set; }

        [ScaffoldColumn(false)]
        public int ListingId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(150)]
        [Required(ErrorMessage = "Please Input a Name For The Listing.", AllowEmptyStrings = false)]
        public string ListingTitle { get; set; }

        [Required(ErrorMessage = "Please Input a Price For The Listing.", AllowEmptyStrings = false)]
        public int ListingPrice { get; set; }

        [Required(ErrorMessage = "Please Input Tags For The Listing", AllowEmptyStrings = false),
         DataType(DataType.MultilineText)]
        public string ListingTags { get; set; }

        [ScaffoldColumn(false)]
        public DateTime ListingDate { get; set; }

        [Required(ErrorMessage = "Please Let Us Know Where You're Listing from")]
        public string Area { get; set; }

        [Required(ErrorMessage = "Please Input Description For The Listing", AllowEmptyStrings = false), DataType(DataType.MultilineText)]
        public string ListingDescription { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Photo> Photos { get; set; }

        public Listing()
        {
            ListingDate = DateTime.Now;
        }
        
    }
}