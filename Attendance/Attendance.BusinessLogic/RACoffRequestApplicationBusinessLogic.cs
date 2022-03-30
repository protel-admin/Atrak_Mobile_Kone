using System;
using System.Collections.Generic;
using System.Linq;
using Attendance.Model;
using Attendance.Repository;
using System.Web.Mvc;

namespace Attendance.BusinessLogic
{
    public class RACoffRequestApplicationBusinessLogic
    {
        public List<RACoffRequestApplication> GetAppliedCoffRequest(string StaffId)
        {
            using (RACoffRequestApplicationRepository repo = new RACoffRequestApplicationRepository())
            { 
                var Obj = repo.GetAppliedCoffRequest(StaffId);
            return Obj;
            }
        }

        public LeaveTypeAndBalance GetLeaveTypeAndBalance(string staffid)
        {
            using (RACoffRequestApplicationRepository repo = new RACoffRequestApplicationRepository())
            { 
                var Obj = repo.GetLeaveTypeAndBalance(staffid);
            return Obj;
            }
        }

        public string GetUniqueId()
        {
            using (var repo = new RACoffRequestApplicationRepository())
            { 
                return repo.GetUniqueId();
            }
        }

        public void SaveRequestApplication(ClassesToSave DataToSave)
        {
            using (RACoffRequestApplicationRepository repo = new RACoffRequestApplicationRepository())
                repo.SaveRequestApplication(DataToSave);
        }

        public void RejectApplication(string Id, string ReportingManagerId)
        {
            //Get the leave application details based on the id passed to this function as a parameter.
            using (RACoffRequestApplicationRepository repo = new RACoffRequestApplicationRepository())
            { 
                var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id);

            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true))    //if the leave application has been cancelled then...
            {
                //throw exception stating that the cancelled leave application cannot be rejected.
                throw new Exception("Cancelled C-off request cannot be rejected.");
            }
            else if (Obj.IsApproved.Equals(true)) //if the leave application has been approved then...
            {
                //throw exception stating that the approved leave application cannot be rejected.
                throw new Exception("Approved C-off request cannot be rejected.");
            }
            else if (Obj.IsRejected.Equals(true))  //if the leave application has been rejected then...
            {
                //throw exception stating that the rejected leave application cannot be rejected.
                throw new Exception("Rejected C-off request cannot be rejected.");
            }
            else //if the leave application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {
                //reject the application.
                Obj.IsRejected = true;

                AA.ApprovalStatusId = 3;
                AA.ApprovedBy = ReportingManagerId;
                AA.ApprovedOn = DateTime.Now;
                AA.Comment = "C-OFF REQUEST HAS BEEN REJECTED BY THE REPORTING MANAGER.";

