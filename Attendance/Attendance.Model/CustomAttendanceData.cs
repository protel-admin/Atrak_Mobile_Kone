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
    public class CustomAttendanceData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(10)]
        [Required]
        public string Id { get; set; }

        [MaxLength(20)]
        [Required]
        public string StaffId { get; set; }

        public string LoggedInUserId { get; set; }

        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        [MaxLength(10)]
        [Required]
        public string NewShiftId { get; set; }

        public string NewShiftName { get; set; }

        public string Reason { get; set; }

        public bool IsCancelled { get; set; }

        public DateTime? ShiftInTime { get; set; }

        public DateTime? ShiftOutTime { get; set; }

        public DateTime? ExpectedWorkingHours { get; set; }


    }
}
