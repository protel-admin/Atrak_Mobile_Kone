using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Models
{
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
}