﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;
using System.Web.Mvc;

namespace Attendance.BusinessLogic
{
    public class RACoffCreditApplicationBusinessLogic
    {
        public List<RACoffCreditRequestApplication> GetAppliedCoffCreditRequest(string StaffId, string AppliedBy, string userRole)
        {
            using (RACoffCreditApplicationRepository repo = new RACoffCreditApplicationRepository())
            { 
            var Obj = repo.GetAppliedCoffCreditRequest(StaffId, AppliedBy, userRole);
            return Obj;
            }
        }

        public string ValidateCoffCreditApplication(string StaffId, string FromDate, string ToDate)
        {
            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
            {
                return rACoffCreditApplicationRepository.ValidateCoffCreditApplication(StaffId, FromDate, ToDate);
            }
        }
        //C-off Avalling for Self
        public List<RACoffCreditRequestApplication> RenderAppliedCompAvailingList(string StaffId, string AppliedBy, string userRole)
        {
            using (RACoffCreditApplicationRepository repo = new RACoffCreditApplicationRepository())
            {
                var Obj = repo.RenderAppliedCompAvailingList(StaffId, AppliedBy, userRole);
            return Obj;
            }
        }
        public List<RACoffCreditRequestApplication> RenderAppliedCompAvailingListMyteam(string StaffId, string AppliedBy, string userRole)
        {
            using (RACoffCreditApplicationRepository repo = new RACoffCreditApplicationRepository())
            { 
                var Obj = repo.RenderAppliedCompAvailingListMyteam(StaffId, AppliedBy, userRole);
            return Obj;
            }
        }

        public List<RACoffCreditRequestApplication> GetAppliedCoffCreditRequestForMyTeam(string StaffId, string AppliedBy, string userRole)
        {
            using (RACoffCreditApplicationRepository repo = new RACoffCreditApplicationRepository())
            { 
                var Obj = repo.GetAppliedCoffCreditRequestForMyTeam(StaffId, AppliedBy, userRole);
            return Obj;
            }
        }

        public List<RACoffCreditRequestApplication> GetCoffCreditRequestMappedUnderMe(string StaffId)
        {
            using (RACoffCreditApplicationRepository repo = new RACoffCreditApplicationRepository())
            { 
                var Obj = repo.GetCoffCreditRequestMappedUnderMe(StaffId);
            return Obj;
            }
        }

        public List<RACoffCreditRequestApplication> GetAllCoffList(string StaffId, string AppliedBy, string userRole)
        {
            using (RACoffCreditApplicationRepository repo = new RACoffCreditApplicationRepository())
            { 
                var Obj = repo.GetAllCoffList(StaffId, AppliedBy, userRole);
            return Obj;
            }

        }

        public List<CoffReqDates> GetAllOTDates(string Staffid, string FromDate, string ToDate)
        {
            using (RACoffCreditApplicationRepository repo = new RACoffCreditApplicationRepository())
            {     
                var Result = repo.GetAllOTDates(Staffid, FromDate, ToDate);
            return Result;
            }
        }

        public string GetUniqueId()
        {
            using (var repo = new RACoffCreditApplicationRepository())
                return repo.GetUniqueId();
        }

        public int GetCompOffLapsePeriod(string LocationId, string StaffId)
        {
            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
            {
                return rACoffCreditApplicationRepository.GetCompOffLapsePeriod(LocationId, StaffId);
            }
        }


