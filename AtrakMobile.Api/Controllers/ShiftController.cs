using AtrakMobileApi.Helpers;
using AtrakMobileApi.Models;
using Attendance.BusinessLogic;
using Attendance.Model;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
 

namespace AtrakMobileApi.Controllers
{

    /// </summary>
    [RoutePrefix("api/Shift")]

    public class ShiftController : ApiController
    {
        [Route("Location/{locationId}")]
        public IHttpActionResult GetLocationShifts(string locationId)
        {
            try
            {
            //    //
            //    if (lst.Count == 0)
            //    {
            //        return NotFound();
            //    }
            //    return Ok(lst);
            return Ok("You will get a list here when implemented");
            }

            catch 
            {
                //Log e;
                return InternalServerError();
            }
        }
    }
}
