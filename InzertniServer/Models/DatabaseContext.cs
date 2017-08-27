using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InzertniServer.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Listing> Listings { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public  DbSet<Report> Reports { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }


    }
}