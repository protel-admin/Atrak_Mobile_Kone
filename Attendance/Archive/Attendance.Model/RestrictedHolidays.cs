using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class RestrictedHolidays
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime? RHDate { get; set; }

        [Required]
        public int RHYear { get; set; }

        [Required]
        public string CompanyId { get; set; }

        [Required]
        public string LeaveId { get; set; }
        
        [Required]
        public DateTime? ImportDate { get; set; }
        
        [Required]
        public string ImportedBy { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [ForeignKey("LeaveId")]
        public virtual LeaveType LeaveType { get; set; }
    }

    public class RestrictedHolidayList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string RHDate { get; set; }
        public string RHYear { get; set; }
        public string CompanyId { get; set; }
        public string LeaveId { get; set; }
        public string ImportDate { get; set; }
        public string ImportedBy { get; set; }
        public string CompanyName { get; set; }
    }
}
