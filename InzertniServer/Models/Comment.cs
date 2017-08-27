using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InzertniServer.Models
{
    public class Comment
    {

        public User User { get; set; }

        public int CommentId { get; set; }

        public DateTime Created { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }

        public Comment()
        {
            Created = DateTime.Now;
        }
    }
}