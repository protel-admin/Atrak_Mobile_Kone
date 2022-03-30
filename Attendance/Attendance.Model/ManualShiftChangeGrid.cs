using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Attendance.Model
{
    public class ManualShiftChangeGrid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [MaxLength(50)]
        public string StaffId { get; set; }

        public string ShiftId { get; set; }

        public DateTime? TxnDate { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
        
    }
}
