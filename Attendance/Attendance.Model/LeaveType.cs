using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class LeaveType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(10)]
        [Required]
        public string Id { get; set; }
        
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(5)]
        [Required]
        public string ShortName { get; set; }

        [Required]
        public bool IsAccountable { get; set; }
        [Required]
        public bool IsEncashable { get; set; }
        [Required]
        public bool IsPaidLeave { get; set; }
        [Required]
        public bool IsCommon { get; set; }
        [Required]
        public bool IsPermission { get; set; }
        [Required]
        public bool CarryForward { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

         [MaxLength(50)]
        public string ModifiedBy { get; set; }
    }


    public class LeaveView {

        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string IsAccountable { get; set; }
        public string IsEncashable { get; set; }
        public string IsPaidLeave { get; set; }
        public string IsCommon { get; set; }
        public string IsPermission { get; set; }
        public string CarryForward { get; set; }
        public string IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

}