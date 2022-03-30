using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Model
{
    public class GroupAssociation
    {        

        [Key]
        [MaxLength(10)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        
        public string ParentId { get; set; }
        public string GroupType { get; set; }
        public string GroupId { get; set; }
        public string EmployeeId { get; set; }

        public bool IsActive { get; set; }

        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        

        [ForeignKey("EmployeeId")]
        public virtual Staff Staff { get; set; }


    }
}
