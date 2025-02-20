﻿using System;
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
    public class RALeaveApplicationBusinessLogic
    {
        public List<RALeaveApplication> GetAppliedLeaves(string StaffId)
        {
            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {
             
                return rALeaveApplicationRepository.GetAppliedLeaves(StaffId);         
            }
        }

        public List<RALeaveApplication> GetAppliedLeavesForMyTeam(string StaffId, string AppliedId, string Role)
        {
            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {
                return rALeaveApplicationRepository.GetAppliedLeavesForMyTeam(StaffId, AppliedId, Role);
            }
        }

        public List<LeaveReasonList> GetLeaveReasonList(string user)
        {
            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {
                return rALeaveApplicationRepository.GetLeaveReasonList(user);
            }
        }

        public List<RALeaveApplication> GetApprovedLeavesForMyTeam(string StaffId)
        {
            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {
                return rALeaveApplicationRepository.GetApprovedLeavesForMyTeam(StaffId);
            }
        }

        public List<LeaveReasonList> GetLeaveTypes(string StaffId)
        {
            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {
                return rALeaveApplicationRepository.GetLeaveTypes(StaffId);
            }
        }
        public List<SelectListItem> ConvertLeaveTypesToListItems(List<LeaveReasonList> objListOfLeaveTypes)
        {
            List<SelectListItem> _ListOfLeaveTypes_ = new List<SelectListItem>();
            foreach (var l in objListOfLeaveTypes)
            {
                _ListOfLeaveTypes_.Add(new SelectListItem
                {
                    Value = l.Id,
                    Text = l.Name
                });
            }

            return _ListOfLeaveTypes_;
        }

        public List<LeaveDuration> GetLeaveDurations()
        {
            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {
                return rALeaveApplicationRepository.GetLeaveDurations();
            }
        }

        public List<SelectListItem> ConvertLeaveDurationsToListItems(List<LeaveDuration> objListOfLeaveDurations)
        {
            List<SelectListItem> _ListOfLeaveDurations_ = new List<SelectListItem>();
            foreach (var l in objListOfLeaveDurations)
            {
                _ListOfLeaveDurations_.Add(new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                });
            }

            return _ListOfLeaveDurations_;
        }

        public string GetTotalDaysLeave(string StaffId, string LeaveStartDurationId, string FromDate, string ToDate, string LeaveEndDurationId, string LeaveType)
        {
            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {
                return rALeaveApplicationRepository.GetTotalDaysLeave(StaffId, LeaveStartDurationId, FromDate, ToDate, LeaveEndDurationId, LeaveType);
            }
        }

        public string GetUniqueId()
        {
            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {

                return rALeaveApplicationRepository.GetUniqueId();
            }
        }

        public List<DropDownListString> GetAllStaffForWorkAllocation(string StaffId)
        {
            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {

                return rALeaveApplicationRepository.GetAllStaffForWorkAllocation(StaffId);
            }
        }

        public void SaveRequestApplication(ClassesToSave DataToSave, string SecurityGroupId, string LocationId)
        {
            
            
            List<EmailSendLog> emailSendLogs = new List<EmailSendLog>();
           
                string overlappingValidationMessage = string.Empty;
                string payPeriodValidationMessage = string.Empty;
                try
                {
                    using (CommonRepository commonRepository = new CommonRepository())
                    {
                        overlappingValidationMessage = commonRepository.ValidateApplicationOverlaping(DataToSave.RA.StaffId, DataToSave.RA.StartDate,
                       DataToSave.RA.LeaveStartDurationId, DataToSave.RA.EndDate, DataToSave.RA.LeaveEndDurationId);
                    }
               

                    if (string.IsNullOrEmpty(overlappingValidationMessage).Equals(false) && overlappingValidationMessage != "Ok")
                    {
                        throw new ApplicationException(overlappingValidationMessage);
                    }
                }
                catch (Exception e)
                {
                    throw new ApplicationException("1-" + e.Message);
                }

                try
                {
                    using (CommonRepository commonRepository = new CommonRepository())
                    {
                        payPeriodValidationMessage = commonRepository.ValidateApplicationForPayDate(DataToSave.RA.StartDate, DataToSave.RA.EndDate);
                    }
                    if (string.IsNullOrEmpty(payPeriodValidationMessage).Equals(false) && payPeriodValidationMessage != "VALID")
                    {
                        throw new ApplicationException("Application of past pay cycle cannot be saved ");
                    }
                }catch(Exception e)
                {
                    throw new ApplicationException("2-" + e.Message);
                }

                try
                {
                    //Validate leave balance.
                    if (DataToSave.RA.LeaveTypeId != "LV0036" && DataToSave.RA.LeaveTypeId != "LV0038" && DataToSave.RA.LeaveTypeId != "LV0039")
                    {
                        ValidateLeaveBalance(DataToSave.RA.StaffId, DataToSave.RA.LeaveTypeId, Convert.ToDecimal(DataToSave.RA.TotalDays));
                    }
                    //Validate Leave Application
                    ValidateLeaveApplication(DataToSave.RA.StaffId, DataToSave.RA.StartDate, DataToSave.RA.LeaveStartDurationId
                        , DataToSave.RA.EndDate, DataToSave.RA.LeaveEndDurationId, DataToSave.RA.LeaveTypeId, Convert.ToDecimal(DataToSave.RA.TotalDays));
                }catch( Exception e)
                {
                    throw new ApplicationException("3 -" + e.Message);
                }
            try
            {
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
                string alternatePersonName = string.Empty;
                string alternatePersonEmailId = string.Empty;
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
                    alternatePersonName = commonRepository.GetStaffName(DataToSave.APA.AlternativeStaffId);
                    alternatePersonEmailId = commonRepository.GetEmailIdOfEmployee(DataToSave.APA.AlternativeStaffId);
                    commonSenderEmailId = commonRepository.GetCommonSenderEmailIdFromEmailSettings();
                    LeaveTypeName = commonRepository.GetLeaveName(DataToSave.RA.LeaveTypeId);
                    ccEmailAddress = commonRepository.GetCCAddress("LEAVEAPPLICATION", LocationId);
                    
                }

            BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();


                if (DataToSave.RA.AppliedBy == DataToSave.RA.StaffId)
            {
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false)) //apply for user
                {
                        //first send acknowledgement email to the user.
                        emailSendLogs.Add(new EmailSendLog
                    {  
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = string.Empty,
                        BCC = string.Empty,
                            EmailSubject = "Request for leave application sent to " + approvalOwner1Name,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your request for leave application from the date from " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "" +
                            " to " + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + " has been sent to your " +
                            "reporting manager " + approvalOwner1Name + " for approval.</p>" +
                            "<p style=\"font-family:tahoma;" + " font-size:9pt;\">Best Regards,</p></body></html>",
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
                        emailSendLogs.Add(new EmailSendLog
                    {
                            //From = model.UserEmailId ?? string.Empty,
                            From = commonSenderEmailId,
                            To = approvalOwner1EmailId,
                            CC = string.Empty,
                        BCC = string.Empty,
                            EmailSubject = "Request for leave application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner1Name + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "A request for leave application has been received from " + applicantName + ". Details are given below</p>" +
                            "<p><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">" +
                            "StaffId:</td><td style=\"width:80%;\">" + DataToSave.RA.StaffId + "</td></tr><tr><td style=\"width:20%;\">" +
                            "From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(DataToSave.RA.StartDate)
                            .ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">" + "To Date:</td><td style=\"width:80%;\">" +
                            "" + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + "</td></tr><tr>" +
                            "<td style=\"width:20%;\">Total Days:</td><td style=\"width:80%;\">" + DataToSave.RA.TotalDays + "</td></tr>" +
                            "<tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + DataToSave.RA.Remarks + "</td></tr></table></p>" +
                            "<p>Your attention is required to either approve or reject this application.</p><a href=\"" + BaseAddress + "\">" +
                            "Click Here</a><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>",
                        CreatedOn = DateTime.Now,
                            CreatedBy = DataToSave.RA.StaffId,
                        IsSent = false,
                            SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                    }
                        //send intimation email to the AlternativeStaff.
                    if (string.IsNullOrEmpty(alternatePersonEmailId).Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = alternatePersonEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Work allocation for leave application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + alternatePersonName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "A request for work allocation application has been received from " + applicantName + " for the date from "
                            + DataToSave.RA.StartDate + " to " + DataToSave.RA.EndDate + ".<p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Best Regards,</p></body></html>",
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

            //If the application has applied by ApprovalOwner 
                else if (DataToSave.RA.AppliedBy.Equals(DataToSave.AA.ApprovalOwner) &&
            DataToSave.RA.AppliedBy != DataToSave.AA.ReviewerOwner) 
            {
                    //approve the application.
                    DataToSave.RA.IsApproved = true;
                    DataToSave.RA.IsReviewed = false;
                    DataToSave.AA.ApprovalStatusId = 2;
                    DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedOn = DateTime.Now;
                    DataToSave.AA.ReviewerstatusId = 1;
                    DataToSave.AA.ReviewedBy = null;
                    DataToSave.AA.ReviewedOn = null;
                    DataToSave.AA.Comment = "APPROVED THE LEAVE REQUEST.";

                    //send intimation email to the User.
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                {
                        emailSendLogs.Add(new EmailSendLog
                {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = ccEmailAddress,
                    BCC = string.Empty,
                    EmailSubject = "Requested leave application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your " + LeaveTypeName + "  application for the date from " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                            " to " + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + " has been applied & approved.</p>" +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
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
                    if (string.IsNullOrEmpty(DataToSave.APA.AlternativeStaffId).Equals(false) &&
                        string.IsNullOrEmpty(alternatePersonEmailId).Equals(false))
                {
                    //send intimation email to the AlternativeStaff.
                        emailSendLogs.Add(new EmailSendLog
                    {
                            From = commonSenderEmailId,
                            To = alternatePersonEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                            EmailSubject = "Work allocation for leave application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + alternatePersonName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "A request for work allocation application has been assigned for " + applicantName + " on application for the date from" +
                            " " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(DataToSave.RA.EndDate)
                            .ToString("dd-MMM-yyyy") + " has been applied & approved <p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + appliedByUserName + " &nbsp;" +
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

                    if (string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                    {
                        emailSendLogs.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Request for leave application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner2Name + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "A request for leave application of " + applicantName + " has been applied and approved by " + appliedByUserName + " . Details are given below</p>" +
                            "<p><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">" +
                            "StaffId:</td><td style=\"width:80%;\">" + DataToSave.RA.StaffId + "</td></tr><tr><td style=\"width:20%;\">" +
                            "From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(DataToSave.RA.StartDate)
                            .ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">" + "To Date:</td><td style=\"width:80%;\">" +
                            "" + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + "</td></tr><tr>" +
                            "<td style=\"width:20%;\">Total Days:</td><td style=\"width:80%;\">" + DataToSave.RA.TotalDays + "</td></tr>" +
                            "<tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + DataToSave.RA.Remarks + "</td></tr></table></p>" +
                            "<p>Your attention is required to either approve or reject this application.</p><a href=\"" + BaseAddress + "\">" +
                            "Click Here</a><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = DataToSave.RA.StaffId,
                            IsSent = false,
                            SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }

                    DataToSave.ESL = emailSendLogs;
            }

            // If the Reviewer has applied the application then
            else if (DataToSave.RA.AppliedBy.Equals(DataToSave.AA.ReviewerOwner)) 
            {
                isFinalLevelApproval = true;
                    //approve the application.
                    DataToSave.RA.IsApproved = true;
                    DataToSave.RA.IsReviewed = true;
                    DataToSave.AA.ApprovalStatusId = 2;
                    DataToSave.AA.ReviewerstatusId = 2;
                    DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedOn = DateTime.Now;
                    DataToSave.AA.ReviewedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ReviewedOn = DateTime.Now;
                    DataToSave.AA.Comment = "REVIEWER APPROVED THE LEAVE REQUEST.";
                    
                    //deduct leave balance from employee leave account table.
                    EmployeeLeaveAccount employeeLeaveAccount = new EmployeeLeaveAccount();
                    employeeLeaveAccount.StaffId = DataToSave.RA.StaffId;
                    employeeLeaveAccount.LeaveTypeId = DataToSave.RA.LeaveTypeId;
                    employeeLeaveAccount.TransactionFlag = 2;
                    employeeLeaveAccount.TransactionDate = DateTime.Now;
                    employeeLeaveAccount.LeaveCount = Convert.ToDecimal(DataToSave.RA.TotalDays) * -1;
                    employeeLeaveAccount.Narration = "Approved the leave application.";
                    employeeLeaveAccount.LeaveCreditDebitReasonId = 22;
                    employeeLeaveAccount.RefId = DataToSave.RA.Id;
                    DataToSave.ELA = employeeLeaveAccount;

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                //send intimation email to the User.
                        emailSendLogs.Add(new EmailSendLog
                {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                    CC = string.Empty,
                    BCC = string.Empty,
                    EmailSubject = "Requested leave application status ",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your " + LeaveTypeName + "  application for the date from " + Convert.ToDateTime(DataToSave.RA.StartDate).
                            ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + "" +
                            " has been applied and approved</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">" + appliedByUserName + " &nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>",
                    CreatedOn = DateTime.Now,
                    CreatedBy = DataToSave.RA.AppliedBy,
                    IsSent = false,
                            SentOn = null,
                    IsError = false,
                    ErrorDescription = "-",
                    SentCounter = 0
                });
                    }
                    if (string.IsNullOrEmpty(DataToSave.APA.AlternativeStaffId).Equals(false) && string.IsNullOrEmpty(alternatePersonEmailId).Equals(false))
                {
                    //send intimation email to the AlternativeStaff.
                        emailSendLogs.Add(new EmailSendLog
                    {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                            EmailSubject = "Work allocation for leave application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + alternatePersonName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "A request for work allocation application has been assigned for " + applicantName + " on application for the date from " +
                            "" + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(DataToSave.RA.EndDate)
                            .ToString("dd-MMM-yyyy") + " has been approved<p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + appliedByUserName + " &nbsp;" +
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
                    DataToSave.ESL = emailSendLogs;
                }

                else if (DataToSave.RA.AppliedBy != DataToSave.RA.StaffId && DataToSave.RA.AppliedBy != DataToSave.AA.ApprovalOwner
                   && DataToSave.RA.AppliedBy != DataToSave.AA.ReviewerOwner && SecurityGroupId == "6" || SecurityGroupId == "3" || SecurityGroupId == "5")
                {
                    isFinalLevelApproval = true;
                    //Check if the leave balance is more than the total days of leave requested.
                    //approve the application.
                    DataToSave.RA.IsApproved = true;
                    DataToSave.RA.IsReviewed = true;
                    DataToSave.AA.ApprovalStatusId = 2;
                    DataToSave.AA.ReviewerstatusId = 2;
                    DataToSave.AA.ReviewedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                    DataToSave.AA.ApprovedOn = DateTime.Now;
                    DataToSave.AA.ReviewedOn = DateTime.Now;
                    DataToSave.AA.Comment = "APPROVED THE LEAVE REQUEST.";

                    //deduct leave balance from employee leave account table.
                    EmployeeLeaveAccount employeeLeaveAccount = new EmployeeLeaveAccount();
                    employeeLeaveAccount.StaffId = DataToSave.RA.StaffId;
                    employeeLeaveAccount.LeaveTypeId = DataToSave.RA.LeaveTypeId;
                    employeeLeaveAccount.TransactionFlag = 2;
                    employeeLeaveAccount.TransactionDate = DateTime.Now;
                    employeeLeaveAccount.LeaveCount = Convert.ToDecimal(DataToSave.RA.TotalDays) * -1;
                    employeeLeaveAccount.Narration = "Approved the leave application.";
                    employeeLeaveAccount.LeaveCreditDebitReasonId = 22;
                    employeeLeaveAccount.RefId = DataToSave.RA.Id;
                    DataToSave.ELA = employeeLeaveAccount;

                   

                    //send intimation email to the User manager.
                    emailSendLogs.Add(new EmailSendLog
                    {
                        From = commonSenderEmailId,
                        To = applicantEmailId,

                        CC = ccEmailAddress,
                        BCC = string.Empty,
                        EmailSubject = "Requested leave application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your " + LeaveTypeName + "  application for the date from" + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") +
                        " to " + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + " has been applied & approved.</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "" + appliedByUserName + " &nbsp;(" + DataToSave.RA.AppliedBy + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = DataToSave.RA.AppliedBy,
                        IsSent = false,
                        SentOn = null,
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                    if (DataToSave.APA.AlternativeStaffId != null)
                {
                        //send intimation email to the AlternativeStaff.
                        emailSendLogs.Add(new EmailSendLog
                    {
                            From = commonSenderEmailId,
                            To = alternatePersonEmailId,
                        CC = string.Empty,
                        BCC = string.Empty,
                            EmailSubject = "Work Allocation for leave application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + alternatePersonName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "A request for work allocation application has been assigned for " + applicantName + " on application for" +
                            " the date from " + Convert.ToDateTime(DataToSave.RA.StartDate).ToString("dd-MMM-yyyy") + " " +
                            "to " + Convert.ToDateTime(DataToSave.RA.EndDate).ToString("dd-MMM-yyyy") + " has been applied & approved " +
                            "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
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
                    DataToSave.ESL = emailSendLogs;
        }
        using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
        {
                    rALeaveApplicationRepository.SaveRequestApplication(DataToSave, SecurityGroupId, isFinalLevelApproval);
        }
        }
        catch (Exception err)
        {       
                throw new ApplicationException("4 - "+ err.Message + "--'"+err.StackTrace);
        }
        }
        public void SaveDocumentInformation(DocumentUpload LawDocument)
        {
            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {
                rALeaveApplicationRepository.SaveDocumentInformation(LawDocument);
            }
        }

     
        public string ValidateLeaveApplication(string StaffId, DateTime Startdate, int LeaveStartDurationId, DateTime EndDate,
            int LeaveEndDurationId, string LeaveTypeId, decimal TotalDays)
        {
            string str = string.Empty;
            
            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {

                str = rALeaveApplicationRepository.ValidateLeaveApplication(StaffId, Startdate, LeaveStartDurationId, EndDate, LeaveEndDurationId, LeaveTypeId, TotalDays);
        }
            if (!str.ToUpper().StartsWith("OK."))
            {
                throw new ApplicationException(str);
            }

            return str;
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

        public void RejectApplication(string Id, string RejectedBy, string LocationId)
        {
            //Get the leave application details based on the id passed to this function as a parameter.
            RequestApplication Obj = new RequestApplication();
            ApplicationApproval AA = new ApplicationApproval();
            AlternativePersonAssign APA = new AlternativePersonAssign();

            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {
                 Obj = rALeaveApplicationRepository.GetRequestApplicationDetails(Id);
                 AA = rALeaveApplicationRepository.GetApplicationApproval(Id);
                 APA = rALeaveApplicationRepository.GetApplicationForWorkAllocation(Id);
            }
         
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            ClassesToSave CTS = new ClassesToSave();
            //first send acknowledgement email to the user.
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner1Name = string.Empty;
            string approvalOwner2Name = string.Empty;
            string rejectedByUserName = string.Empty;
            string rejectedByUserEmailId = string.Empty;
            string alternatePersonName = string.Empty;
            string alternatePersonEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string ccEmailAddress = string.Empty;
            string LeaveTypeName = string.Empty;
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
                LeaveTypeName = commonRepository.GetLeaveName(Obj.LeaveTypeId);
                ccEmailAddress = commonRepository.GetCCAddress("LEAVEAPPLICATION", LocationId);
            }
            if (APA != null && string.IsNullOrEmpty(APA.AlternativeStaffId).Equals(false))
            {
                using (CommonRepository commonRepository = new CommonRepository())
                {
                    alternatePersonName = commonRepository.GetStaffName(APA.AlternativeStaffId);
                    alternatePersonEmailId = commonRepository.GetEmailIdOfEmployee(APA.AlternativeStaffId);
                }
            }

            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true))    //if the leave application has been cancelled then...
            {
                throw new Exception("Cancelled leave request cannot be rejected.");
            }
            else if (Obj.IsApproved.Equals(true) && RejectedBy == approvalOwner1) //if the leave application has been approved then...
            {
                throw new Exception("Approved leave request cannot be rejected.");
            }
            else if (Obj.IsReviewed.Equals(true)) //if the leave application has been approved then...
            {
                throw new Exception("Approved leave request cannot be rejected.");
            }
            else if (Obj.IsRejected.Equals(true))  //if the leave application has been rejected then...
            {
                throw new Exception("Rejected leave request cannot be rejected.");
            }
            else //if the leave application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {

                if (RejectedBy == approvalOwner1 && RejectedBy != approvalOwner2)
                {
                    Obj.IsRejected = true;

                    AA.ApprovalStatusId = 3;
                    AA.ApprovedBy = RejectedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Comment = "LEAVE REQUEST HAS BEEN REJECTED BY THE REPORTING MANAGER.";
                    //email for user
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                    ESL.Add(new EmailSendLog
                    {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = ccEmailAddress,
                        BCC = string.Empty,
                        EmailSubject = "Requested leave application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your " + LeaveTypeName + "  application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy")
                            + " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + " has been rejected.</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + rejectedByUserName + " &nbsp;" +
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
                    // email AlternativeStaff
                    if (APA != null)
                    {
                        APA.IsRejected = true;
                        APA.RejectMailSent = true;
                        if (string.IsNullOrEmpty(alternatePersonEmailId).Equals(false))
                        {
                        ESL.Add(new EmailSendLog
                        {
                                From = commonSenderEmailId,
                                To = alternatePersonEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                                EmailSubject = "Work Allocation for leave application of " + applicantName,
                                EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "Dear " + alternatePersonName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                "A request for work allocation application has been assigned for " + applicantName + " on application for the date "
                                + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate).
                                ToString("dd-MMM-yyyy") + " has been Rejected. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                                "<p style=\"font-family:tahoma; font-size:9pt;\">" + rejectedByUserName + " &nbsp;(" + RejectedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                                CreatedBy = RejectedBy,
                            IsSent = false,
                                SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }
                }
                }
                //reject the application.
                if (RejectedBy == approvalOwner2)
                {
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
                    AA.Comment = "LEAVE REQUEST HAS BEEN REJECTED BY THE REPORTING MANAGER.";
                    //email for user
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                    ESL.Add(new EmailSendLog
                    {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = ccEmailAddress,
                        BCC = string.Empty,
                        EmailSubject = "Requested leave application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your " + LeaveTypeName + "  application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy")
                            + " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + " has been rejected.</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + rejectedByUserName + " &nbsp;" +
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
                    // email AlternativeStaff
                    if (APA != null)
                    {
                        APA.IsRejected = true;
                        APA.RejectMailSent = true;

                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = alternatePersonEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Work Allocation for leave application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + alternatePersonName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "A request for work allocation application has been assigned for " + applicantName + " on application for the date" +
                            " " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate).
                            ToString("dd-MMM-yyyy") + " has been Rejected. <p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + rejectedByUserName + " &nbsp;(" + RejectedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = RejectedBy,
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
                    CTS.APA = APA;
                using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
                {
                    rALeaveApplicationRepository.RejectApplication(CTS);
                }
            }
        }

        public void ApproveApplication(string Id, string ApprovedBy, string LocationId)
        {
            //Get the leave application details based on the Id passed to this function as a parameter.
            RequestApplication Obj = new RequestApplication();
            ApplicationApproval AA = new ApplicationApproval();
            AlternativePersonAssign APA = new AlternativePersonAssign();
     
            EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            ClassesToSave CTS = new ClassesToSave();

            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
            {
                Obj = rALeaveApplicationRepository.GetRequestApplicationDetails(Id);
                AA = rALeaveApplicationRepository.GetApplicationApproval(Id);
                APA = rALeaveApplicationRepository.GetApplicationForWorkAllocation(Id);
            }

            string staffId = string.Empty;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            string applicationId = string.Empty;
            string applicationType = string.Empty;
            string BaseAddress = string.Empty;
            string applicantName = string.Empty;
            string applicantEmailId = string.Empty;
            string approvalOwner1 = string.Empty;
            string approvalOwner2 = string.Empty;
            string approvalOwner1Name = string.Empty;
            string approvalOwner2Name = string.Empty;
            string approvalOwner2EmailId = string.Empty;
            string approvedByUserName = string.Empty;
            string approvedByUserEmailId = string.Empty;
            string alternatePersonName = string.Empty;
            string alternatePersonEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string ccEmailAddress = string.Empty;
            string LeaveTypeName = string.Empty;
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
                LeaveTypeName = commonRepository.GetLeaveName(Obj.LeaveTypeId);
                ccEmailAddress = commonRepository.GetCCAddress("LEAVEAPPLICATION", LocationId);
            }
            BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();

            if (APA != null && string.IsNullOrEmpty(APA.AlternativeStaffId).Equals(false))
            {
                using (CommonRepository commonRepository = new CommonRepository())
                {
                    alternatePersonName = commonRepository.GetStaffName(APA.AlternativeStaffId);
                    alternatePersonEmailId = commonRepository.GetEmailIdOfEmployee(APA.AlternativeStaffId);
                }
            }
         
            if (ApprovedBy == approvalOwner1)
            {
                if (Obj.IsApproved.Equals(true)) //if application has already been approved then...
                {
                    throw new Exception("Cannot approve already approved leave request.");
                }
            }
            else if (ApprovedBy == approvalOwner2)
            {
                if (Obj.IsReviewed.Equals(true)) //if application has already been approved then...
                {
                    throw new Exception("Cannot approve already approved leave request.");
                }
            }

            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true)) //if the leave application has been cancelled then...
            {
                throw new Exception("Cannot approve a cancelled leave application.");
            }
            else if (Obj.IsRejected.Equals(true))
            {
                throw new Exception("Cannot approve already rejected leave request.");
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
                        AA.Comment = "APPROVED THE LEAVE REQUEST.";
                    ELA = null;

                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                        {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = ccEmailAddress,
                            BCC = string.Empty,
                            EmailSubject = "Requested leave application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your " + LeaveTypeName + "  application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
                            " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + " has been approved.</p><p style=\"font-family:tahoma; " +
                            "font-size:9pt;\"><a href=\"" + BaseAddress + "\">Click Here</a></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + approvedByUserName + " &nbsp;(" + ApprovedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = ApprovedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                        }
                    if (string.IsNullOrEmpty(approvalOwner2EmailId).Equals(false))
                        {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = approvalOwner2EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Request for leave application of " + applicantName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + approvalOwner2Name + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "A request for leave application of " + applicantName + " has been approved by " + approvedByUserName + "" +
                            " . Details are given below</p>" +
                            "<p><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">" +
                            "StaffId:</td><td style=\"width:80%;\">" + Obj.StaffId + "</td></tr><tr><td style=\"width:20%;\">" +
                            "From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(Obj.StartDate)
                            .ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">" + "To Date:</td><td style=\"width:80%;\">" +
                            "" + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + "</td></tr><tr>" +
                            "<td style=\"width:20%;\">Total Days:</td><td style=\"width:80%;\">" + Obj.TotalDays + "</td></tr>" +
                            "<tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + Obj.Remarks + "</td></tr></table></p>" +
                            "<p>Your attention is required to either approve or reject this application.</p><a href=\"" + BaseAddress + "\">" +
                            "Click Here</a><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = Obj.StaffId,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }

                        if (APA != null)
                        {
                            if (APA.AlternativeStaffId != null)
                            {
                                APA.IsApproved = true;
                                APA.ConfirmationMailSent = true;
                            if (string.IsNullOrEmpty(alternatePersonEmailId).Equals(false))
                            {
                                ESL.Add(new EmailSendLog
                                {
                                    From = commonSenderEmailId,
                                    To = alternatePersonEmailId,
                                    CC = string.Empty,
                                    BCC = string.Empty,
                                    EmailSubject = "Work Allocation for leave application of " + applicantName,
                                    EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                    "Dear " + alternatePersonName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                                    "A request for work allocation application has been assigned instead of " + applicantName + " for the date of " +
                                    "" + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate)
                                    .ToString("dd-MMM-yyyy") + " has been Approved. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                                    "<p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">Click Here</a></p>" +
                                    "<p style=\"font-family:tahoma; font-size:9pt;\">" + approvedByUserName + " &nbsp;(" + ApprovedBy + ")</p></body></html>",
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
                    }
                    }

                else if (ApprovedBy == approvalOwner2)
                    {
                        //approve the application.
                        Obj.IsApproved = true;
                    Obj.IsReviewed = true;
                    if (approvalOwner1 == approvalOwner2)
                    {
                        AA.ApprovalStatusId = 2;
                        AA.ApprovedBy = ApprovedBy;
                        AA.ApprovedOn = DateTime.Now;
                    }
                    AA.ReviewerstatusId = 2;
                    AA.ReviewedOn = DateTime.Now;
                    AA.ReviewedBy = ApprovedBy;
                        AA.Comment = "APPROVED THE LEAVE REQUEST.";

                    if (Obj.LeaveTypeId != "LV0039")// LV0039-> BUSINESS TRAVEL 
                        {
                        //Deduct leave balance from employee leave account table.
                            ELA.StaffId = Obj.StaffId;
                            ELA.LeaveTypeId = Obj.LeaveTypeId;
                            ELA.TransactionFlag = 2;
                            ELA.TransactionDate = DateTime.Now;
                            ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays) * -1;
                            ELA.Narration = "Approved the leave application.";
                            ELA.LeaveCreditDebitReasonId = 22;
                            ELA.RefId = Obj.Id;
                        }
                        else
                        {
                            ELA = null;
                        }
                    if (string.IsNullOrEmpty(applicantEmailId).Equals(false))
                    {
                        ESL.Add(new EmailSendLog
                        {
                            From = commonSenderEmailId,
                            To = applicantEmailId,
                            CC = ccEmailAddress,
                            BCC = string.Empty,
                            EmailSubject = "Requested leave application status",
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Dear " + applicantName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                            "Your " + LeaveTypeName + "  application for the date " + Convert.ToDateTime(Obj.StartDate).
                            ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + " " +
                            " has been Approved.</p><p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">" +
                            "Click Here</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma;" +
                            " font-size:9pt;\">" + approvedByUserName + " &nbsp;(" + ApprovedBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = ApprovedBy,
                            IsSent = false,
                            SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                        if (APA != null)
                        {
                            if (APA.AlternativeStaffId != null)
                            {
                                APA.IsApproved = true;
                                APA.ConfirmationMailSent = true;
                                if (string.IsNullOrEmpty(alternatePersonEmailId).Equals(false))
                                {
                                ESL.Add(new EmailSendLog
                                {
                                        From = commonSenderEmailId,
                                        To = alternatePersonEmailId,
                                    CC = string.Empty,
                                    BCC = string.Empty,
                                        EmailSubject = "Work allocation for leave application of " + applicantName,
                                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma;" +
                                        " font-size:9pt;\">Dear " + alternatePersonName + ",<br>Greetings</p>" +
                                        "<p style=\"font-family:tahoma; font-size:9pt;\">A request for work allocation application " +
                                        "has been assigned instead of " + applicantName + " for the date of " +
                                        Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " to " +
                                        Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + " has been approved. " +
                                        "<p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; " +
                                        "font-size:9pt;\"><a href=\"" + BaseAddress + "\">Click Here</a></p><p style=\"font-family:tahoma;" +
                                        " font-size:9pt;\">" + approvedByUserName + " &nbsp;(" + ApprovedBy + ")</p></body></html>",
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
                            }
                            }
                        }
                        CTS.RA = Obj;
                        CTS.ELA = ELA;
                        CTS.AA = AA;
                        CTS.ESL = ESL;
                        CTS.APA = APA;
                try
                {
                    using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
                    {
                        // rALeaveApplicationRepository.ApproveApplication(CTS);
                        rALeaveApplicationRepository.ApproveApplication(CTS);
                    }
                }catch(Exception ex)
                {
                    throw new ApplicationException("<<RALEAVE>> " +ex);
                }
            }
        }

        public string CancelApplication(string Id, string CancelledBy, string LocationId)
        {
            //Get the leave application details based on the Id passed to this function as a parameter.

            ClassesToSave CTS = new ClassesToSave();
            RequestApplication Obj = new RequestApplication();
            ApplicationApproval AA = new ApplicationApproval();
            AlternativePersonAssign APA = new AlternativePersonAssign();

            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
                {

                Obj = rALeaveApplicationRepository.GetRequestApplicationDetails(Id);
                AA = rALeaveApplicationRepository.GetApplicationApproval(Id);
                APA = rALeaveApplicationRepository.GetApplicationForWorkAllocation(Id);
        }
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
            var CBL = new CommonBusinessLogic();
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
            string alternatePersonName = string.Empty;
            string alternatePersonEmailId = string.Empty;
            string commonSenderEmailId = string.Empty;
            string LeaveType = string.Empty;
            string ReviewerName = string.Empty;
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
                LeaveType = commonRepository.GetLeaveName(Obj.LeaveTypeId);
            }
            payPeriodValidationMessage = CBL.ValidateApplicationForPayDate(Obj.StartDate, Obj.EndDate);
            if (payPeriodValidationMessage.ToUpper() != "VALID")
            {
                throw new Exception("Application of past pay cycle cannot be cancelled");
            }
            if (APA != null && string.IsNullOrEmpty(APA.AlternativeStaffId).Equals(false))
            {
                using (CommonRepository commonRepository = new CommonRepository())
            {
                    alternatePersonName = commonRepository.GetStaffName(APA.AlternativeStaffId);
                }
            }
                //Check if the leave application has already been cancelled or not.
            if (Obj.IsCancelled.Equals(false) && Obj.IsApproverCancelled.Equals(false) && Obj.IsReviewerCancelled.Equals(false))
            //If the leave application has not been cancelled then...
                {
                    //Cancel the leave application which is in pending state.
                if (CancelledBy == Obj.StaffId)
                    {
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;
                    Obj.CancelledBy = CancelledBy;
                    }

                else if (CancelledBy == approvalOwner1)
                    {
                        Obj.IsApproverCancelled = true;
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;
                        Obj.ApproverCancelledDate = DateTime.Now;
                    Obj.ApproverCancelledBy = CancelledBy;
                    Obj.CancelledBy = CancelledBy;
                        }
                else if (CancelledBy == approvalOwner2)
                    {
                    Obj.IsReviewerCancelled = true;
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;
                    Obj.ReviewerCancelledDate = DateTime.Now;
                    Obj.ReviewerCancelledBy = CancelledBy;
                    Obj.CancelledBy = CancelledBy;
                        }

                if (AA.ApprovalStatusId == 2 && AA.ReviewerstatusId == 2)
                {
                    ELA.StaffId = Obj.StaffId;
                    ELA.LeaveTypeId = Obj.LeaveTypeId;
                    ELA.TransactionFlag = 1;
                    ELA.TransactionDate = DateTime.Now;
                    ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays);
                    ELA.Narration = "Cancelled the Approval leave application.";
                    ELA.RefId = Obj.Id;
                    ELA.LeaveCreditDebitReasonId = 23;
                }
                else
                {
                    ELA = null;
                }

                if (string.IsNullOrEmpty(applicantEmailId).Equals(false) && CancelledBy != Obj.StaffId)
                    {
                        ESL.Add(new EmailSendLog  //Send Mail to User
                        {
                        From = commonSenderEmailId,
                        To = applicantEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Requested leave application status ",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + applicantName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Your " + LeaveType + "  application for the date " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
                        " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + " has been cancelled. <p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
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

                if (string.IsNullOrEmpty(approvalOwner1EmailId).Equals(false) && CancelledBy == Obj.StaffId)
                    {
                    ESL.Add(new EmailSendLog  //Send Mail to User
                        {
                        From = commonSenderEmailId,
                        To = approvalOwner1EmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                        EmailSubject = "Requested leave application status ",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + approvalOwner1Name + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        " " + applicantName + " has cancelled the " + LeaveType + "  application for the date from " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") +
                        " to " + Convert.ToDateTime(Obj.EndDate).ToString("dd-MMM-yyyy") + " . <p style=\"font-family:tahoma;" +
                        " font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;" +
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

                if (APA != null && string.IsNullOrEmpty(alternatePersonName).Equals(false))
                    {
                    ESL.Add(new EmailSendLog  //Send Mail to alternative Person
                        {
                        From = commonSenderEmailId,
                        To = alternatePersonEmailId,
                            CC = string.Empty,
                            BCC = string.Empty,
                        EmailSubject = "Work Allocation for leave application of " + alternatePersonName,
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "Dear " + alternatePersonName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" +
                        "A request for work allocation application has been assigned for " + applicantName + " on application for the date" +
                        " " + Convert.ToDateTime(Obj.StartDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(Obj.EndDate).
                        ToString("dd-MMM-yyyy") + " has been cancelled. <p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p>" +
                        "<p style=\"font-family:tahoma; font-size:9pt;\">" + cancelledByUserName + " &nbsp;(" + CancelledBy + ")</p></body></html>",
                            CreatedOn = DateTime.Now,
                            CreatedBy = Obj.AppliedBy,
                            IsSent = false,
                        SentOn = null,
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }

                    CTS.RA = Obj;
                    CTS.ESL = ESL;
                    CTS.APA = APA;
                        CTS.ELA = ELA;

                using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
                    {
                    rALeaveApplicationRepository.CancelApplication(CTS);
                }
                return "OK";
                    }
                    else //If the leave application has already been cancelled then...
                    {
                        throw new Exception("You cannot cancel a leave request that is already been cancelled.");
                    }
                }

        public decimal GetLeaveBalance(string StaffId, string LeaveTypeId)
                    {

            using (RALeaveApplicationRepository rALeaveApplicationRepository = new RALeaveApplicationRepository())
                            {
                return rALeaveApplicationRepository.GetLeaveBalance(StaffId, LeaveTypeId);
                        }
        }

        public void ValidateLeaveBalance(string StaffId, string LeaveTypeId, decimal TotalDays)
        {
            var LeaveBalance = GetLeaveBalance(StaffId, LeaveTypeId);

            if (LeaveBalance < Convert.ToDecimal(TotalDays))
            {
                throw new Exception("There is no sufficient balance in your specified leave account.");
            }
        }
    }
}
