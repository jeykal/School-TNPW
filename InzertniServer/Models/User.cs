using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace InzertniServer.Models
{
    public class User
    {
        [ScaffoldColumn(false)]
        public Role Role { get; set; }

        [ScaffoldColumn(false)]
        public int UserId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(35)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Please Enter Your Login", AllowEmptyStrings = false)]
        public string Login { get; set; }

        [Required(ErrorMessage = "You Cannot Login Without a Password!", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Column(TypeName =  "VARCHAR")]
        [StringLength(35)]
        [Required(ErrorMessage = "Please Enter Your First Name", AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        [Column(TypeName =  "VARCHAR")]
        [StringLength(35)]
        [Required(ErrorMessage = "Please Enter Your Last Name", AllowEmptyStrings = false )]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Provide a Way Of Contancing You Via Email or a Phone Number.", AllowEmptyStrings = false), DataType(DataType.MultilineText)]
        public string Connection { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Listing> Listings { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Comment> Comments { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Report> Reports { get; set; }
    }
}