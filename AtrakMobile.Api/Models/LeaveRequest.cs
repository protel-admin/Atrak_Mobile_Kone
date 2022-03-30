using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Models
{
    public class LeaveRequestDto
    {

        /* "LeaveTypeId": null,
        "LeaveStartDurationId": null,
        "LeaveEndDurationId": null,
        "ApprovalOwnerId": "00112",
        "ReviewerOwnerId": null,
         */
        public string LeaveApplicationId { get; set; }
        public string StaffId { get; set; }
        //public string LeaveTypeId { get; set; }
        // public string LeaveStartDurationId { get; set; }
        //public string LeaveEndDurationId { get; set; }
        // public string ApprovalOwnerId { get; set; }
        //public string ReviewerOwnerId { get; set; }
        public decimal TotalDays { get; set; }
        public string StaffName { get; set; }
        public string LeaveTypeName { get; set; }
        public string StartDate { get; set; }
        public string LeaveStartDuration { get; set; }
        public string EndDate { get; set; }
        public string LeaveEndDuration { get; set; }
        public string Remarks { get; set; }
        public string ContactNbr { get; set; }
        public string ApprovalStatus { get; set; }

    }
    public class OTRequestDto
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


    public class ODRequestDto
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


    public class MaintenanceOffRequestDto
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

    public class LaterOffRequestDto
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


    public class COffRequestDto
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

    public class PermissionRequestDto
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


              
    public class ShiftChangeRequestDto
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

    public class ManualPunchRequestDto
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
    /*

     public class LeaveRequest_Dto
     {
         public string Id { get; set; }
         public string StaffId { get; set; }
         public string StaffName { get; set; }
         public string StaffEmailId { get; set; }
         public string Remarks { get; set; }
         public string StartDate { get; set; }
         public string EndDate { get; set; }
         public string LeaveStartDurationId { get; set; }
         public string LeaveEndDurationId { get; set; }
         public string LeaveStartDuration { get; set; }
         public string LeaveEndDuration { get; set; }
         public decimal TotalDays { get; set; }
         public string LeaveTypeName { get; set; }
         public string LeaveTypeId { get; set; }

         public string ContactNbr { get; set; }
         public string DateApplied { get; set; }
         public string ApprovalStatus1 { get; set; }
         public string ApprovalStatus2 { get; set; }
         public string ApprovalOwnerId { get; set; }
         public string ReviewerOwnerId { get; set; }
         public string ReviewerOwnerName { get; set; }
         public string Approval1OwnerName { get; set; }
         public string Approval2OwnerName { get; set; }
         public string ReasonId { get; set; }
         public string Reason { get; set; }
         public string ReportingManagerEmailId { get; set; }
         public string ReportingManagerId { get; set; }
         public string ReportingManagerName { get; set; }
         public string IsCancelled { get; set; }
         public bool IsReviewerCancelled { get; set; }
         public bool IsApproverCancelled { get; set; }
         public string AlternateStaffId { get; set; }
         public string AlternateStaffName { get; set; }
         public byte[] ProofCopy { get; set; }
         public string FileExtenstion { get; set; }

     }      
    */

    public class COffAvaillingDto
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
    public class RALeaveApplication_reference
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string AppliedBy { get; set; }
        public string Remarks { get; set; }
        public string FromDuration { get; set; }
        public string StartDate { get; set; }
        public string ToDuration { get; set; }
        public string EndDate { get; set; }
        public string TotalDays { get; set; }
        public string Type { get; set; }
        public string ApproverStatus { get; set; }
        public string ReviewerStatus { get; set; }
        public string Status { get; set; }
        public string ApprovalOwner { get; set; }
        public string  ReviewerOwner { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsReviewerCancelled { get; set; }
        public bool IsApproverCancelled { get; set; }


    }

    public class LeaveInitDto
    {
        public List<Duration> DurationList { get; set; }
        public List<LeaveType> LeaveTypes { get; set; }
       // public LeaveRequestDto LeaveRequestEmptyDto { get; set; }
        public List<AlternativeStaff> AlternativeStaffs { get; set; }

    }

    public class AlternativeStaff
    {
        public string StaffId { get; set; }
        public string Name { get; set; }
    }

    public class Duration
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class LeaveType
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }

    public class LeaveBalanceDto
    {
        public string LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public Decimal LeaveBalance { get; set; }
        public bool IsCommon { get; set; }
        public bool IsPermission { get; set; }
    }

    public class CancelRequestDto
    {
        public string RequestId { get; set; }
        public string StaffId { get; set; }
        public string ReviewerId { get; set; }
        public string ApproverId { get; set; }
    }
    public class PunchOfTheDay
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
}