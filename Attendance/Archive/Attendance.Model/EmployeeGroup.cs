using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Attendance.Model
{
    public class EmployeeGroup
    {
        [Key]
        [MaxLength(10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }

    public class EmployeeGroupView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int StaffCount { get; set; }
        public bool IsActive { get; set; }
        public List<EmployeeGroupTxnView> EmployeeGroupTxn { get; set; }
    }
}