using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class LeaveApplication
    {
        //FOREIGN KEY
        public string StaffId { get; set; }
        public string LeaveTypeId { get; set; }
        public int DurationId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(10)]
        [Required]
        public string Id { get; set; }

        [Required]
        public DateTime ? LeaveStartDate { get; set; }

        [Required]
        public DateTime ? LeaveEndDate { get; set; }

        [Required]
        public DateTime ? ApplicationDate { get; set; }

        [MaxLength(200)]
        [Required]
        public string LeaveReason { get; set; }

        [MaxLength(20)]
        [Required]
        public string ContactNumber { get; set; }
	    
        public bool IsCancelled { get; set; }

        //public int AbsenceApprovalId { get; set; }
        
        public DateTime ? CreatedOn { get; set; }
        public DateTime ? ModifiedOn { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }

        [ForeignKey ( "LeaveTypeId" )]
        public virtual LeaveType LeaveType { get; set; }

        [ForeignKey("DurationId")]
        public virtual LeaveDuration LeaveDuration { get; set; }
        
        [ForeignKey("CreatedBy")]
        public virtual Staff CreatedStaff { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual Staff ModifiedStaff { get; set; }

        //[ForeignKey("AbsenceApprovalId")]
        //public virtual AbsenceApproval AbsenceApproval { get; set; }

    }

    public class LeaveBalanceList {
        public string LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public Decimal LeaveBalance { get; set; }
        public bool IsCommon { get; set; }
        public bool IsPermission { get; set; }
    }

    public class LeaveApplicationHistory
    {
        public string LeaveApplicationId { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string LeaveTypeId { get; set; }
        public string LeaveName { get; set; }
        public int DurationId { get; set; }
        public string DurationName { get; set; }
        public string LeaveReason { get; set; }
        public int LeaveApprovalStatus { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStatusById { get; set; }
        public string ApprovalStatusByName { get; set; }
        public string LeaveStartDate { get; set; }
        public string LeaveEndDate { get; set; }
        public string ContactNumber { get; set; }
        public bool IsCancelled { get; set; }
        public string Comment { get; set; }
        public string IsCancelledText { get; set; }
        public string ApplicationDate { get; set; }
        public int Id { get; set; }
        public bool IsPermission { get; set; }
        public string IsPermissionText { get; set; }
        public int StaffStatusId { get; set; }
        public string StaffStatusName { get; set; }
        public int AbsenceApprovalId { get; set; }
    }
}