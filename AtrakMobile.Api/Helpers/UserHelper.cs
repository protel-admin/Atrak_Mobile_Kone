using AtrakMobileApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
 

namespace AtrakMobileApi.Helpers
{
    public static class UserHelper
    {
     public static  UserClaims  GetUserClaims(ClaimsIdentity identity)
        {
            UserClaims claims = new UserClaims();
           

            claims.StaffId = identity.Claims.First<Claim>(c => c.Type == "StaffId").Value;
            claims.UserName = identity.Claims.First<Claim>(c => c.Type == "FullName").Value;
            claims.EmailId = identity.Claims.First<Claim>(c => c.Type == "EmailId").Value;
            claims.Role = identity.Claims.First<Claim>(c => c.Type == "UserRole").Value;
            claims.RoleId = identity.Claims.First<Claim>(c => c.Type == "RoleId").Value;
            claims.LocationId = identity.Claims.First<Claim>(c => c.Type == "Location").Value;
            claims.ApprovalOwner = identity.Claims.First<Claim>(c => c.Type == "ApprovalOwner").Value;
            claims.ReviewerOwner = identity.Claims.First<Claim>(c => c.Type == "ReviewerOwner").Value;
            claims.ApprovalOwnerEmailId  = identity.Claims.First<Claim>(c => c.Type == "ApprovalOwnerEmailId").Value;
            claims.ReportingManagerName = identity.Claims.First<Claim>(c => c.Type == "ReportingManagerName").Value;

            return claims;
        }


         
    }
}