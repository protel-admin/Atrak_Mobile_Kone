using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class EmployeePhoto
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Required]
        //public int Id{get;set;}

        [Key]
        [Required]
        public string StaffId{get;set;}

        [Required]
        public byte[] EmpPhoto{get;set;}

        public DateTime? CreatedOn{get;set;}

        public string CreatedBy{get;set;}
        
        public DateTime? ModifiedOn { get; set; }
        
        public string ModifiedBy { get; set; }
    }
}
