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

namespace WebApisTokenAuth.Controllers
{
    /// <summary>
    /// Provides API's for the dashboard for the logged in user.
    /// </summary>
    /// 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="StaffId"></param>
    /// <param name="LeaveStartDurationId"></param>
    /// <param name="FromDate"></param>
    /// <param name="ToDate"></param>
    /// <param name="LeaveEndDurationId"></param>
    /// <param name="LeaveType"></param>
    /// <returns></returns>
    ///

    public class LeaveApplicationController : ApiController
    {
        [HttpGet]
        [Route("TotalDays/{StaffId}/{LeaveStartDurationId}/{FromDate}/{ToDate}/{LeaveEndDurationId}/{LeaveType}")]
        public IHttpActionResult GetTotalDaysLeave(string StaffId, string LeaveStartDurationId, string FromDate, string ToDate, string LeaveEndDurationId, string LeaveType)
        {

            var LA = new RALeaveApplicationBusinessLogic();
            var TotalDays = LA.GetTotalDaysLeave(StaffId, LeaveStartDurationId, FromDate, ToDate, LeaveEndDurationId, LeaveType);

            return Ok(TotalDays);

        }

        [HttpGet]
        [Route("LeaveDuration")]
        public IHttpActionResult GetLeaveDuration()
        {
            var Dash = new RALeaveApplicationBusinessLogic();
            var LeaveDuration = Dash.GetLeaveDurations();
            return Ok(LeaveDuration);

        }


