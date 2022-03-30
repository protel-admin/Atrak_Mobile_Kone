﻿//using AtrakMobileApi.ExceptionLog;
using AtrakMobileApi.Helpers;
using AtrakMobileApi.Models;
using Attendance.BusinessLogic;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Web.Http;
using AtrakMobileApi.Logging;

namespace AtrakMobileApi.Controllers
{
    [RoutePrefix("api/WorkFromHome")]
    public class WFHController : ApiController
    {
        RALeaveApplicationBusinessLogic leaveObj = new RALeaveApplicationBusinessLogic();

        /// <summary>
        /// /
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("History/{staffId}")]
        [Authorize]
        public IHttpActionResult ApplicationHistoryWFH(string staffId)
        {
            try
            {

                return Redirect(
                 Url.Link("Default", new { controller = "api/Dashboard", action = $"applicationHistory/workFromHome/{staffId}" })
                 );

            }
            catch
            {
                //Log e;
                return InternalServerError();
            }
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="staffId"></param>
        /// "PendingApprovals/{applnType?}
        /// <returns></returns>
        [HttpGet]
        [Route("PendingApprovals/{staffId}")]
        [Authorize]
        public IHttpActionResult PendingApplication(string staffId)
        {
            try
            {
                return Redirect(
                Url.Link("Default", new { controller = "api/Approvals", action = $"PendingApprovals/workfromhome/{staffId}" }));

            }
            catch
            {
                //Log e;
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("Action/{actionType}/{applnId}")]
        public IHttpActionResult WorkFromHomeApplicationAction(string actionType, string applnId)
        {
            try
            {
                return Redirect(
                Url.Link("Default", new { controller = "api/Approvals", action = $"WFHApplication/{applnId}/{actionType}" }));

            }
            catch
            {
                //Log e;
                return InternalServerError();
            }
        }

        [HttpPost]
        [Authorize]
        [Route("NewApplication")]

        public IHttpActionResult NewWFHApplication(OD_Dto odDto)
        {
            CustomLogging.LogMessage(TracingLevel.INFO, JsonConvert.SerializeObject(odDto));
            string Message = string.Empty;

            var resDate = ConfigurationManager.AppSettings["RestrictionDate"].ToString();
            Response response = null;
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                UserClaims uc = UserHelper.GetUserClaims(identity);
                //--Rajesh

                RAOnDutyApplicationBusinessLogic BL = new RAOnDutyApplicationBusinessLogic();
                RALeaveApplicationBusinessLogic BLL = new RALeaveApplicationBusinessLogic();
                // var SecurityGroupId = "0";

                var EmailStr = string.Empty;
                var ODFromdate = string.Empty;
                var ODTodate = string.Empty;
                string ODDuration = string.Empty;
                if (odDto.Duration.ToUpper().Equals("SINGLEDAY"))
                {
                    if (odDto.ODStartDate == String.Empty)
                    {
                        throw new ApplicationException("Missing WFH StartDate value");
                    }
                    odDto.ODDate = odDto.ODStartDate.Split(' ')[0];
                    odDto.ODStartTime = odDto.ODStartDate.Split(' ')[1];
                    odDto.ODEndTime = odDto.ODEndDate.Split(' ')[1];
                    odDto.SingleDayLeaveStartDurationId = odDto.LeaveStartDurationId;
                    DateTime ODFromTime = Convert.ToDateTime(odDto.ODStartTime);
                    DateTime ODToTime = Convert.ToDateTime(odDto.ODEndTime);
                    if (ODFromTime > ODToTime)
                    {
                        ODToTime = Convert.ToDateTime(ODToTime).AddDays(1);
                        TimeSpan timeSpan = ODToTime - ODFromTime;
                        odDto.TotalHours = timeSpan.ToString();
                    }
                }
                if (odDto.Duration.ToUpper().Equals("MULTIPLEDAY"))
                {
                    if (odDto.ODStartDate == String.Empty || odDto.ODEndDate == String.Empty)
                    {
                        throw new ApplicationException("Missing WFH Start/End date value");

                    }
                }

                var CTS = ODBTHelper.GetClassesToSave(odDto, uc);
                BL.SaveRequestApplication(CTS, uc.RoleId, uc.LocationId);
                response = new Response()
                {
                    StatusCode = "1",
                    Message = "Your request has been sent for approval."
                };

            }

            catch (Exception err)
            {
                while (err.InnerException != null)
                {
                    err = err.InnerException;
                }
                response = new Response()
                {
                    StatusCode = "0",

                    Message = err.Message

                };
            }
            return Ok(response);

        }
        public string ApproveRejectApplication(string ApproverId, string ApplicationApprovalId, bool Approve)
        {
            string str = string.Empty;
            try
            {
                //call function to approve or reject the leave application.
                var bl = new CommonBusinessLogic();
                bl.ApplicationApprovalRejection(ApproverId, ApplicationApprovalId, Approve);
                if (Approve == true)
                {
                    str = "<!DOCTYPE html><html><head><title></title></head><body style=\"font-family:arial;\"><div style=\"padding:10px 10px 10px 10px; border:1px solid orange; margins:20px 20px 20px 20px;\"><h3>The application has been successfully approved.</h3></div></body></html>";
                }
                else
                {
                    str = "<!DOCTYPE html><html><head><title></title></head><body style=\"font-family:arial;\"><div style=\"padding:10px 10px 10px 10px; border:1px solid orange; margins:20px 20px 20px 20px;\"><h3>The application has been rejected.</h3></div></body></html>";
                }

                return str;

            }
            catch (Exception err)
            {
                str = "<!DOCTYPE html><html><head><title></title></head><body style=\"font-family:arial;\"><div style=\"padding:10px 10px 10px 10px; border:1px solid orange; margins:20px 20px 20px 20px;\"><h3>" + err.Message + "</h3></div></body></html>";
                return str;
            }

        }
    }
}
