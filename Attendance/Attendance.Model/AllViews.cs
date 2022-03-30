using Attendance.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Data.EntityClient;
 
namespace Attendance.Model
{
    
    
    public class AllPendingApprovals
  {
    public int Id            { get; set; }
	public string ApplicationId { get; set; }
    public string ParentType    { get; set; }
    public string StaffId { get; set; }
    public string FirstName { get; set; }
    public string LeaveTypeName { get; set; }
    public string StartDurationName { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string EndDurationName { get; set; }
    public string TotalDays { get; set; }
    public string Reason { get; set; }
    public string FromTime { get; set; }
    public string TimeTo { get; set; }
    public string Name { get; set; }
    public string TotalHours { get; set; }
    public string PunchType { get; set; }
    public string InDate { get; set; }
    public string InTime { get; set; }
    public string OutDate { get; set; }
    public string OutTime { get; set; }
    public string WorkedDate { get; set; }
    public string COffAvailDate { get; set; }
    public string ApplicantName { get; set; }
    public string ODDuration { get; set; }
    public string ODFromTime { get; set; }
    public string ODFromDate { get; set; }
    public string ODToDate { get; set; }
    public string ODToTime { get; set; }
    public string OD { get; set; }
    public string ContactNumber { get; set; }
    public string ApprovalStatusName { get; set; }
    public string ReviewerStatusName { get; set; }
    public string ApprovalStaffId { get; set; }
    public string ReviewerStaffId { get; set; }
    public string ApprovalOwnerName { get; set; }
    public string ReviewerOwnerName { get; set; }
    public string ApprovedOnDate { get; set; }
    public string ReviewedOnDate { get; set; }
    public string Comment { get; set; }
    }
    