        public void SaveRequestApplication(ClassesToSave DataToSave, string SecurityGroupId, string BaseAddress)
        {
            using (RACoffCreditApplicationRepository repo = new RACoffCreditApplicationRepository())
            {
            var cm = new CommonRepository();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
            //first send acknowledgement email to the user.
            string ReportingManagerName = string.Empty;
            string StaffName = string.Empty;
            string LeaveTypeName = string.Empty;
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
                        EmailSubject = "Request for Coff Credit application sent to " + ReportingManagerName,
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + "" +
                        ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + StaffName + " has applied for a Coff Credit. Coff Credit details given below.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                        "style=\"width:50%;font-family:tahoma; font-size:9pt;\">" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\"> Worked Date:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime
                        (DataToSave.RA.StartDate).ToString("dd/MM/yyyy") + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Total Days:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.TotalDays + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.Remarks + "</td></tr>" +
                        "</table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required " +
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

                //Send intimation email to the reporting manager.
                if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = approvalOwner1EmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Request for Coff Credit application of " + StaffName,
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + "" +
                        ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + StaffName + " has applied for a Coff Credit. Coff Credit details given below.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                        "style=\"width:50%;font-family:tahoma; font-size:9pt;\">" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\"> Worked Date:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime
                        (DataToSave.RA.StartDate).ToString("dd/MM/yyyy") + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Total Days:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.TotalDays + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.Remarks + "</td></tr>" +
                        "</table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required " +
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
                DataToSave.AA.Comment = "REVIEWED THE COFF CREDIT REQUEST.";

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    //Send intimation to the applicant 
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested Coff Credit application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Your Coff Credit  application " +
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
                if (DataToSave.AA.Approval2Owner != null && string.IsNullOrEmpty(approvalOwner2EmailId)
                    .Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = approvalOwner2EmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Request for Coff Credit application of " + StaffName,
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner2Name + "" +
                        ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + ReportingManagerName + " has applied and approved for a Coff Credit. Coff Credit details given below.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                        "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr>" +
                        "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Date:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime
                        (DataToSave.RA.StartDate).ToString("dd/MM/yyyy") + "</td></tr><tr>" +
                        "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Time From:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime
                        (DataToSave.RA.StartDate).ToString("HH:mm") + "</td></tr><tr><td style=\"width:20%;" +
                        "font-family:tahoma; font-size:9pt;\">Time To:</td><td style=\"width:80%;" +
                        "font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(DataToSave.RA.EndDate)
                        .ToString("HH:mm") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma;" +
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
                if (DataToSave.AA.ApprovalOwner == DataToSave.AA.Approval2Owner)
                {
                    DataToSave.AA.ApprovalStatusId = 2;
                    DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedOn = DateTime.Now;
                }
                DataToSave.AA.Approval2statusId = 2;
                DataToSave.AA.Approval2By = DataToSave.RA.AppliedBy;
                DataToSave.AA.Approval2On = DateTime.Now;
                DataToSave.AA.Comment = "APPROVED THE COFF CREDIT REQUEST.";

                // Credit the comp-off balance to employee leave account.
                ELA.StaffId = DataToSave.RA.StaffId;
                ELA.LeaveTypeId = DataToSave.RA.LeaveTypeId;
                ELA.TransactionFlag = 1;
                ELA.TransactionDate = DateTime.Now;
                ELA.LeaveCount = DataToSave.RA.TotalDays;
                ELA.Narration = "Approved the comp-off credit application.";
                ELA.LeaveCreditDebitReasonId = 20;
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
                        EmailSubject = "Requested Comp-Off Credit application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Your Coff Credit  application" +
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

            if (SecurityGroupId.Equals("1") || SecurityGroupId.Equals("3") || SecurityGroupId.Equals("5"))
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
                    DataToSave.AA.Comment = "APPROVED THE COFF CREDIT REQUEST.";

                    // Credit the comp-off balance to employee leave account.
                    ELA.StaffId = DataToSave.RA.StaffId;
                    ELA.LeaveTypeId = DataToSave.RA.LeaveTypeId;
                    ELA.TransactionFlag = 1;
                    ELA.TransactionDate = DateTime.Now;
                    ELA.LeaveCount = DataToSave.RA.TotalDays;
                    ELA.Narration = "Approved the comp-off credit application.";
                    ELA.LeaveCreditDebitReasonId = 20;
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
                            EmailSubject = "Requested Coff Credit application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your Coff Credit  application for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).
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
            }
            repo.SaveRequestApplication(DataToSave, isFinalLevelApproval);
            
            }
        }

