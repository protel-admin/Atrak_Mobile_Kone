using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model {
    public class Rule {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int id { get; set; }
        [MaxLength ( 50 )]
        [Required]
        public string name { get; set; }
        [MaxLength ( 200 )]
        [Required]
        public string description { get; set; }
        [MaxLength ( 50 )]
        [Required]
        public string datatype { get; set; }
        [MaxLength ( 50 )]
        [Required]
        public string ruletype { get; set; }
        [Required]
        public bool isactive { get; set; }
    }

    public class RuleList {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string datatype { get; set; }
        public string ruletype { get; set; }
        public bool isactive { get; set; }
    }
}
