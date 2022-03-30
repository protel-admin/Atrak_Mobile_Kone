using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class PermissionTxn
    {
        public string StaffId { get; set; }
        public Int64 PermissionId { get; set; }
        public int ApprovalStatusId { get; set; }
        public string ApprovalStatusBy { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Int64 Id { get; set; }
        
        [Required]
        public DateTime? StartTime { get; set; }
        
        [Required]
        public DateTime? EndTime { get; set; }

        [Required]
        public DateTime? ApplicationDate { get; set; }
        
        [MaxLength(200)]
        [Required]
        public string Reason { get; set; }

        [Required]
        public bool IsCancelled { get; set; }

        [Required]
        public DateTime? ApprovalStatusOn { get; set; }
        
        [MaxLength(200)]
        [Required]
        public string Comment { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        [Required]
        public DateTime? ModifiedOn { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }

        [ForeignKey("PermissionId")]
        public virtual PermissionType PermissionType { get; set; }
        
        [ForeignKey("ApprovalStatusId")]
        public virtual ApprovalStatus ApprovalStatus{ get; set; }

        [ForeignKey("ApprovalStatusBy")]
        public virtual Staff ApprovalStaff { get; set; }
        
        [ForeignKey("CreatedBy")]
        public virtual Staff CreatedStaff { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual Staff ModifiedStaff { get; set; }

    }
}