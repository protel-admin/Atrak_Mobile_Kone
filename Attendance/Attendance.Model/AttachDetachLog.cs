using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class AttachDetachLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string StaffId { get; set; }

        [Required]
        public bool IsAttached { get; set; }

        [MaxLength(10)]
        public string ReportingManager { get; set; }

        [Required]
        public DateTime? StateChangedOn { get; set; }
    }
}
