using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Attendance.Model
{
    public class Shifts
    {
        [Key]
        [MaxLength(10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public string Id {get;set;}

        [Required]
        [MaxLength(50)]
        public string Name {get;set;}
        
        [Required]
        [MaxLength(5)]
        public string ShortName {get;set;}
        
        [Required]
        [MaxLength(50)]
        public string SAPShiftName { get;set;}

        [Required]
        public DateTime? StartTime {get;set;}

        [Required]
        public DateTime? EndTime {get;set;}

        [Required]
        public DateTime? GraceLateBy {get;set;}

        [Required]
        public DateTime? GraceEarlyBY {get;set;}

        [Required]
        public DateTime? BreakStartTime { get; set; }

        [Required]
        public DateTime? BreakEndTime { get; set; }

        [Required]
        public decimal MinDayHours {get;set;}

        [Required]
        public decimal  MinWeekHours {get;set;}

        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }
        public string LocationId { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }
    }

    public class ShiftView 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string SAPShiftName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string GraceLateBy { get; set; }
        public string GraceEarlyBY { get; set; }
        public string BreakStartTime { get; set; }
        public string BreakEndTime { get; set; }
        public string MinDayHours { get; set; }
        public string MinWeekHours { get; set; }
        public string IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string LocationId { get; set; }
    }
}