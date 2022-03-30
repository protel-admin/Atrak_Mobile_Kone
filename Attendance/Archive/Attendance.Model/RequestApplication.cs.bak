using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class RequestApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(20)]
        public string Id{get;set;}
        //
        [Required]
        [MaxLength(20)]
        public string StaffId{get;set;}
        //
        [MaxLength(10)]
        public string LeaveTypeId{get;set;}
        //
        public int LeaveStartDurationId{get;set;}
        //
        public DateTime StartDate{get;set;}
        //
        public DateTime EndDate{get;set;}
        //
        public int LeaveEndDurationId{get;set;}
        //
        public string TotalDays{get;set;}
        //
        [MaxLength(20)]
        public string PermissionType{get;set;}// Shift Start, Shift End, In Between Shift
        //
        [MaxLength(20)]
        public string OTRange{get;set;}//Before Shift Start , After Shift End.
        //
        [MaxLength(20)]
        public string ODDuration{get;set;}// SINGLE DAY, MULTIPLE DAYS
        //
        [MaxLength(10)]
        public string NewShiftId{get;set;}
        //
        public int RHId{get;set;}
        //
        public DateTime? TotalHours{get;set;}//This field can be used for Permissions and OT time
        //
        [MaxLength(200)]
        public string Remarks{get;set;}
        //
        public int ReasonId{get;set;}
        //
        [MaxLength(20)]
        public string ContactNumber{get;set;}
        //
        [MaxLength(5)]
        public string PunchType{get;set;}// IN, OUT OR INOUT
        //
        [Required]
        public DateTime? ApplicationDate{get;set;}
        //
        [Required]
        [MaxLength(10)]
        public string AppliedBy{get;set;}
        //
        [Required]
        public bool IsCancelled{get;set;}
        //
        public DateTime? CancelledDate{get;set;}
        //
        [MaxLength(10)]
        public string CancelledBy{get;set;}
        //
        //
        [Required]
        public bool IsReviewerCancelled { get; set; }
        //
        public DateTime? ReviewerCancelledDate { get; set; }
        //
        [MaxLength(10)]
        public string ReviewerCancelledBy { get; set; }
        //
        [Required]
        public bool IsApproverCancelled { get; set; }
        //
        public DateTime? ApproverCancelledDate { get; set; }
        //
        [MaxLength(10)]
        public string ApproverCancelledBy { get; set; }
        //

        [Required]
        public bool IsApproved { get; set; }
        //
        [Required]
        public bool IsReviewed { get; set; }
        //
        [Required]
        public bool IsRejected { get; set; }
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
    }

    public class RALeaveApplication
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string AppliedBy { get; set; }
        public string Remarks { get; set; }
        public string FromDuration {get; set;}
        public string StartDate { get; set; }
        public string ToDuration { get; set; }
        public string EndDate { get; set; }
        public string TotalDays { get; set; }
        public string Type { get; set; }
        public string ApproverStatus { get; set; }
        public string ReviewerStatus { get; set; }
        public string Status { get; set; }
        public string ApprovalOwner { get; set; }
        public string ReviewerOwner { get; set; }
        public bool IsCancelled { get; set; }
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
        public string ApproverStatus { get; set; }
        public string ReviewerStatus { get; set; }
        public string Status { get; set; }
        public string ApprovalOwner { get; set; }
        public string ReviewerOwner { get; set; }
        public bool IsCancelled { get; set; }
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
        public string Status { get; set; }
        public string ApproverStatus { get; set; }
        public string ReviewerStatus { get; set; }
        public string ApprovalOwner { get; set; }
        public string ReviewerOwner { get; set; }
        public bool IsCancelled { get; set; }
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
        public string TotalDays { get; set; }
        public string Status { get; set; }
    }

    public class RAODRequestApplication
    {
        public string ApplicationType { get; set; }
        public string Id { get; set; }
        public string ParentType { get; set; }
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
        public string Status { get; set; }
        public string ApproverStatus { get; set; }
        public string ReviewerStatus { get; set; }
        public string ApprovalOwner { get; set; }
        public string ReviewerOwner { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsReviewerCancelled { get; set; }
        public bool IsApproverCancelled { get; set; }
    }

    public class RAOTRequestApplication
    {
     

         public string Id { get; set; }
         public string StaffId { get; set; }
         public string  FirstName        { get; set; } 
         public string  OTDate           { get; set; }
         public string  ApprovedOTHours  { get; set; }
         public string  ActualOtHours    { get; set; }
         public string  ReviewerStatus   { get; set; }
         public string  ApproverStatus   { get; set; }
         public string ApprovalOwner { get; set; }
         public string ReviewerOwner { get; set; }
         public bool IsCancelled { get; set; }
         public bool IsReviewerCancelled { get; set; }
         public bool IsApproverCancelled { get; set; }
    }

    public class RACoffCreditRequestApplication
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string AppliedBy { get; set; }
        public string Remarks { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TotalDays { get; set; }
        public string Status { get; set; }
        public string ApproverStatus { get; set; }
        public string ReviewerStatus { get; set; }
        public string ApprovalOwner { get; set; }
        public string ReviewerOwner { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsReviewerCancelled { get; set; }
        public bool IsApproverCancelled { get; set; }

    }
}
