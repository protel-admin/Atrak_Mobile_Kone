using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class WeeklyOffs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength ( 10 )]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int Settings { get; set; }
        public bool IsActive { get; set; }
        
        public DateTime? CreatedOn {get;set;}

        [MaxLength(50)]
        public string CreatedBy {get;set;}

        public DateTime? ModifiedOn {get;set;}

        [MaxLength(50)]
        public string ModifiedBy{get;set;}


    }

    public class WeeklyOffList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Settings { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}