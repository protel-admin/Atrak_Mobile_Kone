using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class AlternativePersonAssign
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string ParentId { get; set; }

        [Required]
        [MaxLength(20)]
        public string StaffId { get; set; }

        [Required]
        [MaxLength(20)]
        public string AlternativeStaffId { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsApproved { get; set; }

        public bool IsReviewed { get; set; }

        public bool IsRejected { get; set; }

        public bool IntimationMailSent { get; set; }

        public bool ConfirmationMailSent { get; set; }

        public bool CancellationMailSent { get; set; }

        public bool RejectMailSent { get; set; }
        
    }
}