        [HttpPost]
        [Route("LeaveApplication")]
        public IHttpActionResult RenderNLeaveApplication([FromBody]Models.LeaveApplication model)
        {
            RALeaveApplicationBusinessLogic BL = new RALeaveApplicationBusinessLogic();
            CommonBusinessLogic CB = new CommonBusinessLogic();

            var bl = new IndividualLeaveCreditDebitBusinessLogic();
            try
            {
                //Must check the date is in between or above the pay cycle
                //ValidateApplicationForPayDate(Convert.ToDateTime(model.LeaveStartDate), Convert.ToDateTime(model.LeaveEndDate));
                // date validation.
                BL.FromDateShouldBeLessThanToDate(Convert.ToDateTime(model.LeaveStartDate), Convert.ToDateTime(model.LeaveEndDate));
                //over lapping validation
                BL.RequestApplicationMustNotOverLapWithTheOther(model.StaffId, model.LeaveTypeId, model.LeaveStartDate, model.LeaveEndDate, "LA", "ADD");
                //Validate leave balance.
                if (model.LeaveTypeId != "LV0036")
                {
                    BL.ValidateLeaveBalance(model.StaffId, model.LeaveTypeId, Convert.ToDecimal(model.TotalDays));
                }
                //Validate Leave Application
                //BL.ValidateLeaveApplication(model.StaffId, Convert.ToDateTime(model.LeaveStartDate), model.LeaveTypeId, Convert.ToDecimal(model.TotalDays));

                //Must be same duration when the start and end date are same.
                BL.MustBeSameDurationWhenSameDate(Convert.ToDateTime(model.LeaveStartDate), Convert.ToDateTime(model.LeaveEndDate),
                    Convert.ToInt16(model.LeaveStartDurationId), Convert.ToInt16(model.LeaveEndDurationId));


                if (ModelState.IsValid == true)
                {
                    ClassesToSave CTS = new ClassesToSave();
                    //insert into Request Application Table.
                    RequestApplication RA = new RequestApplication();
                    RA.Id = BL.GetUniqueId();
                    RA.StaffId = model.StaffId;
                    RA.LeaveTypeId = model.LeaveTypeId;
                    RA.LeaveStartDurationId = Convert.ToInt16(model.LeaveStartDurationId);
                    RA.StartDate = Convert.ToDateTime(model.LeaveStartDate);
                    RA.LeaveEndDurationId = Convert.ToInt16(model.LeaveEndDurationId);
                    RA.EndDate = Convert.ToDateTime(model.LeaveEndDate);
                    RA.TotalDays = Convert.ToString(model.TotalDays);
                    RA.ContactNumber = model.ContactNumber;
                    RA.Remarks = model.Remarks;
                    RA.ReasonId = 0;
                    RA.IsCancelled = false;
                    RA.IsApproved = false;
                    RA.IsRejected = false;
                    RA.ApplicationDate = DateTime.Now;
                    RA.AppliedBy = model.User_Id;
                    RA.RequestApplicationType = "LA";
                    RA.IsCancelApprovalRequired = false;
                    RA.IsCancelApproved = false;
                    RA.IsCancelRejected = false;

                    //Get ApproverList
                    string ApproverId = CB.GetApproverId(model.StaffId);

                    // Insert Into Application Approval Table.
                    ApplicationApproval AA = new ApplicationApproval();
                    AA.Id = BL.GetUniqueId();
                    AA.ParentId = RA.Id;
                    AA.ApprovalStatusId = 1;
                    AA.ApprovedBy = null;
                    AA.ApprovedOn = null;
                    AA.Comment = null;
                    AA.ApprovalOwner = ApproverId;
                    AA.ParentType = "LA";
                    AA.ForwardCounter = 1;
                    AA.ApplicationDate = RA.ApplicationDate;


                    //Insert into Email Send Log Table.
                    List<EmailSendLog> ESL = new List<EmailSendLog>();
                    //first send acknowledgement email to the user.
                    string ApproverName = CB.GetName(ApproverId);
                    string StaffName = CB.GetName(model.StaffId);
                    if (RA.StaffId == model.User_Id)
                    {
                        ESL.Add(new EmailSendLog
                        {
                            //From = model.ReviewerEmailId ?? string.Empty,                        
                            From = CB.GetEmailFromAdd(),
                            To = CB.GetOfficialEmail(model.StaffId) ?? "-",
                            CC = string.Empty,
                            BCC = string.Empty,
                            EmailSubject = "Request for leave application sent to " + ApproverName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your request for leave application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + model.LeaveStartDate + "</td></tr><tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + model.LeaveEndDate + "</td></tr><tr><td style=\"width:20%;\">Total Days:</td><td style=\"width:80%;\">" + model.TotalDays + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + model.Remarks + "</td></tr></table></p><p>This application has been sent also to your reviewer.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>",
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
                            EmailSubject = "Request for leave application of " + StaffName,
                            EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ApproverName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">A request for leave application has been received from " +StaffName + ".<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + model.LeaveStartDate + "</td></tr><tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + model.LeaveEndDate + "</td></tr><tr><td style=\"width:20%;\">Total Days:</td><td style=\"width:80%;\">" + model.TotalDays + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + model.Remarks + "</td></tr></table></p><p>Your attention is needed to either approve or reject this application.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>",
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
                    //


                    //DocumentUpload docu = new DocumentUpload();
                    //if (RA.Id != null)
                    //{
                    //    docu.ParentId = RA.Id;
                    //}
                    //if (model.LeaveTypeId == "LV0010" || model.LeaveTypeId == "LV0004")
                    //{
                    //    if (model.ProofCopy != null)
                    //    {
                    //        docu.IsActive = true;
                    //        docu.FileContent = model.ProofCopy;
                    //        docu.TypeOfDocument = model.FileExtenstion;
                    //        BL.SaveDocumentInformation(docu);
                    //    }
                    //}

                    return Ok(HttpStatusCode.OK);
                }
                else
                {
                    return Ok(HttpStatusCode.PartialContent);
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException DBErr)
            {
                // database validation errors
                foreach (var errors in DBErr.EntityValidationErrors)
                    foreach (var errs in errors.ValidationErrors)
                        ModelState.AddModelError("", errs.ErrorMessage);
                throw DBErr;
            }
        }
    }
}