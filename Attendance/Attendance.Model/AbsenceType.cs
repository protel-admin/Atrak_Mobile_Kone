using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model {
    public class AbsenceType {
        [Key]
        [DatabaseGenerated ( DatabaseGeneratedOption.Identity )]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength ( 50 )]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
