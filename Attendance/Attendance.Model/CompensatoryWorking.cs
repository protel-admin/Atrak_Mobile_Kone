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
    public class CompensatoryWorking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime? LeaveDate { get; set; }
        [Required]
        public DateTime? CompensatoryWorkingDate { get; set; }
        [MaxLength(500)]
        public string Reason { get; set; }
        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
    }
}
