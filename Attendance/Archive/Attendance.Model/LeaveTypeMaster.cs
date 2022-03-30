using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class LeaveTypeMaster
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(6)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string ShortName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LeaveType { get; set; }

        public bool PaidLeave { get; set; }

        public bool Accountable { get; set; }

        public bool CarryForward { get; set; }
        [Required]
        public decimal MaxAccDays { get; set; }
        [Required]
        public decimal MaxAccYears { get; set; }
        [Required]
        public decimal MaxDaysPerReq { get; set; }
        [Required]
        public int ElgInMonths { get; set; }
        [Required]
        public bool IsCalcToWorkingDays { get; set; }
        [Required]
        public decimal CalcToWorkingDays { get; set; }
        [Required]
        public bool ConsiderWO { get; set; }
        [Required]
        public bool ConsiderPH { get; set; }
        [Required]
        public bool IsExcessEligibleAllowed { get; set; }
        [Required]
        public bool IsEnCashmentAllowed { get; set; }
        [Required]
        public decimal EncashmentLimit { get; set; }

        [Required]
        public decimal CreditFreq { get; set; }
        [Required]
        public decimal CreditDays { get; set; }
        [Required]
        public bool ProRata { get; set; }

        public string RoundOffTo { get; set; }

        public int RoundOffValue { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class LeaveTypeMasterDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string LeaveType { get; set; }
        public string PaidLeave { get; set; }
        public string Accountable { get; set; }
        public string CarryForward { get; set; }
        public decimal MaxAccDays { get; set; }
        public decimal MaxAccYears { get; set; }
        public decimal MaxDaysPerReq { get; set; }
        public int ElgInMonths { get; set; }
        public string IsCalcToWorkingDays { get; set; }
        public decimal CalcToWorkingDays { get; set; }
        public string ConsiderWO { get; set; }
        public string ConsiderPH { get; set; }
        public string IsExcessEligibleAllowed { get; set; }
        public string IsEnCashmentAllowed { get; set; }
        public decimal EncashmentLimit { get; set; }
        public decimal CreditFreq { get; set; }
        public decimal CreditDays { get; set; }
        public string ProRata { get; set; }
        public string RoundOffTo { get; set; }
        public int RoundOffValue { get; set; }
        public string IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
