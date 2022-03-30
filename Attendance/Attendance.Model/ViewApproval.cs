using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class ViewApproval
    {
        //FOREIGN KEY
        public string ApproverID { get; set; }
        public string LeaveApplicationID { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

	    [Required]
        public bool Viewable { get; set; }

        [ForeignKey("ApproverID")]
        public virtual Staff Staff { get; set; }
        
        [ForeignKey("LeaveApplicationID")]
        public virtual LeaveApplication LeaveApplication { get; set; }

    
    }
}