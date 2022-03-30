using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class EmployeeGroupTxn
    {
        //FOREIGN KEY
        public string EmployeeGroupId { get; set; }
        public string StaffId { get; set; }

        [Key]
        [MaxLength(10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public string Id { get; set; }

        public DateTime? LastUpdatedDate { get; set; }
        public string LastUpdatedShiftId { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("EmployeeGroupId")]
        public virtual EmployeeGroup EmployeeGroup { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
    }

    public class EmployeeGroupTxnView
    {
        public string Id { get; set; }
        public string EmployeeGroupId { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string Department { get; set; }
        public bool IsActive { get; set; }
    }
}