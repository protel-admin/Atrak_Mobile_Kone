using System;
using System.Collections.Generic;
using Attendance.Model;
using Attendance.Repository;
using System.Web.Mvc;
using System.Configuration;

namespace Attendance.BusinessLogic
{
    public class RAOnDutyApplicationBusinessLogic
    {
        public List<RAODRequestApplication> GetAppliedOnDutyRequest(string StaffId, string ApplicationType)
        {
            using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
            {
                return rAOnDutyApplicationRepository.GetAppliedODRequest(StaffId, ApplicationType);
            }
        }

        public List<RAODRequestApplication> GetAppliedOnDutyRequestForMyTeam(string StaffId, string AppliedBy, string Role, string ApplicationType)
        {
            using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
            {
                return rAOnDutyApplicationRepository.GetAppliedODRequestForMyTeam(StaffId, AppliedBy, Role, ApplicationType);
            }
        }

        public string GetUniqueId()
        {
            using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
            {
                return rAOnDutyApplicationRepository.GetUniqueId();
            }
        }

        public List<OnDutyDuration> GetOnDutyDurations()
        {
            using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
            {
                return rAOnDutyApplicationRepository.GetOnDutyDurations();
            }
        }

        public List<SelectListItem> ConvertOnDutyDurationsToListItems(List<OnDutyDuration> onDutyDurations)
        {
            List<SelectListItem> _ListOfOnDutyDurations = new List<SelectListItem>();
            foreach (var l in onDutyDurations)
            {
                _ListOfOnDutyDurations.Add(new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                });
            }

            return _ListOfOnDutyDurations;
        }


