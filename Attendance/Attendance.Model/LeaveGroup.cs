using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class LeaveGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(10)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }

    public class LeaveGroupList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IsActive { get; set; }
    }

    public class AccountableLeaveList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }

    public class LeaveGroupDetailsList
    {
        public string LeaveTypeId { get; set; }
        public string LeaveCount { get; set; }
    }

    public class LeaveTypeSave
    {
        public string LeaveId { get; set; }
        public string LeaveGroupName { get; set; }
        public string IsActive { get; set; }
        public virtual List<LeaveGroupDetailsList> LeaveGroupDetailsList { get; set; }
    }

}