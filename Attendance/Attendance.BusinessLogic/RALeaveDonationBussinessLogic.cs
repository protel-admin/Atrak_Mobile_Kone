using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;
using System.Configuration;

namespace Attendance.BusinessLogic
{
    public class RALeaveDonationBussinessLogic
    {
        public List<LeaveDonationDetails> GetLeaveDonationHistory(string StaffId)
        {
            using (RALeaveDonationRepository repo = new RALeaveDonationRepository())
            { 
                var Obj = repo.GetLeaveDonationHistory(StaffId);
            return Obj;
            }
        }

        public IndividualLeaveCreditDebit_EmpDetails GetEmployeeDetails(string StaffId)
        {
            using (var repo = new RALeaveDonationRepository())
            { 
                var data = repo.GetEmployeeDetails(StaffId);
            return data;
            }
        }

        public void SaveLeaveDonationDetails(IndividualLeaveCreditDebit data, string User, DocumentUpload doc, ClassesToSave CTS)
        {
            using (var repo = new RALeaveDonationRepository())
            { 
                var cm = new CommonRepository();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
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
            string HrId = string.Empty;
            string HrName = string.Empty;
            string HREmailId = string.Empty;

            applicantEmailId = cm.GetEmailIdOfEmployee(CTS.RA.StaffId);
            approvalOwner1EmailId = cm.GetEmailIdOfEmployee(CTS.AA.ApprovalOwner);
            approvalOwner2EmailId = cm.GetEmailIdOfEmployee(CTS.AA.Approval2Owner);
            appliedByUserEmailId = cm.GetEmailIdOfEmployee(CTS.RA.AppliedBy);
            commonSenderEmailId = cm.GetSenderEmailIdFromEmailSettings();
            applicantName = cm.GetStaffName(CTS.RA.StaffId);
            approvalOwner1Name = cm.GetStaffName(CTS.AA.ApprovalOwner);
            approvalOwner2Name = cm.GetStaffName(CTS.AA.Approval2Owner);
            appliedByUserName = cm.GetStaffName(CTS.RA.AppliedBy);
            HrId = cm.GetHR();
            HrName = cm.GetHRName(HrId);
            HREmailId = cm.GetEmailIdOfEmployee(HrId);

            if (CTS.RA.AppliedBy.Equals(CTS.RA.StaffId))
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
                        EmailSubject = "Request for leave donation application sent to " + HrName,
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your request for leave donation application has been sent to your HR " + HrName + "(" + HrId + ") for approval." +
                        "<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\">" +
                        "<tr><td style=\"width:20%;\">Donor Id:</td><td style=\"width:80%;\">" + CTS.RA.StaffId + "</td></tr>" +
                        "<tr><td style=\"width:20%;\">Receiver Id:</td><td style=\"width:80%;\">" + CTS.RA.ReceiverStaffId + "</td></tr>" +
                        "<tr><td style=\"width:20%;\">Total Days:</td><td style=\"width:80%;\">" + CTS.RA.TotalDays + "</td></tr>" +
                        "<tr><td style=\"width:20%;\">Reason:</td> <td style=\"width:80%;\">" + CTS.RA.Remarks + "</td></tr></table></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>",
                        CreatedOn = CTS.RA.ApplicationDate,
                        CreatedBy = CTS.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                ESL.Add(new EmailSendLog
                {
                    From = commonSenderEmailId,
                    To = HREmailId,
                    CC = string.Empty,
                    BCC = string.Empty,
                    EmailSubject = "Request for leave donation application of " + applicantName + "",
                    EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; " +
                   "font-size:9pt;\">Dear " + HrName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                   "A request for leave donation application has been received from " + applicantName + "." +
                   "<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\">" +
                    "<tr><td style=\"width:20%;\">Donar Id:</td><td style=\"width:80%;\">" + CTS.RA.StaffId + "</td></tr>" +
                    "<tr><td style=\"width:20%;\">Receiver Id:</td><td style=\"width:80%;\">" + CTS.RA.ReceiverStaffId + "</td></tr>" +
                    "<tr><td style=\"width:20%;\">Total Days:</td><td style=\"width:80%;\">" + CTS.RA.TotalDays + "</td></tr>" +
                    "<tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + CTS.RA.Remarks + "</td></tr>" +
                   "</table></p><p> Your attention is required to either approve or reject this application.</p>" +
                   "<p style=\"font-family:tahoma; font-size:9pt;\"> Best Regards,</p></body></html>",
                    CreatedOn = DateTime.Now,
                    CreatedBy = CTS.RA.AppliedBy,
                    IsSent = false,
                    SentOn = null,
                    IsError = false,
                    ErrorDescription = "-",
                    SentCounter = 0
                });
                CTS.ESL = ESL;
            }

            CTS.ESL = ESL;
            repo.SaveLeaveDonationDetails(data, User, doc, CTS);
            }
        }

