using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;
using System.Web.Mvc;
using System.Configuration;

namespace Attendance.BusinessLogic
{
    public class RAManualPunchApplicationBusinessLogic
    {

        public List<RAManualPunchApplication> GetAppliedManualPunches(string StaffId)
        {
            using (RAManualPunchApplicationRepository repo = new RAManualPunchApplicationRepository())
            { 
                var Obj = repo.GetAppliedManualPunches(StaffId);
            return Obj;
            }
        }

        public List<RAManualPunchApplication> GetAppliedManualPunchesForMyTeam(string StaffId, string AppliedId, string userRole)
        {
            using (RAManualPunchApplicationRepository repo = new RAManualPunchApplicationRepository())
            { 
                var Obj = repo.GetAppliedManualPunchesForMyTeam(StaffId, AppliedId, userRole);
            return Obj;
            }
        }

        public List<RAManualPunchApplication> GetAllManualPunchList(string StaffId, string AppliedId, string userRole)
        {
            using (RAManualPunchApplicationRepository repo = new RAManualPunchApplicationRepository())
            { 
                var Obj = repo.GetAllManualPunchList(StaffId, AppliedId, userRole);
            return Obj;
            }
        }

        public List<PermissionType> GetPermissionTypes()
        {
            using (RAManualPunchApplicationRepository repo = new RAManualPunchApplicationRepository())
            { 
                var Obj = repo.GetPermissionTypes();
            return Obj;
            }
        }

        public List<SelectListItem> ConvertPermissionTypesToListItems(List<PermissionType> objListOfLeaveTypes)
        {
            List<SelectListItem> _ListOfLeaveTypes_ = new List<SelectListItem>();
            foreach (var l in objListOfLeaveTypes)
            {
                _ListOfLeaveTypes_.Add(new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                });
            }

            return _ListOfLeaveTypes_;
        }

        public string GetUniqueId()
        {
            using (var repo = new RAManualPunchApplicationRepository())
                return repo.GetUniqueId();
        }
        
         public List<RAManualPunchApplication> GetApprovedManualPunchesForMyTeam(string staffId)
        {
            using (RAManualPunchApplicationRepository rAManualPunchApplicationRepository = new RAManualPunchApplicationRepository())
            {
                return rAManualPunchApplicationRepository.GetApprovedManualPunchesForMyTeam(staffId);
            }
        }

        public void FromDateShouldBeLessThanToDate(DateTime FromDate, DateTime ToDate)
        {
            if (FromDate > ToDate)
            {
                throw new Exception("Starting date of your request must be less than the end date.");
            }
        }
        public void MustBeSameDurationWhenSameDate(DateTime? StartDate, DateTime? EndDate)
        {
            if (!(StartDate.Value.Date <= EndDate.Value.Date.AddDays(1)))
            {
                throw new Exception("Please select the same dates for IN and OUT.");
            }
        }
        public string ValidateExistanceManualPunch(string StaffId, string DatetoBeChecked)
        {
            using (var repo = new RAManualPunchApplicationRepository())
                return repo.ValidateExistanceManualPunch(StaffId, DatetoBeChecked);
        }

