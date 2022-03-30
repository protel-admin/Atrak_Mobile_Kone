using AtrakMobileApi.Models;
using Attendance.BusinessLogic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace AtrakMobileApi.Controllers
{
    /// <summary>
    /// Api's to fetch User details
    /// </summary>
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        /// <summary>
        /// Get the user data for the logged in user. User id should be provided as input parameter
        /// </summary>
        /// <param name="staffid"></param>
        /// <returns>JSON object with to get the Staff Details of a given staffId</returns>
        [HttpGet]
        [Route("{staffid}")]
        //[Authorize]
        public IHttpActionResult GetUserBy(string staffid)
        {

            var staffBl = new StaffBusinessLogic();
            var staffInfo = staffBl.GetStaffOfficialInformationForApi(staffid);
            var userDto = new UserProfileDto()
            {
                UserFullName = staffInfo.UserFullName
                 ,
                StaffId = staffInfo.StaffId
                 ,
                DepartmentId = staffInfo.DepartmentId
                 ,
                DesignationId = staffInfo.DesignationId
                 ,
                BranchId = staffInfo.BranchId
                 ,
                DateOfJoining = staffInfo.DateOfJoining?.ToString()
                 ,
                Email = staffInfo.Email
                 ,
                PolicyId = staffInfo.PolicyId
                 ,
                LeaveGroupId = staffInfo.LeaveGroupId
                 ,
                UserRole = staffInfo.UserRole
                 ,
                UserRoleId = staffInfo.UserRoleId
                 ,
                ReportingManagerName = staffInfo.ReportingManagerName
                 ,
                ReportingManagerEmailId = staffInfo.ReportingManagerEmailId
                 ,
                ReportingManagerId = staffInfo.ReportingManagerId
                 ,
                Phone = staffInfo.Phone
                 ,
                Photo = staffInfo.PhotoB64String
                 ,
                Fax = staffInfo.Fax
                // , DivisionName= staffInfo.DivisionName
                 ,
                DepartmentName = staffInfo.DepartmentName
                 //, LocationName= staffInfo.LocationName
                 //, GradeName= staffInfo.GradeName
                 ,
                DesignationName = staffInfo.DesignationName
            };
            return Ok(staffInfo);
        }


        /// <summary>
        /// Get the user data for the logged in user. User id should be provided as input parameter
        /// </summary>
        /// <returns>JSON object with the staff details of the logged in User </returns>

        [HttpGet]
        [Route("GetUserStaffDetail")]
        [Authorize]
        public async Task<IHttpActionResult> GetUserStaffDetail()
        {
            var identity = (ClaimsIdentity)User.Identity;
            string staffId = identity.Claims.First<Claim>(c => c.Type == "StaffId").Value;

            var staffBl = new StaffBusinessLogic();
            var staffPhoto = await staffBl.GetEmployeePhotoAsync(staffId);
            var staffInfo = staffBl.GetStaffOfficialInformationForApi(staffId);
            //TODO Convert the staffOfficialInfo into UserProfileDto
            //return UserProfileDto


            var userDto = new UserProfileDto()
            {
                UserFullName = staffInfo.UserFullName,
                StaffId = staffInfo.StaffId,
                DepartmentId = staffInfo.DepartmentId,
                DesignationId = staffInfo.DesignationId,
                BranchId = staffInfo.BranchId,
                DateOfJoining = staffInfo.DateOfJoining?.ToString(),
                Email = staffInfo.Email,
                PolicyId = staffInfo.PolicyId,
                LeaveGroupId = staffInfo.LeaveGroupId,
                UserRole = staffInfo.UserRole,
                UserRoleId = staffInfo.UserRoleId,
                ReportingManagerName = staffInfo.ReportingManagerName,
                ReportingManagerEmailId = staffInfo.ReportingManagerEmailId,
                ReportingManagerId = staffInfo.ReportingManagerId,
                Phone = staffInfo.Phone,
                Photo = staffPhoto,
                Fax = staffInfo.Fax,
                DivisionName = staffInfo.DivisionName,
                DepartmentName = staffInfo.DepartmentName,
                LocationName = staffInfo.LocationName,
                GradeName = staffInfo.GradeName,
                DesignationName = staffInfo.DesignationName
            };

            return Ok(userDto);
        }




        [HttpGet]
        [Route("GetStaffPhoto/{staffId}")]
        [Authorize]
        public async Task<IHttpActionResult> GetStaffPhotoBy(string staffId)
        {

            var staffBl = new StaffBusinessLogic();
            var staffPhoto = await staffBl.GetEmployeePhotoAsync(staffId);
            return Ok(staffPhoto);

        }

        [HttpGet]
        [Route("GetStaffPhotoImage/{staffId}")]
      
        public  HttpResponseMessage GetStaffPhotoBytesFor(string StaffId)
        {
            //Byte[] b = System.IO.File.ReadAllBytes(@"E:\\Test.jpg");   // You can use your own method over here. 

            var staffBl = new StaffBusinessLogic();
            var staffId = StaffId.Split('.')[0];
            byte[] b =  staffBl.GetEmployeePhotoBytesAsync(staffId);
               

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            if (b != null)
            {
                var dataStream = new MemoryStream(b);
                response.Content = new StreamContent(dataStream);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            }
           
                                                
            return response;
           // return File(b, "image/png");
        }
    }
}