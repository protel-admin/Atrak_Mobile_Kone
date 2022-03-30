using Attendance.Model;
using Attendance.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Attendance.BusinessLogic
{
    public class RACoffAvalingApplicationBusinessLogic
    {
        public void SaveRequestApplication(ClassesToSave DataToSave, string SecurityGroupId, string BaseAddress)
        {
            using (RACoffAvailingApplicationRepository repo = new RACoffAvailingApplicationRepository())
            {
                using (var cm = new CommonRepository())
                { 
                    List<EmailSendLog> ESL = new List<EmailSendLog>();
                EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
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
                string payPeriodValidationMessage = string.Empty;
                string applicationValidationMessage = string.Empty;
                string overlappingValidationMessage = string.Empty;
                CommonBusinessLogic CBL = new CommonBusinessLogic();

                applicantEmailId = cm.GetEmailIdOfEmployee(DataToSave.RA.StaffId);
                approvalOwner1EmailId = cm.GetEmailIdOfEmployee(DataToSave.AA.ApprovalOwner);
                approvalOwner2EmailId = cm.GetEmailIdOfEmployee(DataToSave.AA.Approval2Owner);
                appliedByUserEmailId = cm.GetEmailIdOfEmployee(DataToSave.RA.AppliedBy);
                commonSenderEmailId = cm.GetSenderEmailIdFromEmailSettings();
                StaffName = cm.GetStaffName(DataToSave.RA.StaffId);
                ReportingManagerName = cm.GetStaffName(DataToSave.AA.ApprovalOwner);
                approvalOwner2Name = cm.GetStaffName(DataToSave.AA.Approval2Owner);
                appliedByUserName = cm.GetStaffName(DataToSave.RA.AppliedBy);

                applicationValidationMessage = ValidateCoffAvailing(DataToSave.RA.StaffId, Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy"),
                        Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy"), DataToSave.RA.TotalDays, Convert.ToDateTime(DataToSave.RA.WorkedDate).ToString("dd-MMM-yyyy"));
                overlappingValidationMessage = ValidateApplicationOverlaping(DataToSave.RA.StaffId, Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy"), DataToSave.RA.LeaveStartDurationId.ToString(),
                    Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy"), DataToSave.RA.LeaveEndDurationId.ToString());

                payPeriodValidationMessage = CBL.ValidateApplicationForPayDate(DataToSave.RA.StaffId, Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy"),
                        Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy"));
                if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
                {
                    if (payPeriodValidationMessage.ToUpper() != "VALID")
                    {
                        throw new ApplicationException("Application of past pay cycle cannot be saved");
                    }
                }
                string LeaveTypeName = string.Empty;
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
                            EmailSubject = "Request for Coff Availing application sent to " + ReportingManagerName,
                            EmailBody = "<html><head><title></title></head><body>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br>" +
                            "Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your Coff Availing application has been sent to your Reporting Manager " +
                            " (" + ReportingManagerName + ") for approval.<p style=\"font-family:tahoma;" +
                            " font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma;" +
                            " font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;" +
                            "\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" +
                            "" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma;" +
                            " font-size:9pt;\">Worked Date:</td><td style=\"width:80%;font-family:tahoma;" +
                            " font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.WorkedDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">From Date:</td><td style=\"width:80%;" +
                            "font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy")
                            + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                            "Coff Avail To:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" +
                            "" + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Total Days:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" +
                            "" + DataToSave.RA.TotalDays + "</td></tr>" +
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
                            EmailSubject = "Request for Coff availing application of " + StaffName,
                            EmailBody = "<html><head><title></title></head><body>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br>" +
                            "Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your Coff Availing application has been sent to your Reporting Manager " +
                            " (" + ReportingManagerName + ") for approval.<p style=\"font-family:tahoma;" +
                            " font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma;" +
                            " font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;" +
                            "\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" +
                            "" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma;" +
                            " font-size:9pt;\">Worked Date:</td><td style=\"width:80%;font-family:tahoma;" +
                            " font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.WorkedDate).ToString("dd-MMM-yyyy") +
                            "</td></tr><tr><td style=\"width:20%;font-family:tahoma;" +
                            " font-size:9pt;\"> From Date:</td><td style=\"width:80%;font-family:tahoma; " +
                            "font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy")
                            + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">" +
                            "To Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" +
                            "" + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Total Days:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" +
                            "" + DataToSave.RA.TotalDays + "</td></tr>" +
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
                    DataToSave.AA.Comment = "REVIEWED THE PERMISSION REQUEST.";

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        //Send intimation to the applicant 
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested Coff Availing application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your permission  application " +
                            "for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                            " has been applied and approved by " + appliedByUserName + "" +
                            " and sent for an Approval..</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
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
                            EmailSubject = "Request for Coff Availing application of " + StaffName,
                            EmailBody = "<html><head><title></title></head><body>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner2Name + "" +
                            ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + ReportingManagerName + " has applied and approved for a Coff Availing. Coff Availing details given below.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                            "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr>" +
                            "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Worked Date:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime
                            (DataToSave.RA.WorkedDate).ToString("dd-MMM-yyyy") + "</td></tr><tr>" +
                            "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">From Date:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime
                            (DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;" +
                            "font-family:tahoma; font-size:9pt;\">Time To:</td><td style=\"width:80%;" +
                            "font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.EndDate)
                            .ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma;" +
                            " font-size:9pt;\">Total Hours:</td><td style=\"width:80%;font-family:tahoma;" +
                            " font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.TotalHours).ToString("HH:mm") +
                            "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
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
                    DataToSave.AA.Comment = "APPROVED THE COFF AVAILING REQUEST.";
                    // Deduct the comp-off balance from employee leave account
                    ELA.StaffId = DataToSave.RA.StaffId;
                    ELA.LeaveTypeId = DataToSave.RA.LeaveTypeId;
                    ELA.TransactionFlag = 2;
                    ELA.TransactionDate = DateTime.Now;
                    ELA.LeaveCount = Convert.ToDecimal(DataToSave.RA.TotalDays) * -1;
                    ELA.Narration = "Approved the comp-off availing application.";
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
                            EmailSubject = "Requested Coff Availing application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Your Coff Availing  application" +
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
                    DataToSave.ESL = ESL;
                }

                if (DataToSave.RA.StaffId != DataToSave.RA.AppliedBy && (SecurityGroupId.Equals("1") || SecurityGroupId.Equals("3") ||
                    SecurityGroupId.Equals("5")))
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
                    DataToSave.AA.Comment = "APPROVED THE COFF AVAILING REQUEST.";

                    //Deduct the comp-off balance from employee leave account table.
                    ELA.StaffId = DataToSave.RA.StaffId;
                    ELA.LeaveTypeId = DataToSave.RA.LeaveTypeId;
                    ELA.TransactionFlag = 2;
                    ELA.TransactionDate = DateTime.Now;
                    ELA.LeaveCount = Convert.ToDecimal(DataToSave.RA.TotalDays) * -1;
                    ELA.Narration = "Approved the comp-off availing application.";
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
                            EmailSubject = "Requested Coff Availing application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your Coff Availing  application for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).
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
                repo.SaveRequestApplication(DataToSave, isFinalLevelApproval);
                }
            }
        }
        public string GetCoffReqPeriodBusinessLogic()
        {
            using (RACoffAvailingApplicationRepository Repo = new RACoffAvailingApplicationRepository())

                return Repo.GetCoffReqPeriodRepository();
        }
        public string ValidateCoffAvailing(string StaffId, string COffFromDate, string COffToDate, decimal TotalDays, string COffReqDate)
        {
            using (RACoffAvailingApplicationRepository Repo = new RACoffAvailingApplicationRepository())
                return Repo.ValidateCoffAvailing(StaffId, COffFromDate, COffToDate, TotalDays, COffReqDate);
        }
        public string ValidateApplicationOverlaping(string StaffId, string CoffStartDate, string FromDurationId, string CoffEndDate, string ToDurationId)
        {
            using (RACoffAvailingApplicationRepository repo = new RACoffAvailingApplicationRepository())
                return repo.ValidateApplicationOverlaping(StaffId, CoffStartDate, FromDurationId, CoffEndDate, ToDurationId);
        }
        public string GetUniqueId()
        {
            using (var repo = new RACoffAvailingApplicationRepository())
                return repo.GetUniqueId();
        }
        public List<RACoffAvailingRequestApplication> RenderAppliedCompAvailingList(string StaffId, string AppliedBy, string userRole)
        {
            using (RACoffAvailingApplicationRepository repo = new RACoffAvailingApplicationRepository())
            { 
                var Obj = repo.RenderAppliedCompAvailingList(StaffId, AppliedBy, userRole);
            return Obj;
            }
        }
        public List<RACoffAvailingRequestApplication> RenderAppliedCompAvailingListMyteam(string StaffId, string AppliedBy, string userRole)
        {
            using (RACoffAvailingApplicationRepository repo = new RACoffAvailingApplicationRepository())
            { 
                var Obj = repo.RenderAppliedCompAvailingListMyteam(StaffId, AppliedBy, userRole);
            return Obj;
            }
        }


        public List<RACoffAvailingRequestApplication> GetCoffAvailRequestMappedUnderMe(string loggedInUser)
        {
            using (RACoffAvailingApplicationRepository repo = new RACoffAvailingApplicationRepository())
            { 
                var Obj = repo.GetCoffAvailRequestMappedUnderMe(loggedInUser);
            return Obj;
            }
        }

        //Cancel Pending C-off Availling application
        public string CancelCoffAvaillingApplication(string Id, string CancelledBy)
        {
            //Get the C-Off availing application details based on the Id passed to this function as a parameter.
            using (RACoffAvailingApplicationRepository repo = new RACoffAvailingApplicationRepository())
            {
                ClassesToSave CTS = new ClassesToSave();
                DateTime currentDate = DateTime.Now;
                var Obj = repo.GetRequestApplicationDetails(Id);
                var AA = repo.GetApplicationApprovalForCoffAvailing(Id);
                var cm = new CommonRepository();
                List<EmailSendLog> ESL = new List<EmailSendLog>();
                EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
                CommonBusinessLogic CBL = new CommonBusinessLogic();
                string approvalOwner1Name = string.Empty;
                StringBuilder INandOUTData = new StringBuilder();
                DateTime? ApplicationDate = DateTime.Now;
                string approvalOwner2Name = string.Empty;
                string applicantName = string.Empty;
                string applicantEmailId = string.Empty;
                string cancelledByUserEmailId = string.Empty;
                string cancelledByUserName = string.Empty;
                string commonSenderEmailId = string.Empty;
                string senderEmailId = string.Empty;
                string approvalOwner1EmailId = string.Empty;
                string approvalOwner2EmailId = string.Empty;
                string payPeriodValidationMessage = string.Empty;

                approvalOwner1Name = cm.GetStaffName(AA.ApprovalOwner);
                approvalOwner2Name = cm.GetStaffName(AA.Approval2Owner);
                applicantName = cm.GetStaffName(Obj.StaffId);
                applicantEmailId = cm.GetEmailIdOfEmployee(Obj.StaffId);
                cancelledByUserName = cm.GetStaffName(CancelledBy);
                commonSenderEmailId = cm.GetSenderEmailIdFromEmailSettings();
                applicantEmailId = cm.GetEmailIdOfEmployee(Obj.StaffId);
                cancelledByUserEmailId = cm.GetEmailIdOfEmployee(CancelledBy);
                approvalOwner1EmailId = cm.GetEmailIdOfEmployee(AA.ApprovalOwner);
                approvalOwner2EmailId = cm.GetEmailIdOfEmployee(AA.Approval2Owner);
                if (Obj.AppliedBy == "")
                {
                    Obj.AppliedBy = Obj.StaffId;
                }
                if (Obj.IsCancelled.Equals(false))   //If the coff availing application has not been cancelled then...
                {
                    payPeriodValidationMessage = CBL.ValidateApplicationForPayDate(Obj.StaffId, Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy")
                , Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy"));
                    if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
                    {
                        if (payPeriodValidationMessage.ToUpper() != "VALID")
                        {
                            throw new ApplicationException("Application of past pay cycle cannot be cancelled");
                        }
                    }
                    //Cancel the coff availing application which is in pending state.
                    Obj.IsCancelled = true;
                    Obj.CancelledDate = DateTime.Now;
                    Obj.CancelledBy = CancelledBy;

                    if (AA.Approval2statusId == 2)
                    {
                        ELA.StaffId = Obj.StaffId;
                        ELA.LeaveTypeId = Obj.LeaveTypeId;
                        ELA.TransactionFlag = 1;
                        ELA.TransactionDate = DateTime.Now;
                        ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays);
                        ELA.Narration = "Cancelled the approved C-Off availing application.";
                        ELA.LeaveCreditDebitReasonId = 20;
                        ELA.RefId = Obj.Id;
                        ELA.WorkedDate = Obj.WorkedDate;
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
                                EmailSubject = "Requested C-Off availing application status ",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "C-Off availing application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
                                " has been cancelled. <p style =\"font-family:tahoma; font-size:9pt;\">Best Regards," +
                                "</p> <p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " " +
                                "&nbsp;(" + CancelledBy + ")</p></body></html>",
                                CreatedOn = DateTime.Now,
                                CreatedBy = Obj.AppliedBy,
                                IsSent = false,
                                SentOn = null,
                                IsError = false,
                                ErrorDescription = "-",
                                SentCounter = 0
                            });
                        }
                        if (string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false) &&
                            AA.Approval2statusId == 2)
                        {
                            ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner2
                            {
                                From = commonSenderEmailId,
                                To = approvalOwner2EmailId,
                                CC = string.Empty,
                                BCC = string.Empty,
                                EmailSubject = "Requested C-Off availing application status ",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "C-Off availing application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
                                " has been cancelled. <p style =\"font-family:tahoma; font-size:9pt;\">Best Regards," +
                                "</p> <p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " " +
                                "&nbsp;(" + CancelledBy + ")</p></body></html>",
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
                                EmailSubject = "Requested C-Off availing application status ",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "C-Off availing application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
                                " has been cancelled. <p style =\"font-family:tahoma; font-size:9pt;\">Best Regards," +
                                "</p> <p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " " +
                                "&nbsp;(" + CancelledBy + ")</p></body></html>",
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
                                EmailSubject = "Requested C-Off availing application status ",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "C-Off availing application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
                                " has been cancelled. <p style =\"font-family:tahoma; font-size:9pt;\">Best Regards," +
                                "</p> <p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " " +
                                "&nbsp;(" + CancelledBy + ")</p></body></html>",
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
                                EmailSubject = "Requested Coff availing application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">Your c-off availng application " +
                                "for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
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
                    CTS.ELA = ELA;
                    CTS.AA = AA;
                    repo.CancelApplication(CTS, CancelledBy);
                }
                else //If the leave application has already been cancelled then...
                {
                    //throw exception.
                    throw new Exception("You cannot cancel a c-off Availling that is already been cancelled.");
                }
                return "OK";
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
        //Coff availlingapplication
        public void ApproveCoffAvaillingApplication(string Id, string ApprovedBy, string loggedInUser)
        {
            //Get the C-Off availing application details based on the Id passed to this function as a parameter.
            using (RACoffAvailingApplicationRepository repo = new RACoffAvailingApplicationRepository())
            {
                var Obj = repo.GetRequestApplicationDetails(Id);
                var AA = repo.GetApplicationApprovalForCoffAvailing(Id);
                using (var cm = new CommonRepository())
                {
                    List<EmailSendLog> ESL = new List<EmailSendLog>();
                    EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
                    ClassesToSave CTS = new ClassesToSave();
                    string staffId = string.Empty;
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
                    //Check if the C-Off availing application has been cancelled or not.
                    if (Obj.IsCancelled.Equals(true)) //if the C-Off availing application has been cancelled then...
                    {
                        throw new Exception("Cannot approve a cancelled c-off Availing application.");
                    }
                    else if (Obj.IsRejected.Equals(true))
                    {
                        throw new Exception("Cannot approve already rejected c-off Availing request.");
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
                            AA.Comment = "APPROVED THE COMP-OFF AVAILING.";

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
                                    EmailSubject = "Requested C-Off availing application status",
                                    EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                    "Dear " + approvalOwner2Name + ",<br/><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                    "" + approvedByUserName + " has approved a C-Off availing application of " + applicantName + ". " +
                                    "C-Off availing details are given below.</p>" +
                                    "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;" +
                                    "font-family:tahoma; font-size:9pt;\"> <tr><td style=\"width:20%;font-family:tahoma; " +
                                    "font-size:9pt;\">Employee Code:</td> <td style=\"width:80%;font-family:tahoma; " +
                                    "font-size:9pt;\">" + Obj.StaffId + "</td></tr> Worked Date:</td>" +
                                    "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime
                                    (Obj.WorkedDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                                    "< tr >< td style =\"width:20%;font-family:tahoma; font-size:9pt;\"> From Date:</td>" +
                                    "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime
                                    (Obj.StartDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                                    "< tr >< td style =\"width:20%;font-family:tahoma; font-size:9pt;\"> To Date:</td>" +
                                    "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime
                                    (Obj.EndDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                                    "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Total Days:</td>" +
                                    "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Obj.TotalDays + "</td></tr>" +
                                    "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                                    "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Obj.Remarks + "</td></tr>" +
                                    "</table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required " +
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
                            AA.Comment = "APPROVED THE C-Off AVAILING.";

                            ELA.StaffId = Obj.StaffId;
                            ELA.LeaveTypeId = Obj.LeaveTypeId;
                            ELA.TransactionFlag = 2;
                            ELA.TransactionDate = DateTime.Now;
                            ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays) * -1;
                            ELA.Narration = "APPROVED THE C-OFF AVAILING.";
                            ELA.LeaveCreditDebitReasonId = 22;
                            ELA.RefId = Obj.Id;
                            ELA.TransctionBy = approvedBy;
                            ELA.Month = Convert.ToDateTime(Obj.StartDate).Month;
                            ELA.Year = Convert.ToDateTime(Obj.StartDate).Year;
                        }

                        if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                        {
                            ESL.Add(new EmailSendLog
                            {
                                From = commonSenderEmailId,
                                To = applicantEmailId,
                                CC = string.Empty,
                                BCC = string.Empty,
                                EmailSubject = "Requested C-Off availing application status",
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Your C-Off availing application from " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
                                " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + " has been approved.</p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\"> Best Regards</p><p style=\"font-family:tahoma; " +
                                "font-size:9pt;\">" + approvedByUserName + " &nbsp; (" + approvedBy + ")</p></body></html>",
                                CreatedOn = DateTime.Now,
                                CreatedBy = approvedBy,
                                IsSent = false,
                                SentOn = null,
                                IsError = false,
                                ErrorDescription = "-",
                                SentCounter = 0
                            });
                        }
                        CTS.RA = Obj;
                        CTS.ELA = ELA;
                        CTS.AA = AA;
                        CTS.ESL = ESL;
                        repo.ApproveApplication(CTS, loggedInUser, isFinalLevelApproval);
                    }
                }
            }
        }

        //Coff availlingapplication
        public void RejectCoffAvaillingApplication(string Id, string RejectedBy)
        {
            //Get the coff availing application details based on the id passed to this function as a parameter.
            using (RACoffAvailingApplicationRepository repo = new RACoffAvailingApplicationRepository())
            {

            
                var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApprovalForCoffAvailing(Id);
                using (var cm = new CommonRepository())
                { 
                    List<EmailSendLog> ESL = new List<EmailSendLog>();
            ClassesToSave CTS = new ClassesToSave();
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
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
            //Check if the coff availing application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true))    //if the coff availing application has been cancelled then...
            {
                throw new Exception("Cancelled c-off availing application cannot be rejected.");
            }
            else if (Obj.IsRejected.Equals(true))  //if the coff availing application has been rejected then...
            {
                throw new Exception("Rejected c-off availing cannot be rejected.");
            }
            else //if the coff availing application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {
                //reject the application.
                if (RejectedBy == approvalOwner1 && RejectedBy != approvalOwner2)
                {
                    Obj.IsRejected = true;
                    AA.ApprovalStatusId = 3;
                    AA.ApprovedBy = RejectedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "COFF AVAILING HAS BEEN REJECTED BY THE APPROVALOWNER1.";
                }
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
                    AA.Comment = "C-OFF AVAILLING HAS BEEN REJECTED BY THE APPROVALOWNER2.";
                }
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    //Email to the applicant
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Coff availing application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your C-Off avaling application for the date " + Convert.ToDateTime(Obj.StartDate).
                        ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") +
                        " has been rejected.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
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
                CTS.RA = Obj;
                CTS.AA = AA;
                CTS.ESL = ESL;
                //CTS.APA = APA;
                repo.RejectApplication(CTS);
                        //send rejected mail to the applicant.
                    }
                }
            }
        }
    }
}