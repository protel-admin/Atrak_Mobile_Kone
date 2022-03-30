using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model {
    public class RuleGroup {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int id { get; set; }

        [MaxLength ( 50 )]
        [Required]        
        public string name { get; set; }
        [Required]
        public bool isactive { get; set; }

        public DateTime? CreatedOn { get; set; }
        
        public string CreatedBy { get; set; }
        
        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class RuleGroupList
    {
        public string id { get; set; }
        public string name { get; set; }
        public string isactive { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }        
    }
}
