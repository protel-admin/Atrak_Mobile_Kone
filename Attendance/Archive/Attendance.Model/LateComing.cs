using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class LateComing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(10)]
        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string StaffId { get; set; }

        [Required]
        public DateTime? TxnDate { get; set; }

        [Required]
        public DateTime? ShiftIn { get; set; }

        [Required]
        public DateTime? ShiftOut { get; set; }

        public DateTime? SwipeIn { get; set; }
        public DateTime? SwipeOut { get; set; }
        public bool IsLate { get; set; }
        public bool IsEarly { get; set; }
        public DateTime? LateHours { get; set; }
        public DateTime? EarlyHours { get; set; }
        public bool IsAbsentMarked { get; set; }
        public bool IsLeaveDeducted { get; set; }

    }
}
