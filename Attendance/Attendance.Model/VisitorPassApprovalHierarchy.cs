using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Attendance.Model
{
    public class VisitorPassApprovalHierarchy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id {get;set;}
        
        [Required]
        [MaxLength(10)]
        public string GradeId {get;set;}
        
        [Required]
        public bool SendForApproval {get;set;}
        
        public DateTime? CreateOn {get;set;}
        
        [MaxLength(20)]
        public string CreatedBy {get;set;}
        
        public DateTime? ModifiedOn {get;set;}

        [MaxLength(20)]
        public string ModifiedBy { get; set; }
    }

    public class VisitorPassApprovalHierarchyList
    {
        public int Id { get; set; }
        public string GradeId { get; set; }
        public bool SendForApproval { get; set; }
        public DateTime? CreateOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
