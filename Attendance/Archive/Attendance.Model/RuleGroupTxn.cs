using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model {
    public class RuleGroupTxn {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int id { get; set; }

        public int rulegroupid { get; set; }
        public int ruleid { get; set; }

        [MaxLength(50)]
        [Required]
        public string value { get; set; }
        [MaxLength ( 50 )]
        [Required]
        public string defaultvalue { get; set; }
        [Required]
        public bool isactive { get; set; }

        public string LocationID { get; set; }

        [ForeignKey("LocationID")]
        public virtual Location Location { get; set; }

        [ForeignKey ( "rulegroupid" )]
        public virtual RuleGroup RuleGroup { get; set; }

        [ForeignKey ( "ruleid" )]
        public virtual Rule Rule { get; set; }

    }

    public class RuleGroupTxnsList
    {
        public string Id { get; set; }

        [Required]
        public string RuleID { get; set; }
        [Display(Name = "Role Name:")]
        [UIHint("Entry")]
        //public List<SelectListItem> RName { get; set; }  //selection
        public string Companyid { get; set; }

        public string RuleName { get; set; }

        public int RId { get; set; }
        public string RName { get; set; }
        public bool IsActive { get; set; }

    }
}
