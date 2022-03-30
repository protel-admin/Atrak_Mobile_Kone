using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class AttendanceStatusChange
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string StaffId { get; set; }
        [Required]
        public DateTime ShiftDate { get; set; }
        [Required]
        [MaxLength(10)]
        public string Status { get; set; }
        [Required]
        [MaxLength(200)]
        public string Remarks { get; set; }
        public bool IsCancelled { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        [MaxLength(20)]
        public string CreatedBy { get; set; }
    }
}
