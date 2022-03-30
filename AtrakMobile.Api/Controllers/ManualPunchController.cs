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
using Attendance.Model;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Configuration;
using AtrakMobileApi.Models;
using AtrakMobileApi.Helpers;
using Newtonsoft.Json;
using AtrakMobileApi.Logging;

namespace AtrakMobileApi.Controllers
{
    [RoutePrefix("api/ManualPunch")]
    public class ManualPunchController : ApiController
    {
        [HttpPost]
        [Authorize]
        [Route("CreatePunch")]
        public IHttpActionResult NewManualPunch(ManualPunch_Dto punchDto)
        {
            CustomLogging.LogMessage(TracingLevel.INFO, JsonConvert.SerializeObject(punchDto));
            CommonBusinessLogic CB = new CommonBusinessLogic();
            Response response = null;
            RAManualPunchApplicationBusinessLogic BL = new RAManualPunchApplicationBusinessLogic();
            var identity = (ClaimsIdentity)User.Identity;
            UserClaims uc = UserHelper.GetUserClaims(identity);
            string BaseAddress = string.Empty;
            string LocationId = uc.LocationId;
            var SecurityGroupId = uc.RoleId; // "0";
            var loggedstaffid = uc.StaffId;
            var format = "dd-MM-yyyy HH:mm:ss";

            try
            {

                DateTime endDate;
                DateTime startDate;
                if (punchDto.PunchType.ToUpper().Equals("IN"))
                {
                    if (punchDto.ManualPunchStartDateTime != String.Empty)
                        punchDto.ManualPunchEndDateTime = punchDto.ManualPunchStartDateTime;
                    else
                        throw new ApplicationException("In Punch missing values");
                }
                if (punchDto.PunchType.ToUpper().Equals("OUT"))
                {
                    if (punchDto.ManualPunchEndDateTime != String.Empty)
                        punchDto.ManualPunchStartDateTime = punchDto.ManualPunchEndDateTime;
                    else
                        throw new ApplicationException("Out Punch missing values");
                }

                if (punchDto.PunchType.ToUpper().Equals("INOUT"))
                {
                    if (punchDto.ManualPunchEndDateTime == String.Empty || punchDto.ManualPunchStartDateTime == string.Empty)
                    {
                        throw new ApplicationException("InOut punches missing values");
                    }
                }


                endDate = DateTime.ParseExact(punchDto.ManualPunchEndDateTime, format, CultureInfo.InvariantCulture);
                startDate = DateTime.ParseExact(punchDto.ManualPunchStartDateTime, format, CultureInfo.InvariantCulture);


                punchDto.MPStartDateTime = startDate;
                punchDto.MPEndDateTime = endDate;

                ManualPunchHelper.ValidatePunchInput(punchDto);
                ManualPunchHelper.ValidateDuplication(punchDto);
                ManualPunchHelper.ValidateSameDurationWhenSameDate(punchDto.MPStartDateTime, punchDto.MPEndDateTime);
                var CTS = ManualPunchHelper.GetClassesToSave(punchDto, uc);
                BL.SaveRequestApplication(CTS, SecurityGroupId, LocationId);
                response = new Response()
                {
                    StatusCode = "1",
                    Message = "Your request has been sent for approval."
                };

            }

            catch (Exception err)
            {
                response = new Response()
                {
                    StatusCode = "0",
                    Message = err.Message
                };
                //var logger = new ExceptionManagerApi();
                //logger.Log(JsonConvert.SerializeObject(punchDto));

            }
            return Ok(response);
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("History/{staffId}")]
        [Authorize]
        public IHttpActionResult ApplicationHistory(string staffId)
        {
            try
            {

                return Redirect(
    Url.Link("Default", new { controller = "api/Dashboard", action = $"applicationHistory/manualpunch/{staffId}" })
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
                Url.Link("Default", new { controller = "api/Approvals", action = $"PendingApprovals/manualpunch/{staffId}" }));

            }
            catch 
            {
                //Log e;
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("Action/{actionType}/{applnId}")]
        public IHttpActionResult ManualPunchApplicationAction(string actionType, string applnId)
        {
            try
            {
                return Redirect(
                Url.Link("Default", new { controller = "api/Approvals", action = $"ManualPunchApplication/{applnId}/{actionType}" }));

            }
            catch
            {
                //Log e;
                return InternalServerError();
            }
        }


    }
}