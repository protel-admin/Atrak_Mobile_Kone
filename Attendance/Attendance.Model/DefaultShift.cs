using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class DefaultShift
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string ParentId { get; set; }
        [Required]
        [MaxLength(6)]
        public string ShiftId { get; set; }
        [Required]
        public int Priority { get; set; }
    }


}