        public void RejectApplication(string Id, string RejectedBy)
        {
            //Get the C-Off credit application details based on the id passed to this function as a parameter.
            using (RACoffCreditApplicationRepository repo = new RACoffCreditApplicationRepository())
            { 
                var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id);
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
            //Check if the C-Off credit application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true))    //if the C-Off credit application has been cancelled then...
            {
                throw new Exception("Cancelled C-off request cannot be rejected.");
            }
            else if (Obj.IsRejected.Equals(true))  //if the C-Off credit application has been rejected then...
            {
                throw new Exception("Rejected C-off request cannot be rejected.");
            }
            else //if the C-Off credit application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {
                //reject the application.
                if (RejectedBy == approvalOwner1 && RejectedBy != approvalOwner2)
                {
                    Obj.IsRejected = true;
                    AA.ApprovalStatusId = 3;
                    AA.ApprovedBy = RejectedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "COFF CREDIT REQUEST HAS BEEN REJECTED BY THE APPROVALOWNER1.";
                }
                else if (RejectedBy == approvalOwner2)
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
                    AA.Comment = "C-OFF REQUEST HAS BEEN REJECTED BY THE APPROVALOWNER2.";
                }

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Coff credit request application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your C-Off credit application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
                        " has been rejected.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">" + rejectedByUserName +
                        " &nbsp;(" + RejectedBy + ")</p></body></html>",
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

