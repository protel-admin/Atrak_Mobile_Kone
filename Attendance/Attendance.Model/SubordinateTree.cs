using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class SubordinateTree
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string StaffId { get; set; }
       
        [MaxLength(20)]
        [Required]
        public string ReportingStaffId { get; set; }

        [MaxLength(30)]
        [Required]
        public string Signature { get; set; }

    }
}
