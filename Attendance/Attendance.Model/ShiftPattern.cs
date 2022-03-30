using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class ShiftPattern
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public bool IsRotational { get; set; }

        [Required]
        public bool IsLifeTime { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime? EndDate { get; set; }

        public DateTime? UpdatedUntil { get; set; }

        [Required]
        public int DayPattern { get; set; }
        [Required]
        public DateTime? WOStartDate { get; set; }
        [Required]
        public int WODayOffSet { get; set; }

        public DateTime? WOLastUpdatedDate { get; set; }

        public bool IsActive { get; set; }

        public bool UsedAsGeneralShift { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

    }

    public class DutyRoosterView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsRotational { get; set; }
        public bool IsLifeTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class ShiftList
    {
        public string ShiftId { get; set; }
        public string ShiftShortName { get; set; }
        public string ShiftName { get; set; }
        public string ShiftIn { get; set; }
        public string ShiftOut { get; set; }
    }

    public class ShiftList1
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool isactive { get; set; }
    }

    public class ShiftPatternNewList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IsRotational { get; set; }
        public string IsLifeTime { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string UpdatedUntil { get; set; }
        public string DayPattern { get; set; }
        public string WOStartDate { get; set; }
        public string WODayOffSet { get; set; }
        public string WOLastUpdatedDate { get; set; }
        public string IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public virtual List<ShiftList> ShiftList { get; set; }
    }
}




