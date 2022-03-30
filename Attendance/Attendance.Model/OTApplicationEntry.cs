using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
   public  class OTApplicationEntry
    {
    
           [Key]
           [Required]
           [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
           public int Id { get; set; }

           [Required(ErrorMessage = "*")]
           [MaxLength(50)]
           public string StaffId { get; set; }
           [Required(ErrorMessage = "*")]
           public string StaffName { get; set; }

           [Required(ErrorMessage = "*")]
           public DateTime? FromDate { get; set; }

           [Required(ErrorMessage = "*")]
           public DateTime? ToDate { get; set; }

           [Required]
           public DateTime? CreatedOn { get; set; }

           [Required]
           [MaxLength(50)]
           public string CreatedBy { get; set; }

           [ForeignKey("StaffId")]
           public virtual Staff Staff { get; set; }
    }
}
