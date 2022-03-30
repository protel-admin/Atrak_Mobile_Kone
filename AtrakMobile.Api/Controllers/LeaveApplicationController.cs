using AtrakMobileApi.Helpers;
using AtrakMobileApi.Models;
using Attendance.BusinessLogic;
using Attendance.Model;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using Newtonsoft.Json;
using AtrakMobileApi.Logging;
using LeaveType = AtrakMobileApi.Models.LeaveType;

namespace AtrakMobileApi.Controllers
{

    /// </summary>
    [RoutePrefix("api/Leave")]

    public class LeaveApplicationController : ApiController
    {
        RALeaveApplicationBusinessLogic leaveObj = new RALeaveApplicationBusinessLogic();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Init")]
        [Authorize]
        public IHttpActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            //string staffid = identity.Claims.First<Claim>(c => c.Type == "StaffId").Value;
            string staffId = UserHelper.GetUserClaims(identity).StaffId;
            var obj = new LeaveInitDto();
            obj.DurationList = GetDurationList().ToList();

            obj.LeaveTypes = GetLeaveTypes(staffId);
            obj.AlternativeStaffs = new List<AlternativeStaff>();

            //   obj.LeaveRequestEmptyDto = new LeaveRequest_Dto();
            return Ok(obj);

        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="staffId"></param>
        /// "PendingApprovals/{applnType?}
        /// <returns></returns>
        [HttpGet]
        [Route("History/{staffId}")]
        [Authorize]
        public IHttpActionResult ApplicationHistory(string staffId)
        {
            try
            {
                //var labl = new LeaveApplicationBusinessLogic();
                //var lst = labl.GetLeaveApplications(staffId);

                //var labl = new RALeaveApplicationBusinessLogic();
                //var lst = labl.GetAppliedLeaves(staffId);
                //if (lst.Count==0)
                //{
                //    return NotFound();
                //}
                //return Ok(lst);
                return Redirect(
    Url.Link("Default", new { controller = "api/Dashboard", action = $"applicationHistory/leave/{staffId}" })
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
                Url.Link("Default", new { controller = "api/Approvals", action = $"PendingApprovals/leave/{staffId}" }));

            }
            catch 
            {
                //Log e;
                return InternalServerError();
            }
        }


