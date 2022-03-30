using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Web.Http;
using AtrakMobileApi.Helpers;
using AtrakMobileApi.Models;
using Attendance.BusinessLogic;
using Attendance.Model;
using Newtonsoft.Json;
using AtrakMobileApi.Logging;

namespace AtrakMobileApi.Controllers
{
    [RoutePrefix("api/COff")]
    public class COffController : ApiController
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

            //   obj.LeaveRequestEmptyDto = new LeaveRequest_Dto();
            return Ok(obj);

        }


        [HttpGet]
        [Authorize]
        [Route("GetDatesForAvailing/{StaffId}")]

        public IHttpActionResult GetCOffDatesForAvailaing(string StaffId)
        {
            ///Class COffCreditBalanceList  used for COFFAvailing
            RACoffCreditApplicationBusinessLogic CRBL = new RACoffCreditApplicationBusinessLogic();
            //<>var COffReqAvailingDates = CRBL.GetCompOffRequestList(StaffId);
            var COffReqAvailingDates = CRBL.GetWorkedDatesForCompOffCreditRequest(StaffId);

            return Ok(COffReqAvailingDates); //return COffReqAvailModel
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="staffId"></param>
        [HttpGet]
        [Route("avail/History/{staffId}")]
        [Authorize]
        public IHttpActionResult ApplicationAvailedHistory(string staffId)
        {
            try
            {
                return Redirect(
             Url.Link("Default",
             new { controller = "api/Dashboard", action = $"applicationHistory/COMPOFFAVAIL/{staffId}" }));

            }
            catch 
            {
                //Log e;
                return InternalServerError();
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetWorkedDayDetail/{StaffId}/{WorkedDate}")]
        ///Class COffCreditBalanceList  used for COFFAvailing
        public IHttpActionResult GetAllOTDates(string StaffId, DateTime WorkedDate)
        {
            RACoffCreditApplicationBusinessLogic BL = new RACoffCreditApplicationBusinessLogic();
            var lstComp = BL.GetAllOTDates(StaffId, WorkedDate.ToString("dd-MMM-yyyy"), WorkedDate.ToString("dd-MMM-yyyy"));
            //            var workedDayDetail = lstComp[0] ;

            return Ok(lstComp);//returns COffReqDates object

        }

        [HttpPost]
        [Authorize]
        [Route("NewCreditRequest")]
        public IHttpActionResult NewCreditRequestApplication(COffCreditRequest_Dto COffCrReqDto)
        {
            CustomLogging.LogMessage(TracingLevel.INFO, JsonConvert.SerializeObject(COffCrReqDto));
            string Message = string.Empty;
            string applicationURL = string.Empty;
            string securityGroupId = string.Empty;
            var resDate = ConfigurationManager.AppSettings["RestrictionDate"].ToString();
            applicationURL = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();

            Response response = null;
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                UserClaims uc = UserHelper.GetUserClaims(identity);
                //--Rajesh
                CommonBusinessLogic CB = new CommonBusinessLogic();
                RACoffCreditApplicationBusinessLogic BL = new RACoffCreditApplicationBusinessLogic();
                string loggedstaffid = uc.StaffId;
                string LocationId = uc.LocationId;
                securityGroupId = "2";
                COffHelper.ValidateWorkedDay(COffCrReqDto.TotalDays);
                COffHelper.ValidateCoffCreditApplication(COffCrReqDto);
                var CTS = COffHelper.GetClassesToSave(COffCrReqDto, uc);
                //Rajesh . TO check the manualCredit value when it is set to true/ false.
                //  bool ManualCredit = false;
                BL.SaveRequestApplication(CTS, securityGroupId, applicationURL);
                response = new Response()
                {
                    StatusCode = "OK",
                    Message = "COff Credit Request Applied successfully"
                };

            }

            catch (Exception err)
            {
                response = new Response()
                {
                    StatusCode = "ERROR",
                    Message = err.Message

                };

            }
            return Ok(response);


        }

        [HttpPost]
        [Authorize]
        [Route("NewCOffAvailReqeust")]
        public IHttpActionResult NewCOffAvailRequest(COffAvailingRequest_Dto COffAvailingDto)
        {
            CustomLogging.LogMessage(TracingLevel.INFO, JsonConvert.SerializeObject(COffAvailingDto));
            string Message = string.Empty;
            var resDate = ConfigurationManager.AppSettings["RestrictionDate"].ToString();
            Response response = null;
            try
            {
                string applicationURL = string.Empty;
                applicationURL = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
                var identity = (ClaimsIdentity)User.Identity;
                UserClaims uc = UserHelper.GetUserClaims(identity);
                //--Rajesh

                //RACoffRequestApplicationBusinessLogic BL = new RACoffRequestApplicationBusinessLogic();
                RACoffAvalingApplicationBusinessLogic rACoffAvalingApplicationBusinessLogic = new RACoffAvalingApplicationBusinessLogic();
                ClassesToSave CTS = new ClassesToSave();
                //insert into Request Application Table.
                RequestApplication RA = new RequestApplication();
                CommonBusinessLogic CB = new CommonBusinessLogic();
                RA.Id = rACoffAvalingApplicationBusinessLogic.GetUniqueId();
                RA.StaffId = COffAvailingDto.StaffId;
                RA.LeaveTypeId = "LV0005";
                RA.LeaveStartDurationId = COffAvailingDto.LeaveStartDurationId;
                RA.LeaveEndDurationId = COffAvailingDto.LeaveEndDurationId;
                RA.StartDate = Convert.ToDateTime(COffAvailingDto.CoffStartDate);
                RA.EndDate = Convert.ToDateTime(COffAvailingDto.CoffEndDate);

                RA.TotalDays = Convert.ToDecimal(COffAvailingDto.TotalDays);
                RA.ContactNumber = COffAvailingDto.ContactNumber;
                RA.Remarks = COffAvailingDto.Remarks;
                RA.ReasonId = 0;
                RA.IsCancelled = false;
                RA.IsApproved = false;
                RA.IsRejected = false;
                RA.ApplicationDate = DateTime.Now;
                RA.AppliedBy = COffAvailingDto.StaffId;
                RA.RequestApplicationType = "CO";
                RA.IsCancelApprovalRequired = false;
                RA.IsCancelApproved = false;
                RA.IsCancelRejected = false;
                RA.WorkedDate = COffAvailingDto.WorkedDate;

                // Insert Into Application Approval Table.
                ApplicationApproval AA = new ApplicationApproval
                {
                    Id = rACoffAvalingApplicationBusinessLogic.GetUniqueId(),
                    ParentId = RA.Id,
                    ApprovalStatusId = 1,
                    Approval2statusId = 1,
                    ApprovedBy = null,
                    Approval2By = null,
                    Approval2On = null,
                    ApprovedOn = null,
                    Comment = null,
                    ApprovalOwner = uc.ApprovalOwner// model.ReportingManagerId;
                };
                AA.Approval2Owner = uc.ApprovalOwner;
                AA.ParentType = "CO";
                AA.ForwardCounter = 1;
                AA.ApplicationDate = RA.ApplicationDate;


                CTS.RA = RA;
                CTS.AA = AA;
                //
                rACoffAvalingApplicationBusinessLogic.SaveRequestApplication(CTS, uc.RoleId, applicationURL); //roleid is securitygroupid
                response = new Response()
                {
                    StatusCode = "OK",
                    Message = "Your request has been sent for approval"
                };
            }

            catch (Exception err)
            {
                response = new Response()
                {
                    StatusCode = "ERROR",
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
        /// <summary>
        /// /
        /// </summary>
        /// <param name="staffId"></param>

        /// <returns></returns>
        [HttpGet]
        [Route("Req/History/{staffId}")]
        [Authorize]
        public IHttpActionResult ApplicationReqHistory(string staffId)
        {
            try
            {

                return Redirect(
    Url.Link("Default",
    new { controller = "api/Dashboard", action = $"applicationHistory/COMPOFFCREDITREQ/{staffId}" }));

            }
            catch 
            {
                //Log e;
                return InternalServerError();
            }
        }
    }
}


//**

///// <summary>
///// /
///// </summary>
///// <param name="staffId"></param>
///// <returns></returns>
//[HttpGet]
//[Route("WorkedDates/{staffId}")]
//public IHttpActionResult WorkedDates(string staffId)
//{
//    try
//    {
//        //var labl = new LeaveApplicationBusinessLogic();
//        //var lst = labl.GetLeaveApplications(staffId);
//        var labl = new RALeaveApplicationBusinessLogic();
//        var lst = labl.GetAppliedLeaves(staffId);
//        if (lst.Count == 0)
//        {
//            return NotFound();
//        }
//        return Ok(lst);
//    }
//    catch (Exception)
//    {
//        //Log e;
//        return InternalServerError();
//    }
//}
//**//