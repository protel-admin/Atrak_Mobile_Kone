using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Attendance.Model
{

public class MobilePunch
    {
        public string StaffId { get; set; }
        public string PunchMode { get; set; }
        public DateTime PunchDateTime { get; set; }
        public decimal Longitude { get; set; }
        public decimal Lattitude { get; set; }
        public decimal Radius { get; set; }

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
        public string ApproverStatus { get; set; }
        public string ReviewerStatus { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsReviewerCancelled { get; set; }
        public bool IsApproverCancelled { get; set; }
        public string RequestApplicationType { get; set; }
        public DateTime ApplicationDate { get; set; }
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
        public string Workingpattern { get; set; }
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
        public string FHStatus { get; set; }
        public string SHStatus { get; set; }

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

    public class ApplicationApprovalpendingCountModel
    {
        public string ApplicationType { get; set; }
        public int ApplicationCount { get; set; }
    }



    public class HeadCountViewModel
    {


        public string RMId { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ShiftId { get; set; }
        public string ShifName { get; set; }
        public int TotalHeadCount { get; set; }
        public int ViolationCount { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public decimal PresentPercentage { get; set; }
        public decimal AbsentPercentage { get; set; }
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
        public string ReviewedOnDate { get; set; }
        public string ApprovedOnTime { get; set; }
        public string Remarks { get; set; }
        public string ApprovalOwnerName { get; set; }
        public string ReviewerOwnerName { get; set; }
        public int IsDocumentAvailable { get; set; }
        public string ParentType { get; set; }
    }

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




























   


    public class DocumentData
    {
        public string LeaveApplicationId { get; set; }
        public byte[] FileContent { get; set; }
        public string TypeOfDocument { get; set; }
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

    public class ShiftChangeRequest
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
    public class GetShiftChangeApplication
    {

        public string Id { get; set; }
        public string StaffId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string NewShiftId { get; set; }
    }

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
        public string ReviewerstatusName { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ReviewerStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ReviewerName { get; set; }
        public string ApplicationApprovalId { get; set; }
        public string ApprovedOnDate { get; set; }
        public string ReviewedOnDate { get; set; }
        public string ApprovedOnTime { get; set; }
        public string Comment { get; set; }
        public string ApprovalOwner { get; set; }
        public string ParentType { get; set; }
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
        public string StaffId { get; set; }
        public string CardCode { get; set; }
        public string FirstName { get; set; }
        public string ShiftName { get; set; }
        public string DeptName { get; set; }
        public string Location { get; set; }
        public string GradeName { get; set; }
        public string DivisionName { get; set; }
        public string VolumeName { get; set; }
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
        public string SystemMessage { get; set; }
        public string IsDisputed { get; set; }
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
    }

    public class Form25
    {
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

    }

    public class RoleList
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ManualPunchApprovalList
    {
        public string ManualPunchId { get; set; }
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string InDateTime { get; set; }
        public string OutDateTime { get; set; }
        public string PunchType { get; set; }
        public string ManualPunchReason { get; set; }
        public string FirstName { get; set; }
        public string Location { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string ReviewedBy { get; set; }
        public string ReviewedOn { get; set; }
        public int ApprovalStatusId { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApplicationApprovalId { get; set; }
        public string ApprovedOnDate { get; set; }
        public string ApprovedOnTime { get; set; }
        public string Comment { get; set; }
        public string ApprovalOwner { get; set; }
        public int ReviewalStatusId { get; set; }
        public string ReviewalStatus { get; set; }
        public string ReviewerStaffName { get; set; }
        public string ReviewedOnDate { get; set; }
        public string ReviewedOnTime { get; set; }
        public string ReviewerOwner { get; set; }
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
        public string ReviewerStatusName { get; set; }
        public string ISCANCELLED { get; set; }
        public string LeaveApplicationReason { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApprovalDate { get; set; }
        public string TxnDate { get; set; }
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
        public string TxnDate { get; set; }
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
        public string IsDisputed { get; set; }
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
        public string DomainId { get; set; }
        public string StaffId { get; set; }
        public string StaffFullName { get; set; }
        public int ApprovalLevel { get; set; }
        public int SecurityGroupId { get; set; }
        public string ApproverId { get; set; }
        public string ApproverName { get; set; }
        public string CompanyId { get; set; }
        public string BranchId { get; set; }
        public string DepartmentId { get; set; }
        public string GradeName { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string DeptName { get; set; }
        public string OfficialPhone { get; set; }
        public string UserEmailId { get; set; }
        public string ApproverEmailId { get; set; }
        public string ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        public string ReviewerEmailId { get; set; }
        public string LocationId { get; set; }

        public string ReportingManagerId { get; set; }
        public string ReportingManagerName { get; set; }
        public string Approver2Id { get; set; }
        public string ReportingManagerEmailId { get; set; }
        public string IsMobileApplicationEligible { get; set; }
    }

    public class ContinuousAbsent
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string DivisionName { get; set; }
        public int NoOfDays { get; set; }
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

    public class TodaysPunchesDashBoard_DAIMLER
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
        public string Grade { get; set; }
        public string Branch { get; set; }

        public string Division { get; set; }
        public string Indate { get; set; }
        public string InTime { get; set; }

    }

    public class IndividualLeaveCreditDebit
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
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
        public string GRADE { get; set; }
        public string TXNDATE { get; set; }
        public string PLANNEDSHIFT { get; set; }
        public string ACTUALSHIFT { get; set; }
        public string ACTUALINTIME { get; set; }

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
        public string StaffName { get; set; }
        public string Plant { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string ReportingHead { get; set; }
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
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
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
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
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
        public string LocationId { get; set; }
        public string ShiftId { get; set; }
        public string DepartmentName { get; set; }
        public string LocationName { get; set; }
        public string DesignationName { get; set; }
        public string ShiftName { get; set; }
        public int HeadCount { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int GroupNo { get; set; }
        public int LocSeq { get; set; }
        public int DeptSeq { get; set; }
        public int DesgSeq { get; set; }
        public int GradeSeq { get; set; }
        public int Seq { get; set; }
    }

    public class ClassesToSave
    {
        public RequestApplication RA { get; set; }
        public ApplicationApproval AA { get; set; }
        public List<EmailSendLog> ESL { get; set; }
        public EmployeeLeaveAccount ELA { get; set; }
        public AlternativePersonAssign APA { get; set; }
        public Testing Test { get; set; }
        public OTApplication OTA { get; set; }
        public AttendanceControlTable ACT { get; set; }
        public AttendanceData AD { get; set; }
        public string loggedstaffid { get; set; }
    }

    public class ClassesToSaveforOT
    {
        public ApplicationApproval AA { get; set; }
        public List<EmailSendLog> ESL { get; set; }
        public EmployeeLeaveAccount ELA { get; set; }
        public OTApplication OTP { get; set; }
        public string loggedstaffid { get; set; }
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

    public class Remainder
    {
        public string Lastattendanceprocessed { get; set; }
        public Int32 ShiftRoisteringAlertCount { get; set; }
        public string LasttransactionSyncDate { get; set; }
        public Int32 PlannedLeaveCount { get; set; }
        public List<ShiftRoisteringAlert> ShiftPlanChangeList { get; set; }
        public int HeadCount { get; set; }
        public int BirthdayAlert { get; set; }

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

    public class EmployeeHistory
    {
        public int ID { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string ChangeLog { get; set; }
        public string Values { get; set; }
        public string Before { get; set; }
        public string ChangedTo { get; set; }
        public string ActionType { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string TableName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
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

    public class UserList
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string DeptName { get; set; }
        public string REPMGRFIRSTNAME { get; set; }
    }

    public class PWandMsg
    {
        public string ErrorMsgType { get; set; }
        public string Message { get; set; }
        public string Password { get; set; }
    }

    public class DropDownListString
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }

    public class MonthlyLeavePlanner
    {
        //public Int64 Id { get; set; }
        public string DeptId { get; set; }
        public string GradeId { get; set; }
        public string EmpCode { get; set; }
        public string DepartmentName { get; set; }
        public string GradeName { get; set; }
        public string EmpName { get; set; }
        public int? Day1 { get; set; }
        public int? Day2 { get; set; }
        public int? Day3 { get; set; }
        public int? Day4 { get; set; }
        public int? Day5 { get; set; }
        public int? Day6 { get; set; }
        public int? Day7 { get; set; }
        public int? Day8 { get; set; }
        public int? Day9 { get; set; }
        public int? Day10 { get; set; }
        public int? Day11 { get; set; }
        public int? Day12 { get; set; }
        public int? Day13 { get; set; }
        public int? Day14 { get; set; }
        public int? Day15 { get; set; }
        public int? Day16 { get; set; }
        public int? Day17 { get; set; }
        public int? Day18 { get; set; }
        public int? Day19 { get; set; }
        public int? Day20 { get; set; }
        public int? Day21 { get; set; }
        public int? Day22 { get; set; }
        public int? Day23 { get; set; }
        public int? Day24 { get; set; }
        public int? Day25 { get; set; }
        public int? Day26 { get; set; }
        public int? Day27 { get; set; }
        public int? Day28 { get; set; }
        public int? Day29 { get; set; }
        public int? Day30 { get; set; }
        public int? Day31 { get; set; }
        public int DepSeq { get; set; }
        public int GradeSeq { get; set; }
        public int Seq { get; set; }
    }

    public class ShiftList1
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool isactive { get; set; }
    }

    public class LeaveApplicationListNew
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public string LeaveStartDate { get; set; }
        public string LeaveStartDurationId { get; set; }
        public string LeaveStartDurationName { get; set; }
        public string LeaveEndDate { get; set; }
        public string LeaveEndDurationId { get; set; }
        public string LeaveEndDurationName { get; set; }
        public string Remarks { get; set; }
        public string LeaveApplicationReason { get; set; }
        public string ContactNumber { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ReviewerStatusName { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApprovedOn { get; set; }
        public string ApprovalOwner { get; set; }
        public string ReviewerOwner { get; set; }
        public string ReviewerOwnerName { get; set; }
        public string ApprovalOwnerName { get; set; }
        public string IsUserCancelled { get; set; }
        public string ApplicationDate { get; set; }
        public string TotalDays { get; set; }
        public string ReviewedOn { get; set; }
        public string IsReviewerCancelled { get; set; }
        public string IsApproverCancelled { get; set; }
    }

    public class POApplicationList
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string FromTime { get; set; }
        public string TimeTo { get; set; }
        public string TotalHours { get; set; }
        public string Name { get; set; }
        public string PermissionDate { get; set; }
        public string PermissionOffReason { get; set; }
        public string ContactNumber { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApprovedOn { get; set; }
        public string ApprovalOwner { get; set; }
        public string IsUserCancelled { get; set; }
        public string ReviewerStatusName { get; set; }
        public string ReviewedBy { get; set; }
        public string ReviewedOn { get; set; }
        public string ReviewerOwner { get; set; }
        public string ReviewerOwnerName { get; set; }
        public string IsReviewerCancelled { get; set; }
        public string IsApproverCancelled { get; set; }

    }


    public class PermissionOffReport
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string FromTime { get; set; }
        public string TimeTo { get; set; }
        public string TotalHours { get; set; }
        public string Name { get; set; }
        public string PermissionDate { get; set; }
        public string PermissionOffReason { get; set; }
        public string ContactNumber { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalStaffName { get; set; }
        public string ApprovedOn { get; set; }
        public string ApprovalOwner { get; set; }
        public string IsUserCancelled { get; set; }
        public string ReviewerStatusName { get; set; }
        public string ReviewedBy { get; set; }
        public string ReviewedOn { get; set; }
        public string ReviewerOwner { get; set; }
        public string ReviewerOwnerName { get; set; }

    }
    public class ODApplicationListNew
    {
        public string StaffId { get; set; }
        public string NAME { get; set; }
        public string ODDuration { get; set; }
        public string ODFromTime { get; set; }
        public string ODToTime { get; set; }
        public string ODFromDate { get; set; }
        public string ODToDate { get; set; }
        public string OD { get; set; }
        public string Reason { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalStaffId { get; set; }
        public string APPROVALSTAFFNAME { get; set; }
        public string ApprovedOn { get; set; }
        public string ApprovalOwner { get; set; }
        public string ApprovedBy { get; set; }
        public string IsUserCancelled { get; set; }
        public string ReviewerstatusId { get; set; }
        public string ReviewalStatus { get; set; }
        public string ReviewedBy { get; set; }
        public string ReviewedOn { get; set; }
        public string ReviewerOwner { get; set; }
        public string ReviewerOwnerName { get; set; }
        public string IsReviewerCancelled { get; set; }
        public string IsApproverCancelled { get; set; }
    }


    public class StaffExport
    {
        public string StaffId { get; set; }
        public string StatusName { get; set; }
        public string CardCode { get; set; }
        public string FirstName { get; set; }
        public string ShortName { get; set; }
        public string Gender { get; set; }
        public string DateOfJoining { get; set; }
        public string DateOfResignation { get; set; }
        public string OfficialPhone { get; set; }
        public string OfficalEmail { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string DeptName { get; set; }
        public string DivisionName { get; set; }
        public string VolumeName { get; set; }
        public string DesignationName { get; set; }
        public string GradeName { get; set; }
        public string CategoryName { get; set; }
        public string CostCentreName { get; set; }
        public string LocationName { get; set; }
        public string SecurityGroupName { get; set; }
        public string LeaveGroupName { get; set; }
        public string WeeklyOffName { get; set; }
        public string WeeklyOffIsActive { get; set; }
        public string HolidayGroupName { get; set; }
        public string HolidayGroupIsActive { get; set; }
        public string BloodGroup { get; set; }
        public string MaritalStatus { get; set; }
        public string HomeAddress { get; set; }
        public string HomeLocation { get; set; }
        public string HomeCity { get; set; }
        public string DateOfBirth { get; set; }
        public string MarriageDate { get; set; }
        public string REPORTINGMGRID { get; set; }
        public string REPMGRFIRSTNAME { get; set; }
        public string MedicalClaimNumber { get; set; }
        public string Reviewer { get; set; }
        public string OTReviewer { get; set; }
        public string OTReportingManager { get; set; }
        public string ReviewerName { get; set; }
        public string OTReviewerName { get; set; }
        public string OTReportingManagerName { get; set; }
    }

    public class BulkShiftImportModel
    {
        public string StaffId { get; set; }
        public string ShiftName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

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

    public class Emailforwordconfigmodel
    {
        public int Id { get; set; }
        public string ScreenID { get; set; }
        public string Fromaddress { get; set; }
        public string LocationId { get; set; }
        public string Toaddress { get; set; }
        public string CCaddress { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class Getemailforwaordconfigmodel
    {
        public int ID { get; set; }
        public string ScreenName { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string ScreenId { get; set; }
        public string Fromadd { get; set; }
        public string Toadd { get; set; }
        public string CCadd { get; set; }
    }
    public class EmployeeImportModel
    {
        public bool IsValid { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string Gender { get; set; }
        public string LocationId { get; set; }
        public string BranchId { get; set; }
        public string DepartmentId { get; set; }
        public string DesignationId { get; set; }
        public string LeaveGroupId { get; set; }
        public int PolicyId { get; set; }
        public int WorkingDayPatternId { get; set; }
        public string CategoryId { get; set; }
        public string CostCentreId { get; set; }
        public int SecurityGroupId { get; set; }
        public DateTime? DOJ { get; set; }
        public string OfficialEmail { get; set; }
        public string ReportingManager { get; set; }
        public string Reviewer { get; set; }
        public string ShiftId { get; set; }
        public int BloodGroupId { get; set; }
        public int HolidayZoneId { get; set; }
        public int MaritalStatusId { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? MarriageDate { get; set; }
        public int ApproverLevel { get; set; }
    }
    public class InvalidDataExportXLModel
    {
        public string ErrorMessage { get; set; }
    }

    public class DrawFORM1
    {
        public int Id { get; set; }
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string ApprenticeAct { get; set; }
        public string DOJ { get; set; }
        public string Complete480Days { get; set; }
        public string DOJWords { get; set; }
        public string Complete480DaysWords { get; set; }
        public string Remarks { get; set; }
        public string Signature { get; set; }
    }
    public class DrawFORM10
    {
        public int Id { get; set; }
        public string StaffId { get; set; }
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
    public class DrawFORM12
    {
        public int Id { get; set; }
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Designation { get; set; }
        public string LetterOfGroup { get; set; }
        public string NoOfRelays { get; set; }
        public string NoOfCertificate { get; set; }
        public string TokenNo { get; set; }
        public string Remarks { get; set; }
    }
    public class FormVI
    {
        public string StaffId { get; set; }
        public string NAME { get; set; }
        public string FUNCTION { get; set; }
        public string SUBFUNCTION { get; set; }
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
        public string Day1DATE { get; set; }
        public string Day2DATE { get; set; }
        public string Day3DATE { get; set; }
        public string Day4DATE { get; set; }
        public string Day5DATE { get; set; }
        public string Day6DATE { get; set; }
        public string Day7DATE { get; set; }
        public string Day8DATE { get; set; }
        public string Day9DATE { get; set; }
        public string Day10DATE { get; set; }
        public string Day11DATE { get; set; }
        public string Day12DATE { get; set; }
    }
    public class FormQ
    {
        public string StaffId { get; set; }
        public string NAME { get; set; }
        public string DOJ { get; set; }
        public string DOB { get; set; }
        public string PLANT { get; set; }
        public string DEPARTMENT { get; set; }
        public string DESIGNATION { get; set; }
        public string REPORTINGMANAGER { get; set; }
        public string Division { get; set; }
        public string Volume { get; set; }
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
        public string TotalHours_Workded_InMonth { get; set; }
        public string TotalHours_OT_InCurrentMonth { get; set; }
        public decimal Opening_CLBalance_CurrentMonth { get; set; }
        public decimal Opening_ELBalance_CurrentMonth { get; set; }
        public decimal Opening_SLBalance_CurrentMonth { get; set; }
        public decimal SumOf_CL_Availed_CurrentMonth { get; set; }
        public decimal SumOfSL_Availed_CurrentMonth { get; set; }
        public decimal SumOfEL_Availed_CurrentMonth { get; set; }
        public decimal SumOfML_Availed_CurrentMonth { get; set; }
        public decimal CL_Closing_Balance { get; set; }
        public decimal EL_Closing_Balance { get; set; }
        public decimal SL_Closing_Balance { get; set; }
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
    public class LeaveSummary
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string PFNo { get; set; }
        public decimal OB_CL { get; set; }
        public decimal OB_SL { get; set; }
        public decimal OB_PL { get; set; }
        public decimal AV_CL { get; set; }
        public decimal AV_SL { get; set; }
        public decimal AV_PL { get; set; }
        public decimal BAL_CL { get; set; }
        public decimal BAL_SL { get; set; }
        public decimal BAL_PL { get; set; }
        public decimal CR_CL { get; set; }
        public decimal CR_SL { get; set; }
        public decimal CR_PL { get; set; }
        public decimal DR_CL { get; set; }
        public decimal DR_SL { get; set; }
        public decimal DR_PL { get; set; }
    }
    public class ShiftExtension
    {
        public string StaffId { get; set; }
        public string ShiftId { get; set; }
        public DateTime Date { get; set; }
        public DateTime ShiftOutTime { get; set; }
        public DateTime OutPunch { get; set; }
        public DateTime ShiftOutTime4Hours { get; set; }
        public DateTime ShiftOutTime12Hours { get; set; }
        public DateTime OverStayHours { get; set; }
    }
    public class CandAReport
    {
        public string StaffId { get; set; }
        public string ShiftName { get; set; }
        // public DateTime Date                { get; set; }
        public string ShiftOutTime { get; set; }
        public string OutPunch { get; set; }
        //public string ShiftOutTime4Hours  { get; set; }
        //public string ShiftOutTime12Hours { get; set; }
        public string OverStayHours { get; set; }
    }
    public class AttendanceDataViewNewModel
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string TxnDate { get; set; }
        public string ShiftName { get; set; }
        public string ActualIn { get; set; }
        public string ActualOut { get; set; }
        public string TotalHoursWorked { get; set; }
        public string Txn_DayName { get; set; }
        public string AccountedOTTime { get; set; }
        public string ActualOTTime { get; set; }
        public string OTDuration { get; set; }
        public bool IsOTExceed { get; set; }
        public string QuarterStart { get; set; }
        public string QuarterEnd { get; set; }
        public bool IsOTTimeProcessed { get; set; }
        public string TotalOTInQuarter { get; set; }
    }
    public class BusinessTravelReportModel
    {
        public string StaffId { get; set; }
        public string ApplicantName { get; set; }
        public string Duration { get; set; }
        public string FromDate { get; set; }
        public string FromTime { get; set; }
        public string ToDate { get; set; }
        public string ToTime { get; set; }
        public string TotalDays { get; set; }
        public string Reason { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ReviewerStatusName { get; set; }
        public string ApprovalStaffId { get; set; }
        public string ApprovalOwnerName { get; set; }
        public string ReviewerStaffId { get; set; }
        public string ReviewerOwnerName { get; set; }
        public string ApprovedOn { get; set; }
        public string ReviewdOn { get; set; }
        public string Comment { get; set; }
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
    #endregion
    public class CompOffRequistionModel
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
        public string WorkedDate { get; set; }
        public string FromDate { get; set; }
        public string FromDuration { get; set; }
        public string ToDate { get; set; }
        public string ToDuration { get; set; }
        public string TotalDays { get; set; }
        public string Reason { get; set; }
        public string IsCancelled { get; set; }
        public string CancelledOn { get; set; }
        public string CancelledBy { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalStatusId { get; set; }
        public string ReviewalStatusId { get; set; }
        public string ReviewalStatus { get; set; }
        public string ApplicationDate { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string ReviewedBy { get; set; }
        public string ReviewedOn { get; set; }
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
    public class RACoffAvailingRequestApplication
    {
        public DateTime? ExpiryDate { get; set; }
        public string Id { get; set; }
        public string StaffName { get; set; }
        public string AppliedBy { get; set; }
        public string Remarks { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string Approval1Owner { get; set; }
        public string Approval2Owner { get; set; }

        public string StaffId { get; set; }
        public string COffId { get; set; }
        public string Name { get; set; }
        public string LeaveTypeId { get; set; }
        public string LeaveShortName { get; set; }
        public string Workeddate { get; set; }
        public string AvailFromDate { get; set; }
        public string AvailToDate { get; set; }
        public string COffReason { get; set; }
        public string TotalDays { get; set; }
        public string IsCancelled { get; set; }
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

    public class LeaveDeduction
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public string LeaveType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string LeaveCount { get; set; }
        public string TransactionType { get; set; }
        public string TransactionBy { get; set; }
        public string LeaveCreditDebitReason { get; set; }
    }
}