        public string GetUniqueId()
        {
            using (var repo = new RALeaveDonationRepository())
                return repo.GetUniqueId();
        }

        public void ApproveLeaveDonationApplication(string Id, string ReportingManagerId, string ParentType)
        {
            using (RALeaveDonationRepository rALeaveDonationRepository = new RALeaveDonationRepository())
            { 
                CommonRepository commonRepository = new CommonRepository();
            ClassesToSave CTS = new ClassesToSave();
            List<EmployeeLeaveAccount> employeeLeaveAccounts = new List<EmployeeLeaveAccount>();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            var Obj = rALeaveDonationRepository.GetRequestApplicationDetails(Id);
            var AA = rALeaveDonationRepository.GetApplicationApproval(Id, ParentType);
            string staffId = string.Empty;
            DateTime currentDate = DateTime.Now;
            string applicationId = string.Empty;
            string applicationType = string.Empty;
            string ApproverOwner = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string senderEmailId = string.Empty;
            string appliedByUserName = string.Empty;
            string approvalOwnerEmailId = string.Empty;
            string creditLeaveTypeForLeaveDonation = string.Empty;
            ApproverOwner = commonRepository.GetHR();
            applicantName = commonRepository.GetStaffName(Obj.StaffId);
            appliedByUserName = commonRepository.GetStaffName(Obj.AppliedBy);
            commonSenderEmailId = commonRepository.GetSenderEmailIdFromEmailSettings();
            applicantEmailId = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
            approvalOwnerEmailId = commonRepository.GetEmailIdOfEmployee(AA.ApprovalOwner);
            try
            {
                creditLeaveTypeForLeaveDonation = ConfigurationManager.AppSettings["CreditLeaveTypeForLeaveDonation"].ToString();
            }
            catch
            {
                creditLeaveTypeForLeaveDonation = "LV0002";
            }
            //approve the application.
            Obj.IsApproved = true;
            AA.ApprovalStatusId = 2;
            AA.Approval2statusId = 2;
            AA.ApprovedBy = ReportingManagerId;
            AA.Approval2By = ReportingManagerId;
            AA.ApprovedOn = DateTime.Now;
            AA.Approval2On = DateTime.Now;
            AA.Comment = "Successfully Approved the LeaveDonation Request.";
            if (Obj.AppliedBy == "")
            {
                Obj.AppliedBy = Obj.StaffId;
            }
            employeeLeaveAccounts.Add(new EmployeeLeaveAccount
            {
                StaffId = Obj.StaffId,
                LeaveTypeId = "LV0002",
                LeaveCount = Obj.TotalDays * -1,
                Narration = "Debit against leave donation.",
                LeaveCreditDebitReasonId = 22,
                TransactionFlag = 2,
                TransactionDate = DateTime.Now,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year,
                RefId = Obj.Id
            });

            employeeLeaveAccounts.Add(new EmployeeLeaveAccount
            {
                StaffId = Obj.ReceiverStaffId,
                LeaveTypeId = creditLeaveTypeForLeaveDonation,
                LeaveCount = Obj.TotalDays,
                Narration = "Credit based on leave donation.",
                LeaveCreditDebitReasonId = 20,
                TransactionFlag = 1,
                TransactionDate = DateTime.Now,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year,
                RefId = Obj.Id
            });

            ESL.Add(new EmailSendLog
            {
                From = commonSenderEmailId,
                To = applicantEmailId,
                CC = string.Empty,
                BCC = string.Empty,
                EmailSubject = "Requested Leave donation status",
                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                   "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                   "Your " + Obj.TotalDays + " Privilege Leave donation for " + Obj.ReceiverStaffId +
                   " has been approved..</p> <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                   "<p style=\"font-family:tahoma; font-size:9pt;\">" + approvalOwnerEmailId + " &nbsp;" +
                   "(" + Obj.AppliedBy + ")</p></body></html>",
                CreatedOn = DateTime.Now,
                CreatedBy = Obj.AppliedBy,
                IsSent = false,
                SentOn = null,
                IsError = false,
                ErrorDescription = "-",
                SentCounter = 0
            });

            CTS.RA = Obj;
            CTS.employeeLeaveAccounts = employeeLeaveAccounts;
            CTS.AA = AA;
            CTS.ESL = ESL;
            rALeaveDonationRepository.ApproveApplication(CTS, ReportingManagerId, ParentType);
            }
        }

