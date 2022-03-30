using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class RHApplication
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id {get;set;}
        [Required]
        public string StaffId{get;set;}
        [Required]
        public int RHId{get;set;}
        [Required]
        public DateTime? ApplicationDate{get;set;}
        [Required]
        public bool IsCancelled{get;set;}
        public DateTime? CreatedOn{get;set;}
        public string CreatedBy{get;set;}
        public DateTime? ModifiedOn{get;set;}
        public string ModifiedBy { get; set; }
        [ForeignKey("StaffId")]
        public virtual Staff Staff{get;set;}
        [ForeignKey("RHId")]
        public virtual RestrictedHolidays RestrictedHolidays{get;set;}
    }

    public class RHApplicationList
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public int RHId { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
