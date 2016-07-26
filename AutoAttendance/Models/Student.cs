using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoAttendance.Models
{
    public class Student
    {
        [Key]
        [Column(Order = 10)]
        public string ApplicationUserId { get; set; }
        [Key]
        [Display(Name="Class")]
        [Column(Order = 20)]
        public string ClassName { get; set; }
        [Key]
        [Column(Order = 30)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string present { get; set; }
    }
}