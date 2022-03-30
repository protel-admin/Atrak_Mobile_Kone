using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Attendance.Model;
using System.ComponentModel;

namespace Attendance.Model
{
    public class AttendanceData
    {
        //FOREIGN KEY
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Int64 Id { get; set; }
        public string StaffId { get; set; }
        public string ShiftId { get; set; }
        public string ShiftShortName { get; set; }
        public DateTime? ShiftInDate { get; set; }
        public DateTime? ShiftInTime { get; set; }
        public DateTime? ShiftOutDate { get; set; }
        public DateTime? ShiftOutTime { get; set; }
        public DateTime? ExpectedWorkingHours { get; set; }
        [MaxLength(10)]
        public string ActualShiftId { get; set; }

        [MaxLength(5)]
        public string ActualShiftShortName { get; set; }

        public DateTime? ActualInDate { get; set; }
        public DateTime? ActualInTime { get; set; }
        public DateTime? BreakOutTime { get; set; }
        public DateTime? BreakInTime { get; set; }
        public DateTime? ActualOutDate { get; set; }
        public DateTime? ActualOutTime { get; set; }
        public DateTime? ActualWorkedHours { get; set; }
        public DateTime? NetWorkedHours { get; set; }
        public DateTime? BreakHours { get; set; }
        public DateTime? ExtraBreakTime { get; set; }
        public bool IsBreakExceeded { get; set; }
        public bool IsBreakExceedValid { get; set; }
        public bool IsBreakDisputed { get; set; }

        //--------KEPT FOR BACKWARD COMPATIBILITY.
        public DateTime? OTHours { get; set; }
        public DateTime? EarlyComing { get; set; }
        public DateTime? LateComing { get; set; }
        public DateTime? EarlyGoing { get; set; }
        public DateTime? LateGoing { get; set; }
        //---------
        public DateTime ? ActualEarlyComingTime { get; set; }
        public DateTime ? ActualLateComingTime { get; set; }
        public DateTime ? ActualEarlyGoingTime  { get; set; }
        public DateTime ? ActualLateGoingTime  { get; set; }
        public DateTime ? ActualOTTime  { get; set; }
        public DateTime ? AccountedEarlyComingTime  { get; set; }
        public DateTime ? AccountedLateComingTime  { get; set; }
        public DateTime ? AccountedEarlyGoingTime  { get; set; }
        public DateTime ? AccountedLateGoingTime  { get; set; }
        public DateTime ? AccountedOTTime  { get; set; }
        

        [Required]
        public bool IsEarlyComing { get; set; }
        [Required]
        public bool IsEarlyComingValid { get; set; }


        [Required]
        public bool IsLateComing { get; set; }
        [Required]
        public bool IsLateComingValid { get; set; }
        
        
        [Required]
        public bool IsEarlyGoing { get; set; }
        [Required]
        public bool IsEarlyGoingValid { get; set; }

        [Required]
        public bool IsLateGoing { get; set; }
        [Required]
        public bool IsLateGoingValid { get; set; }


        [Required]
        public bool IsOT { get; set; }
        [Required]
        public bool IsOTValid { get; set; }

        [Required]
        public bool IsManualPunch { get; set; }
        [Required]
        public bool IsSinglePunch { get; set; }
        [Required]
        public bool IsIncorrectPunches { get; set; }
        [Required]
        public bool IsDisputed { get; set; }

        [Required]
        public bool OverRideEarlyComing { get; set; }
        [Required]
        public bool OverRideLateComing { get; set; }
        [Required]
        public bool OverRideEarlyGoing { get; set; }
        [Required]
        public bool OverRideLateGoing { get; set; }
        [Required]
        public bool OverRideOT { get; set; }

        
        [MaxLength(5)]
        [Required]
        public string AttendanceStatus { get; set; }
        [MaxLength(5)]
        [Required]
        public string FHStatus { get; set; }
        [MaxLength(5)]
        [Required]
        public string SHStatus { get; set; }
        [Required]
        public double AbsentCount { get; set; }
        [Required]
        public double DayAccount { get; set; }
        [Required]
        public bool IsLeave { get; set; }
        [Required]
        public bool IsLeaveValid { get; set; }
        [Required]
        public bool IsLeaveWithWages { get; set; }

        [Required]
        public bool IsAutoShift{ get; set; }
        [Required]
        public bool IsWeeklyOff{ get; set; }
        [Required]
        public bool IsPaidHoliday{ get; set; }
        
        [Required]
        public bool IsProcessed { get; set; }
        
        [Required]
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public bool IsShiftPlanMissing { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }

        public bool IsManualStatus { get; set; } = false;
        public bool IsAutoLeaveDeducted { get; set; } = false;
        public string ApprovedExtraHours { get; set; } 
        public string ConsiderExtraHoursFor { get; set; }
        public bool IsExtraHoursProcessed { get; set; } = false;
        public DateTime? ExtraHoursApprovedOn { get; set; }
        public string ExtraHoursApprovedBy { get; set; }

        [DefaultValue(0)]
        public bool IsSpecialLate { get; set; }
    }
    public class OtInformation
    {
        public string Id { get; set; }

        public string StaffId { get; set; }
        public DateTime OTDate { get; set; }
        public string OTTime { get; set; }
        public DateTime OTDuration { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string OTReason { get; set; }
        public bool IsCancelled { get; set; }
    }

    public class OTData
    {
        public string StaffId { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string OTDate { get; set; }
        public string OTTime { get; set; }
        public string OTDuration { get; set; }
        public string OTReason { get; set; }
        public string IsCancelled { get; set; }
        public string SolidLine { get; set; }
        public string EditedTime { get; set; }
        public string TotalOTHours { get; set; }
    }

    public class AttendanceDataView
    {
        //public string Id { get; set; }
        //public string StaffId { get; set; }
        //public string NAME { get; set; }
        //public string FirstName { get; set; }
        //public string ShiftInDate { get; set; }
        //public string TXNDATE { get; set; }
        //public string InTime { get; set; }
        //public string ActualInTime { get; set; }
        //public string OutTime { get; set; }
        //public string ActualOutTime { get; set; }
        //public string ActualOTTime { get; set; }
        //public string ShiftShortName { get; set; }
        //public string ShiftInTime { get; set; }
        //public string ShiftOutTime { get; set; }

public string STAFFID                 {get; set;}   
public string FirstName               {get; set;}
public string TXNDATE                 {get; set;}
public string ShiftShortName          {get; set;}
public string ShiftInTime             {get; set;}
public string ShiftOutTime            {get; set;}
public string ActualInTime            {get; set;}
public string ActualOutTime           {get; set;}
public string ActualExtraHoursWorked  {get; set;}
    }
}