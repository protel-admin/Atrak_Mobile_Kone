using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class EmployeeLeaveAccount
    {
        //FOREIGN KEYS
        public string StaffId { get; set; }
        public string LeaveTypeId { get; set; }
        public int TransactionFlag { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Int64 Id { get; set; }

        [Required]
        public DateTime? TransactionDate { get; set; }
        [Required]
        public Decimal LeaveCount { get; set; }

        [Required]
        [MaxLength(100)]
        public string Narration { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }

        [ForeignKey("LeaveTypeId")]
        public virtual LeaveType LeaveType { get; set; }

        [ForeignKey("TransactionFlag")]
        public virtual LeaveTransactionType LeaveTransactionType { get; set; }

        [MaxLength(30)]
        public string RefId { get; set; }

        public DateTime? FinancialYearStart { get; set; }

        public DateTime? FinancialYearEnd { get; set; }

        public int LeaveCreditDebitReasonId { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }

        public bool IsManuallyExtended { get; set; }

        public decimal ExtensionPeriod { get; set; }

        [MaxLength(50)]
        public string TransctionBy { get; set; }

        public bool IsSystemAction { get; set; }

        public DateTime? WorkedDate { get; set; }

        [ForeignKey("LeaveCreditDebitReasonId")]
        public virtual LeaveCreditDebitReason LeaveCreditDebitReason { get; set; }

    }
}