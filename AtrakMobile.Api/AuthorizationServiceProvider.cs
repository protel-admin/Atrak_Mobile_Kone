using Attendance.BusinessLogic;
using AtrakMobileApi.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApisTokenAuth
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); // 
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {

            // Change authentication ticket for refresh token requests  
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
             
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            
            context.Validated(newTicket);

            return Task.FromResult<object>(null);

        }

   
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                //Accounts acc = new Accounts();
                // Initialization.  
                var claims = new List<Claim>();
                // Setting  
                if (context.UserName == string.Empty || context.Password == string.Empty)
                {
                    throw new ApplicationException("Missing username or password");
                }
                CommonBusinessLogic cbl = new CommonBusinessLogic();
               
                var enCryPass = cbl.GetPasswordForUserName(context.UserName);
                if (enCryPass == null || enCryPass == string.Empty)
                {
                    CustomLogging.LogMessage(TracingLevel.INFO, string.Concat(context.UserName, " The password is empty or not found for the user"));

                    context.SetError("invalid_grant", $"Username and password should not be empty");
                    return;
                }
                bool isMobileAppEligible = false;
                isMobileAppEligible = cbl.CheckMobileAppEligible(context.UserName);
                if (isMobileAppEligible == false)
                {
                    CustomLogging.LogMessage(TracingLevel.INFO, string.Concat(context.UserName, "Not privileged to use the mobile app"));
                    context.SetError("invalid_grant", $"Invalid privilege for mobile access {context.UserName}-{enCryPass}");
                    return;
                }

                var deCryPass = cbl.Decrypt(enCryPass);
                claims.Add(new Claim(ClaimTypes.Name, context.UserName));
                // Setting Claim Identities for OAUTH 2 protocol.  
                ClaimsIdentity oAuthClaimIdentity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);

                if (context.Password.Equals(deCryPass))
                {
                    // identity.AddClaim(new Claim(ClaimTypes.Role, acc.GetUserRole(context.UserName)));

                    StaffBusinessLogic staffBl = new StaffBusinessLogic();
                    var staffClaims = staffBl.GetStaffOfficialInformationForApi(context.UserName);
                    
                    identity.AddClaim(new Claim(ClaimTypes.Role, staffClaims.UserRole));
                    //  identity.AddClaim(new Claim("username", context.UserName));
                    identity.AddClaim(new Claim("StaffId", context.UserName));
                    identity.AddClaim(new Claim("FullName", staffClaims.UserFullName));
                    identity.AddClaim(new Claim("EmailId", staffClaims.Email));
                    identity.AddClaim(new Claim("UserRole", staffClaims.UserRole));
                    identity.AddClaim(new Claim("Location", staffClaims.LocationId));
                    identity.AddClaim(new Claim("RoleId", staffClaims.UserRoleId.ToString()));
                    identity.AddClaim(new Claim("ApprovalOwner", staffClaims.ReportingManagerId));
                    identity.AddClaim(new Claim("ReviewerOwner", staffClaims.Reviewer));
                    identity.AddClaim(new Claim("ReportingManagerName", staffClaims.ReportingManagerName));
                    identity.AddClaim(new Claim("ApprovalOwnerEmailId", staffClaims.ReportingManagerEmailId));

                    var AuthMgr = context.OwinContext.Authentication;
                    context.Validated(identity);
                    AuthMgr.SignIn(identity);

                }
                else
                {
                    CustomLogging.LogMessage(TracingLevel.INFO, string.Concat(context.UserName, " The password is incorrect"));
                    context.SetError("invalid_grant", "Incorrect credentials or user name or password missing");
                    return;
                }
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", ex.Message);
                return;
            }
        }
    }
    /// <summary>
    /// /
    /// </summary>
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();
        
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {

            var guid = Guid.NewGuid().ToString();

            // copy all properties and set the desired lifetime of refresh token  
            var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
            {
                IssuedUtc = context.Ticket.Properties.IssuedUtc,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            };

            var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);

            _refreshTokens.TryAdd(guid, refreshTokenTicket);

            // consider storing only the hash of the handle  
            context.SetToken(guid);
        }


        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            // context.DeserializeTicket(context.Token);
            AuthenticationTicket ticket;
            string header = context.OwinContext.Request.Headers["Authorization"];

            if (_refreshTokens.TryRemove(context.Token, out ticket))
            {
                context.SetTicket(ticket);
            }
        }
    }
}

//GrantResourceOwnershipCredentials  (){
/*
                   identity.AddClaim(new Claim(ClaimTypes.Role, "HR"));
                   identity.AddClaim(new Claim("username", context.UserName));
                   identity.AddClaim(new Claim("StaffId", context.UserName));
                   identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                   identity.AddClaim(new Claim("EmailId", "abc@abc.com"));
                   identity.AddClaim(new Claim("UserRole", "0"));
                   identity.AddClaim(new Claim("Location", "L00001"));
                   identity.AddClaim(new Claim("SecurityGroupId", "3"));
                   identity.AddClaim(new Claim("ApprovalOwner", "3"));
                   identity.AddClaim(new Claim("ReviewerOwner", "3"));
                   }*/
