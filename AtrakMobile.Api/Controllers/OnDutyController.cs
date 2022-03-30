using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Attendance.BusinessLogic;
using Newtonsoft.Json;
using WebApisTokenAuth.Models;
using Attendance.Model;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Configuration;

namespace WebApisTokenAuth.Controllers
{
    public class OnDutyController : ApiController
    {
        [HttpPost]
        [Route("OnDuty")]
        public IHttpActionResult RenderNOnDutyApplication([FromBody]Models.ODApplication model)
        {
            CommonBusinessLogic CB = new CommonBusinessLogic();
            RAOnDutyApplicationBusinessLogic BL = new RAOnDutyApplicationBusinessLogic();
            RALeaveApplicationBusinessLogic BLL = new RALeaveApplicationBusinessLogic();
            string BaseAddress = string.Empty;
            var EmailStr = string.Empty;
            var ODFromdate = string.Empty;
            var ODTodate = string.Empty;
            string ODDuration = string.Empty;
            BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
            try
            {
                //over lapping validation

                if (model.Duration == "MULTIPLEDAY")
                {
                    BL.ValidateBeforeSave(model.StaffId, model.ODStartDate, model.ODEndDate, model.Duration);
                }
                else
                {
                    DateTime fd = Convert.ToDateTime(model.ODDate + " " + model.ODStartTime);
                    DateTime td = Convert.ToDateTime(model.ODDate + " " + model.ODEndTime);
                    BL.ValidateBeforeSave(model.StaffId, fd.ToString(), td.ToString(), model.Duration);
                }

                if (ModelState.IsValid == true)
                {
                    ClassesToSave CTS = new ClassesToSave();
                    //insert into Request Application Table.
                    RequestApplication RA = new RequestApplication();
                    RA.Id = BL.GetUniqueId();
                    RA.StaffId = model.StaffId;
                    RA.LeaveTypeId = "LV0012";
                    RA.ODDuration = model.Duration;
                    RA.LeaveStartDurationId = model.LeaveStartDurationId;
                    RA.LeaveEndDurationId = model.LeaveEndDurationId;
                    if (model.Duration.ToUpper() == "SINGLEDAY")
                    {
                        RA.StartDate = Convert.ToDateTime(model.ODDate + " " + model.ODStartTime);
                        RA.EndDate = Convert.ToDateTime(model.ODDate + " " + model.ODEndTime);
                        RA.TotalHours = Convert.ToDateTime(model.TotalHours);
                    }
                    if (model.Duration.ToUpper() == "MULTIPLEDAY")
                    {
                        RA.StartDate = Convert.ToDateTime(model.ODStartDate);
                        RA.EndDate = Convert.ToDateTime(model.ODEndDate);
                        RA.TotalDays = model.TotalDays;
                    }

                    //validate the date for paycycle
                    //ValidateApplicationForPayDate(Convert.ToDateTime(RA.StartDate), Convert.ToDateTime(RA.EndDate));

                    RA.ContactNumber = model.ContactNumber;
                    RA.Remarks = model.Remarks;
                    RA.ReasonId = 0;
                    RA.IsCancelled = false;
                    RA.IsApproved = false;
                    RA.IsRejected = false;
                    RA.ApplicationDate = DateTime.Now;
                    RA.AppliedBy = model.User_Id;
                    RA.RequestApplicationType = "OD";
                    RA.IsCancelApprovalRequired = false;
                    RA.IsCancelApproved = false;
                    RA.IsCancelRejected = false;

                    // Insert Into Application Approval Table.
                    string ApproverId = CB.GetApproverId(model.StaffId);
                    ApplicationApproval AA = new ApplicationApproval();
                    AA.Id = BL.GetUniqueId();
                    AA.ParentId = RA.Id;
                    AA.ApprovalStatusId = 1;
                    AA.ApprovedBy = null;
                    AA.ApprovedOn = null;
                    AA.Comment = null;
                    AA.ApprovalOwner = ApproverId;
                    AA.ParentType = "OD";
                    AA.ForwardCounter = 1;
                    AA.ApplicationDate = RA.ApplicationDate;

                    // Insert into Email Send Log Table.
                    List<EmailSendLog> ESL = new List<EmailSendLog>();
                    string ApproverName = CB.GetName(ApproverId);
                    string StaffName = CB.GetName(model.StaffId);
                    if (RA.StaffId == model.User_Id)
                    {
                        //first send acknowledgement email to the user.
                        ESL.Add(new EmailSendLog
                        {
                            //From = model.ReviewerEmailId ?? string.Empty,
                            From = CB.GetEmailFromAdd(),
                            To = CB.GetOfficialEmail(model.StaffId) ?? "-",
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Request for On Duty application sent to " + ApproverName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your On Duty has been submitted to your Reporting Manager (" + ApproverName + ") for Approval.<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">From Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + RA.StartDate + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">To Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + RA.EndDate + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Duration:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + RA.ODDuration + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + RA.Remarks + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>",
                            CreatedOn = RA.ApplicationDate,
                            CreatedBy = model.StaffId,
                            IsSent = false,
                            SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });

                        //send intimation email to the reporting manager.
                        ESL.Add(new EmailSendLog
                        {
                            //From = model.UserEmailId ?? string.Empty,
                            From = CB.GetEmailFromAdd(),
                            To = CB.GetOfficialEmail(ApproverId) ?? "-",
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Request for On Duty application of " + StaffName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ApproverName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a On Duty. On Duty details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">From Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + RA.StartDate + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">To Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + RA.EndDate + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Duration:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + RA.ODDuration + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Reason:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + RA.Remarks + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required for this application.</p><p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">10.114.76.61:8011</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " &nbsp; (" + model.StaffId + ")</p></body></html>",
                            CreatedOn = RA.ApplicationDate,
                            CreatedBy = model.StaffId,
                            IsSent = false,
                            SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                            IsError = false,
                            ErrorDescription = "-",
                            SentCounter = 0
                        });
                    }


                    CTS.RA = RA;
                    CTS.AA = AA;
                    CTS.ESL = ESL;
                    //
                    BL.SaveRequestApplication(CTS);
                    return Ok(HttpStatusCode.OK);
                }
                else
                {
                    return Ok(HttpStatusCode.PartialContent);
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException DBErr)
            {
                foreach (var errors in DBErr.EntityValidationErrors)
                    foreach (var errs in errors.ValidationErrors)
                        ModelState.AddModelError("", errs.ErrorMessage);
                throw DBErr;
            }
        }
    }
}