using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class PrefixSuffixSetting
    {
        public string LeaveTypeId { get; set; }
        public string PrefixLeaveTypeId { get; set; }
        public string SuffixLeaveTypeId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        
        [Required]
        public bool IsActive { get; set; }

        [ForeignKey("LeaveTypeId")]
        public virtual LeaveType LeaveType { get; set; }

        [ForeignKey("PrefixLeaveTypeId")]
        public virtual LeaveType PrefixLeaveType { get; set; }

        [ForeignKey("SuffixLeaveTypeId")]
        public virtual LeaveType SuffixLeaveType { get; set; }

    }

    public class PrefixSuffixSettingList {
        public int Id { get; set; }
        public string LeaveTypeId { get; set; }
        public string PrefixLeaveTypeId { get; set; }
        public string SuffixLeaveTypeId { get; set; }
        public bool IsActive { get; set; }
    }
}