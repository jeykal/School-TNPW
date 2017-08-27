using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InzertniServer.Models
{
    public class Report
    {
        public User User { get; set; }

        [ScaffoldColumn(false)]
        public int ReportId { get; set; }

        [Required(ErrorMessage = "Please Enter The Cause of The Report.", AllowEmptyStrings = false), DataType(DataType.MultilineText)]
        public string Cause { get; set; }

    }
}