using Attendance.Model;
using Attendance.Repository;
using System;
using System.Collections.Generic;

namespace Attendance.BusinessLogic
{
    public class RequestApplicationBusinessLogic
    {
        public string GetUniqueId()
        {
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
                return requestApplicationRepository.GetUniqueId();
        }
        public List<HolidayWorkingListItem> GetAppliedHolidayWorkingList(string staffId)
        {
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
                return requestApplicationRepository.GetAppliedHolidayWorkingList(staffId);
        }
        public List<HolidayWorkingPendingApprovalListItem> GetPendingHolidayWorkingApprovals(string ReportingManagerId)
        {
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
                return requestApplicationRepository.GetPendingHolidayWorkingApprovals(ReportingManagerId);

        }
        public void SaveHolidayWorkingDetails(ClassesToSave classesToSave, string SecurityGroupId, string approvalLevel)
        {
            try
            {
                using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
                { 
                    List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
                CommonBusinessLogic commonBusinessLogic = new CommonBusinessLogic();
                string approvalOwner1Name = string.Empty;
                string StaffName = string.Empty;
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
                string payPeriodValidationMessage = string.Empty;
                string shiftIn = string.Empty;
                string shiftOut = string.Empty;
                string shiftInDate = string.Empty;
                string shiftOutDate = string.Empty;
                string shiftInTime = string.Empty;
                string shiftOutTime = string.Empty;
                string approvalOwner = string.Empty;
                string approvalOwner2 = string.Empty;
                shiftIn = string.Concat(shiftInDate, " ", shiftInTime);
                shiftOut = string.Concat(shiftOutDate, " ", shiftOutTime);

                //payPeriodValidationMessage = commonBusinessLogic.ValidateApplicationForPayDate(classesToSave.RA.StartDate.ToString(),
                //classesToSave.RA.EndDate.ToString());
                payPeriodValidationMessage = commonBusinessLogic.ValidateApplicationForPayDate(classesToSave.RA.StaffId,Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy"),
                   Convert.ToDateTime(classesToSave.RA.EndDate).ToString("dd-MMM-yyyy"));
                if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
                {
                    if (payPeriodValidationMessage.ToUpper() != "VALID")
                    {
                        throw new ApplicationException("Application of past pay cycle cannot be saved");
                    }
                }

                CommonRepository commonRepository = new CommonRepository();
                appliedByUserEmailId = commonRepository.GetEmailIdOfEmployee(classesToSave.RA.AppliedBy);
                commonSenderEmailId = commonRepository.GetSenderEmailIdFromEmailSettings();
                applicantEmailId = commonRepository.GetEmailIdOfEmployee(classesToSave.RA.StaffId);
                applicantName = commonRepository.GetStaffName(classesToSave.RA.StaffId);
                appliedByUserName = commonRepository.GetStaffName(classesToSave.RA.AppliedBy);
                approvalOwner1Name = commonRepository.GetStaffName(classesToSave.AA.ApprovalOwner);
                approvalOwner1EmailId = commonRepository.GetEmailIdOfEmployee(classesToSave.AA.ApprovalOwner);
                if (approvalLevel == "1" || string.IsNullOrEmpty(classesToSave.AA.Approval2Owner).Equals(true))
                {
                    classesToSave.AA.Approval2Owner = classesToSave.AA.ApprovalOwner;
                    approvalOwner2EmailId = approvalOwner1EmailId;
                    approvalOwner2Name = approvalOwner1Name;
                }
                else
                {
                    approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(classesToSave.AA.Approval2Owner);
                    approvalOwner2Name = commonRepository.GetStaffName(classesToSave.AA.Approval2Owner);
                }

                if (classesToSave.RA.AppliedBy.Equals(classesToSave.RA.StaffId))
                {
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        // Send intimation to the applicant
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Request for holiday working application sent to " + approvalOwner1Name,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your holiday working application for the date " + Convert.ToDateTime(classesToSave.RA.StartDate).
                            ToString("dd-MMM-yyyy") + " has been sent to your reporting manager "
                            + approvalOwner1Name + " for approval.</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = classesToSave.RA.AppliedBy,
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
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Request for holiday working application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner1Name + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            " " + applicantName + " has applied for a holiday working. Holiday working details are given below.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; " +
                            "font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + classesToSave.RA.StaffId + "</td></tr><tr>" +
                            "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; " +
                            "font-size:9pt;\">" + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td></tr><tr>" +
                            "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift In:</td><td style=\"width:80%;font-family:tahoma; " +
                            "font-size:9pt;\">" + shiftIn + "</td></tr><tr><td style=\"width:20%;font-family:tahoma;font-size:9pt;\">" +
                            "Shift Out:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftOut + "</td></tr></table></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"> Your attention is needed to either approve or reject this " +
                            "application.</p><p style=\"font-family:tahoma; font-size:9pt;\"> Best Regards,</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\"> " + applicantName + " &nbsp;(" + classesToSave.RA.StaffId + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = classesToSave.RA.AppliedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                    classesToSave.ESL = emailSendLogs;
                }

                // If the application has applied by the Approval Owner1 or Reporting Manager
                if (classesToSave.RA.AppliedBy.Equals(classesToSave.AA.ApprovalOwner) &&
                    classesToSave.RA.AppliedBy != classesToSave.AA.Approval2Owner)
                {
                    classesToSave.RA.IsApproved = false;
                    classesToSave.AA.ApprovalStatusId = 2;
                    classesToSave.AA.Approval2statusId = 1;
                    classesToSave.AA.ApprovedBy = classesToSave.RA.AppliedBy;
                    classesToSave.AA.ApprovedOn = DateTime.Now;
                    classesToSave.AA.Comment = "APPROVED THE HOLIDAY WORKING REQUEST.";

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        //Send intimation to the applicant 
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested holiday working application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your holiday working application for the " +
                            "date " + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "" +
                            " has been applied and approved by " + appliedByUserName + " and sent for second level approval.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; " +
                            "font-size:9pt;\">" + appliedByUserName + " &nbsp;(" + classesToSave.RA.AppliedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = classesToSave.RA.AppliedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }

                    //Send intimation to Approval Owner2
                    if (string.IsNullOrEmpty(classesToSave.AA.Approval2Owner).Equals(false) && string.IsNullOrEmpty(approvalOwner2EmailId)
                        .Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Request for holiday working application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner2Name + "" +
                            ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + approvalOwner1Name + " has applied and approved for a holiday working.Holiday working details are given below.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; " +
                            "font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + classesToSave.RA.StaffId + "</td></tr><tr>" +
                            "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; " +
                            "font-size:9pt;\">" + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td></tr><tr>" +
                            "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift In:</td><td style=\"width:80%;font-family:tahoma; " +
                            "font-size:9pt;\">" + shiftIn + "</td></tr><tr><td style=\"width:20%;font-family:tahoma;font-size:9pt;\">" +
                            "Shift Out:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftOut + "</td></tr></table></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this" +
                            " application.</p><p style=\"font-family:tahoma; font-size:9pt;\"> Best Regards,</p><p style=\"font-family:tahoma; " +
                            "font-size:9pt;\"> " + approvalOwner1Name + " &nbsp;(" + classesToSave.RA.AppliedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = classesToSave.RA.AppliedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                    classesToSave.ESL = emailSendLogs;
                }

                // If the application has applied by Approval Owner2 or Reviewer
                else if (classesToSave.RA.AppliedBy.Equals(classesToSave.AA.Approval2Owner))
                {
                    isFinalLevelApproval = true;
                    classesToSave.RA.IsApproved = true;
                    classesToSave.AA.ApprovalStatusId = 2;
                    classesToSave.AA.ApprovedBy = classesToSave.RA.AppliedBy;
                    classesToSave.AA.ApprovedOn = DateTime.Now;
                    classesToSave.AA.Approval2statusId = 2;
                    classesToSave.AA.Approval2By = classesToSave.RA.AppliedBy;
                    classesToSave.AA.Approval2On = DateTime.Now;
                    classesToSave.AA.Comment = "APPROVED THE HOLIDAY WORKING REQUEST.";

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested holiday working application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your holiday working application" +
                            " for the date " + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy")
                            + " has been applied and approved.</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">" + "Best Regards</p><p style=\"font-family:tahoma; " +
                            "font-size:9pt;\">" + approvalOwner1Name + " &nbsp;" +
                            "(" + classesToSave.AA.ApprovalOwner + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = classesToSave.RA.AppliedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                    classesToSave.ESL = emailSendLogs;
                }
                if (classesToSave.RA.AppliedBy != classesToSave.RA.StaffId && (SecurityGroupId.Equals("1") || SecurityGroupId.Equals("3")
               || SecurityGroupId.Equals("5")))
                {
                    isFinalLevelApproval = true;
                    classesToSave.RA.IsApproved = true;
                    classesToSave.AA.ApprovalStatusId = 2;
                    classesToSave.AA.ApprovedBy = classesToSave.RA.AppliedBy;
                    classesToSave.AA.ApprovedOn = DateTime.Now;
                    classesToSave.AA.Approval2statusId = 2;
                    classesToSave.AA.Approval2By = classesToSave.RA.AppliedBy;
                    classesToSave.AA.Approval2On = DateTime.Now;
                    classesToSave.AA.Comment = "APPROVED THE HOLIDAY WORKING REQUEST.";

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested holiday working application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your holiday working application for the date " + Convert.ToDateTime(classesToSave.RA.StartDate).
                            ToString("dd-MMM-yyyy") + " has been applied and approved.</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + appliedByUserName + " &nbsp;(" + classesToSave.RA.AppliedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = classesToSave.RA.AppliedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                    classesToSave.ESL = emailSendLogs;
                }
                classesToSave.ESL = emailSendLogs;
                requestApplicationRepository.SaveHolidayWorkingDetails(classesToSave, isFinalLevelApproval);
            }
            }
            catch (Exception err)
            {
                throw err;
            }

        }
        public void ApproveHolidayWorking(string Id, string ApprovedBy, string parentType)
        {
            //Get the holiday working application details based on the Id passed to this function as a parameter.
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
            { 
                var Obj = requestApplicationRepository.GetRequestApplicationDetails(Id);
            var AA = requestApplicationRepository.GetApplicationApproval(Id, parentType);
            ClassesToSave classesToSave = new ClassesToSave();
            List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            string applicationId = string.Empty;
            string applicantName = string.Empty;
            string applicationType = string.Empty;
            string payPeriodValidationMessage = string.Empty;

            string applicantEmailId = string.Empty;
            string approvedBy = string.Empty;
            string approvedByUserEmailId = string.Empty;
            string approvedByUserName = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            bool isFinalLevelApproval = false;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string approvalOwner2Name = string.Empty;
            string shiftIn = string.Empty;
            string shiftOut = string.Empty;
            string shiftInDate = string.Empty;
            string shiftOutDate = string.Empty;
            string shiftInTime = string.Empty;
            string shiftOutTime = string.Empty;
            string ccEmployeeCode = string.Empty;
            string ccEmailId = string.Empty;
            string ccUserName = string.Empty;
            string ccEmailSubject = string.Empty;

            shiftIn = string.Concat(shiftInDate, " ", shiftInTime);
            shiftOut = string.Concat(shiftOutDate, " ", shiftOutTime);
            CommonRepository commonRepository = new CommonRepository();
            applicantEmailId = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
            applicantName = commonRepository.GetStaffName(Obj.StaffId);
            approvalOwner1 = commonRepository.GetApproverOwner(Id);
            approvalOwner2 = commonRepository.GetReviewerOwner(Id);
            approvedBy = ApprovedBy;
            approvedByUserName = commonRepository.GetStaffName(approvedBy);
            approvedByUserEmailId = commonRepository.GetEmailIdOfEmployee(approvedBy);
            commonSenderEmailId = commonRepository.GetSenderEmailIdFromEmailSettings();

            // Check the approval level and then get the approval owner 2 name and email id
            approvalOwner2Name = commonRepository.GetStaffName(approvalOwner2);
            approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner2);
            if (Obj.AppliedBy == "")
            {
                Obj.AppliedBy = Obj.StaffId;
            }
            payPeriodValidationMessage = commonRepository.ValidateApplicationForPayDate(Obj.StaffId, Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy"),
                 Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy"));
            if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
            {
                if (payPeriodValidationMessage.ToUpper() != "VALID")
                {
                    throw new ApplicationException("Application of past pay cycle cannot be approved");
                }
            }
            if (Obj.IsRejected.Equals(true))
            {
                throw new ApplicationException("Cannot approve already rejected holiday working request.");
            }
            else
            {
                if (approvedBy.Equals(approvalOwner1) && approvedBy != approvalOwner2)
                {
                    //Approve the application.
                    Obj.IsApproved = false;
                    AA.ApprovalStatusId = 2;
                    AA.ApprovedBy = approvedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "APPROVED THE HOLIDAY WORKING REQUEST.";
                    //Send intimation to Approval Owner2
                    if (string.IsNullOrEmpty(approvalOwner2).Equals(false) && string.IsNullOrEmpty(approvalOwner2EmailId)
                       .Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested holiday working application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner2Name + "" +
                            ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + applicantName + " has applied for a holiday working. Holiday working details are given below.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; " +
                            "font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Obj.StaffId + "</td></tr><tr>" +
                            "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; " +
                            "font-size:9pt;\">" + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + "</td></tr><tr>" +
                            "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift In:</td><td style=\"width:80%;font-family:tahoma; " +
                            "font-size:9pt;\">" + shiftIn + "</td></tr><tr><td style=\"width:20%;font-family:tahoma;font-size:9pt;\">" +
                            "Shift Out:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftOut + "</td></tr></table></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this" +
                            " application</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; " +
                            "font-size:9pt;\">" + approvedByUserName + " &nbsp;(" + ApprovedBy + ")</p></body></html>",
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
                    //Approve the application.
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
                    AA.Comment = "APPROVED THE HOLIDAY WORKING REQUEST.";
                }
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested holiday working application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        " Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        " Your holiday working application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
                        " has been approved.</p><p style=\"font-family:tahoma; font-size:9pt;\"></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + approvedByUserName + " &nbsp;(" + approvedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = approvedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                if (isFinalLevelApproval == true)
                {
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = ccEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested holiday working application status ",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + ccUserName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Holiday working application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
                        " has been approved.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
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

                classesToSave.RA = Obj;
                classesToSave.AA = AA;
                classesToSave.ESL = emailSendLogs;
                requestApplicationRepository.ApproveHolidayWorking(classesToSave, isFinalLevelApproval);
            }
            }
        }
        public void RejectHolidayWorkingApplication(string Id, string RejectedBy, string parentType)
        {
            //Get the holiday working application details based on the id passed to this function as a parameter.
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
            { 
                var Obj = requestApplicationRepository.GetRequestApplicationDetails(Id);
            var AA = requestApplicationRepository.GetApplicationApproval(Id, parentType);
            ClassesToSave classesToSave = new ClassesToSave();
            List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string LeaveTypeName = string.Empty;
            string approvalOwner1Name = string.Empty;
            string approvalOwner2Name = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string rejectedByUserName = string.Empty;
            string rejectedByUserEmailid = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string payPeriodValidationMessage = string.Empty;
            CommonRepository commonRepository = new CommonRepository();
            applicantName = commonRepository.GetStaffName(Obj.StaffId);
            applicantEmailId = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
            approvalOwner1 = commonRepository.GetApproverOwner(Id);
            approvalOwner2 = commonRepository.GetReviewerOwner(Id);
            approvalOwner1Name = commonRepository.GetStaffName(approvalOwner1);
            approvalOwner2Name = commonRepository.GetStaffName(approvalOwner2);
            rejectedByUserName = commonRepository.GetStaffName(RejectedBy);
            rejectedByUserEmailid = commonRepository.GetEmailIdOfEmployee(RejectedBy);
            approvalOwner1EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner2);
            commonSenderEmailId = commonRepository.GetSenderEmailIdFromEmailSettings();
            if (Obj.AppliedBy == "")
            {
                Obj.AppliedBy = Obj.StaffId;
            }
            payPeriodValidationMessage = commonRepository.ValidateApplicationForPayDate(Obj.StaffId, Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy"),
                 Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy"));
            if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
            {
                if (payPeriodValidationMessage.ToUpper() != "VALID")
                {
                    throw new ApplicationException("Application of past pay cycle cannot be rejected");
                }
            }
            if (Obj.IsRejected.Equals(true)) //if the holiday working application has been rejected then..
            {
                throw new ApplicationException("Rejected holiday working request cannot be rejected.");
            }
            else //if the holiday working application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {
                if (RejectedBy == approvalOwner1 && RejectedBy != approvalOwner2)
                {
                    //reject the application.
                    Obj.IsRejected = true;
                    AA.ApprovalStatusId = 3;
                    AA.ApprovedBy = RejectedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "HOLIDAY WORKING REQUEST HAS BEEN REJECTED BY THE APPROVAL OWNER1.";
                }
                else if (RejectedBy == approvalOwner2)
                {
                    //Reject the application.
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
                    AA.Comment = "HOLIDAY WORKING REQUEST HAS BEEN REJECTED BY THE APPROVAL OWNER2.";
                }
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested holiday working application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your holiday working application for the date " + Convert.ToDateTime(Obj.StartDate).ToString() + " has been rejected.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
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
                classesToSave.RA = Obj;
                classesToSave.AA = AA;
                classesToSave.ESL = emailSendLogs;
                requestApplicationRepository.RejectHolidayWorkingApplication(classesToSave);
            }
            }
        }
        public string CancelHolidayWorkingApplication(string Id, string CancelledBy, string parentType)
        {
            //Get the holiday working application details based on the Id passed to this function as a parameter.
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
            { 
                ClassesToSave CTS = new ClassesToSave();
            var Obj = requestApplicationRepository.GetRequestApplicationDetails(Id);
            var AA = requestApplicationRepository.GetApplicationApproval(Id, parentType);
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            CommonBusinessLogic CBL = new CommonBusinessLogic();
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner1Name = string.Empty;
            string approvalOwner2Name = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string cancelledByUserEmailId = string.Empty;
            string cancelledByUserName = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string payPeriodValidationMessage = string.Empty;
            CommonRepository commonRepository = new CommonRepository();
            approvalOwner1Name = commonRepository.GetStaffName(AA.ApprovalOwner);
            approvalOwner2Name = commonRepository.GetStaffName(AA.Approval2Owner);
            applicantName = commonRepository.GetStaffName(Obj.StaffId);
            applicantEmailId = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
            cancelledByUserEmailId = commonRepository.GetEmailIdOfEmployee(CancelledBy);
            cancelledByUserName = commonRepository.GetStaffName(CancelledBy);
            commonSenderEmailId = commonRepository.GetSenderEmailIdFromEmailSettings();
            if (Obj.AppliedBy == "")
            {
                Obj.AppliedBy = Obj.StaffId;
            }
            //Check if the holiday working application has already been cancelled or not.
            if (Obj.IsCancelled.Equals(false))   //If the holiday working application has not been cancelled then...
            {
                payPeriodValidationMessage = CBL.ValidateApplicationForPayDate(Obj.StaffId,Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy"),
                   Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy"));
                if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
                {
                    if (payPeriodValidationMessage.ToUpper() != "VALID")
                    {
                        throw new ApplicationException("Application of past pay cycle cannot be cancelled");
                    }
                }
                //Cancel the holiday working application which is in pending state.
                Obj.IsCancelled = true;
                Obj.CancelledDate = DateTime.Now;
                Obj.CancelledBy = CancelledBy;

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
                            EmailSubject = "Requested holiday working application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:" +
                            "tahoma; font-size:9pt;\">Dear " + approvalOwner1Name + ",<br/><br>" +
                            "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Holiday working application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).
                            ToString("dd-MMM-yyyy") + " has been cancelled. </p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
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
                    if (AA.ApprovalStatusId == 2 && string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner2
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested holiday working application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            " Dear " + approvalOwner2Name + ",< br />< br > " +
                            "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Holiday working application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).
                            ToString("dd-MMM-yyyy") + " has been cancelled. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
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
                    if (CancelledBy == AA.ApprovalOwner && string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner2
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested holiday working application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            " Dear " + approvalOwner2Name + ",< br />< br > " +
                            "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Holiday working application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).
                            ToString("dd-MMM-yyyy") + " has been cancelled. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
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
                    if (CancelledBy == AA.Approval2Owner && string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner1
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested holiday working application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            " Dear " + approvalOwner1Name + ",< br />< br > " +
                            "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Holiday working application " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).
                            ToString("dd-MMM-yyyy") + " has been cancelled. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
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
                            EmailSubject = "Requested holiday working application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:" +
                            "tahoma; font-size:9pt;\">Dear " + applicantName + ",<br/><br>" +
                            "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Holiday working application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).
                            ToString("dd-MMM-yyyy") + " has been cancelled. </p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                            "(" + CancelledBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = Obj.StaffId,
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
                requestApplicationRepository.CancelHolidayWorkingApplication(CTS, CancelledBy);
            }
            else //If the holiday working application has already been cancelled then...
            {
                throw new ApplicationException("You cannot cancel a holiday working request that is already been cancelled.");
            }
            return "OK";
            }
        }
        public List<ShiftExtenstionListItem> GetAppliedShiftExtenstionList(string staffId)
        {
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
            {
                return requestApplicationRepository.GetAppliedShiftExtenstionList(staffId);
            }
        }
        public void SaveShiftExtensionDetails(ClassesToSave classesToSave, string SecurityGroupId, string approvalLevel)
        {
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
            { 
                List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
            CommonBusinessLogic commonBusinessLogic = new CommonBusinessLogic();
            string approvalOwner1Name = string.Empty;
            string StaffName = string.Empty;
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
            string payPeriodValidationMessage = string.Empty;
            string validateShiftExtension = string.Empty;

            payPeriodValidationMessage = commonBusinessLogic.ValidateApplicationForPayDate(classesToSave.RA.StaffId,Convert.ToDateTime(classesToSave.RA.StartDate).ToString("yyyy-MM-dd"),
            Convert.ToDateTime(classesToSave.RA.EndDate).ToString("yyyy-MM-dd"));
            if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
            {
                if (payPeriodValidationMessage.ToUpper() != "VALID")
                {
                    throw new ApplicationException("Application of past pay cycle cannot be saved");
                }
            }
            
            if (classesToSave.RA.DurationOfHoursExtension == "AfterShift")
            {
                validateShiftExtension = commonBusinessLogic.ValidateShiftExtension(classesToSave.RA.StaffId, Convert.ToDateTime(classesToSave.RA.StartDate).ToString("yyyy-MM-dd"),
             classesToSave.RA.DurationOfHoursExtension, classesToSave.RA.HoursBeforeShift.ToString(),
             Convert.ToDateTime(classesToSave.RA.HoursAfterShift).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (classesToSave.RA.DurationOfHoursExtension == "BeforeShift")
            {
                validateShiftExtension = commonBusinessLogic.ValidateShiftExtension(classesToSave.RA.StaffId, Convert.ToDateTime(classesToSave.RA.StartDate).ToString("yyyy-MM-dd"),
             classesToSave.RA.DurationOfHoursExtension, Convert.ToDateTime(classesToSave.RA.HoursBeforeShift).ToString("yyyy-MM-dd"),
             classesToSave.RA.HoursAfterShift.ToString());
            }
            else {
                validateShiftExtension = commonBusinessLogic.ValidateShiftExtension(classesToSave.RA.StaffId, Convert.ToDateTime(classesToSave.RA.StartDate).ToString("yyyy-MM-dd"),
                   classesToSave.RA.DurationOfHoursExtension, Convert.ToDateTime(classesToSave.RA.HoursBeforeShift).ToString("yyyy-MM-dd"),
                   Convert.ToDateTime(classesToSave.RA.HoursAfterShift).ToString("yyyy-MM-dd"));
            }
            if(validateShiftExtension != "OK")
            {
                throw new Exception(validateShiftExtension);
            }
            CommonRepository commonRepository = new CommonRepository();
            applicantEmailId = commonRepository.GetEmailIdOfEmployee(classesToSave.RA.StaffId);
            approvalOwner1EmailId = commonRepository.GetEmailIdOfEmployee(classesToSave.AA.ApprovalOwner);

            if (approvalLevel == "1" || string.IsNullOrEmpty(classesToSave.AA.Approval2Owner).Equals(true))
            {
                classesToSave.AA.Approval2Owner = classesToSave.AA.ApprovalOwner;
                approvalOwner2EmailId = approvalOwner1EmailId;
                approvalOwner2Name = approvalOwner1Name;
            }
            else
            {
                approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(classesToSave.AA.Approval2Owner);
                approvalOwner2Name = commonRepository.GetStaffName(classesToSave.AA.Approval2Owner);
            }

            appliedByUserEmailId = commonRepository.GetEmailIdOfEmployee(classesToSave.RA.AppliedBy);
            commonSenderEmailId = commonRepository.GetSenderEmailIdFromEmailSettings();
            applicantName = commonRepository.GetStaffName(classesToSave.RA.StaffId);
            approvalOwner1Name = commonRepository.GetStaffName(classesToSave.AA.ApprovalOwner);
            approvalOwner2Name = commonRepository.GetStaffName(classesToSave.AA.Approval2Owner);
            appliedByUserName = commonRepository.GetStaffName(classesToSave.RA.AppliedBy);

            if (classesToSave.RA.AppliedBy.Equals(classesToSave.RA.StaffId))
            {
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    // Send intimation to the applicant
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Request for shift extension application sent to " + approvalOwner1Name,
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + applicantName + ",<br><br>" +
                        "Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your Shift extension application for the date " + Convert.ToDateTime(classesToSave.RA.StartDate).
                        ToString("dd-MMM-yyyy") + " has been sent to your reporting manager "
                        + approvalOwner1Name + " for approval.</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = classesToSave.RA.AppliedBy,
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
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = approvalOwner1EmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Request for shift extension application of " + applicantName,
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + approvalOwner1Name + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        " " + applicantName + " has applied for a shift extension. Shift extension details are given below.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma;" +
                        " font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + classesToSave.RA.StaffId + "</td></tr><tr>" +
                        "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;" +
                        "font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td>" +
                        "</tr>   <tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Duration:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + classesToSave.RA.DurationOfHoursExtension + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Before Shift Hours:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + classesToSave.RA.HoursBeforeShift + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">After Shift Hours:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + classesToSave.RA.HoursAfterShift + "</td></tr>" +
                        "</table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this" +
                        " application.</p><p style=\"font-family:tahoma; font-size:9pt;\"> Best Regards,</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"> " + applicantName + " &nbsp;" +
                        "(" + classesToSave.RA.StaffId + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = classesToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                classesToSave.ESL = emailSendLogs;
            }


            // If the application has applied by the Approval Owner1 or Reporting Manager
            if (classesToSave.RA.AppliedBy.Equals(classesToSave.AA.ApprovalOwner) &&
                classesToSave.RA.AppliedBy != classesToSave.AA.Approval2Owner)
            {
                classesToSave.RA.IsApproved = false;
                classesToSave.AA.ApprovalStatusId = 2;
                classesToSave.AA.Approval2statusId = 1;
                classesToSave.AA.ApprovedBy = classesToSave.RA.AppliedBy;
                classesToSave.AA.ApprovedOn = DateTime.Now;
                classesToSave.AA.Comment = "APPROVED THE SHIFT EXTENSION REQUEST.";
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    //Send intimation to the applicant 
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested shift extension application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Your shift extension application " +
                        "for the date " + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                        " has been applied and approved by " + appliedByUserName + "" +
                        " and sent to second level approval.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + appliedByUserName + " &nbsp;(" + classesToSave.RA.AppliedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = classesToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }

                //Send intimation to Approval Owner2
                if (string.IsNullOrEmpty(classesToSave.AA.Approval2Owner).Equals(false) && string.IsNullOrEmpty(approvalOwner2EmailId)
                    .Equals(false))
                {
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = approvalOwner2EmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Request for shift extension application of " + applicantName,
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + approvalOwner2Name + ", <br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + approvalOwner1Name + " has applied and approved for a shift extension. Shift extension details are given below.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma;" +
                        " font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + classesToSave.RA.StaffId + "</td></tr><tr>" +
                        "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;" +
                        "font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td>" +
                        "</tr>   <tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Duration:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + classesToSave.RA.DurationOfHoursExtension + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Before Shift Hours:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + classesToSave.RA.HoursBeforeShift + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">After Shift Hours:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + classesToSave.RA.HoursAfterShift + "</td></tr>" +
                        "</table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this " +
                        " application.</p><p style=\"font-family:tahoma; font-size:9pt;\"> Best Regards,</p><p style=\"font-family:tahoma; " +
                        "font-size:9pt;\"> " + approvalOwner1Name + " &nbsp;(" + classesToSave.RA.AppliedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = classesToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                classesToSave.ESL = emailSendLogs;
            }

            // If the application has applied by Approval Owner2 or Reviewer
            else if (classesToSave.RA.AppliedBy.Equals(classesToSave.AA.Approval2Owner))
            {
                isFinalLevelApproval = true;
                if (classesToSave.AA.Approval2Owner == classesToSave.AA.ApprovalOwner)
                {
                    classesToSave.RA.IsApproved = true;
                    classesToSave.AA.ApprovalStatusId = 2;
                    classesToSave.AA.ApprovedBy = classesToSave.RA.AppliedBy;
                }
                classesToSave.AA.ApprovedOn = DateTime.Now;
                classesToSave.AA.Approval2statusId = 2;
                classesToSave.AA.Approval2By = classesToSave.RA.AppliedBy;
                classesToSave.AA.Approval2On = DateTime.Now;
                classesToSave.AA.Comment = "APPROVED THE SHIFT EXTENSION REQUEST.";

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested shift extension application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Your shift extension application" +
                        " for the date " + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy")
                        + " has been applied and approved.</p><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">" + "Best Regards</p><p style=\"font-family:tahoma; " +
                        "font-size:9pt;\">" + approvalOwner1Name + " &nbsp;" +
                        "(" + classesToSave.AA.ApprovalOwner + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = classesToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                classesToSave.ESL = emailSendLogs;
            }

            if (classesToSave.RA.AppliedBy != classesToSave.RA.StaffId && (SecurityGroupId.Equals("1") || SecurityGroupId.Equals("3")
               || SecurityGroupId.Equals("5")))
            {
                isFinalLevelApproval = true;
                classesToSave.RA.IsApproved = true;
                classesToSave.AA.ApprovalStatusId = 2;
                classesToSave.AA.ApprovedBy = classesToSave.RA.AppliedBy;
                classesToSave.AA.ApprovedOn = DateTime.Now;
                classesToSave.AA.Approval2statusId = 2;
                classesToSave.AA.Approval2By = classesToSave.RA.AppliedBy;
                classesToSave.AA.Approval2On = DateTime.Now;
                classesToSave.AA.Comment = "APPROVED THE SHIFT EXTENSION REQUEST.";

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested shift extension application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your shift extension application for the date " + Convert.ToDateTime(classesToSave.RA.StartDate).
                        ToString("dd-MMM-yyyy") + " has been applied and approved.</p><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + appliedByUserName + " &nbsp;(" + classesToSave.RA.AppliedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = classesToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                classesToSave.ESL = emailSendLogs;
            }
            classesToSave.ESL = emailSendLogs;
            requestApplicationRepository.SaveShiftExtensionDetails(classesToSave, isFinalLevelApproval);
            }
        }

        public void ApproveShiftExtensionApplication(string Id, string ApprovedBy, string parantType)
        {
            //Get the shift extension application details based on the Id passed to this function as a parameter.
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
            { 
                var Obj = requestApplicationRepository.GetRequestApplicationDetails(Id);
            var AA = requestApplicationRepository.GetApplicationApproval(Id, parantType);
            ClassesToSave classesToSave = new ClassesToSave();
            List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
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
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string approvalOwner2Name = string.Empty;
            string payPeriodValidationMessage = string.Empty;

            CommonRepository commonRepository = new CommonRepository();
            applicantEmailId = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
            applicantName = commonRepository.GetStaffName(Obj.StaffId);
            approvalOwner1 = commonRepository.GetApproverOwner(Id);
            approvalOwner2 = commonRepository.GetReviewerOwner(Id);
            approvedBy = ApprovedBy;
            approvedByUserName = commonRepository.GetStaffName(approvedBy);
            approvedByUserEmailId = commonRepository.GetEmailIdOfEmployee(approvedBy);
            commonSenderEmailId = commonRepository.GetSenderEmailIdFromEmailSettings();

            // Check the approval level and then get the approval owner 2 name and email id
            approvalOwner2Name = commonRepository.GetStaffName(approvalOwner2);
            approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner2);
            if (Obj.AppliedBy == "")
            {
                Obj.AppliedBy = Obj.StaffId;
            }
            payPeriodValidationMessage = commonRepository.ValidateApplicationForPayDate(Obj.StaffId, Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy"),
                 Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy"));
            if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
            {
                if (payPeriodValidationMessage.ToUpper() != "VALID")
                {
                    throw new ApplicationException("Application of past pay cycle cannot be approved");
                }
            }
            if (Obj.IsRejected.Equals(true))
            {
                throw new ApplicationException("Cannot approve already rejected shift extension request.");
            }
            else
            {
                if (approvedBy.Equals(approvalOwner1) && approvedBy != approvalOwner2)
                {
                    //Approve the application.
                    Obj.IsApproved = false;
                    AA.ApprovalStatusId = 2;
                    AA.ApprovedBy = approvedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "APPROVED THE SHIFT EXTENSION REQUEST.";
                    //Send intimation to Approval Owner2
                    if (string.IsNullOrEmpty(approvalOwner2).Equals(false) && string.IsNullOrEmpty(approvalOwner2EmailId)
                       .Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested shift extension application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner2Name + "" +
                            ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + applicantName + " has applied for a shift extension.Shift extension details are given below.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma;" +
                            " font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + classesToSave.RA.StaffId + "</td></tr><tr>" +
                            "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;" +
                            "font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td>" +
                            "</tr>   <tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Duration:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + classesToSave.RA.DurationOfHoursExtension + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Before Shift Hours:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + classesToSave.RA.HoursBeforeShift + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">After Shift Hours:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + classesToSave.RA.HoursAfterShift + "</td></tr>" +
                            "</table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this" +
                            " application</p><p style=\"font-family:tahoma;" +
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
                }
                // If the applicaation has approevd by Approval Owner2
                else if (approvedBy.Equals(approvalOwner2))
                {
                    //Approve the application.
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
                    AA.Comment = "APPROVED THE SHIFT EXTENSION REQUEST.";
                }
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested shift extension application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your shift extension application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
                        " has been approved.</p><p style=\"font-family:tahoma; font-size:9pt;\"></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + approvedByUserName + " &nbsp;(" + approvedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = approvedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }

                classesToSave.RA = Obj;
                classesToSave.AA = AA;
                classesToSave.ESL = emailSendLogs;
                requestApplicationRepository.ApproveShiftExtensionApplication(classesToSave, isFinalLevelApproval);
            }
            }
        }

        public void RejectShiftExtensionApplication(string Id, string RejectedBy, string ParentType)
        {
            //Get the shift extension application details based on the id passed to this function as a parameter.
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
            { 
                var Obj = requestApplicationRepository.GetRequestApplicationDetails(Id);
            var AA = requestApplicationRepository.GetApplicationApproval(Id, ParentType);
            ClassesToSave classesToSave = new ClassesToSave();
            List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string LeaveTypeName = string.Empty;
            string approvalOwner1Name = string.Empty;
            string approvalOwner2Name = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string rejectedByUserName = string.Empty;
            string rejectedByUserEmailid = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string payPeriodValidationMessage = string.Empty;
            CommonRepository commonRepository = new CommonRepository();
            applicantName = commonRepository.GetStaffName(Obj.StaffId);
            applicantEmailId = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
            approvalOwner1 = commonRepository.GetApproverOwner(Id);
            approvalOwner2 = commonRepository.GetReviewerOwner(Id);
            approvalOwner1Name = commonRepository.GetStaffName(approvalOwner1);
            approvalOwner2Name = commonRepository.GetStaffName(approvalOwner2);
            rejectedByUserName = commonRepository.GetStaffName(RejectedBy);
            rejectedByUserEmailid = commonRepository.GetEmailIdOfEmployee(RejectedBy);
            approvalOwner1EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner2);
            commonSenderEmailId = commonRepository.GetSenderEmailIdFromEmailSettings();
            if (Obj.AppliedBy == "")
            {
                Obj.AppliedBy = Obj.StaffId;
            }
            payPeriodValidationMessage = commonRepository.ValidateApplicationForPayDate(Obj.StaffId, Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy"),
                 Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy"));
            if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
            {
                if (payPeriodValidationMessage.ToUpper() != "VALID")
                {
                    throw new ApplicationException("Application of past pay cycle cannot be rejected");
                }
            }
            if (Obj.IsRejected.Equals(true))  //if the shift extension application has been rejected then...
            {
                throw new ApplicationException("Rejected shift extension request cannot be rejected.");
            }
            else //if the shift extension application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {
                if (RejectedBy == approvalOwner1 && RejectedBy != approvalOwner2)
                {
                    //reject the application.
                    Obj.IsRejected = true;
                    AA.ApprovalStatusId = 3;
                    AA.ApprovedBy = RejectedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "SHIFT EXTENSION REQUEST HAS BEEN REJECTED BY THE REVIEWER.";
                }
                else if (RejectedBy == approvalOwner2)
                {
                    //Reject the application.
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
                    AA.Comment = "SHIFT EXTENSION REQUEST HAS BEEN REJECTED BY THE REPORTING MANAGER.";
                }
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested shift extension application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your shift extension application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy")
                        + " has been rejected.</p> <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
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

                classesToSave.RA = Obj;
                classesToSave.AA = AA;
                classesToSave.ESL = emailSendLogs;
                requestApplicationRepository.RejectShiftExtensionApplication(classesToSave);
            }
            }
        }
        public string CancelShiftExtensionApplication(string Id, string CancelledBy, string parentType)
        {
            //Get the shift extension application details based on the Id passed to this function as a parameter.
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
            { 
                ClassesToSave CTS = new ClassesToSave();
            var Obj = requestApplicationRepository.GetRequestApplicationDetails(Id);
            var AA = requestApplicationRepository.GetApplicationApproval(Id, parentType);
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            CommonBusinessLogic CBL = new CommonBusinessLogic();
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner1Name = string.Empty;
            string approvalOwner2Name = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string cancelledByUserEmailId = string.Empty;
            string cancelledByUserName = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string payPeriodValidationMessage = string.Empty;

            CommonRepository commonRepository = new CommonRepository();
            approvalOwner1Name = commonRepository.GetStaffName(AA.ApprovalOwner);
            approvalOwner2Name = commonRepository.GetStaffName(AA.Approval2Owner);
            applicantName = commonRepository.GetStaffName(Obj.StaffId);
            applicantEmailId = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
            cancelledByUserEmailId = commonRepository.GetEmailIdOfEmployee(CancelledBy);
            cancelledByUserName = commonRepository.GetStaffName(CancelledBy);
            commonSenderEmailId = commonRepository.GetSenderEmailIdFromEmailSettings();
            if (Obj.AppliedBy == "")
            {
                Obj.AppliedBy = Obj.StaffId;
            }
            //Check if the shift extension application has already been cancelled or not.
            if (Obj.IsCancelled.Equals(false))   //If the shift extension application has not been cancelled then...
            {
                payPeriodValidationMessage = CBL.ValidateApplicationForPayDate(Obj.StaffId,Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy"),
                   Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy"));
                if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
                {
                    if (payPeriodValidationMessage.ToUpper() != "VALID")
                    {
                        throw new ApplicationException("Application of past pay cycle cannot be cancelled");
                    }
                }
                //Cancel the shift extension application which is in pending state.
                Obj.IsCancelled = true;
                Obj.CancelledDate = DateTime.Now;
                Obj.CancelledBy = CancelledBy;

                if (CancelledBy == Obj.StaffId)
                {
                    if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner1
                        {
                            From =commonSenderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested shift extension application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:" +
                            "tahoma; font-size:9pt;\">Dear " + approvalOwner1Name + ",<br/><br>" +
                            "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Permission application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).
                            ToString("dd-MMM-yyyy") + " has been cancelled. </p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
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
                    if (AA.ApprovalStatusId == 2 && string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner2
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested shift extension application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            " Dear " + approvalOwner2Name + ",< br />< br > " +
                            "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Shift extension application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).
                            ToString("dd-MMM-yyyy") + " has been cancelled. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
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
                    if (CancelledBy == AA.ApprovalOwner && string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner2
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested shift extension application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            " Dear " + approvalOwner2Name + ",< br />< br > " +
                            "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Shift extension application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).
                            ToString("dd-MMM-yyyy") + " has been cancelled. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
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
                    if (CancelledBy == AA.Approval2Owner && string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner1
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested shift extension application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            " Dear " + approvalOwner1Name + ",< br />< br > " +
                            "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Shift extension application " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).
                            ToString("dd-MMM-yyyy") + " has been cancelled. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
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
                            EmailSubject = "Requested shift extension application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:" +
                            "tahoma; font-size:9pt;\">Dear " + applicantName + ",<br/><br>" +
                            "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Shift extension application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).
                            ToString("dd-MMM-yyyy") + " has been cancelled. </p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                            "(" + CancelledBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = Obj.StaffId,
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
                requestApplicationRepository.CancelShiftExtensionApplication(CTS, CancelledBy);
            }
            else //If the shift extension application has already been cancelled then...
            {
                throw new ApplicationException("You cannot cancel a shift extension request that is already been cancelled.");
            }
            return "OK";
            }
        }
        public List<ShiftExtenstionListItem> GetAllShiftExtensionList(string StaffId)
        {
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
            { 
                var Obj = requestApplicationRepository.GetAllShiftExtensionList(StaffId);
            return Obj;
            }
        }
        public List<ShiftExtenstionListItem> GetAppliedShiftExtensionForMyTeam(string StaffId, string AppliedId, string userRole)
        {
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
            { 
                var Obj = requestApplicationRepository.GetAppliedShiftExtensionForMyTeam(StaffId, AppliedId, userRole);
            return Obj;
            }
        }
        public List<ShiftExtensionPendingApprovalListItem> GetPendingShiftExtensionApprovals(string ReportingManagerId)
        {
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
                return requestApplicationRepository.GetPendingShiftExtensionApprovals(ReportingManagerId);
        }
        public List<ShiftchangeListItemViewModel> GetAppliedShiftChangeList(string staffId)
        {
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
            { 
                var Obj = requestApplicationRepository.GetAppliedShiftChangeList(staffId);
            return Obj;
            }
        }
        public void SaveShiftChangeInformation(ClassesToSave classesToSave, string SecurityGroupId,
              string approvalLevel, string BaseAddress)
        {
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
            { 
                List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
            string approvalOwner1Name = string.Empty;
            string StaffName = string.Empty;
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
            string apprvalOwner = string.Empty;
            string payPeriodValidationMessage = string.Empty;
            CommonBusinessLogic commonBusinessLogic = new CommonBusinessLogic();
            if (Convert.ToDateTime(classesToSave.RA.StartDate).Date < DateTime.Now.Date)
            {
                payPeriodValidationMessage = commonBusinessLogic.ValidateApplicationForPayDate
                    (classesToSave.RA.StaffId,Convert.ToDateTime(classesToSave.RA.StartDate).ToString("yyyy-MM-dd"),
                Convert.ToDateTime(classesToSave.RA.EndDate).ToString("yyyy-MM-dd"));
                if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
                {
                    if (payPeriodValidationMessage.ToUpper() != "VALID")
                    {
                        throw new ApplicationException("Application of past pay cycle cannot be saved");
                    }
                }
            }
            CommonRepository commonRepository = new CommonRepository();
            applicantEmailId = commonRepository.GetEmailIdOfEmployee(classesToSave.RA.StaffId);
            approvalOwner1EmailId = commonRepository.GetEmailIdOfEmployee(classesToSave.AA.ApprovalOwner);

            if (approvalLevel == "1" || string.IsNullOrEmpty(classesToSave.AA.Approval2Owner).Equals(true))
            {
                approvalOwner2EmailId = approvalOwner1EmailId;
                approvalOwner2Name = approvalOwner1Name;
                classesToSave.AA.Approval2Owner = classesToSave.AA.ApprovalOwner;
            }
            else
            {
                approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(classesToSave.AA.Approval2Owner);
                approvalOwner2Name = commonRepository.GetStaffName(classesToSave.AA.Approval2Owner);
            }

            appliedByUserEmailId = commonRepository.GetEmailIdOfEmployee(classesToSave.RA.AppliedBy);
            commonSenderEmailId = commonRepository.GetSenderEmailIdFromEmailSettings();
            applicantName = commonRepository.GetStaffName(classesToSave.RA.StaffId);
            approvalOwner1Name = commonRepository.GetStaffName(classesToSave.AA.ApprovalOwner);
            approvalOwner2Name = commonRepository.GetStaffName(classesToSave.AA.Approval2Owner);
            appliedByUserName = commonRepository.GetStaffName(classesToSave.RA.AppliedBy);

            var NewShiftName = commonRepository.GetNewShiftName(classesToSave.RA.NewShiftId);

            if (classesToSave.RA.AppliedBy.Equals(classesToSave.RA.StaffId))
            {
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    // Send intimation to the applicant
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Request for Shift Change application sent to " + approvalOwner1Name,
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + applicantName + ",<br><br>" +
                        "Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your Shift change application for the date " + Convert.ToDateTime(classesToSave.RA.StartDate).
                        ToString("dd-MMM-yyyy") + " has been sent to your reporting manager "
                        + approvalOwner1Name + " for approval.</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = classesToSave.RA.AppliedBy,
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
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = approvalOwner1EmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Request for Shift Change application of " + applicantName,
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + approvalOwner1Name + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;" +
                        "\">" + applicantName + " has applied for a Shift Change. Shift Change details are given below.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma;" +
                        " font-size:9pt;\"><tr><td style=\"width:20%;\">Employee Code:</td><td style=\"width:80%;\">" + classesToSave.RA.StaffId + "</td></tr>" +
                        "<tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" +
                        "" + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">" +
                        "To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(classesToSave.RA.EndDate).ToString("dd-MMM-yyyy") + "</td>" +
                        "</tr><tr><td style=\"width:20%;\">New Shift:</td><td style=\"width:80%;\">" + NewShiftName + "</td></tr>" +
                        "<tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + classesToSave.RA.Remarks + "</td></tr></table></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this" +
                        " application.</p><p style=\"font-family:tahoma; " +
                        "font-size:9pt;\"> Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\"> " + applicantName + " &nbsp;" +
                        "(" + classesToSave.RA.StaffId + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = classesToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                classesToSave.ESL = emailSendLogs;
            }
            // If the application has applied by the Approval Owner1 or Reporting Manager
            if (classesToSave.RA.AppliedBy.Equals(classesToSave.AA.ApprovalOwner) &&
                classesToSave.RA.AppliedBy != classesToSave.AA.Approval2Owner)
            {
                classesToSave.RA.IsApproved = false;
                classesToSave.AA.ApprovalStatusId = 2;
                classesToSave.AA.Approval2statusId = 1;
                classesToSave.AA.ApprovedBy = classesToSave.RA.AppliedBy;
                classesToSave.AA.ApprovedOn = DateTime.Now;
                classesToSave.AA.Comment = "APPROVED THE SHIFT CHANGE REQUEST.";
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    //Send intimation to the applicant 
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested shift change application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Your shift change application " +
                        "for the date " + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                        " has been applied and approved by " + appliedByUserName + "" +
                        " and sent for an approval.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + appliedByUserName + " &nbsp;(" + classesToSave.RA.AppliedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = classesToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                //Send intimation to Approval Owner2
                if (string.IsNullOrEmpty(classesToSave.AA.Approval2Owner).Equals(false) && string.IsNullOrEmpty(approvalOwner2EmailId)
                    .Equals(false))
                {
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = approvalOwner2EmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Request for Shift Change application of " + applicantName,
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner2Name + "" +
                        ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + approvalOwner1Name + " has applied and approved for a Shift change. Shift change details given below.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma;" +
                        " font-size:9pt;\"><tr><td style=\"width:20%;\">Employee Code:</td><td style=\"width:80%;\">" + classesToSave.RA.StaffId + "</td></tr>" +
                        "<tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" +
                        "" + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">" +
                        "To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(classesToSave.RA.EndDate).ToString("dd-MMM-yyyy") + "</td>" +
                        "</tr><tr><td style=\"width:20%;\">New Shift:</td><td style=\"width:80%;\">" + NewShiftName + "</td></tr>" +
                        "<tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + classesToSave.RA.Remarks + "</td></tr></table></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this" +
                        " application.</p><p style=\"font-family:tahoma; " +
                        "font-size:9pt;\"> Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + approvalOwner1Name + " &nbsp;(" + classesToSave.RA.AppliedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = classesToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                classesToSave.ESL = emailSendLogs;
            }
            // If the application has applied by Approval Owner2 or Reviewer
            else if (classesToSave.RA.AppliedBy.Equals(classesToSave.AA.Approval2Owner))
            {
                isFinalLevelApproval = true;
                classesToSave.RA.IsApproved = true;
                classesToSave.AA.ApprovalStatusId = 2;
                classesToSave.AA.ApprovedBy = classesToSave.RA.AppliedBy;
                classesToSave.AA.ApprovedOn = DateTime.Now;
                classesToSave.AA.Approval2statusId = 2;
                classesToSave.AA.Approval2By = classesToSave.RA.AppliedBy;
                classesToSave.AA.Approval2On = DateTime.Now;
                classesToSave.AA.Comment = "APPROVED THE SHIFT CHANGE REQUEST.";

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested shift change application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Your shift change application" +
                        " for the date " + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy")
                        + " has been applied and approved.</p><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">" + "Best Regards</p><p style=\"font-family:tahoma; " +
                        "font-size:9pt;\">" + approvalOwner1Name + " &nbsp;" +
                        "(" + classesToSave.AA.ApprovalOwner + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = classesToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                classesToSave.ESL = emailSendLogs;
            }

            if (classesToSave.RA.AppliedBy != classesToSave.RA.StaffId && (SecurityGroupId.Equals("1") || SecurityGroupId.Equals("3")
                || SecurityGroupId.Equals("5")))
            {
                isFinalLevelApproval = true;
                classesToSave.RA.IsApproved = true;
                classesToSave.AA.ApprovalOwner = classesToSave.RA.AppliedBy;
                classesToSave.AA.Approval2Owner = classesToSave.RA.AppliedBy;
                classesToSave.AA.ApprovalStatusId = 2;
                classesToSave.AA.ApprovedBy = classesToSave.RA.AppliedBy;
                classesToSave.AA.ApprovedOn = DateTime.Now;
                classesToSave.AA.Approval2statusId = 2;
                classesToSave.AA.Approval2By = classesToSave.RA.AppliedBy;
                classesToSave.AA.Approval2On = DateTime.Now;
                classesToSave.AA.Comment = "APPROVED THE SHIFT CHANGE REQUEST.";

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested shift change application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your shift change application for the date " + Convert.ToDateTime(classesToSave.RA.StartDate).
                        ToString("dd-MMM-yyyy") + " has been applied and approved.</p><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + appliedByUserName + " &nbsp;(" + classesToSave.RA.AppliedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = classesToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                classesToSave.ESL = emailSendLogs;
            }
            classesToSave.ESL = emailSendLogs;
            requestApplicationRepository.SaveShiftExtensionDetails(classesToSave, isFinalLevelApproval);
        }
        }
        public void ApproveShiftChangeApplication(string Id, string ApprovedBy, string parantType)
        {
            //Get the shift change application details based on the Id passed to this function as a parameter.
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
            { 
                var Obj = requestApplicationRepository.GetRequestApplicationDetails(Id);
            var AA = requestApplicationRepository.GetApplicationApproval(Id, parantType);
            ClassesToSave classesToSave = new ClassesToSave();
            List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
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
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string approvalOwner2Name = string.Empty;
            string payPeriodValidationMessage = string.Empty;

            CommonRepository commonRepository = new CommonRepository();
            applicantEmailId = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
            applicantName = commonRepository.GetStaffName(Obj.StaffId);
            approvalOwner1 = commonRepository.GetApproverOwner(Id);
            approvalOwner2 = commonRepository.GetReviewerOwner(Id);
            approvedBy = ApprovedBy;
            approvedByUserName = commonRepository.GetStaffName(approvedBy);
            approvedByUserEmailId = commonRepository.GetEmailIdOfEmployee(approvedBy);
            commonSenderEmailId = commonRepository.GetSenderEmailIdFromEmailSettings();
            var NewShiftName = commonRepository.GetNewShiftName(Obj.NewShiftId);

            // Check the approval level and then get the approval owner 2 name and email id
            approvalOwner2Name = commonRepository.GetStaffName(approvalOwner2);
            approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner2);

            if (Obj.AppliedBy == "")
            {
                Obj.AppliedBy = Obj.StaffId;
            }
            payPeriodValidationMessage = commonRepository.ValidateApplicationForPayDate(Obj.StaffId, Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy"),
                 Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy"));
            if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
            {
                if (payPeriodValidationMessage.ToUpper() != "VALID")
                {
                    throw new ApplicationException("Application of past pay cycle cannot be approved");
                }
            }
            if (Obj.IsRejected.Equals(true))
            {
                throw new ApplicationException("Cannot approve already rejected shift change request.");
            }
            else
            {
                if (approvedBy.Equals(approvalOwner1) && approvedBy != approvalOwner2)
                {
                    //Approve the application.
                    Obj.IsApproved = false;
                    AA.ApprovalStatusId = 2;
                    AA.ApprovedBy = approvedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "APPROVED THE SHIFT CHANGE REQUEST.";
                    //Send intimation to Approval Owner2
                    if (string.IsNullOrEmpty(approvalOwner2).Equals(false) && string.IsNullOrEmpty(approvalOwner2EmailId)
                       .Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested shift change application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner2Name + "" +
                            ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + applicantName + " has applied for a shift change. Shift Change details are given below.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma;" +
                            " font-size:9pt;\"><tr><td style=\"width:20%;\">Employee Code:</td><td style=\"width:80%;\">" + Obj.StaffId + "</td></tr>" +
                            "<tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" +
                            "" + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">" +
                            "To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + "</td>" +
                            "</tr><tr><td style=\"width:20%;\">New Shift:</td><td style=\"width:80%;\">" + NewShiftName + "</td></tr>" +
                            "<tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + Obj.Remarks + "</td></tr></table></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this" +
                            " application</p><p style=\"font-family:tahoma;" +
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
                }
                // If the applicaation has approevd by Approval Owner2
                else if (approvedBy.Equals(approvalOwner2))
                {
                    //Approve the application.
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
                    AA.Comment = "APPROVED THE SHIFT CHANGE REQUEST.";
                }
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested shift change application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Your shift change application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
                                " has been approved.</p><p style=\"font-family:tahoma; font-size:9pt;\"></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\"></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "" + approvedByUserName + " &nbsp;(" + approvedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = approvedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                classesToSave.RA = Obj;
                classesToSave.AA = AA;
                classesToSave.ESL = emailSendLogs;
                requestApplicationRepository.ApproveShiftChangeApplication(classesToSave, isFinalLevelApproval);
            }
            }
        }
        public void RejectShiftChangeApplication(string Id, string RejectedBy, string parentType)
        {
            //Get the shift change application details based on the id passed to this function as a parameter.
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
            { 
                var Obj = requestApplicationRepository.GetRequestApplicationDetails(Id);
            var AA = requestApplicationRepository.GetApplicationApproval(Id, parentType);
            ClassesToSave classesToSave = new ClassesToSave();
            List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string LeaveTypeName = string.Empty;
            string approvalOwner1Name = string.Empty;
            string approvalOwner2Name = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string rejectedByUserName = string.Empty;
            string rejectedByUserEmailid = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string payPeriodValidationMessage = string.Empty;
            CommonRepository commonRepository = new CommonRepository();
            applicantName = commonRepository.GetStaffName(Obj.StaffId);
            applicantEmailId = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
            approvalOwner1 = commonRepository.GetApproverOwner(Id);
            approvalOwner2 = commonRepository.GetReviewerOwner(Id);
            approvalOwner1Name = commonRepository.GetStaffName(approvalOwner1);
            approvalOwner2Name = commonRepository.GetStaffName(approvalOwner2);
            rejectedByUserName = commonRepository.GetStaffName(RejectedBy);
            rejectedByUserEmailid = commonRepository.GetEmailIdOfEmployee(RejectedBy);
            approvalOwner1EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner2);
            commonSenderEmailId = commonRepository.GetSenderEmailIdFromEmailSettings();
            if (Obj.AppliedBy == "")
            {
                Obj.AppliedBy = Obj.StaffId;
            }
            payPeriodValidationMessage = commonRepository.ValidateApplicationForPayDate(Obj.StaffId, Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy"),
                 Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy"));
            if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
            {
                if (payPeriodValidationMessage.ToUpper() != "VALID")
                {
                    throw new ApplicationException("Application of past pay cycle cannot be rejected");
                }
            }
            if (Obj.IsRejected.Equals(true))  //if the shift change application has been rejected then...
            {
                throw new ApplicationException("Rejected shift change request cannot be rejected.");
            }
            else //if the shift change application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {
                if (RejectedBy == approvalOwner1 && RejectedBy != approvalOwner2)
                {
                    //reject the application.
                    Obj.IsRejected = true;
                    AA.ApprovalStatusId = 3;
                    AA.ApprovedBy = RejectedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "SHIFT CHANGE REQUEST HAS BEEN REJECTED BY THE REVIEWER.";
                }
                else if (RejectedBy == approvalOwner2)
                {
                    //Reject the application.
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
                    AA.Comment = "SHIFT CHANGE REQUEST HAS BEEN REJECTED BY THE REPORTING MANAGER.";
                }
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested shift change application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your shift change application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " has been rejected.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
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
                classesToSave.RA = Obj;
                classesToSave.AA = AA;
                classesToSave.ESL = emailSendLogs;
                requestApplicationRepository.RejectShiftChangeApplication(classesToSave);
            }
        }
        }
        public List<WeeklyCalender> GetWeeklyCalenderList(string StaffId)
        {
            using (RequestApplicationRepository requestApplicationRepository = new RequestApplicationRepository())
                return requestApplicationRepository.GetWeeklyCalenderList(StaffId);
        }
    }
}