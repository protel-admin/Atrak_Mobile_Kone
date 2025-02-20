﻿using System;
using System.Collections.Generic;
using Attendance.Model;
using Attendance.Repository;
using System.Web.Mvc;

namespace Attendance.BusinessLogic
{
    public class RAOnDutyApplicationBusinessLogic
    {
        public List<RAODRequestApplication> GetAppliedOnDutyRequest(string StaffId, string AppliedId, string userRole, string ApplicationType)
        {
            using (RAOnDutyApplicationRepository repo = new RAOnDutyApplicationRepository())
            { 
                var Obj = repo.GetAppliedODRequest(StaffId, AppliedId, userRole, ApplicationType);
            return Obj;
            }
        }

        public List<RAODRequestApplication> GetAppliedOnDutyRequestForMyTeam(string StaffId, string AppliedBy, string userRole, string ApplicationType)
        {
            using (RAOnDutyApplicationRepository repo = new RAOnDutyApplicationRepository())
            { 
                var Obj = repo.GetAppliedODRequestForMyTeam(StaffId, AppliedBy, userRole, ApplicationType);
            return Obj;
            }
        }
        public string ValidateApplicationOverlaping(string StaffId, string FromDate, int FromDurationiD, string ToDate, int ToDurationiD)
        {
            using (RAOnDutyApplicationRepository repo = new RAOnDutyApplicationRepository())
            {
                var str = repo.ValidateApplicationOverlaping(StaffId, FromDate, FromDurationiD, ToDate, ToDurationiD);
            if (!str.ToUpper().StartsWith("OK"))
            {
                throw new Exception(str);
            }
            return str;
            }
        }

        public List<RAODRequestApplication> GetAllODList(string StaffId, string AppliedBy, string userRole, string ApplicationType)
        {
            using (RAOnDutyApplicationRepository repo = new RAOnDutyApplicationRepository())
            { 
                var Obj = repo.GetAllODList(StaffId, AppliedBy, userRole, ApplicationType);
            return Obj;
            }
        }

        public string GetUniqueId()
        {
            using (var repo = new RAOnDutyApplicationRepository())
                return repo.GetUniqueId();
        }

 public List<RAODRequestApplication> GetApprovedOnDutyForMyteam(string staffId)
        {
            using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
            {
                return rAOnDutyApplicationRepository.GetApprovedOnDutyForMyTeam(staffId);
            }
        }

 public List<RAODRequestApplication> GetApprovedBusinessTravelForMyTeam(string staffId)
        {
            using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
            {
                return rAOnDutyApplicationRepository.GetApprovedBTRequestForMyTeam(staffId);
            }
        }