        [HttpGet]
        [Route("Action/{actionType}/{applnId}")]
        public IHttpActionResult LeaveApplicationAction(string actionType, string applnId)
        {
            try
            {
                return Redirect(
                Url.Link("Default", new { controller = "api/Approvals", action = $"LeaveApplication/{applnId}/{actionType}" }));

            }
            catch 
            {
                //Log e;
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("Balance/{staffId}")]
        public IHttpActionResult ShowLeaveBalanceTable(string staffId)
        {
            //function call to load the leave balance table.
            var bl = new UserLandingPageBusinessLogic();
            var lst = bl.ShowLeaveBalanceTable(staffId);
            var result = lst.Select(l => new { l.LeaveTypeId, l.LeaveTypeName, LeaveBalance = l.LeaveBalance });
            return Ok(result);
        }

        [HttpGet]
        [Route("Balance/{staffId}/{leaveTypeId}")]
        public IHttpActionResult GetLeaveBalance(string staffid, string leaveTypeId)
        {
            var labl = new RALeaveApplicationBusinessLogic();
            var balance = labl.GetLeaveBalance(staffid, leaveTypeId);
            return Ok(balance);
        }

        [HttpPost]
        [Authorize]
        [Route("NewApplication")]
        //public IHttpActionResult LeaveApplication(LeaveApplicationWabco law)  //LeaveApplication
        public IHttpActionResult NewLeaveApplication(LeaveRequest_Dto lrDto)  //LeaveApplication
        {
            CustomLogging.LogMessage(TracingLevel.INFO, JsonConvert.SerializeObject(lrDto));
            string Message = string.Empty;
            Response response = null;
            var resDate = ConfigurationManager.AppSettings["RestrictionDate"].ToString();
            var labl = new RALeaveApplicationBusinessLogic();
            try
            {
                var startDate = String.Empty;
                var endDate = String.Empty;
                try
                {
                    startDate = Convert.ToDateTime(lrDto.StartDate).ToString("dd-MMM-yyyy");
                    endDate = Convert.ToDateTime(lrDto.EndDate).ToString("dd-MMM-yyyy");
                }
                catch
                {
                    var format = "dd-MMM-yyyy";
                    startDate = DateTime.ParseExact(lrDto.StartDate, format, CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                    endDate = DateTime.ParseExact(lrDto.EndDate, format, CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                }
                lrDto.TotalDays = Convert.ToDecimal(labl.GetTotalDaysLeave(lrDto.StaffId, lrDto.LeaveStartDurationId,
                    startDate, endDate, lrDto.LeaveEndDurationId, lrDto.LeaveTypeId));
              
                lrDto.StartDate = startDate;
                lrDto.EndDate = endDate;

                if (lrDto.TotalDays == 0)
                {
                    throw new ApplicationException("Invalid start duration and end duration.");
                }

                var identity = (ClaimsIdentity)User.Identity;
                UserClaims uc = UserHelper.GetUserClaims(identity);

                var clsToSave = LeaveApplicationHelper.GetClassesToSave(lrDto, uc);


                //Rajesh added aa instance to Request Application
                labl.SaveRequestApplication(clsToSave, uc.RoleId, uc.LocationId);
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
            }
            return Ok(response); //success
        }


        private static HttpResponseMessage throwExceptionResult(Exception e)
        {
            //TODO Log exception
            HttpResponseMessage message = new HttpResponseMessage();
            message.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            if (e.InnerException != null)
                message.Content = new StringContent(e.InnerException.StackTrace);
            else
                message.Content = new StringContent("kumar" + e.Message + " -  " + e.StackTrace);
            return (message);
        }

        [HttpPost]
        [Authorize]
        [Route("TotalDaysLeave")]
        public IHttpActionResult GetTotalDaysLeave(LeaveRequest_Dto dto)
        {
            CustomLogging.LogMessage(TracingLevel.INFO, JsonConvert.SerializeObject(dto));
            //string StaffId, string LeaveStartDurationId, string StartDate, string EndDate, string LeaveEndDurationId, string LeaveTypeId
            string data = string.Empty;
            if (string.IsNullOrEmpty(dto.StartDate) || string.IsNullOrEmpty(dto.EndDate))
            {
                return Ok("0");
            }

            var startDt = Convert.ToDateTime(dto.StartDate).ToString("yyyy-MMM-dd");
            var endDt = Convert.ToDateTime(dto.EndDate).ToString("yyyy-MMM-dd");

            if (startDt.Equals(endDt))
            {
                if (dto.LeaveEndDurationId != dto.LeaveStartDurationId)
                {
                    return Ok("0");
                }
            }
            var bl = new RALeaveApplicationBusinessLogic();
            data = bl.GetTotalDaysLeave(dto.StaffId, dto.LeaveStartDurationId, startDt, endDt, dto.LeaveEndDurationId, dto.LeaveTypeId);


            return Ok(data);
        }

        public IEnumerable<Duration> GetDurationList()
        {
            var labl = new RALeaveApplicationBusinessLogic();
            var lst = labl.GetLeaveDurations(); // GetDurationList();
            return lst.Select(d => new Duration() { Id = d.Id, Name = d.Name });

        }

        [HttpPost]

        public string CancelLeaveApplication(CancelRequestDto cancelReq)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                UserClaims uc = UserHelper.GetUserClaims(identity);
                var bl = new RALeaveApplicationBusinessLogic();
                //<>bl.CancelApplication(cancelReq.RequestId,cancelReq.StaffId,cancelReq.ReviewerId, cancelReq.ApproverId);
                // bl.CancelApplication(cancelReq.RequestId, cancelReq.StaffId, uc.LocationId);

                return "OK";
            }
            catch (Exception err)
            {
                return "ERROR! " + err.Message;
            }

        }

        [HttpGet]
        [Route("ApplicationStatus")]
        public List<LeaveRequestDto> GetLeaveApplications(string StaffId)
        {
            var labl = new RALeaveApplicationBusinessLogic();
            var lst = labl.GetAppliedLeaves(StaffId);
            List<LeaveRequestDto> leaveReqList = new List<LeaveRequestDto>();
            TinyMapper.Bind<RALeaveApplication, LeaveRequestDto>();
            TinyMapper.Map(lst, leaveReqList);

            return leaveReqList;


        }

        public List<LeaveType> GetLeaveTypes(string StaffId)
        {
            var bl = new RALeaveApplicationBusinessLogic();
            var lst = bl.GetLeaveReasonList(StaffId);

            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            var leaveTypes = lst.Select(lr => new LeaveType() { Id = lr.Id, Name = textInfo.ToTitleCase(lr.Name.ToLower()) }).ToList();
            var businessTravel = leaveTypes.Find(l => l.Id == "LV0039");
            leaveTypes.Remove(businessTravel);
            businessTravel = leaveTypes.Find(l => l.Id == "LV0005");
            leaveTypes.Remove(businessTravel);
            return leaveTypes;
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