using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InzertniServer.Models
{
    public class Photo
    {
        public Listing Listing { get; set; }

        public int PhotoId { get; set; }

        public string PhotoName { get; set; }
    }
}