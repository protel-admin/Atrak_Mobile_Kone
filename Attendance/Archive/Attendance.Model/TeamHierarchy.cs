using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class TeamHierarchy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string StaffId { get;set;}
        [Required]
        [MaxLength(20)]
        public string ReportingManagerId { get; set; }
        [Required] 
        public bool IsActive { get; set; }
        [ForeignKey("StaffId")]
        public virtual Staff StaffMain{get;set;}
        [ForeignKey("ReportingManagerId")]
        public virtual Staff Staff { get; set; }
    }
}