    public class AllApplicationHistory
    {
        
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string FromDuration { get; set; }
        public string ToDuration { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TotalHours { get; set; }
        public string TotalDays { get; set; }
        public string AppliedBy { get; set; }
        public string Type { get; set; }
        public string PunchType { get; set; }
        public string ODDuration { get; set; }
        public string ExpiryDate { get; set; }
        public string WorkedDate { get; set; }
        public string COffAvailDate { get; set; }
        public string Remarks { get; set; }
        public string ApprovalOwner { get; set; }
        public string ReviewerOwner { get; set; }
        public string   ApproverStatus { get; set; }
        public string  ReviewerStatus { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsReviewerCancelled { get; set; }
        public bool IsApproverCancelled { get; set; }
        public string RequestApplicationType { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
    public class DropDownModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class DropDownStrModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class ShiftPatternTemp
    {
        public int ShiftPatternID { get; set; }
        public bool IsRotational { get; set; }
        public bool IsLifeTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? UpdatedUntil { get; set; }
    }

    public class ShiftSettings
    {
        private int RecId;

        public ShiftSettings()
        {
            RecId = 0;
        }


        public int Id
        {
            get { return RecId; }
            set { RecId += 1; }
        }

        public string ShiftId { get; set; }
        public DateTime? ShiftStartTime { get; set; }
        public DateTime? ShiftEndDate { get; set; }
        public DateTime? ShiftEndTime { get; set; }
    }

    public class EmployeeImportResultMesss
    {
        public string Staffid { get; set; }
        public string MessageVal { get; set; }

    }
    public class EmployeeImportStaff
    {
        public string Empcode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PANNO { get; set; }
        public string BankACNo { get; set; }
        public string BankName { get; set; }
        public string BankIFSCCode { get; set; }
        public string PassPortNo { get; set; }
        public string AadharNo { get; set; }
        public string DOJ { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string Department { get; set; }
        public string Company { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string Branch { get; set; }
        public string Domainid { get; set; }
        public string PersonalMobile { get; set; }
        public string OfficialMobile { get; set; }
        public string Email { get; set; }
        public string ReportingManagerid { get; set; }
        public string CostCentre { get; set; }
        public string Division { get; set; }
        public string District { get; set; }
        public string MedicalClaimNumber { get; set; }
        public string Reviewer { get; set; }

        public string Approval { get; set; }
        public string ApproverLevel { get; set; }
        public string Shifttype { get; set; }
        public string Flexi { get; set; }
        public string Flexishift { get; set; }
        public string Auto { get; set; }
        public string General { get; set; }
        public string ShiftPattern { get; set; }
        public string WorkingpatternType { get; set; }
        public string Workingdaypattern { get; set; }
        public string weeklyoff { get; set; }
        public string Policygroup { get; set; }
        public string SecurityGroup { get; set; }

        public string IsAutoShift { get; set; }
        public string IsGeneralShift { get; set; }
        public string IsShiftPattern { get; set; }
        public string Isflexishift { get; set; }
        public string ShiftId { get; set; }
        public string ShiftPatternId { get; set; }
        public string IsWorkingDayPatternLocked { get; set; }
        public string IsWeeklyOffLocked { get; set; }

        public string OTReviewer { get; set; }
        public string OTReportingManager { get; set; }
        public string PFNo { get; set; }
        public string ESINo { get; set; }
        public string PresentAddress { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string EmergencyContactPerson { get; set; }

    }

    public class CompanyRule
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }

    public class DatesTemp
    {
        public DateTime? ShiftDate { get; set; }
        public int Id { get; set; }
    }

    public class StaffsTemp
    {
        public string StaffId { get; set; }
    }

    public class ShiftsFinal
    {
        public DateTime? ShiftStartDate { get; set; }
        public string ShiftId { get; set; }
        public string StaffId { get; set; }
        public DateTime? ShiftStartTime { get; set; }
        public DateTime? ShiftEndDate { get; set; }
        public DateTime? ShiftEndTime { get; set; }
    }

    public class CalendarDays
    {
        public string Id { get; set; }
        public string WeekNumber { get; set; }
        public string Day { get; set; }
        public string ShortName { get; set; }
        public string AttendanceStatus { get; set; }
        public string FHStatus { get; set; }
        public string SHStatus { get; set; }
        public string AttendanceStatusCode { get; set; }
        public string FHStatusCode { get; set; }
        public string SHStatusCode { get; set; }
        public string ActualInTime { get; set; }
        public string ActualOutTime { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string ActualWorkedHours { get; set; }
        public string LateComing { get; set; }
        public string EarlyGoing { get; set; }
        public string DayName { get; set; }
        public bool IsCurrentDay { get; set; }
        public string ActualDate { get; set; }
    }
    public class AttendanceDetails
    {
        public string Id { get; set; }
        public string Day { get; set; }
        public string WeekNumber { get; set; }
        public string ShortName { get; set; }
        public string AttendanceStatus { get; set; }
        public string FHStatus { get; set; }
        public string SHStatus { get; set; }
        public string ActualInTime { get; set; }
        public string ActualOutTime { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string ActualWorkedHours { get; set; }
        public string LateComing { get; set; }
        public string EarlyGoing { get; set; }
        public string DayName { get; set; }
        public string ActualDate { get; set; }
    }
    public class CalendarColorModel
    {
        public string ColorCode { get; set; }
        public string AttendanceStatus { get; set; }
    }
    public class StaffEditReqModel
    {
        public string Requestid { get; set; }
        public string Staffid { get; set; }
        public string Staff { get; set; }
        public string Staffofficial { get; set; }
        public string StaffPersonal { get; set; }
        public string AdditionalField { get; set; }
        public string createdon { get; set; }
        public string Status { get; set; }
        public string Approvedon { get; set; }
    }
    public class AdditionalFieldModel
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string Type { get; set; }
        public string Access { get; set; }
        public string Value { get; set; }
        public string Staffid { get; set; }

    }
    public class ShiftChangeList
    {
        public string FirstName { get; set; }
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string DepartmentName { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string NewShiftId { get; set; }
        public string NewShiftName { get; set; }
        public string Reason { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
    public class ShiftChangeDetailViewModel
    {
        public string Staffid { get; set; }
        public string staffname { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public List<Dates> Date { get; set; }

    }
    public class ShiftviewList
    {
        public string Shiftid { get; set; }
        public string Shiftshortname { get; set; }
        public string Shiftstarttime { get; set; }
        public string Shiftendtime { get; set; }
        public string OldShiftid { get; set; }
        public string OldStaffid { get; set; }
        public string OldShiftdate { get; set; }

    }
    public class Dates
    {
        public string Staffid { get; set; }
        public string staffname { get; set; }
        public string ShiftDate { get; set; }
        public string WeekDayName { get; set; }
        public string Shiftshortname { get; set; }
        public string Shiftid { get; set; }
        public string SDate { get; set; }
        public string SMonth { get; set; }
        public int WeekDayValue { get; set; }
        public bool Updated { get; set; }

    }
    public class OldShift
    {
        public string Id { get; set; }
        public string ShortName { get; set; }
        public string ShiftInDate { get; set; }
        public string ShiftInTime { get; set; }
        public string ShiftOutTime { get; set; }
    }

    public class ShiftDetailsForShiftChange
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        // public DateTime ExpectedWorkingHours { get; set; }
    }

    public class LeaveApplicationList
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string StartDate { get; set; }
        public string LeaveStartDurationId { get; set; }
        public string StartDuration { get; set; }
        public string EndDate { get; set; }
        public string LeaveEndDurationId { get; set; }
        public string EndDuration { get; set; }
        public string Remarks { get; set; }
        public string ReasonId { get; set; }
        public string Reason { get; set; }
        public string TotalDays { get; set; }
        public string ContactNumber { get; set; }
        public string LeaveTypeId { get; set; }
        public string LeaveName { get; set; }
        public string ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }
        public string Cancelled { get; set; }
        public string ApprovedBy { get; set; }
        public string DateApplied { get; set; }
        public string CanCancel { get; set; }
    }

    public class HolidayList
    {
        public string Hid { get; set; }
        public string LeaveTypeId { get; set; }
        public string Name { get; set; }
        public string HolidayDateFrom { get; set; }
        public string HolidayDateTo { get; set; }
        public string IsFixed { get; set; }
        public string LeaveYear { get; set; }
    }

    public class PrefixSuffixList
    {
        public string Id { get; set; }
        public string LeaveTypeId { get; set; }
        public string LeaveName { get; set; }
        public string PrefixLeaveTypeId { get; set; }
        public string PrefixLeaveName { get; set; }
        public string SuffixLeaveTypeId { get; set; }
        public string SuffixLeavename { get; set; }
        public string IsActive { get; set; }
    }

    public class DepartmentWiseHeadCount
    {
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string TotalHeadCount { get; set; }
        public string TotalPresent { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Category { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string LateBy { get; set; }
        public string TotalAbsent { get; set; }
        public string TotalLate { get; set; }
        public string PresentPercentage { get; set; }
        public string AbsentPercentage { get; set; }
        public string WorkStationId { get; set; }
    }

    public class ShiftWiseHeadCount
    {
        public string CompanyName { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string ShiftName { get; set; }
        public string TotalHeadCount { get; set; }
        public string TotalPresent { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Category { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string LateBy { get; set; }
        public string TotalAbsent { get; set; }
        public string TotalLate { get; set; }
        public string PresentPercentage { get; set; }
        public string AbsentPercentage { get; set; }
        public string WorkStationId { get; set; }
    }

    public class LeaveRequestList
    {
        public string LeaveApplicationId { get; set; }
        public string StaffId { get; set; }
        public string LeaveTypeId { get; set; }
        public string LeaveStartDurationId { get; set; }
        public string LeaveEndDurationId { get; set; }
        public string ApprovalStatusId { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ReviewerStaffId { get; set; }
        public string TotalDays { get; set; }
        public string ApplicationApprovalId { get; set; }
        public string FirstName { get; set; }
        public string LeaveTypeName { get; set; }
        public string LeaveStartDate { get; set; }
        public string LeaveStartDurationName { get; set; }
        public string LeaveEndDate { get; set; }
        public string LeaveEndDurationName { get; set; }
        public string LeaveApplicationReason { get; set; }
        public string ContactNumber { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ReviewerstatusName { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApprovedOnDate { get; set; }
        //public string ReviewedOnDate { get; set; }
        public string ApprovedOnTime { get; set; }
        public string Comment { get; set; }
        public string Remarks { get; set; }

        public string ApprovalOwnerName { get; set; }
        public string ReviewerOwnerName { get; set; }
        public string ReviewedOnDate { get; set; }
        //public string ReviewerstatusName { get; set; }
        public string ApprovalOwner { get; set; }
        public int IsDocumentAvailable { get; set; }
        public string ParentType { get; set; }
       
        //  public string TotalDays { get; set; }
    }

    public class DocumentData
    {
        public string LeaveApplicationId { get; set; }
        public byte[] FileContent { get; set; }
        public string TypeOfDocument { get; set; }
    }

    //public class ManualPunchRequest
    //{
    //    public string ManualPunchId { get; set; }
    //    public string StaffId { get; set; }
    //    public string PunchType { get; set; }
    //    public string InDate { get; set; }
    //    public string InTime { get; set; }
    //    public string OutDate { get; set; }
    //    public string OutTime { get; set; }
    //    public string ManualPunchReason { get; set; }
    //    public string FirstName { get; set; }
    //    public string ApprovalStatusId { get; set; }
    //    public string ApprovalStatusName { get; set; }
    //    public string ApprovalStaffId { get; set; }
    //    public string ApprovalStaffName { get; set; }
    //    public string ApplicationApprovalId { get; set; }
    //    public string ApprovedOnDate { get; set; }
    //    public string ApprovedOnTime { get; set; }
    //    public string Comment { get; set; }
    //    public string ApprovalOwner { get; set; }
    //    public string ParentType { get; set; }
    //}

    public class ShiftChangeRequest
    {
        public int ApproverLevel { get; set; }
        public string Approval2Owner { get; set; }
        public string ShiftChangeId { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string NewShiftId { get; set; }
        public string NewShiftName { get; set; }
        public string ShiftChangeReason { get; set; }
        public string ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApplicationApprovalId { get; set; }
        public string ApprovedOnDate { get; set; }
        public string ApprovedOnTime { get; set; }
        public string Comment { get; set; }
        public string ApprovalOwner { get; set; }
    }
    public class GetShiftChangeApplication
    {

        public string Id { get; set; }
        public string StaffId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string NewShiftId { get; set; }
    }

    //public class PermissionRequest
    //{
    //    public string PermissionId { get; set; }
    //    public string StaffId { get; set; }
    //    public string FirstName { get; set; }
    //    public string PermissionDate { get; set; }
    //    public string FromTime { get; set; }
    //    public string TimeTo { get; set; }
    //    public string PermissionOffReason { get; set; }
    //    public string ContactNumber { get; set; }
    //    public string ApprovalStatusId { get; set; }
    //    public string ApprovalStatusName { get; set; }
    //    public string ApprovalStaffId { get; set; }
    //    public string ApprovalStaffName { get; set; }
    //    public string ApplicationApprovalId { get; set; }
    //    public string ApprovedOnDate { get; set; }
    //    public string ApprovedOnTime { get; set; }
    //    public string Comment { get; set; }
    //    public string ApprovalOwner { get; set; }
    //    public string ParentType { get; set; }
    //}

    public class COffRequest
    {
        public string COffId { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string COffReqDate { get; set; }
        public string COffAvailDate { get; set; }
        public string TotalDays { get; set; }
        public string COffReason { get; set; }
        public string COffFromDate { get; set; }
        public string COffFromDateDuration { get; set; }
        public string COffToDate { get; set; }
        public string COffToDateDuration { get; set; }
        public string ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApplicationApprovalId { get; set; }
        public string ApprovedOnDate { get; set; }
        public string ApprovedOnTime { get; set; }
        public string Comment { get; set; }
        public string ApprovalOwner { get; set; }
        public string ParentType { get; set; }
    }
    public class COffAvailling
    {
        public string COffId { get; set; }
        // public string StaffId { get; set; }
        public string FirstName { get; set; }
        //public string COffReqDate { get; set; }
        public string COffReqStartDate { get; set; }
        public string COffReqEndDate { get; set; }
        public string COffAvailDate { get; set; }
        // public string TotalDays { get; set; }
        public string COffReason { get; set; }
        public string COffFromDate { get; set; }
        public string COffFromDateDuration { get; set; }
        public string COffToDate { get; set; }
        public string COffToDateDuration { get; set; }
        public string ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApplicationApprovalId { get; set; }
        public string ApprovedOnDate { get; set; }
        public string ApprovedOnTime { get; set; }
        public string Comment { get; set; }
        public string ApprovalOwner { get; set; }
        public string ParentType { get; set; }

        //public string    Id         { get; set; }
        public string StaffId { get; set; }
        public string WorkedDate { get; set; }
        public string StartDate { get; set; }
        public string COffReqDate { get; set; }
        public string TotalDays { get; set; }

    }

    public class COffCreditRequest
    {
        public string COffCreditId { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string CoffReqFrom { get; set; }
        public string CoffReqTo { get; set; }
        public string COffReason { get; set; }
        public string TotalDays { get; set; }
        public string ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApplicationApprovalId { get; set; }
        public string ApprovedOnDate { get; set; }
        public string ApprovedOnTime { get; set; }
        public string Comment { get; set; }
        public string ApprovalOwner { get; set; }
    }

    public class CoffReqDates
    {
        public string Staffid { get; set; }
        public string ShiftDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string ActualCoffTime { get; set; }
        public string CoffBalance { get; set; }
    }
    public class CoffBalance
    {
        public string CheckBox { get; set; }
        public string Staffid { get; set; }
        public string TransactionDate { get; set; }
        public decimal Balance { get; set; }
        public string UtilizedBalance { get; set; }

    }

    public class LaterOffRequest
    {
        public string COffId { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string LaterOffReqDate { get; set; }
        public string LaterOffAvailDate { get; set; }
        public string LaterOffReason { get; set; }
        public string ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApplicationApprovalId { get; set; }
        public string ApprovedOnDate { get; set; }
        public string ApprovedOnTime { get; set; }
        public string Comment { get; set; }
        public string ApprovalOwner { get; set; }
    }

    public class MaintenanceOffRequest
    {
        public string MaintenanceOffId { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string MaintenanceOffReason { get; set; }
        public string ContactNumber { get; set; }
        public string ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApplicationApprovalId { get; set; }
        public string ApprovedOnDate { get; set; }
        public string ApprovedOnTime { get; set; }
        public string Comment { get; set; }
        public string ApprovalOwner { get; set; }
    }

    public class OTRequest
    {
        public string OTApplicationId { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string OTDate { get; set; }
        public string OTTime { get; set; }
        public string OTDuration { get; set; }
        public string OTReason { get; set; }
        public string ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApplicationApprovalId { get; set; }
        public string ApprovedOnDate { get; set; }
        public string ApprovedOnTime { get; set; }
        public string Comment { get; set; }
        public string ApprovalOwner { get; set; }
    }

    public class ODRequest
    {
        public string ODApplicationId { get; set; }
        public string StaffId { get; set; }
        public string ApplicantName { get; set; }
        public string ODDuration { get; set; }
        public string ODFromDate { get; set; }
        public string ODFromTime { get; set; }
        public string ODToDate { get; set; }
        public string ODToTime { get; set; }
        public string OD { get; set; }
        public string ODReason { get; set; }
        public string ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApplicationApprovalId { get; set; }
        public string ApprovedOnDate { get; set; }
        public string ApprovedOnTime { get; set; }
        public string Comment { get; set; }
        public string ApprovalOwner { get; set; }
        public string ParentType { get; set; }
    }

    public class RawPunchDetails
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string DeptName { get; set; }
        public string Location { get; set; }
        public string Designation { get; set; }
        public string Division { get; set; }
        public string Volume { get; set; }
        public string GradeName { get; set; }
        public string SwipeDate { get; set; }
        public string SwipeTime { get; set; }
        public string InOut { get; set; }
        public string ReaderName { get; set; }
        public string AccessType { get; set; }
    }

    public class FirstInLastOutNew
    {
        public Int64 Id { get; set; }
        public string StaffName { get; set; }
        public string Plant { get; set; }
        public string Team { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ReportingHead { get; set; }
        public string Division { get; set; }
        public string Grade { get; set; }
        public string TxnDate { get; set; }
        public string Shift { get; set; }
        public string InTime { get; set; }
        public string InReader { get; set; }
        public string OutTime { get; set; }
        public string OutReader { get; set; }
        public string TotalHoursWorked { get; set; }
        public string AttendanceStatus { get; set; }
    }
    public class FirstInLastOut
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string DeptName { get; set; }
        public string Location { get; set; }
        public string Designation { get; set; }
        public string ReportingHead { get; set; }
        public string DivisionName { get; set; }
        public string GradeName { get; set; }
        public string SwipeDate { get; set; }
        public string Shift { get; set; }
        public string FirstInTime { get; set; }
        public string InReaderName { get; set; }
        public string LastOutTime { get; set; }
        public string OutReaderName { get; set; }
        public string TotalHoursWorked { get; set; }
        public string AttendanceStatus { get; set; }
    }

    public class FirstInLastOutDiamlerNew
    {
        public Int64 Id { get; set; }
        public string STAFFID { get; set; }
        public string STAFFNAME { get; set; }
        public string PLANT { get; set; }
        public string TEAM { get; set; }
        public string DEPARTMENT { get; set; }
        public string DESIGNATION { get; set; }
        public string DIVISON { get; set; }
        public string GRADE { get; set; }
        public string SHIFT { get; set; }
        public string TXNDATE { get; set; }
        public string INTIME { get; set; }
        public string INREADER { get; set; }
        public string OUTTIME { get; set; }
        public string OUTREADER { get; set; }
        public string LATEIN { get; set; }
        public string EARLYEXIT { get; set; }
        public string TOTALHOURSWORKED { get; set; }
        public string AttendanceStatus { get; set; }
    }

    public class PunchDetails
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string DeptName { get; set; }
        public string Designation { get; set; }
        public string TxnDate { get; set; }
        public string ActualInDate { get; set; }
        public string ActualInTime { get; set; }
        public string ActualOutDate { get; set; }
        public string ActualOutTime { get; set; }
        public string ActualWorkedHours { get; set; }
        public string OTHours { get; set; }
        public string AbsentStatus { get; set; }
    }
    public class BranchWiseDailyAttendance
    {
        public Int64 Id { get; set; }
        public Int32 Staffid { get; set; }
        public string ShiftName { get; set; }
        public Int32 SHIFTCOUNT { get; set; }
        public string Branch { get; set; }
        public decimal HalfDayCount { get; set; }
        public decimal LeaveCount { get; set; }
        public decimal ABSENTCOUNT { get; set; }
        public decimal ODCount { get; set; }
        public decimal PRESENTCOUNT { get; set; }
        public decimal HOLIDAYCOUNT { get; set; }
        public decimal WEEKLYOFFCOUNT { get; set; }
        public decimal COFFCOUNT { get; set; }
        public string EarlyArrival { get; set; }
        public string LateArrival { get; set; }
        public string EarlyGoing { get; set; }
        public string LateGoing { get; set; }
    }
    public class DepartmentWiseDailyAttendance
    {
        public Int64 Id { get; set; }
        public Int32 Staffid { get; set; }
        public string ShiftName { get; set; }
        public Int32 SHIFTCOUNT { get; set; }
        public string Department { get; set; }
        public decimal HalfDayCount { get; set; }
        public decimal LeaveCount { get; set; }
        public decimal ABSENTCOUNT { get; set; }
        public decimal ODCount { get; set; }
        public decimal PRESENTCOUNT { get; set; }
        public decimal HOLIDAYCOUNT { get; set; }
        public decimal WEEKLYOFFCOUNT { get; set; }
        public decimal COFFCOUNT { get; set; }
        public string EarlyArrival { get; set; }
        public string LateArrival { get; set; }
        public string EarlyGoing { get; set; }
        public string LateGoing { get; set; }
    }
    public class DailyExtraHoursWorkedDetails
    {
        public Int64 ID { get; set; }
        public string BRANCH { get; set; }
        public string staffid { get; set; }
        public string NAME { get; set; }
        public string COSTCENTER { get; set; }
        public string DEPARTMENT { get; set; }
        public String DESIGNATION { get; set; }
        public string Day1 { get; set; }
        public string Day2 { get; set; }
        public string Day3 { get; set; }
        public string Day4 { get; set; }
        public string Day5 { get; set; }
        public string Day6 { get; set; }
        public string Day7 { get; set; }
        public string Day8 { get; set; }
        public string Day9 { get; set; }
        public string Day10 { get; set; }
        public string Day11 { get; set; }
        public string Day12 { get; set; }
        public string Day13 { get; set; }
        public string Day14 { get; set; }
        public string Day15 { get; set; }
        public string Day16 { get; set; }
        public string Day17 { get; set; }
        public string Day18 { get; set; }
        public string Day19 { get; set; }
        public string Day20 { get; set; }
        public string Day21 { get; set; }
        public string Day22 { get; set; }
        public string Day23 { get; set; }
        public string Day24 { get; set; }
        public string Day25 { get; set; }
        public string Day26 { get; set; }
        public string Day27 { get; set; }
        public string Day28 { get; set; }
        public string Day29 { get; set; }
        public string Day30 { get; set; }
        public string Day31 { get; set; }
        public string NORMALHOURS { get; set; }
        public string HOLIDAYHOURS { get; set; }
        public string WOHOURS { get; set; }
        public string TOTALOTHOURS { get; set; }
        public string TotalHoursWorked { get; set; }
    }
    public class DailyAttendance
    {
        public Int64 Id { get; set; }
        public string Branch { get; set; }
        public string StaffId { get; set; }
        public string NAME { get; set; }
        public string CostCenter { get; set; }
        public string DEPARTMENT { get; set; }
        public string Designation { get; set; }
        public string ShiftName { get; set; }
        public Int32 SHIFTCOUNT { get; set; }
        public string ShiftInDate { get; set; }
        public string ActualInTime { get; set; }
        public string ActualOutTime { get; set; }
        public string TotalWorkedHours { get; set; }
        public decimal HalfDayCount { get; set; }
        public decimal LeaveCount { get; set; }
        public decimal AbsentCount { get; set; }
        public decimal ODCount { get; set; }
        public decimal PresentCount { get; set; }
        public decimal HolidayCount { get; set; }
        public decimal WeeklyoffCount { get; set; }
        public decimal COFFCOUNT { get; set; }
        public string EarlyArrival { get; set; }
        public string LateArrival { get; set; }
        public string EarlyGoing { get; set; }
        public string LateGoing { get; set; }
    }
    public class DailyPerformance
    {
        public string Designation { get; set; }
        public string StaffId { get; set; }
        public string CardCode { get; set; }
        public string FirstName { get; set; }
        public string ShiftName { get; set; }
        public string DeptName { get; set; }
        public string Location { get; set; }
        public string GradeName { get; set; }
        public string DivisionName { get; set; }
        public string VolumeName { get; set; }
        public string ReportingManager { get; set; }
        public string ShiftInDate { get; set; }
        public string ActualInDate { get; set; }
        public string ActualInTime { get; set; }
        public string ActualOutDate { get; set; }
        public string ActualOutTime { get; set; }
        public string ActualWorkedHours { get; set; }
        public string AttendanceStatus { get; set; }
        public string AccountedEarlyComingTime { get; set; }
        public string AccountedLateComingTime { get; set; }
        public string AccountedEarlyGoingTime { get; set; }
        public string AccountedLateGoingTime { get; set; }
        public string AccountedOTHours { get; set; }
    }

    public class PresentAndAbsentCountReport
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string Department { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int HalfDayCount { get; set; }
    }

    public class ShiftAllowanceReport
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public int SecondShift { get; set; }
        public int NightShift { get; set; }
    }

    public class AttendanceIncentive
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string DeptName { get; set; }
        public string DesignationName { get; set; }
        public int AbsentCount { get; set; }
        public int PresentCount { get; set; }
        public int HalfDayCount { get; set; }
    }

    public class PresentList //same class is used for absent list and attendance list.
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string ShiftId { get; set; }
        public string ShiftName { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string Location { get; set; }
        public string Designation { get; set; }
        public string ReportingHead { get; set; }
        public string GradeName { get; set; }
        public string ShiftInDate { get; set; }
        public string ActualInDate { get; set; }
        public string ActualInTime { get; set; }
        public string ActualOutDate { get; set; }
        public string ActualOutTime { get; set; }
        public string ActualWorkedHours { get; set; }
        public string AttendanceStatus { get; set; }
        public string DateOfJoining { get; set; }
        public string DateOfResignation { get; set; }
        public string TransactionDate { get; set; }
        public string Category { get; set; }
    }

    public class Form25
    {
        public string Day1Shift { get; set; }
        public string Day2Shift { get; set; }
        public string Day3Shift { get; set; }
        public string Day4Shift { get; set; }
        public string Day5Shift { get; set; }
        public string Day6Shift { get; set; }
        public string Day7Shift { get; set; }
        public string Day8Shift { get; set; }
        public string Day9Shift { get; set; }
        public string Day10Shift { get; set; }
        public string Day11Shift { get; set; }
        public string Day12Shift { get; set; }
        public string Day13Shift { get; set; }
        public string Day14Shift { get; set; }
        public string Day15Shift { get; set; }
        public string Day16Shift { get; set; }
        public string Day17Shift { get; set; }
        public string Day18Shift { get; set; }
        public string Day19Shift { get; set; }
        public string Day20Shift { get; set; }
        public string Day21Shift { get; set; }
        public string Day22Shift { get; set; }
        public string Day23Shift { get; set; }
        public string Day24Shift { get; set; }
        public string Day25Shift { get; set; }
        public string Day26Shift { get; set; }
        public string Day27Shift { get; set; }
        public string Day28Shift { get; set; }
        public string Day29Shift { get; set; }
        public string Day30Shift { get; set; }
        public string Day31Shift { get; set; }
        public Int64 Id { get; set; }
        public string staffid { get; set; }
        public string EmployeeDetails { get; set; }
        public int RelayNo { get; set; }
        public string PeriodOfEmployment { get; set; }
        public string PeriodOfWork { get; set; }
        public string Day1 { get; set; }
        public string Day2 { get; set; }
        public string Day3 { get; set; }
        public string Day4 { get; set; }
        public string Day5 { get; set; }
        public string Day6 { get; set; }
        public string Day7 { get; set; }
        public string Day8 { get; set; }
        public string Day9 { get; set; }
        public string Day10 { get; set; }
        public string Day11 { get; set; }
        public string Day12 { get; set; }
        public string Day13 { get; set; }
        public string Day14 { get; set; }
        public string Day15 { get; set; }
        public string Day16 { get; set; }
        public string Day17 { get; set; }
        public string Day18 { get; set; }
        public string Day19 { get; set; }
        public string Day20 { get; set; }
        public string Day21 { get; set; }
        public string Day22 { get; set; }
        public string Day23 { get; set; }
        public string Day24 { get; set; }
        public string Day25 { get; set; }
        public string Day26 { get; set; }
        public string Day27 { get; set; }
        public string Day28 { get; set; }
        public string Day29 { get; set; }
        public string Day30 { get; set; }
        public string Day31 { get; set; }
        public string Day1IN { get; set; }
        public string Day2IN { get; set; }
        public string Day3IN { get; set; }
        public string Day4IN { get; set; }
        public string Day5IN { get; set; }
        public string Day6IN { get; set; }
        public string Day7IN { get; set; }
        public string Day8IN { get; set; }
        public string Day9IN { get; set; }
        public string Day10IN { get; set; }
        public string Day11IN { get; set; }
        public string Day12IN { get; set; }
        public string Day13IN { get; set; }
        public string Day14IN { get; set; }
        public string Day15IN { get; set; }
        public string Day16IN { get; set; }
        public string Day17IN { get; set; }
        public string Day18IN { get; set; }
        public string Day19IN { get; set; }
        public string Day20IN { get; set; }
        public string Day21IN { get; set; }
        public string Day22IN { get; set; }
        public string Day23IN { get; set; }
        public string Day24IN { get; set; }
        public string Day25IN { get; set; }
        public string Day26IN { get; set; }
        public string Day27IN { get; set; }
        public string Day28IN { get; set; }
        public string Day29IN { get; set; }
        public string Day30IN { get; set; }
        public string Day31IN { get; set; }
        public string Day1OUT { get; set; }
        public string Day2OUT { get; set; }
        public string Day3OUT { get; set; }
        public string Day4OUT { get; set; }
        public string Day5OUT { get; set; }
        public string Day6OUT { get; set; }
        public string Day7OUT { get; set; }
        public string Day8OUT { get; set; }
        public string Day9OUT { get; set; }
        public string Day10OUT { get; set; }
        public string Day11OUT { get; set; }
        public string Day12OUT { get; set; }
        public string Day13OUT { get; set; }
        public string Day14OUT { get; set; }
        public string Day15OUT { get; set; }
        public string Day16OUT { get; set; }
        public string Day17OUT { get; set; }
        public string Day18OUT { get; set; }
        public string Day19OUT { get; set; }
        public string Day20OUT { get; set; }
        public string Day21OUT { get; set; }
        public string Day22OUT { get; set; }
        public string Day23OUT { get; set; }
        public string Day24OUT { get; set; }
        public string Day25OUT { get; set; }
        public string Day26OUT { get; set; }
        public string Day27OUT { get; set; }
        public string Day28OUT { get; set; }
        public string Day29OUT { get; set; }
        public string Day30OUT { get; set; }
        public string Day31OUT { get; set; }
        public string Day1TotalHours { get; set; }
        public string Day2TotalHours { get; set; }
        public string Day3TotalHours { get; set; }
        public string Day4TotalHours { get; set; }
        public string Day5TotalHours { get; set; }
        public string Day6TotalHours { get; set; }
        public string Day7TotalHours { get; set; }
        public string Day8TotalHours { get; set; }
        public string Day9TotalHours { get; set; }
        public string Day10TotalHours { get; set; }
        public string Day11TotalHours { get; set; }
        public string Day12TotalHours { get; set; }
        public string Day13TotalHours { get; set; }
        public string Day14TotalHours { get; set; }
        public string Day15TotalHours { get; set; }
        public string Day16TotalHours { get; set; }
        public string Day17TotalHours { get; set; }
        public string Day18TotalHours { get; set; }
        public string Day19TotalHours { get; set; }
        public string Day20TotalHours { get; set; }
        public string Day21TotalHours { get; set; }
        public string Day22TotalHours { get; set; }
        public string Day23TotalHours { get; set; }
        public string Day24TotalHours { get; set; }
        public string Day25TotalHours { get; set; }
        public string Day26TotalHours { get; set; }
        public string Day27TotalHours { get; set; }
        public string Day28TotalHours { get; set; }
        public string Day29TotalHours { get; set; }
        public string Day30TotalHours { get; set; }
        public string Day31TotalHours { get; set; }
        public string Day1LateIn { get; set; }
        public string Day2LateIn { get; set; }
        public string Day3LateIn { get; set; }
        public string Day4LateIn { get; set; }
        public string Day5LateIn { get; set; }
        public string Day6LateIn { get; set; }
        public string Day7LateIn { get; set; }
        public string Day8LateIn { get; set; }
        public string Day9LateIn { get; set; }
        public string Day10LateIn { get; set; }
        public string Day11LateIn { get; set; }
        public string Day12LateIn { get; set; }
        public string Day13LateIn { get; set; }
        public string Day14LateIn { get; set; }
        public string Day15LateIn { get; set; }
        public string Day16LateIn { get; set; }
        public string Day17LateIn { get; set; }
        public string Day18LateIn { get; set; }
        public string Day19LateIn { get; set; }
        public string Day20LateIn { get; set; }
        public string Day21LateIn { get; set; }
        public string Day22LateIn { get; set; }
        public string Day23LateIn { get; set; }
        public string Day24LateIn { get; set; }
        public string Day25LateIn { get; set; }
        public string Day26LateIn { get; set; }
        public string Day27LateIn { get; set; }
        public string Day28LateIn { get; set; }
        public string Day29LateIn { get; set; }
        public string Day30LateIn { get; set; }
        public string Day31LateIn { get; set; }
        public string Day1EarlyOut { get; set; }
        public string Day2EarlyOut { get; set; }
        public string Day3EarlyOut { get; set; }
        public string Day4EarlyOut { get; set; }
        public string Day5EarlyOut { get; set; }
        public string Day6EarlyOut { get; set; }
        public string Day7EarlyOut { get; set; }
        public string Day8EarlyOut { get; set; }
        public string Day9EarlyOut { get; set; }
        public string Day10EarlyOut { get; set; }
        public string Day11EarlyOut { get; set; }
        public string Day12EarlyOut { get; set; }
        public string Day13EarlyOut { get; set; }
        public string Day14EarlyOut { get; set; }
        public string Day15EarlyOut { get; set; }
        public string Day16EarlyOut { get; set; }
        public string Day17EarlyOut { get; set; }
        public string Day18EarlyOut { get; set; }
        public string Day19EarlyOut { get; set; }
        public string Day20EarlyOut { get; set; }
        public string Day21EarlyOut { get; set; }
        public string Day22EarlyOut { get; set; }
        public string Day23EarlyOut { get; set; }
        public string Day24EarlyOut { get; set; }
        public string Day25EarlyOut { get; set; }
        public string Day26EarlyOut { get; set; }
        public string Day27EarlyOut { get; set; }
        public string Day28EarlyOut { get; set; }
        public string Day29EarlyOut { get; set; }
        public string Day30EarlyOut { get; set; }
        public string Day31EarlyOut { get; set; }
        public string Day1OT { get; set; }
        public string Day2OT { get; set; }
        public string Day3OT { get; set; }
        public string Day4OT { get; set; }
        public string Day5OT { get; set; }
        public string Day6OT { get; set; }
        public string Day7OT { get; set; }
        public string Day8OT { get; set; }
        public string Day9OT { get; set; }
        public string Day10OT { get; set; }
        public string Day11OT { get; set; }
        public string Day12OT { get; set; }
        public string Day13OT { get; set; }
        public string Day14OT { get; set; }
        public string Day15OT { get; set; }
        public string Day16OT { get; set; }
        public string Day17OT { get; set; }
        public string Day18OT { get; set; }
        public string Day19OT { get; set; }
        public string Day20OT { get; set; }
        public string Day21OT { get; set; }
        public string Day22OT { get; set; }
        public string Day23OT { get; set; }
        public string Day24OT { get; set; }
        public string Day25OT { get; set; }
        public string Day26OT { get; set; }
        public string Day27OT { get; set; }
        public string Day28OT { get; set; }
        public string Day29OT { get; set; }
        public string Day30OT { get; set; }
        public string Day31OT { get; set; }

        public string ExemptingOrder { get; set; }
        public string WeeklyRest { get; set; }
        public string CompensatoryHolidayDate { get; set; }
        public string LostRestDays { get; set; }
        public string NoOfDaysWorked { get; set; }
        public string LeaveWithWages { get; set; }
        public string LeaveWithOutWages { get; set; }
        public string Remarks { get; set; }
        public string NAME { get; set; }
        public string PLANT { get; set; }
        public string DEPARTMENT { get; set; }
        public string NOOFDAYSWORKING { get; set; }
        //public decimal NOOFDAYSWORKED { get; set; }
        public string NOOFDAYSNOTWORKED { get; set; }
        public string NOOFDAYSABSENT { get; set; }
        public string NOOFDAYSLEAVE { get; set; }
        public string NSA1 { get; set; }
        public string NSA2 { get; set; }
        public string ATTINCENTIVE { get; set; }
        public string LATEPENALTY { get; set; }
        public string DOJ { get; set; }
        public string DOR { get; set; }
        public string Division { get; set; }
        public string Volume { get; set; }
        public string NOOFDAYSWO { get; set; }
        public string TOTALOTHOURS { get; set; }
    }

    public class RoleList
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ManualPunchApprovalList
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string Category { get; set; }
        public string CostCentre { get; set; }
        public string PunchType { get; set; }
        public string InDateTime { get; set; }
        public string OutDateTime { get; set; }
        public string Reason { get; set; }
        public string ApprovalStatus { get; set; }
        public string ReviewalStatus { get; set; }
        public string ApplicationDate { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string ReviewedBy { get; set; }
        public string ReviewedOn { get; set; }
    }
    public class ShiftChangeApproval
    {
        public string ShiftChangeId { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string NewShiftId { get; set; }
        public string NewShiftName { get; set; }
        public string ShiftChangeReason { get; set; }
        public string ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApplicationApprovalId { get; set; }
        public string ApprovedOnDate { get; set; }
        public string ApprovedOnTime { get; set; }
        public string Comment { get; set; }
        public string ApprovalOwner { get; set; }
    }
    public class CustomShiftChangeApplication
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string LoggedInUserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string NewShiftId { get; set; }
        public string Reason { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
    public class EarlyDeparture
    {
        public string STAFFID { get; set; }
        public string NAME { get; set; }
        public string SHIFTSHORTNAME { get; set; }
        public string TXNDATE { get; set; }
        public string SCHINTIME { get; set; }
        public string SCHOUTTIME { get; set; }
        public string INTIME { get; set; }
        public string OUTTIME { get; set; }
        public string EARLYGOING { get; set; }
        public string COL1 { get; set; }
        public string COL2 { get; set; }
        public string COL3 { get; set; }
        public string COL4 { get; set; }
        public string COL5 { get; set; }
        public string COL6 { get; set; }
        public string COL7 { get; set; }
        public string COL8 { get; set; }
        public string COL9 { get; set; }

    }
    public class EarlyArraival
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string CompanyName { get; set; }
        public string DeptName { get; set; }
        public string GradeName { get; set; }
        public string ShiftId { get; set; }
        public string Name { get; set; }
        public string ShiftInDate { get; set; }
        public string ShiftInTime { get; set; }
        public string ShiftOutDate { get; set; }
        public string ShiftOutTime { get; set; }
        public string ActualInDate { get; set; }
        public string ActualInTime { get; set; }
        public string ActualOutDate { get; set; }
        public string ActualOutTime { get; set; }
        public string ActualEarlyComingTime { get; set; }
        public string AccountedEarlyComingTime { get; set; }
    }

    public class PlannedLeave
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string ShiftName { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string GradeName { get; set; }
        public string LeaveTypeName { get; set; }
        public string LeaveStartDate { get; set; }
        public string LeaveStartDurationName { get; set; }
        public string LeaveEndDate { get; set; }
        public string LeaveEndDurationName { get; set; }
        public string ApprovalStatusName { get; set; }
        public string LeaveApplicationReason { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApprovedOnDate { get; set; }
    }

    public class UnPlannedLeave
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string ShiftName { get; set; }
        public string DepartmentName { get; set; }
        public string GradeName { get; set; }
        public string ShiftInDate { get; set; }
        public string ShiftInTime { get; set; }
        public string ShiftOutDate { get; set; }
        public string ShiftOutTime { get; set; }
        public string AttendanceStatus { get; set; }
        public string LeaveStatus { get; set; }
        public string CompanyName { get; set; }
        public string LeaveTypeName { get; set; }
        public string LeaveStartDate { get; set; }
        public string LeaveStartDurationName { get; set; }
        public string LeaveEndDate { get; set; }
        public string LeaveEndDurationName { get; set; }
        public string ApprovalStatusName { get; set; }
        public string LeaveApplicationReason { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
    }

    public class DepartmentSummary
    {
        public string DEPTID { get; set; }
        public string DEPTNAME { get; set; }
        public string TXNDATE { get; set; }
        public int HEADCOUNT { get; set; }
        public int TOTALPRESENT { get; set; }
        public int TOTALABSENT { get; set; }
        public decimal PRESENTPAGE { get; set; }
        public decimal ABSENTPAGE { get; set; }
    }

    public class Permissiomoffrpt
    {
        public string STAFFID { get; set; }
        public string STAFFNAME { get; set; }
        public string DEPARTMENTNAME { get; set; }
        public string TimeFrom { get; set; }
        public string TIMETO { get; set; }
        public string TotalHours { get; set; }
        public string PermissionDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovedBy { get; set; }
        public string Approvedon { get; set; }

    }

    public class LateComers
    {
        public string StaffID { get; set; }
        public string FirstName { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string Location { get; set; }
        public string Designation { get; set; }
        public string ReportingHead { get; set; }
        public string DivisionName { get; set; }
        public string ShiftName { get; set; }
        public string ShiftInDate { get; set; }
        public string ShiftInTime { get; set; }
        public string ShiftOutTime { get; set; }
        public string ActualInTime { get; set; }
        public string ActualWorkedHours { get; set; }
        public string LateComing { get; set; }
        public string AccountedLateComingTime { get; set; }
        public string IsValid { get; set; }
        public string TotalHoursPermission { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffName { get; set; }
    }

    public class OvertimeStatement
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string CompanyName { get; set; }
        public string DeptName { get; set; }
        public string GradeName { get; set; }
        public string ShiftId { get; set; }
        public string Name { get; set; }
        public string ShiftInDate { get; set; }
        public string ShiftInTime { get; set; }
        public string ShiftOutDate { get; set; }
        public string ShiftOutTime { get; set; }
        public string ActualInDate { get; set; }
        public string ActualInTime { get; set; }
        public string ActualOutDate { get; set; }
        public string ActualOutTime { get; set; }
        public string ActualOTTime { get; set; }
        public string AccountedOTTime { get; set; }
    }

    public class ExtraHoursWorked
    {
        public string STAFFID { get; set; }
        public string NAME { get; set; }
        public string SHIFTSHORTNAME { get; set; }
        public string TXNDATE { get; set; }
        public string SCHINTIME { get; set; }
        public string SCHOUTTIME { get; set; }
        public string INTIME { get; set; }
        public string OUTTIME { get; set; }
        public string EARLYCOMING { get; set; }
        public string LATEGOING { get; set; }
        public string OTHOURS { get; set; }
        public string COL1 { get; set; }
        public string COL2 { get; set; }
        public string COL3 { get; set; }
        public string COL4 { get; set; }
        public string COL5 { get; set; }
        public string COL6 { get; set; }
        public string COL7 { get; set; }
        public string COL8 { get; set; }
        public string COL9 { get; set; }
    }
    public class HWOWokingReport
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string SubFunction { get; set; }
        public string WorkedDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string TotalHoursWorked { get; set; }
        public string Remarks { get; set; }
    }
    public class LeaveSummaryReport
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Department { get; set; }
        public string SubFunction { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string Category { get; set; }
        public string LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public decimal BroughtForward { get; set; }
        public decimal Entitlement { get; set; }
        public decimal Adjustment { get; set; }
        public decimal Fortified { get; set; }
        public decimal ApprovedLeave { get; set; }
        public decimal PendingApproval { get; set; }
        public decimal Encashment { get; set; }
        public decimal Balance { get; set; }
    }
    public class EmployeeAdditionDeletionReport
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string DateOfJoining { get; set; }
        public string ExpectedJobEndDate { get; set; }
        public string DateOfResignation { get; set; }
        public string HRWT { get; set; }
        public string PayrollId { get; set; }
        public string LocallyAssignedId { get; set; }
        public string PayGroup { get; set; }
        public string OfficialPhone { get; set; }
        public string OfficalEmail { get; set; }
        public string Company { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Designation { get; set; }
        public string SubFunction { get; set; }
        public string Grade { get; set; }
        public string Category { get; set; }
        public string CostCentre { get; set; }
        public string Location { get; set; }
        public string Business { get; set; }
        public string ClassMaster { get; set; }
        public string Entity { get; set; }
        public string EmploymentType { get; set; }
        public string Level { get; set; }
        public string Zone { get; set; }
        public string LeaveGroup { get; set; }
        public string HolidayGroup { get; set; }
        public string ReportingManager { get; set; }
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
        public string MarriageDate { get; set; }
        public string PanNo { get; set; }
        public string PassportNo { get; set; }
        public string DrivingLicense { get; set; }
        public string PersonalBank { get; set; }
        public string PersonalBankAccount { get; set; }
        public string PersonalBankIFSCCode { get; set; }
        public string PersonalBankBranch { get; set; }
        public string RecordType { get; set; }
        public string Volume { get; set; }
    }
    public class Form25FLSmidth
    {
        public Int64 RecId { get; set; }
        public Int64 Id { get; set; }
        public string StaffId { get; set; }
        public string NameOfWorker { get; set; }
        public string FatherName { get; set; }
        public string Designation { get; set; }
        public Int32 BirthDay { get; set; }
        public int BirthMonth { get; set; }
        public Int32 BirthYear { get; set; }
        public string PlaceOfEmployment { get; set; }
        public Int32 GroupNo { get; set; }
        public Int32 RelayNo { get; set; }
        public string PeriodOfEmployment { get; set; }
        public string PeriodOfWork { get; set; }
        public string Day1 { get; set; }
        public string Day2 { get; set; }
        public string Day3 { get; set; }
        public string Day4 { get; set; }
        public string Day5 { get; set; }
        public string Day6 { get; set; }
        public string Day7 { get; set; }
        public string Day8 { get; set; }
        public string Day9 { get; set; }
        public string Day10 { get; set; }
        public string Day11 { get; set; }
        public string Day12 { get; set; }
        public string Day13 { get; set; }
        public string Day14 { get; set; }
        public string Day15 { get; set; }
        public string Day16 { get; set; }
        public string Day17 { get; set; }
        public string Day18 { get; set; }
        public string Day19 { get; set; }
        public string Day20 { get; set; }
        public string Day21 { get; set; }
        public string Day22 { get; set; }
        public string Day23 { get; set; }
        public string Day24 { get; set; }
        public string Day25 { get; set; }
        public string Day26 { get; set; }
        public string Day27 { get; set; }
        public string Day28 { get; set; }
        public string Day29 { get; set; }
        public string Day30 { get; set; }
        public string Day31 { get; set; }
        //public string ExemptingOrder { get; set; }
        //public string WeeklyRest { get; set; }
        //public string CompensatoryHolidayDate { get; set; }
        //public string LostRestDays { get; set; }
        //public decimal NoOfDaysWorked { get; set; }
        //public decimal LeaveWithWages { get; set; }
        //public decimal LeaveWithOutWages { get; set; }
        //public string Remarks { get; set; }

    }

    public class HolidayZoneWiseHolidayList
    {
        public string HolidayId { get; set; }
        public string LeaveYear { get; set; }
        public string LeaveTypeId { get; set; }
        public string HolidayName { get; set; }
        public string HolidayDateFrom { get; set; }
        public string HolidayDateTo { get; set; }
        public string IsChecked { get; set; }
    }

    public class ReportingList
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string ReportingManagerName { get; set; }
        public string ReviewerName { get; set; }
        public int ApproverLevel { get; set; }
    }

    public class AssociateEmployeeGroupShiftPlan
    {
        public string EmployeeGroupId { get; set; }
        public string EmployeeGroupName { get; set; }
        public string Id { get; set; }
        public string ShiftPlanId { get; set; }
        public string ShiftPlan { get; set; }
        public string IsActive { get; set; }
    }

    public class EmployeeList
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string DeptName { get; set; }
        public string DesignationName { get; set; }
        public string GradeName { get; set; }
        public string StatusName { get; set; }
        public string WorkingPattern { get; set; }
    }

    public class TempData
    {
        public string ParentId { get; set; }
        public string ParentType { get; set; }
        public string ApprovalStatus { get; set; }
    }

    public class LoggedInUserDetails
    {
        public string UserName { get; set; }
        public string Approver2Id { get; set; }
        public string Gender { get; set; }
        public string DomainId { get; set; }
        public string StaffId { get; set; }
        public string StaffFullName { get; set; }
        public int SecurityGroupId { get; set; }
        public string ReportingManagerId { get; set; }
        public string ReportingManagerName { get; set; }
        public string CompanyId { get; set; }
        public string BranchId { get; set; }
        public string LocationId { get; set; }
        public string DepartmentId { get; set; }
        public string CategoryId { get; set; }
        public string GradeName { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string DeptName { get; set; }
        public int ApprovalLevel { get; set; }
        public string OfficialPhone { get; set; }
        public string UserEmailId { get; set; }
        public string ReportingManagerEmailId { get; set; }
    }

    public class ContinuousAbsent
    {
        //public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string DivisionName { get; set; }
        public int NoOfDays { get; set; }

        public string STAFFID { get; set; }
        public string NAME { get; set; }
        public string DEPARTMENT { get; set; }
        public string Designation { get; set; }
        public string FROMDATE { get; set; }
        public string TODATE { get; set; }
        public Int32 TOTALDAYS { get; set; }
    }

    public class GraceTime
    {
        public string STAFFID { get; set; }
        public string NAME { get; set; }
        public string SHIFTSHORTNAME { get; set; }
        public string TXNDATE { get; set; }
        public string SCHINTIME { get; set; }
        public string SCHOUTTIME { get; set; }
        public string INTIME { get; set; }
        public string OUTTIME { get; set; }
        public string LATECOMING { get; set; }
        public string EARLYGOING { get; set; }
    }

    public class ContinuousLateComing
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string Designation { get; set; }
        public string DivisionName { get; set; }
        public int NoOfDays { get; set; }
        public string TotalHours { get; set; }
    }

    public class ContinuousEarlyGoing
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string DivisionName { get; set; }
        public int NoOfDays { get; set; }
        public string TotalHours { get; set; }
    }

    public class MissedPunchList
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string Designation { get; set; }
        public string DivisionName { get; set; }
        public string TxnDate { get; set; }
        public string MissedPunch { get; set; }
    }

    public class ShiftChangeStatement
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string DivisionName { get; set; }
        public string TxnDate { get; set; }
        public string NewShiftName { get; set; }
        public string ShiftChangeReason { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApprovedOnDate { get; set; }
    }

    public class OutDoorStatement
    {
        public string STAFFID { get; set; }
        public string STAFFNAME { get; set; }
        public string TXNDATE { get; set; }
        public string FROMTIME { get; set; }
        public string TOTIME { get; set; }
        public string ODREASON { get; set; }
        public string APPROVALSTATUSNAME { get; set; }
        public string APPROVEDBY { get; set; }
        public string APPROVEDON { get; set; }
        public string COMMENT { get; set; }
        public string APPROVALOWNERNAME { get; set; }
        public string COL1 { get; set; }
        public string COL2 { get; set; }
        public string COL3 { get; set; }
        public string COL4 { get; set; }
        public string COL5 { get; set; }
        public string COL6 { get; set; }
        public string COL7 { get; set; }
        public string COL8 { get; set; }
        public string COL9 { get; set; }
    }

    public class PresentOnNFH
    {
        public string STAFFID { get; set; }
        public string STAFFNAME { get; set; }
        public string TXNDATE { get; set; }
        public string PRESENT { get; set; }
        public string WEEKLYOFF { get; set; }
        public string PAIDHOLIDAY { get; set; }
        public string COL1 { get; set; }
        public string COL2 { get; set; }
        public string COL3 { get; set; }
        public string COL4 { get; set; }
        public string COL5 { get; set; }
        public string COL6 { get; set; }
        public string COL7 { get; set; }
        public string COL8 { get; set; }
        public string COL9 { get; set; }
    }

    public class LeaveApplicationReport
    {
        public string STAFFID { get; set; }
        public string STAFFNAME { get; set; }
        public string TXNDATE { get; set; }
        public string LEAVETYPENAME { get; set; }
        public string LEAVEAPPLICATIONREASON { get; set; }
        public string APPROVALSTATUSNAME { get; set; }
        public string APPROVALSTAFFID { get; set; }
        public string APPROVALSTAFFNAME { get; set; }
        public string APPROVEDONDATE { get; set; }
        public string APPROVEDONTIME { get; set; }
        public string APPROVALOWNWERID { get; set; }
        public string APPROVALOWNERNAME { get; set; }
    }

    public class MOffApplicationReport
    {
        public string STAFFID { get; set; }
        public string STAFFNAME { get; set; }
        public string TXNDATE { get; set; }
        public string LEAVESHORTNAME { get; set; }
        public string MAINTENANCEOFFREASON { get; set; }
        public string APPROVALSTATUSNAME { get; set; }
        public string APPROVALSTAFFID { get; set; }
        public string APPROVALSTAFFNAME { get; set; }
        public string APPROVEDONDATE { get; set; }
        public string APPROVEDONTIME { get; set; }
        public string APPROVALOWNWERID { get; set; }
        public string APPROVALOWNERNAME { get; set; }
    }

    public class FilterList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class TodaysPunchDashBoard
    {
        public string StaffID { get; set; }
        public string InDate { get; set; }
        public string InTime { get; set; }
        public string OutDate { get; set; }
        public string OutTime { get; set; }
        public string InReaderName { get; set; }
        public string OutReaderName { get; set; }
        public string AttendanceStatus { get; set; }
    }
    
    //public class TodaysPunchDashBoard    renamed to TeamAttendance
    public class TeamAttendance
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string AttendanceDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string ShiftName { get; set; }
        public string ShiftShortName { get; set; }
        public string TotalHoursWorked { get; set; }
        public string AttendanceStatus { get; set; }
        public string OTTime { get; set; }
    }

    public class TodaysPunchesDashBoardForMobile
    {
        public string StaffId { get; set; }
        public string ShiftIn { get; set; }
        public string ShiftOut { get; set; }
        public string SwipeIn { get; set; }
        public string SwipeOut { get; set; }
        public string InReaderName { get; set; }
        public string OutReaderName { get; set; }
        public string LateIn { get; set; }
        public string EarlyOut { get; set; }
        public string SlideMode { get; set; }
    }

    public class MOApplicableYear
    {
        public string MOYear { get; set; }
    }

    public class EmpData
    {
        public string StaffName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string GradeName { get; set; }
    }

    public class RHHistory
    {
        public string RHApplicationId { get; set; }
        public string HolidayName { get; set; }
        public string RHDate { get; set; }
        public string RHYear { get; set; }
        public string RHID { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string LeaveId { get; set; }
        public string ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }
    }

    public class LeaveApplicationDeatails
    {
        public string STAFFID { get; set; }
        public string STAFFNAME { get; set; }
        public string PLANT { get; set; }
        public string TEAM { get; set; }
        public string DEPARTMENTNAME { get; set; }
        public string DESIGNATION { get; set; }
        public string STARTDATE { get; set; }
        public string STARTDURATION { get; set; }
        public string ENDDATE { get; set; }
        public string ENDDURATION { get; set; }
        public string CANCELLED { get; set; }
        public string REMARKS { get; set; }
        public string REASON { get; set; }
        public string TOTALDAYS { get; set; }
        public string APPROVALSTATUSNAME { get; set; }
        public string APPROVEDBY { get; set; }
        public string APPLTYPE { get; set; }
        public string APPLICATIONDATE { get; set; }
        public string LEAVEENDDURATION { get; set; }
    }
    public class CanteenReport
    {
        public string Empcode { get; set; }
        public string Name { get; set; }
        public string Plant { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string Branch { get; set; }

        public string Division { get; set; }
        public string Indate { get; set; }
        public string InTime { get; set; }

    }

    public class IndividualLeaveCreditDebit
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string LeaveReson { get; set; }
        public string ActionUser { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string ReceiverName { get; set; }
        public string Department { get; set; }
        public string CasualLeaveBalance { get; set; }
        public string EarnedLeaveBalance { get; set; }
        public string LOHPLeaveBalance { get; set; }
        public string LTALeaveBalance { get; set; }
        public string SICKLEAVEBALANCE { get; set; }
        public string PAIDLEAVEBALANCE { get; set; }
        public string PATERNITYLEAVEBALANCE { get; set; }
        public string MATERNITYLEAVEBALANCE { get; set; }
        public string BEREAVEMENTLEAVEBALANCE { get; set; }
        public string NONCONFIRMEDLEAVEBALANCE { get; set; }
        public string LeaveType { get; set; }
        public string TransactionFlag { get; set; }
        public string LeaveCount { get; set; }
        public string Narration { get; set; }
    }

    public class IndiviualCreditDebit
    {
        public string StaffName { get; set; }
        public string Department { get; set; }
        public List<LeaveTypeAndBalance> LeaveBal { get; set; }
    }

    public class LeaveTypeAndBalance
    {
        public string LeaveTypeId { get; set; }
        public string LeaveName { get; set; }
        public string LeaveBalance { get; set; }
        public string AvailableBalance { get; set; }
    }

    public class IndividualLeaveCreditDebit_EmpDetails
    {
        public string StaffName { get; set; }
        public string Department { get; set; }
        public string CasualLeaveBalance { get; set; }
        public string EarnedLeaveBalance { get; set; }
        public string LOHPLeaveBalance { get; set; }
        public string LTALeaveBalance { get; set; }
        public string RHLeaveBalance { get; set; }
        public string SICKLEAVEBALANCE { get; set; }
        public string BEREAVEMENTLEAVEBALANCE { get; set; }
        public string PAIDLEAVEBALANCE { get; set; }
        public string PATERNITYLEAVEBALANCE { get; set; }
        public string MATERNITYLEAVEBALANCE { get; set; }
        public string NONCONFIRMEDLEAVEBALANCE { get; set; }
    }

    public class NightShiftData
    {
        public string STAFFID { get; set; }
        public string STAFFNAME { get; set; }
        public string PLANT { get; set; }
        public string DEPARTMENT { get; set; }
        public String DESIGNATION { get; set; }
        public string CATEGORY { get; set; }
        public string TXNDATE { get; set; }
        public string SHIFTSHORTNAME { get; set; }
    }

    public class ShiftViolation
    {
        public string STAFFID { get; set; }
        public string NAME { get; set; }
        public string PLANT { get; set; }
        public string TEAM { get; set; }
        public string DEPARTMENT { get; set; }
        public string DESIGNATION { get; set; }
        public string GRADE { get; set; }
        public string TXNDATE { get; set; }
        public string PLANNEDSHIFT { get; set; }
        public string ACTUALSHIFT { get; set; }
    }

    public class RHHolidayList
    {
        public string RHId { get; set; }
        public string RHName { get; set; }
        public string RHDate { get; set; }
    }

    public class Form15
    {
        public string YearDateApplicationForLeave { get; set; }
        public string NOOFDAYS { get; set; }
        public string DATEFROM { get; set; }
        public string DATETO { get; set; }
        public string REASON { get; set; }
        public string NOOFWORKINGDAYS { get; set; }
        public string NOOFWORKEDDAYS { get; set; }
        public string NOOFDAYSLAYOFF { get; set; }
        public string NOOFDAYSLEAVEEARNED { get; set; }
        public string TOTALLEAVESATCREDIT { get; set; }
        public string NOOFDAYSWITHPAY { get; set; }
        public string NOOFDAYSWITHLOSSOFPAY { get; set; }
        public string LEAVEBALANCE { get; set; }
        public string SEC79 { get; set; }
        public string LATEMINUTES { get; set; }
        public string ABSENCE { get; set; }
    }

    public class Form15StaffPersonalDetails
    {
        public string StaffId { get; set; }
        public string Department { get; set; }
        public string DateOfJoining { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string ResAddr { get; set; }
        public string DateOfLeaving { get; set; }
    }

    public class VisitAppointment
    {
        public int VisitId { get; set; }
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string StaffEmailId { get; set; }
        public string CellNo { get; set; }
        public int VisitorId { get; set; }
        public string VisitorName { get; set; }
        public int VisitorCompanyId { get; set; }
        public string VisitorCompany { get; set; }
        public int VisitorBranchId { get; set; }
        public string VisitorAddress { get; set; }
        public string VisitorCellNo { get; set; }
        public int PurposeId { get; set; }
        public int WaitLocationId { get; set; }
        public int VisitorTypeId { get; set; }
        public string VisitStatus { get; set; }
        public List<SelectListItem> PermittedMaterials { get; set; }
        public List<PermittedMaterialList> PermittedMaterialList { get; set; }
        public string VisitDate { get; set; }
        public string ReportingManagerId { get; set; }
        public string ReportingManagerEmailId { get; set; }
        public string ReportingManagerName { get; set; }
        public bool IsEmailToBeSent { get; set; }
        public string Department { get; set; }
        public int MaterialCount { get; set; }
        public int AdditionalVisitors { get; set; }
        public bool NeedApproval { get; set; }
        public bool ShopFloorAccessNeeded { get; set; }
    }

    public class VisitorDetails
    {
        public string Mobile { get; set; }
        public string VisitorName { get; set; }
        public string VisitorCompanyName { get; set; }
        public string BranchAddress { get; set; }
    }

    public class VisitorPassView
    {
        public string Id { get; set; }
        public string VisitorName { get; set; }
        public string VisitorCompany { get; set; }
        public string VisitorAddress { get; set; }
        public string VisitorCellNo { get; set; }
        public string Reason { get; set; }
        public string VisitDate { get; set; }
        public string AllowedThings { get; set; }
    }


    public class VisitPurposeList
    {
        public int VisitPurposeID { get; set; }
        public string Description { get; set; }
    }

    public class VisitTypeList
    {
        public int VisitorTypeID { get; set; }
        public string Description { get; set; }
    }

    public class VisitingAreaList
    {
        public int WaitLocationID { get; set; }
        public string Description { get; set; }
    }

    public class PermittedMaterialList
    {
        public int PermittedMaterialId { get; set; }
        public string PermittedMaterialName { get; set; }
        public bool Checked { get; set; }
    }

    public class DeptWiseReportingManagers
    {
        public string DepartmentId { get; set; }
        public string ReportingManagerId { get; set; }
        public string ReportingManagerName { get; set; }
    }

    public class CellNo
    {
        public int Staffid { get; set; }
        public string Phone { get; set; }

    }

    public class StaffView
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string OfficialPhone { get; set; }
        public bool SendForApproval { get; set; }
    }

    public class ShiftPatternList
    {
        public string Id { get; set; }
        public string PatternName { get; set; }
        public bool UsedAsGeneralShift { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    public class ShiftWeeklyPosting
    {
        public int PatternId { get; set; }
        public List<ShiftWeek> ShiftWeekList { get; set; }
    }

    public class ShiftWeek
    {
        public bool Check { get; set; }
        public int Id { get; set; }
        public string Sunday { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class SMaxData
    {
        public int Id { get; set; }
        public string StaffId { get; set; }
        public string Cardnumber { get; set; }
        public string Title { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string ShortName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public Nullable<System.DateTime> DOS { get; set; }
        public string Company { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Division { get; set; } // Mapped to category
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string Plant { get; set; }
    }
    public class MapScreenToRoleList
    {
        public bool Check { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
    public class MapSecurityScreentoRoleList
    {
        public int Id { get; set; }
        public bool Check { get; set; }
        public int screenid { get; set; }
        public string ScreenName { get; set; }
        public int Roleid { get; set; }
        public string RoleName { get; set; }

    }


    public class ACTList
    {
        public string StaffId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ApplicationType { get; set; }
    }
    public class NightShiftCount
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Plant { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Designation { get; set; }
        public string ReportingManager { get; set; }
        public string AttendanceDate { get; set; }
        public string AttendanceStatus { get; set; }
        public string TotalCount { get; set; }
    }
    public class StaffReportInformation
    {
        public string Staffid { get; set; }
        public string StaffName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string Category { get; set; }
        public string DateofJoining { get; set; }
        public string Resignationdate { get; set; }
        public string OfficialEmail { get; set; }
        public string ConfirmationDate { get; set; }
        public decimal Tenure { get; set; }
        public string Interimhike { get; set; }
        public string OfficialNumber { get; set; }
        public string PersonalNumber { get; set; }
        public string PANNo { get; set; }
        public string PassPortNo { get; set; }
        public string DrivingLicense { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string BankACNo { get; set; }
        public string BankIFSCCode { get; set; }
        public string PFNo { get; set; }
        public string ESINo { get; set; }
        public string PersonalEmail { get; set; }
        public string Gender { get; set; }
        public string StaffStatus { get; set; }
        public string Reportingmanager { get; set; }
        public string BloodGroup { get; set; }
        public string MaritalStatus { get; set; }
        public string Addr { get; set; }
        public string DateofBirth { get; set; }

    }
    public class LeaveandABReport
    {
        public string STAFFID { get; set; }
        public string STAFFNAME { get; set; }
        public string REPORTINGMANAGER { get; set; }
        public string DEPARTMENT { get; set; }
        public string CATEGORY { get; set; }
        public string TXNDATE { get; set; }
        public string ATTSTATUS { get; set; }
        public string DURATION { get; set; }
    }
    public class LeaveAgeingReport
    {
        public string STAFFID { get; set; }
        public string STAFFNAME { get; set; }
        public string REPORTINGMANAGER { get; set; }
        public string DEPARTMENT { get; set; }
        public string CATEGORY { get; set; }
        public decimal LEAVECOUNT { get; set; }
    }
    public class HolidayWorkingDetails
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string Category { get; set; }
        public string ReportingManager { get; set; }
        public string TxnDate { get; set; }
        public string ShiftType { get; set; }
        public string ShiftInTime { get; set; }
        public string ShiftOutTime { get; set; }
        public string ActualInTime { get; set; }
        public string ActualOutTime { get; set; }
        public string ActualWorkedHours { get; set; }
        public string AttendanceStatus { get; set; }

    }
    public class BreakHoursReport
    {
        public string Staffid { get; set; }
        public string StaffName { get; set; }
        public string DEPARTMENT { get; set; }
        public string CATEGORY { get; set; }
        public string ShiftInDate { get; set; }
        public string ShiftOutDate { get; set; }
        public string ActualInDate { get; set; }
        public string BreakHours { get; set; }
        public string IsBreakExceed { get; set; }
    }

    public class ApplicationforcancellationList
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string LeaveType { get; set; }
        public string LeaveTypename { get; set; }
        public string ApplicationShortname { get; set; }
        public string ApplicationId { get; set; }
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string PermissionDate { get; set; }
        public string TotalHours { get; set; }
        public DateTime InDateTime { get; set; }
        public DateTime OutDateTime { get; set; }
        public string PunchType { get; set; }
        public string COffReqDate { get; set; }
        public string COffAvailDate { get; set; }
        public string TotalDays { get; set; }
        public string ODDuration { get; set; }
        public string Reason { get; set; }
        public string AadharNo { get; set; }
        public int IsDocumentAvailable { get; set; }
        public string TxnDate { get; set; }
        public string CoffFrom { get; set; }
        public string CoffTo { get; set; }
        public string WorkedDate { get; set; }
        public string Credit { get; set; }
        public string Duration { get; set; }
        public string ODType { get; set; }
        public string NewShiftName { get; set; }
        public string BeforeShiftHours { get; set; }
        public string AfterShiftHours { get; set; }

    }

    public class BenchReportingManagerDataTableModel
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string Department { get; set; }
        public bool IsActive { get; set; }
    }

    public class BenchReportingManagerModel
    {
        public int Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string Department { get; set; }
        public bool IsActive { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public List<BenchReportingManagerDataTableModel> BenchReportingManagerDataTable { get; set; }
    }

    //public class AttachDetachList
    //{
    //    public int Id { get; set; }
    //    public string StaffId { get; set; }
    //    public string StaffName { get; set; }
    //    public string IsActive { get; set; }
    //}

    //public class AttachDetach
    //{
    //    public int Id { get; set; }
    //    public string StaffId { get; set; }
    //    public string StaffName { get; set; }
    //    public string Department { get; set; }
    //    public string IsActiveText { get; set; }
    //    public bool IsActive { get; set; }
    //    public DateTime? CreatedOn { get; set; }
    //    public string CreatedBy { get; set; }
    //    public List<AttachDetachList> AttachDetachList { get; set; }
    //}


    public class AttachDetachList
    {
        public bool Checked { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string IsActive { get; set; }
    }

    public class AttachDetach
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string DepartmentName { get; set; }
        public List<AttachDetachList> AttachDetachList { get; set; }
    }

    public class AttachDetachStaffList
    {
        public bool Checked { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationId { get; set; }
        public string DesignationName { get; set; }
    }

    public class SubordinateList
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public int SubordinateCount { get; set; }
        public string ReportingStaffId { get; set; }
        public string ReportingStaffName { get; set; }
        public string Signature { get; set; }
        public bool HasSubordinates { get; set; }
    }

    public class CompleteHeadCount
    {
        public string StaffId { get; set; }
        public string TransactionDate { get; set; }
        public string FirstName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string CategoryName { get; set; }
        public string BranchName { get; set; }
        public string GradeName { get; set; }
        public string ShiftName { get; set; }
        public int HeadCount { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
    }

    public class HeadCountOverAll
    {
        public string DepartmentId { get; set; }
        public string DesignationId { get; set; }
        public string CategoryId { get; set; }
        public string ShiftId { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string CategoryName { get; set; }
        public string ShiftName { get; set; }
        public int HeadCount { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int GroupNo { get; set; }
        public int DeptSeq { get; set; }
        public int DesgSeq { get; set; }
        public int GradeSeq { get; set; }
        public int Seq { get; set; }
        public int TotalHeadCount { get; set; }
        public int TotalPresentCount { get; set; }
        public int TotalAbsentCount { get; set; }
    }

    public class ClassesToSave
    {
        public RequestApplication RA { get; set; }
        public ApplicationApproval AA { get; set; }
        public List<EmailSendLog> ESL { get; set; }
        public EmployeeLeaveAccount ELA { get; set; }
        public List<EmployeeLeaveAccount> employeeLeaveAccounts { get; set; }
        public Testing Test { get; set; }
        public AlternativePersonAssign APA { get; set; }
    }

    public class RuleGroup1
    {
        public Int32 RuleId { get; set; }
        public Int32 RuleGroupId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public List<RuleGroup1> Values { get; set; }
    }

    public class MapRuletoRuleGroupList
    {
        public int Id { get; set; }
        public int RuleGroupId { get; set; }
        public string companyid { get; set; }
        public string companyName { get; set; }
        public string RuleGroupTxnsvalue { get; set; }
        public string RuleGroupTxnsdefaultvalue { get; set; }
        public string RuleGroupName { get; set; }
        public int Ruleid { get; set; }
        public string RuleName { get; set; }
        public bool isactive { get; set; }
    }

    public class UserList
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string DeptName { get; set; }
        public string REPMGRFIRSTNAME { get; set; }
    }

    public class LeaveTypeListForHoliday
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class HolidayListForMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public bool IsActive { get; set; }
    }

    public class ShiftDetails
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class LeaveTypeListForExcel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }



    public class BasicDetails
    {
        public string StaffId { get; set; }
        public string Firstname { get; set; }
        public string DateofJoining { get; set; }
        public string Gender { get; set; }
        public string BranchName { get; set; }
        public string DeptName { get; set; }
        public string OfficialPhone { get; set; }
        public string OfficialEmail { get; set; }
        public string MaritalStatus { get; set; }
        public string BloodGroup { get; set; }
        public string DesignationName { get; set; }
        public string CategoryName { get; set; }
        public string LocationName { get; set; }
        public string HomeLocation { get; set; }
        public string HomeCity { get; set; }
        public string HomeDistrict { get; set; }
        public string HomeState { get; set; }
        public string HomeCountry { get; set; }
        public string DateofBirth { get; set; }
        public string PersonalBankName { get; set; }
        public string PersonalBankAccount { get; set; }
        public string PersonalBankIFSCCode { get; set; }
        public string PersonalBankBranch { get; set; }
        public string AadharNo { get; set; }
    }

    public class OverTime
    {
        public Int64 Id { get; set; }
        public string staffid { get; set; }
        public string NAME { get; set; }
        public string Day1 { get; set; }
        public string Day2 { get; set; }
        public string Day3 { get; set; }
        public string Day4 { get; set; }
        public string Day5 { get; set; }
        public string Day6 { get; set; }
        public string Day7 { get; set; }
        public string Day8 { get; set; }
        public string Day9 { get; set; }
        public string Day10 { get; set; }
        public string Day11 { get; set; }
        public string Day12 { get; set; }
        public string Day13 { get; set; }
        public string Day14 { get; set; }
        public string Day15 { get; set; }
        public string Day16 { get; set; }
        public string Day17 { get; set; }
        public string Day18 { get; set; }
        public string Day19 { get; set; }
        public string Day20 { get; set; }
        public string Day21 { get; set; }
        public string Day22 { get; set; }
        public string Day23 { get; set; }
        public string Day24 { get; set; }
        public string Day25 { get; set; }
        public string Day26 { get; set; }
        public string Day27 { get; set; }
        public string Day28 { get; set; }
        public string Day29 { get; set; }
        public string Day30 { get; set; }
        public string Day31 { get; set; }
        public string Total { get; set; }
    }

    public class Remainder
    {
        public string Lastattendanceprocessed { get; set; }
        public Int32 ShiftRoisteringAlertCount { get; set; }
        public string LasttransactionSyncDate { get; set; }
        public int HeadCount { get; set; }
        public int BirthdayAlert { get; set; }
        public Int32 PlannedLeaveCount { get; set; }
        public List<ShiftRoisteringAlert> ShiftPlanChangeList { get; set; }
    }

    public class ShiftRoisteringAlert
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string Department { get; set; }
        public string WithEffectFrom { get; set; }
        public string ShiftName { get; set; }
        public string WorkingDayPattern { get; set; }
        public string WeeklyOff { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string Reason { get; set; }

    }

    public class GetPlannedLeave
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string Department { get; set; }
        public string LeaveTypes { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TotalDays { get; set; }
        public string Reason { get; set; }
    }

    public class GetHeadCountAlertDetails
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string DepartmentName { get; set; }
        public string TransactionDate { get; set; }
        public string DesignationName { get; set; }
        public string CategoryName { get; set; }
        public string BranchName { get; set; }
    }

    public class GetBirthdayAlert
    {
        public string OfficalEmail { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string DepartmentName { get; set; }
        public string DateOfBirth { get; set; }
        public string DesignationName { get; set; }
        public string CategoryName { get; set; }
        public string BranchName { get; set; }
    }

    public class CompoOffModel
    {

        public string StaffName { get; set; }
        public string Department { get; set; }
        public string Category { get; set; }

        public string Remark { get; set; }
        public string StaffId { get; set; }
        public DateTime CompFrom { get; set; }
        public DateTime CompTo { get; set; }
        public string ContNo { get; set; }
        public Decimal LeaveCount { get; set; }
        public string AppliedBy { get; set; }
        public string Id { get; set; }


        public List<LeaveTypeAndBalanceA> LeaveBal { get; set; }
    }
    public class LeaveTypeAndBalanceA
    {
        public string LeaveTypeId { get; set; }
        public string LeaveName { get; set; }
        public string LeaveBalance { get; set; }
        public string AvailableBalance { get; set; }
    }
    public class PolicyDocUploadModel
    {
        public int Id { get; set; }
        public string Staffid { get; set; }
        public string staffname { get; set; }
        public string Policyname { get; set; }
        public string FileType { get; set; }
        public byte[] FileExtension { get; set; }
    }
    public class GetPolicyDoc
    {
        public int Id { get; set; }
        public string Staffid { get; set; }
        public string staffname { get; set; }
        public string Policyname { get; set; }
        public string FileType { get; set; }
        public byte[] FileExtension { get; set; }
    }
    public class RoleOfEmpModel
    {
        public int Id { get; set; }
        public string EmpStfId { get; set; }
        public string Roles { get; set; }
        public string Respons { get; set; }
        public string CreatedBy { get; set; }
        public string Authorities { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class GetEmpRoleModel
    {
        public string Roles { get; set; }
        public string Responsibilities { get; set; }
        public string Authorities { get; set; }
    }

    public class HierarchyEmpList
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public int SubordinateCount { get; set; }
        public string ReportingStaffId { get; set; }
        public string ReportingStaffName { get; set; }
        public string Signature { get; set; }
        public bool HasSubordinates { get; set; }
        public string Email { get; set; }
    }

    public class PWandMsg
    {
        public string ErrorMsgType { get; set; }
        public string Message { get; set; }
        public string Password { get; set; }
    }
    public class GetCompanyHierarchy
    {
        public string DepId { get; set; }
        public string DepName { get; set; }
        public int SubordinateCount { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string Email { get; set; }
    }
    public class ClassesToSaveforOT
    {
        public ApplicationApproval AA { get; set; }
        public List<EmailSendLog> ESL { get; set; }
        public EmployeeLeaveAccount ELA { get; set; }
        public OTApplication OTP { get; set; }
        public string loggedstaffid { get; set; }

    }
    #region Bulk Shift Import
    public class BulkShiftImportModel
    {
        public string StaffId { get; set; }
        public string ShiftName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsValid { get; set; }
    }
    public class FestivalHolidayModel
    {
        public string LeaveTypeName { get; set; }
        public DateTime HolidayDateFrom { get; set; }
        public DateTime HolidayDateTo { get; set; }
    }
    #endregion
    public class GenerateAttendance
    {
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string stafflist { get; set; }
    }
    public class CompensatoryWorkingModel
    {
        public DateTime? LeaveDate { get; set; }
        public DateTime? CompensatoryWorkingDate { get; set; }
        public string Reason { get; set; }
        public string CreatedBy { get; set; }
    }
    //Self
    public class LeaveGroupTxnListModel
    {
        public int Id { get; set; }
        public string LeaveGroupId { get; set; }
        public string LeaveTypeId { get; set; }
        public string Name { get; set; }
        public string LeaveType { get; set; }
        public int LeaveCount { get; set; }
        public int MaxSeqLeaves { get; set; }
        public bool PaidLeave { get; set; }
        public bool Accountable { get; set; }
        public bool CarryForward { get; set; }
        public decimal MaxAccDays { get; set; }
        public decimal MaxAccYears { get; set; }
        public decimal MaxDaysPerReq { get; set; }
        public decimal MinDaysPerReq { get; set; }
        public bool CheckBalance { get; set; }
        public int ElgInMonths { get; set; }
        public bool IsCalcToWorkingDays { get; set; }
        public decimal CalcToWorkingDays { get; set; }
        public bool ConsiderWO { get; set; }
        public bool ConsiderPH { get; set; }
        public bool IsExcessEligibleAllowed { get; set; }
        public bool IsEnCashmentAllowed { get; set; }
        public decimal EncashmentLimit { get; set; }
        public int ComponentId { get; set; }
        public decimal CreditFreq { get; set; }
        public decimal CreditDays { get; set; }
        public bool ProRata { get; set; }
        public string LCAFor { get; set; }
        public string RoundOffTo { get; set; }
        public int RoundOffValue { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
    #region New Reports
    public class DeptWiseGenderHeadCount
    {
        public string DEPARTMENT { get; set; }
        public int MALEHC { get; set; }
        public int FEMALEHC { get; set; }
        public int TOTALHC { get; set; }
        public int MALEPRHC { get; set; }
        public int FEMALEPRHC { get; set; }
        public int TOTALPRHC { get; set; }
        public int MALEABHC { get; set; }
        public int FEMALEABHC { get; set; }
        public int TOTALABHC { get; set; }
    }
    public class ShiftSummaryReport
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string Business { get; set; }
        public string ClassMasterName { get; set; }
        public string CategoryName { get; set; }
        public string CostCentreName { get; set; }
        public string DeptName { get; set; }
        public string DesignationName { get; set; }
        public string EntityName { get; set; }
        public string GradeName { get; set; }
        public string LevelName { get; set; }
        public string SubCategory { get; set; }
        public string SubFunction { get; set; }
        public string Unit { get; set; }
        public string ZoneName { get; set; }
        public string VolumeName { get; set; }
        public string ShiftId { get; set; }
        public string ShiftName { get; set; }
        public int? FOS9_1 { get; set; }
        public int? FOS9_2 { get; set; }
        public int? SGFG9 { get; set; }
        public int? FOS9_3 { get; set; }
        public int? SGFG8 { get; set; }
        public int? CTFG9 { get; set; }
        public int? CTCG8 { get; set; }
        public int? SGCG8 { get; set; }
        public int? SGCG9 { get; set; }
        public int? SWFG1_9H { get; set; }
        public int? SWFG2_9H { get; set; }
        public int? SWFG1_8H { get; set; }
        public int? SWFG2_8H { get; set; }
    }
    public class DrawAttendanceYearlySummary
    {
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public string Designation { get; set; }
        public string SubfunctionName { get; set; }
        public decimal JAN_WORKINGDAYS { get; set; }
        public decimal JAN_PRESENT { get; set; }
        public decimal JAN_ABSENT { get; set; }
        public decimal FEB_WORKINGDAYS { get; set; }
        public decimal FEB_PRESENT { get; set; }
        public decimal FEB_ABSENT { get; set; }
        public decimal MAR_WORKINGDAYS { get; set; }
        public decimal MAR_PRESENT { get; set; }
        public decimal MAR_ABSENT { get; set; }
        public decimal APR_WORKINGDAYS { get; set; }
        public decimal APR_PRESENT { get; set; }
        public decimal APR_ABSENT { get; set; }
        public decimal MAY_WORKINGDAYS { get; set; }
        public decimal MAY_PRESENT { get; set; }
        public decimal MAY_ABSENT { get; set; }
        public decimal JUN_WORKINGDAYS { get; set; }
        public decimal JUN_PRESENT { get; set; }
        public decimal JUN_ABSENT { get; set; }
        public decimal JUL_WORKINGDAYS { get; set; }
        public decimal JUL_PRESENT { get; set; }
        public decimal JUL_ABSENT { get; set; }
        public decimal AUG_WORKINGDAYS { get; set; }
        public decimal AUG_PRESENT { get; set; }
        public decimal AUG_ABSENT { get; set; }
        public decimal SEP_WORKINGDAYS { get; set; }
        public decimal SEP_PRESENT { get; set; }
        public decimal SEP_ABSENT { get; set; }
        public decimal OCT_WORKINGDAYS { get; set; }
        public decimal OCT_PRESENT { get; set; }
        public decimal OCT_ABSENT { get; set; }
        public decimal NOV_WORKINGDAYS { get; set; }
        public decimal NOV_PRESENT { get; set; }
        public decimal NOV_ABSENT { get; set; }
        public decimal DEC_WORKINGDAYS { get; set; }
        public decimal DEC_PRESENT { get; set; }
        public decimal DEC_ABSENT { get; set; }
        public decimal TOTAL_WORKINGDAYS { get; set; }
        public decimal TOTAL_PRESENT { get; set; }
        public decimal TOTAL_ABSENT { get; set; }

    }
    public class DrawFORM10
    {
        public int Id { get; set; }
        public string PayrollId { get; set; }
        public string EmpName { get; set; }
        public string Department { get; set; }
        public string OTDate { get; set; }
        public string ExtentOfOT { get; set; }
        public string PieceWorkers { get; set; }
        public string NormalHours { get; set; }
        public string NormalRate { get; set; }
        public string OverTimeRate { get; set; }
        public string NormalEarnings { get; set; }
        public string OverTimeEarning { get; set; }
        public string CashEquivalent { get; set; }
        public string TotalEarnings { get; set; }
        public string PaymentOn { get; set; }
    }
    public class DrawFORMS
    {
        public int SRNO { get; set; }
        public string PayrollId { get; set; }
        public string EmployeeName { get; set; }
        public string Sex { get; set; }
        public string FatherName { get; set; }
        public string Designation { get; set; }
        public string DOJ { get; set; }
        public string AdultAdolocence { get; set; }
        public string ShiftNumber { get; set; }
        public string TimeOfCommencementOfWork { get; set; }
        public string TimeOfRestInterval { get; set; }
        public string TimeAtWhichWorkCeases { get; set; }
        public string WeeklyHoliday { get; set; }
        public string ClassOfWorkers { get; set; }
        public Decimal MaxRatesOfWages { get; set; }
        public Decimal MinRatesOfWages { get; set; }
    }
    public class LateInEarlyOutReport
    {
        public string StaffId { get; set; }
        public string TxnDate { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Designation { get; set; }
        public string SubFunction { get; set; }
        public string Grade { get; set; }
        public string Category { get; set; }
        public string CostCentre { get; set; }
        public string Location { get; set; }
        public string Business { get; set; }
        public string ClassMaster { get; set; }
        public string Entity { get; set; }
        public string EmploymentType { get; set; }
        public string Level { get; set; }
        public string Zone { get; set; }
        public string ShiftName { get; set; }
        public string ScheduledInTime { get; set; }
        public string ActualInTime { get; set; }
        public string ScheduledOutTime { get; set; }
        public string ActualOutTime { get; set; }
        public string LateIn { get; set; }
        public string EarlyOut { get; set; }
    }


    public class SalaryOTSAfinal
    {
        public Int64 Id { get; set; }
        public string StaffId { get; set; }
        public string NAME { get; set; }
        public string UNIT { get; set; }
        public string DEPARTMENT { get; set; }
        public string TRADE { get; set; }
        public string CATEGORY { get; set; }
        public string Day1Name { get; set; }
        public string Day2Name { get; set; }
        public string Day3Name { get; set; }
        public string Day4Name { get; set; }
        public string Day5Name { get; set; }
        public string Day6Name { get; set; }
        public string Day7Name { get; set; }
        public string Day8Name { get; set; }
        public string Day9Name { get; set; }
        public string Day10Name { get; set; }
        public string Day11Name { get; set; }
        public string Day12Name { get; set; }
        public string Day13Name { get; set; }
        public string Day14Name { get; set; }
        public string Day15Name { get; set; }
        public string Day16Name { get; set; }
        public string Day17Name { get; set; }
        public string Day18Name { get; set; }
        public string Day19Name { get; set; }
        public string Day20Name { get; set; }
        public string Day21Name { get; set; }
        public string Day22Name { get; set; }
        public string Day23Name { get; set; }
        public string Day24Name { get; set; }
        public string Day25Name { get; set; }
        public string Day26Name { get; set; }
        public string Day27Name { get; set; }
        public string Day28Name { get; set; }
        public string Day29Name { get; set; }
        public string Day30Name { get; set; }
        public string Day31Name { get; set; }
        public string Day1 { get; set; }
        public string Day2 { get; set; }
        public string Day3 { get; set; }
        public string Day4 { get; set; }
        public string Day5 { get; set; }
        public string Day6 { get; set; }
        public string Day7 { get; set; }
        public string Day8 { get; set; }
        public string Day9 { get; set; }
        public string Day10 { get; set; }
        public string Day11 { get; set; }
        public string Day12 { get; set; }
        public string Day13 { get; set; }
        public string Day14 { get; set; }
        public string Day15 { get; set; }
        public string Day16 { get; set; }
        public string Day17 { get; set; }
        public string Day18 { get; set; }
        public string Day19 { get; set; }
        public string Day20 { get; set; }
        public string Day21 { get; set; }
        public string Day22 { get; set; }
        public string Day23 { get; set; }
        public string Day24 { get; set; }
        public string Day25 { get; set; }
        public string Day26 { get; set; }
        public string Day27 { get; set; }
        public string Day28 { get; set; }
        public string Day29 { get; set; }
        public string Day30 { get; set; }
        public string Day31 { get; set; }
        public string PRESENT { get; set; }
        public string OD { get; set; }
        public string CO { get; set; }
        public string SATOFF { get; set; }
        public string PL { get; set; }
        public string ML { get; set; }
        public string ABSENTCOUNT { get; set; }
        public string SATWO { get; set; }
        public string SUNWO { get; set; }
        public string NH { get; set; }
        public string OTHERS { get; set; }
        public string TOTAL { get; set; }
        public string OPENINGLEAVEBALANCE { get; set; }
        public string LEAVEAVAILED { get; set; }
        public string CLOSINGBALANCE { get; set; }
        public string General { get; set; }
        public string FirstShift { get; set; }
        public string SecondShift { get; set; }
        public string ThirdShift { get; set; }
        public string FTWHR { get; set; }
        public string STWHR { get; set; }
        public string ACTUALOTHOURS { get; set; }
        public string TOTALOTHOURS { get; set; }
        public string SAT { get; set; }
        public string SUN { get; set; }
        public string HOLIDAY { get; set; }
    }
    public class ShiftAllowance
    {
        public Int64 Id { get; set; }
        public string StaffId { get; set; }
        public string NAME { get; set; }
        public string UNIT { get; set; }
        public string DEPARTMENT { get; set; }
        public string TRADE { get; set; }
        public string CATEGORY { get; set; }
        public string Day1 { get; set; }
        public string Day2 { get; set; }
        public string Day3 { get; set; }
        public string Day4 { get; set; }
        public string Day5 { get; set; }
        public string Day6 { get; set; }
        public string Day7 { get; set; }
        public string Day8 { get; set; }
        public string Day9 { get; set; }
        public string Day10 { get; set; }
        public string Day11 { get; set; }
        public string Day12 { get; set; }
        public string Day13 { get; set; }
        public string Day14 { get; set; }
        public string Day15 { get; set; }
        public string Day16 { get; set; }
        public string Day17 { get; set; }
        public string Day18 { get; set; }
        public string Day19 { get; set; }
        public string Day20 { get; set; }
        public string Day21 { get; set; }
        public string Day22 { get; set; }
        public string Day23 { get; set; }
        public string Day24 { get; set; }
        public string Day25 { get; set; }
        public string Day26 { get; set; }
        public string Day27 { get; set; }
        public string Day28 { get; set; }
        public string Day29 { get; set; }
        public string Day30 { get; set; }
        public string Day31 { get; set; }
        public string General { get; set; }
        public string FirstShift { get; set; }
        public string SecondShift { get; set; }
        public string ThirdShift { get; set; }
        public string FTWHR { get; set; }
        public string STWHR { get; set; }
        public string TOTALSHIFT { get; set; }
    }

    public class YearlyReportShiftDetails
    {

        public Int64 Id { get; set; }
        public string StaffId { get; set; }
        public string NAME { get; set; }
        public string UNIT { get; set; }
        public string DEPARTMENT { get; set; }
        public string TRADE { get; set; }
        public string CATEGORY { get; set; }
        public string GeneralM1 { get; set; }
        public string GeneralM2 { get; set; }
        public string GeneralM3 { get; set; }
        public string GeneralM4 { get; set; }
        public string GeneralM5 { get; set; }
        public string GeneralM6 { get; set; }
        public string GeneralM7 { get; set; }
        public string GeneralM8 { get; set; }
        public string GeneralM9 { get; set; }
        public string GeneralM10 { get; set; }
        public string GeneralM11 { get; set; }
        public string GeneralM12 { get; set; }
        public string FirstShiftM1 { get; set; }
        public string FirstShiftM2 { get; set; }
        public string FirstShiftM3 { get; set; }
        public string FirstShiftM4 { get; set; }
        public string FirstShiftM5 { get; set; }
        public string FirstShiftM6 { get; set; }
        public string FirstShiftM7 { get; set; }
        public string FirstShiftM8 { get; set; }
        public string FirstShiftM9 { get; set; }
        public string FirstShiftM10 { get; set; }
        public string FirstShiftM11 { get; set; }
        public string FirstShiftM12 { get; set; }
        public string SecondShiftM1 { get; set; }
        public string SecondShiftM2 { get; set; }
        public string SecondShiftM3 { get; set; }
        public string SecondShiftM4 { get; set; }
        public string SecondShiftM5 { get; set; }
        public string SecondShiftM6 { get; set; }
        public string SecondShiftM7 { get; set; }
        public string SecondShiftM8 { get; set; }
        public string SecondShiftM9 { get; set; }
        public string SecondShiftM10 { get; set; }
        public string SecondShiftM11 { get; set; }
        public string SecondShiftM12 { get; set; }
        public string ThirdShiftM1 { get; set; }
        public string ThirdShiftM2 { get; set; }
        public string ThirdShiftM3 { get; set; }
        public string ThirdShiftM4 { get; set; }
        public string ThirdShiftM5 { get; set; }
        public string ThirdShiftM6 { get; set; }
        public string ThirdShiftM7 { get; set; }
        public string ThirdShiftM8 { get; set; }
        public string ThirdShiftM9 { get; set; }
        public string ThirdShiftM10 { get; set; }
        public string ThirdShiftM11 { get; set; }
        public string ThirdShiftM12 { get; set; }
        public string FTWHRM1 { get; set; }
        public string FTWHRM2 { get; set; }
        public string FTWHRM3 { get; set; }
        public string FTWHRM4 { get; set; }
        public string FTWHRM5 { get; set; }
        public string FTWHRM6 { get; set; }
        public string FTWHRM7 { get; set; }
        public string FTWHRM8 { get; set; }
        public string FTWHRM9 { get; set; }
        public string FTWHRM10 { get; set; }
        public string FTWHRM11 { get; set; }
        public string FTWHRM12 { get; set; }
        public string STWHRM1 { get; set; }
        public string STWHRM2 { get; set; }
        public string STWHRM3 { get; set; }
        public string STWHRM4 { get; set; }
        public string STWHRM5 { get; set; }
        public string STWHRM6 { get; set; }
        public string STWHRM7 { get; set; }
        public string STWHRM8 { get; set; }
        public string STWHRM9 { get; set; }
        public string STWHRM10 { get; set; }
        public string STWHRM11 { get; set; }
        public string STWHRM12 { get; set; }
        public string TotalGeneral { get; set; }
        public string TotalFirstShift { get; set; }
        public string TotalSecondShift { get; set; }
        public string TotalThirdShift { get; set; }
        public string TotalFTWHR { get; set; }
        public string TotalSTWHR { get; set; }

    }
    public class YearlyReportWorkedDays
    {
        public Int64 Id { get; set; }
        public string StaffId { get; set; }
        public string NAME { get; set; }
        public string UNIT { get; set; }
        public string DEPARTMENT { get; set; }
        public string TRADE { get; set; }
        public string CATEGORY { get; set; }
        public string PresentM1 { get; set; }
        public string PresentM2 { get; set; }
        public string PresentM3 { get; set; }
        public string PresentM4 { get; set; }
        public string PresentM5 { get; set; }
        public string PresentM6 { get; set; }
        public string PresentM7 { get; set; }
        public string PresentM8 { get; set; }
        public string PresentM9 { get; set; }
        public string PresentM10 { get; set; }
        public string PresentM11 { get; set; }
        public string PresentM12 { get; set; }
        public string ODM1 { get; set; }
        public string ODM2 { get; set; }
        public string ODM3 { get; set; }
        public string ODM4 { get; set; }
        public string ODM5 { get; set; }
        public string ODM6 { get; set; }
        public string ODM7 { get; set; }
        public string ODM8 { get; set; }
        public string ODM9 { get; set; }
        public string ODM10 { get; set; }
        public string ODM11 { get; set; }
        public string ODM12 { get; set; }
        public string TOTALOTHOURS { get; set; }
    }
    public class YearlyOTReport
    {
        public Int64 Id { get; set; }
        public string StaffId { get; set; }
        public string NAME { get; set; }
        public string UNIT { get; set; }
        public string DEPARTMENT { get; set; }
        public string TRADE { get; set; }
        public string CATEGORY { get; set; }
        public string OTM1 { get; set; }
        public string OTM2 { get; set; }
        public string OTM3 { get; set; }
        public string OTM4 { get; set; }
        public string OTM5 { get; set; }
        public string OTM6 { get; set; }
        public string OTM7 { get; set; }
        public string OTM8 { get; set; }
        public string OTM9 { get; set; }
        public string OTM10 { get; set; }
        public string OTM11 { get; set; }
        public string OTM12 { get; set; }
        public string TOTALOTHOURS { get; set; }
    }
    public class LeaveReport
    {

        public Int64 Id { get; set; }
        public string StaffId { get; set; }
        public string NAME { get; set; }
        public string UNIT { get; set; }
        public string DEPARTMENT { get; set; }
        public string TRADE { get; set; }
        public string CATEGORY { get; set; }
        public string Day1 { get; set; }
        public string Day2 { get; set; }
        public string Day3 { get; set; }
        public string Day4 { get; set; }
        public string Day5 { get; set; }
        public string Day6 { get; set; }
        public string Day7 { get; set; }
        public string Day8 { get; set; }
        public string Day9 { get; set; }
        public string Day10 { get; set; }
        public string Day11 { get; set; }
        public string Day12 { get; set; }
        public string Day13 { get; set; }
        public string Day14 { get; set; }
        public string Day15 { get; set; }
        public string Day16 { get; set; }
        public string Day17 { get; set; }
        public string Day18 { get; set; }
        public string Day19 { get; set; }
        public string Day20 { get; set; }
        public string Day21 { get; set; }
        public string Day22 { get; set; }
        public string Day23 { get; set; }
        public string Day24 { get; set; }
        public string Day25 { get; set; }
        public string Day26 { get; set; }
        public string Day27 { get; set; }
        public string Day28 { get; set; }
        public string Day29 { get; set; }
        public string Day30 { get; set; }
        public string Day31 { get; set; }
        public string PL { get; set; }
        public string ML { get; set; }
        public string AB { get; set; }
        public string OTHERS { get; set; }
        public string TOTALDAYS { get; set; }
        public string LEAVECREDITED { get; set; }
        public string LEAVEAVAILABLE { get; set; }
        public string PERCENTAGEOFLEAVETAKEN { get; set; }
        public string LOP { get; set; }
        public string PERMISSION { get; set; }
        public string WORKINGDAYS { get; set; }
        public string WORKEDDAYS { get; set; }
        public string NOOFDAYS { get; set; }
    }
    public class ShopFloorAttendance
    {

        public Int64 Id { get; set; }
        public string STAFFID { get; set; }
        public string STAFFNAME { get; set; }
        public string DEPARTMENT { get; set; }
        public string BRANCH { get; set; }
        public string CATEGORY { get; set; }
        public string DOJ { get; set; }
        public string TRADE { get; set; }
        public string TRADESF { get; set; }
        public string SHIFTDATE { get; set; }
        public string SHIFTS { get; set; }
        public string GATEIN { get; set; }
        public string GATEOUT { get; set; }
        public string FIRSTSHOPFLOORIN { get; set; }
        public string LUNCHOUT { get; set; }
        public string LUNCHIN { get; set; }
        public string LASTSHOPFLOOROUT { get; set; }
        public string DIFFERENCEINHOURS { get; set; }
        public string SHOPFLOORHOURS { get; set; }
        public string DIFINHOURS { get; set; }
        public string TOTALHOURS { get; set; }

    }
    public class DailyAttendance_Trd_Dep_Cat
    {

        public Int64 Id { get; set; }
        public string NAME { get; set; }
        public string DayP1 { get; set; }
        public string DayP2 { get; set; }
        public string DayP3 { get; set; }
        public string DayP4 { get; set; }
        public string DayP5 { get; set; }
        public string DayP6 { get; set; }
        public string DayP7 { get; set; }
        public string DayP8 { get; set; }
        public string DayP9 { get; set; }
        public string DayP10 { get; set; }
        public string DayP11 { get; set; }
        public string DayP12 { get; set; }
        public string DayP13 { get; set; }
        public string DayP14 { get; set; }
        public string DayP15 { get; set; }
        public string DayP16 { get; set; }
        public string DayP17 { get; set; }
        public string DayP18 { get; set; }
        public string DayP19 { get; set; }
        public string DayP20 { get; set; }
        public string DayP21 { get; set; }
        public string DayP22 { get; set; }
        public string DayP23 { get; set; }
        public string DayP24 { get; set; }
        public string DayP25 { get; set; }
        public string DayP26 { get; set; }
        public string DayP27 { get; set; }
        public string DayP28 { get; set; }
        public string DayP29 { get; set; }
        public string DayP30 { get; set; }
        public string DayP31 { get; set; }
        public string DayA1 { get; set; }
        public string DayA2 { get; set; }
        public string DayA3 { get; set; }
        public string DayA4 { get; set; }
        public string DayA5 { get; set; }
        public string DayA6 { get; set; }
        public string DayA7 { get; set; }
        public string DayA8 { get; set; }
        public string DayA9 { get; set; }
        public string DayA10 { get; set; }
        public string DayA11 { get; set; }
        public string DayA12 { get; set; }
        public string DayA13 { get; set; }
        public string DayA14 { get; set; }
        public string DayA15 { get; set; }
        public string DayA16 { get; set; }
        public string DayA17 { get; set; }
        public string DayA18 { get; set; }
        public string DayA19 { get; set; }
        public string DayA20 { get; set; }
        public string DayA21 { get; set; }
        public string DayA22 { get; set; }
        public string DayA23 { get; set; }
        public string DayA24 { get; set; }
        public string DayA25 { get; set; }
        public string DayA26 { get; set; }
        public string DayA27 { get; set; }
        public string DayA28 { get; set; }
        public string DayA29 { get; set; }
        public string DayA30 { get; set; }
        public string DayA31 { get; set; }
        public string TOTALHC { get; set; }
    }
    public class ShopFloorAttendanceMonthWise
    {

        public Int64 Id { get; set; }
        public string StaffId { get; set; }
        public string NAME { get; set; }
        public string PLANT { get; set; }
        public string DEPARTMENT { get; set; }
        public string CATEGORY { get; set; }
        public string TRADE { get; set; }
        public string Day1GI { get; set; }
        public string Day2GI { get; set; }
        public string Day3GI { get; set; }
        public string Day4GI { get; set; }
        public string Day5GI { get; set; }
        public string Day6GI { get; set; }
        public string Day7GI { get; set; }
        public string Day8GI { get; set; }
        public string Day9GI { get; set; }
        public string Day10GI { get; set; }
        public string Day11GI { get; set; }
        public string Day12GI { get; set; }
        public string Day13GI { get; set; }
        public string Day14GI { get; set; }
        public string Day15GI { get; set; }
        public string Day16GI { get; set; }
        public string Day17GI { get; set; }
        public string Day18GI { get; set; }
        public string Day19GI { get; set; }
        public string Day20GI { get; set; }
        public string Day21GI { get; set; }
        public string Day22GI { get; set; }
        public string Day23GI { get; set; }
        public string Day24GI { get; set; }
        public string Day25GI { get; set; }
        public string Day26GI { get; set; }
        public string Day27GI { get; set; }
        public string Day28GI { get; set; }
        public string Day29GI { get; set; }
        public string Day30GI { get; set; }
        public string Day31GI { get; set; }
        public string Day1SFI { get; set; }
        public string Day2SFI { get; set; }
        public string Day3SFI { get; set; }
        public string Day4SFI { get; set; }
        public string Day5SFI { get; set; }
        public string Day6SFI { get; set; }
        public string Day7SFI { get; set; }
        public string Day8SFI { get; set; }
        public string Day9SFI { get; set; }
        public string Day10SFI { get; set; }
        public string Day11SFI { get; set; }
        public string Day12SFI { get; set; }
        public string Day13SFI { get; set; }
        public string Day14SFI { get; set; }
        public string Day15SFI { get; set; }
        public string Day16SFI { get; set; }
        public string Day17SFI { get; set; }
        public string Day18SFI { get; set; }
        public string Day19SFI { get; set; }
        public string Day20SFI { get; set; }
        public string Day21SFI { get; set; }
        public string Day22SFI { get; set; }
        public string Day23SFI { get; set; }
        public string Day24SFI { get; set; }
        public string Day25SFI { get; set; }
        public string Day26SFI { get; set; }
        public string Day27SFI { get; set; }
        public string Day28SFI { get; set; }
        public string Day29SFI { get; set; }
        public string Day30SFI { get; set; }
        public string Day31SFI { get; set; }
        public string Day1BO { get; set; }
        public string Day2BO { get; set; }
        public string Day3BO { get; set; }
        public string Day4BO { get; set; }
        public string Day5BO { get; set; }
        public string Day6BO { get; set; }
        public string Day7BO { get; set; }
        public string Day8BO { get; set; }
        public string Day9BO { get; set; }
        public string Day10BO { get; set; }
        public string Day11BO { get; set; }
        public string Day12BO { get; set; }
        public string Day13BO { get; set; }
        public string Day14BO { get; set; }
        public string Day15BO { get; set; }
        public string Day16BO { get; set; }
        public string Day17BO { get; set; }
        public string Day18BO { get; set; }
        public string Day19BO { get; set; }
        public string Day20BO { get; set; }
        public string Day21BO { get; set; }
        public string Day22BO { get; set; }
        public string Day23BO { get; set; }
        public string Day24BO { get; set; }
        public string Day25BO { get; set; }
        public string Day26BO { get; set; }
        public string Day27BO { get; set; }
        public string Day28BO { get; set; }
        public string Day29BO { get; set; }
        public string Day30BO { get; set; }
        public string Day31BO { get; set; }
        public string Day1BI { get; set; }
        public string Day2BI { get; set; }
        public string Day3BI { get; set; }
        public string Day4BI { get; set; }
        public string Day5BI { get; set; }
        public string Day6BI { get; set; }
        public string Day7BI { get; set; }
        public string Day8BI { get; set; }
        public string Day9BI { get; set; }
        public string Day10BI { get; set; }
        public string Day11BI { get; set; }
        public string Day12BI { get; set; }
        public string Day13BI { get; set; }
        public string Day14BI { get; set; }
        public string Day15BI { get; set; }
        public string Day16BI { get; set; }
        public string Day17BI { get; set; }
        public string Day18BI { get; set; }
        public string Day19BI { get; set; }
        public string Day20BI { get; set; }
        public string Day21BI { get; set; }
        public string Day22BI { get; set; }
        public string Day23BI { get; set; }
        public string Day24BI { get; set; }
        public string Day25BI { get; set; }
        public string Day26BI { get; set; }
        public string Day27BI { get; set; }
        public string Day28BI { get; set; }
        public string Day29BI { get; set; }
        public string Day30BI { get; set; }
        public string Day31BI { get; set; }
        public string Day1SFO { get; set; }
        public string Day2SFO { get; set; }
        public string Day3SFO { get; set; }
        public string Day4SFO { get; set; }
        public string Day5SFO { get; set; }
        public string Day6SFO { get; set; }
        public string Day7SFO { get; set; }
        public string Day8SFO { get; set; }
        public string Day9SFO { get; set; }
        public string Day10SFO { get; set; }
        public string Day11SFO { get; set; }
        public string Day12SFO { get; set; }
        public string Day13SFO { get; set; }
        public string Day14SFO { get; set; }
        public string Day15SFO { get; set; }
        public string Day16SFO { get; set; }
        public string Day17SFO { get; set; }
        public string Day18SFO { get; set; }
        public string Day19SFO { get; set; }
        public string Day20SFO { get; set; }
        public string Day21SFO { get; set; }
        public string Day22SFO { get; set; }
        public string Day23SFO { get; set; }
        public string Day24SFO { get; set; }
        public string Day25SFO { get; set; }
        public string Day26SFO { get; set; }
        public string Day27SFO { get; set; }
        public string Day28SFO { get; set; }
        public string Day29SFO { get; set; }
        public string Day30SFO { get; set; }
        public string Day31SFO { get; set; }
        public string Day1GO { get; set; }
        public string Day2GO { get; set; }
        public string Day3GO { get; set; }
        public string Day4GO { get; set; }
        public string Day5GO { get; set; }
        public string Day6GO { get; set; }
        public string Day7GO { get; set; }
        public string Day8GO { get; set; }
        public string Day9GO { get; set; }
        public string Day10GO { get; set; }
        public string Day11GO { get; set; }
        public string Day12GO { get; set; }
        public string Day13GO { get; set; }
        public string Day14GO { get; set; }
        public string Day15GO { get; set; }
        public string Day16GO { get; set; }
        public string Day17GO { get; set; }
        public string Day18GO { get; set; }
        public string Day19GO { get; set; }
        public string Day20GO { get; set; }
        public string Day21GO { get; set; }
        public string Day22GO { get; set; }
        public string Day23GO { get; set; }
        public string Day24GO { get; set; }
        public string Day25GO { get; set; }
        public string Day26GO { get; set; }
        public string Day27GO { get; set; }
        public string Day28GO { get; set; }
        public string Day29GO { get; set; }
        public string Day30GO { get; set; }
        public string Day31GO { get; set; }
        public string Day1SFTH { get; set; }
        public string Day2SFTH { get; set; }
        public string Day3SFTH { get; set; }
        public string Day4SFTH { get; set; }
        public string Day5SFTH { get; set; }
        public string Day6SFTH { get; set; }
        public string Day7SFTH { get; set; }
        public string Day8SFTH { get; set; }
        public string Day9SFTH { get; set; }
        public string Day10SFTH { get; set; }
        public string Day11SFTH { get; set; }
        public string Day12SFTH { get; set; }
        public string Day13SFTH { get; set; }
        public string Day14SFTH { get; set; }
        public string Day15SFTH { get; set; }
        public string Day16SFTH { get; set; }
        public string Day17SFTH { get; set; }
        public string Day18SFTH { get; set; }
        public string Day19SFTH { get; set; }
        public string Day20SFTH { get; set; }
        public string Day21SFTH { get; set; }
        public string Day22SFTH { get; set; }
        public string Day23SFTH { get; set; }
        public string Day24SFTH { get; set; }
        public string Day25SFTH { get; set; }
        public string Day26SFTH { get; set; }
        public string Day27SFTH { get; set; }
        public string Day28SFTH { get; set; }
        public string Day29SFTH { get; set; }
        public string Day30SFTH { get; set; }
        public string Day31SFTH { get; set; }
        public string Day1THC { get; set; }
        public string Day2THC { get; set; }
        public string Day3THC { get; set; }
        public string Day4THC { get; set; }
        public string Day5THC { get; set; }
        public string Day6THC { get; set; }
        public string Day7THC { get; set; }
        public string Day8THC { get; set; }
        public string Day9THC { get; set; }
        public string Day10THC { get; set; }
        public string Day11THC { get; set; }
        public string Day12THC { get; set; }
        public string Day13THC { get; set; }
        public string Day14THC { get; set; }
        public string Day15THC { get; set; }
        public string Day16THC { get; set; }
        public string Day17THC { get; set; }
        public string Day18THC { get; set; }
        public string Day19THC { get; set; }
        public string Day20THC { get; set; }
        public string Day21THC { get; set; }
        public string Day22THC { get; set; }
        public string Day23THC { get; set; }
        public string Day24THC { get; set; }
        public string Day25THC { get; set; }
        public string Day26THC { get; set; }
        public string Day27THC { get; set; }
        public string Day28THC { get; set; }
        public string Day29THC { get; set; }
        public string Day30THC { get; set; }
        public string Day31THC { get; set; }
        public string Day1D { get; set; }
        public string Day2D { get; set; }
        public string Day3D { get; set; }
        public string Day4D { get; set; }
        public string Day5D { get; set; }
        public string Day6D { get; set; }
        public string Day7D { get; set; }
        public string Day8D { get; set; }
        public string Day9D { get; set; }
        public string Day10D { get; set; }
        public string Day11D { get; set; }
        public string Day12D { get; set; }
        public string Day13D { get; set; }
        public string Day14D { get; set; }
        public string Day15D { get; set; }
        public string Day16D { get; set; }
        public string Day17D { get; set; }
        public string Day18D { get; set; }
        public string Day19D { get; set; }
        public string Day20D { get; set; }
        public string Day21D { get; set; }
        public string Day22D { get; set; }
        public string Day23D { get; set; }
        public string Day24D { get; set; }
        public string Day25D { get; set; }
        public string Day26D { get; set; }
        public string Day27D { get; set; }
        public string Day28D { get; set; }
        public string Day29D { get; set; }
        public string Day30D { get; set; }
        public string Day31D { get; set; }
        public string WORKED { get; set; }
        public string NOTWORKED { get; set; }
        public string ACHIEVED { get; set; }
        public string NOTACHIEVED { get; set; }
        public string UPDATEFLAG { get; set; }

    }
    public class ExtraHours
    {
        public string staffid { get; set; }
        public string NAME { get; set; }
        public string Unit { get; set; }
        public string Dept { get; set; }
        public string Trade { get; set; }
        public string Category { get; set; }
        public string Total { get; set; }
        public string Day1 { get; set; }
        public string Day2 { get; set; }
        public string Day3 { get; set; }
        public string Day4 { get; set; }
        public string Day5 { get; set; }
        public string Day6 { get; set; }
        public string Day7 { get; set; }
        public string Day8 { get; set; }
        public string Day9 { get; set; }
        public string Day10 { get; set; }
        public string Day11 { get; set; }
        public string Day12 { get; set; }
        public string Day13 { get; set; }
        public string Day14 { get; set; }
        public string Day15 { get; set; }
        public string Day16 { get; set; }
        public string Day17 { get; set; }
        public string Day18 { get; set; }
        public string Day19 { get; set; }
        public string Day20 { get; set; }
        public string Day21 { get; set; }
        public string Day22 { get; set; }
        public string Day23 { get; set; }
        public string Day24 { get; set; }
        public string Day25 { get; set; }
        public string Day26 { get; set; }
        public string Day27 { get; set; }
        public string Day28 { get; set; }
        public string Day29 { get; set; }
        public string Day30 { get; set; }
        public string Day31 { get; set; }
    }
    #endregion
    public class BirthDayModel
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string CompanyName { get; set; }
        public string Designation { get; set; }
        public string DeptName { get; set; }
    }
    public class BulkLeaveCreditDebitModel
    {
        public string StaffId { get; set; }
        public string LeaveType { get; set; }
        public string TransactionType { get; set; }
        public string LeaveCount { get; set; }
        public string LeaveCreditDebitReason { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsValid { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
    public class RandomSearch
    {
        public string StaffId { get; set; }
        public string StaffList { get; set; }
        public bool includetermination { get; set; }
        public string beginning { get; set; }
        public string ending { get; set; }
    }
    public class ValidateForgotPwdModel
    {
        public string StaffId { get; set; }
        public DateTime? DOJ { get; set; }
        public string Email { get; set; }
    }
    #region Attendance Policy
    public class AttendancePolicyModel
    {
        public string RuleName { get; set; }
        public string Type { get; set; }
        public string RuleType { get; set; }
        public int SeqId { get; set; }
        public string PolicyName { get; set; }
        public string LocationName { get; set; }
        public string Value { get; set; }
    }
    #endregion
    public class PermissionRequest
    {
        public string PermissionId { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string PermissionDate { get; set; }
        public string FromTime { get; set; }
        public string TimeTo { get; set; }
        public string PermissionOffReason { get; set; }
        public string ContactNumber { get; set; }
        public string ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApplicationApprovalId { get; set; }
        public string ApprovedOnDate { get; set; }
        public string ApprovedOnTime { get; set; }
        public string Comment { get; set; }
        public string ApprovalOwner { get; set; }
        public string ParentType { get; set; }
    }
    public class PermissionRequisitionHistory
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string PermissionDate { get; set; }
        public string PermissionType { get; set; }
        public string TotalHours { get; set; }
        public string Reason { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string ReviewalStatus { get; set; }
        public string ReviewedBy { get; set; }
        public string ReviewedOn { get; set; }
    }
    public class ManualPunchRequest
    {
        public string ManualPunchId { get; set; }
        public string StaffId { get; set; }
        public string PunchType { get; set; }
        public string InDate { get; set; }
        public string InTime { get; set; }
        public string OutDate { get; set; }
        public string OutTime { get; set; }
        public string ManualPunchReason { get; set; }
        public string FirstName { get; set; }
        public string ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApplicationApprovalId { get; set; }
        public string ApprovedOnDate { get; set; }
        public string ApprovedOnTime { get; set; }
        public string Comment { get; set; }
        public string ApprovalOwner { get; set; }
        public string ParentType { get; set; }
    }
    public class OnDutyReportModel
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ODFromDate { get; set; }
        public string ODToDate { get; set; }
        public string ODDuration { get; set; }
        public string ToTalHours { get; set; }
        public string Reason { get; set; }
        public string ApplicationDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string AppliedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string ReviewalStatus { get; set; }
        public string ReviewedBy { get; set; }
    }
    public class COffCreditReportModel
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string WorkedDate { get; set; }
        public decimal Credit { get; set; }
        public string Reason { get; set; }
        public string ApplicationDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ReviewalStatus { get; set; }
        public string ReviewedBy { get; set; }
        public string ReviewedOn { get; set; }
    }
    public class ShiftChangeApprvalModel
    {
        public string Approver1 { get; set; }
        public string Approver2 { get; set; }
        public int ApproverLevel { get; set; }
    }
    public class ValidateShiftChangeModel
    {
        public int ApprovalStatusId { get; set; }
        public int Approval2statusId { get; set; }
    }
    public class BusinessTravelReportModel
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Duration { get; set; }
        public string ToTalHours { get; set; }
        public string Reason { get; set; }
        public string ApplicationDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string AppliedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string ReviewalStatus { get; set; }
        public string ReviewedBy { get; set; }
    }
    public class COffReqAvailModel
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }
        public string WorkedDate { get; set; }
        public string ExpiryDate { get; set; }
    }
    #region Common Permission
    public class CommonPermissionModel
    {
        public string PermissionDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TotalHours { get; set; }
        public string Remarks { get; set; }
        public string PermissionTypeId { get; set; }
    }
    public class CommonPermissionReportModel
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Branch { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string TotalHours { get; set; }
        public string PermissionDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string ReviewalStatus { get; set; }
        public string ApprovedBy { get; set; }
        public string ReviewedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string ReviewedOn { get; set; }
    }
    public class LeaveBalance
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Department { get; set; }
        public string Grade { get; set; }
        public string Category { get; set; }
        public decimal BAL_CL { get; set; }
        public decimal BAL_SL { get; set; }
        public decimal BAL_PL { get; set; }
    }
    public class LeaveAvailedReport
    {
        public string STAFFID { get; set; }
        public string NAME { get; set; }
        public string DESIGNATION { get; set; }
        public string DIVISION { get; set; }
        public string DEPARTMENT { get; set; }
        public decimal CL_TAKEN { get; set; }
        public decimal LOP_TAKEN { get; set; }
        public decimal PL_TAKEN { get; set; }
        public decimal SL_TAKEN { get; set; }
        public decimal CL_DEDUCTION { get; set; }
        public decimal PL_DEDUCTION { get; set; }
        public decimal SL_DEDUCTION { get; set; }
        public decimal LOP_DEDUCTION { get; set; }
        public decimal CL_TOTAL { get; set; }
        public decimal PL_TOTAL { get; set; }
        public decimal SL_TOTAL { get; set; }
        public decimal LOP_TOTAL { get; set; }
    }

    #endregion

    #region  Bulk Masters Import
    public class BulkMasterImportModel
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string MasterValue { get; set; }
    }
    #endregion

    #region Staff Removing Process
    public class RemoveStaffModel
    {
        public string StaffId { get; set; }
        public DateTime? RelievingDate { get; set; }
        public DateTime? ResignationDate { get; set; }
        public string Status { get; set; }
    }
    public class ImportExcelErrorListModel
    {
        public string StaffId { get; set; }
        public string ErrorMessage { get; set; }
    }
    #endregion

    #region Bulk User Account
    public class BulkUserAccountImportModel
    {
        public string StaffId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    #endregion
    public class HolidayGroupTxn1
    {
        public string HolidayName { get; set; }
        public string HolidayDateFrom { get; set; }
        public string HolidayDateTo { get; set; }
        public string DayName { get; set; }
    }
    public class EmployeeDetails
    {
        public string Name { get; set; }
        public string Department { get; set; }
    }
    public class LeaveTransactionDetails
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string Category { get; set; }
        public string CostCentre { get; set; }
        public string Volume { get; set; }
        public string LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public Decimal LeaveCount { get; set; }
        public int TransactionFlag { get; set; }
        public string TransactionType { get; set; }
        public string TransactionDate { get; set; }
        public string Narration { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
        public bool IsSystemAction { get; set; }
        public string TransactionBy { get; set; }
    }
    public class WorkFromHome
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Branch { get; set; }
        public string Category { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string CostCentre { get; set; }
        public string Volume { get; set; }
        public string WorkStation { get; set; }
        public string Duration { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Total { get; set; }
        public string Reason { get; set; }
        public string ApplicationDate { get; set; }
        public string AppliedBy { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovedBy { get; set; }
        public string ReviewalStatus { get; set; }
        public string ReviewedBy { get; set; }
        public string IsCancelled { get; set; }
        public string CancelledOn { get; set; }
        public string CancelledBy { get; set; }
    }
    public class AttendanceStatusChangeModelList
    {
        public List<AttendanceStatusChangeModel> StatusModel { get; set; }
    }
    public class AttendanceStatusChangeModel
    {
        public string Checkbox { get; set; }
        public string Staffid { get; set; }
        public string StaffName { get; set; }
        public string Department { get; set; }
        public string Date { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
    public class LeaveBalanceReport
    {
        public string StaffId { get; set; }
        public string LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string Category { get; set; }
        public decimal LapseCount { get; set; }
        public decimal Encashment { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal OpeningCredits { get; set; }
        public decimal LeaveAvailed { get; set; }
        public decimal CurrentBalance { get; set; }
    }
    public class GetCompOffLapsReport
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ReportingManager { get; set; }
        public string WorkedDate { get; set; }
        // public DateTime AvailedWorkedDate { get; set; }
        public Decimal Credit { get; set; }
        public string ExpiryDate { get; set; }
        // public bool IsAvailed { get; set; }
    }
    public class AutoLeaveDeduction
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ReportingManager { get; set; }
        public string TxnDate { get; set; }
        public string LeaveType { get; set; }
        public decimal LeaveCount { get; set; }
        public string DeductedOn { get; set; }
    }
    public class OffRoleAttendanceReport
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string CostCentre { get; set; }
        public string Category { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ReportingManager { get; set; }
        public int TotalNoOfWorkingDays { get; set; }
        public decimal NoOfDaysPresent { get; set; }
        public decimal NoOfDaysAbsent { get; set; }
        public int NoOfDaysWeeklyOff { get; set; }
        public int NoOfDaysHoliday { get; set; }
    }
    public class LeaveNightShiftAllowance
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Plant { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string ReportingManager { get; set; }
        public string AttendanceDate { get; set; }
        public string AttendanceStatus { get; set; }
        public string NightShiftCount { get; set; }
    }
    public class FormQ
    {
        public Int64 Id { get; set; }
        public string StaffId { get; set; }
        public string DOB { get; set; }
        public string DOJ { get; set; }
        public string NAME { get; set; }
        public string PLANT { get; set; }
        public string DEPARTMENT { get; set; }
        public string Designation { get; set; }
        public string Division { get; set; }
        public string Volume { get; set; }
        public string Day1 { get; set; }
        public string Day2 { get; set; }
        public string Day3 { get; set; }
        public string Day4 { get; set; }
        public string Day5 { get; set; }
        public string Day6 { get; set; }
        public string Day7 { get; set; }
        public string Day8 { get; set; }
        public string Day9 { get; set; }
        public string Day10 { get; set; }
        public string Day11 { get; set; }
        public string Day12 { get; set; }
        public string Day13 { get; set; }
        public string Day14 { get; set; }
        public string Day15 { get; set; }
        public string Day16 { get; set; }
        public string Day17 { get; set; }
        public string Day18 { get; set; }
        public string Day19 { get; set; }
        public string Day20 { get; set; }
        public string Day21 { get; set; }
        public string Day22 { get; set; }
        public string Day23 { get; set; }
        public string Day24 { get; set; }
        public string Day25 { get; set; }
        public string Day26 { get; set; }
        public string Day27 { get; set; }
        public string Day28 { get; set; }
        public string Day29 { get; set; }
        public string Day30 { get; set; }
        public string Day31 { get; set; }
        public string Day1IN { get; set; }
        public string Day2IN { get; set; }
        public string Day3IN { get; set; }
        public string Day4IN { get; set; }
        public string Day5IN { get; set; }
        public string Day6IN { get; set; }
        public string Day7IN { get; set; }
        public string Day8IN { get; set; }
        public string Day9IN { get; set; }
        public string Day10IN { get; set; }
        public string Day11IN { get; set; }
        public string Day12IN { get; set; }
        public string Day13IN { get; set; }
        public string Day14IN { get; set; }
        public string Day15IN { get; set; }
        public string Day16IN { get; set; }
        public string Day17IN { get; set; }
        public string Day18IN { get; set; }
        public string Day19IN { get; set; }
        public string Day20IN { get; set; }
        public string Day21IN { get; set; }
        public string Day22IN { get; set; }
        public string Day23IN { get; set; }
        public string Day24IN { get; set; }
        public string Day25IN { get; set; }
        public string Day26IN { get; set; }
        public string Day27IN { get; set; }
        public string Day28IN { get; set; }
        public string Day29IN { get; set; }
        public string Day30IN { get; set; }
        public string Day31IN { get; set; }
        public string Day1OUT { get; set; }
        public string Day2OUT { get; set; }
        public string Day3OUT { get; set; }
        public string Day4OUT { get; set; }
        public string Day5OUT { get; set; }
        public string Day6OUT { get; set; }
        public string Day7OUT { get; set; }
        public string Day8OUT { get; set; }
        public string Day9OUT { get; set; }
        public string Day10OUT { get; set; }
        public string Day11OUT { get; set; }
        public string Day12OUT { get; set; }
        public string Day13OUT { get; set; }
        public string Day14OUT { get; set; }
        public string Day15OUT { get; set; }
        public string Day16OUT { get; set; }
        public string Day17OUT { get; set; }
        public string Day18OUT { get; set; }
        public string Day19OUT { get; set; }
        public string Day20OUT { get; set; }
        public string Day21OUT { get; set; }
        public string Day22OUT { get; set; }
        public string Day23OUT { get; set; }
        public string Day24OUT { get; set; }
        public string Day25OUT { get; set; }
        public string Day26OUT { get; set; }
        public string Day27OUT { get; set; }
        public string Day28OUT { get; set; }
        public string Day29OUT { get; set; }
        public string Day30OUT { get; set; }
        public string Day31OUT { get; set; }
        public string Day1TotalHours { get; set; }
        public string Day2TotalHours { get; set; }
        public string Day3TotalHours { get; set; }
        public string Day4TotalHours { get; set; }
        public string Day5TotalHours { get; set; }
        public string Day6TotalHours { get; set; }
        public string Day7TotalHours { get; set; }
        public string Day8TotalHours { get; set; }
        public string Day9TotalHours { get; set; }
        public string Day10TotalHours { get; set; }
        public string Day11TotalHours { get; set; }
        public string Day12TotalHours { get; set; }
        public string Day13TotalHours { get; set; }
        public string Day14TotalHours { get; set; }
        public string Day15TotalHours { get; set; }
        public string Day16TotalHours { get; set; }
        public string Day17TotalHours { get; set; }
        public string Day18TotalHours { get; set; }
        public string Day19TotalHours { get; set; }
        public string Day20TotalHours { get; set; }
        public string Day21TotalHours { get; set; }
        public string Day22TotalHours { get; set; }
        public string Day23TotalHours { get; set; }
        public string Day24TotalHours { get; set; }
        public string Day25TotalHours { get; set; }
        public string Day26TotalHours { get; set; }
        public string Day27TotalHours { get; set; }
        public string Day28TotalHours { get; set; }
        public string Day29TotalHours { get; set; }
        public string Day30TotalHours { get; set; }
        public string Day31TotalHours { get; set; }
        public string ExemptingOrder { get; set; }
        public string WeeklyRest { get; set; }
        public string CompensatoryHolidayDate { get; set; }
        public string LostRestDays { get; set; }
        public string NoOfDaysWorked { get; set; }
        public string LeaveWithWages { get; set; }
        public string LeaveWithOutWages { get; set; }
        public string Remarks { get; set; }
        public string NOOFDAYSWORKING { get; set; }
        public string NOOFDAYSNOTWORKED { get; set; }
        public string NOOFDAYSABSENT { get; set; }
        public string NOOFDAYSLEAVE { get; set; }
        public string NSA1 { get; set; }
        public string NSA2 { get; set; }
        public string ATTINCENTIVE { get; set; }
        public string LATEPENALTY { get; set; }
        public string NOOFDAYSWO { get; set; }
        public decimal Opening_CLBalance_CurrentMonth { get; set; }
        public decimal Opening_PLBalance_CurrentMonth { get; set; }
        public decimal Opening_SLBalance_CurrentMonth { get; set; }
        public decimal SumOf_CL_Availed_CurrentMonth { get; set; }
        public decimal SumOfSL_Availed_CurrentMonth { get; set; }
        public decimal SumOfBL_Availed_CurrentMonth { get; set; }
        public decimal SumOfPL_Availed_CurrentMonth { get; set; }
        public decimal SumOfML_Availed_CurrentMonth { get; set; }
        public decimal CL_Closing_Balance { get; set; }
        public decimal PL_Closing_Balance { get; set; }
        public decimal SL_Closing_Balance { get; set; }
        public decimal SumOfUnApprovedAndApprovedLeave { get; set; }
        public string TotalHours_OT_InCurrentMonth1 { get; set; }
        public string TotalHours_OT_InCurrentMonth { get; set; }
        public string TotalHours_Workded_InMonth { get; set; }
    }
    public class ReportingDetails
    {
        public string ApprovalOwner1 { get; set; }
        public string ApprovalOwner2 { get; set; }
        public int ApproverLevel { get; set; }
    }
    public class HolidayWorkingListItem
    {
        public DateTime ApplicationDate { get; set; }
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string TransactionDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string Remarks { get; set; }
        public string IsCancelled { get; set; }
        public string ApproverStatus1 { get; set; }
        public string ApproverStatus2 { get; set; }
        public string ApprovalOwner { get; set; }
        public string Approval2Owner { get; set; }
    }


    public class LeaveDonationDetails
    {
        public string ApplicationId { get; set; }
        public string DonarStaffId { get; set; }
        public string DonarName { get; set; }
        public string ReceiverStaffId { get; set; }
        public string ReceiverName { get; set; }
        public decimal LeaveCount { get; set; }
        public string Reason { get; set; }
        public string ApprovalStatus { get; set; }
    }
    public class HolidayWorkingPendingApprovalListItem
    {
        public string HolidayWorkingId { get; set; }
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string TransactionDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string HolidayWorkingReason { get; set; }
    }
    public class ManualAttendanceStatusChange
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Division { get; set; }
        public string Department { get; set; }
        public string TxnDate { get; set; }
        public string AttStatus { get; set; }
    }
    public class ShiftExtension
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Branch { get; set; }
        public string Category { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string CostCentre { get; set; }
        public string Volume { get; set; }
        public string WorkStation { get; set; }
        public string Duration { get; set; }
        public string ShiftName { get; set; }
        public string TxnDate { get; set; }
        public string DurationOfHoursExtension { get; set; }
        public string HoursBeforeShift { get; set; }
        public string HoursAfterShift { get; set; }
        public string Reason { get; set; }
        public string ApplicationDate { get; set; }
        public string AppliedBy { get; set; }
        public string Approval1Status { get; set; }
        public string Approved1By { get; set; }
        public string Approved1On { get; set; }
        public string Approval2Status { get; set; }
        public string Approved2By { get; set; }
        public string Approved2On { get; set; }
        public string IsCancelled { get; set; }
        public string CancelledOn { get; set; }
        public string CancelledBy { get; set; }
    }
    public class UserDetails
    {
        public string LocationId { get; set; }
        public string UserName { get; set; }
        public string Approver2Id { get; set; }
        public string Gender { get; set; }
        public string DomainId { get; set; }
        public string StaffId { get; set; }
        public string StaffFullName { get; set; }
        public int SecurityGroupId { get; set; }
        public int ApprovalLevel { get; set; }

        public string CompanyId { get; set; }
        public string BranchId { get; set; }
        public string DepartmentId { get; set; }
        public string GradeName { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string DeptName { get; set; }
        public string OfficialPhone { get; set; }
        public string UserEmailId { get; set; }
        public string ReportingManagerId { get; set; }
        public string ReportingManagerName { get; set; }
        public string ReportingManagerEmailId { get; set; }
        public string ApprovalOwner2Id { get; set; }
        public string ApprovalOwner2Name { get; set; }
        public string ApprovalOwner2EmailId { get; set; }
    }
    public class ShiftExtenstionListItem
    {
        public string Id { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string Department { get; set; }
        public string TxnDate { get; set; }
        public string DurationOfHoursExtension { get; set; }
        public string HoursBeforeShift { get; set; }
        public string HoursAfterShift { get; set; }
        public string IsCancelled { get; set; }
        public string CancelledBy { get; set; }
        public string CancelledDate { get; set; }
        public string Remarks { get; set; }
        public string ApprovalOwner { get; set; }
        public string ApprovalStatus1 { get; set; }
        public string Approval2Owner { get; set; }
        public string ApprovalStatus2 { get; set; }

    }
    public class ShiftExtensionPendingApprovalListItem
    {
        public string ShiftExtensionId { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string ExtensionDate { get; set; }
        public string DurationOfHoursExtension { get; set; }
        public string HoursBeforeShift { get; set; }
        public string HoursAfterShift { get; set; }
        public string ShiftExtensionReason { get; set; }
    }
    public class ShiftchangeListItemViewModel
    {
        public DateTime Applicationdate { get; set; }
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string DeptName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string NewShiftId { get; set; }
        public string ShiftName { get; set; }
        public string Remarks { get; set; }
        public string ApproverStatus1 { get; set; }
        public string ApproverStatus2 { get; set; }
        public string IsCancelled { get; set; }
    }
    public class HolidayWorkingRequisition
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Branch { get; set; }
        public string Category { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string CostCentre { get; set; }
        public string Volume { get; set; }
        public string WorkStation { get; set; }
        public string AttendanceDate { get; set; }
        public string ShiftIn { get; set; }
        public string ShiftOut { get; set; }
        public string Reason { get; set; }
        public string ApplicationDate { get; set; }
        public string AppliedBy { get; set; }
        public string Approval1Status { get; set; }
        public string Approved1By { get; set; }
        public string Approved1On { get; set; }
        public string Approval2Status { get; set; }
        public string Approved2By { get; set; }
        public string Approved2On { get; set; }
        public string IsCancelled { get; set; }
        public string CancelledOn { get; set; }
        public string CancelledBy { get; set; }
    }
    public class WeeklyCalender
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string DEPARTMENT { get; set; }
        public string Week1TotalHours { get; set; }
        public string Week2TotalHours { get; set; }
        public string Week3TotalHours { get; set; }
        public string Week4TotalHours { get; set; }
        public string Week5TotalHours { get; set; }
        public string Week6TotalHours { get; set; }
        public string Week7TotalHours { get; set; }
        public string Week8TotalHours { get; set; }
        public string Week9TotalHours { get; set; }
        public string Week10TotalHours { get; set; }
        public string Week11TotalHours { get; set; }
        public string Week12TotalHours { get; set; }
        public string Week13TotalHours { get; set; }
        public string Week14TotalHours { get; set; }
        public string Week15TotalHours { get; set; }
        public string Week16TotalHours { get; set; }
        public string Week17TotalHours { get; set; }
        public string Week18TotalHours { get; set; }
        public string Week19TotalHours { get; set; }
        public string Week20TotalHours { get; set; }
        public string Week21TotalHours { get; set; }
        public string Week22TotalHours { get; set; }
        public string Week23TotalHours { get; set; }
        public string Week24TotalHours { get; set; }
        public string Week25TotalHours { get; set; }
        public string Week26TotalHours { get; set; }
        public string Week27TotalHours { get; set; }
        public string Week28TotalHours { get; set; }
        public string Week29TotalHours { get; set; }
        public string Week30TotalHours { get; set; }
        public string Week31TotalHours { get; set; }
        public string Week32TotalHours { get; set; }
        public string Week33TotalHours { get; set; }
        public string Week34TotalHours { get; set; }
        public string Week35TotalHours { get; set; }
        public string Week36TotalHours { get; set; }
        public string Week37TotalHours { get; set; }
        public string Week38TotalHours { get; set; }
        public string Week39TotalHours { get; set; }
        public string Week40TotalHours { get; set; }
        public string Week41TotalHours { get; set; }
        public string Week42TotalHours { get; set; }
        public string Week43TotalHours { get; set; }
        public string Week44TotalHours { get; set; }
        public string Week45TotalHours { get; set; }
        public string Week46TotalHours { get; set; }
        public string Week47TotalHours { get; set; }
        public string Week48TotalHours { get; set; }
        public string Week49TotalHours { get; set; }
        public string Week50TotalHours { get; set; }
        public string Week51TotalHours { get; set; }
        public string Week52TotalHours { get; set; }
        public string Week1ColorCode { get; set; }
        public string Week2ColorCode { get; set; }
        public string Week3ColorCode { get; set; }
        public string Week4ColorCode { get; set; }
        public string Week5ColorCode { get; set; }
        public string Week6ColorCode { get; set; }
        public string Week7ColorCode { get; set; }
        public string Week8ColorCode { get; set; }
        public string Week9ColorCode { get; set; }
        public string Week10ColorCode { get; set; }
        public string Week11ColorCode { get; set; }
        public string Week12ColorCode { get; set; }
        public string Week13ColorCode { get; set; }
        public string Week14ColorCode { get; set; }
        public string Week15ColorCode { get; set; }
        public string Week16ColorCode { get; set; }
        public string Week17ColorCode { get; set; }
        public string Week18ColorCode { get; set; }
        public string Week19ColorCode { get; set; }
        public string Week20ColorCode { get; set; }
        public string Week21ColorCode { get; set; }
        public string Week22ColorCode { get; set; }
        public string Week23ColorCode { get; set; }
        public string Week24ColorCode { get; set; }
        public string Week25ColorCode { get; set; }
        public string Week26ColorCode { get; set; }
        public string Week27ColorCode { get; set; }
        public string Week28ColorCode { get; set; }
        public string Week29ColorCode { get; set; }
        public string Week30ColorCode { get; set; }
        public string Week31ColorCode { get; set; }
        public string Week32ColorCode { get; set; }
        public string Week33ColorCode { get; set; }
        public string Week34ColorCode { get; set; }
        public string Week35ColorCode { get; set; }
        public string Week36ColorCode { get; set; }
        public string Week37ColorCode { get; set; }
        public string Week38ColorCode { get; set; }
        public string Week39ColorCode { get; set; }
        public string Week40ColorCode { get; set; }
        public string Week41ColorCode { get; set; }
        public string Week42ColorCode { get; set; }
        public string Week43ColorCode { get; set; }
        public string Week44ColorCode { get; set; }
        public string Week45ColorCode { get; set; }
        public string Week46ColorCode { get; set; }
        public string Week47ColorCode { get; set; }
        public string Week48ColorCode { get; set; }
        public string Week49ColorCode { get; set; }
        public string Week50ColorCode { get; set; }
        public string Week51ColorCode { get; set; }
        public string Week52ColorCode { get; set; }
    }
    public class AutoLeaveDeductionDryRun
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string ShiftInDate { get; set; }
        public string AttendanceStatus { get; set; }
        public string FHStatus { get; set; }
        public string SHStatus { get; set; }
        public decimal AbsentCount { get; set; }
    }
    public class ResetPasswordDetails
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string DeptName { get; set; }
        public string DesignationName { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string LoggedInStaffId { get; set; }
    }
    public class AspnetuserFilterDetails
    {
        public string DepartmentId { get; set; }
        public string DesignationId { get; set; }
        public string UserId { get; set; }
        public string StaffId { get; set; }
        public List<DropDownStrModel> Department { get; set; }
        public List<DropDownStrModel> Designation { get; set; }
    }
}
