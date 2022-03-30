using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
   public class PunchTypeHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(40)]
        [Required]
        public string StaffId { get; set; }

        [MaxLength(10)]
        public string LastPunchType { get; set; }

    }
}
