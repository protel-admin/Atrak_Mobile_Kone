using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Models
{
    public class ManualPunch_Dto
    {
        public string ContactNumber { get; set; }
        public string ManualPunchEndDateTime { get; set; }
        public string ManualPunchStartDateTime { get; set; }

        public DateTime MPEndDateTime { get; set; }
        public DateTime MPStartDateTime { get; set; }

        public string PunchType { get; set; }
        public string Remarks { get; set; }
        public string StaffId { get; set; }
    }

}

/*{AttendanceManagement.Models.RAManualPunchApplicationViewModel
 *  AppliedBy: null
ApprovalOwner: null
ApprovalStatus: null
ApproverEmailId: null
ApproverId: "00118"
ApproverName: "V.KAILASAM-"
ApproverStatus: null
ContactNumber: "2323"
DepartmentName: "Accounts"
Id: null
IsApproverCancelled: false
IsCancelled: false
IsReviewerCancelled: false
LeaveEndDurationId: null
LeaveStartDurationId: null
ManualPunchEndDateTime: "12-Jun-2020 23:20"
ManualPunchStartDateTime: "12-Jun-2020 09:30"
PunchType: "InOut"
Remarks: "ewwew"
ReviewerEmailId: "kailash@tagros.com"
ReviewerId: "00118"
ReviewerName: "V.KAILASAM-"
ReviewerOwner: null
ReviewerStatus: null
SecurityGroupId: 0
StaffId: "00163"
StaffName: "S.RAMESH BABU-"
StfId: null
UserEmailId: "indira@tagros.com"
 * */
