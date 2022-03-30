using Attendance.Model;
using Attendance.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace Attendance.BusinessLogic
{
    public class RALeaveApplicationBusinessLogic
    {
        RALeaveApplicationRepository repoObj = new RALeaveApplicationRepository();
        public List<RALeaveApplication> GetAppliedLeaves(string StaffId, string AppliedId, string userRole)
        {
            using (RALeaveApplicationRepository repo = new RALeaveApplicationRepository())
            { 
                var Obj = repo.GetAppliedLeaves(StaffId, AppliedId, userRole);
            return Obj;
            }
        }
        public List<LeaveReasonList> GetLeaveCDReasonList(string user)
        {
            using (var repo = new RALeaveApplicationRepository())
            { 
                var lst = repo.GetLeaveCDReasonList(user);
            return lst;
            }
        }

        public string ValidateLeaveApplication(string StaffId, DateTime Startdate,  DateTime EndDate, string LeaveTypeId, decimal TotalDays)
        {
            string str = string.Empty;

            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {

                str = rALeaveApplicationRepository.ValidateLeaveApplication(StaffId, Startdate,  EndDate,  LeaveTypeId, TotalDays);
            }
        
            return str;
        }
        public List<RALeaveApplication> GetAppliedLeavesForMyTeam(string StaffId, string AppliedId, string userRole)
        {
            using (RALeaveApplicationRepository repo = new RALeaveApplicationRepository())
            { 
                var Obj = repo.GetAppliedLeavesForMyTeam(StaffId, AppliedId, userRole);
            return Obj;
            }
        }

        public List<RALeaveApplication> GetAppliedLeaves(string StaffId)
        {
            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {

                return rALeaveApplicationRepository.GetAppliedLeaves(StaffId);
            }
        }



        public List<RALeaveApplication> GetAllEmployeesLeaveList(string StaffId, string userRole, string AppliedId)
        {
            using (RALeaveApplicationRepository repo = new RALeaveApplicationRepository())
            { 
                var Obj = repo.GetAllEmployeesLeaveList(StaffId, userRole, AppliedId);
            return Obj;
            }
        }

        public List<LeaveReasonList> GetLeaveReasonList(string user)
        {
            using (var repo = new RALeaveApplicationRepository())
            { 
                var lst = repo.GetLeaveReasonList(user);
            return lst;
            }
        }

        public List<RALeaveApplication> GetApprovedLeavesForMyTeam(string StaffId)
        {
            using (RALeaveApplicationRepository repo = new RALeaveApplicationRepository())
            {     
                var Obj = repo.GetApprovedLeavesForMyTeam(StaffId);
            return Obj;
            }
        }

        public List<LeaveReasonList> GetLeaveTypes(string StaffId, string Gender)
        {
            using (RALeaveApplicationRepository repo = new RALeaveApplicationRepository())
            { 
                var Obj = repo.GetLeaveTypes(StaffId, Gender);
            return Obj;
            }
        }
        public List<SelectListItem> ConvertLeaveTypesToListItems(List<LeaveReasonList> objListOfLeaveTypes)
        {
            List<SelectListItem> _ListOfLeaveTypes_ = new List<SelectListItem>();
            foreach (var l in objListOfLeaveTypes)
            {
                _ListOfLeaveTypes_.Add(new SelectListItem
                {
                    Value = l.Id,
                    Text = l.Name
                });
            }

            return _ListOfLeaveTypes_;
        }

        public List<LeaveDuration> GetLeaveDurations()
        {
            using (RALeaveApplicationRepository repo = new RALeaveApplicationRepository())
            { 
                var Obj = repo.GetLeaveDurations();
            return Obj;
            }
        }

        public List<SelectListItem> ConvertLeaveDurationsToListItems(List<LeaveDuration> objListOfLeaveDurations)
        {
            List<SelectListItem> _ListOfLeaveDurations_ = new List<SelectListItem>();
            foreach (var l in objListOfLeaveDurations)
            {
                _ListOfLeaveDurations_.Add(new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                });
            }

            return _ListOfLeaveDurations_;
        }

        public string GetTotalDaysLeave(string StaffId, string LeaveStartDurationId, string FromDate, string ToDate, string LeaveEndDurationId, string LeaveType)
        {
            using (var repo = new RALeaveApplicationRepository())
            { 
                var data = repo.GetTotalDaysLeave(StaffId, LeaveStartDurationId, FromDate, ToDate, LeaveEndDurationId, LeaveType);
            return data;
            }
        }

        public string GetUniqueId()
        {
            using (var repo = new RALeaveApplicationRepository())
                return repo.GetUniqueId();
        }

        public void SaveRequestApplication(ClassesToSave DataToSave, string SecurityGroupId, string BaseAddress)
        {
            using (RALeaveApplicationRepository repo = new RALeaveApplicationRepository())
            { 
                var cm = new CommonRepository();
            //Changes made by Aarthi on 25/04/2020
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
            CommonBusinessLogic CBL = new CommonBusinessLogic();

            string approvalOwner1Name = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string senderEmailId = string.Empty;
            string appliedByUserEmailId = string.Empty;
            string appliedByUserName = string.Empty;
            string approvalOwner2Name = string.Empty;
            string leaveTypeName = string.Empty;
            bool isFinalLevelApproval = false;
            string payPeriodValidationMessage = string.Empty;

            applicantEmailId = cm.GetEmailIdOfEmployee(DataToSave.RA.StaffId);
            approvalOwner1EmailId = cm.GetEmailIdOfEmployee(DataToSave.AA.ApprovalOwner);

            //Added by Rajesh Nov 24    - trinary condition
            approvalOwner2EmailId = DataToSave.AA.Approval2Owner != null ? cm.GetEmailIdOfEmployee(DataToSave.AA.Approval2Owner) : string.Empty;

            appliedByUserEmailId = cm.GetEmailIdOfEmployee(DataToSave.RA.AppliedBy);
            commonSenderEmailId = cm.GetSenderEmailIdFromEmailSettings();
            applicantName = cm.GetStaffName(DataToSave.RA.StaffId);
            approvalOwner1Name = cm.GetStaffName(DataToSave.AA.ApprovalOwner);

            //Added by Rajes on Nov 24 2021 - trinary condition
            approvalOwner2Name = cm.GetStaffName(DataToSave.AA.Approval2Owner == null ? DataToSave.AA.ApprovalOwner : DataToSave.AA.Approval2Owner);

            appliedByUserName = cm.GetStaffName(DataToSave.RA.AppliedBy);
            leaveTypeName = cm.GetLeaveName(DataToSave.RA.LeaveTypeId);
            var Startduration = repo.GetStartduration(DataToSave.RA.LeaveStartDurationId);
            var Endduration = repo.GetEndduration(DataToSave.RA.LeaveEndDurationId);

            payPeriodValidationMessage = CBL.ValidateApplicationForPayDate(DataToSave.RA.StaffId, Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy"),
                    Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy"));
            if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
            {
                if (payPeriodValidationMessage.ToUpper() != "VALID")
                {
                    throw new ApplicationException("Application of past pay cycle cannot be saved");
                }
            }
            string validationMessage = ValidateLeaveApplication(DataToSave.RA.StaffId, DataToSave.RA.StartDate,  DataToSave.RA.EndDate,  DataToSave.RA.LeaveTypeId, DataToSave.RA.TotalDays);
            if (string.IsNullOrEmpty(validationMessage).Equals(false))
            {
                if (validationMessage.ToUpper() != "OK" && validationMessage.ToUpper() != "OK.")
                {
                    throw new ApplicationException(validationMessage);
                }
            }
            if (DataToSave.RA.AppliedBy.Equals(DataToSave.RA.StaffId))
            {
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    // Send intimation to the applicant
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Request for Leave application sent to " + approvalOwner1Name,
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your leave application has been submitted to your Reporting Manager (" + approvalOwner1Name + ") for Approval." +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; " +
                        "font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;" +
                        "font-family:tahoma; font-size:9pt;\">" + applicantName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; " +
                        "font-size:9pt;\">LeaveType:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + leaveTypeName + "</td>" +
                        "</tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">From Date:</td><td style=\"width:80%;" +
                        "font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td>" +
                        "</tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Start Duration:</td><td style=\"width:80%;" +
                        "font-family:tahoma; font-size:9pt;\">" + Startduration + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">End Date:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + "</td>" +
                        "</tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">End Duration:</td><td style=\"width:80%;font-family:tahoma; " +
                        "font-size:9pt;\">" + Endduration + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td><td style=\"width:80%;" +
                        "font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.Remarks + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Best Regards",
                        CreatedOn = DateTime.Now,
                        CreatedBy = DataToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }

                //Send intimation email to the reporting manager.
                if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = approvalOwner1EmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Request for Leave application of " + applicantName,
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner1Name + "" +
                        ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + applicantName + " has applied for a Leave. Leave details are given below.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                        "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr>" +
                        "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">StaffId:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">LeaveType:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + leaveTypeName + "</td></tr><tr>" +
                        "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">From Date:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                        "</td></tr><tr><td style=\"width:20%;" + "font-family:tahoma; font-size:9pt;\"> To Date:</td><td style=\"width:80%;" +
                        "font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") +
                        "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Total Days:</td><td style=\"width:80%;font-family:tahoma;" +
                        " font-size:9pt;\">" + DataToSave.RA.TotalDays + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.Remarks + "</td>" +
                        "</tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required " +
                        "to approve or reject this application.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "<a href=\"" + BaseAddress + "\">Redirect To Site</a></p><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + applicantName + " &nbsp;(" + DataToSave.RA.StaffId + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = DataToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                DataToSave.ESL = ESL;
            }


            // If the application has applied by the Approval Owner1 or Reporting Manager
            else if (DataToSave.RA.AppliedBy.Equals(DataToSave.AA.ApprovalOwner) &&
                 DataToSave.RA.AppliedBy != DataToSave.AA.Approval2Owner)
            {
                DataToSave.RA.IsApproved = false;
                DataToSave.AA.ApprovalStatusId = 2;
                DataToSave.AA.Approval2statusId = 1;
                DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                DataToSave.AA.ApprovedOn = DateTime.Now;
                DataToSave.AA.Comment = "APPROVED THE LEAVE REQUEST.";


                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    //Send intimation to the applicant 
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested Leave application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Your Leave  application " +
                        "from " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                        " to " + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + " has been applied and approved by " + appliedByUserName + "" +
                        " and sent for second level approval.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + appliedByUserName + " &nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = DataToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }

                //Send intimation to Approval Owner2
                if (string.IsNullOrEmpty(DataToSave.AA.Approval2Owner).Equals(false) && string.IsNullOrEmpty(approvalOwner2EmailId)
                    .Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = approvalOwner1EmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Request for Leave application of " + applicantName,
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner2Name + "" +
                        ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + appliedByUserName + " has applied and approved a Leave application for " + applicantName + " . Leave details are given below.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                        "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr>" +
                        "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">StaffId:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">LeaveType:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + leaveTypeName + "</td></tr><tr>" +
                        "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">From Date:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                        "</td></tr><tr><td style=\"width:20%;" + "font-family:tahoma; font-size:9pt;\"> To Date:</td><td style=\"width:80%;" +
                        "font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") +
                        "</td></tr><tr><td style=\"width:20%;font-family:tahoma;" +
                        " font-size:9pt;\">Total Days:</td><td style=\"width:80%;font-family:tahoma;" +
                        " font-size:9pt;\">" + DataToSave.RA.TotalDays +
                        "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.Remarks + "</td>" +
                        "</tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required " +
                        " to approve or reject this application.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "<a href=\"" + BaseAddress + "\">Redirect To Site</a></p><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + appliedByUserName + " &nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = DataToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                DataToSave.ESL = ESL;
            }

            // If the application has applied by Approval Owner2 or Reviewer
            else if (DataToSave.RA.AppliedBy.Equals(DataToSave.AA.Approval2Owner))
            {
                isFinalLevelApproval = true;
                DataToSave.RA.IsApproved = true;
                if (DataToSave.AA.ApprovalOwner == DataToSave.AA.Approval2Owner)
                {
                    DataToSave.AA.ApprovalStatusId = 2;
                    DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedOn = DateTime.Now;
                }
                DataToSave.AA.Approval2statusId = 2;
                DataToSave.AA.Approval2By = DataToSave.RA.AppliedBy;
                DataToSave.AA.Approval2On = DateTime.Now;
                DataToSave.AA.Comment = "APPROVED THE LEAVE REQUEST.";

                //deduct leave balance from employee leave account table.
                ELA.StaffId = DataToSave.RA.StaffId;
                ELA.LeaveTypeId = DataToSave.RA.LeaveTypeId;
                ELA.TransactionFlag = 2;
                ELA.TransactionDate = DateTime.Now;
                ELA.LeaveCount = Convert.ToDecimal(DataToSave.RA.TotalDays) * -1;
                ELA.Narration = "Approved the leave application.";
                ELA.LeaveCreditDebitReasonId = 22;
                ELA.RefId = DataToSave.RA.Id;
                ELA.Month = Convert.ToDateTime(DataToSave.RA.StartDate).Month;
                ELA.Year = Convert.ToDateTime(DataToSave.RA.StartDate).Year;
                ELA.TransctionBy = DataToSave.RA.AppliedBy;
                DataToSave.ELA = ELA;

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested Leave application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Your Leave application" +
                        " from " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy")
                        + " to " + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + " has been applied and approved.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">" + "Best Regards,</p><p style=\"font-family:tahoma; " +
                        "font-size:9pt;\">" + appliedByUserName + " &nbsp;" +
                        "(" + DataToSave.RA.AppliedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = DataToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                DataToSave.ESL = ESL;
            }

            else if (DataToSave.RA.AppliedBy != DataToSave.RA.StaffId && DataToSave.RA.AppliedBy != DataToSave.AA.ApprovalOwner &&
                  (SecurityGroupId.Equals("1") || SecurityGroupId.Equals("3") || SecurityGroupId.Equals("5")))
            {
                isFinalLevelApproval = true;
                DataToSave.RA.IsApproved = true;
                DataToSave.AA.ApprovalOwner = DataToSave.RA.AppliedBy;
                DataToSave.AA.Approval2Owner = DataToSave.RA.AppliedBy;
                DataToSave.AA.ApprovalStatusId = 2;
                DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                DataToSave.AA.ApprovedOn = DateTime.Now;
                DataToSave.AA.Approval2statusId = 2;
                DataToSave.AA.Approval2By = DataToSave.RA.AppliedBy;
                DataToSave.AA.Approval2On = DateTime.Now;
                DataToSave.AA.Comment = "APPROVED THE LEAVE REQUEST.";


                //deduct leave balance from employee leave account table.
                ELA.StaffId = DataToSave.RA.StaffId;
                ELA.LeaveTypeId = DataToSave.RA.LeaveTypeId;
                ELA.TransactionFlag = 2;
                ELA.TransactionDate = DateTime.Now;
                ELA.LeaveCount = Convert.ToDecimal(DataToSave.RA.TotalDays) * -1;
                ELA.Narration = "Approved the leave application.";
                ELA.LeaveCreditDebitReasonId = 22;
                ELA.RefId = DataToSave.RA.Id;
                ELA.Month = Convert.ToDateTime(DataToSave.RA.StartDate).Month;
                ELA.Year = Convert.ToDateTime(DataToSave.RA.StartDate).Year;
                ELA.TransctionBy = DataToSave.RA.AppliedBy;
                DataToSave.ELA = ELA;

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested Leave application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your Leave application from " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                        " to " + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + " has been applied and approved.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + appliedByUserName + " &nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = DataToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                DataToSave.ESL = ESL;
            }
            repo.SaveRequestApplication(DataToSave, isFinalLevelApproval);
            }
        }


        public void RejectApplication(string Id, string RejectedBy, string LocationId)
        {
            //Get the leave application details based on the id passed to this function as a parameter.
            RequestApplication Obj = new RequestApplication();
            ApplicationApproval AA = new ApplicationApproval();
            AlternativePersonAssign APA = new AlternativePersonAssign();

            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {
                Obj = rALeaveApplicationRepository.GetRequestApplicationDetails(Id);
                AA = rALeaveApplicationRepository.GetApplicationApproval(Id);
                APA = rALeaveApplicationRepository.GetApplicationForWorkAllocation(Id);
            }

            List<EmailSendLog> ESL = new List<EmailSendLog>();
            ClassesToSave CTS = new ClassesToSave();
            //first send acknowledgement email to the user.
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner1Name = string.Empty;
            string approvalOwner2Name = string.Empty;
            string rejectedByUserName = string.Empty;
            string rejectedByUserEmailId = string.Empty;
            string alternatePersonName = string.Empty;
            string alternatePersonEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string ccEmailAddress = string.Empty;
            string LeaveTypeName = string.Empty;
            using (CommonRepository commonRepository = new CommonRepository())
            {
                approvalOwner1 = commonRepository.GetApproverOwner(Id);
                approvalOwner2 = commonRepository.GetReviewerOwner(Id);
                applicantName = commonRepository.GetStaffName(Obj.StaffId);
                applicantEmailId = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
                rejectedByUserName = commonRepository.GetStaffName(RejectedBy);
                rejectedByUserEmailId = commonRepository.GetEmailIdOfEmployee(RejectedBy);
                approvalOwner2Name = commonRepository.GetStaffName(approvalOwner2);
                commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
                LeaveTypeName = commonRepository.GetLeaveName(Obj.LeaveTypeId);
                ccEmailAddress = commonRepository.GetCCAddress("LEAVEAPPLICATION", LocationId);
            }
            if (APA != null && string.IsNullOrEmpty(APA.AlternativeStaffId).Equals(false))
            {
                using (CommonRepository commonRepository = new CommonRepository())
                {
                    alternatePersonName = commonRepository.GetStaffName(APA.AlternativeStaffId);
                    alternatePersonEmailId = commonRepository.GetEmailIdOfEmployee(APA.AlternativeStaffId);
                }
            }

            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true))    //if the leave application has been cancelled then...
            {
                throw new Exception("Cancelled leave request cannot be rejected.");
            }
            else if (Obj.IsApproved.Equals(true) && RejectedBy == approvalOwner1) //if the leave application has been approved then...
            {
                throw new Exception("Approved leave request cannot be rejected.");
            }
            else if (Obj.IsApproved.Equals(true)) //if the leave application has been approved then...
            {
                throw new Exception("Approved leave request cannot be rejected.");
            }
            else if (Obj.IsRejected.Equals(true))  //if the leave application has been rejected then...
            {
                throw new Exception("Rejected leave request cannot be rejected.");
            }
            else //if the leave application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {

                if (RejectedBy == approvalOwner1 && RejectedBy != approvalOwner2)
                {
                    Obj.IsRejected = true;

                    AA.ApprovalStatusId = 3;
                    AA.ApprovedBy = RejectedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "LEAVE REQUEST HAS BEEN REJECTED BY THE REPORTING MANAGER.";
                    //email for user
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = ccEmailAddress,
                            BCC = string.Empty,
                            EmailSubject = "Requested leave application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Your " + LeaveTypeName + "  application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy")
                                + " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + " has been rejected.</p><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + rejectedByUserName + " &nbsp;" +
                                "(" + RejectedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = RejectedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                    // email AlternativeStaff
                    if (APA != null)
                    {
                        APA.IsRejected = true;
                        APA.RejectMailSent = true;
                        if (string.IsNullOrEmpty(alternatePersonEmailId).Equals(false))
                        {
                            ESL.Add(new EmailSendLog
                            {
                                From = commonSenderEmailId,
                                To = alternatePersonEmailId,
                                CC = string.Empty,
                                BCC = string.Empty,
                                EmailSubject = "Work Allocation for leave application of " + applicantName,
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                    "Dear " + alternatePersonName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                    "A request for work allocation application has been assigned for " + applicantName + " on application for the date "
                                    + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate).
                                    ToString("dd-MMM-yyyy") + " has been Rejected. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                                    "<p style=\"font-family:tahoma; font-size:9pt;\">" + rejectedByUserName + " &nbsp;(" + RejectedBy + ")</p></body></html>",
                                CreatedOn = DateTime.Now,
                                CreatedBy = RejectedBy,
                                IsSent = false,
                                SentOn = null,
                                IsError = false,
                                ErrorDescription = "-",
                                SentCounter = 0
                            });
                        }
                    }
                }
                //reject the application.
                if (RejectedBy == approvalOwner2)
                {
                    Obj.IsRejected = true;
                    if (approvalOwner1 == approvalOwner2)
                    {
                        AA.ApprovalStatusId = 3;
                        AA.ApprovedBy = RejectedBy;
                        AA.ApprovedOn = DateTime.Now;
                    }
                    AA.Approval2statusId = 3;
                    AA.Approval2On = DateTime.Now;
                    AA.Approval2By = RejectedBy;
                    AA.Comment = "LEAVE REQUEST HAS BEEN REJECTED BY THE REPORTING MANAGER.";
                    //email for user
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = ccEmailAddress,
                            BCC = string.Empty,
                            EmailSubject = "Requested leave application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Your " + LeaveTypeName + "  application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy")
                                + " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + " has been rejected.</p><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + rejectedByUserName + " &nbsp;" +
                                "(" + RejectedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = RejectedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                    // email AlternativeStaff
                    if (APA != null)
                    {
                        APA.IsRejected = true;
                        APA.RejectMailSent = true;

                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = alternatePersonEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Work Allocation for leave application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + alternatePersonName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "A request for work allocation application has been assigned for " + applicantName + " on application for the date" +
                            " " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate).
                            ToString("dd-MMM-yyyy") + " has been Rejected. <p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + rejectedByUserName + " &nbsp;(" + RejectedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = RejectedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                }

                CTS.RA = Obj;
                CTS.AA = AA;
                CTS.ESL = ESL;
                CTS.APA = APA;
                using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
                {
                    rALeaveApplicationRepository.RejectApplication(CTS);
                }
            }
        }





        public void SaveDocumentInformation(DocumentUpload LawDocument)
        {
            using (var repo = new RALeaveApplicationRepository())
                repo.SaveDocumentInformation(LawDocument);
        }

        public void FromDateShouldBeLessThanToDate(DateTime FromDate, DateTime ToDate)
        {

        }

        public string RequestApplicationMustNotOverLapWithTheOther(string StaffId, string LeaveStartDurationId, string LeaveStartDate, string LeaveEndDate, string LeaveEndDurationId)
        {
            using (RALeaveApplicationRepository repo = new RALeaveApplicationRepository())
            { 
                var str = repo.RequestApplicationMustNotOverLapWithTheOther(StaffId, LeaveStartDurationId, LeaveStartDate, LeaveEndDate, LeaveEndDurationId);

            if (!str.ToUpper().StartsWith("OK"))
            {
                throw new Exception(str);
            }

            return str;
            }
        }

        private bool IsFromDateMoreOrEqualToCurrerntDate(DateTime? LeaveStartDate, DateTime? CurrentDate)
        {
            if (LeaveStartDate.Value.Date < CurrentDate.Value.Date)
            {
                return false;
            }
            else if (LeaveStartDate.Value.Date >= CurrentDate.Value.Date)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RejectApplication(string Id, string RejectedBy)
        {
            //Get the leave application details based on the id passed to this function as a parameter.
            using (RALeaveApplicationRepository repo = new RALeaveApplicationRepository())
            { 
                var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id);
            var cm = new CommonRepository();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            ClassesToSave CTS = new ClassesToSave();
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string leaveTypeName = string.Empty;
            string approvalOwnerName = string.Empty;
            string approvalOwner2Name = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string rejectedByUserName = string.Empty;
            string rejectedByUserEmailid = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string payPeriodValidationMessage = string.Empty;

            applicantName = cm.GetStaffName(Obj.StaffId);
            applicantEmailId = cm.GetEmailIdOfEmployee(Obj.StaffId);
            approvalOwner1 = cm.GetApproverOwner(Id);
            approvalOwner2 = cm.GetReviewerOwner(Id);
            approvalOwnerName = cm.GetStaffName(approvalOwner1);
            approvalOwner2Name = cm.GetStaffName(approvalOwner2);
            rejectedByUserName = cm.GetStaffName(RejectedBy);
            rejectedByUserEmailid = cm.GetEmailIdOfEmployee(RejectedBy);
            approvalOwner1EmailId = cm.GetEmailIdOfEmployee(approvalOwner2);
            commonSenderEmailId = cm.GetSenderEmailIdFromEmailSettings();
            leaveTypeName = cm.GetLeaveName(Obj.LeaveTypeId);
            if (Obj.AppliedBy == "")
            {
                Obj.AppliedBy = Obj.StaffId;
            }
            payPeriodValidationMessage = cm.ValidateApplicationForPayDate(Obj.StaffId, Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy"),
                  Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy"));
            if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
            {
                if (payPeriodValidationMessage.ToUpper() != "VALID")
                {
                    throw new ApplicationException("Application of past pay cycle cannot be rejected");
                }
            }
            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true))    //if the leave application has been cancelled then...
            {
                throw new Exception("Cancelled leave request cannot be rejected.");
            }
            else if (Obj.IsRejected.Equals(true))  //if the leave application has been rejected then...
            {
                throw new Exception("Rejected leave request cannot be rejected.");
            }
            else //if the leave application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {
                //reject the application.
                if (RejectedBy == approvalOwner1 && RejectedBy != approvalOwner2)
                {
                    Obj.IsRejected = true;
                    AA.ApprovalStatusId = 3;
                    AA.ApprovedBy = RejectedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "LEAVE REQUEST HAS BEEN REJECTED BY THE APPROVALOWNER1.";
                }
                else if (RejectedBy == approvalOwner2)
                {
                    if (approvalOwner1 == approvalOwner2)
                    {
                        AA.ApprovalStatusId = 3;
                        AA.ApprovedBy = RejectedBy;
                        AA.ApprovedOn = DateTime.Now;
                    }
                    Obj.IsRejected = true;
                    AA.Approval2statusId = 3;
                    AA.Approval2On = DateTime.Now;
                    AA.Approval2By = RejectedBy;
                    AA.Comment = "LEAVE REQUEST HAS BEEN REJECTED BY THE APPROVALOWNER2.";
                    //email for user
                }

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested leave application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your " + leaveTypeName + "  application from " + Convert.ToDateTime(Obj.StartDate)
                        .ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + "" +
                        " has been rejected.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">" + rejectedByUserName + " &nbsp;(" + RejectedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = RejectedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }

                CTS.RA = Obj;
                CTS.AA = AA;
                CTS.ESL = ESL;
                repo.RejectApplication(CTS);
            }
            }
        }

        public void ApproveApplication(string Id, string ApprovedBy, string LocationId)
        {
            //Get the leave application details based on the Id passed to this function as a parameter.
            RequestApplication Obj = new RequestApplication();
            ApplicationApproval AA = new ApplicationApproval();
            AlternativePersonAssign APA = new AlternativePersonAssign();

            EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            ClassesToSave CTS = new ClassesToSave();

            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {
                Obj = rALeaveApplicationRepository.GetRequestApplicationDetails(Id);
                AA = rALeaveApplicationRepository.GetApplicationApproval(Id);
                APA = rALeaveApplicationRepository.GetApplicationForWorkAllocation(Id);
            }

            string staffId = string.Empty;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            string applicationId = string.Empty;
            string applicationType = string.Empty;
            string BaseAddress = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner1Name = string.Empty;
            string approvalOwner2Name = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string approvedByUserName = string.Empty;
            string approvedByUserEmailId = string.Empty;
            string alternatePersonName = string.Empty;
            string alternatePersonEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string ccEmailAddress = string.Empty;
            string LeaveTypeName = string.Empty;
            using (CommonRepository commonRepository = new CommonRepository())
            {
                approvalOwner1 = commonRepository.GetApproverOwner(Id);
                approvalOwner2 = commonRepository.GetReviewerOwner(Id);
                applicantName = commonRepository.GetStaffName(Obj.StaffId);
                applicantEmailId = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
                approvedByUserName = commonRepository.GetStaffName(ApprovedBy);
                approvedByUserEmailId = commonRepository.GetEmailIdOfEmployee(ApprovedBy);
                approvalOwner2Name = commonRepository.GetStaffName(approvalOwner2);
                approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner2);
                commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
                LeaveTypeName = commonRepository.GetLeaveName(Obj.LeaveTypeId);
                ccEmailAddress = commonRepository.GetCCAddress("LEAVEAPPLICATION", LocationId);
            }
            BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();

            if (APA != null && string.IsNullOrEmpty(APA.AlternativeStaffId).Equals(false))
            {
                using (CommonRepository commonRepository = new CommonRepository())
                {
                    alternatePersonName = commonRepository.GetStaffName(APA.AlternativeStaffId);
                    alternatePersonEmailId = commonRepository.GetEmailIdOfEmployee(APA.AlternativeStaffId);
                }
            }

            if (ApprovedBy == approvalOwner1)
            {
                if (Obj.IsApproved.Equals(true)) //if application has already been approved then...
                {
                    throw new Exception("Cannot approve already approved leave request.");
                }
            }
            else if (ApprovedBy == approvalOwner2)
            {
                if (Obj.IsApproved.Equals(true)) //if application has already been approved then...
                {
                    throw new Exception("Cannot approve already approved leave request.");
                }
            }

            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true)) //if the leave application has been cancelled then...
            {
                throw new Exception("Cannot approve a cancelled leave application.");
            }
            else if (Obj.IsRejected.Equals(true))
            {
                throw new Exception("Cannot approve already rejected leave request.");
            }
            else
            {
                if (ApprovedBy == approvalOwner1 && ApprovedBy != approvalOwner2)
                {
                    //approve the application.
                    Obj.IsApproved = true;
                    AA.ApprovalStatusId = 2;
                    AA.ApprovedBy = ApprovedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "APPROVED THE LEAVE REQUEST.";
                    ELA = null;

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = ccEmailAddress,
                            BCC = string.Empty,
                            EmailSubject = "Requested leave application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your " + LeaveTypeName + "  application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
                            " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + " has been approved.</p><p style=\"font-family:tahoma; " +
                            "font-size:9pt;\"><a href=\"" + BaseAddress + "\">Click Here</a></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + approvedByUserName + " &nbsp;(" + ApprovedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = ApprovedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                    if (string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Request for leave application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner2Name + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "A request for leave application of " + applicantName + " has been approved by " + approvedByUserName + "" +
                            " . Details are given below</p>" +
                            "<p><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">" +
                            "StaffId:</td><td style=\"width:80%;\">" + Obj.StaffId + "</td></tr><tr><td style=\"width:20%;\">" +
                            "From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(Obj.StartDate)
                            .ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">" + "To Date:</td><td style=\"width:80%;\">" +
                            "" + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + "</td></tr><tr>" +
                            "<td style=\"width:20%;\">Total Days:</td><td style=\"width:80%;\">" + Obj.TotalDays + "</td></tr>" +
                            "<tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + Obj.Remarks + "</td></tr></table></p>" +
                            "<p>Your attention is required to either approve or reject this application.</p><a href=\"" + BaseAddress + "\">" +
                            "Click Here</a><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = Obj.StaffId,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }

                    if (APA != null)
                    {
                        if (APA.AlternativeStaffId != null)
                        {
                            APA.IsApproved = true;
                            APA.ConfirmationMailSent = true;
                            if (string.IsNullOrEmpty(alternatePersonEmailId).Equals(false))
                            {
                                ESL.Add(new EmailSendLog
                                {
                                    From = commonSenderEmailId,
                                    To = alternatePersonEmailId,
                                    CC = string.Empty,
                                    BCC = string.Empty,
                                    EmailSubject = "Work Allocation for leave application of " + applicantName,
                                    EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                    "Dear " + alternatePersonName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                    "A request for work allocation application has been assigned instead of " + applicantName + " for the date of " +
                                    "" + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate)
                                    .ToString("dd-MMM-yyyy") + " has been Approved. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                                    "<p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">Click Here</a></p>" +
                                    "<p style=\"font-family:tahoma; font-size:9pt;\">" + approvedByUserName + " &nbsp;(" + ApprovedBy + ")</p></body></html>",
                                    CreatedOn = DateTime.Now,
                                    CreatedBy = ApprovedBy,
                                    IsSent = false,
                                    SentOn = null,
                                    IsError = false,
                                    ErrorDescription = "-",
                                    SentCounter = 0
                                });
                            }
                        }
                    }
                }

                else if (ApprovedBy == approvalOwner2)
                {
                    //approve the application.
                    Obj.IsApproved = true;
                    if (approvalOwner1 == approvalOwner2)
                    {
                        AA.ApprovalStatusId = 2;
                        AA.ApprovedBy = ApprovedBy;
                        AA.ApprovedOn = DateTime.Now;
                    }
                    AA.Approval2statusId = 2;
                    AA.Approval2On = DateTime.Now;
                    AA.Approval2By = ApprovedBy;
                    AA.Approval2Owner = approvalOwner2; //added by rajesh 24 Nov 2021
                    AA.Comment = "APPROVED THE LEAVE REQUEST.";

                    if (Obj.LeaveTypeId != "LV0039")// LV0039-> BUSINESS TRAVEL 
                    {
                        //Deduct leave balance from employee leave account table.
                        ELA.StaffId = Obj.StaffId;
                        ELA.LeaveTypeId = Obj.LeaveTypeId;
                        ELA.TransactionFlag = 2;
                        ELA.TransactionDate = DateTime.Now;
                        ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays) * -1;
                        ELA.Narration = "Approved the leave application.";
                        ELA.LeaveCreditDebitReasonId = 22;
                        ELA.RefId = Obj.Id;
                    }
                    else
                    {
                        ELA = null;
                    }
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = ccEmailAddress,
                            BCC = string.Empty,
                            EmailSubject = "Requested leave application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your " + LeaveTypeName + "  application for the date " + Convert.ToDateTime(Obj.StartDate).
                            ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + " " +
                            " has been Approved.</p><p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">" +
                            "Click Here</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">" + approvedByUserName + " &nbsp;(" + ApprovedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = ApprovedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                        if (APA != null)
                        {
                            if (APA.AlternativeStaffId != null)
                            {
                                APA.IsApproved = true;
                                APA.ConfirmationMailSent = true;
                                if (string.IsNullOrEmpty(alternatePersonEmailId).Equals(false))
                                {
                                    ESL.Add(new EmailSendLog
                                    {
                                        From = commonSenderEmailId,
                                        To = alternatePersonEmailId,
                                        CC = string.Empty,
                                        BCC = string.Empty,
                                        EmailSubject = "Work allocation for leave application of " + applicantName,
                                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                            " font-size:9pt;\">Dear " + alternatePersonName + ",<br>Greetings</p>" +
                                            "<p style=\"font-family:tahoma; font-size:9pt;\">A request for work allocation application " +
                                            "has been assigned instead of " + applicantName + " for the date of " +
                                            Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " to " +
                                            Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + " has been approved. " +
                                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; " +
                                            "font-size:9pt;\"><a href=\"" + BaseAddress + "\">Click Here</a></p><p style=\"font-family:tahoma;" +
                                            " font-size:9pt;\">" + approvedByUserName + " &nbsp;(" + ApprovedBy + ")</p></body></html>",
                                        CreatedOn = DateTime.Now,
                                        CreatedBy = ApprovedBy,
                                        IsSent = false,
                                        SentOn = null,
                                        IsError = false,
                                        ErrorDescription = "-",
                                        SentCounter = 0
                                    });
                                }
                            }
                        }
                    }
                }
                CTS.RA = Obj;
                CTS.ELA = ELA;
                CTS.AA = AA;
                CTS.ESL = ESL;
                CTS.APA = APA;
                try
                {
                    using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
                    {
                        // rALeaveApplicationRepository.ApproveApplication(CTS);
                        rALeaveApplicationRepository.ApproveApplication(CTS);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("<<RALEAVE>> " + ex);
                }
            }
        }

        public void ApproveApplication(string Id, string ApprovedBy)
        {
            //Get the leave application details based on the Id passed to this function as a parameter.
            using (RALeaveApplicationRepository repo = new RALeaveApplicationRepository())
            { 
                var cm = new CommonRepository();
            ClassesToSave CTS = new ClassesToSave();
            EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id);
            string staffId = string.Empty;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            string applicationId = string.Empty;
            string applicantName = string.Empty;
            string applicationType = string.Empty;

            string applicantEmailId = string.Empty;
            string approvedBy = string.Empty;
            string approvedByUserEmailId = string.Empty;
            string approvedByUserName = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            bool isFinalLevelApproval = false;
            string leaveTypeName = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string approvalOwner2Name = string.Empty;
            string payPeriodValidationMessage = string.Empty;

            applicantEmailId = cm.GetEmailIdOfEmployee(Obj.StaffId);
            applicantName = cm.GetStaffName(Obj.StaffId);
            approvalOwner1 = cm.GetApproverOwner(Id);
            approvalOwner2 = cm.GetReviewerOwner(Id);
            approvedBy = ApprovedBy;
            approvedByUserName = cm.GetStaffName(approvedBy);
            approvedByUserEmailId = cm.GetEmailIdOfEmployee(approvedBy);
            commonSenderEmailId = cm.GetSenderEmailIdFromEmailSettings();
            approvalOwner2Name = cm.GetStaffName(approvalOwner2);

            leaveTypeName = cm.GetLeaveName(Obj.LeaveTypeId);
            if (Obj.AppliedBy == "")
            {
                Obj.AppliedBy = Obj.StaffId;
            }
            payPeriodValidationMessage = cm.ValidateApplicationForPayDate(Obj.StaffId, Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy"),
                   Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy"));
            if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
            {
                if (payPeriodValidationMessage.ToUpper() != "VALID")
                {
                    throw new ApplicationException("Application of past pay cycle cannot be approved");
                }
            }
            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true)) //if the leave application has been cancelled then...
            {
                throw new Exception("Cannot approve a cancelled leave application.");
            }
            else if (Obj.IsRejected.Equals(true))
            {
                throw new Exception("Cannot approve already rejected leave request.");
            }
            else
            {
                // If the application has approved by Reporting Manager or approval owner 1
                if (approvedBy.Equals(approvalOwner1) && approvedBy != approvalOwner2)
                {
                    //approve the application.
                    Obj.IsApproved = false;
                    AA.ApprovalStatusId = 2;
                    AA.ApprovedBy = approvedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "APPROVED THE LEAVE REQUEST.";
                    //Send intimation to Approval Owner2
                    if (string.IsNullOrEmpty(approvalOwner2).Equals(false) && string.IsNullOrEmpty(approvalOwner2EmailId)
                        .Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Request for Leave application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner2Name + "" +
                        ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + approvedByUserName + " has approved a Leave application of " + applicantName + " . Leave details are given below.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                        "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr>" +
                        "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee code:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Obj.StaffId + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">LeaveType:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + leaveTypeName + "</td></tr><tr>" +
                        "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">From Date:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
                        "</td></tr><tr><td style=\"width:20%;" + "font-family:tahoma; font-size:9pt;\"> To Date:</td><td style=\"width:80%;" +
                        "font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") +
                        "</td></tr><tr><td style=\"width:20%;font-family:tahoma;" +
                        " font-size:9pt;\">Total Days:</td><td style=\"width:80%;font-family:tahoma;" +
                        " font-size:9pt;\">" + Obj.TotalDays +
                        "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Obj.Remarks + "</td>" +
                        "</tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required " +
                        "to approve or reject this application.</p><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + approvedByUserName + " &nbsp;(" + ApprovedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = approvedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested leave application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your " + leaveTypeName + "  application for the date "
                            + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate)
                            .ToString("dd-MMM-yyyy") + " has been approved and sent for second level approval.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + approvedByUserName +
                            " &nbsp;(" + approvedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = approvedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                }
                // If the applicaation has approevd by Approval Owner2
                else if (approvedBy.Equals(approvalOwner2))
                {
                    //approve the application.
                    isFinalLevelApproval = true;
                    Obj.IsApproved = true;
                    if (approvalOwner1 == approvalOwner2)
                    {
                        AA.ApprovalStatusId = 2;
                        AA.ApprovedBy = approvedBy;

                        AA.ApprovedOn = DateTime.Now;
                    }
                    AA.Approval2statusId = 2;
                    AA.Approval2By = approvedBy;
                    AA.Approval2On = DateTime.Now;
                    AA.Comment = "APPROVED THE LEAVE REQUEST.";

                    //Deduct leave balance from employee leave account table.                            
                    ELA.StaffId = Obj.StaffId;
                    ELA.LeaveTypeId = Obj.LeaveTypeId;
                    ELA.TransactionFlag = 2;
                    ELA.TransactionDate = DateTime.Now;
                    ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays) * -1;
                    ELA.Narration = "Approved the leave application.";
                    ELA.LeaveCreditDebitReasonId = 22;
                    ELA.RefId = Obj.Id;
                    ELA.TransctionBy = approvedBy;
                    ELA.Month = Convert.ToDateTime(Obj.StartDate).Month;
                    ELA.Year = Convert.ToDateTime(Obj.StartDate).Year;

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested leave application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your " + leaveTypeName + "  application for the date "
                            + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate)
                            .ToString("dd-MMM-yyyy") + " has been approved.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + approvedByUserName +
                            " &nbsp;(" + approvedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = approvedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                }
                CTS.RA = Obj;
                CTS.ELA = ELA;
                CTS.AA = AA;
                CTS.ESL = ESL;
                repo.ApproveApplication(CTS, isFinalLevelApproval);
            }
            }
        }
        // To be removed as it integrated from Tagros.
        //public string CancelApplication(string Id, string CancelledBy, string LocationId)
        //{
        //    //Get the leave application details based on the Id passed to this function as a parameter.

        //    ClassesToSave CTS = new ClassesToSave();
        //    RequestApplication Obj = new RequestApplication();
        //    ApplicationApproval AA = new ApplicationApproval();
        //    AlternativePersonAssign APA = new AlternativePersonAssign();

        //    using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
        //    {

        //        Obj = rALeaveApplicationRepository.GetRequestApplicationDetails(Id);
        //        AA = rALeaveApplicationRepository.GetApplicationApproval(Id);
        //        APA = rALeaveApplicationRepository.GetApplicationForWorkAllocation(Id);
        //    }
        //    List<EmailSendLog> ESL = new List<EmailSendLog>();
        //    EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
        //    var CBL = new CommonBusinessLogic();
        //    //first send acknowledgement email to the user.
        //    string applicantName = string.Empty;
        //    string applicantEmailId = string.Empty;
        //    string approvalOwner1 = string.Empty;
        //    string approvalOwner2 = string.Empty;
        //    string approvalOwner1Name = string.Empty;
        //    string approvalOwner2Name = string.Empty;
        //    string approvalOwner1EmailId = string.Empty;
        //    string approvalOwner2EmailId = string.Empty;
        //    string cancelledByUserName = string.Empty;
        //    string cancelledByUserEmailId = string.Empty;
        //    string alternatePersonName = string.Empty;
        //    string alternatePersonEmailId = string.Empty;
        //    string commonSenderEmailId = string.Empty;
        //    string LeaveType = string.Empty;
        //    string ReviewerName = string.Empty;
        //    string payPeriodValidationMessage = string.Empty;

        //    approvalOwner1 = AA.ApprovalOwner;
        //    approvalOwner2 = AA.ReviewerOwner;
        //    using (CommonRepository commonRepository = new CommonRepository())
        //    {
        //        applicantName = commonRepository.GetStaffName(Obj.StaffId);
        //        applicantEmailId = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
        //        approvalOwner1Name = commonRepository.GetStaffName(approvalOwner1);
        //        approvalOwner2Name = commonRepository.GetStaffName(approvalOwner2);
        //        approvalOwner1EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner1);
        //        approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner2);
        //        cancelledByUserName = commonRepository.GetStaffName(CancelledBy);
        //        cancelledByUserEmailId = commonRepository.GetEmailIdOfEmployee(CancelledBy);
        //        commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
        //        LeaveType = commonRepository.GetLeaveName(Obj.LeaveTypeId);
        //    }
        //    payPeriodValidationMessage = CBL.ValidateApplicationForPayDate(Obj.StartDate, Obj.EndDate);
        //    if (payPeriodValidationMessage.ToUpper() != "VALID")
        //    {
        //        throw new Exception("Application of past pay cycle cannot be cancelled");
        //    }
        //    if (APA != null && string.IsNullOrEmpty(APA.AlternativeStaffId).Equals(false))
        //    {
        //        using (CommonRepository commonRepository = new CommonRepository())
        //        {
        //            alternatePersonName = commonRepository.GetStaffName(APA.AlternativeStaffId);
        //        }
        //    }
        //    //Check if the leave application has already been cancelled or not.
        //    if (Obj.IsCancelled.Equals(false) && Obj.IsApproverCancelled.Equals(false) && Obj.IsReviewerCancelled.Equals(false))
        //    //If the leave application has not been cancelled then...
        //    {
        //        //Cancel the leave application which is in pending state.
        //        if (CancelledBy == Obj.StaffId)
        //        {
        //            Obj.IsCancelled = true;
        //            Obj.CancelledDate = DateTime.Now;
        //            Obj.CancelledBy = CancelledBy;
        //        }

        //        else if (CancelledBy == approvalOwner1)
        //        {
        //            Obj.IsApproverCancelled = true;
        //            Obj.IsCancelled = true;
        //            Obj.CancelledDate = DateTime.Now;
        //            Obj.ApproverCancelledDate = DateTime.Now;
        //            Obj.ApproverCancelledBy = CancelledBy;
        //            Obj.CancelledBy = CancelledBy;
        //        }
        //        else if (CancelledBy == approvalOwner2)
        //        {
        //            Obj.IsReviewerCancelled = true;
        //            Obj.IsCancelled = true;
        //            Obj.CancelledDate = DateTime.Now;
        //            Obj.ReviewerCancelledDate = DateTime.Now;
        //            Obj.ReviewerCancelledBy = CancelledBy;
        //            Obj.CancelledBy = CancelledBy;
        //        }

        //        if (AA.ApprovalStatusId == 2 && AA.ReviewerstatusId == 2)
        //        {
        //            ELA.StaffId = Obj.StaffId;
        //            ELA.LeaveTypeId = Obj.LeaveTypeId;
        //            ELA.TransactionFlag = 1;
        //            ELA.TransactionDate = DateTime.Now;
        //            ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays);
        //            ELA.Narration = "Cancelled the Approval leave application.";
        //            ELA.RefId = Obj.Id;
        //            ELA.LeaveCreditDebitReasonId = 23;
        //        }
        //        else
        //        {
        //            ELA = null;
        //        }

        //        if (string.IsNullOrEmpty(applicantEmailId).Equals(false) && CancelledBy != Obj.StaffId)
        //        {
        //            ESL.Add(new EmailSendLog  //Send Mail to User
        //            {
        //                From = commonSenderEmailId,
        //                To = applicantEmailId,
        //                CC = string.Empty,
        //                BCC = string.Empty,
        //                EmailSubject = "Requested leave application status ",
        //                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
        //            "Dear " + applicantName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
        //            "Your " + LeaveType + "  application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
        //            " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + " has been cancelled. <p style=\"font-family:tahoma;" +
        //            " font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
        //            "(" + CancelledBy + ")</p></body></html>",
        //                CreatedOn = DateTime.Now,
        //                CreatedBy = Obj.AppliedBy,
        //                IsSent = false,
        //                SentOn = null,
        //                IsError = false,
        //                ErrorDescription = "-",
        //                SentCounter = 0
        //            });
        //        }

        //        if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false) && CancelledBy == Obj.StaffId)
        //        {
        //            ESL.Add(new EmailSendLog  //Send Mail to User
        //            {
        //                From = commonSenderEmailId,
        //                To = approvalOwner1EmailId,
        //                CC = string.Empty,
        //                BCC = string.Empty,
        //                EmailSubject = "Requested leave application status ",
        //                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
        //                "Dear " + approvalOwner1Name + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
        //                " " + applicantName + " has cancelled the " + LeaveType + "  application for the date from " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
        //                " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + " . <p style=\"font-family:tahoma;" +
        //                " font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
        //                "(" + CancelledBy + ")</p></body></html>",
        //                CreatedOn = DateTime.Now,
        //                CreatedBy = Obj.AppliedBy,
        //                IsSent = false,
        //                SentOn = null,
        //                IsError = false,
        //                ErrorDescription = "-",
        //                SentCounter = 0
        //            });
        //        }

        //        if (APA != null && string.IsNullOrEmpty(alternatePersonName).Equals(false))
        //        {
        //            ESL.Add(new EmailSendLog  //Send Mail to alternative Person
        //            {
        //                From = commonSenderEmailId,
        //                To = alternatePersonEmailId,
        //                CC = string.Empty,
        //                BCC = string.Empty,
        //                EmailSubject = "Work Allocation for leave application of " + alternatePersonName,
        //                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
        //                "Dear " + alternatePersonName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
        //                "A request for work allocation application has been assigned for " + applicantName + " on application for the date" +
        //                " " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate).
        //                ToString("dd-MMM-yyyy") + " has been cancelled. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
        //                "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;(" + CancelledBy + ")</p></body></html>",
        //                CreatedOn = DateTime.Now,
        //                CreatedBy = Obj.AppliedBy,
        //                IsSent = false,
        //                SentOn = null,
        //                IsError = false,
        //                ErrorDescription = "-",
        //                SentCounter = 0
        //            });
        //        }

        //        CTS.RA = Obj;
        //        CTS.ESL = ESL;
        //        CTS.APA = APA;
        //        CTS.ELA = ELA;

        //        using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
        //        {
        //            rALeaveApplicationRepository.CancelApplication(CTS);
        //        }
        //        return "OK";
        //    }
        //    else //If the leave application has already been cancelled then...
        //    {
        //        throw new Exception("You cannot cancel a leave request that is already been cancelled.");
        //    }
        //}


        public string CancelApplication(string Id, string CancelledBy)
        {
            //Get the leave application details based on the Id passed to this function as a parameter.
            using (RALeaveApplicationRepository repo = new RALeaveApplicationRepository())
            { 
                ClassesToSave CTS = new ClassesToSave();
            var cm = new CommonRepository();
            var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id);
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
            CommonBusinessLogic CBL = new CommonBusinessLogic();
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner1Name = string.Empty;
            string approvalOwner2Name = string.Empty;
            string applicantName = string.Empty;
            string leaveTypeName = string.Empty;
            string applicantEmailId = string.Empty;
            string cancelledByUserEmailId = string.Empty;
            string cancelledByUserName = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string payPeriodValidationMessage = string.Empty;

            approvalOwner1Name = cm.GetStaffName(AA.ApprovalOwner);
            approvalOwner2Name = cm.GetStaffName(AA.Approval2Owner);
            applicantName = cm.GetStaffName(Obj.StaffId);
            leaveTypeName = cm.GetLeaveName(Obj.LeaveTypeId);
            applicantEmailId = cm.GetEmailIdOfEmployee(Obj.StaffId);
            cancelledByUserEmailId = cm.GetEmailIdOfEmployee(CancelledBy);
            cancelledByUserName = cm.GetStaffName(CancelledBy);
            commonSenderEmailId = cm.GetSenderEmailIdFromEmailSettings();
            if (Obj.AppliedBy == "")
            {
                Obj.AppliedBy = Obj.StaffId;
            }
            if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
            {
                payPeriodValidationMessage = CBL.ValidateApplicationForPayDate(Obj.StaffId, Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy"),
                Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy"));
                if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
                {
                    if (payPeriodValidationMessage.ToUpper() != "VALID")
                    {
                        throw new ApplicationException("Application of past pay cycle cannot be cancelled");
                    }
                }

                //Cancel the leave application.
                Obj.IsCancelled = true;
                Obj.CancelledDate = DateTime.Now;
                Obj.CancelledBy = CancelledBy;
                if (AA.Approval2statusId == 2)
                {
                    ELA.StaffId = Obj.StaffId;
                    ELA.LeaveTypeId = Obj.LeaveTypeId;
                    ELA.TransactionFlag = 1;
                    ELA.TransactionDate = DateTime.Now;
                    ELA.LeaveCount = Obj.TotalDays;
                    ELA.Narration = "Cancelled the approved leave application.";
                    ELA.RefId = Obj.Id;
                    ELA.LeaveCreditDebitReasonId = 23;
                }
                else
                {
                    ELA = null;
                }
                if (CancelledBy == Obj.StaffId)
                {
                    if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner1
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested leave application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner1Name + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + leaveTypeName + "  application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate)
                            .ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") +
                            " has been cancelled. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                            "(" + CancelledBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = Obj.AppliedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                    if (string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner2
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested leave application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner2Name + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + leaveTypeName + "  application of " + applicantName + " for the date from " + Convert.ToDateTime(Obj.StartDate)
                            .ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") +
                            " has been cancelled. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                            "(" + CancelledBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = Obj.AppliedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                }
                else if (CancelledBy != Obj.StaffId)
                {
                    if (CancelledBy == AA.ApprovalOwner && AA.ApprovalStatusId == 2 && string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner2
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested leave application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner2Name + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + leaveTypeName + "  application of " + applicantName + " for the date from " + Convert.ToDateTime(Obj.StartDate)
                            .ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") +
                            " has been cancelled. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                            "(" + CancelledBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = Obj.AppliedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                    else if (CancelledBy == AA.Approval2Owner && string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner1
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested leave application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner1Name + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + leaveTypeName + " application of " + applicantName + " for the date from " + Convert.ToDateTime(Obj.StartDate)
                            .ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") +
                            " has been cancelled. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                            "(" + CancelledBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = Obj.AppliedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to User
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested leave application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your " + leaveTypeName + "  application for the date from " + Convert.ToDateTime(Obj.StartDate)
                            .ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") +
                            " has been cancelled. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                            "(" + CancelledBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = Obj.AppliedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                }
                CTS.RA = Obj;
                CTS.AA = AA;
                CTS.ESL = ESL;
                CTS.ELA = ELA;
                repo.CancelApplication(CTS, CancelledBy);
            }
            else   //If the leave application has already been cancelled then...
            {
                throw new Exception("You cannot cancel a leave request that is already been cancelled.");
            }
            return "OK";
        }
        }







        public decimal GetLeaveBalance(string StaffId, string LeaveTypeId)
        {
            using (RALeaveApplicationRepository BL = new RALeaveApplicationRepository())
                return BL.GetLeaveBalance(StaffId, LeaveTypeId);
        }

        public void ValidateLeaveBalance(string StaffId, string LeaveTypeId, decimal TotalDays)
        {
            var LeaveBalance = GetLeaveBalance(StaffId, LeaveTypeId);

            if (LeaveBalance < Convert.ToDecimal(TotalDays))
            {
                throw new Exception("There is no sufficient balance in your specified leave account.");
            }
        }

        public void MustBeSameDurationWhenSameDate(DateTime? StartDate, DateTime? EndDate, int LeaveStartDurationId, int LeaveEndDurationId)
        {
            if (StartDate.Value.Date == EndDate.Value.Date)
            {
                if (LeaveStartDurationId != LeaveEndDurationId)
                {
                    throw new Exception("Please select same duration when the dates are same.");
                }
            }
        }
        public string SaveCompensatoryWorkingBusinessLogic(CompensatoryWorkingModel model)
        {
            using (RALeaveApplicationRepository repoObj = new RALeaveApplicationRepository())
                return repoObj.SaveCompensatoryWorkingRepository(model);
        }
        public string NewValidateLeavePrefixSuffix(string StaffId, DateTime LeaveStartDate, DateTime LeaveEndDate, string FromDuration, string ToDuration, string LeaveTypeId, decimal TotalDays)
        {
            using (RALeaveApplicationRepository repo = new RALeaveApplicationRepository())
            { 
                var str = repo.NewValidateLeavePrefixSuffix(StaffId, LeaveStartDate, LeaveEndDate, FromDuration, ToDuration, LeaveTypeId, TotalDays);
            if (!str.ToUpper().StartsWith("OK"))
            {
                throw new Exception(str);
            }
            return str;
            }
        }
    }
}

