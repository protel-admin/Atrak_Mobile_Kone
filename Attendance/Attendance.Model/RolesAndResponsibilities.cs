using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Attendance.Model
{
    public class RolesAndResponsibilities
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string StaffId { get; set; }

        public string Roles { get; set; }

        public string Responsibilities { get; set; }

        public DateTime? CreatedOn { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
        [MaxLength(50)]
        public string ModifiedBy { get; set; }
        [DefaultValue(false)]
        public bool IsActive { get; set; }
       
        public string Authorities { get; set; }
    }
}

