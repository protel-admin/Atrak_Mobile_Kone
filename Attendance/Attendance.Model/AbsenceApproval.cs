using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model {
    public class AbsenceApproval {

        //FOREIGN KEYS
        public string AbsenceId { get; set; }
        public int ApprovalStatusId { get; set; }
        public string ApprovedById { get; set; }
        
        [Key]
        [DatabaseGenerated ( DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime ? ApprovedOn { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "Please enter your comments.")]
        public string Comment { get; set; }

        [ForeignKey ( "AbsenceId" )]
        public virtual LeaveApplication LeaveApplication { get; set; }

        [ForeignKey ( "ApprovalStatusId" )]
        public virtual ApprovalStatus ApprovalStatus { get; set; }

        [ForeignKey ( "ApprovedById" )]
        public virtual Staff ApprovalStaff { get; set; }

        public AbsenceApproval()
        {
            ApprovalStatusId = 1;
            ApprovedOn = DateTime.Now;
        }
    }
}
