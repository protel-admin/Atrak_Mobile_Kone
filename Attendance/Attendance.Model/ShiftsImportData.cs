using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Model
{
    public class ShiftsImportData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string StaffId { get; set; }
        [Required]
        [MaxLength(10)]
        public string ShiftId { get; set; }
        [Required]
        public DateTime ShiftFromDate { get; set; }
        [Required]
        public DateTime ShiftToDate { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool IsProcessed { get; set; }
        [Required]
        public DateTime? CreatedOn { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        public DateTime? ProcessedOn { get; set; }
    }
}