        public List<SelectListItem> ConvertODTypetoListItems(List<LeaveView> ObjListOfLeaveTypes)
        {
            List<SelectListItem> _ListofLeaveTypes_ = new List<SelectListItem>();
            foreach (var l in ObjListOfLeaveTypes)
            {
                _ListofLeaveTypes_.Add(new SelectListItem
                {
                    Value = l.Id,
                    Text = l.Name
                });
            }
            return _ListofLeaveTypes_;
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

        public List<RAODRequestApplication> GetApprovedWFHForMyTeam(string staffId)
        {
            using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
            {
                return rAOnDutyApplicationRepository.GetApprovedWFHRequestForMyTeam(staffId);
            }
        }

        public List<LeaveView> GetODList()
        {
            using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
            {
                return rAOnDutyApplicationRepository.GetODList();
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
                    payPeriodValidationMessage = commonRepository.ValidateApplicationForPayDate(DataToSave.RA.StartDate, DataToSave.RA.EndDate);
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
                    approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.AA.ReviewerOwner);
                    appliedByUserEmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.RA.AppliedBy);
                    commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
                    appliedByUserName = commonRepository.GetStaffName(DataToSave.RA.AppliedBy);
                    approvalOwner1Name = commonRepository.GetStaffName(DataToSave.AA.ApprovalOwner);
                    applicantName = commonRepository.GetStaffName(DataToSave.RA.StaffId);
                    approvalOwner2Name = commonRepository.GetStaffName(DataToSave.AA.ReviewerOwner);
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
                else if (DataToSave.RA.AppliedBy.Equals(DataToSave.AA.ApprovalOwner) && DataToSave.RA.AppliedBy != DataToSave.AA.ReviewerOwner)
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
                else if (DataToSave.AA.ReviewerOwner.Equals(DataToSave.RA.AppliedBy))
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
                    DataToSave.RA.IsReviewed = true;
                    DataToSave.AA.ApprovalStatusId = 2;
                    DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedOn = DateTime.Now;
                    DataToSave.AA.ReviewerstatusId = 2;
                    DataToSave.AA.ReviewedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ReviewedOn = DateTime.Now;
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


        public string ValidateBeforeSave(string StaffId, string FromDate, string ToDate, string Duration)
        {
            string msg = string.Empty;
            using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
        {
                msg = rAOnDutyApplicationRepository.ValidateBeforeSave(StaffId, FromDate, ToDate, Duration);
            }
            if (!msg.ToUpper().StartsWith("OK"))
            {
                throw new Exception(msg);
            }
            return msg;
        }

        public void RejectApplication(string Id, string rejectedBy, string applicationType)
        {
            //Get the leave application details based on the id passed to this function as a parameter.
            RequestApplication Obj = new RequestApplication();
            ApplicationApproval AA = new ApplicationApproval();

            using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
            {
                Obj = rAOnDutyApplicationRepository.GetRequestApplicationDetails(Id);
                AA = rAOnDutyApplicationRepository.GetApplicationApproval(Id, applicationType);
            }
            
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            ClassesToSave CTS = new ClassesToSave();
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string fromDate = string.Empty;
            string toDate = string.Empty;
            string duration = string.Empty;
            string ReportingManagerName = string.Empty;
            string rejectedByUserName = string.Empty;
            string rejectedByUserEmailId = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            var mailbody = string.Empty;
            using (CommonRepository commonRepository = new CommonRepository())
            {
                rejectedByUserName = commonRepository.GetStaffName(rejectedBy);
                rejectedByUserEmailId = commonRepository.GetEmailIdOfEmployee(rejectedBy);
                applicantName = commonRepository.GetStaffName(Obj.StaffId);
                applicantEmailId = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
                approvalOwner1 = commonRepository.GetApproverOwner(Id);
                approvalOwner2 = commonRepository.GetReviewerOwner(Id);
                commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
            }

            if (Obj.ODDuration.ToUpper().Equals("SINGLEDAY"))
            {
                fromDate = Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy HH:mm");
                toDate = Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy HH:mm");
                duration = "Single Day";
            }
            else
            {
                fromDate = Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy");
                toDate = Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy");
                duration = "Multiple Days";
            }
            senderEmailId = commonSenderEmailId;
            //Check if the OD/BT application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true))    //if the OD/BT application has been cancelled then...
            {
                throw new Exception("Cancelled " + applicationType + " request cannot be rejected.");
            }
            else if (Obj.IsRejected.Equals(true))  //if the OD/BT application has been rejected then...
            {
                throw new Exception("Rejected " + applicationType + " request cannot be rejected.");
            }
            else //if the OD/BT application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {
                if (rejectedBy.Equals(approvalOwner1) && rejectedBy != approvalOwner2)
                {
                    //reject the application.
                    Obj.IsRejected = true;
                    AA.ApprovalStatusId = 3;
                    AA.ApprovedBy = rejectedBy;
                    AA.ApprovedOn = DateTime.Now;
                }

                else if (rejectedBy.Equals(approvalOwner2))
                {
                    //reject the application.
                    Obj.IsRejected = true;
                    if (approvalOwner1 == approvalOwner2)
                    {
                    AA.ApprovalStatusId = 3;
                        AA.ApprovedBy = rejectedBy;
                    AA.ApprovedOn = DateTime.Now;
                }
                    AA.ReviewerstatusId = 3;
                    AA.ReviewedBy = rejectedBy;
                    AA.ReviewedOn = DateTime.Now;
                    AA.Comment = "" + applicationType + " REQUEST HAS BEEN REJECTED BY THE APPROVAL OWNER2.";
                    }

                mailbody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your " + applicationType + " application from " + fromDate + " to " + toDate + " for " + duration + "" +
                            " has been rejected by " + rejectedByUserName + ".</p>" + "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + "" + rejectedByUserName + "" + " " +
                            "&nbsp;(" + rejectedBy + ")</p></body></html>";
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                    //email to applicant
                    ESL.Add(new EmailSendLog
                    {
                        From = senderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested " + applicationType + " application status",
                        EmailBody = mailbody,
                        CreatedOn = DateTime.Now,
                        CreatedBy = rejectedBy,
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
                using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
                {
                    rAOnDutyApplicationRepository.RejectApplication(CTS);
                }
            }
        }

        public void ApproveApplication(string Id, string approvedBy, string applicationType)
        {
            //Get the OD/BT application details based on the Id passed to this function as a parameter.
            RequestApplication Obj = new RequestApplication();
            ApplicationApproval AA = new ApplicationApproval();
            using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
        {
                Obj = rAOnDutyApplicationRepository.GetRequestApplicationDetails(Id);
                AA = rAOnDutyApplicationRepository.GetApplicationApproval(Id, applicationType);
            }
           
            List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
            ClassesToSave classesToSave = new ClassesToSave();

            string applicantName = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string applicantEmailId = string.Empty;
            string approvedByUserName = string.Empty;
            string approvedByUserEmailId = string.Empty;
            string approvalOwnerName = string.Empty;
            string approvalOwner2Name = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string fromDate = string.Empty;
            string toDate = string.Empty;
            string duration = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            bool isFinalLevelApproval = false;
            using (CommonRepository commonRepository = new CommonRepository())
            {
                applicantName = commonRepository.GetStaffName(Obj.StaffId);
                applicantEmailId = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
                approvalOwner1 = commonRepository.GetApproverOwner(Id);
                approvalOwner2 = commonRepository.GetReviewerOwner(Id);
                approvedByUserName = commonRepository.GetStaffName(approvedBy);
                approvedByUserEmailId = commonRepository.GetEmailIdOfEmployee(approvedBy);
                approvalOwner2Name = commonRepository.GetStaffName(approvalOwner2);
                approvalOwner2EmailId = commonRepository.GetEmailIdOfEmployee(approvalOwner2);
                commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
            }

            if (Obj.ODDuration.ToUpper().Equals("SINGLEDAY"))
                {
                fromDate = Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy HH:mm");
                toDate = Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy HH:mm");
                duration = "Single Day";
                }
            else
            {
                fromDate = Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy");
                toDate = Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy");
                duration = "Multiple Days";
            }
            senderEmailId = commonSenderEmailId;

            //Check if the OD/BT application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true)) //if the OD/BT application has been cancelled then...
            {
                throw new ApplicationException("Cannot approve a cancelled " + applicationType + " application.");
            }
            else if (Obj.IsRejected.Equals(true))
            {
                throw new ApplicationException("Cannot approve already rejected " + applicationType + " request.");
                }
                else
                {
                if (approvedBy.Equals(approvalOwner1) && approvedBy != approvalOwner2)
                {
                    Obj.IsApproved = true;
                    AA.ApprovalStatusId = 2;
                    AA.ApprovedBy = approvedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "APPROVED THE " + applicationType + " REQUEST.";
                    var mailbody = string.Empty;
                    mailbody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your " + applicationType + " application from " + fromDate + " to " + toDate + " for " + duration + "" +
                        "  has been approved and sent for second level approval.</p><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\"></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">" + approvedByUserName + " &nbsp;" +
                        "(" + approvedBy + ")</p></body></html>";

                    // Send intimation mail to the applicant
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
                            "" + applicantName + " has applied for a " + applicationType + " application and " +
                            "approved by " + approvedByUserName + ". Application details are given below.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma;" +
                            " font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">StaffId:</td><td style=\"width:80%;" +
                            "font-family:tahoma; font-size:9pt;\">" + Obj.StaffId + "</td></tr><tr><td style=\"width:20%;font-family:tahoma;" +
                            " font-size:9pt;\">From Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + fromDate + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">To Date:</td><td style=\"width:80%;font-family:tahoma;" +
                            " font-size:9pt;\">" + toDate + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                            "Duration:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + duration + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td><td style=\"width:80%;font-family:tahoma;" +
                            " font-size:9pt;\">" + Obj.Remarks + "</td></tr></table></p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Your attention is required to approve or reject this application.</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + approvedByUserName + " " +
                            "&nbsp; (" + approvedBy + ")</p></body></html>",
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
                    isFinalLevelApproval = true;
                    Obj.IsApproved = true;
                    Obj.IsReviewed = true;
                    if (approvalOwner1 == approvalOwner2)
                    {
                        AA.ApprovalStatusId = 2;
                        AA.ApprovedOn = DateTime.Now;
                        AA.ApprovedBy = approvedBy;
                    }
                    AA.ReviewerstatusId = 2;
                    AA.ReviewedBy = approvedBy;
                    AA.ReviewedOn = DateTime.Now;
                    AA.Comment = "APPROVED THE " + applicationType + " REQUEST.";

                    var mailbody = string.Empty;

                    mailbody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Your " + applicationType + " application from " + fromDate + " to " + toDate + "" +
                                " for " + duration + " has been approved.</p><p style=\"font-family:tahoma; font-size:9pt;\"></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">" + approvedByUserName + " &nbsp;(" + approvedBy + ")</p></body></html>";
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
                            CreatedBy = approvedBy,
                        IsSent = false,
                            SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                }
                classesToSave.RA = Obj;
                classesToSave.AA = AA;
                classesToSave.ESL = emailSendLogs;
                using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
                {
                    rAOnDutyApplicationRepository.ApproveApplication(classesToSave, isFinalLevelApproval);
                }
            }
        }

        public string CancelApplication(string id, string cancelledBy, string locationId)
        {
            //Get the OD/BT application details based on the Id passed to this function as a parameter.
            RequestApplication Obj = new RequestApplication();
            ApplicationApproval AA = new ApplicationApproval();
            ClassesToSave classesToSave = new ClassesToSave();
            using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
            {
                Obj = rAOnDutyApplicationRepository.GetRequestApplicationDetails(id);
                AA = rAOnDutyApplicationRepository.GetApplicationApproval(id, Obj.RequestApplicationType);
            }
            
            CommonBusinessLogic commonBusinessLogic = new CommonBusinessLogic();
            List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
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
            string senderEmailId = string.Empty;
            string payPeriodValidationMessage = string.Empty;
            string applicationType = string.Empty;

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
                cancelledByUserName = commonRepository.GetStaffName(cancelledBy);
                cancelledByUserEmailId = commonRepository.GetEmailIdOfEmployee(cancelledBy);
                commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
            }
            senderEmailId = commonSenderEmailId;
            payPeriodValidationMessage = commonBusinessLogic.ValidateApplicationForPayDate(Obj.StartDate , Obj.EndDate);
            if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false) && payPeriodValidationMessage.ToUpper() != "VALID")
            {
                throw new Exception("Application of past pay cycle cannot be cancelled");
            }
            applicationType = Obj.RequestApplicationType;
            if (Obj.IsCancelled.Equals(false))   //If the OD/BT application has not been cancelled then...
                {
                if (cancelledBy == Obj.StaffId)
                    {
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;
                    Obj.CancelledBy = cancelledBy;
                    if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                        {
                        emailSendLogs.Add(new EmailSendLog  //Send email to Approval Owner1
                            {
                            From = senderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                                BCC = string.Empty,
                            EmailSubject = "Requested " + applicationType + " application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"> " + applicationType + " application of " + applicantName + "" +
                            " for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
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
                    if (AA.ApprovalStatusId == 2 && AA.ApprovalOwner != AA.ReviewerOwner &&
                        string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                            {
                        emailSendLogs.Add(new EmailSendLog  //Send email to Approval Owner2
                            {
                            From = senderEmailId,
                            To = approvalOwner2EmailId,
                                CC = string.Empty,
                                BCC = string.Empty,
                            EmailSubject = "Requested " + applicationType + " application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + applicationType + " application of " + applicantName +
                            " for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
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
                else if (cancelledBy != Obj.StaffId)
                        {
                    if (cancelledBy == approvalOwner1)
                            {
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;
                        Obj.CancelledBy = cancelledBy;
                        Obj.IsApproverCancelled = true;
                        Obj.ApproverCancelledDate = DateTime.Now;
                        Obj.ApproverCancelledBy = cancelledBy;

                        if (Obj.IsApproved == true && string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                            {
                            emailSendLogs.Add(new EmailSendLog  //Send email to Approval Owner2
                            {
                                From = senderEmailId,
                                To = approvalOwner2EmailId,
                                CC = string.Empty,
                                BCC = string.Empty,
                                EmailSubject = "Requested " + applicationType + " application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">" + applicationType + " application of " + applicantName +
                                " for the dat " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
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
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;
                        Obj.CancelledBy = cancelledBy;
                        Obj.IsReviewerCancelled = true;
                        Obj.ReviewerCancelledDate = DateTime.Now;
                        Obj.ReviewerCancelledBy = cancelledBy;

                        if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                        {
                            emailSendLogs.Add(new EmailSendLog  //Send email to Approval Owner1
                            {
                                From = senderEmailId,
                                To = approvalOwner1EmailId,
                                CC = string.Empty,
                                BCC = string.Empty,
                                EmailSubject = "Requested " + applicationType + " application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">" + applicationType + " application of " + applicantName + "" +
                                "for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
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
                    Obj.IsCancelled = true;
                    Obj.CancelledDate = DateTime.Now;
                        Obj.CancelledBy = cancelledBy;

                        if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false) && AA.ApprovalStatusId == 2)
                    {
                            emailSendLogs.Add(new EmailSendLog  //Send email to Approval Owner1
                        {
                                From = senderEmailId,
                                To = approvalOwner1EmailId,
                                CC = string.Empty,
                            BCC = string.Empty,
                                EmailSubject = "Requested " + applicationType + " application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">" + applicationType + " application of " + applicantName + "" +
                                "for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
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
                        if (string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false) && AA.ReviewerstatusId == 2)
                        {
                            emailSendLogs.Add(new EmailSendLog  //Send email to Approval Owner1
                        {
                                From = senderEmailId,
                                To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                                EmailSubject = "Requested " + applicationType + " application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">" + applicationType + " application of " + applicantName + "" +
                                "for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
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
                            EmailSubject = "Requested " + applicationType + " application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your " + applicationType + " application for the date" +
                            " " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " has been cancelled.</p>" +
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
                classesToSave.RA = Obj;
                classesToSave.AA = AA;
                classesToSave.ESL = emailSendLogs;
                using (RAOnDutyApplicationRepository rAOnDutyApplicationRepository = new RAOnDutyApplicationRepository())
                    {
                    rAOnDutyApplicationRepository.CancelApplication(classesToSave);
                }
                return "OK";
            }
            else   //If the OD/BT application has already been cancelled then...
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
    }
}
