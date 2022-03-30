using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Models
{
    public class Permission_Dto
    {

        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string StaffEmailId { get; set; }
        public string PermissionOffReason { get; set; }

        public string FromTime { get; set; }
        public string ToTime { get; set; }

        public DateTime FromTimeStart { get; set; }
        public DateTime ToTimeEnd { get; set; }


        public string TotalHours { get; set; }
        public DateTime TotHours { get; set; }

        public string ContactNbr { get; set; }
        public string DateApplied { get; set; }
        public string PermissionStartDate { get; set; }
        public string PermissionEndDate { get; set; }

        public DateTime PerStartDate { get; set; }
        public DateTime PerEndDate { get; set; }


        public string ApprovalStatus1 { get; set; }
        public string ApprovalStatus2 { get; set; }
        public string ApprovalOwnerId { get; set; }
        public string ReviewerOwnerId { get; set; }
        public string ReviewerOwnerName { get; set; }
        public string Approval1OwnerName { get; set; }
        public string Approval2OwnerName { get; set; }
        public string PermissionTypeId { get; set; }
        public string PermissionType { get; set; }
        public string ReportingManagerEmailId { get; set; }
        public string ReportingManagerId { get; set; }
        public string ReportingManagerName { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsReviewerCancelled { get; set; }
        public bool IsApproverCancelled { get; set; }
    }
}