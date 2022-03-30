using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Attendance.Model;

namespace Attendance.Model
{
    public class HolidayGroup
    {
        [Key]
        [MaxLength(10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public int LeaveYear { get; set; }

        [Required]
        public bool IsCurrent { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
        [MaxLength(50)]
        public string ModifiedBy { get; set; }
    }

    public class HolidayGroupList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int LeaveYear { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public List<HolidayList> HolidayGroupTxnList { get; set; }
        public List<HolidayGroupTxn> HolidayList { get; set; }
    }
}