        public void SaveRequestApplication(ClassesToSave DataToSave, string SecurityGroupId, string BaseAddress, string LocationId)
        {
            using (RAManualPunchApplicationRepository repo = new RAManualPunchApplicationRepository())
            { 
                var cm = new CommonRepository();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            string payPeriodValidationMessage = string.Empty;
            CommonBusinessLogic CBL = new CommonBusinessLogic();
            StringBuilder emailbody = new StringBuilder();
            StringBuilder emailbodyA = new StringBuilder();
            StringBuilder INandOUTData = new StringBuilder();

            string ReportingManagerName = string.Empty;
            string StaffName = string.Empty;
            string applicantEmailId = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string senderEmailId = string.Empty;
            string appliedByUserEmailId = string.Empty;
            string appliedByUserName = string.Empty;
            string approvalOwner2Name = string.Empty;
            bool isFinalLevelApproval = false;

            applicantEmailId = cm.GetEmailIdOfEmployee(DataToSave.RA.StaffId);
            approvalOwner1EmailId = cm.GetEmailIdOfEmployee(DataToSave.AA.ApprovalOwner);
            approvalOwner2EmailId = cm.GetEmailIdOfEmployee(DataToSave.AA.Approval2Owner);
            appliedByUserEmailId = cm.GetEmailIdOfEmployee(DataToSave.RA.AppliedBy);
            commonSenderEmailId = cm.GetSenderEmailIdFromEmailSettings();
            StaffName = cm.GetStaffName(DataToSave.RA.StaffId);
            ReportingManagerName = cm.GetStaffName(DataToSave.AA.ApprovalOwner);
            approvalOwner2Name = cm.GetStaffName(DataToSave.AA.Approval2Owner);
            appliedByUserName = cm.GetStaffName(DataToSave.RA.AppliedBy);
            ReportingManagerName = cm.GetStaffName(DataToSave.AA.ApprovalOwner);

            payPeriodValidationMessage = CBL.ValidateApplicationForPayDate(DataToSave.RA.StaffId, Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy"),
                    Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy"));
            if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
            {
                if (payPeriodValidationMessage.ToUpper() != "VALID")
                {
                    throw new ApplicationException("Application of past pay cycle cannot be saved");
                }
            }

            if (DataToSave.RA.PunchType.Equals("In"))
            {
                INandOUTData.Clear();
                INandOUTData.Append("<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                    "In Time:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                    + DataToSave.RA.StartDate + "</td></tr>");
            }
            else if (DataToSave.RA.PunchType.Equals("Out"))
            {
                INandOUTData.Clear();
                INandOUTData.Append("<tr><td style=\"width:20%;font-family:tahoma;" +
                    "font-size:9pt;\"> Out Time:</td><td style=\"width:80%;font-family:tahoma;" +
                    " font-size:9pt;\">" + DataToSave.RA.EndDate + "</td></tr>");
            }
            else
            {
                INandOUTData.Clear();
                INandOUTData.Append("<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                "In Time:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                + DataToSave.RA.StartDate + "</td></tr><tr><td style =\"width:20%;font-family:tahoma;" +
                 "font-size:9pt;\"> Out Time:</td><td style=\"width:80%;font-family:tahoma;" +
                 " font-size:9pt;\">" + DataToSave.RA.EndDate + "</td></tr> ");
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
                        EmailSubject = "Request for Manual Punch application sent to " + ReportingManagerName,
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br>" +
                        "Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your Manual Punch application has been sent to your Reporting Manager " +
                        " (" + ReportingManagerName + ") for approval.<p style=\"font-family:tahoma;" +
                        " font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma;" +
                        " font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;" +
                        "\">Employee Code:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" +
                        "" + DataToSave.RA.StaffId + "</td></tr> " + INandOUTData + "" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.Remarks + "</td>" +
                        "</tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>",
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
                        EmailSubject = "Request for Manual Punch application of " + StaffName,
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + "" +
                        ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + StaffName + " has applied for a Manual Punch. Manual Punch details given below.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                        "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr>" +
                        "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" +
                        "" + INandOUTData + " <tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.Remarks + "</td>" +
                        "</tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required " +
                        "for this application.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "<a href=\"" + BaseAddress + "\">Redirect To Site</a></p><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + StaffName + " &nbsp;(" + DataToSave.RA.StaffId + ")</p></body></html>",
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
            if (DataToSave.RA.AppliedBy.Equals(DataToSave.AA.ApprovalOwner) &&
                DataToSave.RA.AppliedBy != DataToSave.AA.Approval2Owner)
            {
                DataToSave.RA.IsApproved = false;
                DataToSave.AA.ApprovalStatusId = 2;
                DataToSave.AA.Approval2statusId = 1;
                DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                DataToSave.AA.ApprovedOn = DateTime.Now;
                DataToSave.AA.Comment = "REVIEWED THE Manual Punch REQUEST.";

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    //Send intimation to the applicant 
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested Manual Punch application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Your Manual Punch application " +
                        "for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                        " has been applied and approved by " + appliedByUserName + "" +
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
                if (string.IsNullOrEmpty(DataToSave.AA.Approval2Owner).Equals(false) && string.IsNullOrEmpty(approvalOwner2EmailId)
                    .Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = approvalOwner2EmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Request for Manual Punch application of " + StaffName,
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner2Name + "" +
                        ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + ReportingManagerName + " has applied and approved for a Manual Punch. Manual Punch details are given below.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                        "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr>" +
                        "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" +
                        " " + INandOUTData + "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.Remarks + "</td>" +
                        "</tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required " +
                        "for this application.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
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
                isFinalLevelApproval = true;
                DataToSave.RA.IsApproved = true;
                DataToSave.AA.ApprovalStatusId = 2;
                DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                DataToSave.AA.ApprovedOn = DateTime.Now;
                DataToSave.AA.Approval2statusId = 2;
                DataToSave.AA.Approval2By = DataToSave.RA.AppliedBy;
                DataToSave.AA.Approval2On = DateTime.Now;
                DataToSave.AA.Comment = "APPROVED THE MANUAL PUNCH REQUEST.";

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested Manual Punch application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Your Manual Punch  application" +
                        " for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy")
                        + " has been applied and approved..</p><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">" + "Best Regards</p><p style=\"font-family:tahoma; " +
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

            else if (DataToSave.RA.AppliedBy != DataToSave.RA.StaffId && DataToSave.RA.AppliedBy !=
                 DataToSave.AA.ApprovalOwner && (SecurityGroupId.Equals("1") || SecurityGroupId.Equals("3")
                 || SecurityGroupId.Equals("5")))
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
                DataToSave.AA.Comment = "APPROVED THE MANUAL PUNCH REQUEST.";

                senderEmailId = appliedByUserEmailId;
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested Manual Punch application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your Manual Punch  application for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).
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
                DataToSave.ESL = ESL;
            }
            repo.SaveRequestApplication(DataToSave, SecurityGroupId, isFinalLevelApproval,LocationId);
            }
        }
public void SaveRequestApplication(ClassesToSave DataToSave, string SecurityGroupId, string LocationId)
        {
    
            try
            {
                string payPeriodValidationMessage = string.Empty;
                using (CommonRepository commonRepository = new CommonRepository())
                {
                    payPeriodValidationMessage = commonRepository.ValidateApplicationForPayDate(DataToSave.RA.StaffId,DataToSave.RA.StartDate, DataToSave.RA.EndDate);
                }
                    if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false) && payPeriodValidationMessage != "VALID")
                {
                    throw new ApplicationException("Application of past pay cycle cannot be saved ");
                }

                ValidateManualPunch(DataToSave.RA.StaffId, DataToSave.RA.StartDate, DataToSave.RA.EndDate);

                List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
                StringBuilder emailbody = new StringBuilder();
                StringBuilder emailbodyA = new StringBuilder();
                StringBuilder INandOUTData = new StringBuilder();
                StringBuilder userIntimationMailContent = new StringBuilder();
                string BaseAddress = string.Empty;
                bool isFinalLevelApproval = false;
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
                try
                {
                    BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
                }
                catch
                {

                }

                if (DataToSave.RA.PunchType.Equals("In"))
                {
                    INandOUTData.Clear();
                    INandOUTData.Append("<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                        "IN Time:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                        + DataToSave.RA.StartDate + "</td></tr>");
                }
                else if (DataToSave.RA.PunchType.Equals("Out"))
                {
                    INandOUTData.Clear();
                    INandOUTData.Append("<tr><td style=\"width:20%;font-family:tahoma;" +
                        "font-size:9pt;\"> OUT Time:</td><td style=\"width:80%;font-family:tahoma;" +
                        " font-size:9pt;\">" + DataToSave.RA.EndDate + "</td></tr>");
                }
                else
                {
                    INandOUTData.Clear();
                    INandOUTData.Append("<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                    "IN Time:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                    + DataToSave.RA.StartDate + "</td></tr><tr><td style =\"width:20%;font-family:tahoma;" +
                     "font-size:9pt;\"> Out Time:</td><td style=\"width:80%;font-family:tahoma;" +
                     " font-size:9pt;\">" + DataToSave.RA.EndDate + "</td></tr> ");
                }
                userIntimationMailContent.Clear();
                userIntimationMailContent.Append("<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + applicantName + ",<br/><br>" +
                        "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your punch regularisation  application for the punch type " + DataToSave.RA.PunchType +
                        " on " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + " has been applied and approved by" +
                        " " + appliedByUserName + ".</p>" + "<p style=\"font-family:tahoma; " +
                        "font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + appliedByUserName + " &nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>");

                if (DataToSave.RA.AppliedBy == DataToSave.RA.StaffId)
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
                            EmailSubject = "Request for punch regularisation application sent to " + approvalOwner1Name,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + applicantName + ",<br><br>Greetings</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your punch regularisation " +
                            "application for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                            " has been sent to your reporting manager &nbsp; " + "(" + approvalOwner1Name + ") for approval." +
                            "<p style=\"font-family:tahoma; " + "font-size:9pt;\">" + "Best Regards</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = DataToSave.RA.AppliedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }

                    //Send intimation email to the reporting manager or Approval Owner1.
                    if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Request for punch regularisation application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            "font-size:9pt;\">Dear " + approvalOwner1Name + ",<br><br>Greetings</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + applicantName + " has applied for" +
                            " a punch regularisation application. Punch regularisation details are given below.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                            "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;" +
                            "font-family:tahoma; font-size:9pt;\">StaffId:</td><td style=\"width:80%;" +
                            "font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" + INandOUTData + "" +
                            "<tr><td style=\"width:20%;font-family:" + "tahoma; font-size:9pt;\">" +
                            "Punch Type:</td><td style=\"width:80%;font-family:" +
                            "tahoma; font-size:9pt;\">" + DataToSave.RA.PunchType + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                            + DataToSave.RA.Remarks + "</td></tr></table></p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Your attention is required to approve or reject the application.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">" +
                            "Return to Site</a></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + applicantName + "" +
                            "&nbsp;(" + DataToSave.RA.StaffId + ")</p></body></html>",
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

                // If Reporting manager or Approval Owner1 has applied the application
                else if (DataToSave.RA.AppliedBy.Equals(DataToSave.AA.ApprovalOwner) &&
                    DataToSave.RA.AppliedBy != DataToSave.AA.Approval2Owner)
                {
                    DataToSave.RA.IsApproved = true;
                    DataToSave.AA.ApprovalStatusId = 2;
                    DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedOn = DateTime.Now;
                    DataToSave.AA.Approval2statusId = 1;
                    DataToSave.AA.Approval2By = null;
                    DataToSave.AA.Approval2On = null;
                    DataToSave.AA.Comment = "APPROVED THE MANUAL PUNCH REQUEST.";

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested punch regularisation application status",
                            EmailBody = userIntimationMailContent.ToString(),
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
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Punch regularisation aplication of " + applicantName + "",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                   "font-size:9pt;\">Dear " + approvalOwner2Name + ",<br><br>Greetings</p>" +
                                   "<p style=\"font-family:tahoma; font-size:9pt;\">" + approvalOwner1Name + " has applied for" +
                                   " a punch regularisation application for " + applicantName + ". Punch regularisation details are given below.</p>" +
                                   "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                                   "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;" +
                                   "font-family:tahoma; font-size:9pt;\">StaffId:</td><td style=\"width:80%;" +
                                   "font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" + INandOUTData + "" +
                                   "<tr><td style=\"width:20%;font-family:" + "tahoma; font-size:9pt;\">" +
                                   "Punch Type:</td><td style=\"width:80%;font-family:" +
                                   "tahoma; font-size:9pt;\">" + DataToSave.RA.PunchType + "</td></tr>" +
                                   "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                                   "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                                   + DataToSave.RA.Remarks + "</td></tr></table></p><p style=\"font-family:tahoma;" +
                                   " font-size:9pt;\">Your attention is required for the application.</p>" +
                                   "<p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">" +
                                   "Return to Site</a></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                   "Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + applicantName + "" +
                                   "&nbsp;(" + DataToSave.RA.StaffId + ")</p></body></html>",
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
                // If the Reviewer or Approval Owner 2 has applied the application
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
                    DataToSave.AA.Comment = "APPROVED AND REVIEWED THE MANUAL PUNCH REQUEST.";
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested punch regularisation application status",
                            EmailBody = userIntimationMailContent.ToString(),
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

                else if (DataToSave.RA.AppliedBy != DataToSave.RA.StaffId && DataToSave.RA.AppliedBy != DataToSave.AA.ApprovalOwner &&
                     DataToSave.RA.AppliedBy != DataToSave.AA.Approval2Owner && (SecurityGroupId == "3" ||
                      SecurityGroupId == "5" || SecurityGroupId == "6"))
                {
                    isFinalLevelApproval = true;
                    DataToSave.RA.IsApproved = true;
                    DataToSave.AA.ApprovalStatusId = 2;
                    DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedOn = DateTime.Now;
                    DataToSave.AA.Approval2statusId = 2;
                    DataToSave.AA.Approval2By = DataToSave.RA.AppliedBy;
                    DataToSave.AA.Approval2On = DateTime.Now;
                    DataToSave.AA.Comment = "REVIEWER APPROVED THE MANUAL PUNCH REQUEST.";
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested punch regularisation application status",
                            EmailBody = userIntimationMailContent.ToString(),
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
                using (RAManualPunchApplicationRepository rAManualPunchApplicationRepository = new RAManualPunchApplicationRepository())
                {
                    rAManualPunchApplicationRepository.SaveRequestApplication(DataToSave, LocationId, isFinalLevelApproval);
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public void RejectApplication(string Id, string RejectedBy,string LocationId)
        {
            //Get the Manual punch application details based on the id passed to this function as a parameter.
            using (RAManualPunchApplicationRepository repo = new RAManualPunchApplicationRepository())
            { 
                var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id);
            var cm = new CommonRepository();
            ClassesToSave CTS = new ClassesToSave();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            StringBuilder emailbody = new StringBuilder();
            string applicantName = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string rejectedByUserName = string.Empty;
            string rejectedByUserMailId = string.Empty;
            string applicantMailId = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string payPeriodValidationMessage = string.Empty;
            applicantName = cm.GetStaffName(Obj.StaffId);
            approvalOwner1 = cm.GetApproverOwner(Id);
            approvalOwner2 = cm.GetReviewerOwner(Id);
            rejectedByUserName = cm.GetStaffName(RejectedBy);
            rejectedByUserMailId = cm.GetEmailIdOfEmployee(RejectedBy);
            applicantMailId = cm.GetEmailIdOfEmployee(Obj.StaffId);
            commonSenderEmailId = cm.GetSenderEmailIdFromEmailSettings();
            if(Obj.AppliedBy == "")
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
            //Check if the manual punch application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true))    //if the manual punch application has been cancelled then...
            {
                throw new Exception("Cancelled manual punch request cannot be rejected.");
            }
            else if (Obj.IsRejected.Equals(true))  //if the manual punch application has been rejected then...
            {
                throw new Exception("Rejected manual punch request cannot be rejected.");
            }
            else //if the manual punch application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {
                string swipeDate = string.Empty;
                string punchType = string.Empty;
                if (Obj.PunchType == "In")
                {
                    swipeDate = Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy");
                    punchType = "In";
                }
                else if (Obj.PunchType == "Out")
                {
                    swipeDate = Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy");
                    punchType = "Out";
                }
                else
                {
                    swipeDate = Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy");
                    punchType = "In & Out";
                }
                if (RejectedBy == approvalOwner1 && RejectedBy != approvalOwner2)
                {
                    //reject the application.
                    Obj.IsRejected = true;
                    AA.ApprovalStatusId = 3;
                    AA.ApprovedBy = RejectedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "MANUAL PUNCH REQUEST HAS BEEN REJECTED BY THE APPROVALOWNER1.";
                }
                else if (RejectedBy == approvalOwner2)
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
                    AA.Comment = "MANUAL PUNCH REQUEST HAS BEEN REJECTED BY THE APPROVALOWNER2.";
                }
                if (string.IsNullOrEmpty(applicantMailId).Equals(false))
                {
                    emailbody.Clear();
                    emailbody.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your Punch Regularization  application for the Punch Type " + punchType + " on " + swipeDate + "" +
                        " has been rejected.</p> <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">"
                        + rejectedByUserName + " &nbsp;(" + RejectedBy + ")</p></body></html>");

                    //email to user
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantMailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested manual punch application status",
                        EmailBody = emailbody.ToString(),
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
                repo.RejectApplication(CTS, LocationId);
            }
            }
        }

        public void ApproveApplication(string Id, string ApprovedBy, string LocationId)
        {
            //Get the manual punch application details based on the Id passed to this function as a parameter.
            using (RAManualPunchApplicationRepository repo = new RAManualPunchApplicationRepository())
            { 
                var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id);
            var cm = new CommonRepository();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            ClassesToSave CTS = new ClassesToSave();
            StringBuilder Emailbody = new StringBuilder();
            string ReportingManagerName = string.Empty;
            string StaffName = string.Empty;
            StringBuilder INandOUTData = new StringBuilder();
            DateTime? ApplicationDate = DateTime.Now;
            string ApproverOwner = string.Empty;
            string ReviewerOwner = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvedByUserName = string.Empty;
            string approvedByUserEmailId = string.Empty;
            string approvalOwner2Name = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            bool isFinalLevelApproval = false;
            string payPeriodValidationMessage = string.Empty;
            StringBuilder emailbody = new StringBuilder();

            approvalOwner1 = cm.GetApproverOwner(Id);
            approvalOwner2 = cm.GetReviewerOwner(Id);
            approvedByUserName = cm.GetStaffName(ApprovedBy);
            approvedByUserEmailId = cm.GetEmailIdOfEmployee(ApprovedBy);
            applicantName = cm.GetStaffName(Obj.StaffId);
            applicantEmailId = cm.GetEmailIdOfEmployee(Obj.StaffId);
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
            if (Obj.PunchType.Equals("In"))
            {
                INandOUTData.Clear();
                INandOUTData.Append("<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                    "In Time:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                    + Obj.StartDate + "</td></tr>");
            }
            else if (Obj.PunchType.Equals("Out"))
            {
                INandOUTData.Clear();
                INandOUTData.Append("<tr><td style=\"width:20%;font-family:tahoma;" +
                    "font-size:9pt;\"> Out Time:</td><td style=\"width:80%;font-family:tahoma;" +
                    " font-size:9pt;\">" + Obj.EndDate + "</td></tr>");
            }
            else
            {
                INandOUTData.Clear();
                INandOUTData.Append("<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                "In Time:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                + Obj.StartDate + "</td></tr><tr><td style =\"width:20%;font-family:tahoma;" +
                 "font-size:9pt;\"> Out Time:</td><td style=\"width:80%;font-family:tahoma;" +
                 " font-size:9pt;\">" + Obj.EndDate + "</td></tr> ");
            }

            if (Obj.IsApproved.Equals(true)) //if application has already been approved then...
            {
                throw new Exception("Cannot approve already approved manual punch request.");
            }
            else if (Obj.IsRejected.Equals(true))
            {
                throw new Exception("Cannot approve already rejected manual punch request.");
            }
            else
            {
                if (Obj.PunchType.Equals("In"))
                {
                    ApplicationDate = Obj.StartDate;
                }
                else if (Obj.PunchType.Equals("Out"))
                {
                    ApplicationDate = Obj.EndDate;
                }
                else
                {
                    ApplicationDate = Obj.StartDate;
                }
                // If the appliction has approved by ReportingManager or Approval Owner1
                if (ApprovedBy == approvalOwner1 && ApprovedBy != approvalOwner2)
                {
                    //approve the application.
                    Obj.IsApproved = false;
                    AA.ApprovalStatusId = 2;
                    AA.ApprovedBy = ApprovedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "APPROVED THE MANUAL PUNCH REQUEST.";

                   
                    emailbody.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your Punch Regularization  application for the Punch Type " + Obj.PunchType + " on "
                        + Convert.ToDateTime(ApplicationDate).ToString("dd-MMM-yyyy") + " " +
                        " has been approved and send for second level approval.</p><p style=\"font-family:tahoma; font-size:9pt;\"></p><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + approvedByUserName + "" +
                        " &nbsp;(" + ApprovedBy + ")</p></body></html>");

                    //send intimation mail to approvalOwner2
                    if (string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Request for Manual Punch application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner2Name + "" +
                            ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + applicantName + " has applied for a Manual Punch. Manual Punch details are given below.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                            "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr>" +
                            "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Obj.StaffId + "</td></tr>" +
                            "" + INandOUTData + " <tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Obj.Remarks + "</td>" +
                            "</tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required " +
                            "to approve or reject this application.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            " Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + approvedByUserName + " &nbsp;(" + ApprovedBy + ")</p></body></html>",
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

                else if (ApprovedBy == approvalOwner2)
                {
                    //approve the application.
                    isFinalLevelApproval = true;
                    Obj.IsApproved = true;
                    if (approvalOwner1 == approvalOwner2)
                    {
                        AA.ApprovalStatusId = 2;
                        AA.ApprovedBy = ApprovedBy;
                        AA.ApprovedOn = DateTime.Now;
                    }
                    AA.Approval2statusId = 2;
                    AA.Approval2By = ApprovedBy;
                    AA.Approval2On = DateTime.Now;
                    AA.Comment = "APPROVED THE MANUAL PUNCH REQUEST.";

                    emailbody.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                    "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                    "Your Punch Regularization application for the punch type " + Obj.PunchType + " on " +
                                    Convert.ToDateTime(ApplicationDate).ToString("dd-MMM-yyyy") + " " +
                                    " has been approved.</p><p style=\"font-family:tahoma; font-size:9pt;\"></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                    "Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + approvedByUserName + "" +
                                    " &nbsp;(" + ApprovedBy + ")</p></body></html>");
                }
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested manual punch application status",
                        EmailBody = emailbody.ToString(),
                        CreatedOn = DateTime.Now,
                        CreatedBy = ApprovedBy,
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
                repo.ApproveApplication(CTS, ApprovedBy, isFinalLevelApproval, LocationId);
            }
            }
        }

        public string CancelApplication(string Id, string CancelledBy)
        {
            //Get the Manual punch application details based on the Id passed to this function as a parameter.
            using (RAManualPunchApplicationRepository repo = new RAManualPunchApplicationRepository())
            { 
                ClassesToSave CTS = new ClassesToSave();
            var cm = new CommonRepository();
            var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id);
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            CommonBusinessLogic CBL = new CommonBusinessLogic();
            StringBuilder INandOUTData = new StringBuilder();
            DateTime? ApplicationDate = DateTime.Now;
            string approvalOwner1Name = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner2Name = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string cancelledByUserEmailId = string.Empty;
            string cancelledByUserName = string.Empty;
            string commonSenderEmailId = string.Empty;
            string senderEmailId = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string approvalOwner2EmailId = string.Empty;
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
            if (Obj.PunchType.Equals("In"))
            {
                INandOUTData.Clear();
                INandOUTData.Append("<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                    "In Time:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                    + Obj.StartDate + "</td></tr>");
            }
            else if (Obj.PunchType.Equals("Out"))
            {
                INandOUTData.Clear();
                INandOUTData.Append("<tr><td style=\"width:20%;font-family:tahoma;" +
                    "font-size:9pt;\"> Out Time:</td><td style=\"width:80%;font-family:tahoma;" +
                    " font-size:9pt;\">" + Obj.EndDate + "</td></tr>");
            }
            else
            {
                INandOUTData.Clear();
                INandOUTData.Append("<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                "In Time:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                + Obj.StartDate + "</td></tr><tr><td style =\"width:20%;font-family:tahoma;" +
                 "font-size:9pt;\"> Out Time:</td><td style=\"width:80%;font-family:tahoma;" +
                 " font-size:9pt;\">" + Obj.EndDate + "</td></tr> ");
            }

            if (Obj.IsCancelled.Equals(false))   //If the manual punch application has not been cancelled then...
            {
                //Cancel the manual punch application which is in pending state.
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
                if (CancelledBy == Obj.StaffId)
                {
                    if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                    {
                        mailBody = string.Empty;
                        mailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        " " + applicantName + " has cancelled Punch Regularization  application for the punch type " + Obj.PunchType + " on "
                        + Convert.ToDateTime(ApplicationDate).ToString("dd-MMM-yyyy") +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                        "(" + CancelledBy + ")</p></body></html>";

                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner1
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested for swipe regularisation application status ",
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
                        mailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                       "Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                       " " + applicantName + " has cancelled Punch Regularization  application for the punch type " + Obj.PunchType + " on "
                       + Convert.ToDateTime(ApplicationDate).ToString("dd-MMM-yyyy") +
                       "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                       "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                       "(" + CancelledBy + ")</p></body></html>";
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner2
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested for swipe regularisation application status ",
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
                                " " + approvalOwner1Name + " has cancelled the manual application for the punch type " + Obj.PunchType + "on" +
                                "" + Convert.ToDateTime(ApplicationDate).ToString("dd - MMM - yyyy") +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                                "(" + CancelledBy + ")</p></body></html>";

                            ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner2
                            {
                                From = commonSenderEmailId,
                                To = approvalOwner2EmailId,
                                CC = string.Empty,
                                BCC = string.Empty,
                                EmailSubject = "Requested swipe regularisation application status ",
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
                                " " + approvalOwner2Name + " has cancelled the manual application for the punch type " + Obj.PunchType + "on" +
                                "" + Convert.ToDateTime(ApplicationDate).ToString("dd - MMM - yyyy") +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                                "(" + CancelledBy + ")</p></body></html>";

                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner2
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested swipe regularisation application status ",
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
                        "Your Punch Regularization  application for the punch type " + Obj.PunchType + " on "
                        + Convert.ToDateTime(ApplicationDate).ToString("dd-MMM-yyyy") + " " +
                        " has been cancelled. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
                        "(" + CancelledBy + ")</p></body></html>";
                        ESL.Add(new EmailSendLog  //Send Mail to User
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested swipe regularisation application status ",
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
                CTS.ESL = ESL;
                repo.CancelApplication(CTS, CancelledBy);
            }
            else   //If the manual punch application has already been cancelled then...
            {
                throw new Exception("You cannot cancel a manual punch request that is already been cancelled.");
            }
            return "OK";
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
public string ValidateManualPunch(string staffId, DateTime time1, DateTime time2)
        {
            string msg = string.Empty;
            using (RAManualPunchApplicationRepository rAManualPunchApplicationRepository = new RAManualPunchApplicationRepository())
        {
                msg = rAManualPunchApplicationRepository.ValidateManualPunch(staffId, time1, time2);
            }
            if (!msg.ToUpper().StartsWith("OK."))
            {
                throw new ApplicationException(msg);
            }
            return msg;
        }
    }
}