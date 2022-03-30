using AtrakMobileApi.Helpers;
using AtrakMobileApi.Models;
using Attendance.BusinessLogic;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Web.Http;
using Newtonsoft.Json;
using AtrakMobileApi.Logging;


using LeaveType = AtrakMobileApi.Models.LeaveType;

namespace AtrakMobileApi.Controllers
{

    /// </summary>
    [RoutePrefix("api/Permission")]

    public class PermissionController : ApiController
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
            var pTypes = new[] {
              new  {Id=1,Name="LATE COMING" },
              new  {Id=2,Name="EARLY GOING" },
              new  {Id=3,Name="ON DUTY PERMISSION" }

            };
           
            return Ok(new { PermissionTypes=pTypes});

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
    Url.Link("Default", new { controller = "api/Dashboard", action = $"applicationHistory/permission/{staffId}" })
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
                Url.Link("Default", new { controller = "api/Approvals", action = $"PendingApprovals/permission/{staffId}" }));

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
        
        public IHttpActionResult NewPermissionApplication(Permission_Dto perDto)  
        {
            CustomLogging.LogMessage(TracingLevel.INFO, JsonConvert.SerializeObject(perDto));
            string Message = string.Empty;
            Response response = null;

            var resDate = ConfigurationManager.AppSettings["RestrictionDate"].ToString();
            //var format = "dd-MMM-yyyy";
         
            var endDate = String.Empty;
            try
            {
              
                try
                {
                    perDto.PerStartDate = Convert.ToDateTime(perDto.PermissionStartDate);
                    perDto.TotHours = Convert.ToDateTime(perDto.TotalHours); //, "hh:mm:ss", CultureInfo.InvariantCulture);
                    //endDate = Convert.ToDateTime(perDto.PermissionEndDate).ToString("dd-MMM-yyyy");

                    perDto.FromTimeStart = Convert.ToDateTime(perDto.FromTime);// "hh:mm:ss", CultureInfo.InvariantCulture); 
                    perDto.ToTimeEnd = Convert.ToDateTime(perDto.ToTime); 
                }
                catch 
                {
                    throw new ApplicationException("Incorrect Date format");
                }

                PermissionHelper.ValidatePermission(perDto.StaffId, perDto.PerStartDate, perDto.PermissionType, perDto.FromTime,perDto.ToTime, perDto.TotHours);

                var identity = (ClaimsIdentity)User.Identity;
                UserClaims uc = UserHelper.GetUserClaims(identity);

                var CTS = PermissionHelper.GetClassesToSave(perDto,uc);
                RAPermissionApplicationBusinessLogic BL = new RAPermissionApplicationBusinessLogic();
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
                  
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("Action/{actionType}/{applnId}")]
        public IHttpActionResult PermissionApplicationAction(string actionType, string applnId)
        {
            try
            {
                return Redirect(
                Url.Link("Default", new { controller = "api/Approvals", action = $"Permission/{applnId}/{actionType}" }));

            }
            catch 
            {
                //Log e;
                return InternalServerError();
            }
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