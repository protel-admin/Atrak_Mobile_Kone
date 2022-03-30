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
    public class ManualPunch
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [MaxLength(10)]
        public string Id { get; set; }

        //FOREIGN KEY
        public String StaffId { get; set; }

        [Required]
        public DateTime? InDateTime { get; set; }

        
        [Required]
        public DateTime? OutDateTime { get; set; }

        [Required]
        [MaxLength ( 200 )]
        public string Reason{ get; set; }

        [Required]
        public bool IsCancelled { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }

        public DateTime? CreatedOn{ get; set; }

        public String CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public String ModifiedBy { get; set; }

        public string PunchType { get; set; }
    }

    public class ManualPunchList
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string InDate { get; set; }
        public string InTime { get; set; }
        public string OutDate { get; set; }
        public string OutTime { get; set; }
        public string Reason { get; set; }
        public string StatusId { get; set; }
        public string Status { get; set; }
        public string PunchType { get; set; }
    }
    public class Manualpunchforsmax
    {
       
        public string StaffId { get; set; }
        public DateTime Indatetime { get; set; }
        public DateTime Outdatetime { get; set; }
        public string PunchType { get; set; }
    }
}