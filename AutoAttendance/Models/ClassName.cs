using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoAttendance.Models
{
    public class ClassName
    {
        [Key]
        [Column(Order = 10)]
        public string ApplicationUserId { get; set; }
        [Key]
        [Display(Name = "Class")]
        [Column(Order = 20)]
        public string CName { get; set; }
    }
}