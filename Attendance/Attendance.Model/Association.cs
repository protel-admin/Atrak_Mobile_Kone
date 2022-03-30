using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class Association
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id{get;set;}
        
        [MaxLength(50)]
        [Required]
        public string Combination{get;set;}
        
        [MaxLength(10)]
        [Required]
        public string ParentId{get;set;}
        
        [MaxLength(5)]
        [Required]
        public string ParentType{get;set;}
        
        [Required]
        public int Priority{get;set;}
        
        [MaxLength(5)]
        [Required]
        public string WorkingDayPattern{get;set;}
        
        [MaxLength(5)]
        [Required]
        public string Gender{get;set;}
        
        [Required]
        public bool IsActive{get;set;}

        public DateTime? CreatedOn{get;set;}
        public string CreatedBy{get;set;}
        public DateTime? ModifiedOn{get;set;}
        public string ModifiedBy { get; set; }
    }

    public class AssociationList
    {
        public int Id { get; set; }
        public string Combination { get; set; }
        public string ParentId { get; set; }
        public string ParentType { get; set; }
        public int Priority { get; set; }
        public string WorkingDayPattern { get; set; }
        public string Gender { get; set; }
        public string IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }


}
