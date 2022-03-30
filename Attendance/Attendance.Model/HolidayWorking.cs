using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class HolidayWorking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string StaffId { get; set; }
        [MaxLength(10)]
        public string ShiftId { get; set; }
        public DateTime? TxnDate { get; set; }
        public DateTime? ShiftInDate { get; set; }
        public DateTime? ShiftInTime { get; set; }
        public DateTime? ShiftOutDate { get; set; }
        public DateTime? ShiftOutTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }

    }
    public class HolidayWorkingList
    {
        public string StaffId { get; set; }
        public string ShiftId { get; set; }
        public DateTime? TxnDate { get; set; }
        public DateTime? ShiftInDate { get; set; }
        public DateTime? ShiftInTime { get; set; }
        public DateTime? ShiftOutDate { get; set; }
        public DateTime? ShiftOutTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
