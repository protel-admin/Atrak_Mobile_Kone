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
        [MaxLength(50)]
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

        [MaxLength(10)]
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

        [DefaultValue(0)]
        public bool IsSpecialLate { get; set; }

        [Required]
        public bool IsProcessed { get; set; }
        
        [Required]
        public DateTime? CreatedOn { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public bool IsShiftPlanMissing { get; set; }



        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }

        public DateTime? ActualShiftIN { get; set; }
        public DateTime? ActualShiftOUT { get; set; }
        public DateTime? ActualWorkingHours { get; set; }
        public decimal   FHAccount { get; set; }
        public decimal   SHAccount { get; set; }
        public int       ProcessId { get; set; }
        public bool      IsToBeLeaveDeducted { get; set; }
        public bool      IsAutoLeaveDeducted { get; set; }

        public bool IsManualStatus { get; set; } = false;
        public string ApprovedExtraHours { get; set; }
        public string ConsiderExtraHoursFor { get; set; }
        public bool IsExtraHoursProcessed { get; set; } = false;
        public DateTime? ExtraHoursApprovedOn { get; set; }
        public string ExtraHoursApprovedBy { get; set; }
        public int IFlexiShiftTime { get; set; }
        [DefaultValue(0)]
        public bool IsFlexiShift { get; set; }
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
        public string Id { get; set; }
        public string STAFFID { get; set; }
        public string FirstName { get; set; }
        public string ShiftInDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string ActualOTTime { get; set; }
        public string ShiftShortName { get; set; }
        public string ShiftInTime { get; set; }
        public string ShiftOutTime { get; set; }
        public int NofoEmpCount { get; set; }
        
        public string TXNDATE { get; set; }
        public string ActualInTime { get; set; }
        public string ActualOutTime { get; set; }
        public string ActualExtraHoursWorked { get; set; }
    }
    #region Employee Master Summary Report
    public class EmployeeMasterSummaryReportModel
    {
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string EmergencyContactNo1 { get; set; }
        public string EmergencyContactNo2 { get; set; }
        public string AadharNo { get; set; }
        public string UANNo { get; set; }
        public string ESICNo { get; set; }
        public string ShiftType { get; set; }
        public string ShiftName { get; set; }
        public string StaffId   { get; set; }
        public string Name      { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public string BloodGroup { get; set; }
        public string MaritalStatus { get; set; }
        public string HomeAddress { get; set; }
        public string HomeLocation { get; set; }
        public string HomeCity { get; set; }
        public string HomeDistrict { get; set; }
        public string HomeState { get; set; }
        public string HomeCountry { get; set; }
        public string HomePostalCode { get; set; }
        public string HomePhone { get; set; }
        public string PersonalEmail { get; set; }
        public string DateOfBirth { get; set; }
        public string Qualificatin { get; set; }
        public string MarriageDate { get; set; }
        public string PanNo { get; set; }
        public string PassportNo { get; set; }
        public string DrivingLicense { get; set; }
        public string PersonalBank { get; set; }
        public string PersonalBankAccount { get; set; }
        public string PersonalBankIFSCCode { get; set; }
        public string PersonalBankBranch { get; set; }
        public string DateOfJoining { get; set; }
        public string DateOfResignation { get; set; }
        public string OfficialPhone { get; set; }
        public string OfficalEmail { get; set; }
        public string Company { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string Category { get; set; }
        public string CostCentre { get; set; }
        public string Location { get; set; }
        public string LeaveGroup { get; set; }
        public string HolidayGroup { get; set; }
        public string ReportingManager { get; set; }
        public string RecordType { get; set; }
    }
    public class LeaveRequisitionHistoryModel
    {

        public string StaffId           { get; set; }  
        public string Name              { get; set; }
        public string Department        { get; set; }
        public string Designation       { get; set; }
        public string LeaveType         { get; set; }
        public string StartDate         { get; set; }
        public string EndDate           { get; set; }
        public string StartDuration     { get; set; }
        public string EndDuration       { get; set; }
        public decimal TotalDays        { get; set; }
        public string ApprovalStatus    { get; set; }
        public int    ApprovalStatusId  { get; set; }
        public string ApplicationDate   { get; set; }
        public string ApprovedBy        { get; set; }
        public string ApprovedOn        { get; set; }
        public string Reason            { get; set; }

    }
    public class CoffAvailingModel
    {
       public string StaffId        { get; set; }
       public string Name           { get; set; }
       public string Department     { get; set; }
       public string Designation    { get; set; }
       public string WorkedDate     { get; set; }
       public string StartDate      { get; set; }
       public string StartDuration { get; set; }
       public string EndDate { get; set; }
       public string EndDuration    { get; set; }
       public string AppliedDate    { get; set; }
       public string ApprovalStatus { get; set; }
       public string ApprovedDate   { get; set; }
       public string ApprovalOwner  { get; set; }

    }
    public class CurrentDayInSwipModel
    {
       public string StaffId     { get; set; }
       public string Name        { get; set; }
       public string Branch      { get; set; }
       public string Department  { get; set; }
       public string Designation { get; set; }
	   public string ShiftName   { get; set; }
       public string InTime      { get; set; }
    }
    public class GetCoffCreditReportModel
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string WorkedDate { get; set; }
        public string Credit { get; set; }
        public string Reason { get; set; }
        public string ApplicationDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ReviewalStatus { get; set; }
        public string ReviewedOn { get; set; }
        public string ReviewedBy { get; set; }
    }
    #endregion
}
//}