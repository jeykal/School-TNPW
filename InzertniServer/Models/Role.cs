using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InzertniServer.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        public string RoleType { get; set; }

        public virtual ICollection<User> Users { get; set; }

    }
}