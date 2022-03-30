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
    public class EmployeeShiftPlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string StaffId { get; set; }

        [MaxLength(6)]
        [Required]
        public string ShiftId { get; set; }


        [Required]
        public int PatternId { get; set; }

        [Required]
        public int DayPatternId { get; set; }

        [MaxLength(10)]
        [Required]
        public string WeeklyOffId { get; set; }

        [Required]
        public bool IsGeneralShift { get; set; }

        [Required]
        [DefaultValue(0)]
        public bool IsFlexiShift { get; set; }

        [Required]
        [DefaultValue(0)]
        public bool IsAutoShift { get; set; } 

        [Required]
        [DefaultValue(0)]
        public bool IsManualShift { get; set; }

        [Required]
        public bool UseDayPattern { get; set; }

        [Required]
        public bool IsWeekPattern { get; set; }

        [Required]
        public bool UseWeeklyOff { get; set; }

        public int NoOfDaysShift { get; set; }

        public bool IsMonthlyPattern { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public DateTime StartDate { get; set; }

        [MaxLength(150)]
        [Required]
        public string Reason { get; set; }

        public int LastUpdatedShiftId { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        [MaxLength(20)]
        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }

    public class PermanantShiftChangeList
    {
        public Int32 Id { get; set; }

        public string StaffId { get; set; }

        public string StaffName { get; set; }

        public string Department { get; set; }

        public string ShiftName { get; set; }

        public string PatternName { get; set; }

        public string WorkingDayPattern { get; set; }

        public string WeeklyOff { get; set; }

        public string CreatedBy { get; set; }

        public bool IsGeneralShift { get; set; }

        public string WithEffectFrom { get; set; }

        public string Reason { get; set; }
    }
}
