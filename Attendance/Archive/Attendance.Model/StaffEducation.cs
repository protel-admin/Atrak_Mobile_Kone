using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class StaffEducation
    {
        //FOREIGN KEY.
        public string StaffId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(10)]
        public string Id { get; set; }

        
        [MaxLength(50)]
        public string CourseName { get; set; }

        
        [MaxLength(50)]
        public string University { get; set; }
	    
        public bool Completed { get; set; }
        public int CompletionYear { get; set; }
        public decimal Percentage { get; set; }

        
        [MaxLength(5)]
        public string Grade { get; set; }
        //[MaxLength(5)]
        //public string Test { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }    
    }

    public class StaffEducationInformation
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string CourseName { get; set; }
        public string University { get; set; }
        public bool Completed { get; set; }
        public int CompletionYear { get; set; }
        public decimal Percentage { get; set; }
        public string Grade { get; set; }
    }
}