        public void SaveRequestApplication(ClassesToSave DataToSave, string SecurityGroupId, string senderEmail, string BaseAddress)
        {
            using (RAOnDutyApplicationRepository repo = new RAOnDutyApplicationRepository())
            { 
                var cm = new CommonRepository();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            //first send acknowledgement email to the user.
            string ReportingManagerName = string.Empty;
            string applicantName = string.Empty;
            string LeaveTypeName = string.Empty;
            string applicantEmailId = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string senderEmailId = string.Empty;
            string appliedByUserEmailId = string.Empty;
            string appliedByUserName = string.Empty;
            string approvalOwner2Name = string.Empty;
            string ODFrom = string.Empty;
            string ODTo = string.Empty;
            string Total = string.Empty;
            string Duration = string.Empty;
            bool isFinalLevelApproval = false;
            string payPeriodValidationMessage = string.Empty;
            CommonBusinessLogic CBL = new CommonBusinessLogic();

            ReportingManagerName = cm.GetStaffName(DataToSave.AA.ApprovalOwner);
            applicantName = cm.GetStaffName(DataToSave.RA.StaffId);
            applicantEmailId = cm.GetEmailIdOfEmployee(DataToSave.RA.StaffId);
            approvalOwner1EmailId = cm.GetEmailIdOfEmployee(DataToSave.AA.ApprovalOwner);
            approvalOwner2EmailId = cm.GetEmailIdOfEmployee(DataToSave.AA.Approval2Owner);
            appliedByUserEmailId = cm.GetEmailIdOfEmployee(DataToSave.RA.AppliedBy);
            commonSenderEmailId = cm.GetSenderEmailIdFromEmailSettings();
            approvalOwner2Name = cm.GetStaffName(DataToSave.AA.Approval2Owner);
            appliedByUserName = cm.GetStaffName(DataToSave.RA.AppliedBy);

            payPeriodValidationMessage = CBL.ValidateApplicationForPayDate(DataToSave.RA.StaffId ,Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy"),
                    Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy"));
            if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
            {
                if (payPeriodValidationMessage.ToUpper() != "VALID")
                {
                    throw new ApplicationException("Application of past pay cycle cannot be saved");
                }
            }

            var ccconfig = DataToSave.RA.RequestApplicationType;
            if (ccconfig == "OD")
            {
                ccconfig = "ON DUTY";
            }
            else if (ccconfig == "BT")
            {
                ccconfig = "BUSINESS TRAVEL";
            }
            else
            {
                ccconfig = "WORK FROM HOME";
            }

            if (DataToSave.RA.ODDuration == "SINGLEDAY")
            {
                ODFrom = Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy HH:mm");
                ODTo = Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy HH:mm");
                Total = Convert.ToDateTime(DataToSave.RA.TotalHours).ToString("HH:mm");
                Duration = "Single Day";
            }
            else
            {
                ODFrom = Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy");
                ODTo = Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy");
                Total = DataToSave.RA.TotalDays.ToString();
                Duration = "Multiple Days";
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
                        EmailSubject = "Request for " + ccconfig + " application sent to " + ReportingManagerName,
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + applicantName + ",<br><br>" +
                        "Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your " + ccconfig + "  application from " + ODFrom + " to " + ODTo + " has been sent to your Reporting Manager " +
                        " (" + ReportingManagerName + ") for approval." +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>",
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
                        EmailSubject = "Request for " + ccconfig + " application of " + applicantName,
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + "" +
                        ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + applicantName + " has applied for a " + ccconfig + ". " + ccconfig + " details are given below.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                        "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr>" +
                        "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">OD Duration:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Duration + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">From:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + ODFrom + "</td></tr>" +
                        "<tr><td style=\"width:20%;" +
                        "font-family:tahoma; font-size:9pt;\"> To:</td><td style=\"width:80%;" +
                        "font-family:tahoma; font-size:9pt;\">" + ODTo + "</td></tr><tr><td style=\"width:20%;font-family:tahoma;" +
                        " font-size:9pt;\">Total Hours/Days:</td><td style=\"width:80%;font-family:tahoma;" +
                        " font-size:9pt;\">" + Total +
                        "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.Remarks + "</td>" +
                        "</tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required to approve or reject " +
                        " this application.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
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

                //if (DataToSave.RA.ODDuration == "MULTIPLEDAY")
                //{
                //    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                //    {

                //        // Send intimation to the applicant
                //        ESL.Add(new EmailSendLog
                //        {
                //            From = commonSenderEmailId,
                //            To = applicantEmailId,
                //            CC = string.Empty,
                //            BCC = string.Empty,
                //            EmailSubject = "Request for " + ccconfig + " application sent to " + ReportingManagerName,
                //            EmailBody = "<html><head><title></title></head><body>" +
                //            "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br>" +
                //            "Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                //            "Your " + ccconfig + " application has been sent to your Reporting Manager " +
                //            " (" + ReportingManagerName + ") for approval.<p style=\"font-family:tahoma;" +
                //            " font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma;" +
                //            " font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;" +
                //            "\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" +
                //            "" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma;" +
                //            " font-size:9pt;\">From Date:</td><td style=\"width:80%;font-family:tahoma;" +
                //            " font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd/MM/yyyy") + "</td></tr>" +
                //            "<tr><td style=\"width:20%;font-family:tahoma;" +
                //            " font-size:9pt;\">To Date:</td><td style=\"width:80%;font-family:tahoma; " +
                //            "font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd/MM/yyyy") + "</td></tr>" +
                //            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Hours:</td>" +
                //            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" +
                //            "" + DataToSave.RA.TotalDays + "</td></tr>" +
                //            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                //            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.Remarks + "</td>" +
                //            "</tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>",
                //            CreatedOn = DateTime.Now,
                //            CreatedBy = DataToSave.RA.AppliedBy,
                //            IsSent = false,
                //            SentOn = null,
                //            IsError = false,
                //            ErrorDescription = "-",
                //            SentCounter = 0
                //        });

                //    }

                //Send intimation email to the reporting manager.

                //    if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                //    {
                //        senderEmailId = applicantEmailId;
                //        if (string.IsNullOrEmpty(applicantEmailId).Equals(true))
                //        {
                //            senderEmailId = commonSenderEmailId;
                //        }
                //        ESL.Add(new EmailSendLog
                //        {
                //            From = senderEmailId,
                //            To = approvalOwner1EmailId,
                //            CC = string.Empty,
                //            BCC = string.Empty,
                //            EmailSubject = "Request for " + ccconfig + " application of " + StaffName,
                //            EmailBody = "<html><head><title></title></head><body>" +
                //            "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br>" +
                //            "Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                //            "Your " + ccconfig + " application has been sent to your Reporting Manager " +
                //            " (" + ReportingManagerName + ") for approval.<p style=\"font-family:tahoma;" +
                //            " font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma;" +
                //            " font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;" +
                //            "\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" +
                //            "" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma;" +
                //            " font-size:9pt;\">From Date:</td><td style=\"width:80%;font-family:tahoma;" +
                //            " font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd/MM/yyyy") + "</td></tr>" +
                //            "<tr><td style=\"width:20%;font-family:tahoma;" +
                //            " font-size:9pt;\">To Date:</td><td style=\"width:80%;font-family:tahoma; " +
                //            "font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd/MM/yyyy") + "</td></tr>" +
                //            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Hours:</td>" +
                //            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" +
                //            "" + DataToSave.RA.TotalDays + "</td></tr>" +
                //            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                //            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.Remarks + "</td>" +
                //            "</tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>",
                //            CreatedOn = DateTime.Now,
                //            CreatedBy = DataToSave.RA.AppliedBy,
                //            IsSent = false,
                //            SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                //            IsError = false,
                //            ErrorDescription = "-",
                //            SentCounter = 0
                //        });
                //    }
                //}
                DataToSave.ESL = ESL;
            }

            // If the application has applied by the Approval Owner1 or Reporting Manager
            if (DataToSave.RA.AppliedBy.Equals(DataToSave.AA.ApprovalOwner) && DataToSave.RA.AppliedBy != DataToSave.AA.Approval2Owner)
            {
                DataToSave.RA.IsApproved = false;
                DataToSave.AA.ApprovalStatusId = 2;
                DataToSave.AA.Approval2statusId = 1;
                DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                DataToSave.AA.ApprovedOn = DateTime.Now;
                DataToSave.AA.Comment = "REVIEWED THE " + ccconfig + " REQUEST.";

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    //Send intimation to the applicant 
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested " + ccconfig + " application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Your " + ccconfig + "  application " +
                        " from " + ODFrom +
                        " to " + ODTo + " has been applied and approved by " + appliedByUserName + "" +
                        " and sent for second level approval..</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
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
                if (DataToSave.AA.Approval2Owner != null && string.IsNullOrEmpty(approvalOwner2EmailId)
                    .Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = approvalOwner2EmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Request for " + ccconfig + " application of " + applicantName,
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner2Name + "" +
                        ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + ReportingManagerName + " has applied and approved for a " + ccconfig + " application. " +
                        " Details are given below.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                        "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr>" +
                        "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">OD Duration:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Duration + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">From:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + ODFrom + "</td></tr>" +
                        "<tr><td style=\"width:20%;" +
                        "font-family:tahoma; font-size:9pt;\"> To:</td><td style=\"width:80%;" +
                        "font-family:tahoma; font-size:9pt;\">" + ODTo + "</td></tr><tr><td style=\"width:20%;font-family:tahoma;" +
                        " font-size:9pt;\">Total Hours/Days:</td><td style=\"width:80%;font-family:tahoma;" +
                        " font-size:9pt;\">" + Total +
                        "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.Remarks + "</td>" +
                        "</tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required to approve or reject " +
                        " this application.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "<a href=\"" + BaseAddress + "\">Redirect To Site</a></p><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + ReportingManagerName + " &nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>",
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
                if (DataToSave.RA.ODDuration == "SINGLEDAY")
                {
                    isFinalLevelApproval = true;
                    DataToSave.RA.IsApproved = true;
                    DataToSave.AA.ApprovalStatusId = 2;
                    DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedOn = DateTime.Now;
                    DataToSave.AA.Approval2statusId = 2;
                    DataToSave.AA.Approval2By = DataToSave.RA.AppliedBy;
                    DataToSave.AA.Approval2On = DateTime.Now;
                    DataToSave.AA.Comment = "APPROVED THE " + ccconfig + " REQUEST.";

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested " + ccconfig + " application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your " + ccconfig + "  application" +
                            " for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy")
                            + " has been applied and approved..</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">" + "Best Regards</p><p style=\"font-family:tahoma; " +
                            "font-size:9pt;\">" + ReportingManagerName + " &nbsp;" +
                            "(" + DataToSave.AA.ApprovalOwner + ")</p></body></html>",
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

                if (DataToSave.RA.ODDuration == "MULTIPLEDAY")
                {
                    isFinalLevelApproval = true;
                    DataToSave.RA.IsApproved = true;
                    DataToSave.AA.ApprovalStatusId = 2;
                    DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedOn = DateTime.Now;
                    DataToSave.AA.Approval2statusId = 2;
                    DataToSave.AA.Approval2By = DataToSave.RA.AppliedBy;
                    DataToSave.AA.Approval2On = DateTime.Now;
                    DataToSave.AA.Comment = "APPROVED THE " + ccconfig + " REQUEST.";

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested " + ccconfig + " application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your " + ccconfig + "  application" +
                            " From the date " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy")
                            + " to " + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd - MMM - yyyy") +
                            " has been applied and approved..</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">" + "Best Regards</p><p style=\"font-family:tahoma; " +
                            "font-size:9pt;\">" + ReportingManagerName + " &nbsp;" +
                            "(" + DataToSave.AA.ApprovalOwner + ")</p></body></html>",
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
                DataToSave.ESL = ESL;
            }

            if ((SecurityGroupId.Equals("1") || SecurityGroupId.Equals("3") || SecurityGroupId.Equals("5")) &&
                DataToSave.RA.AppliedBy != DataToSave.RA.StaffId && DataToSave.RA.AppliedBy != DataToSave.AA.ApprovalOwner)
            {
                if (DataToSave.RA.ODDuration == "SINGLEDAY")
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
                    DataToSave.AA.Comment = "APPROVED THE " + ccconfig + " REQUEST.";

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested " + ccconfig + " application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your " + ccconfig + "  application for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).
                            ToString("dd-MMM-yyyy") + " has been applied and approved..</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
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
                }

                if (DataToSave.RA.ODDuration == "MULTIPLEDAY")
                {
                    if (DataToSave.RA.StaffId != DataToSave.RA.AppliedBy)
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
                        DataToSave.AA.Comment = "APPROVED THE " + ccconfig + " REQUEST.";

                        if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                        {
                            ESL.Add(new EmailSendLog
                            {
                                From = commonSenderEmailId,
                                To = applicantEmailId,
                                CC = string.Empty,
                                BCC = string.Empty,
                                EmailSubject = "Requested " + ccconfig + " application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Your " + ccconfig + "  application From the date " + Convert.ToDateTime(DataToSave.RA.StartDate).
                                ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(DataToSave.RA.EndDate).
                                ToString("dd-MMM-yyyy") + "  has been applied and approved..</p><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
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
                    }
                }
                DataToSave.ESL = ESL;
            }
            repo.SaveRequestApplication(DataToSave, isFinalLevelApproval);
            }
        }


public List<RAODRequestApplication> GetApprovedWFHForMyTeam(string staffId)
        {
            using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
            {
                return rAOnDutyApplicationRepository.GetApprovedWFHRequestForMyTeam(staffId);
            }
        }


        public string ValidateBeforeSave(string StaffId, string FromDate, string ToDate, string Duration)
        {
            using (RAOnDutyApplicationRepository repo = new RAOnDutyApplicationRepository())
            { 
                var str = repo.ValidateBeforeSave(StaffId, FromDate, ToDate, Duration);
            if (!str.ToUpper().StartsWith("OK"))
            {
                throw new Exception(str);
            }
            return str;
            }
        }

        public void RejectApplication(string Id, string RejectedBy, string ApplicationType)
        {
            //Get the OD/BT application details based on the id passed to this function as a parameter.
            using (RAOnDutyApplicationRepository repo = new RAOnDutyApplicationRepository())
            { 
                var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id, ApplicationType);
            var cm = new CommonRepository();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            ClassesToSave CTS = new ClassesToSave();
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string LeaveTypeName = string.Empty;
            string approvalOwnerName = string.Empty;
            string approvalOwner2Name = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string rejectedByUserName = string.Empty;
            string rejectedByUserEmailid = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string ODFrom = string.Empty;
            string ODTo = string.Empty;
            string Total = string.Empty;
            string Duration = string.Empty;
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
            var mailbody = string.Empty;
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
            if (Obj.ODDuration == "SINGLEDAY")
            {
                ODFrom = Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy HH:mm");
                ODTo = Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy HH:mm");
                Total = Convert.ToDateTime(Obj.TotalHours).ToString("HH:mm");
                Duration = "Single Day";
            }
            else
            {
                ODFrom = Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy");
                ODTo = Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy");
                Total = Obj.TotalDays.ToString();
                Duration = "Multiple Days";
            }
            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true))    //if the leave application has been cancelled then...
            {
                throw new Exception("Cancelled " + ApplicationType + " request cannot be rejected.");
            }
            else if (AA.ApprovalStatusId == 2 && RejectedBy == approvalOwner1) //if the leave application has been approved then...
            {
                throw new Exception("Approved " + ApplicationType + " request cannot be rejected.");
            }
            else if (AA.Approval2statusId == 2 && RejectedBy == approvalOwner2) //if the leave application has been approved then...
            {
                throw new Exception("Approved " + ApplicationType + " request cannot be rejected.");
            }
            else if (Obj.IsRejected.Equals(true))  //if the leave application has been rejected then...
            {
                throw new Exception("Rejected " + ApplicationType + " request cannot be rejected.");
            }
            else //if the OD/BT application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {
                //reject the application.
                if (RejectedBy == approvalOwner1 && RejectedBy != approvalOwner2)
                {
                    Obj.IsRejected = true;
                    AA.ApprovalStatusId = 3;
                    AA.ApprovedBy = RejectedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "" + ApplicationType + " REQUEST HAS BEEN REJECTED BY THE APPROVALOWNER1.";
                }
                if (RejectedBy == approvalOwner2)
                {
                    //reject the application.
                    Obj.IsRejected = true;
                    if (approvalOwner1 == approvalOwner2)
                    {
                        AA.ApprovalStatusId = 3;
                        AA.ApprovedBy = RejectedBy;
                        AA.ApprovedOn = DateTime.Now;
                    }
                    AA.Approval2statusId = 3;
                    AA.Approval2By = RejectedBy;
                    AA.Approval2On = DateTime.Now;

                    AA.Comment = "" + ApplicationType + " REQUEST HAS BEEN REJECTED BY THE APPROVALOWNER2.";
                }
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                            mailbody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                             "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                             "Your " + ApplicationType + " application from " + ODFrom + " to " +
                            ODTo + " for " + Duration + "  has been rejected." +
                             "</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; " +
                             "font-size:9pt;\">" + rejectedByUserName + " &nbsp;(" + RejectedBy + ")</p></body></html>";
                    //email to applicant
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested " + ApplicationType + " application status",
                        EmailBody = mailbody,
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
        public void ApproveApplication(string Id, string ApprovedBy, string ApplicationType)
        {
            //Get the OD/BT application details based on the Id passed to this function as a parameter.
            using (RAOnDutyApplicationRepository repo = new RAOnDutyApplicationRepository())
            { 
                var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id, ApplicationType);
            var cm = new CommonRepository();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
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
            string LeaveTypeName = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string approvalOwner2Name = string.Empty;
            var mailBody1 = string.Empty;
            string mailBody2 = string.Empty;
            string ODDuration = string.Empty;
            string ODFrom = string.Empty;
            string ODTo = string.Empty;
            string total = string.Empty;
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
            approvalOwner2EmailId = cm.GetEmailIdOfEmployee(approvalOwner2);
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
            //Check if the OD/BT application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true)) //if the OD/BT application has been cancelled then...
            {
                throw new Exception("Cannot approve a cancelled " + ApplicationType + " application. Apply for a new " + ApplicationType + " application.");
            }
            else if (AA.ApprovalStatusId == 2 && ApprovedBy == approvalOwner1) //if application has already been approved then...
            {
                throw new Exception("Cannot approve already approved application.");
            }
            else if (AA.Approval2statusId == 2 && ApprovedBy == approvalOwner2) //if application has already been approved then...
            {
                throw new Exception("Cannot approve already approved application.");
            }
            else if (Obj.IsRejected.Equals(true))
            {
                throw new Exception("Cannot approve already rejected " + ApplicationType + " request.");
            }
            else
            {
                if (Obj.ODDuration == "MULTIPLEDAY")
                {

                    ODDuration = "Multiple Days";
                    ODFrom = Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy");
                    ODTo = Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy");
                    total = Convert.ToString(Obj.TotalDays);
                }
                else if (Obj.ODDuration == "SINGLEDAY")
                {

                    ODDuration = "Single Day";
                    ODFrom = Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy HH:mm");
                    ODTo = Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy HH:mm");
                    total = Convert.ToString(Obj.TotalDays);
                }
                
                // If the application has approved by Reporting Manager or approval owner 1
                if (approvedBy.Equals(approvalOwner1) && approvedBy != approvalOwner2)
                {
                    //approve the application.
                    Obj.IsApproved = false;
                    AA.ApprovalStatusId = 2;
                    AA.ApprovedBy = approvedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "APPROVED THE " + ApplicationType + " REQUEST.";

                    mailBody1 = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                           "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                           "Your " + ApplicationType + " application from " + ODFrom +
                           " to " + ODTo + " for " + ODDuration +
                           "  has been approved and sent for second level approval.</p><p style=\"font-family:tahoma; font-size:9pt;\"></p>" +
                           "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                           "<p style=\"font-family:tahoma; font-size:9pt;\"></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                           "" + approvedByUserName + " &nbsp;(" + approvedBy + ")</p></body></html>";
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested " + ApplicationType + " application status",
                            EmailBody = mailBody1,
                            CreatedOn = DateTime.Now,
                            CreatedBy = approvedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }

                    if (string.IsNullOrEmpty(approvalOwner2).Equals(false) &&
                    string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {

                        AA.Comment = "APPROVED THE " + ApplicationType + " REQUEST.";

                        mailBody2 = "<html><head><title></title></head><body>" +
                         "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner2Name + "" +
                         ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                         "" + applicantName + " has applied for a " + ApplicationType + ". " + ApplicationType + " details are given below.</p>" +
                         "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                         "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr>" +
                         "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                         "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Obj.StaffId + "</td></tr>" +
                         "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">OD Duration:</td>" +
                         "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + ODDuration + "</td></tr>" +
                         "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">From:</td>" +
                         "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + ODFrom + "</td></tr>" +
                         "<tr><td style=\"width:20%; font-family:tahoma; font-size:9pt;\"> To:</td><td style=\"width:80%;" +
                         "font-family:tahoma; font-size:9pt;\">" + ODTo + "</td></tr><tr><td style=\"width:20%;font-family:tahoma;" +
                         " font-size:9pt;\">Total Hours/Days:</td><td style=\"width:80%;font-family:tahoma;" +
                         " font-size:9pt;\">" + total + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                         "Reason:</td> <td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Obj.Remarks + "</td>" +
                         "</tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required " +
                        "to approve or reject this application.</p><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + approvedByUserName + " &nbsp;(" + ApprovedBy + ")</p></body></html>";

                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested " + ApplicationType + " application status",
                            EmailBody = mailBody2,
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
                    AA.Comment = "APPROVED THE " + ApplicationType + " REQUEST.";

                }
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    mailBody1 = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your " + ApplicationType + " application from " + ODFrom +
                        " to " + ODTo + " for " + ODDuration +
                        "  has been approved.</p><p style=\"font-family:tahoma; font-size:9pt;\"></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + approvedByUserName + " &nbsp;(" + approvedBy + ")</p></body></html>";
                }

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested " + ApplicationType + " application status",
                        EmailBody = mailBody1,
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
            ClassesToSave CTS = new ClassesToSave();
            CTS.RA = Obj;
            CTS.AA = AA;
            CTS.ESL = ESL;
            repo.ApproveApplication(CTS, isFinalLevelApproval);
            }
        }

        public string CancelApplication(string Id, string CancelledBy, string ApplicationType)
        {
            //Get the OD/BT application details based on the Id passed to this function as a parameter.
            using (RAOnDutyApplicationRepository repo = new RAOnDutyApplicationRepository())
            { 
                ClassesToSave CTS = new ClassesToSave();
            var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id, ApplicationType);
            var cm = new CommonRepository();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
            CommonBusinessLogic CBL = new CommonBusinessLogic();
            string approvalOwner1Name = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            DateTime? ApplicationDate = DateTime.Now;
            string approvalOwner2Name = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string cancelledByUserEmailId = string.Empty;
            string cancelledByUserName = string.Empty;
            string commonSenderEmailId = string.Empty;
            string senderEmailId = string.Empty;
            string ODDuration = string.Empty;
            string ODFrom = string.Empty;
            string ODTo = string.Empty;
            string total = string.Empty;
            string mailBody = string.Empty;
            string mailBody2 = string.Empty;
            string payPeriodValidationMessage = string.Empty;

            approvalOwner1Name = cm.GetStaffName(AA.ApprovalOwner);
            approvalOwner2Name = cm.GetStaffName(AA.Approval2Owner);
            applicantName = cm.GetStaffName(Obj.StaffId);
            applicantEmailId = cm.GetEmailIdOfEmployee(Obj.StaffId);
            cancelledByUserEmailId = cm.GetEmailIdOfEmployee(CancelledBy);
            cancelledByUserName = cm.GetStaffName(CancelledBy);
            approvalOwner1EmailId = cm.GetEmailIdOfEmployee(AA.ApprovalOwner);
            approvalOwner2EmailId = cm.GetEmailIdOfEmployee(AA.Approval2Owner);
            commonSenderEmailId = cm.GetSenderEmailIdFromEmailSettings();
            if (Obj.AppliedBy == "")
            {
                Obj.AppliedBy = Obj.StaffId;
            }
            //Check if the application has already been cancelled or not.
            if (Obj.IsCancelled.Equals(false))   //If the application has not been cancelled then...
            {
                //Cancel the application.
                Obj.IsCancelled = true;
                Obj.CancelledDate = DateTime.Now;
                Obj.CancelledBy = CancelledBy;
                payPeriodValidationMessage = CBL.ValidateApplicationForPayDate(Obj.StaffId,Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy"),
                   Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy"));
                if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
                {
                    if (payPeriodValidationMessage.ToUpper() != "VALID")
                    {
                        throw new ApplicationException("Application of past pay cycle cannot be cancelled");
                    }
                }
                if (Obj.ODDuration == "MULTIPLEDAY")
                {

                    ODDuration = "Multiple Days";
                    ODFrom = Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy");
                    ODTo = Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy");
                    total = Convert.ToString(Obj.TotalDays);
                }
                else if (Obj.ODDuration == "SINGLEDAY")
                {

                    ODDuration = "Single Day";
                    ODFrom = Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy HH:mm");
                    ODTo = Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy HH:mm");
                    total = Convert.ToString(Obj.TotalDays);
                }
                if (CancelledBy == Obj.StaffId)
                {
                    if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                    {
                        mailBody = string.Empty;
                        mailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            " " + applicantName + " has cancelled the " + ApplicationType + " application from " + ODFrom + " to " + ODTo + " for " +
                            ODDuration + ". <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                            "(" + CancelledBy + ")</p></body></html>";
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner1
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested " + ApplicationType + " application status ",
                            EmailBody = mailBody,
                            CreatedOn = DateTime.Now,
                            CreatedBy = CancelledBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                    if (AA.ApprovalStatusId == 2 && string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                        mailBody2 = string.Empty;
                        mailBody2 = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            " " + applicantName + " has cancelled the " + ApplicationType + " application from " + ODFrom + " to " + ODTo + " for " +
                            ODDuration + ". <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                            "(" + CancelledBy + ")</p></body></html>";

                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner2
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested " + ApplicationType + " application status ",
                            EmailBody = mailBody,
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
                else if (CancelledBy != Obj.StaffId)
                {
                    if (CancelledBy == approvalOwner1 && AA.ApprovalStatusId == 2)
                    {
                        if (string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                        {
                            mailBody2 = string.Empty;
                            mailBody2 = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                " " + approvalOwner1Name + " has cancelled the " + ApplicationType + " application of " + applicantName + " " +
                                " from " + ODFrom + " to " + ODTo + " for " + ODDuration + ". " +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                                "(" + CancelledBy + ")</p></body></html>";

                            ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner2
                            {
                                From = commonSenderEmailId,
                                To = approvalOwner2EmailId,
                                CC = string.Empty,
                                BCC = string.Empty,
                                EmailSubject = "Requested " + ApplicationType + " application status ",
                                EmailBody = mailBody,
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

                    else if (CancelledBy == approvalOwner2 && string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                    {
                        mailBody2 = string.Empty;
                        mailBody2 = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            " " + approvalOwner2Name + " has cancelled the " + ApplicationType + " application of " + applicantName + "" +
                            " from " + ODFrom + " to " + ODTo + " for " + ODDuration + ". <p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                            "(" + CancelledBy + ")</p></body></html>";

                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner2
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested " + ApplicationType + " application status ",
                            EmailBody = mailBody2,
                            CreatedOn = DateTime.Now,
                            CreatedBy = CancelledBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        mailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your " + ApplicationType + " application from " + ODFrom + " to " + ODTo + " for " +
                        ODDuration + " has been cancelled. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                        "(" + CancelledBy + ")</p></body></html>";
                        ESL.Add(new EmailSendLog  //Send Mail to User
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested " + ApplicationType + " application status ",
                            EmailBody = mailBody,
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
                repo.CancelApplication(CTS, CancelledBy);
            }
            else //If the OD/BT application has already been cancelled then...
            {
                //throw exception.
                throw new Exception("You cannot cancel a " + ApplicationType + " request that is already been cancelled.");
            }
            return "OK";
        }
        }

        //public string CancelApprovedApplication(string Id, string StfId, string ApplicationType)
        //{
        //    string staffId = string.Empty;
        //    DateTime fromDate = DateTime.Now;
        //    DateTime toDate = DateTime.Now;
        //    DateTime currentDate = DateTime.Now;
        //    string applicationId = string.Empty;
        //    string applicationType = string.Empty;
        //    string punchtype = string.Empty;
        //    //Get the leave application details based on the Id passed to this function as a parameter.
        //    RAOnDutyApplicationRepository repo = new RAOnDutyApplicationRepository();
        //    ClassesToSave CTS = new ClassesToSave();
        //    var Obj = repo.GetRequestApplicationDetails(Id);
        //    var AA = repo.GetApplicationApproval(Id, ApplicationType);
        //    var cm = new CommonRepository();
        //    List<EmailSendLog> ESL = new List<EmailSendLog>();
        //    //first send acknowledgement email to the user.
        //    string ReportingManagerName = string.Empty;
        //    string StaffName = string.Empty;
        //    string LeaveTypeName = string.Empty;
        //    ReportingManagerName = cm.GetStaffName(StfId);
        //    StaffName = cm.GetStaffName(Obj.StaffId);

        //    var mailbody = string.Empty;
        //    if (Obj.ODDuration == "MULTIPLEDAY")
        //    {
        //        mailbody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your " + ApplicationType + " application from " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + " for " + Obj.ODDuration + "  has been cancelled.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ReportingManagerName + " &nbsp;(" + StfId + ")</p></body></html>";
        //    }
        //    else
        //    {
        //        mailbody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your " + ApplicationType + " application on " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " for " + Obj.ODDuration + "  has been cancelled.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ReportingManagerName + " &nbsp;(" + StfId + ")</p></body></html>";
        //    }

        //    ESL.Add(new EmailSendLog
        //    {
        //        From = cm.GetEmailIdOfEmployee(StfId) ?? string.Empty,
        //        To = cm.GetEmailIdOfStaff(Obj.StaffId) ?? "-",
        //        CC = string.Empty,
        //        BCC = string.Empty,
        //        EmailSubject = "Requested leave application status",
        //        EmailBody = mailbody,
        //        CreatedOn = DateTime.Now,
        //        CreatedBy = StfId,
        //        IsSent = false,
        //        SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
        //        IsError = false,
        //        ErrorDescription = "-",
        //        SentCounter = 0
        //    });
        //    //
        //    //Check whether the starting date of the leave application is below the current date.
        //    var IsFutureDate = IsFromDateMoreOrEqualToCurrerntDate(Obj.StartDate, DateTime.Now);
        //    //
        //    //If the leave application date is future to the current date.
        //    if (IsFutureDate == true)
        //    {
        //        //Check if the leave application has been approved or not.
        //        if (Obj.IsApproved.Equals(true))    //If the leave application has not been approved. (i.e. in the pending state) then...
        //        {
        //            //Check if the leave application has already been cancelled or not.
        //            if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
        //            {
        //                //Cancel the leave application which is in pending state.
        //                Obj.IsCancelled = true;
        //                Obj.CancelledDate = DateTime.Now;
        //                Obj.CancelledBy = StfId;
        //                CTS.RA = Obj;
        //                CTS.ESL = ESL;
        //                CTS.AA = AA;
        //                repo.CancelApplication(CTS, StfId);
        //            }
        //            else   //If the leave application has already been cancelled then...
        //            {
        //                //throw exception (first of all the cancel link must not be visible.)
        //                throw new Exception("You cannot cancel a " + ApplicationType + " request that is already been cancelled.");
        //            }
        //        }
        //        else//If the leave application has been approved then...
        //        {
        //            //Check if the leave application has already been cancelled or not.
        //            if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
        //            {
        //                //Cancel the leave application which is in approved state.
        //                Obj.IsCancelled = true;
        //                Obj.CancelledDate = DateTime.Now;
        //                Obj.CancelledBy = StfId;

        //                CTS.RA = Obj;
        //                CTS.ESL = ESL;
        //                CTS.AA = AA;
        //                //CTS.ELA = ELA;
        //                repo.CancelApplication(CTS, StfId);
        //            }
        //            else //If the leave application has already been cancelled then...
        //            {
        //                //throw exception.
        //                throw new Exception("You cannot cancel a " + ApplicationType + " request that is already been cancelled.");
        //            }
        //        }
        //    }
        //    else  //If the leave application is a past date then...
        //    {
        //        //Check if the leave application has been approved or not.
        //        if (Obj.IsApproved.Equals(true))    //If the leave application has not been approved. (i.e. in the pending state) then...
        //        {
        //            //Check if the leave application has already been cancelled or not.
        //            if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
        //            {
        //                //Cancel the leave application which is in pending state.
        //                Obj.IsCancelled = true;
        //                Obj.CancelledDate = DateTime.Now;
        //                Obj.CancelledBy = StfId;
        //                CTS.RA = Obj;
        //                CTS.ESL = ESL;
        //                repo.CancelApplication(CTS, StfId);

        //                CommonRepository obj = new CommonRepository();

        //                try
        //                {
        //                    var data = obj.GetList(Obj.Id);
        //                    staffId = data.StaffId;
        //                    fromDate = data.FromDate;
        //                    toDate = data.ToDate;
        //                    applicationType = Obj.RequestApplicationType;
        //                    applicationId = Obj.Id;
        //                }
        //                catch (Exception err)
        //                {
        //                    throw err;
        //                }
        //             /*   if (fromDate.Date < currentDate.Date)*/
        //                {
        //                    if (toDate.Date >= currentDate.Date)
        //                    {
        //                        toDate = DateTime.Now.AddDays(-1);
        //                    }
        //                    obj.LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, applicationId);
        //                }
        //            }
        //            else //If the leave application has already been cancelled then...
        //            {
        //                //throw exception (first of all the cancel link must not be visible for the application that has already been cancelled.)
        //                throw new Exception("You cannot cancel a " + ApplicationType + " request that is already been cancelled.");
        //            }
        //        }
        //        else  //If the leave application has been approved then...
        //        {
        //            //Check if the leave application has already been cancelled or not.
        //            if (Obj.IsCancelled.Equals(false))//If the leave application has not been cancelled then...
        //            {
        //                Obj.IsCancelled = true;
        //                Obj.CancelledDate = DateTime.Now;
        //                Obj.CancelledBy = StfId;
        //                CTS.RA = Obj;
        //                repo.CancelApplication(CTS, StfId);
        //            }
        //            else  //If the leave application has already been cancelled then...
        //            {
        //                //throw exception.
        //                throw new Exception("You cannot cancel a " + ApplicationType + " request that is already been cancelled.");
        //            }
        //        }
        //    }

        //    return "Ok";
        //}

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
        
        public void SaveRequestApplication(ClassesToSave DataToSave, string securityGroupId, string baseAddress)
        {
            List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
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
                    payPeriodValidationMessage = commonRepository.ValidateApplicationForPayDate(DataToSave.RA.StaffId,DataToSave.RA.StartDate, DataToSave.RA.EndDate);
        }
                if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false) && payPeriodValidationMessage != "VALID")
        {
                    throw new ApplicationException("Application of past pay cycle cannot be saved ");
                }

                string applicantName = string.Empty;
                string applicantEmailId = string.Empty;
                string approvalOwner1EmailId = string.Empty;
                string approvalOwner2EmailId = string.Empty;
                string commonSenderEmailId = string.Empty;
                string senderEmailId = string.Empty;
                string appliedByUserEmailId = string.Empty;
            string appliedByUserName = string.Empty;
                string approvalOwner1Name = string.Empty;
                string approvalOwner2Name = string.Empty;
                string fromDate = string.Empty;
                string toDate = string.Empty;
                string Duration = string.Empty;
            bool isFinalLevelApproval = false;
                string applicationType = string.Empty;

                using (CommonRepository commonRepository = new CommonRepository())
                {
                    applicantEmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.RA.StaffId);
                    approvalOwner1EmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.AA.ApprovalOwner);
                    approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.AA.Approval2Owner);
                    appliedByUserEmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.RA.AppliedBy);
                    commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
                    appliedByUserName = commonRepository.GetStaffName(DataToSave.RA.AppliedBy);
                    approvalOwner1Name = commonRepository.GetStaffName(DataToSave.AA.ApprovalOwner);
                    applicantName = commonRepository.GetStaffName(DataToSave.RA.StaffId);
                    approvalOwner2Name = commonRepository.GetStaffName(DataToSave.AA.Approval2Owner);
                }
           
                senderEmailId = commonSenderEmailId;
            if (DataToSave.RA.RequestApplicationType.Equals("OD"))
            {
                    applicationType = "On Duty";
            }
            else if (DataToSave.RA.RequestApplicationType.Equals("BT"))
            {
                   
            }
            else
            {
                    applicationType = "Work from Home";
            }
                if (DataToSave.RA.ODDuration.ToUpper().Equals("SINGLEDAY"))
                    {
                    fromDate = Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy HH:mm");
                    toDate = Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy HH:mm");
                    Duration = "Single Day";
                    }
                    else
                    {
                    fromDate = Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy");
                    toDate = Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy");
                    Duration = "Multiple Days";
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
                            EmailSubject = "Request for  " + applicationType + " application sent to " + approvalOwner1Name,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your " + applicationType + " application for the date from " + fromDate + " to " + toDate + " " +
                            "has been submitted to your Reporting Manager " + "(" + approvalOwner1Name + ") for approval." +
                             "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>",
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
                            From = senderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Request for " + applicationType + " application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner1Name + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + applicantName + " has applied for a " + applicationType + " application.Application details are given below.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma;" +
                            " font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">StaffId:</td><td style=\"width:80%;" +
                            "font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr><tr><td style=\"width:20%;font-family:tahoma;" +
                            " font-size:9pt;\">From Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + fromDate + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">To Date:</td><td style=\"width:80%;font-family:tahoma;" +
                            " font-size:9pt;\">" + toDate + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                            "Duration:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Duration + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td><td style=\"width:80%;font-family:tahoma;" +
                            " font-size:9pt;\">" + DataToSave.RA.Remarks + "</td></tr></table></p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Your attention is required to approve or reject this application.</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\"><a href=\"" + baseAddress + "\">10.114.76.61:8011</a></p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + applicantName + " " +
                            "&nbsp; (" + DataToSave.RA.StaffId + ")</p></body></html>",
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

                // If the application has applied by the Reporting Manager or Approval Owner1
                else if (DataToSave.RA.AppliedBy.Equals(DataToSave.AA.ApprovalOwner) && DataToSave.RA.AppliedBy != DataToSave.AA.Approval2Owner)
            {
                DataToSave.RA.IsApproved = true;
                DataToSave.AA.ApprovalStatusId = 2;
                DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                DataToSave.AA.ApprovedOn = DateTime.Now;
                DataToSave.AA.Comment = "APPROVED THE " + DataToSave.RA.RequestApplicationType + " REQUEST.";
                var mailbody = string.Empty;
                    mailbody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                       "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                       "Your " + applicationType + " application from " + fromDate + " to "
                       + toDate + " for " + Duration + "" + " has been applied and approved by " + approvalOwner1Name + "" +
                       " and send for second level approval</p>" + "<p style=\"font-family:tahoma;" + " font-size:9pt;\">Best Regards</p>" +
                       "<p style=\"font-family:tahoma;" + " font-size:9pt;\">" + approvalOwner1Name + "" + "&nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>";
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                        emailSendLogs.Add(new EmailSendLog
                {
                            From = senderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested " + applicationType + " application status",
                            EmailBody = mailbody,
                            CreatedOn = DateTime.Now,
                            CreatedBy = DataToSave.RA.AppliedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                }

                    if (string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                        //Send intimation email to the Approval owner2.
                        emailSendLogs.Add(new EmailSendLog
                {
                            From = senderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                    BCC = string.Empty,
                            EmailSubject = "Request for " + applicationType + " application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner2Name + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + appliedByUserName + " has applied and approved a " + applicationType + " application for " + applicantName + "." +
                            " Application details are given below.</p>" + "<p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "<table border=\"1\" style=\"width:50%;font-family:tahoma;" + " font-size:9pt;\"><tr><td style=\"width:20%;" +
                            "font-family:tahoma; font-size:9pt;\">StaffId:</td><td style=\"width:80%;" + "font-family:tahoma; font-size:9pt;\">" +
                            "" + DataToSave.RA.StaffId + "</td></tr>" + "<tr><td style=\"width:20%;font-family:tahoma;" + " font-size:9pt;\">" +
                            "From Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + fromDate + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">To Date:</td><td style=\"width:80%;" +
                            "font-family:tahoma;" + " font-size:9pt;\">" + toDate + "</td></tr><tr><td style=\"width:20%;font-family:tahoma;" +
                            " font-size:9pt;\">" + "Duration:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" +
                            "" + Duration + "</td></tr>" + "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                            "Reason:</td><td style=\"width:80%;font-family:tahoma;" + " font-size:9pt;\">" + DataToSave.RA.Remarks + "</td></tr>" +
                            "</table></p><p style=\"font-family:tahoma;" + " font-size:9pt;\">Your attention is required to approve or reject this application." +
                            "</p><p style=\"font-family:tahoma;" + " font-size:9pt;\"><a href=\"" + baseAddress + "\">10.114.76.61:8011</a></p>" +
                            "<p style=\"font-family:tahoma;" + " font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">" + appliedByUserName + " " + "&nbsp; (" + DataToSave.RA.AppliedBy + ")</p></body></html>",
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
                // If the application has applied by the Approval Owner2
                else if (DataToSave.AA.Approval2Owner.Equals(DataToSave.RA.AppliedBy))
            {
                isFinalLevelApproval = true;
                DataToSave.RA.IsApproved = true;
                DataToSave.AA.ApprovalStatusId = 2;
                DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                DataToSave.AA.ApprovedOn = DateTime.Now;
                DataToSave.AA.Approval2statusId = 2;
                DataToSave.AA.Approval2By = DataToSave.RA.AppliedBy;
                DataToSave.AA.Approval2On = DateTime.Now;
                    DataToSave.AA.Comment = "APPROVED THE " + DataToSave.RA.RequestApplicationType + " REQUEST.";
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                        emailSendLogs.Add(new EmailSendLog
                {
                            From = senderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested " + applicationType + " application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                           "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                           "Your " + applicationType + " application from " + fromDate + " to "
                           + toDate + " for " + Duration + "" + "  has been applied and approved by " + approvalOwner2Name + ".</p>" +
                           "<p style=\"font-family:tahoma;" + " font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                           " font-size:9pt;\">" + approvalOwner2Name + "" + "&nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>",
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

                if (DataToSave.RA.AppliedBy != DataToSave.RA.StaffId && DataToSave.RA.AppliedBy != DataToSave.AA.ApprovalOwner &&
                    (securityGroupId.Equals("3") || securityGroupId.Equals("5") || securityGroupId.Equals("6")))
                {
                    isFinalLevelApproval = true;
                    DataToSave.RA.IsApproved = true;
                    DataToSave.AA.ApprovalStatusId = 2;
                    DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedOn = DateTime.Now;
                    DataToSave.AA.Approval2statusId = 2;
                    DataToSave.AA.Approval2By = DataToSave.RA.AppliedBy;
                    DataToSave.AA.Approval2On = DateTime.Now;
                    DataToSave.AA.Comment = "APPROVED THE " + DataToSave.RA.RequestApplicationType + " REQUEST.";
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                {
                            From = senderEmailId,
                            To = applicantEmailId,
                    CC = string.Empty,
                    BCC = string.Empty,
                            EmailSubject = "Requested " + applicationType + " application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                           "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                           "Your " + applicationType + " application from " + fromDate + " to "
                           + toDate + " for " + Duration + "" + "  has been applied and approved by " + appliedByUserName + ".</p>" +
                           "<p style=\"font-family:tahoma;" + " font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                           " font-size:9pt;\">" + appliedByUserName + "" + "&nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>",
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
        using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
        {
                    rAOnDutyApplicationRepository.SaveRequestApplication(DataToSave, isFinalLevelApproval);
        }
       }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
