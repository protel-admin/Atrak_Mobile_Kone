﻿using Attendance.Model;
using Attendance.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace Attendance.BusinessLogic
{
    public class RAPermissionApplicationBusinessLogic
    {
        RAPermissionApplicationRepository repo = new RAPermissionApplicationRepository();
        public List<RAPermissionApplication> GetAppliedPermissions(string StaffId)
        {
            using (RAPermissionApplicationRepository repo = new RAPermissionApplicationRepository())
            { 
                var Obj = repo.GetAppliedPermissions(StaffId);
            return Obj;
            }
        }
        public List<RAPermissionApplication> GetAppliedPermissionsForMyTeam(string StaffId, string userRole, string AppliedId)
        {
            using (RAPermissionApplicationRepository repo = new RAPermissionApplicationRepository())
            { 
                var Obj = repo.GetAppliedPermissionsForMyTeam(StaffId, userRole, AppliedId);
            return Obj;
            }
        }

        //public string ValidateEligibility(string StaffId, string ToDate, string TotalHours)
        public string ValidateEligibility(string StaffId, DateTime PermissionStartDate, string Duration, string TimeFrom, string TimeTo, DateTime TotalHours)
        {
            string str = string.Empty;
            using (RAPermissionApplicationRepository rAPermissionApplicationRepository = new RAPermissionApplicationRepository())
            {
                str = rAPermissionApplicationRepository.ValidatePermissionOffApplication(StaffId, PermissionStartDate, Duration, TimeFrom, TimeTo, TotalHours);
            }
            return str;
        }
        public List<RAPermissionApplication> GetAllEmployeesPermissionList(string StaffId, string userRole, string AppliedId)
        {
            using (RAPermissionApplicationRepository repo = new RAPermissionApplicationRepository())
            { 
                var Obj = repo.GetAllEmployeesPermissionList(StaffId, userRole, AppliedId);
            return Obj;
            }
        }

        public List<PermissionType> GetPermissionTypes()
        {
            using (RAPermissionApplicationRepository repo = new RAPermissionApplicationRepository())
            { 
                var Obj = repo.GetPermissionTypes();
            return Obj;
            }
        }
        public string GetPermissionTypeById(string PermissionTypeId)
        {
            using (RAPermissionApplicationRepository repo = new RAPermissionApplicationRepository())
            {
                var Result = repo.GetPermissionTypeById(PermissionTypeId);
            return Result;
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
            using (var repo = new RAPermissionApplicationRepository())
                return repo.GetUniqueId();
        }

        public void SaveRequestApplication(ClassesToSave DataToSave, string SecurityGroupId, string BaseAddress)
        {
            using (RAPermissionApplicationRepository repo = new RAPermissionApplicationRepository())
            { 
                var cm = new CommonRepository();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            CommonBusinessLogic CBL = new CommonBusinessLogic();
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
            bool isFinalLevelApproval = false;
            string payPeriodValidationMessage = string.Empty;

            applicantEmailId = cm.GetEmailIdOfEmployee(DataToSave.RA.StaffId);
            approvalOwner1EmailId = cm.GetEmailIdOfEmployee(DataToSave.AA.ApprovalOwner);
            approvalOwner2EmailId = cm.GetEmailIdOfEmployee(DataToSave.AA.Approval2Owner);
            appliedByUserEmailId = cm.GetEmailIdOfEmployee(DataToSave.RA.AppliedBy);
            commonSenderEmailId = cm.GetSenderEmailIdFromEmailSettings();
            applicantName = cm.GetStaffName(DataToSave.RA.StaffId);
            approvalOwner1Name = cm.GetStaffName(DataToSave.AA.ApprovalOwner);
            approvalOwner2Name = cm.GetStaffName(DataToSave.AA.Approval2Owner);
            appliedByUserName = cm.GetStaffName(DataToSave.RA.AppliedBy);

            payPeriodValidationMessage = CBL.ValidateApplicationForPayDate(DataToSave.RA.StaffId, Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy"),
                    Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy"));
            if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
            {
                if (payPeriodValidationMessage.ToUpper() != "VALID")
                {
                    throw new ApplicationException("Application of past pay cycle cannot be saved");
                }
            }
            string validationMessage = string.Empty;
            validationMessage = ValidateEligibility(DataToSave.RA.StaffId, DataToSave.RA.StartDate, DataToSave.RA.PermissionType
                , Convert.ToDateTime(DataToSave.RA.StartDate).ToString("HH:mm:ss"),
                Convert.ToDateTime(DataToSave.RA.EndDate).ToString("HH:mm:ss"), Convert.ToDateTime(DataToSave.RA.TotalHours));
            if (string.IsNullOrEmpty(validationMessage).Equals(false))
            {
                if (validationMessage.ToUpper() != "OK." && validationMessage.ToUpper() != "OK")
                {
                    throw new ApplicationException(validationMessage);
                }
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
                        EmailSubject = "Request for Permission application sent to " + approvalOwner1Name,
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + applicantName + ",<br><br>" +
                        "Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your Permission application for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).
                        ToString("dd-MMM-yyyy") + " has been sent to your reporting manager "
                        + approvalOwner1Name + " for approval.</p></body></html>",
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
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner1Name + "" +
                        ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + applicantName + " has applied for a permission. Permission details are given below.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                        "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr>" +
                        "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">StaffId:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Date:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime
                        (DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td></tr><tr>" +
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
                        "to approve or reject this application.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
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
                DataToSave.ESL = ESL;
            }

            // If the application has applied by the Approval Owner1 or Reporting Manager
            else if (DataToSave.RA.AppliedBy.Equals(DataToSave.AA.ApprovalOwner) &&
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
                        EmailSubject = "Requested permission application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Your permission  application " +
                        "for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                        " has been applied and approved by " + appliedByUserName + "" +
                        " and sent for second approval.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
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
                        EmailSubject = "Request for Permission application of " + applicantName,
                        EmailBody = "<html><head><title></title></head><body>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner2Name + "" +
                        ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + approvalOwner1Name + " has applied and approved a permission. Permission details are given below.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                        "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr>" +
                        "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">StaffId:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + DataToSave.RA.StaffId + "</td></tr>" +
                        "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Date:</td>" +
                        "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime
                        (DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</td></tr><tr>" +
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
                        "to approve or reject this application.</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "<a href=\"" + BaseAddress + "\">Redirect To Site</a></p><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
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
                DataToSave.AA.Comment = "APPROVED THE PERMISSION REQUEST.";

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested permission application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Dear " + applicantName + ",<br/><br>Greetings<br/></p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Your permission  application" +
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
            if (DataToSave.RA.AppliedBy != DataToSave.RA.StaffId && (SecurityGroupId.Equals("1") || SecurityGroupId.Equals("3") || SecurityGroupId.Equals("5")))
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
                DataToSave.AA.Comment = "APPROVED THE PERMISSION REQUEST.";

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested permission application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your permission application for the date " + Convert.ToDateTime(DataToSave.RA.StartDate).
                        ToString("dd-MMM-yyyy") + " has been applied and approved.</p><p style=\"font-family:tahoma;" +
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
        public string ValidatePermissionOffApplication(string StaffId, string ToDate, string TotalHours, string Duration, string PermissionStartTime, string PermissionEndTime)
        {
            using (RAPermissionApplicationRepository repo = new RAPermissionApplicationRepository())
            { 
                var str = repo.ValidatePermissionOffApplication(StaffId, ToDate, TotalHours, Duration, PermissionStartTime, PermissionEndTime);
            if (!str.ToUpper().StartsWith("OK"))
            {
                throw new Exception(str);
            }
            return str;
            }
        }
        public void RejectApplication(string Id, string RejectedBy)
        {
            //Get the permission application details based on the id passed to this function as a parameter.
            using (RAPermissionApplicationRepository repo = new RAPermissionApplicationRepository())
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
            string approvalOwner1Name = string.Empty;
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
            approvalOwner1Name = cm.GetStaffName(approvalOwner1);
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
            //Check if the permission application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true))    //if the permission application has been cancelled then...
            {
                throw new Exception("Cancelled permission request cannot be rejected.");
            }
            else if (Obj.IsRejected.Equals(true))  //if the permission application has been rejected then...
            {
                throw new Exception("Rejected permission request cannot be rejected.");
            }
            else //if the permission application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {
                if (RejectedBy == approvalOwner1 && RejectedBy != approvalOwner2)
                {
                    //reject the application.
                    Obj.IsRejected = true;
                    AA.ApprovalStatusId = 3;
                    AA.ApprovedBy = RejectedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "PERMISSION REQUEST HAS BEEN REJECTED BY THE APPROVAL OWNER1.";
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
                    AA.Comment = "PERMISSION REQUEST HAS BEEN REJECTED BY THE APPROVAL OWNER2.";
                }
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested permission application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your permission  application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
                        " has been rejected..</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p>" +
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
                repo.RejectApplication(CTS);
            }
            }
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
                    AA.Approval2statusId = 3;
                    AA.Approval2On = DateTime.Now;
                    AA.Approval2By = RejectedBy;
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
            else if (ApprovedBy == approvalOwner2 && Obj.IsApproved.Equals(true))
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
                    if (approvalOwner2 == approvalOwner1)
                    {
                        AA.ApprovalStatusId = 2;
                        AA.ApprovedBy = ApprovedBy;
                        AA.ApprovedOn = DateTime.Now;
                    }
                    AA.Approval2statusId = 2;
                    AA.Approval2By = ApprovedBy;
                    AA.Approval2On = DateTime.Now;
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
        public void ApproveApplication(string Id, string ApprovedBy)
        {
            //Get the permission application details based on the Id passed to this function as a parameter.
            using (RAPermissionApplicationRepository repo = new RAPermissionApplicationRepository())
            { 
                var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id);
            var cm = new CommonRepository();
            ClassesToSave CTS = new ClassesToSave();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
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
            if (Obj.IsCancelled.Equals(true)) //if the permission application has been cancelled then...
            {
                throw new Exception("Cannot approve a cancelled permission application.");
            }
            else if (Obj.IsRejected.Equals(true))
            {
                throw new Exception("Cannot approve already rejected permission request.");
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
                    AA.Comment = "APPROVED THE PERMISSION REQUEST.";
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
                            EmailSubject = "Requested permission application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Dear " + approvalOwner2Name + "" +
                            ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "" + applicantName + " has applied for a permission. Permission details are given below.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" " +
                            "style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr>" +
                            "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Employee Code:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Obj.StaffId + "</td></tr>" +
                            "<tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Date:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime
                            (Obj.StartDate).ToString("dd-MMM-yyyy") + "</td></tr><tr>" +
                            "<td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Time From:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime
                            (Obj.StartDate).ToString("HH:mm") + "</td></tr><tr><td style=\"width:20%;" +
                            "font-family:tahoma; font-size:9pt;\">Time To:</td><td style=\"width:80%;" +
                            "font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(Obj.EndDate)
                            .ToString("HH:mm") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma;" +
                            " font-size:9pt;\">Total Hours:</td><td style=\"width:80%;font-family:tahoma;" +
                            " font-size:9pt;\">" + Convert.ToDateTime(Obj.TotalHours).ToString("HH:mm") +
                            "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td>" +
                            "<td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Obj.Remarks + "</td>" +
                            "</tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required " +
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
                    AA.Comment = "APPROVED THE PERMISSION REQUEST.";
                }
                if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                    ESL.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested permission application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Your permission application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
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
                CTS.RA = Obj;
                CTS.AA = AA;
                CTS.ESL = ESL;
                repo.ApproveApplication(CTS, isFinalLevelApproval);
            }
            }
        }
        public string CancelApplication(string Id, string CancelledBy)
        {
            //Get the permission application details based on the Id passed to this function as a parameter.
            using (RAPermissionApplicationRepository repo = new RAPermissionApplicationRepository())
            { 
                ClassesToSave CTS = new ClassesToSave();
            var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id);
            var cm = new CommonRepository();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            CommonBusinessLogic CBL = new CommonBusinessLogic();
            string approvalOwner1Name = string.Empty;
            string approvalOwner2Name = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string approvalOwner1EmailId = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string cancelledByUserEmailId = string.Empty;
            string cancelledByUserName = string.Empty;
            string senderEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string payPeriodValidationMessage = string.Empty;
            approvalOwner1Name = cm.GetStaffName(AA.ApprovalOwner);
            approvalOwner1EmailId = cm.GetEmailIdOfEmployee(AA.ApprovalOwner);
            approvalOwner2Name = cm.GetStaffName(AA.Approval2Owner);
            approvalOwner2EmailId = cm.GetEmailIdOfEmployee(AA.Approval2Owner);
            applicantName = cm.GetStaffName(Obj.StaffId);
            applicantEmailId = cm.GetEmailIdOfEmployee(Obj.StaffId);
            cancelledByUserEmailId = cm.GetEmailIdOfEmployee(CancelledBy);
            cancelledByUserName = cm.GetStaffName(CancelledBy);
            commonSenderEmailId = cm.GetSenderEmailIdFromEmailSettings();
            if (Obj.AppliedBy == "")
            {
                Obj.AppliedBy = Obj.StaffId;
            }
            //Check if the permission application has already been cancelled or not.
            if (Obj.IsCancelled.Equals(false))   //If the permission application has not been cancelled then...
            {
                payPeriodValidationMessage = CBL.ValidateApplicationForPayDate(Obj.StaffId, Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy"),
                   Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy"));
                if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false))
                {
                    if (payPeriodValidationMessage.ToUpper() != "VALID")
                    {
                        throw new ApplicationException("Application of past pay cycle cannot be cancelled");
                    }
                }
                //Cancel the leave application which is in pending state.
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
                            EmailSubject = "Requested permission application status ",
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
                            EmailSubject = "Requested permission application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            " Dear " + approvalOwner2Name + ",< br />< br > " +
                            "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Permission application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).
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
                    if (CancelledBy == AA.ApprovalOwner && AA.ApprovalStatusId == 2 && string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner2
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested permission application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            " Dear " + approvalOwner2Name + ",< br />< br > " +
                            "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Permission application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).
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
                    else if (CancelledBy == AA.Approval2Owner && string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to ApprovalOwner1
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested permission application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            " Dear " + approvalOwner1Name + ",< br />< br > " +
                            "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Permission application " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).
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
                            EmailSubject = "Requested permission application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:" +
                            "tahoma; font-size:9pt;\">Dear " + applicantName + ",<br/><br>" +
                            "Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Permission application of " + applicantName + " for the date " + Convert.ToDateTime(Obj.StartDate).
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
                repo.CancelApplication(CTS, CancelledBy);
            }
            else //If the permission application has already been cancelled then...
            {
                throw new Exception("You cannot cancel a permission request that is already been cancelled.");
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
        #region Common Permission
        public string BulkSaveCommonPermissionBusinessLogic(CommonPermissionModel model, string StaffList, string CreatedBy)
        {
            using (RAPermissionApplicationRepository repo = new RAPermissionApplicationRepository())
                return repo.BulkSaveCommonPermissionRepository(model, StaffList, CreatedBy);
        }
        #endregion
    }
}
