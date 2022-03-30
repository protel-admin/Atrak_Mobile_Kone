using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class LeaveDebits
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string ParentId { get; set; }

        [Required]
        [MaxLength(20)]
        public string StaffId { get; set; }

        [Required]
        [MaxLength(10)]
        public string LeaveTypeId { get; set; }

        [Required]
        [MaxLength(5)]
        public string LeaveType { get; set; }

        [Required]
        public decimal TotalDays { get;set; }

        [Required]
        [MaxLength(1)]
        public string DCFlag { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        [Required]
        public bool IsProcessed { get; set; }

        public DateTime? ProcessedOn { get; set; }

    }
}
