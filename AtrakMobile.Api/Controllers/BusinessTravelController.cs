//using AtrakMobileApi.ExceptionLog;
using AtrakMobileApi.Helpers;
using AtrakMobileApi.Logging;
using AtrakMobileApi.Models;
using Attendance.BusinessLogic;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Web.Http;

namespace AtrakMobileApi.Controllers
{
    [RoutePrefix("api/BusinessTravel")]
    public class BusinessTravelController : ApiController
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
        public IHttpActionResult ApplicationHistoryBT(string staffId)
        {
            try
            {

           return Redirect(
            Url.Link("Default", new { controller = "api/Dashboard", action = $"applicationHistory/businessTravel/{staffId}" })
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
                Url.Link("Default", new { controller = "api/Approvals", action = $"PendingApprovals/businesstravel/{staffId}" }));

            }
            catch 
            {
                //Log e;
                return InternalServerError();
            }
        }



        [HttpGet]
        [Route("Action/{actionType}/{applnId}")]
        public IHttpActionResult BusinessTravelApplicationAction(string actionType, string applnId)
        {
            try
            {
                return Redirect(
                Url.Link("Default", new { controller = "api/Approvals", action = $"BTApplication/{applnId}/{actionType}" }));

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

        public IHttpActionResult NewBTApplication(OD_Dto odDto)
        {
            CustomLogging.LogMessage(TracingLevel.INFO, JsonConvert.SerializeObject(odDto));
            string Message = string.Empty;
     
            var resDate = ConfigurationManager.AppSettings["RestrictionDate"].ToString();
            Response response = null;
            try
            {
                // TinyMapper.Bind<Permission_Dto, RequestApplication>(c => c.Ignore(x => x.Id));
                // var law = TinyMapper.Map<RequestApplication>(perDto);
                var identity = (ClaimsIdentity)User.Identity;
                UserClaims uc = UserHelper.GetUserClaims(identity);
                //--Rajesh


                RAOnDutyApplicationBusinessLogic BL = new RAOnDutyApplicationBusinessLogic();
                RALeaveApplicationBusinessLogic BLL = new RALeaveApplicationBusinessLogic();
                var SecurityGroupId = "0";

                var EmailStr = string.Empty;
                var ODFromdate = string.Empty;
                var ODTodate = string.Empty;
                string ODDuration = string.Empty;
                if (odDto.Duration.ToUpper().Equals("SINGLEDAY"))
                {
                    if (odDto.ODStartDate == String.Empty)
                    {
                        throw new ApplicationException("Missing BT StartDate value");
                    }
                    odDto.ODDate = odDto.ODStartDate.Split(' ')[0];
                    odDto.ODStartTime = odDto.ODStartDate.Split(' ')[1];
                    odDto.ODEndTime = odDto.ODEndDate.Split(' ')[1];
                    odDto.SingleDayLeaveStartDurationId = odDto.LeaveStartDurationId;
                    
                    TimeSpan diff= DateTime.Parse(odDto.ODEndTime)-  DateTime.Parse(odDto.ODStartTime);
                    odDto.TotalHours = diff.ToString();
                }
                if (odDto.Duration.ToUpper().Equals("MULTIPLEDAY"))
                {
                    if (odDto.ODStartDate == String.Empty || odDto.ODEndDate == String.Empty)
                    {
                        throw new ApplicationException("Missing OD Start/End date value");

                    }
                }
                ODBTHelper.ValidateApplication(odDto);

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
                response = new Response()
                {
                    StatusCode = "0",
                    Message = err.Message

                };
                //var logger = new ExceptionManagerApi();
                //logger.Log(err.Message);
                //logger.Log(JsonConvert.SerializeObject(odDto));

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
