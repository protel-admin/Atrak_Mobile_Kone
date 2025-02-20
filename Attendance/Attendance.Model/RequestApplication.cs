﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Attendance.Model
{
    public class RequestApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(20)]
        public string Id { get; set; }
        //
        [Required]
        [MaxLength(50)]
        public string StaffId { get; set; }
        //
        [MaxLength(10)]
        public string LeaveTypeId { get; set; }
        //
        public int LeaveStartDurationId { get; set; }
        //
        public DateTime StartDate { get; set; }
        //
        public DateTime EndDate { get; set; }
        //
        public int LeaveEndDurationId { get; set; }
        //
        public decimal TotalDays { get; set; }
        //
        [MaxLength(20)]
        public string PermissionType { get; set; }// Shift Start, Shift End, In Between Shift
        //
        [MaxLength(20)]
        public string OTRange { get; set; }//Before Shift Start , After Shift End.
        //
        [MaxLength(20)]
        public string ODDuration { get; set; }// SINGLE DAY, MULTIPLE DAYS
        //
        [MaxLength(10)]
        public string NewShiftId { get; set; }
        //
        public int RHId { get; set; }
        //
        public DateTime? TotalHours { get; set; }//This field can be used for Permissions and OT time
        //
        [MaxLength(200)]
        public string Remarks { get; set; }
        //
        public int ReasonId { get; set; }
        //
        [MaxLength(20)]
        public string ContactNumber { get; set; }
        //
        [MaxLength(5)]
        public string PunchType { get; set; }// IN, OUT OR INOUT
        //
        [Required]
        public DateTime? ApplicationDate { get; set; }
        //
        [Required]
        [MaxLength(50)]
        public string AppliedBy { get; set; }
        //
        [Required]
        public bool IsCancelled { get; set; }
        //
        public DateTime? CancelledDate { get; set; }
        //
        [MaxLength(50)]
        public string CancelledBy { get; set; }
        //
        [Required]
        public bool IsApproved { get; set; }
        //

        //[Required]
        //public bool IsReviewed { get; set; }
        [Required]
        public bool IsRejected { get; set; }

        //[DefaultValue(0)]
        //public bool IsApproved2 { get; set; }
        ////
        //[Required]
        //public bool IsApproved2Cancelled { get; set; }
        ////
        //public DateTime? Approved2CancelledDate { get; set; }
        ////
        //[MaxLength(10)]
        //public string Approved2CancelledBy { get; set; }

        //
        [Required]
        [MaxLength(5)]
        public string RequestApplicationType { get; set; }
        //
        [Required]
        public bool IsCancelApprovalRequired { get; set; }
        //
        [Required]
        public bool IsCancelApproved { get; set; }
        //
        [Required]
        public bool IsCancelRejected { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public DateTime? WorkedDate { get; set; }

        public string ReceiverStaffId { get; set; }

        [MaxLength(20)]
        public string ShiftExtensionType { get; set; }

        [MaxLength(30)]
        public string DurationOfHoursExtension { get; set; }

        public DateTime? HoursBeforeShift { get; set; }

        public DateTime? HoursAfterShift { get; set; }
        //[Required]
        //public bool IsApproverCancelled { get; set; }
        ////
        //public DateTime? ApproverCancelledDate { get; set; }
        ////
        //[MaxLength(10)]
        //public string ApproverCancelledBy { get; set; }

        //[Required]
        //public bool IsReviewerCancelled { get; set; }
        ////
        //public DateTime? ReviewerCancelledDate { get; set; }
        ////
        //[MaxLength(10)]
        //public string ReviewerCancelledBy { get; set; }

    }

    public class RALeaveApplication
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
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string IsCancelled { get; set; }
        public string Approval1Owner { get; set; }
        public string Approval2Owner { get; set; }
        public string ApproverStatus { get; set; }
        public string ReviewerStatus { get; set; }
        public string Status { get; set; }
        public string ApprovalOwner { get; set; }
        public string ReviewerOwner { get; set; }
        // public bool IsCancelled { get; set; }
        public bool IsReviewerCancelled { get; set; }
        public bool IsApproverCancelled { get; set; }
    }

    public class RAPermissionApplication
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string AppliedBy { get; set; }
        public string Remarks { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TotalHours { get; set; }
        public string Type { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string IsCancelled { get; set; }
        public string Approval1Owner { get; set; }
        public string Approval2Owner { get; set; }
        public string ApproverStatus { get; set; }
        public string ReviewerStatus { get; set; }
        public string Status { get; set; }
        public string ApprovalOwner { get; set; }
        public string ReviewerOwner { get; set; }
        //public bool IsCancelled { get; set; }
        public bool IsReviewerCancelled { get; set; }
        public bool IsApproverCancelled { get; set; }
    }

    public class RAManualPunchApplication
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string AppliedBy { get; set; }
        public string Remarks { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string Type { get; set; }
        public string Approval1StatusName { get; set; }
        public string Approval2statusName { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string IsCancelled { get; set; }
        public string Approval1Owner { get; set; }
        public string Approval2Owner { get; set; }

        public string Status { get; set; }
        public string ApproverStatus { get; set; }
        public string ReviewerStatus { get; set; }
        public string ApprovalOwner { get; set; }
        public string ReviewerOwner { get; set; }
        //public bool IsCancelled { get; set; }
        public bool IsReviewerCancelled { get; set; }
        public bool IsApproverCancelled { get; set; }

    }

    public class RACoffRequestApplication
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string AppliedBy { get; set; }
        public string Remarks { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        //public decimal TotalDays { get; set; }
        public string TotalDays { get; set; }
        public string Status { get; set; }
    }

    public class RAODRequestApplication
    {
        public string ApplicationType { get; set; }
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string AppliedBy { get; set; }
        public string Remarks { get; set; }
        public string ODDuration { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TotalDays { get; set; }
        public string TotalHours { get; set; }
        public string Type { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        //Rajesh Nov 29 . changed string to bool
        public bool IsCancelled { get; set; }
        public string Approval1Owner { get; set; }
        public string Approval2Owner { get; set; }
        public string ParentType { get; set; }
        public string Status { get; set; }
        public string ApproverStatus { get; set; }
        public string ReviewerStatus { get; set; }
        public string ApprovalOwner { get; set; }
        public string ReviewerOwner { get; set; }
        // public bool IsCancelled { get; set; }
        public bool IsReviewerCancelled { get; set; }
        public bool IsApproverCancelled { get; set; }
    }

    public class RAOTRequestApplication
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string AppliedBy { get; set; }
        public string Remarks { get; set; }
        public string OTDuration { get; set; }
        public string OTDate { get; set; }
        public string AppliedDate { get; set; }
        public string TotalHours { get; set; }
        public string Status { get; set; }

        public string FirstName { get; set; }
        public string ApprovedOTHours { get; set; }
        public string ActualOtHours { get; set; }
        public string ReviewerStatus { get; set; }
        public string ApproverStatus { get; set; }
        public string ApprovalOwner { get; set; }
        public string ReviewerOwner { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsReviewerCancelled { get; set; }
        public bool IsApproverCancelled { get; set; }
    }

    public class RACoffCreditRequestApplication
    {
        public DateTime? ExpiryDate { get; set; }
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string AppliedBy { get; set; }
        public string Remarks { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TotalDays { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string IsCancelled { get; set; }
        public string Approval1Owner { get; set; }
        public string Approval2Owner { get; set; }

        public string COffId { get; set; }
        public string Name { get; set; }
        public string LeaveTypeId { get; set; }
        public string LeaveShortName { get; set; }
        public string WorkedDate { get; set; }
        public string AvailFromDate { get; set; }
        public string AvailToDate { get; set; }
        public string COffReason { get; set; }
        public string ApprovalStatusName { get; set; }

        public string Status { get; set; }
        public string ApproverStatus { get; set; }
        public string ReviewerStatus { get; set; }
        public string ApprovalOwner { get; set; }
        public string ReviewerOwner { get; set; }
        // public bool IsCancelled { get; set; }
        public bool IsReviewerCancelled { get; set; }
        public bool IsApproverCancelled { get; set; }


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


    public class RACoffCreditRequestApplicationNew
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string AppliedBy { get; set; }
        public string Remarks { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TotalDays { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string Is_Cancelled { get; set; }
        public string Approval1Owner { get; set; }
        public string Approval2Owner { get; set; }

    }
    public class ManualCoffRequestApplicationModel
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Totaldays { get; set; }
        public string Remarks { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string RequestApplicationType { get; set; }
        public string PermissionType { get; set; }
        public string LeaveTypeId { get; set; }
        public Boolean iscancelled { get; set; }
        /*C-off Leavebalance check */
        public string LeaveName { get; set; }
        public string LeaveBalance { get; set; }
    }
    public class OTRequestApplicationModel
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string OTDate { get; set; }
        public string OTTime { get; set; }
        public string OTDuration { get; set; }
        public string OTReason { get; set; }
        public Boolean IsCancelled { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CancelledOn { get; set; }
        public string CanceledBy { get; set; }
        public string FirstName { get; set; }

    }
    public class RALeaveDonation
    {
        public string LeaveTypeId { get; set; }
        public bool IsCancelled { get; set; }
        public string CancelledOn { get; set; }
        public string CancelledBy { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(10)]
        public string Id { get; set; }

        public string DonarStaffID { get; set; }

        [ForeignKey("DonarStaffID")]
        public virtual Staff staff { get; set; }

        public string ReceiverStaffID { get; set; }

        [ForeignKey("ReceiverStaffID")]
        public virtual Staff Staff { get; set; }

        [Required]
        public DateTime? TransactionDate { get; set; }

        [ForeignKey("LeaveTypeId")]
        public virtual LeaveType LeaveType { get; set; }

        [Required]
        public Decimal LeaveCount { get; set; }

        [Required]
        [MaxLength(100)]
        public string Narration { get; set; }

        public string LeaveDonationApplicationId { get; set; }
        public string ApprovalStatusName { get; set; }
        public string ApprovalStaffName { get; set; }
        public string Comment { get; set; }
        public string ApprovalOwner { get; set; }
        public int IsDocumentAvailable { get; set; }
    }
}