        public void RejectLeaveDonationApplication(string Id, string ReportingManagerId, string ParentType)
        {
            using (RALeaveDonationRepository repo = new RALeaveDonationRepository())
            { 
                var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id, ParentType);
            var cm = new CommonRepository();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            string ApproverOwner = string.Empty;
            ApproverOwner = cm.GetHR();
            string applicantName = string.Empty;
            string LeaveTypeName = string.Empty;
            string applicantEmailId = string.Empty;
            string approvedByUserEmailId = string.Empty;
            string HrName = string.Empty;
            string HrId = string.Empty;
            string commonSenderEmailId = string.Empty;
            ClassesToSave CTS = new ClassesToSave();
            applicantName = cm.GetStaffName(Obj.StaffId);
            LeaveTypeName = cm.GetLeaveName(Obj.LeaveTypeId);
            applicantEmailId = cm.GetEmailIdOfEmployee(Obj.StaffId);
            commonSenderEmailId = cm.GetSenderEmailIdFromEmailSettings();
            approvedByUserEmailId = cm.GetEmailIdOfEmployee(AA.ApprovalOwner);
            //HrName = cm.GetHRName(HrId);
            if (Obj.AppliedBy == "")
            {
                Obj.AppliedBy = Obj.StaffId;
            }
            if (Obj.IsRejected.Equals(true))
            {
                throw new Exception("Rejected leave request cannot be rejected.");
            }
            else
            {
                //reject the application.
                if (ApproverOwner == ReportingManagerId)
                {
                    Obj.IsRejected = true;
                    AA.ApprovalStatusId = 3;
                    AA.Approval2statusId = 3;
                    AA.ApprovedBy = ReportingManagerId;
                    AA.Approval2By = ReportingManagerId;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Approval2On = DateTime.Now;
                    AA.Comment = "LEAVE DONATION REQUEST HAS BEEN REJECTED BY THE HR.";
                    //email for user
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested leaveDonation application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your leave donation  application for " + Obj.ReceiverStaffId + " has been rejected.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; " +
                        "font-size:9pt;\">" + HrName + " &nbsp;(" + ReportingManagerId + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = ReportingManagerId,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });

                    CTS.RA = Obj;
                    CTS.AA = AA;
                    CTS.ESL = ESL;
                    repo.RejectApplication(CTS);
                }
            }
        }
        }
    }
}