                ClassesToSave CTS = new ClassesToSave();
                CTS.RA = Obj;
                CTS.AA = AA;
                repo.RejectApplication(CTS);
                //send rejected mail to the applicant.
            }
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
                    payPeriodValidationMessage = commonRepository.ValidateApplicationForPayDate(DataToSave.RA.StaffId, DataToSave.RA.StartDate, DataToSave.RA.EndDate);
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
                    approvalOwner2Name = commonRepository.GetStaffName(DataToSave.AA.Approval2Owner);
                    approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.AA.Approval2Owner);
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
                DataToSave.RA.AppliedBy != DataToSave.AA.Approval2Owner)
                {
                    DataToSave.RA.IsApproved = true;
                    DataToSave.AA.ApprovalStatusId = 2;
                    DataToSave.AA.Approval2statusId = 1;
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
                    if (string.IsNullOrEmpty(DataToSave.AA.Approval2Owner).Equals(false) &&
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
                else if (DataToSave.RA.AppliedBy.Equals(DataToSave.AA.Approval2Owner))
                {
                    isFinalLevelApproval = true;
                    DataToSave.RA.IsApproved = true;
                    DataToSave.AA.ApprovalStatusId = 2;
                    DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedOn = DateTime.Now;
                    DataToSave.AA.Approval2statusId = 2;
                    DataToSave.AA.Approval2By = DataToSave.RA.AppliedBy;
                    DataToSave.AA.Approval2On = DateTime.Now;
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
                    DataToSave.AA.ApprovalStatusId = 2;
                    DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedOn = DateTime.Now;
                    DataToSave.AA.Approval2statusId = 2;
                    DataToSave.AA.Approval2By = DataToSave.RA.AppliedBy;
                    DataToSave.AA.Approval2On = DateTime.Now;
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
        public void ApproveApplication(string Id, string ReportingManagerId)
        {
            //Get the leave application details based on the Id passed to this function as a parameter.
            using (RACoffRequestApplicationRepository repo = new RACoffRequestApplicationRepository())
            { 
                var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id);

            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true)) //if the leave application has been cancelled then...
            {
                //throw exception that a cancelled leave application cannot be approved.
                throw new Exception("Cannot approve a cancelled c-off application. Apply for a new leave.");
            }
            else if (Obj.IsApproved.Equals(true)) //if application has already been approved then...
            {
                //throw exception stating that an already approved application cannot be reapproved.
                throw new Exception("Cannot approve already approved c-off request.");
            }
            else if (Obj.IsRejected.Equals(true))
            {
                //throw exception stating that an already rejected application cannot be approved.
                throw new Exception("Cannot approve already rejected c-off request.");
            }
            else
            {
                ////Get the leave balance based on the employee and the leave type.
                ////var LeaveBalance = repo.GetLeaveBalance(Obj.StaffId, Obj.LeaveTypeId);
                ////Check if the leave balance is more than the total days of leave requested.
                //if (LeaveBalance >= Convert.ToDecimal(Obj.TotalDays)) //if the leave balance is more then...
                //{
                //approve the application.
                Obj.IsApproved = true;

                //update the reporting manager in application Approval.
                //
                //
                AA.ApprovalStatusId = 2;
                AA.ApprovedBy = ReportingManagerId;
                AA.ApprovedOn = DateTime.Now;
                AA.Comment = "APPROVED THE C-OFF REQUEST.";

                //deduct leave balance from employee leave account table.
                EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
                ELA.StaffId = Obj.StaffId;
                ELA.LeaveTypeId = Obj.LeaveTypeId;
                ELA.TransactionFlag = 2;
                ELA.TransactionDate = DateTime.Now;
                ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays) * -1;
                ELA.LeaveCreditDebitReasonId = 22;
                ELA.Narration = "Approved the c-off application.";
                ELA.RefId = Obj.Id;

                //send confirmation email to the applicant.
                //
                //

                ClassesToSave CTS = new ClassesToSave();
                CTS.RA = Obj;
                CTS.ELA = ELA;
                CTS.AA = AA;
                repo.ApproveApplication(CTS);
            }
            }
        }

        public void CancelApplication(string Id, string user)
        {
            //Get the leave application details based on the Id passed to this function as a parameter.
            using (RACoffRequestApplicationRepository repo = new RACoffRequestApplicationRepository())
            { 
                ClassesToSave CTS = new ClassesToSave();
            var Obj = repo.GetRequestApplicationDetails(Id);
            //
            //Check whether the starting date of the leave application is below the current date.
            var IsFutureDate = IsFromDateMoreOrEqualToCurrerntDate(Obj.StartDate, DateTime.Now);
            //
            //If the leave application date is future to the current date.
            if (IsFutureDate == true)
            {
                //Check if the leave application has been approved or not.
                if (Obj.IsApproved.Equals(false))    //If the leave application has not been approved. (i.e. in the pending state) then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                    {
                        //Cancel the leave application which is in pending state.
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;
                        Obj.CancelledBy = user;
                        CTS.RA = Obj;
                        repo.CancelApplication(CTS);
                    }
                    else   //If the leave application has already been cancelled then...
                    {
                        //throw exception (first of all the cancel link must not be visible.)
                        throw new Exception("You cannot cancel a c-off request that is already been cancelled.");
                    }
                }
                else//If the leave application has been approved then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                    {
                        //Cancel the leave application which is in approved state.
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;
                        Obj.CancelledBy = user;
                        //Credit back the leave balance that was deducted.
                        EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
                        ELA.StaffId = Obj.StaffId;
                        ELA.LeaveTypeId = Obj.LeaveTypeId;
                        ELA.TransactionFlag = 1;
                        ELA.TransactionDate = DateTime.Now;
                        ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays);
                        ELA.Narration = "Cancelled the pending c-off application.";
                        ELA.LeaveCreditDebitReasonId = 22;
                        ELA.RefId = Obj.Id;
                        //
                        CTS.RA = Obj;
                        CTS.ELA = ELA;
                        repo.CancelApplication(CTS);
                    }
                    else //If the leave application has already been cancelled then...
                    {
                        //throw exception.
                        throw new Exception("You cannot cancel a c-off request that is already been cancelled.");
                    }
                }
            }
            else  //If the leave application is a past date then...
            {
                //Check if the leave application has been approved or not.
                if (Obj.IsApproved.Equals(false))    //If the leave application has not been approved. (i.e. in the pending state) then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                    {
                        //Cancel the leave application which is in pending state.
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;
                        Obj.CancelledBy = user;
                        CTS.RA = Obj;
                        repo.CancelApplication(CTS);
                    }
                    else //If the leave application has already been cancelled then...
                    {
                        //throw exception (first of all the cancel link must not be visible for the application that has already been cancelled.)
                        throw new Exception("You cannot cancel a c-off request that is already been cancelled.");
                    }
                }
                else  //If the leave application has been approved then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))//If the leave application has not been cancelled then...
                    {
                        //throw exception informing the user that the leave application has to be cancelled by his supervisor.
                        throw new Exception("You cannot cancel past c-off request that have already been approved. It has to be cancelled by your supervisor.");
                    }
                    else  //If the leave application has already been cancelled then...
                    {
                        //throw exception.
                        throw new Exception("You cannot cancel a c-off request that is already been cancelled.");
                    }
                }
            }
            }
        }



        public string CancelApprovedApplication(string Id)
        {
            string staffId = string.Empty;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            string applicationId = string.Empty;
            string applicationType = string.Empty;
            string punchtype = string.Empty;
            //Get the leave application details based on the Id passed to this function as a parameter.
            using (RACoffRequestApplicationRepository repo = new RACoffRequestApplicationRepository())
            { 
                ClassesToSave CTS = new ClassesToSave();
            var Obj = repo.GetRequestApplicationDetails(Id);
            //
            //Check whether the starting date of the leave application is below the current date.
            var IsFutureDate = IsFromDateMoreOrEqualToCurrerntDate(Obj.StartDate, DateTime.Now);
            //
            //If the leave application date is future to the current date.
            if (IsFutureDate == true)
            {
                //Check if the leave application has been approved or not.
                if (Obj.IsApproved.Equals(true))    //If the leave application has not been approved. (i.e. in the pending state) then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                    {
                        //Cancel the leave application which is in pending state.
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;
                        CTS.RA = Obj;
                        repo.CancelApplication(CTS);
                    }
                    else   //If the leave application has already been cancelled then...
                    {
                        //throw exception (first of all the cancel link must not be visible.)
                        throw new Exception("You cannot cancel a c-off request that is already been cancelled.");
                    }
                }
            }
            else  //If the leave application is a past date then...
            {
                //Check if the leave application has been approved or not.
                if (Obj.IsApproved.Equals(true))    //If the leave application has not been approved. (i.e. in the pending state) then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                    {
                        //Cancel the leave application which is in pending state.
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;

                        //Credit back the leave balance that was deducted.
                        EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
                        ELA.StaffId = Obj.StaffId;
                        ELA.LeaveTypeId = Obj.LeaveTypeId;
                        ELA.TransactionFlag = 1;
                        ELA.TransactionDate = DateTime.Now;
                        ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays);
                        ELA.Narration = "Cancelled the approved leave application.";
                        ELA.RefId = Obj.Id;
                        ELA.LeaveCreditDebitReasonId = 23;
                        //
                        CTS.RA = Obj;
                        CTS.ELA = ELA;
                        repo.CancelApplication(CTS);
                        ApplicationEntryRepository cmr = new ApplicationEntryRepository();
                        cmr.RemovePunchesFromSmax(CTS.RA.Id, CTS.RA.StaffId);
                        //repo.RemoveManualPunch(CTS);

                        CommonRepository obj = new CommonRepository();

                        try
                        {
                            var data = obj.GetList(Obj.Id);
                            staffId = data.StaffId;
                            fromDate = data.FromDate;
                            toDate = data.ToDate;
                            applicationType = Obj.RequestApplicationType;
                            applicationId = Obj.Id;
                        }
                        catch (Exception err)
                        {
                            throw err;
                        }
                        if (fromDate.Date < currentDate.Date)
                        {
                            if (toDate.Date >= currentDate.Date)
                            {
                                toDate = DateTime.Now.AddDays(-1);
                            }
                            obj.LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, applicationId);
                        }
                    }
                    else //If the leave application has already been cancelled then...
                    {
                        //throw exception (first of all the cancel link must not be visible for the application that has already been cancelled.)
                        throw new Exception("You cannot cancel a c-off request that is already been cancelled.");
                    }
                }
                else  //If the leave application has been approved then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))//If the leave application has not been cancelled then...
                    {
                        //throw exception informing the user that the leave application has to be cancelled by his supervisor.
                        throw new Exception("You cannot cancel past c-off request that have already been approved. It has to be cancelled by your supervisor.");
                    }
                    else  //If the leave application has already been cancelled then...
                    {
                        //throw exception.
                        throw new Exception("You cannot cancel a c-off request that is already been cancelled.");
                    }
                }
            }

            return "ok";
            }
        }
        private bool IsFromDateMoreOrEqualToCurrerntDate(DateTime? LeaveStartDate, DateTime? CurrentDate)
        {
            //TimeSpan TS1 = new TimeSpan();
            //TS1 = LeaveStartDate;


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

        public List<SelectListItem> GetDurationListBusinessLogic()
        {
            using (RACoffRequestApplicationRepository Repo = new RACoffRequestApplicationRepository())
            { 
                var lst = Repo.GetDurationListRepository();

            var item = new List<SelectListItem>();

            item = lst.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString(),
                Selected = false
            }).ToList();


            return item;
            }
        }
        public string ValidateCoffAvailing(string StaffId, string COffFromDate, string COffToDate, decimal TotalDays, string COffReqDate)
        {
            using (RACoffAvailingApplicationRepository repo = new RACoffAvailingApplicationRepository())
                return repo.ValidateCoffAvailing(StaffId, COffFromDate, COffToDate, TotalDays, COffReqDate);
        }
        public string GetCoffReqPeriodBusinessLogic()
        {
            using (RACoffRequestApplicationRepository Repo = new RACoffRequestApplicationRepository())
                return Repo.GetCoffReqPeriodRepository();
        }
        public string GetCoffReqPeriodBusinessLogic(string StaffId)
        {
            using (RACoffRequestApplicationRepository Repo = new RACoffRequestApplicationRepository())
                return Repo.GetCoffReqPeriodRepository(StaffId);
        }
        public int GetDesignationRank(string StaffId)
        {
            using (RACoffRequestApplicationRepository repo = new RACoffRequestApplicationRepository())
            { 
                int designationRank = repo.GetDesignationRank(StaffId);
            return designationRank;
            }
        }
        public string ValidateCOFFCreditApplication(string StaffId, string COffFromDate)
        {
            using (RACoffRequestApplicationRepository repo = new RACoffRequestApplicationRepository())
                return repo.ValidateCOFFCreditApplication(StaffId, COffFromDate);
        }

    }
}