        public void ApproveApplication(string Id, string ApprovedBy)
        {
            //Get the Coff credit application details based on the Id passed to this function as a parameter.
            using (RACoffCreditApplicationRepository repo = new RACoffCreditApplicationRepository())
            { 
                var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id);
            EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
            var cm = new CommonRepository();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            ClassesToSave CTS = new ClassesToSave();
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
            //Check if the Coff credit application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true)) //if the Coff credit application has been cancelled then...
            {
                //throw exception that a cancelled leave application cannot be approved.
                throw new Exception("Cannot approve a cancelled c-off credit application. Apply for a new c-off credit request.");
            }
            else if (Obj.IsRejected.Equals(true))
            {
                //throw exception stating that an already rejected application cannot be approved.
                throw new Exception("Cannot approve already rejected c-off credit request.");
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
                    AA.Comment = "APPROVED THE C-OFF CREDIT REQUEST.";
                    if (string.IsNullOrEmpty(approvalOwner2).Equals(false) && string.IsNullOrEmpty(approvalOwner2EmailId)
                         .Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested C-Off credit application status",
                            EmailBody = "<html><head><title></title></head><body>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner2Name + "" +
                            ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + applicantName + " has applied for a C-Off Credit. C-Off Credit details are given below.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                            "style=\"width:50%;font-family:tahoma; font-size:9pt;\">" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Obj.StaffId + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\"> Worked Date:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime
                            (Obj.StartDate).ToString("dd-MMM-yyyy") + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Credit Days:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Obj.TotalDays + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Obj.Remarks + "</td></tr>" +
                            "</table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required " +
                            "to approve or reject this application.</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
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
                    AA.Comment = "APPROVED THE C-OFF CREDIT REQUEST.";

                    //Credit Comp-Off balance into employee leave account table.
                    ELA.StaffId = Obj.StaffId;
                    ELA.LeaveTypeId = Obj.LeaveTypeId;
                    ELA.TransactionFlag = 1;
                    ELA.TransactionDate = DateTime.Now;
                    ELA.LeaveCount = Obj.TotalDays;
                    ELA.Narration = "APPROVED THE COMP-OFF CREDIT REQUEST.";
                    ELA.LeaveCreditDebitReasonId = 20;
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
                        EmailSubject = "Requested C-Off credit application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your C-Off credit application for the date " + Convert.ToDateTime(Obj.StartDate).
                        ToString("dd-MMM-yyyy") + " has been approved." +
                        "</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; " +
                        "font-size:9pt;\">" + approvedByUserName + " &nbsp;(" + ApprovedBy + ")</p></body></html>",
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
                CTS.ELA = ELA;
                CTS.AA = AA;
                CTS.ESL = ESL;
                repo.ApproveApplication(CTS, ApprovedBy, isFinalLevelApproval);
            }
        }
        }
        public string CancelApplication(string Id, string CancelledBy)
        {
            //Get the C-Off credit application details based on the Id passed to this function as a parameter.
            using (RACoffCreditApplicationRepository repo = new RACoffCreditApplicationRepository())
            { 
            ClassesToSave CTS = new ClassesToSave();
            EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
            var cm = new CommonRepository();
            var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id);
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            string approvalOwner1Name = string.Empty;
            StringBuilder INandOUTData = new StringBuilder();
            DateTime? ApplicationDate = DateTime.Now;
            string approvalOwner2Name = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string cancelledByUserEmailId = string.Empty;
            string cancelledByUserName = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string senderEmailId = string.Empty;
            bool isCompOffAvailed = false;
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
            //Check if the C-Off credit application has already been cancelled or not.
            if (Obj.IsCancelled.Equals(false))   //If the C-Off credit application has not been cancelled then...
            {
                if (Obj.StartDate != null)
                {
                    isCompOffAvailed = repo.CheckIsCompOffAvailed(Obj.StaffId, Convert.ToDateTime(Obj.StartDate));
                }
                if (isCompOffAvailed == true)
                {
                    throw new Exception("You cannot cancel the comp-off credit request because comp-off availing has been " +
                        "applied for the worked date.");
                }
                //Cancel the leave application which is in pending state.
                Obj.IsCancelled = true;
                Obj.CancelledDate = DateTime.Now;
                Obj.CancelledBy = CancelledBy;
                if (AA.Approval2statusId == 2)
                {
                    ELA.StaffId = Obj.StaffId;
                    ELA.LeaveTypeId = Obj.LeaveTypeId;
                    ELA.TransactionFlag = 2;
                    ELA.TransactionDate = DateTime.Now;
                    ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays) * -1;
                    ELA.Narration = "Cancelled the approved c-off credit application.";
                    ELA.RefId = Obj.Id;
                    ELA.LeaveCreditDebitReasonId = 21;
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
                            EmailSubject = "Requested C-Off credit application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "C-Off Credit application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
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
                    if (string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner2
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested C-Off credit application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "C-Off Credit application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
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
                    if (CancelledBy == AA.ApprovalOwner && string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false) && AA.ApprovalStatusId == 2)
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner2
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested C-Off credit application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner2Name + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "C-Off Credit application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
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
                            EmailSubject = "Requested C-Off credit application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner1Name + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "C-Off Credit application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
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

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false) && Obj.StaffId != CancelledBy)
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to User
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested C-Off credit application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your C-Off Credit application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
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
                CTS.RA = Obj;
                CTS.AA = AA;
                CTS.ELA = ELA;
                repo.CancelApplication(CTS, CancelledBy);
            }
            else //If the Coff credit application has already been cancelled then...
            {
                //throw exception.
                throw new Exception("You cannot cancel a c-off request that is already been cancelled.");
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

        public List<COffReqAvailModel> GetWorkedDatesForCompOffCreditRequest(string StaffId)
        {
            using (RACoffCreditApplicationRepository rACoffCreditApplicationRepository = new RACoffCreditApplicationRepository())
            {
                return rACoffCreditApplicationRepository.GetWorkedDatesForCompOffCreditRequest(StaffId);
            }
        }


        public List<COffReqAvailModel> GetCompOffRequestList(string StaffId)
        {
            using (RACoffCreditApplicationRepository repo = new RACoffCreditApplicationRepository())
            { 
                var Obj = repo.GetCompOffRequestList(StaffId);
            return Obj;
            }
        }

    }
}