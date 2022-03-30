using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model {
    public class WorkingDayPattern {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(5)]
        public string PsCode { get; set; }

        [Required]
        public double WorkingPattern { get; set; }

        [Required]
        public string PatternDesc { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }

    public class WorkingDayPatternList
    {
        public int Id { get; set; }
        public string PsCode { get; set; }
        public double WorkingPattern { get; set; }
        public string PatternDesc { get; set; }
        public bool IsActive { get; set; }
    }
}
