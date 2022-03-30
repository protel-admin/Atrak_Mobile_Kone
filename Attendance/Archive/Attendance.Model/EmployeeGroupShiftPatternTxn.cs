using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Attendance.Model
{
    public class EmployeeGroupShiftPatternTxn
    {
        //FOREIGN KEY
        public string EmployeeGroupId { get; set; }
        public int ShiftPatternId { get; set; }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Int64 Id { get; set; }

        public bool IsActive { get; set; }
        
        [ForeignKey("EmployeeGroupId")]
        public virtual EmployeeGroup EmployeeGroup { get; set; }

        [ForeignKey("ShiftPatternId")]
        public virtual ShiftPattern ShiftPattern { get; set; }

    
    }
}