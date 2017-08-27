using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InzertniServer.Models
{
    public class Category
    { 
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public virtual ICollection<Listing> Listings { get; set; }
    }
}