using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
//https://www.ecanarys.com/Blogs/ArticleID/308/Token-Based-Authentication-for-Web-APIs
namespace WebApisTokenAuth.Controllers
{
    [RoutePrefix("api/Authentication")]
    public class AuthenticationController : ApiController
    {

        [HttpGet]
        [Route("NoAuth")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult NoAuth()
        {
            Accounts userRoles = new Accounts();
            return Ok(userRoles.GetUserRoles());
        }

        [HttpGet]
        [Route("AuthorizedUser")]
        [Authorize(Roles ="Admin")]
        public IHttpActionResult AuthorizedUser()
        {
            Accounts userRoles = new Accounts();
            return Ok(userRoles.GetUserRoles());
        }

        [HttpGet]
        [Route("AuthenticatedUser")]
        [Authorize]
        public IHttpActionResult AuthenticatedUser()
        {
            Employees emp = new Employees();
            return Ok(emp.GetEmployees());
        }


        [HttpGet]
        [Route("FirstUser")]
        [Authorize]
        public Employee GetFirstUser()
        {
            Employees emp = new Employees();
            var e= emp.FirstEmployee();
            HttpResponseMessage message = new System.Net.Http.HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("Employee not found in the database")
            };
            throw new HttpResponseException(message);
        }


        [HttpGet]
        [Route("LastUser")]
        [Authorize]
        public HttpResponseMessage GetLastUser()
        {
            Employees emp = new Employees();
            var e = emp.FirstEmployee();
            HttpResponseMessage message = new System.Net.Http.HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("Employee not found in the database")
            };
            if (! e.EmloyeeName.Equals("rajesh"))
            {
                //throw new HttpResponseException(message);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Its an error");
            }
            return Request.CreateResponse(HttpStatusCode.OK, e);


        }


        [HttpPost]
        [Route("PostUserData")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult PostData([FromBody]Models.User userData)
        {
            Accounts regObj = new Accounts();
            ApplicationUser regData = new ApplicationUser();
            regData.EmailID = userData.EmailID;
            regData.Password = Encoding.UTF8.GetBytes(userData.Password);
            regData.UserName = userData.UserName;
            regData.RoleId = userData.RoleId;

            return Ok(regObj.Register(regData));
        }

    }
}
