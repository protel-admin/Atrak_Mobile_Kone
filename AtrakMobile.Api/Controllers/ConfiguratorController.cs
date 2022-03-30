using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using AtrakMobileApi.Models;
using System.Security;
using System.Security.Claims;
using AtrakMobileApi.Helpers;

namespace AtrakMobileApi.Controllers
{
    /// <summary>
    /// Initial configuration setting for launching Atrak mobile for any registered company
    /// </summary>
    [RoutePrefix("api/Configurator")]
    public class ConfiguratorController : ApiController
    {/// <summary>
     /// List out all the client companies of Atrak mobile application
     /// </summary>
     /// <returns></returns>
        [HttpGet]
        [Route("RegdCompanies")]
        public IHttpActionResult GetCompanies()
        {
            Company company = new Company();
            return Ok(company.GetAllCompanies());
        }
        /// <summary>
        /// Get the mobile setting configuration for a particular client company by passing the companyId as input parameter
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Settings/{companyId}")]
        public IHttpActionResult GetConfiguration(string companyId)
        {


            AtrakConfiguration atrakConfig = new AtrakConfiguration();
            return Ok(atrakConfig.GetConfigurationFor(companyId));
        }


        [HttpGet]
        [Route("Settings/Logo")]
        public IHttpActionResult GetThumbnail()
        {
            var mediaRoot = System.Web.HttpContext.Current.Server.MapPath("~/images");

            var imgPath = Path.Combine(mediaRoot, "Logo.png");
            var fileStream = File.OpenRead(imgPath);
            var fileStreamContent = new StreamContent(fileStream);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = fileStreamContent;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            response.Content.Headers.ContentLength = fileStream.Length;

            return ResponseMessage(response);

        }



        // [HttpGet]
        [Route("Settings/GetPullUpMenus")]
        [HttpGet]
        [Authorize]

        public IHttpActionResult GetMenuList()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roleId = UserHelper.GetUserClaims(identity).RoleId;


            Dictionary<string, string> menuList = new Dictionary<string, string>();
            //menuList.Add("Dashboard", "dashboard/index");
            //menuList.Add("My Attendance", "attendance/myattendance");
            //menuList.Add("Holiday List", "holiday/holidayCalendar");
            //menuList.Add("Leave Apply", "application/leave");
            //menuList.Add("Shift Change", "shifts/index");
            //menuList.Add("Permission", "application/permission");
            //menuList.Add("On Duty", "application/onduty");
            //menuList.Add("Over time", "application/overtime");
            //menuList.Add("Comp Off Req", "application/compoffreq");
            //menuList.Add("Manual Punch", "application/manualpunch");
            //menuList.Add("Team Attendance", "attendance/team");
            //menuList.Add("My Approvals", "approvals/list");
            //menuList.Add("My Profile", "profile/index");
            //menuList.Add("Log Out", "logout");
            if (roleId != "6")
            {
                var menus = new List<object> {
                new  {code="DASH",screen="dashboard"},
                new  {code="MYAT",screen="myattendance"},
                new  {code="HOLI",screen="holidayCalendar"},
                new  {code="RLEA",screen="leaveRequest"},
               // new  {code="RSCH",screen="shiftChangeRequest"},
                new  {code="RWFH",screen="workFromHomeRequest"},
                new  {code="RPER",screen="permissionRequest"},
                new  {code="ROND",screen="onDutyRequest"},
                new  {code="RBUS",screen="busTravelRequest"},
               // new  {code="ROVT",screen="overTimeRequest"},
                new {code="RMAP",  screen  ="manualPunchRequest" },
                  new  {code="RCOC",screen="compOffCreditRequest"},       //Rajesh - Dec 20,2021
                  new  {code="RCOA",screen="compOffRequest"},
                //new  {code="TMAT",screen="teamAttendance"},
                //new  {code="MYAP",screen="myApprovals"},
              //  new  {code="PROF",screen="profile"},
                new  {code="LOUT",screen="logout"},
                   };
                if (roleId == "3" || roleId == "5" || roleId == "1" || roleId == "4")
                {
                    menus.Add(new { code = "TMAT", screen = "teamAttendance" });
                    menus.Add(new { code = "MYAP", screen = "myApprovals" });
                }
                return Ok(menus.Select(m => m));
            }
            else if (roleId == "6")
            {
                var menus = new List<object> {
                new  {code="DASH",screen="dashboard"},
                new  {code="MYAT",screen="myattendance"},
                new  {code="HOLI",screen="holidayCalendar"},
               // new  {code="RSCH",screen="shiftChangeRequest"},
                new  {code="RWFH",screen="workFromHomeRequest"},
                new  {code="ROND",screen="onDutyRequest"},
                new  {code="RBUS",screen="busTravelRequest"},
                new {code="RMAP",  screen  ="manualPunchRequest" },
                new  {code="LOUT",screen="logout"},

                };
                return Ok(menus.Select(m => m));
            }
            return Ok("");
        }
    }
}




/*
 *   menuList.Add("DASH", "dashboard");
            menuList.Add("MYAT", "myattendance");
            menuList.Add("HOLI", "holidayCalendar");
            menuList.Add("RLEA", "leaveRequest");
            menuList.Add("RSCH", "shiftChangeRequest");
            menuList.Add("RPER", "permissionRequest");
              menuList.Add("ROND", "onDutyRequest");
            menuList.Add("ROVT", "overTimeRequest");
            menuList.Add("RCOR", "compOffRequest");
            menuList.Add("RMAP", "manualPunchRequest");
            menuList.Add("TMAT", "teamAttendance");
            menuList.Add("MYAP", "myApprovals");
            menuList.Add("PROF", "profile");
            menuList.Add("LOUT", "logout");
 * 
 * */


