using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class BenchReportingManager
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string StaffId { get; set; }
        
        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        [MaxLength(10)]
        public string CreatedBy { get; set; }
    }
}
