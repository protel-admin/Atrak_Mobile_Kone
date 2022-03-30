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
            using (RAManualPunchApplicationRepository rAManualPunchApplicationRepository = new RAManualPunchApplicationRepository())
            {              
                return rAManualPunchApplicationRepository.GetAppliedManualPunches(StaffId);
              
            }
        }
     

        public List<RAManualPunchApplication> GetAppliedManualPunchesForMyTeam(string StaffId, string AppliedBy, string Role)
        {
            using (RAManualPunchApplicationRepository rAManualPunchApplicationRepository = new RAManualPunchApplicationRepository())
            {
                return rAManualPunchApplicationRepository.GetAppliedManualPunchesForMyTeam(StaffId, AppliedBy, Role);
            }
        }

        public List<PermissionType> GetPermissionTypes()
        {
            using (RAManualPunchApplicationRepository rAManualPunchApplicationRepository = new RAManualPunchApplicationRepository())
            {
                return rAManualPunchApplicationRepository.GetPermissionTypes();
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
            using (RAManualPunchApplicationRepository rAManualPunchApplicationRepository = new RAManualPunchApplicationRepository())
            {
                return rAManualPunchApplicationRepository.GetUniqueId();
            }
        }
        public string ValidateExistanceManualPunch(string StaffId, string DatetoBeChecked)
        {
            using (RAManualPunchApplicationRepository rAManualPunchApplicationRepository = new RAManualPunchApplicationRepository())
            {
                return rAManualPunchApplicationRepository.ValidateExistanceManualPunch(StaffId, DatetoBeChecked);
            }
        }
        public void FromDateShouldBeLessThanToDate(DateTime FromDate, DateTime ToDate)
        {
            if (FromDate > ToDate)
            {
                throw new Exception("Starting date of your application must be less than the Ending datey.");
            }
        }

        public List<RAManualPunchApplication> GetApprovedManualPunchesForMyTeam(string staffId)
        {
            using (RAManualPunchApplicationRepository rAManualPunchApplicationRepository = new RAManualPunchApplicationRepository())
            {
                return rAManualPunchApplicationRepository.GetApprovedManualPunchesForMyTeam(staffId);
            }
        }

        public void SaveRequestApplication(ClassesToSave DataToSave, string SecurityGroupId, string LocationId)
        {
    
            try
            {
                string payPeriodValidationMessage = string.Empty;
                using (CommonRepository commonRepository = new CommonRepository())
                {
                    payPeriodValidationMessage = commonRepository.ValidateApplicationForPayDate(DataToSave.RA.StartDate, DataToSave.RA.EndDate);
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
                    approvalOwner2Name = commonRepository.GetStaffName(DataToSave.AA.ReviewerOwner);
                    approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.AA.ReviewerOwner);
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
                    DataToSave.RA.AppliedBy != DataToSave.AA.ReviewerOwner)
                {
                    DataToSave.RA.IsApproved = true;
                    DataToSave.RA.IsReviewed = false;
                    DataToSave.AA.ApprovalStatusId = 2;
                    DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedOn = DateTime.Now;
                    DataToSave.AA.ReviewerstatusId = 1;
                    DataToSave.AA.ReviewedBy = null;
                    DataToSave.AA.ReviewedOn = null;
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
                     DataToSave.RA.AppliedBy != DataToSave.AA.ReviewerOwner && (SecurityGroupId == "3" ||
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
        public void RejectApplication(string Id, string RejectedBy, string LocationId)
        {
            //Get the leave application details based on the id passed to this function as a parameter.
            RequestApplication Obj = new RequestApplication();
            ApplicationApproval AA = new ApplicationApproval();

            using (RAManualPunchApplicationRepository rAManualPunchApplicationRepository = new RAManualPunchApplicationRepository())
        {
                Obj = rAManualPunchApplicationRepository.GetRequestApplicationDetails(Id);
                AA = rAManualPunchApplicationRepository.GetApplicationApproval(Id);
            }
            
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            ClassesToSave CTS = new ClassesToSave();
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner1Name = string.Empty;
            string approvalOwner2Name = string.Empty;
            string rejectedByUserName = string.Empty;
            string rejectedByUserEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string ccEmailAddress = string.Empty;
            StringBuilder INandOUTData = new StringBuilder();
            StringBuilder emailbody = new StringBuilder();

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
            }

            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true))    //if the leave application has been cancelled then...
            {
                throw new Exception("Cancelled manual punch request cannot be rejected.");
            }
            else if (Obj.IsApproved.Equals(true) && RejectedBy == approvalOwner1) //if the leave application has been approved then...
            {
                throw new Exception("Approved manual punch request cannot be rejected.");
            }
            else if (Obj.IsReviewed.Equals(true) && RejectedBy == approvalOwner2) //if the leave application has been approved then...
            {
                throw new Exception("Approved manual punch request cannot be rejected.");
            }
            else if (Obj.IsRejected.Equals(true))  //if the leave application has been rejected then...
            {
                throw new Exception("Rejected manual punch request cannot be rejected.");
            }
            else //if the leave application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {

                if (RejectedBy == approvalOwner1 && RejectedBy != approvalOwner2)
                {
                    //reject the application.
                    Obj.IsRejected = true;
                    AA.ApprovalStatusId = 3;
                    AA.ApprovedBy = RejectedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "MANUAL PUNCH REQUEST HAS BEEN REJECTED BY THE REPORTING MANAGER.";
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
                    AA.ReviewerstatusId = 3;
                    AA.ReviewedBy = RejectedBy;
                    AA.ReviewedOn = DateTime.Now;
                    AA.Comment = "MANUAL PUNCH REQUEST HAS BEEN REJECTED BY THE APPROVAL OWNER2.";
                    }

                        emailbody.Clear();
                emailbody.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                    "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                    "Your punch regularisation application for the punch type " + Obj.PunchType + " on "
                    + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " has been rejected.</p>" +
                    "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                    "" + rejectedByUserName + " &nbsp;(" + RejectedBy + ")</p></body></html>");
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested punch regularisation application status",
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

                using (RAManualPunchApplicationRepository rAManualPunchApplicationRepository = new RAManualPunchApplicationRepository())
                {
                    rAManualPunchApplicationRepository.RejectApplication(CTS);
                }
            }
        }

        public void ApproveApplication(string Id, string ApprovedBy, string LocationId)
        {
            //Get the leave application details based on the Id passed to this function as a parameter.
            RequestApplication Obj = new RequestApplication();
            ApplicationApproval AA = new ApplicationApproval();

            using (RAManualPunchApplicationRepository rAManualPunchApplicationRepository = new RAManualPunchApplicationRepository())
            {
                Obj = rAManualPunchApplicationRepository.GetRequestApplicationDetails(Id);
                AA = rAManualPunchApplicationRepository.GetApplicationApproval(Id);
            }
           
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            StringBuilder emailbody = new StringBuilder();
            StringBuilder INandOUTData = new StringBuilder();
            ClassesToSave CTS = new ClassesToSave();
            string applicationId = string.Empty;
            string applicationType = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner2Name = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string approvedByUserName = string.Empty;
            string approvedByUserEmailId = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            bool isFinalLevelApproval = false;
            using (CommonRepository commonRepository = new CommonRepository())
            {
                applicantName = commonRepository.GetStaffName(Obj.StaffId);
                applicantEmailId = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
                approvalOwner1 = commonRepository.GetApproverOwner(Id);
                approvalOwner2 = commonRepository.GetReviewerOwner(Id);
                approvalOwner2Name = commonRepository.GetStaffName(approvalOwner2);
                approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner2);
                approvedByUserName = commonRepository.GetStaffName(ApprovedBy);
                approvedByUserEmailId = commonRepository.GetEmailIdOfEmployee(ApprovedBy);
                commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
            }

            if (Obj.PunchType.Equals("In"))
            {
                INandOUTData.Clear();
                INandOUTData.Append("<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                    "IN Time:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
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
                "IN Time:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                + Obj.StartDate + "</td></tr><tr><td style =\"width:20%;font-family:tahoma;" +
                 "font-size:9pt;\"> Out Time:</td><td style=\"width:80%;font-family:tahoma;" +
                 " font-size:9pt;\">" + Obj.EndDate + "</td></tr> ");
            }
            if (ApprovedBy == approvalOwner1 && Obj.IsApproved == true)
            {
                throw new Exception("Cannot approve already approved punch regularisation request.");
                }
            else if (ApprovedBy == approvalOwner2 && Obj.IsReviewed == true)
                {
                throw new Exception("Cannot approve already apporved punch regularisation request.");
            }

            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true)) //if the leave application has been cancelled then...
            {
                throw new Exception("Cannot approve a cancelled punch regularisation request.");
            }
            else if (Obj.IsRejected.Equals(true))
            {
                throw new Exception("Cannot approve already rejected punch regularisation request.");
            }
            else
            {
                if (ApprovedBy == approvalOwner1 && ApprovedBy != approvalOwner2)
                {
                    Obj.IsApproved = true;
                    Obj.IsReviewed = false;
                    AA.ApprovalStatusId = 2;
                    AA.ApprovedBy = ApprovedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "APPROVED THE MANUAL PUNCH REQUEST.";
                    //Send intimation to the Approval Owner2
                    if (string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                    ESL.Add(new EmailSendLog
                    {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                        BCC = string.Empty,
                            EmailSubject = "Request for punch regularisation application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                             "font-size:9pt;\">Dear " + approvalOwner2Name + ",<br><br>Greetings</p>" +
                             "<p style=\"font-family:tahoma; font-size:9pt;\">" + approvedByUserName + " has approved " +
                             " a punch regularisation application. Details are given below.</p>" +
                             "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                             "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;" +
                             "font-family:tahoma; font-size:9pt;\">StaffId:</td><td style=\"width:80%;" +
                             "font-family:tahoma; font-size:9pt;\">" + Obj.StaffId + "</td></tr>" + INandOUTData + "" +
                             "<tr><td style=\"width:20%;font-family:" + "tahoma; font-size:9pt;\">" +
                             "Punch Type:</td><td style=\"width:80%;font-family:" +
                             "tahoma; font-size:9pt;\">" + Obj.PunchType + "</td></tr>" +
                             "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                             "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">"
                             + Obj.Remarks + "</td></tr></table></p><p style=\"font-family:tahoma;" +
                             " font-size:9pt;\">Your attention is required to approve or reject the application.</p>" +
                             "<p style=\"font-family:tahoma; font-size:9pt;\">" +
                             "Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + approvedByUserName + "" +
                             "&nbsp;(" + ApprovedBy + ")</p></body></html>",
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
                // If the application has approved by Approval Owner2
                else if (ApprovedBy == approvalOwner2)
                {
                    //Approve the application.
                    isFinalLevelApproval = true;
                    Obj.IsApproved = true;
                    Obj.IsReviewed = true;
                    if (approvalOwner1 == approvalOwner2)
                    {
                    AA.ApprovalStatusId = 2;
                        AA.ApprovedBy = ApprovedBy;
                    AA.ApprovedOn = DateTime.Now;
                }
                    AA.ReviewerstatusId = 2;
                    AA.ReviewedBy = ApprovedBy;
                    AA.ReviewedOn = DateTime.Now;
                    AA.Comment = "APPROVED THE MANUAL PUNCH REQUEST.";
                    }

                        emailbody.Clear();
                emailbody.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                    "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                    "Your punch regularisation application for the punch type " + Obj.PunchType + " on " + Convert.ToDateTime(Obj.StartDate)
                    .ToString("dd-MMM-yyyy") + " has been approved.</p><p style=\"font-family:tahoma; font-size:9pt;\"></p><p style=\"font-family:tahoma;" +
                    " font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + approvedByUserName + "" +
                    " &nbsp;(" + ApprovedBy + ")</p></body></html>");

                // Send intimation to the applicant
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested punch regularisation application status",
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

                using (RAManualPunchApplicationRepository rAManualPunchApplicationRepository = new RAManualPunchApplicationRepository())
                {
                    rAManualPunchApplicationRepository.ApproveApplication(CTS, isFinalLevelApproval, LocationId);
                }
            }
        }
        public string CancelApplication(string Id, string CancelledBy, string LocationId)
        {
            //Get the Punch Regularisation application details based on the Id passed to this function as a parameter.
            RequestApplication Obj = new RequestApplication();
            ApplicationApproval AA = new ApplicationApproval();
            ClassesToSave CTS = new ClassesToSave();
            using (RAManualPunchApplicationRepository rAManualPunchApplicationRepository = new RAManualPunchApplicationRepository())
            {
                 Obj = rAManualPunchApplicationRepository.GetRequestApplicationDetails(Id);
                 AA = rAManualPunchApplicationRepository.GetApplicationApproval(Id);
                            }

            var CBL = new CommonBusinessLogic();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
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
            string commonSenderEmailId = string.Empty;
            string payPeriodValidationMessage = string.Empty;

            approvalOwner1 = AA.ApprovalOwner;
            approvalOwner2 = AA.ReviewerOwner;
            using (CommonRepository commonRepository = new CommonRepository())
                            {
                applicantName = commonRepository.GetStaffName(Obj.StaffId);
                applicantEmailId = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
                approvalOwner1Name = commonRepository.GetStaffName(approvalOwner1);
                approvalOwner2Name = commonRepository.GetStaffName(approvalOwner2);
                approvalOwner1EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner1);
                approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner2);
                cancelledByUserName = commonRepository.GetStaffName(CancelledBy);
                cancelledByUserEmailId = commonRepository.GetEmailIdOfEmployee(CancelledBy);
                commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
                            }

            payPeriodValidationMessage = CBL.ValidateApplicationForPayDate(Obj.StartDate, Obj.EndDate);
            if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false) && payPeriodValidationMessage.ToUpper() != "VALID")
                            {
                throw new Exception("Application of past pay cycle cannot be cancelled");
                            }
            if (Obj.IsCancelled.Equals(false))   //If the punch regularisation application has not been cancelled then...
                            {
                if (CancelledBy == Obj.StaffId)
                            {
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;
                    Obj.CancelledBy = CancelledBy;
                    if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                        {
                        ESL.Add(new EmailSendLog  //Send email to Approval Owner1
                            {
                            From = commonSenderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                                BCC = string.Empty,
                            EmailSubject = "Requested punch regularisation application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"> Punch regularisation application of " + applicantName + " for the date" +
                            " " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
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
                    if (AA.ApprovalStatusId == 2 && AA.ApprovalOwner != AA.ReviewerOwner && string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                            {
                        ESL.Add(new EmailSendLog  //Send email to Approval Owner2
                            {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                                CC = string.Empty,
                                BCC = string.Empty,
                            EmailSubject = "Requested punch regularisation application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Punch regularisation application of " + applicantName + " for the date" +
                            " " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
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
                else if (CancelledBy != Obj.StaffId)
                {
                    if (CancelledBy == approvalOwner1)
                    {
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;
                        Obj.CancelledBy = CancelledBy;
                        Obj.IsApproverCancelled = true;
                        Obj.ApproverCancelledDate = DateTime.Now;
                        Obj.ApproverCancelledBy = CancelledBy;
                        if (Obj.IsApproved == true && string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                        {
                            ESL.Add(new EmailSendLog  //Send email to Approval Owner2
                            {
                                From = commonSenderEmailId,
                                To = approvalOwner2EmailId,
                                CC = string.Empty,
                                BCC = string.Empty,
                                EmailSubject = "Requested punch regularisation application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">Punch regularisation application of " + applicantName + " for the date" +
                                " " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
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
                        Obj.IsReviewerCancelled = true;
                        Obj.IsCancelled = true;
                        Obj.ReviewerCancelledDate = DateTime.Now;
                        Obj.CancelledDate = DateTime.Now;
                        Obj.ReviewerCancelledBy = CancelledBy;
                        Obj.CancelledBy = CancelledBy;
                        if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
            {
                            ESL.Add(new EmailSendLog  //Send email to Approval Owner1
                {
                                From = commonSenderEmailId,
                                To = approvalOwner1EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                                EmailSubject = "Requested punch regularisation application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">Punch regularisation application of " + applicantName + " for the date" +
                                " " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
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
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                        {
                        ESL.Add(new EmailSendLog  //Send Mail to User
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested punch regularisation application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your punch regularisation application for the date" +
                            " " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
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
                    CTS.RA = Obj;
                CTS.AA = AA;
                    CTS.ESL = ESL;
                using (RAManualPunchApplicationRepository rAManualPunchApplicationRepository = new RAManualPunchApplicationRepository())
                {
                    rAManualPunchApplicationRepository.CancelApplication(CTS);
                }
                return "ok";
            }
            else   //If the punch regularisation application has already been cancelled then...
                            {
                throw new Exception("You cannot cancel a permission request that is already been cancelled.");
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
