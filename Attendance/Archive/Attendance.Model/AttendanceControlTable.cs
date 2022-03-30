using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Attendance.Model;
namespace Attendance.Model
{
    public class AttendanceControlTable
    {
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string StaffId { get; set; }

        [Required]
        public DateTime ?  FromDate { get; set; }

        [Required]
        public DateTime? ToDate { get; set; }

        [Required]
        public bool IsProcessed { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        [MaxLength(20)]
        public string CreatedBy { get; set; }


        public string ApplicationType { get; set; }


        public string ApplicationId { get; set; }

    }
}
