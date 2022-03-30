using Attendance.Model;
using Attendance.Repository;
using System;
using System.Collections.Generic;

namespace Attendance.BusinessLogic
{
    public class RACoffCreditApplicationBusinessLogic
    {
        public List<RACoffCreditRequestApplication> GetAppliedCoffCreditRequest(string StaffId)
        {
            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
            {
                return rACoffCreditApplicationRepository.GetAppliedCoffCreditRequest(StaffId);
            }
        }

        public List<RACoffCreditRequestApplication> GetAppliedCoffCreditRequestForMyTeam(string StaffId, string AppliedBy, string Role)
        {
            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
            {
                return rACoffCreditApplicationRepository.GetAppliedCoffCreditRequestForMyTeam(StaffId, AppliedBy, Role);
            }
        }

        public List<CoffReqDates> GetAllOTDates(string Staffid, string FromDate, string ToDate)
        {
            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
            {
                return rACoffCreditApplicationRepository.GetAllOTDates(Staffid, FromDate, ToDate);
            }
        }

        public string GetUniqueId()
        {
            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
            {
                return rACoffCreditApplicationRepository.GetUniqueId();
            }
        }

        public void SaveRequestApplication(ClassesToSave DataToSave, string SecurityGroupId, string LocationId, string BaseAddress, bool ManualCredit)
        {
            
           
            List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
            EmployeeLeaveAccount employeeLeaveAccount = new EmployeeLeaveAccount();
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
            bool isFinalLevelApproval = false;
            string applicationValidationMessage = string.Empty;

            try
            {
                applicationValidationMessage = ValidateCoffCreditApplication(DataToSave.RA.StaffId, Convert.ToDateTime(DataToSave.RA.StartDate),
                    DataToSave.RA.StartDate, "CR");
                if (string.IsNullOrEmpty(applicationValidationMessage).Equals(false) && applicationValidationMessage != "OK.")
            {
                    throw new ApplicationException(applicationValidationMessage);
                }
                using (CommonRepository commonRepository = new CommonRepository())
                {
                    applicantEmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.RA.StaffId);
                    approvalOwner1EmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.AA.ApprovalOwner);
                    approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.AA.ReviewerOwner);
                    appliedByUserEmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.RA.AppliedBy);
                    commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
                    appliedByUserName = commonRepository.GetStaffName(DataToSave.RA.AppliedBy);
                    approvalOwner1Name = commonRepository.GetStaffName(DataToSave.AA.ApprovalOwner);
                    applicantName = commonRepository.GetStaffName(DataToSave.RA.StaffId);
                    approvalOwner2Name = commonRepository.GetStaffName(DataToSave.AA.ReviewerOwner);
                }

                if (DataToSave.RA.AppliedBy.Equals(DataToSave.RA.StaffId))
                {
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        //Send acknowledgement email to the applicant.
                        emailSendLogs.Add(new EmailSendLog
                    {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                        BCC = string.Empty,
                            EmailSubject = "Request for Compensatory Off credit application sent to " + approvalOwner1Name,
                            EmailBody = "<html><head><title></title></head><body>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + applicantName + ",<br><br>" +
                            "Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Compensatory Off" +
                            " credit requisition for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                            " has been submitted to your Reporting Manager " + "(" + approvalOwner1Name + ") for approval</p>.</body></html>",
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
                        emailSendLogs.Add(new EmailSendLog
                {
                            From = commonSenderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                    BCC = string.Empty,
                            EmailSubject = "Compensatory Off credit application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + approvalOwner1Name + ",<br><br>Greetings</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + applicantName + " has applied for a" +
                            " Compensatory Off Credit  . Compensatory Off Credit requisition details are given below." +
                            "</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                            "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;" +
                            "font-family:tahoma; font-size:9pt;\">StaffId:</td><td style=\"width:80%;" +
                            "font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Worked Date:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                            + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Credits:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.TotalDays
                            + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                            "Reason:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                            + DataToSave.RA.Remarks + "</td></tr></table></p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Your attention is required to approve or reject this application.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">" +
                            "Redirect To Site</a></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
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


                // If the application has applied by Approval Owner1 or Reporting Manager
                else if (DataToSave.RA.AppliedBy.Equals(DataToSave.AA.ApprovalOwner)
                    && DataToSave.RA.AppliedBy != DataToSave.AA.ReviewerOwner)
            {
                DataToSave.RA.IsApproved = true;
                    DataToSave.RA.IsReviewed = false;
                DataToSave.AA.ApprovalStatusId = 2;
                DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                DataToSave.AA.ApprovedOn = DateTime.Now;
                DataToSave.AA.Comment = "APPROVED THE C-OFF CREDIT REQUEST.";
                    // Send intimaton to the applicant
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                    CC = string.Empty,
                    BCC = string.Empty,
                            EmailSubject = "Requested Compensatory Off credit application status",
                            EmailBody = "<html><head><title></title></head><body> <p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br> Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your Compensatory Off credit requisition for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                            " has been applied and approved by " + appliedByUserName + "  (" + DataToSave.RA.AppliedBy + ") and sent to second level approval.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p> <p style=\"font-family:tahoma; font-size:9pt;\">" +
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
                    //Send intimation to the approval owner2
                    if (string.IsNullOrEmpty(DataToSave.AA.ReviewerOwner).Equals(false)
                        && string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
            {
                        emailSendLogs.Add(new EmailSendLog
                    {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                        BCC = string.Empty,
                            EmailSubject = "Compensatory Off  credit application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + approvalOwner2Name + ",<br><br>Greetings</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + applicantName + " has applied for a" +
                            " Compensatory Off credit and has been approved by " + approvalOwner1Name +
                            ". Compensatory Off credit requisition details are given below." +
                            "</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                            "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;" +
                            "font-family:tahoma; font-size:9pt;\">StaffId:</td><td style=\"width:80%;" +
                            "font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Worked Date:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                            + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Credits:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.TotalDays
                            + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                            "Reason:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                            + DataToSave.RA.Remarks + "</td></tr></table></p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Your attention is required to approve or reject this application.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">" +
                            "Redirect To Site</a></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
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
                    DataToSave.AA.ApprovalOwner = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedOn = DateTime.Now;
                    DataToSave.AA.ReviewerstatusId = 2;
                    DataToSave.AA.ReviewedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ReviewedOn = DateTime.Now;
                    DataToSave.AA.Comment = "APPROVED THE C-OFF CREDIT REQUEST.";

                    //Credit leave balance into employee leave account.
                    employeeLeaveAccount.StaffId = DataToSave.RA.StaffId;
                    employeeLeaveAccount.LeaveTypeId = DataToSave.RA.LeaveTypeId;
                    employeeLeaveAccount.TransactionFlag = 1;
                    employeeLeaveAccount.TransactionDate = DateTime.Now;
                    employeeLeaveAccount.LeaveCount = Convert.ToDecimal(DataToSave.RA.TotalDays);
                    employeeLeaveAccount.LeaveCreditDebitReasonId = 20;
                    employeeLeaveAccount.Narration = "Approved the Compensatory Off  credit application.";
                    employeeLeaveAccount.RefId = DataToSave.RA.Id;
                    employeeLeaveAccount.WorkedDate = DataToSave.RA.WorkedDate;
                    employeeLeaveAccount.Month = Convert.ToDateTime(DataToSave.RA.WorkedDate).Month;
                    employeeLeaveAccount.Year = Convert.ToDateTime(DataToSave.RA.WorkedDate).Year;
                    employeeLeaveAccount.TransctionBy = DataToSave.RA.AppliedBy;
                    // Send intimation to the applicant
                    if (string.IsNullOrEmpty(appliedByUserEmailId).Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                    {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                            EmailSubject = "Requested Compensatory Off credit application status",
                            EmailBody = "<html><head><title></title></head><body>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + applicantName + ",<br/><br>" +
                            "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your Compensatory Off credit requisition for the date " +
                            "" + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                            " has been applied and approved by " + appliedByUserName + " (" + DataToSave.RA.AppliedBy + ").</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + "" + appliedByUserName +
                            " &nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>",
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



                if ((DataToSave.RA.StaffId != DataToSave.RA.AppliedBy || ManualCredit == true) && (SecurityGroupId == "3" ||
                    SecurityGroupId == "5" || SecurityGroupId == "6"))
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
                    DataToSave.AA.Comment = "APPROVED AND REVIEWED THE C-OFF CREDIT REQUEST.";

                    //Credit the Compensatory Off balance into employee leave account table.

                    employeeLeaveAccount.StaffId = DataToSave.RA.StaffId;
                    employeeLeaveAccount.LeaveTypeId = DataToSave.RA.LeaveTypeId;
                    employeeLeaveAccount.TransactionFlag = 1;
                    employeeLeaveAccount.TransactionDate = DateTime.Now;
                    employeeLeaveAccount.LeaveCount = Convert.ToDecimal(DataToSave.RA.TotalDays);
                    employeeLeaveAccount.LeaveCreditDebitReasonId = 20;
                    employeeLeaveAccount.Narration = "Approved the c-off credit application.";
                    employeeLeaveAccount.RefId = DataToSave.RA.Id;
                    employeeLeaveAccount.WorkedDate = DataToSave.RA.WorkedDate;
                    employeeLeaveAccount.Month = Convert.ToDateTime(DataToSave.RA.WorkedDate).Month;
                    employeeLeaveAccount.Year = Convert.ToDateTime(DataToSave.RA.WorkedDate).Year;
                    employeeLeaveAccount.TransctionBy = DataToSave.RA.AppliedBy;


                    // Send intimation to the applicant
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
            {
                        emailSendLogs.Add(new EmailSendLog
                {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                    BCC = string.Empty,
                            EmailSubject = "Requested Compensatory Off credit application status",
                            EmailBody = "<html><head><title></title></head><body>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + applicantName + ",<br/><br>" +
                            "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your Compensatory Off credit requisition for the date "
                            + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                            " has been applied and approved by " + appliedByUserName + " (" + DataToSave.RA.AppliedBy + ").</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + "" + appliedByUserName +
                            " &nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>",
                    CreatedOn = DateTime.Now,
                            CreatedBy = DataToSave.RA.AppliedBy,
                    IsSent = false,
                            SentOn = null,
                    IsError = false,
                    ErrorDescription = "-",
                    SentCounter = 0
                });
            }
                    DataToSave.ELA = employeeLeaveAccount;
                    DataToSave.ESL = emailSendLogs;
            }
                using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
            {
                    rACoffCreditApplicationRepository.SaveRequestApplication(DataToSave, isFinalLevelApproval);
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            }

        /* Code changed by Karuppiah on 07-06-2020 for the below purpose.
        1. To resolve the issue in RejectApplication method
        2. To avoid the duplicate conditions
        3. Email content has been corrected in all email intimations
        4. To check the email of the recipient and sender email id before sent the notification */
        public void RejectApplication(string Id, string RejectedBy)
            {
            //Get the Compensatory Off credit application application details based on the id passed to this function as a parameter.

            RequestApplication requestApplication = new RequestApplication();
            ApplicationApproval applicationApproval = new ApplicationApproval();

            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
                {

                requestApplication = rACoffCreditApplicationRepository.GetRequestApplicationDetails(Id);
                applicationApproval  = rACoffCreditApplicationRepository.GetApplicationApproval(Id);
                }

            List<EmailSendLog> ESL = new List<EmailSendLog>();
                    ClassesToSave CTS = new ClassesToSave();
            string applicantName = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string applicantEmailId = string.Empty;
            string Approval2Owner = string.Empty;
            string rejectedByUserName = string.Empty;
            string rejectedByUserEmailId = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string approvalOwner2Name = string.Empty;
            using (CommonRepository commonRepository = new CommonRepository())
                {
                applicantName = commonRepository.GetStaffName(requestApplication.StaffId);
                approvalOwner1 = commonRepository.GetApproverOwner(Id);
                approvalOwner2 = commonRepository.GetReviewerOwner(Id);
                applicantEmailId = commonRepository.GetEmailIdOfEmployee(requestApplication.StaffId);
                rejectedByUserName = commonRepository.GetStaffName(RejectedBy);
                rejectedByUserEmailId = commonRepository.GetEmailIdOfEmployee(RejectedBy);
                commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
                approvalOwner2Name = commonRepository.GetStaffName(Approval2Owner);
                approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(Approval2Owner);

                }
            senderEmailId = commonSenderEmailId;
            //Check if the Compensatory Off credit application has been cancelled or not.
            if (requestApplication.IsCancelled.Equals(true))    //if the Compensatory Off credit application has been cancelled then...
                {
                throw new ApplicationException("Cancelled C-Off credit request cannot be rejected.");
                }
            else if (requestApplication.IsRejected.Equals(true))  //if the Compensatory Off credit application has been rejected then...
            {
                throw new ApplicationException("Rejected C-Off credit request cannot be rejected.");
            }
            else //if the Compensatory Off credit application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {
                if (RejectedBy == approvalOwner1 && RejectedBy != approvalOwner2)
                {
                    requestApplication.IsRejected = true;
                    applicationApproval.ApprovalStatusId = 3;
                    applicationApproval.ApprovedBy = RejectedBy;
                    applicationApproval.ApprovedOn = DateTime.Now;
                    applicationApproval.Comment = "COFF REQUEST HAS BEEN REJECTED BY THE APPROVER.";
                    //Send intimation to the applicant
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
        {

                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                                BCC = string.Empty,
                            EmailSubject = "Requested Compensatory Off credit application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your Compensatory Off credit application for the date " + Convert.ToDateTime(requestApplication.StartDate).
                            ToString("dd-MMM-yyyy") + " has been rejected.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + rejectedByUserName + " &nbsp;" +
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
                    }
                else if (RejectedBy == approvalOwner2)
                        {
                    requestApplication.IsRejected = true;
                    if (approvalOwner1 == approvalOwner2)
                            {
                        applicationApproval.ApprovalStatusId = 3;
                        applicationApproval.ApprovedBy = RejectedBy;
                        applicationApproval.ApprovedOn = DateTime.Now;
                        }
                    applicationApproval.ReviewerstatusId = 3;
                    applicationApproval.ReviewedOn = DateTime.Now;
                    applicationApproval.ReviewedBy = RejectedBy;
                    applicationApproval.Comment = "C-OFF REQUEST HAS BEEN REJECTED BY THE REPORTING MANAGER.";
                    //Send intimation to the applicant
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                        {
                        ESL.Add(new EmailSendLog
                            {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                                CC = string.Empty,
                                BCC = string.Empty,
                            EmailSubject = "Requested Compensatory Off  credit application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your Compensatory Off credit application for the date " + Convert.ToDateTime(requestApplication.StartDate).
                            ToString("dd-MMM-yyyy") + " has been rejected.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + rejectedByUserName + " &nbsp;" +
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
                    }
                CTS.RA = requestApplication;
                CTS.AA = applicationApproval;
                CTS.ESL = ESL;
                using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
                    {
                    rACoffCreditApplicationRepository.RejectApplication(CTS);
                }
            }
                        }

        /* Code changed by Karuppiah on 07-06-2020 for the below purposes.
        1. To resolve the issue in ApproveApplication method
        2. To avoid the duplicate conditions
        3. Email content has been corrected in all email intimations
        4. To check the email of the recipient and sender email id before sent the notification */
        public void ApproveApplication(string Id, string ApprovedBy)
        {
            //Get the Compensatory Off  credit application details based on the Id passed to this function as a parameter.
            RequestApplication requestApplication = new RequestApplication();
            ApplicationApproval applicationApproval = new ApplicationApproval();


            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
            {
                requestApplication = rACoffCreditApplicationRepository.GetRequestApplicationDetails(Id);
                applicationApproval = rACoffCreditApplicationRepository.GetApplicationApproval(Id);
                    }
            
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
            ClassesToSave CTS = new ClassesToSave();
            string applicantEmailId = string.Empty;
            string approvedByUserName = string.Empty;
            string applicantName = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvedByUserEmailId = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string approvalOwner2Name = string.Empty;
            bool isFinalLevelApproval = false;
            using (CommonRepository commonRepository = new CommonRepository())
            {
                approvalOwner1 = commonRepository.GetApproverOwner(Id);
                approvalOwner2 = commonRepository.GetReviewerOwner(Id);
                applicantName = commonRepository.GetStaffName(requestApplication.StaffId);
                applicantEmailId = commonRepository.GetEmailIdOfEmployee(requestApplication.StaffId);
                approvedByUserName = commonRepository.GetStaffName(ApprovedBy);
                approvedByUserEmailId = commonRepository.GetEmailIdOfEmployee(ApprovedBy);
                commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
                approvalOwner2Name = commonRepository.GetStaffName(approvalOwner2);
                approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner2);
                }
            ////Check if the Compensatory Off  credit application has been cancelled or not.
            if (requestApplication.IsCancelled.Equals(true)) //if the Compensatory Off  credit application has been cancelled then...
                {
                throw new Exception("Cannot approve a cancelled c-off credit request.");
                }
            else if (requestApplication.IsRejected.Equals(true))
            {
                throw new Exception("Cannot approve a rejected c-off credit request.");
            }
            else
            {
                if (ApprovedBy.Equals(approvalOwner1) && ApprovedBy != approvalOwner2)
                {
                    //approve the application.
                    requestApplication.IsApproved = true;
                    requestApplication.IsReviewed = false;
                    applicationApproval.ApprovalStatusId = 2;
                    applicationApproval.ApprovedBy = ApprovedBy;
                    applicationApproval.ApprovedOn = DateTime.Now;
                    applicationApproval.Comment = "APPROVED THE COMP-OFF REQUEST.";
                    // Send intimation to the applicant
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested Compensatory Off credit application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            "font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma;" +
                             " font-size:9pt;\">Your Compensatory Off credit application for the date " +
                             "" + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy") + " has been approved.</p>" +
                             "<p style=\"font-family:tahoma; font-size:9pt;\">" + "Best Regards</p><p style=\"font-family:tahoma;" +
                             " font-size:9pt;\">" + approvedByUserName + " &nbsp;" + "(" + ApprovedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = ApprovedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                    //Send intimation to the approval owner2
                    if (string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Compensatory Off credit application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + approvalOwner2Name + ",<br><br>Greetings</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + applicantName + " has applied for a" +
                            " Compensatory Off credit application and has been approved by " + approvedByUserName +
                            ".Details are given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                            "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;" +
                            "font-family:tahoma; font-size:9pt;\">StaffId:</td><td style=\"width:80%;" +
                            "font-family:tahoma; font-size:9pt;\">" + requestApplication.StaffId + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Worked Date:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                            + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Credits:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + requestApplication.TotalDays
                            + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                            "Reason:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                            + requestApplication.Remarks + "</td></tr></table></p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Your attention is required to approve or reject this application.</p>" +
                           " <p style=\"font-family:tahoma; font-size:9pt;\">" + "Best Regards,</p><p style=\"font-family:tahoma;" +
                           " font-size:9pt;\">" + approvedByUserName + "&nbsp;" + "(" + ApprovedBy + ")</p></body></html>",
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
                else if (ApprovedBy.Equals(approvalOwner2))
                {
                    //approve the application.
                    isFinalLevelApproval = true;
                    requestApplication.IsApproved = true;
                    requestApplication.IsReviewed = true;
                    if (approvalOwner1 == approvalOwner2)
                    {
                        applicationApproval.ApprovalStatusId = 2;
                        applicationApproval.ApprovedBy = ApprovedBy;
                        applicationApproval.ApprovedOn = DateTime.Now;
                }
                    applicationApproval.ReviewerstatusId = 2;
                    applicationApproval.ReviewedBy = ApprovedBy;
                    applicationApproval.ReviewedOn = DateTime.Now;
                    applicationApproval.Comment = "APPROVED THE COMP-OFF CREDIT REQUEST.";


                    ELA.StaffId = requestApplication.StaffId;
                    ELA.LeaveTypeId = requestApplication.LeaveTypeId;
                    ELA.TransactionFlag = 1;
                    ELA.TransactionDate = DateTime.Now;
                    ELA.LeaveCount = Convert.ToDecimal(requestApplication.TotalDays);
                    ELA.Narration = "Approved the Compensatory Off  credit application.";
                    ELA.RefId = requestApplication.Id;
                    ELA.LeaveCreditDebitReasonId = 20;
                    ELA.WorkedDate = requestApplication.WorkedDate;
                    ELA.TransctionBy = ApprovedBy;
                    ELA.IsSystemAction = false;
                    if (requestApplication.WorkedDate != null)
                    {
                        ELA.Month = Convert.ToDateTime(requestApplication.WorkedDate).Month;
                        ELA.Year = Convert.ToDateTime(requestApplication.WorkedDate).Year;
                }
                    //Send the intimation to the applicant
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
            ESL.Add(new EmailSendLog
            {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                BCC = string.Empty,
                            EmailSubject = "Requested Compensatory Off credit application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                             "font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma;" +
                              " font-size:9pt;\">Your Compensatory Off credit application for the date " +
                              "" + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy") + " has been approved.</p>" +
                              "<p style=\"font-family:tahoma; font-size:9pt;\">" + "Best Regards</p><p style=\"font-family:tahoma;" +
                              " font-size:9pt;\">" + approvedByUserName + " &nbsp;" + "(" + ApprovedBy + ")</p></body></html>",
                CreatedOn = DateTime.Now,
                            CreatedBy = ApprovedBy,
                IsSent = false,
                            SentOn = null,
                IsError = false,
                ErrorDescription = "-",
                SentCounter = 0
            });
                    }
                        CTS.ELA = ELA;
                }
                CTS.RA = requestApplication;
                CTS.AA = applicationApproval;
                if (ELA.StaffId != null)
                {
                        CTS.ELA = ELA;
                        }
                        CTS.ESL = ESL;
                using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
                        {
                    rACoffCreditApplicationRepository.ApproveApplication(CTS, isFinalLevelApproval);
                        }
                        }
                            }

        public string CancelApplication(string Id, string CancelledBy)
                    {
            //Get the Compensatory application details based on the Id passed to this function as a parameter.
            
            ClassesToSave CTS = new ClassesToSave();
            EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            RequestApplication requestApplication = new RequestApplication();
            ApplicationApproval applicationApproval = new ApplicationApproval();            

            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
                    {
                requestApplication = rACoffCreditApplicationRepository.GetRequestApplicationDetails(Id);
                applicationApproval = rACoffCreditApplicationRepository.GetApplicationApproval(Id);
                    }
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string cancelledByUserName = string.Empty;
            string cancelledByUserEmailId = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner1Name = string.Empty;
            string approvalOwner2Name = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string approvalOwner2EmailId = string.Empty;

            approvalOwner1 = applicationApproval.ApprovalOwner;
            approvalOwner2 = applicationApproval.ReviewerOwner;
            using (CommonRepository commonRepository = new CommonRepository())
                    {
                applicantName = commonRepository.GetStaffName(requestApplication.StaffId);
                applicantEmailId = commonRepository.GetEmailIdOfEmployee(requestApplication.StaffId);
                cancelledByUserName = commonRepository.GetStaffName(CancelledBy);
                cancelledByUserEmailId = commonRepository.GetEmailIdOfEmployee(CancelledBy);
                commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();

                approvalOwner1Name = commonRepository.GetStaffName(applicationApproval.ApprovalOwner);
                approvalOwner2Name = commonRepository.GetStaffName(applicationApproval.ReviewerOwner);
                approvalOwner1EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner1);
                approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner2);
                }

            //Check if the Compensatory Off credit application has already been cancelled or not.
            if (requestApplication.IsCancelled.Equals(false))   //If the Compensatory Off credit application has not been cancelled then...
        {
                bool isCompOffAvailed = false;
                requestApplication.IsCancelled = true;
                requestApplication.CancelledDate = DateTime.Now;
                requestApplication.CancelledBy = CancelledBy;

                if (applicationApproval.ReviewerstatusId == 2)
            {
                    if (requestApplication.StartDate != null)
            {
                        using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
            {
                            isCompOffAvailed = rACoffCreditApplicationRepository.CheckIsCompOffAvailed(requestApplication.StaffId, requestApplication.StartDate);
            }
        }
                    if (isCompOffAvailed == true)
        {
                        throw new ApplicationException("You cannot cancel the C-Off credit request because C-Off availing has been applied for the worked date.");
        }

                    //Debit the comp-off credit that was credited.                            
                    ELA.StaffId = requestApplication.StaffId;
                    ELA.LeaveTypeId = requestApplication.LeaveTypeId;
                    ELA.TransactionFlag = 2;
                    ELA.TransactionDate = DateTime.Now;
                    ELA.LeaveCount = Convert.ToDecimal(requestApplication.TotalDays) * -1;
                    ELA.Narration = "Cancelled the approved Comp-Off credit application.";
                    ELA.LeaveCreditDebitReasonId = 22;
                    ELA.RefId = requestApplication.Id;
                    ELA.WorkedDate = requestApplication.WorkedDate;
        }
                else
        {
                    ELA = null;
        }
                if (CancelledBy == requestApplication.StaffId)
        {
                    requestApplication.IsCancelled = true;
                    requestApplication.CancelledDate = DateTime.Now;
                    requestApplication.CancelledBy = CancelledBy;
                    if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
            {
                        ESL.Add(new EmailSendLog  //Send email to Approval Owner1
                {
                            From = commonSenderEmailId,
                            To = approvalOwner1EmailId,
                    CC = string.Empty,
                    BCC = string.Empty,
                            EmailSubject = "Requested Compensatory Off credit application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"> Compensatory Off credit application of " + applicantName + "" +
                            " for the date " + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">" + cancelledByUserName + " &nbsp;(" + CancelledBy + ")</p></body></html>",
                    CreatedOn = DateTime.Now,
                            CreatedBy = CancelledBy,
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
                        ESL.Add(new EmailSendLog  //Send email to Approval Owner2
                    {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                            EmailSubject = "Requested Compensatory Off credit application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"> Compensatory Off credit application of " + applicantName + "" +
                            " for the date " + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">" + cancelledByUserName + " &nbsp;(" + CancelledBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                            CreatedBy = CancelledBy,
                        IsSent = false,
                            SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
            }
                else if (CancelledBy != requestApplication.StaffId)
            {
                    if (CancelledBy == approvalOwner1)
                    {
                        requestApplication.IsCancelled = true;
                        requestApplication.CancelledDate = DateTime.Now;
                        requestApplication.CancelledBy = CancelledBy;
                        requestApplication.IsApproverCancelled = true;
                        requestApplication.ApproverCancelledDate = DateTime.Now;
                        requestApplication.ApproverCancelledBy = CancelledBy;
                        if (applicationApproval.ApprovalStatusId == 2 && string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
            {
                            ESL.Add(new EmailSendLog  //Send email to Approval Owner2
                {
                                From = commonSenderEmailId,
                                To = approvalOwner2EmailId,
                    CC = string.Empty,
                    BCC = string.Empty,
                                EmailSubject = "Requested Compensatory Off credit application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\"> Compensatory Off credit application of " + applicantName + "" +
                                " for the date " + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">" + cancelledByUserName + " &nbsp;(" + CancelledBy + ")</p></body></html>",
                    CreatedOn = DateTime.Now,
                                CreatedBy = CancelledBy,
                    IsSent = false,
                                SentOn = null,
                    IsError = false,
                    ErrorDescription = "-",
                    SentCounter = 0
                });
            }
        }
                    else if (CancelledBy == approvalOwner2)
        {
                        requestApplication.IsReviewerCancelled = true;
                        requestApplication.IsCancelled = true;
                        requestApplication.ReviewerCancelledDate = DateTime.Now;
                        requestApplication.CancelledDate = DateTime.Now;
                        requestApplication.ReviewerCancelledBy = CancelledBy;
                        requestApplication.CancelledBy = CancelledBy;
                        if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
        {
                            ESL.Add(new EmailSendLog  //Send email to Approval Owner1
            {
                                From = commonSenderEmailId,
                                To = approvalOwner1EmailId,
                CC = string.Empty,
                BCC = string.Empty,
                                EmailSubject = "Requested Compensatory Off credit application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\"> Compensatory Off credit application of " + applicantName + "" +
                                " for the date " + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">" + cancelledByUserName + " &nbsp;(" + CancelledBy + ")</p></body></html>",
                CreatedOn = DateTime.Now,
                                CreatedBy = CancelledBy,
                IsSent = false,
                                SentOn = null,
                IsError = false,
                ErrorDescription = "-",
                SentCounter = 0
            });
            }
            }

                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested Compensatory Off credit application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your Compensatory Off credit requisition for the date " + Convert.ToDateTime(requestApplication.StartDate).ToString("dd-MMM-yyyy") + "" +
                        " has been cancelled.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Best Regards</p><p style=\"font-family:tahoma;font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                        "(" + CancelledBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = CancelledBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                CTS.RA = requestApplication;
                CTS.AA = applicationApproval;
                    CTS.ELA = ELA;
                    CTS.ESL = ESL;
                using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
                    {
                    rACoffCreditApplicationRepository.CancelApplication(CTS);
                }

                return "ok";
                }
            else   //If the Compensatory Off credit application has already been cancelled then...
                    {
                throw new ApplicationException("You cannot cancel a Compensatory Off credit request that is already been cancelled.");
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
        public List<COffReqAvailModel> GetWorkedDatesForCompOffCreditRequest(string StaffId)
                {
            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
                    {
                return rACoffCreditApplicationRepository.GetWorkedDatesForCompOffCreditRequest(StaffId);
            }
                }

        public string ValidateApplicationOverlaping(string StaffId, string CoffStartDate, string FromDurationId, string CoffEndDate, string ToDurationId)
                {
            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
                    {
                return rACoffCreditApplicationRepository.ValidateApplicationOverlaping(StaffId, CoffStartDate, FromDurationId, CoffEndDate, ToDurationId);
            }
                }

        public List<RACoffAvailingRequestApplication> RenderAppliedCompAvailingList(string StaffId, string AppliedBy, string userRole)
        {
            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
            {
                return rACoffCreditApplicationRepository.RenderAppliedCompAvailingList(StaffId, AppliedBy, userRole);
            }
        }
        #endregion

        #region Coff Req Availing

        #endregion
        // Changes Made by aarthi on 28/2/2020 to get the lapse period for CompOff Availing
        // Changes made by Karuppiah on 07-06-2020 to fine tune the query and avoid type casting
        public int GetCompOffLapsePeriod(string LocationId, string StaffId)
        {
            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
        {
                return rACoffCreditApplicationRepository.GetCompOffLapsePeriod(LocationId, StaffId);
            }
        }

        public string ValidateCoffCreditApplication(string StaffId, DateTime FromDate, DateTime ToDate, string ApplicationType)
        {
            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
        {
                return rACoffCreditApplicationRepository.ValidateCoffCreditApplication(StaffId, FromDate, ToDate, ApplicationType);
            }
        }
    }
}
