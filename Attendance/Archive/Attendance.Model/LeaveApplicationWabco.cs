using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model {
    public class LeaveApplicationWabco {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(10)]
        public string Id { get; set; }
        [Required]
        [MaxLength ( 20 )]
        public string StaffId { get; set; }
        [Required]
        [MaxLength ( 6 )]
        public string LeaveTypeId { get; set; }
        [Required]
        public DateTime ? LeaveStartDate { get; set; }
        [Required]
        public int LeaveStartDurationId { get; set; }
        [Required]
        public DateTime ? LeaveEndDate { get; set; }
        [Required]
        public int LeaveEndDurationId { get; set; }
        [Required]
        [MaxLength ( 200 )]
        public string Remarks { get; set; }
        //[Required]
        //[MaxLength(200)]
        //public string Reason { get; set; }
        [Required]
        public decimal TotalDays { get; set; }

        [Required]
        public int ReasonId { get; set; }

        //[Required]
        [MaxLength ( 15 )]
        public string ContactNumber { get; set; }

        public bool IsCancelled { get; set; }

        [ForeignKey ( "LeaveTypeId" )]
        public virtual LeaveType LeaveType { get; set; }

        [ForeignKey ( "LeaveStartDurationId" )]
        public virtual LeaveDuration LeaveStartDuration { get; set; }

        [ForeignKey ( "LeaveEndDurationId" )]
        public virtual LeaveDuration LeaveEndDuration { get; set; }
        
        [ForeignKey("ReasonId")]
        public virtual LeaveReason LeaveReason { get; set; }

    }

}
