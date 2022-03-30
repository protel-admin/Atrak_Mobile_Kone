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
    public class RAPermissionApplicationBusinessLogic
    {
        
        public List<RAPermissionApplication> GetAppliedPermissions(string StaffId)
        {
            using (RAPermissionApplicationRepository rAPermissionApplicationRepository = new RAPermissionApplicationRepository())
            {
                return rAPermissionApplicationRepository.GetAppliedPermissions(StaffId);
            }
        }
        public List<RAPermissionApplication> GetAppliedPermissionsForMyTeam(string StaffId, string AppliedBy, string Role)
        {
            using (RAPermissionApplicationRepository rAPermissionApplicationRepository = new RAPermissionApplicationRepository())
            {
                return rAPermissionApplicationRepository.GetAppliedPermissionsForMyTeam(StaffId, AppliedBy, Role);
            }
        }

        public List<PermissionType> GetPermissionTypes()
        {
            using (RAPermissionApplicationRepository rAPermissionApplicationRepository = new RAPermissionApplicationRepository())
            {
                return rAPermissionApplicationRepository.GetPermissionTypes();
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

        public List<RAPermissionApplication> GetApprovedPermissions(string staffId)
        {
            using (RAPermissionApplicationRepository rAPermissionApplicationRepository = new RAPermissionApplicationRepository())
            {
                return rAPermissionApplicationRepository.GetApprovedPermissionsForMyTeam(staffId);
            }
        }

        public string GetUniqueId()
        {
            using (RAPermissionApplicationRepository rAPermissionApplicationRepository = new RAPermissionApplicationRepository())
            {
                return rAPermissionApplicationRepository.GetUniqueId();
            }
        }

        public void SaveRequestApplication(ClassesToSave DataToSave, string SecurityGroupId, string LocationId)
        {
            
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

                ValidateEligibility(DataToSave.RA.StaffId, Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy"), 
                    Convert.ToDateTime(DataToSave.RA.TotalHours).ToString("HH:mm:ss"));

            List<EmailSendLog> ESL = new List<EmailSendLog>();
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
                    ccEmailAddress = commonRepository.GetCCAddress("PERMISSION", LocationId);
                }
                try
                {
                    BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
                }
                catch
                {

                }
                if (DataToSave.RA.AppliedBy == DataToSave.RA.StaffId)
            {
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
            {
                    ESL.Add(new EmailSendLog
                    {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                        BCC = string.Empty,
                            EmailSubject = "Request for Permission application sent to " + approvalOwner1Name,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your Permission application for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + " " +
                            " has been submitted to your Reporting Manager  (" + approvalOwner1Name + ") for approval.</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Best Regards</p></body></html>",
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
                            EmailSubject = "Request for Permission application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner1Name + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + applicantName + "" +
                            " has applied for a permission. Permission details are given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"></p>" +
                            "<p><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma;" +
                            " font-size:9pt;\">StaffId:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Date:</td><td style=\"width:80%;font-family:tahoma; " +
                            "font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Time From:</td><td style=\"width:80%;font-family:tahoma;" +
                            " font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("HH:mm") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Time To:</td><td style=\"width:80%;font-family:tahoma;" +
                            " font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("HH:mm") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Total Hours:</td><td style=\"width:80%;font-family:tahoma;" +
                            " font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.TotalHours).ToString("HH:mm") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td><td style=\"width:80%;font-family:tahoma;" +
                            " font-size:9pt;\">" + DataToSave.RA.Remarks + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your attention is required for this application.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "<a href=\"" + BaseAddress + "\">Click Here</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + applicantName + " &nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>",
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

            // If Reporting Manager or Approval Owner1 has applied the application
                else if (DataToSave.RA.AppliedBy == DataToSave.AA.ApprovalOwner &&
                        DataToSave.RA.AppliedBy != DataToSave.AA.ReviewerOwner)
            {
                DataToSave.RA.IsApproved = true;
                DataToSave.RA.IsReviewed = false;
                DataToSave.AA.ApprovalStatusId = 2;
                DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                DataToSave.AA.ApprovedOn = DateTime.Now;
                DataToSave.AA.ReviewerstatusId = 1;
                DataToSave.AA.Comment = "APPROVED THE PERMISSION REQUEST.";
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        //Send acknowledgement to applicant 
                ESL.Add(new EmailSendLog
                {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                    CC = string.Empty,
                    BCC = string.Empty,
                    EmailSubject = "Requested permission application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your permission application" +
                            " for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + " has been applied and approved by " +
                            "" + appliedByUserName + " and sent for second level approval. </p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + appliedByUserName + " &nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>",
                    CreatedOn = DateTime.Now,
                            CreatedBy = DataToSave.RA.AppliedBy,
                    IsSent = false,
                            SentOn = null,
                    IsError = false,
                    ErrorDescription = "-",
                    SentCounter = 0
                });
                    }
                    //Send intimation to Reviewer or Approval Owner 2
                    if (string.IsNullOrEmpty(DataToSave.AA.ReviewerOwner).Equals(false) &&
                        string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                            EmailSubject = "Permission application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner2Name + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            " Permission  application of " + applicantName + " has been applied and approved by " + appliedByUserName + "" +
                            " and sent for your approval.. Permission details are given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"></p>" +
                            "<p><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma;" +
                            " font-size:9pt;\">StaffId:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Date:</td><td style=\"width:80%;font-family:tahoma; " +
                            "font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Time From:</td><td style=\"width:80%;font-family:tahoma;" +
                            " font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("HH:mm") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Time To:</td><td style=\"width:80%;font-family:tahoma;" +
                            " font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("HH:mm") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Total Hours:</td><td style=\"width:80%;font-family:tahoma;" +
                            " font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.TotalHours).ToString("HH:mm") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td><td style=\"width:80%;font-family:tahoma;" +
                            " font-size:9pt;\">" + DataToSave.RA.Remarks + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your attention is required to approve or reject this application.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "<a href=\"" + BaseAddress + "\">Click Here</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + appliedByUserName + " &nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>",
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

            // If Reviewer or Approval Owner 2 has applied the application
                else if (DataToSave.RA.AppliedBy == DataToSave.AA.ReviewerOwner)
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
                DataToSave.AA.Comment = "REVIEWED THE PERMISSION REQUEST.";
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                ESL.Add(new EmailSendLog
                {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = ccEmailAddress,
                    BCC = string.Empty,
                    EmailSubject = "Requested permission application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your permission application for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + " " +
                            "has been applied and approved.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + approvalOwner1Name + " &nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>",
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

                if (DataToSave.RA.AppliedBy != DataToSave.RA.StaffId && (SecurityGroupId == "3" ||
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
                    DataToSave.AA.Comment = "APPROVED & REVIEWED THE PERMISSION REQUEST.";
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = ccEmailAddress,
                            BCC = string.Empty,
                            EmailSubject = "Requested permission application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your permission application for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                            " has been applied and approved.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">" + appliedByUserName + " &nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>",
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
                using (RAPermissionApplicationRepository rAPermissionApplicationRepository = new RAPermissionApplicationRepository())
                {
                    rAPermissionApplicationRepository.SaveRequestApplication(DataToSave, isFinalLevelApproval);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string ValidateEligibility(string StaffId, string ToDate, string TotalHours)
        {
            string str = string.Empty;
            using (RAPermissionApplicationRepository rAPermissionApplicationRepository = new RAPermissionApplicationRepository())
            {
                str= rAPermissionApplicationRepository.ValidatePermissionOffApplication(StaffId, ToDate, TotalHours);
            }
            if (!str.ToUpper().StartsWith("OK"))
            {
                throw new ApplicationException(str);
            }
            return str;
        }

        public void RejectApplication(string Id, string RejectedBy, string LocationId)
        {
            //Get the permission application details based on the id passed to this function as a parameter.
            RequestApplication Obj = new RequestApplication();
            ApplicationApproval AA = new ApplicationApproval();

            using (RAPermissionApplicationRepository rAPermissionApplicationRepository = new RAPermissionApplicationRepository())
        {
                Obj = rAPermissionApplicationRepository.GetRequestApplicationDetails(Id);
                AA = rAPermissionApplicationRepository.GetApplicationApproval(Id);
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
                ccEmailAddress = commonRepository.GetCCAddress("PERMISSION", LocationId);
            }

            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true))    //if the leave application has been cancelled then...
            {
                throw new Exception("Cancelled permission request cannot be rejected.");
            }
            else if (Obj.IsRejected.Equals(true))  //if the permission application has been rejected then...
            {
                throw new Exception("Rejected permission request cannot be rejected.");
            }
            else //if the permission application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {
                if (RejectedBy == approvalOwner1)
                {
                    //Reject the application.
                    Obj.IsRejected = true;
                    AA.ApprovalStatusId = 3;
                    AA.ApprovedBy = RejectedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "PERMISSION REQUEST HAS BEEN REJECTED BY THE REPORTING MANAGER.";
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
                    AA.ReviewerstatusId = 3;
                    AA.ReviewedOn = DateTime.Now;
                    AA.ReviewedBy = RejectedBy;
                    AA.Comment = "PERMISSION REQUEST HAS BEEN REJECTED BY THE REPORTING MANAGER.";
                }

                    ESL.Add(new EmailSendLog
                    {
                    From = commonSenderEmailId,
                    To = applicantEmailId,
                    CC = ccEmailAddress,
                        BCC = string.Empty,
                        EmailSubject = "Requested permission application status",
                    EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                       "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                       "Your permission application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy")
                       + " has been rejected.</p>" + "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                       "<p style=\"font-family:tahoma; font-size:9pt;\">" + "" + rejectedByUserName + " " +
                       "&nbsp;(" + RejectedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                    CreatedBy = RejectedBy,
                        IsSent = false,
                    SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });

                    CTS.RA = Obj;
                    CTS.AA = AA;
                    CTS.ESL = ESL;
                using (RAPermissionApplicationRepository rAPermissionApplicationRepository = new RAPermissionApplicationRepository())
                {
                    rAPermissionApplicationRepository.RejectApplication(CTS);
                }
            }
        }

        public void ApproveApplication(string Id, string ApprovedBy, string LocationId)
        {
            //Get the leave application details based on the Id passed to this function as a parameter.
            RequestApplication Obj = new RequestApplication();
            ApplicationApproval AA = new ApplicationApproval();

            using (RAPermissionApplicationRepository rAPermissionApplicationRepository = new RAPermissionApplicationRepository())
            {
                 Obj = rAPermissionApplicationRepository.GetRequestApplicationDetails(Id);
                 AA = rAPermissionApplicationRepository.GetApplicationApproval(Id);
            }
            var cm = new CommonRepository();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            ClassesToSave CTS = new ClassesToSave();
            string BaseAddress = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner2Name = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string approvedByUserName = string.Empty;
            string approvedByUserEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string ccEmailAddress = string.Empty;
            BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
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
                ccEmailAddress = commonRepository.GetCCAddress("PERMISSION", LocationId);
                
            }

            if (ApprovedBy == approvalOwner1 && Obj.IsApproved.Equals(true))
                {
                throw new Exception("Cannot approve already approved permission request.");
            }
            else if (ApprovedBy == approvalOwner2 && Obj.IsReviewed.Equals(true))
            {
                throw new Exception("Cannot approve already approved permission request.");
            }

            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true)) //if the leave application has been cancelled then...
            {
                throw new Exception("Cannot approve a cancelled permission application.");
            }
            else if (Obj.IsRejected.Equals(true))
            {
                throw new Exception("Cannot approve already rejected permission request.");
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
                    AA.Comment = "APPROVED THE PERMISSION REQUEST.";

                    if (string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                    ESL.Add(new EmailSendLog
                    {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                        BCC = string.Empty,
                            EmailSubject = "Request for Permission application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner2Name + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + approvedByUserName + " has approved for a permission of " + applicantName + ". Permission details are given below.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"></p><p><table border=\"1\" style=\"width:50%;font-family:tahoma; " +
                            "font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma;font-size:9pt;\">StaffId:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Obj.StaffId + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Date:</td><td style=\"width:80%;font-family:tahoma;" +
                            "font-size:9pt;\">" + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Time From:</td><td style=\"width:80%;" +
                            "font-family:tahoma;font-size:9pt;\">" + Convert.ToDateTime(Obj.StartDate).ToString("HH:mm") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Time To:</td><td style=\"width:80%;" +
                            "font-family:tahoma;font-size:9pt;\">" + Convert.ToDateTime(Obj.EndDate).ToString("HH:mm") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Total Hours:</td><td style=\"width:80%;" +
                            "font-family:tahoma;font-size:9pt;\">" + Convert.ToDateTime(Obj.TotalHours).ToString("HH:mm") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td><td style=\"width:80%;" +
                            "font-family:tahoma;font-size:9pt;\">" + Obj.Remarks + "</td></tr></table></p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Your attention is required to approve or reject this application.</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\"><a href=\"" + BaseAddress + "\">Click Here</a></p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + approvedByUserName + "" +
                            " &nbsp;(" + ApprovedBy + ")</p></body></html>",
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
                if (ApprovedBy == approvalOwner2)
                {
                    //approve the application.
                    Obj.IsApproved = true;
                    Obj.IsReviewed = true;
                    if (approvalOwner2 == approvalOwner1)
                    {
                    AA.ApprovalStatusId = 2;
                        AA.ApprovedBy = ApprovedBy;
                    AA.ApprovedOn = DateTime.Now;
                }
                    AA.ReviewerstatusId = 2;
                    AA.ReviewedBy = ApprovedBy;
                    AA.ReviewedOn = DateTime.Now;
                    AA.Comment = "APPROVED THE PERMISSION REQUEST.";
                }

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = ccEmailAddress,
                        BCC = string.Empty,
                        EmailSubject = "Requested permission application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your permission application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + "" +
                        " has been approved..</p><p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">Click Here</a></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
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
                    CTS.RA = Obj;
                    CTS.AA = AA;
                    CTS.ESL = ESL;
                using (RAPermissionApplicationRepository rAPermissionApplicationRepository = new RAPermissionApplicationRepository())
                {
                    rAPermissionApplicationRepository.ApproveApplication(CTS);
                }
            }
        }

        public string CancelApplication(string Id, string CancelledBy, string LocationId)
        {
            //Get the leave application details based on the Id passed to this function as a parameter.
            RequestApplication Obj = new RequestApplication();
            ApplicationApproval AA = new ApplicationApproval();

            using (RAPermissionApplicationRepository rAPermissionApplicationRepository = new RAPermissionApplicationRepository())
            {
                 Obj = rAPermissionApplicationRepository.GetRequestApplicationDetails(Id);
                 AA = rAPermissionApplicationRepository.GetApplicationApproval(Id);
            }
            ClassesToSave CTS = new ClassesToSave();
           
            
            var CBL = new CommonBusinessLogic();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            //first send acknowledgement email to the user.
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
                    if (Obj.IsCancelled.Equals(false))   //If the permission application has not been cancelled then...
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
                                    EmailSubject = "Requested permission application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"> Permission application of " + applicantName + " for the date" +
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
                                    EmailSubject = "Requested permission application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Permission application of " + applicantName + " for the date" +
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
                    Obj.IsApproverCancelled = true;
                            Obj.IsCancelled = true;
                            Obj.CancelledDate = DateTime.Now;
                    Obj.ApproverCancelledDate = DateTime.Now;
                        Obj.ApproverCancelledBy = CancelledBy;
                        Obj.CancelledBy = CancelledBy;
                        if (Obj.IsApproved == true && string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                            {
                            ESL.Add(new EmailSendLog  //Send email to Approval Owner2
                                {
                                From = commonSenderEmailId,
                                To = approvalOwner2EmailId,
                                    CC = string.Empty,
                                    BCC = string.Empty,
                                    EmailSubject = "Requested permission application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">Permission application of " + applicantName + " for the date" +
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
                            EmailSubject = "Requested permission application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">Permission application of " + applicantName + "for the date" +
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
                            EmailSubject = "Requested permission application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your permission application for the date" +
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
                        CTS.ESL = ESL;
                using (RAPermissionApplicationRepository rAPermissionApplicationRepository = new RAPermissionApplicationRepository())
                    {
                    rAPermissionApplicationRepository.CancelApplication(CTS);
                    }
                return "OK";
                }
            else   //If the permission application has already been cancelled then...
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
        #region Common Permission
        public string BulkSaveCommonPermissionBusinessLogic(CommonPermissionModel model, string StaffList, string CreatedBy)
        {
            using (RAPermissionApplicationRepository rAPermissionApplicationRepository = new RAPermissionApplicationRepository())
            {
                return rAPermissionApplicationRepository.BulkSaveCommonPermissionRepository(model, StaffList, CreatedBy);
            }
        }
        #endregion
    }
}
