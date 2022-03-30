using Attendance.Model;
using Attendance.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Attendance.BusinessLogic
{
    public class RACoffRequestApplicationBusinessLogic
    {
        public List<RACoffRequestApplication> GetAppliedCoffRequest(string StaffId)
        {
            using (RACoffRequestApplicationRepository rACoffRequestApplicationRepository = new RACoffRequestApplicationRepository())
            {
                return rACoffRequestApplicationRepository.GetAppliedCoffRequest(StaffId);
            }
        }

        public LeaveTypeAndBalance GetLeaveTypeAndBalance(string staffid)
        {
            using (RACoffRequestApplicationRepository rACoffRequestApplicationRepository = new RACoffRequestApplicationRepository())
            {
                return rACoffRequestApplicationRepository.GetLeaveTypeAndBalance(staffid);
            }
        }

        public string GetUniqueId()
        {
            using (RACoffRequestApplicationRepository rACoffRequestApplicationRepository = new RACoffRequestApplicationRepository())
            {
                return rACoffRequestApplicationRepository.GetUniqueId();
            }
        }


        public void SaveRequestApplication(ClassesToSave DataToSave, string SecurityGroupId)
        {
            
            List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
            EmployeeLeaveAccount employeeLeaveAccount = new EmployeeLeaveAccount();
            try
            {
                string overlappingValidationMessage = string.Empty;
                string payPeriodValidationMessage = string.Empty;
                using (CommonRepository commonRepository = new CommonRepository())
                {
                    overlappingValidationMessage = commonRepository.ValidateApplicationOverlaping(DataToSave.RA.StaffId, DataToSave.RA.StartDate,

                    DataToSave.RA.LeaveStartDurationId, DataToSave.RA.EndDate, DataToSave.RA.LeaveEndDurationId);
                }
                if (string.IsNullOrEmpty(overlappingValidationMessage).Equals(false) && overlappingValidationMessage != "Ok")
                {
                    throw new ApplicationException(overlappingValidationMessage);
                }
                using (CommonRepository commonRepository = new CommonRepository())
                {
                    payPeriodValidationMessage = commonRepository.ValidateApplicationForPayDate(DataToSave.RA.StartDate, DataToSave.RA.EndDate);
                }
                    if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false) && payPeriodValidationMessage != "VALID")
                {
                    throw new ApplicationException("Application of past pay cycle cannot be saved ");
                }
                string BaseAddress = string.Empty;
                bool isFinalLevelApproval = false;
            string LeaveTypeName = string.Empty;
                string applicantEmailId = string.Empty;
                string applicantName = string.Empty;
                string approvalOwner1Name = string.Empty;
                string approvalOwner1EmailId = string.Empty;
                string approvalOwner2Name = string.Empty;
                string approvalOwner2EmailId = string.Empty;
                string appliedByUserName = string.Empty;
                string appliedByUserEmailId = string.Empty;
                string senderEmailId = string.Empty;
                string commonSenderEmailId = string.Empty;
                string alternatePersonName = string.Empty;
                string alternatePersonEmailId = string.Empty;
                string ccEmailAddress = string.Empty;
                using (CommonRepository commonRepository = new CommonRepository())
                {
                    applicantName = commonRepository.GetStaffName(DataToSave.RA.StaffId);
                    applicantEmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.RA.StaffId);
                    approvalOwner1Name = commonRepository.GetStaffName(DataToSave.AA.ApprovalOwner);
                    approvalOwner1EmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.AA.ApprovalOwner);
                    approvalOwner2Name = commonRepository.GetStaffName(DataToSave.AA.ReviewerOwner);
                    approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.AA.ReviewerOwner);
                    appliedByUserName = commonRepository.GetStaffName(DataToSave.RA.AppliedBy);
                    appliedByUserEmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.RA.AppliedBy);
                    commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
                }
                senderEmailId = commonSenderEmailId;
                if (DataToSave.RA.AppliedBy.Equals(DataToSave.RA.StaffId))
                {
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        //Send acknowledgement email to the user.
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Request for Comp-Off availing application sent to " + approvalOwner1Name,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + applicantName + ",<br>Greetings</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your request for Comp-Off availing" +
                            " application has been sent to " + approvalOwner1Name + " for approval. </p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\">" +
                            "<tr><td style=\"width:20%;\">StaffId:</td><td style=\"width:80%;\">" + DataToSave.RA.StaffId + "</td>" +
                            "</tr><tr><td style=\"width:20%;\">Worked Date:</td><td style=\"width:80%;\">" +
                            "" + Convert.ToDateTime(DataToSave.RA.WorkedDate).ToString("dd-MMM-yyyy") + "</td></tr></tr>" +
                            "<tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" +
                            "" + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;\">" + "To Date:</td><td style=\"width:80%;\">"
                            + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + DataToSave.RA.Remarks +
                            "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = DataToSave.RA.AppliedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                    if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                    {
                        //Send intimation email to the reporting manager.
                        emailSendLogs.Add(new EmailSendLog
            {
                            From = applicantEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Request for Comp-Off availing application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + approvalOwner1Name + ",<br>Greetings</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">A request for Comp-Off availing" +
                            " application has been received from " + applicantName + ". Application details are given below.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                            "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">" +
                            "StaffId:</td><td style=\"width:80%;\">" + DataToSave.RA.StaffId + "</td></tr><tr>" +
                            "<td style=\"width:20%;\">Worked Date:</td><td style=\"width:80%;\">" +
                            "" + Convert.ToDateTime(DataToSave.RA.WorkedDate).ToString("dd-MMM-yyyy") + "</td></tr><tr>" +
                            "<td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(DataToSave.RA.StartDate)
                            .ToString("dd-MMM-yyyy") + "</td></tr>" + "<tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" +
                            "" + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + DataToSave.RA.Remarks + "</td></tr>" +
                            "</table></p><p>Your attention is required to approve or reject this application.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + applicantName + "&nbsp;" +
                            "(" + DataToSave.RA.StaffId + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = DataToSave.RA.AppliedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                }
                    DataToSave.ESL = emailSendLogs;
            }

                // If the application has applied by Reporting Manager or Approval Owner1
                else if (DataToSave.RA.AppliedBy.Equals(DataToSave.AA.ApprovalOwner) &&
                DataToSave.RA.AppliedBy != DataToSave.AA.ReviewerOwner)
                {
                    DataToSave.RA.IsApproved = true;
                DataToSave.RA.IsReviewed = false;
                    DataToSave.AA.ApprovalStatusId = 2;
                    DataToSave.AA.ReviewerstatusId = 1;
                    DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedOn = DateTime.Now;
                DataToSave.AA.Comment = "APPROVED THE C-OFF AVAILING REQUEST.";
                    senderEmailId = appliedByUserEmailId;

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                        {

                            From = senderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Comp-Off availing application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your Comp-Off availng" +
                            " requisition from " + Convert.ToDateTime(DataToSave.RA.StartDate)
                            .ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") +
                            " has been applied and approved by " + appliedByUserName + "" +
                            " (" + DataToSave.RA.AppliedBy + ") and send for second level approval.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + appliedByUserName + "&nbsp;" +
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
                    if (string.IsNullOrEmpty(DataToSave.AA.ReviewerOwner).Equals(false) &&
                        string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                {
                        emailSendLogs.Add(new EmailSendLog
                    {
                            From = senderEmailId,
                            To = approvalOwner2EmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                            EmailSubject = "Comp-Off availing application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + approvalOwner2Name + ",<br>Greetings</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">A request for Comp-Off availing" +
                            " application of " + applicantName + " has been approved by " + appliedByUserName + ". Application details are given below.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                            "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">" +
                            "StaffId:</td><td style=\"width:80%;\">" + DataToSave.RA.StaffId + "</td></tr><tr><td style=\"width:20%;\">Worked Date:</td>" +
                            "<td style=\"width:80%;\">" + "" + Convert.ToDateTime(DataToSave.RA.WorkedDate).ToString("dd-MMM-yyyy") + "</td></tr><tr>" +
                            "<td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + DataToSave.RA.Remarks + "</td></tr>" +
                            "</table></p><p>Your attention is required to approve or reject this application.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + applicantName + "&nbsp;" +
                            "(" + DataToSave.RA.StaffId + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = DataToSave.RA.AppliedBy,
                        IsSent = false,
                            SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                    DataToSave.ESL = emailSendLogs;
            }

                // If the application has applied by Approval Owner2
            else if (DataToSave.RA.AppliedBy.Equals(DataToSave.AA.ReviewerOwner))
            {
                isFinalLevelApproval = true;
                DataToSave.RA.IsApproved = true;
                DataToSave.RA.IsReviewed = true;
                DataToSave.AA.ApprovalStatusId = 2;
                DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                DataToSave.AA.ApprovedOn = DateTime.Now;
                DataToSave.AA.ReviewerstatusId = 2;
                DataToSave.AA.ReviewedBy = DataToSave.RA.AppliedBy;
                DataToSave.AA.ReviewedOn = DateTime.Now;
                    DataToSave.AA.Comment = "APPROVED THE C-OFF AVAILNG REQUEST.";

                    //deduct leave balance from employee leave account table.

                    employeeLeaveAccount.StaffId = DataToSave.RA.StaffId;
                    employeeLeaveAccount.LeaveTypeId = DataToSave.RA.LeaveTypeId;
                    employeeLeaveAccount.TransactionFlag = 2;
                    employeeLeaveAccount.TransactionDate = DateTime.Now;
                    employeeLeaveAccount.LeaveCount = Convert.ToDecimal(DataToSave.RA.TotalDays) * -1;
                    employeeLeaveAccount.LeaveCreditDebitReasonId = 22;
                    employeeLeaveAccount.Narration = "Approved the comp-off availing application.";
                    employeeLeaveAccount.RefId = DataToSave.RA.Id;
                    employeeLeaveAccount.WorkedDate = DataToSave.RA.WorkedDate;
                    employeeLeaveAccount.TransctionBy = DataToSave.RA.AppliedBy;

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                        emailSendLogs.Add(new EmailSendLog
                    {
                            From = senderEmailId,
                            To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                            EmailSubject = "Requested Comp-Off availing application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your Comp-Off availing " +
                            "requisition from " + Convert.ToDateTime(DataToSave.RA.StartDate)
                            .ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + " has been applied" +
                            " and approved by " + appliedByUserName + "" + " (" + DataToSave.RA.AppliedBy + ").</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + appliedByUserName + "&nbsp;" +
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
                    DataToSave.ESL = emailSendLogs;
                    DataToSave.ELA = employeeLeaveAccount;
            }

                else if (DataToSave.RA.AppliedBy != DataToSave.RA.StaffId && DataToSave.RA.AppliedBy != DataToSave.AA.ApprovalOwner
                    && (SecurityGroupId.Equals("3") || SecurityGroupId.Equals("5") || SecurityGroupId.Equals("6")))
            {
                isFinalLevelApproval = true;
                DataToSave.RA.IsApproved = true;
                DataToSave.RA.IsReviewed = true;
                DataToSave.AA.ApprovalStatusId = 2;
                DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                DataToSave.AA.ApprovedOn = DateTime.Now;
                DataToSave.AA.ReviewerstatusId = 2;
                DataToSave.AA.ReviewedBy = DataToSave.RA.AppliedBy;
                DataToSave.AA.ReviewedOn = DateTime.Now;
                DataToSave.AA.Comment = "APPROVED THE C-OFF AVAILING REQUEST.";

                    // deduct leave balance from employee leave account table.
                    employeeLeaveAccount.StaffId = DataToSave.RA.StaffId;
                    employeeLeaveAccount.LeaveTypeId = DataToSave.RA.LeaveTypeId;
                    employeeLeaveAccount.TransactionFlag = 2;
                    employeeLeaveAccount.TransactionDate = DateTime.Now;
                    employeeLeaveAccount.LeaveCount = Convert.ToDecimal(DataToSave.RA.TotalDays) * -1;
                    employeeLeaveAccount.LeaveCreditDebitReasonId = 22;
                    employeeLeaveAccount.Narration = "Approved comp-off availing application.";
                    employeeLeaveAccount.RefId = DataToSave.RA.Id;
                    employeeLeaveAccount.WorkedDate = DataToSave.RA.WorkedDate;
                    employeeLeaveAccount.TransctionBy = DataToSave.RA.AppliedBy;
                    DataToSave.ELA = employeeLeaveAccount;

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                        emailSendLogs.Add(new EmailSendLog
                    {
                            From = senderEmailId,
                            To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                            EmailSubject = "Requested Comp-Off availing application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your Comp-Off availing " +
                            "requisition from " + Convert.ToDateTime(DataToSave.RA.StartDate)
                            .ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + " has been applied" +
                            " and approved by " + appliedByUserName + "" + " (" + DataToSave.RA.AppliedBy + ").</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + appliedByUserName + "&nbsp;" +
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
            }
                using (RACoffRequestApplicationRepository rACoffRequestApplicationRepository = new RACoffRequestApplicationRepository())
                {
                    rACoffRequestApplicationRepository.SaveRequestApplication(DataToSave, isFinalLevelApproval);
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public void RejectApplication(string id, string rejectedBy)
        {
            //Get the Comp-Off application details based on the id passed to this function as a parameter.
            RequestApplication requestApplication = new RequestApplication();
            ApplicationApproval applicationApproval = new ApplicationApproval();

            using (RACoffRequestApplicationRepository rACoffRequestApplicationRepository = new RACoffRequestApplicationRepository())
            {
                requestApplication = rACoffRequestApplicationRepository.GetRequestApplicationDetails(id);
                applicationApproval = rACoffRequestApplicationRepository.GetApplicationApproval(id);
            }

            List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
            ClassesToSave classesToSave = new ClassesToSave();

            string rejectedByUserName = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string rejectedByUserEmailId = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner2Name = string.Empty;
            string approvalOwner2EmalId = string.Empty;
            using (CommonRepository commonRepository = new CommonRepository())
            {
                applicantName = commonRepository.GetStaffName(requestApplication.StaffId);
                applicantEmailId = commonRepository.GetEmailIdOfEmployee(requestApplication.StaffId);
                approvalOwner1 = commonRepository.GetApproverOwner(id);
                approvalOwner2 = commonRepository.GetReviewerOwner(id);
                rejectedByUserEmailId = commonRepository.GetEmailIdOfEmployee(rejectedBy);
                rejectedByUserName = commonRepository.GetStaffName(rejectedBy);
                commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
                approvalOwner2Name = commonRepository.GetStaffName(approvalOwner2);
                approvalOwner2EmalId = commonRepository.GetEmailIdOfEmployee(approvalOwner2);
            }

            //Check if the Comp-Off application has been cancelled or not.
            if (requestApplication.IsCancelled.Equals(true))    //if the Comp-Off application has been cancelled then...
            {
                throw new ApplicationException("Cancelled Comp-Off availing application cannot be rejected.");
            }
            else if (requestApplication.IsRejected.Equals(true))  //if the Comp-Off application has been rejected then...
            {
                throw new ApplicationException("Rejected Comp-Off availing cannot be rejected.");
            }
            else //if the Comp-Off application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {
                if (rejectedBy == approvalOwner1 && rejectedBy != approvalOwner2)
                {
                    requestApplication.IsRejected = true;
                    applicationApproval.ApprovalStatusId = 3;
                    applicationApproval.ApprovedBy = rejectedBy;
                    applicationApproval.ApprovedOn = DateTime.Now;
                    applicationApproval.Comment = "COFF AVAILING HAS BEEN REJECTED BY THE REPORTING MANAGER.";
        }
                else if (rejectedBy == approvalOwner2)
        {
                    requestApplication.IsRejected = true;
                    if (approvalOwner1 == approvalOwner2)
            {
                        applicationApproval.ApprovalStatusId = 3;
                        applicationApproval.ApprovedBy = rejectedBy;
                        applicationApproval.ApprovedOn = DateTime.Now;
                    }
                    applicationApproval.ReviewerstatusId = 3;
                    applicationApproval.ReviewedOn = DateTime.Now;
                    applicationApproval.ReviewedBy = rejectedBy;
                    applicationApproval.Comment = "C-OFF AVAILLING HAS BEEN REJECTED BY THE APPROVAL OWNER2.";
            }
                senderEmailId = commonSenderEmailId;
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
            {
                    emailSendLogs.Add(new EmailSendLog
            {
                        From = senderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Comp-Off availing application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your Compensatory Off availing requisition from " + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy")
                        + " to " + Convert.ToDateTime(requestApplication.EndDate).ToString("dd-MMM-yyyy") + " has been rejected by " + rejectedByUserName + " (" + rejectedBy + ").</p><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + rejectedByUserName + "&nbsp;" +
                                "(" + rejectedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = rejectedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
            }
                classesToSave.RA = requestApplication;
                classesToSave.AA = applicationApproval;
                classesToSave.ESL = emailSendLogs;
                using (RACoffRequestApplicationRepository rACoffRequestApplicationRepository = new RACoffRequestApplicationRepository())
            {
                    rACoffRequestApplicationRepository.RejectApplication(classesToSave);
                    }
                }
            }

        public void ApproveApplication(string id, string approvedBy)
        {
            //Get the Comp-Off application details based on the Id passed to this function as a parameter.
            RequestApplication requestApplication = new RequestApplication();
            ApplicationApproval applicationApproval = new ApplicationApproval();

            using (RACoffRequestApplicationRepository rACoffRequestApplicationRepository = new RACoffRequestApplicationRepository())
                        {
                requestApplication = rACoffRequestApplicationRepository.GetRequestApplicationDetails(id);
                applicationApproval = rACoffRequestApplicationRepository.GetApplicationApproval(id);
        }

            List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
            ClassesToSave classesToSave = new ClassesToSave();
            EmployeeLeaveAccount employeeLeaveAccount = new EmployeeLeaveAccount();
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string approvedByUserName = string.Empty;
            string LeaveTypeName = string.Empty;
            string ApprovalOwner1 = string.Empty;
            string ApprovalOwner2 = string.Empty;
            string approvedByUserEmailId = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string approvalOwner2Name = string.Empty;
            bool isFinalLevelApproval = false;
            using (CommonRepository commonRepository = new CommonRepository())
                        {
                applicantName = commonRepository.GetStaffName(requestApplication.StaffId);
                applicantEmailId = commonRepository.GetEmailIdOfEmployee(requestApplication.StaffId);
                approvedByUserName = commonRepository.GetStaffName(approvedBy);
                approvedByUserEmailId = commonRepository.GetEmailIdOfEmployee(approvedBy);
                commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
                ApprovalOwner1 = commonRepository.GetApproverOwner(id);
                ApprovalOwner2 = commonRepository.GetReviewerOwner(id);
                approvalOwner2Name = commonRepository.GetStaffName(ApprovalOwner2);
                approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(ApprovalOwner2);
                        }

            //Check if the Comp-Off application has been cancelled or not.
            if (requestApplication.IsCancelled.Equals(true)) //if the Comp-Off application has been cancelled then...
                        {
                throw new ApplicationException("Cannot approve a cancelled C-Off availing application.");
                        }
            else if (requestApplication.IsRejected.Equals(true))
                        {
                throw new ApplicationException("Cannot approve already rejected C-Off availing request.");
                        }
            else
                        {
                if (approvedBy.Equals(ApprovalOwner1) && approvedBy != ApprovalOwner2)
                {
                    //Approve the application and forward the same to second level approval.
                    requestApplication.IsApproved = true;
                    applicationApproval.ApprovalStatusId = 2;
                    applicationApproval.ApprovedBy = approvedBy;
                    applicationApproval.ApprovedOn = DateTime.Now;
                    applicationApproval.Comment = "APPROVED THE COMP-OFF AVAILING.";

                    senderEmailId = commonSenderEmailId;
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                            {
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = senderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Comp-Off availing application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your Compensatory Off availing requisition from " + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy")
                            + " to " + Convert.ToDateTime(requestApplication.EndDate).ToString("dd-MMM-yyyy") + " has been approved by " +
                            "" + approvedByUserName + " (" + approvedBy + ") and send for second level approval.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + approvedByUserName + " &nbsp;" + "(" + approvedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = approvedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                            }
                    if (string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = senderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Comp-Off availing application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + approvalOwner2Name + ",<br>Greetings</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">A request for Comp-Off availing" +
                            " application of " + applicantName + " has been approved by " + approvedByUserName + ". Application details are given below.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                            "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">" +
                            "StaffId:</td><td style=\"width:80%;\">" + requestApplication.StaffId + "</td></tr><tr><td style=\"width:20%;\">Worked Date:</td>" +
                            "<td style=\"width:80%;\">" + "" + Convert.ToDateTime(requestApplication.WorkedDate).ToString("dd-MMM-yyyy") + "</td></tr><tr>" +
                            "<td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(requestApplication.EndDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + requestApplication.Remarks + "</td></tr>" +
                            "</table></p><p>Your attention is required to approve or reject this application.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + approvedByUserName + "&nbsp;" +
                            "(" + approvedBy + ")</p></body></html>",
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
                else if (approvedBy.Equals(ApprovalOwner2))
                    {
                    //Approve the application.
                    isFinalLevelApproval = true;
                    requestApplication.IsApproved = true;
                    requestApplication.IsReviewed = true;
                    if (ApprovalOwner2 == ApprovalOwner1)
                    {
                        applicationApproval.ApprovalStatusId = 2;
                        applicationApproval.ApprovedBy = approvedBy;
                        applicationApproval.ApprovedOn = DateTime.Now;
                    }
                    applicationApproval.ReviewerstatusId = 2;
                    applicationApproval.ReviewedBy = approvedBy;
                    applicationApproval.ReviewedOn = DateTime.Now;
                    applicationApproval.Comment = "APPROVED THE COMP-OFF AVAILING.";

                    //Deduct comp-off balance from employee leave account .
                    employeeLeaveAccount.StaffId = requestApplication.StaffId;
                    employeeLeaveAccount.LeaveTypeId = requestApplication.LeaveTypeId;
                    employeeLeaveAccount.TransactionFlag = 2;
                    employeeLeaveAccount.TransactionDate = DateTime.Now;
                    employeeLeaveAccount.TransctionBy = approvedBy;
                    employeeLeaveAccount.LeaveCount = Convert.ToDecimal(requestApplication.TotalDays) * -1;
                    employeeLeaveAccount.Narration = "Approved the comp-off availing application.";
                    employeeLeaveAccount.LeaveCreditDebitReasonId = 22;
                    employeeLeaveAccount.RefId = requestApplication.Id;
                    employeeLeaveAccount.WorkedDate = requestApplication.WorkedDate;
                    employeeLeaveAccount.TransctionBy = approvedBy;
                    employeeLeaveAccount.Month = Convert.ToDateTime(requestApplication.StartDate).Month;
                    employeeLeaveAccount.Year = Convert.ToDateTime(requestApplication.StartDate).Year;
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                        emailSendLogs.Add(new EmailSendLog
                    {
                            From = senderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Comp-Off availing application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your Compensatory Off availing requisition from " + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy")
                            + " to " + Convert.ToDateTime(requestApplication.EndDate).ToString("dd-MMM-yyyy") + " has been approved by " + approvedByUserName + " (" + approvedBy + ").</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + approvedByUserName + " &nbsp;" + "(" + approvedBy + ")</p></body></html>",
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
                classesToSave.RA = requestApplication;
                classesToSave.ELA = employeeLeaveAccount;
                classesToSave.AA = applicationApproval;
                classesToSave.ESL = emailSendLogs;
                using (RACoffRequestApplicationRepository rACoffRequestApplicationRepository = new RACoffRequestApplicationRepository())
                    {
                    rACoffRequestApplicationRepository.ApproveApplication(classesToSave, isFinalLevelApproval);
                    }
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

        #region Coff Req Availing
        public List<SelectListItem> GetDurationListBusinessLogic()
        {
            List<LeaveDuration> leaveDurations = new List<LeaveDuration>();
            using (RACoffRequestApplicationRepository rACoffRequestApplicationRepository = new RACoffRequestApplicationRepository())
            {
                leaveDurations = rACoffRequestApplicationRepository.GetDurationListRepository();
            }

            var item = new List<SelectListItem>();

            item = leaveDurations.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString(),
                Selected = false
            }).ToList();

            return item;
        }
        public List<RACoffAvailingRequestApplication> RenderAppliedCompAvailingList(string StaffId, string AppliedBy, string userRole)
        {
            using (RACoffRequestApplicationRepository rACoffRequestApplicationRepository = new RACoffRequestApplicationRepository())
            {
                return rACoffRequestApplicationRepository.RenderAppliedCompAvailingList(StaffId, AppliedBy, userRole);
            }
        }

        public string CancelCoffAvaillingApplication(string Id, string cancelledBy)
        {
            //Get the Comp-Off application details based on the Id passed to this function as a parameter.


            RequestApplication requestApplication = new RequestApplication();
            ApplicationApproval applicationApproval = new ApplicationApproval();

            using (RACoffRequestApplicationRepository rACoffRequestApplicationRepository = new RACoffRequestApplicationRepository())
            {
                requestApplication = rACoffRequestApplicationRepository.GetRequestApplicationDetails(Id);
                applicationApproval = rACoffRequestApplicationRepository.GetApplicationApproval(Id);
            }
            ClassesToSave classesToSave = new ClassesToSave();
            
            
            List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
            EmployeeLeaveAccount employeeLeaveAccount = new EmployeeLeaveAccount();
            CommonBusinessLogic commonBusinessLogic = new CommonBusinessLogic();

            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner1Name = string.Empty;
            string approvalOwner2Name = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string cancelledByUserName = string.Empty;
            string cancelledByUserEmailId = string.Empty;
            string alternatePersonName = string.Empty;
            string alternatePersonEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string senderEmailId = string.Empty;
            string payPeriodValidationMessage = string.Empty;

            approvalOwner1 = applicationApproval.ApprovalOwner;
            approvalOwner2 = applicationApproval.ReviewerOwner;
            using (CommonRepository commonRepository = new CommonRepository())
            {
                applicantName = commonRepository.GetStaffName(requestApplication.StaffId);
                applicantEmailId = commonRepository.GetEmailIdOfEmployee(requestApplication.StaffId);
                approvalOwner1Name = commonRepository.GetStaffName(approvalOwner1);
                approvalOwner2Name = commonRepository.GetStaffName(approvalOwner2);
                approvalOwner1EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner1);
                approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner2);
                cancelledByUserName = commonRepository.GetStaffName(cancelledBy);
                cancelledByUserEmailId = commonRepository.GetEmailIdOfEmployee(cancelledBy);
                commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
            }
            senderEmailId = commonSenderEmailId;
            payPeriodValidationMessage = commonBusinessLogic.ValidateApplicationForPayDate(requestApplication.StartDate, requestApplication.EndDate);
            if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
            {

            }
            if (payPeriodValidationMessage.ToUpper() != "VALID")
            {
                throw new ApplicationException("Application of past pay cycle cannot be cancelled");
            }
            //Check if the Comp-Off application has already been cancelled or not.
            if (requestApplication.IsCancelled.Equals(false) && requestApplication.IsApproverCancelled.Equals(false) && requestApplication.IsReviewerCancelled.Equals(false))
            //If the Comp-Off application has not been cancelled then...
            {
                if (cancelledBy == requestApplication.StaffId)
            {
                    requestApplication.IsCancelled = true;
                    requestApplication.CancelledDate = DateTime.Now;
                    requestApplication.CancelledBy = cancelledBy;
                }

                else if (cancelledBy == approvalOwner1)
                {
                    requestApplication.IsCancelled = true;
                    requestApplication.CancelledDate = DateTime.Now;
                    requestApplication.CancelledBy = cancelledBy;
                    requestApplication.IsApproverCancelled = true;
                    requestApplication.ApproverCancelledDate = DateTime.Now;
                    requestApplication.ApproverCancelledBy = cancelledBy;

                }
                else if (cancelledBy == approvalOwner2)
                {
                    requestApplication.IsCancelled = true;
                    requestApplication.CancelledBy = cancelledBy;
                    requestApplication.CancelledDate = DateTime.Now;
                    requestApplication.IsReviewerCancelled = true;
                    requestApplication.ReviewerCancelledDate = DateTime.Now;
                    requestApplication.ReviewerCancelledBy = cancelledBy;
                }

                if (applicationApproval.ApprovalStatusId == 2 && applicationApproval.ReviewerstatusId == 2)
                {
                    employeeLeaveAccount.StaffId = requestApplication.StaffId;
                    employeeLeaveAccount.LeaveTypeId = requestApplication.LeaveTypeId;
                    employeeLeaveAccount.TransactionFlag = 1;
                    employeeLeaveAccount.TransactionDate = DateTime.Now;
                    employeeLeaveAccount.LeaveCount = Convert.ToDecimal(requestApplication.TotalDays);
                    employeeLeaveAccount.Narration = "Cancelled the approved Comp-Off application.";
                    employeeLeaveAccount.RefId = requestApplication.Id;
                    employeeLeaveAccount.LeaveCreditDebitReasonId = 23;
                }

                if (cancelledBy == requestApplication.StaffId)
                {
                    requestApplication.IsCancelled = true;
                    requestApplication.CancelledDate = DateTime.Now;
                    requestApplication.CancelledBy = cancelledBy;
                    if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog  //Send email to Approval Owner1
            {
                            From = senderEmailId,
                            To = approvalOwner1EmailId,
                CC = string.Empty,
                BCC = string.Empty,
                            EmailSubject = "Requested Comp-Off availing application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"> Comp-Off availing application of " + applicantName + "" +
                            " for the date " + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">" + cancelledByUserName + " &nbsp;(" + cancelledBy + ")</p></body></html>",
                CreatedOn = DateTime.Now,
                            CreatedBy = cancelledBy,
                IsSent = false,
                            SentOn = null,
                IsError = false,
                ErrorDescription = "-",
                SentCounter = 0
            });
                    }
                    if (applicationApproval.ApprovalStatusId == 2 && applicationApproval.ApprovalOwner != applicationApproval.ReviewerOwner &&
                        string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
            {
                        emailSendLogs.Add(new EmailSendLog  //Send email to Approval Owner2
                {
                            From = senderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested Comp-Off availing application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"> Comp-Off availing application of " + applicantName +
                            " for the date " + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">" + cancelledByUserName + " &nbsp;(" + cancelledBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = cancelledBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                }
                else if (cancelledBy != requestApplication.StaffId)
                    {
                    if (cancelledBy == approvalOwner1)
                    {
                        requestApplication.IsCancelled = true;
                        requestApplication.CancelledDate = DateTime.Now;
                        requestApplication.CancelledBy = cancelledBy;
                        requestApplication.IsApproverCancelled = true;
                        requestApplication.ApproverCancelledDate = DateTime.Now;
                        requestApplication.ApproverCancelledBy = cancelledBy;

                        if (requestApplication.IsApproved == true && string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                            emailSendLogs.Add(new EmailSendLog  //Send email to Approval Owner2
                    {
                                From = senderEmailId,
                                To = approvalOwner2EmailId,
                                CC = string.Empty,
                                BCC = string.Empty,
                                EmailSubject = "Requested Comp-Off availing application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\"> Comp-Off availing application of " + applicantName +
                                " for the dat " + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">" + cancelledByUserName + " &nbsp;(" + cancelledBy + ")</p></body></html>",
                                CreatedOn = DateTime.Now,
                                CreatedBy = cancelledBy,
                                IsSent = false,
                                SentOn = null,
                                IsError = false,
                                ErrorDescription = "-",
                                SentCounter = 0
                            });
                }
            }
                    else if (cancelledBy == approvalOwner2)
            {
                        requestApplication.IsCancelled = true;
                        requestApplication.CancelledDate = DateTime.Now;
                        requestApplication.CancelledBy = cancelledBy;
                        requestApplication.IsReviewerCancelled = true;
                        requestApplication.ReviewerCancelledDate = DateTime.Now;
                        requestApplication.ReviewerCancelledBy = cancelledBy;

                        if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                {
                            emailSendLogs.Add(new EmailSendLog  //Send email to Approval Owner1
                    {
                                From = senderEmailId,
                                To = approvalOwner1EmailId,
                                CC = string.Empty,
                                BCC = string.Empty,
                                EmailSubject = "Requested Comp-Off availing application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\"> Comp-Off availing application of " + applicantName + "" +
                                "for the date " + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">" + cancelledByUserName + " &nbsp;(" + cancelledBy + ")</p></body></html>",
                                CreatedOn = DateTime.Now,
                                CreatedBy = cancelledBy,
                                IsSent = false,
                                SentOn = null,
                                IsError = false,
                                ErrorDescription = "-",
                                SentCounter = 0
                            });
                        }
                    }
                    else
                    {
                        requestApplication.IsCancelled = true;
                        requestApplication.CancelledDate = DateTime.Now;
                        requestApplication.CancelledBy = cancelledBy;

                        if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false) && applicationApproval.ApprovalStatusId == 2)
                        {
                            emailSendLogs.Add(new EmailSendLog  //Send email to Approval Owner1
                        {
                                From = senderEmailId,
                                To = approvalOwner1EmailId,
                                CC = string.Empty,
                                BCC = string.Empty,
                                EmailSubject = "Requested Comp-Off availing application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\"> Comp-Off availing application of " + applicantName + "" +
                                "for the date " + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">" + cancelledByUserName + " &nbsp;(" + cancelledBy + ")</p></body></html>",
                                CreatedOn = DateTime.Now,
                                CreatedBy = cancelledBy,
                                IsSent = false,
                                SentOn = null,
                                IsError = false,
                                ErrorDescription = "-",
                                SentCounter = 0
                            });
                        }
                        if (string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false) && applicationApproval.ReviewerstatusId == 2)
                        {
                            emailSendLogs.Add(new EmailSendLog  //Send email to Approval Owner1
                        {
                                From = senderEmailId,
                                To = approvalOwner2EmailId,
                                CC = string.Empty,
                                BCC = string.Empty,
                                EmailSubject = "Requested Comp-Off availing application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\"> Comp-Off application of " + applicantName + "" +
                                "for the date " + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">" + cancelledByUserName + " &nbsp;(" + cancelledBy + ")</p></body></html>",
                                CreatedOn = DateTime.Now,
                                CreatedBy = cancelledBy,
                                IsSent = false,
                                SentOn = null,
                                IsError = false,
                                ErrorDescription = "-",
                                SentCounter = 0
                            });
                        }
                        }
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                        {
                        emailSendLogs.Add(new EmailSendLog  //Send intimation to applicant
                            {
                            From = senderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested Comp-Off availing application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your Comp-Off availing application for the date" +
                            " " + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">" + cancelledByUserName + " &nbsp;(" + cancelledBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = cancelledBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                            }
                        }

                classesToSave.RA = requestApplication;
                classesToSave.AA = applicationApproval;
                classesToSave.ESL = emailSendLogs;
                if (employeeLeaveAccount.StaffId != null)
                {
                    classesToSave.ELA = employeeLeaveAccount;
                    }
                using (RACoffRequestApplicationRepository rACoffRequestApplicationRepository = new RACoffRequestApplicationRepository())
                    {
                    rACoffRequestApplicationRepository.CancelApplication(classesToSave);
            }

            return "OK";
        }
            else   //If the Comp-Off application has already been cancelled then...
        {
                throw new Exception("You cannot cancel a Comp-Off request that is already been cancelled.");
                    }
        }

        public List<RACoffAvailingRequestApplication> RenderAppliedCompAvailingListMyteam(string StaffId, string AppliedBy, string userRole)
                    {
            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
                    {
             
                return rALeaveApplicationRepository.RenderAppliedCompAvailingListMyteam(StaffId, AppliedBy, userRole);
               
                    }
                }

        public string ValidateCoffAvailing(string StaffId, string COffFromDate, string COffToDate, decimal TotalDays, string COffReqDate, string LeaveType)
                {
            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
                {
                
                return rACoffCreditApplicationRepository.ValidateCoffAvailing(StaffId, COffFromDate, COffToDate, TotalDays, COffReqDate, LeaveType);
                }
            }
        public string ValidateApplicationOverlaping(string StaffId, string CoffStartDate, string FromDurationId, string CoffEndDate, string ToDurationId)
        {
            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
        {
                return rACoffCreditApplicationRepository.ValidateApplicationOverlaping(StaffId, CoffStartDate, FromDurationId, CoffEndDate, ToDurationId);
        }
        }

        #endregion
    }
}
