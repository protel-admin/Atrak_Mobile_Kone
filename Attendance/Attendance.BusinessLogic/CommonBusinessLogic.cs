using Attendance.Model;
using Attendance.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Attendance.BusinessLogic
{
    public class CommonBusinessLogic
    {

        string Message = string.Empty;
        public int ValidateUserAccountBusinessLogic(string StaffId)
        {
            using (CommonRepository commonRepository = new CommonRepository())
            { 
                return commonRepository.ValidateUserAccountRepository(StaffId);
            }
        }

        public void SaveMobilePunch(DashboardSwipes ds)
        {
            using (CommonRepository commonRepository = new CommonRepository())
            { 
                commonRepository.SaveMobilePunch(ds);
            }
        }

        public string GetStaffName(string StaffId)
        {

            using (CommonRepository commonRepository = new CommonRepository())
            {
                if (!string.IsNullOrEmpty(StaffId))   //Added by Rajesh on Nov 24 2021
                {
                    return commonRepository.GetStaffName(StaffId);
                }
                else
                    return string.Empty;
            }
        }

        //Rajesh:12-aug-20 :Added this methiod for mobile login validation
        public string Decrypt(string password)
        {
            using (CommonRepository commonRepository = new CommonRepository())
            {
                return commonRepository.Decrypt(password);
            }

        }

        public string ValidateApplicationForPayDate(string StaffId, DateTime ApplicationStartDate, DateTime ApplicationEndDate)
        {

            using (CommonRepository commonRepository = new CommonRepository())
            {
                return commonRepository.ValidateApplicationForPayDate(StaffId, ApplicationStartDate, ApplicationEndDate);
            }

        }
        public string ValidateApplication(string StaffId, string FromDate, string ToDate, string TotalDays, string LeaveTypeId)
        {using (CommonRepository commonRepository = new CommonRepository())
            {
                var Result = commonRepository.ValidateApplication(StaffId, FromDate, ToDate, TotalDays, LeaveTypeId);
                if (!Result.ToUpper().StartsWith("OK"))
                {
                    throw new Exception(Result);
                }
                return Result;
            }
         }

        public string ValidateDonationApplication(string DonorStaffID, string ReceiverStaffID, string LeaveCount)
        {
            using (var repo = new CommonRepository())
            { 
                return repo.ValidateDonationApplication(DonorStaffID, ReceiverStaffID, LeaveCount);
            }
        }
        public int CheckPunchExistOrNotBusinessLogic(string StaffId, string ManualPunchStartDateTime, string ManualPunchEndDateTime)
        {using (CommonRepository commonRepository = new CommonRepository())
            {
                return commonRepository.CheckPunchExistOrNotRepository(StaffId, ManualPunchStartDateTime, ManualPunchEndDateTime);
            }
        }
        public string ValidateApplicationForPayDate(string StaffId, string ApplicationStartDate, string ApplicationEndDate)
        {
            using (var repo = new CommonRepository())
            { 
                string msg = repo.ValidateApplicationForPayDate(StaffId, ApplicationStartDate, ApplicationEndDate);
            return msg;
            }
        }
        public string ValidateShiftExtension(string StaffId, string Date, string Duration, string BeforeShift, string AfterShift)
        {using (var repo = new CommonRepository())
            {
                string msg = repo.ValidateShiftExtension(StaffId, Date, Duration, BeforeShift, AfterShift);
                return msg;
            }
        }

        public bool IfUserHasLeafs(string StaffId)
        {using (var repo = new CommonRepository())
            {
                return repo.IfUserHasLeafs(StaffId);
            }
        }

        public List<SelectListItem> GetRoleList()
        {
            using (var repo = new CommonRepository())
            { 
                var lst = repo.GetRoleList();
            return ConvertRoleListToListItem(lst);
            }
        }

        public List<SelectListItem> ConvertRoleListToListItem(List<RoleList> lst)
        {
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = true
            }).ToList();

            return items;
        }

        public string GetScreens(int UserRoleId)
        {
            using (var repo = new CommonRepository())
            { 
            var str = repo.GetScreens(UserRoleId);
            return str;
            }
        }

        public LoggedInUserDetails GetDomainIdBasedDetails(string DomainId)
        {using (var repo = new CommonRepository())
            {
                var UsrDet = repo.GetDomainIdBasedDetails(DomainId);
                return UsrDet;
            }
        }

        public LoggedInUserDetails GetUserIdBasedDetails(string StaffId)
        {
            using (var repo = new CommonRepository())
            { 
            var UsrDet = repo.GetUserIdBasedDetails(StaffId);
            return UsrDet;
            }
        }

        public void ApplicationApprovalRejection(string ApproverId, string ApplicationApprovalId, bool Approve, string ParentType, string LocationId)
        {
            using (var repo = new CommonRepository())
            { 
                repo.ApplicationApprovalRejection(ApproverId, ApplicationApprovalId, Approve, ParentType, LocationId);
            }
        }


        public void ApplicationApprovalRejection(string ApproverId, string ApplicationApprovalId, bool Approve)
        {
            using (CommonRepository commonRepository = new CommonRepository())
            {
                commonRepository.ApplicationApprovalRejection(ApproverId, ApplicationApprovalId, Approve);
            }
        }


        public void ResetPassword(string UserName, string StaffId)
        {using (var repo = new CommonRepository())
            {
                repo.ResetPassword(UserName, StaffId);
            }
        }
        public void DeleteAspNetUsers(string UserName, string StaffId)
        {using (var repo = new CommonRepository())
            {
                repo.DeleteAspNetUsers(UserName, StaffId);
            }
        }
        public void DeleteAtrakUserDetails(string UserName, string StaffId)
        {using (var repo = new CommonRepository())
            {
                repo.DeleteAtrakUserDetails(UserName, StaffId);
            }
        }

        public void SendEmailMessage(string pFrom, string pTo, string pCC, string pBCC, string EmailSubject, string EmailBody)
        {
            using (var repo = new CommonRepository())
            { 
                repo.SendEmailMessage(pFrom, pTo, pCC, pBCC, EmailSubject, EmailBody);
            }
        }

        public string GetAccessLevel(string Staffid)
        {
            using (var repo = new CommonRepository())
            { 
            var dt = repo.GetAccessLevel(Staffid);
            return dt;
            }
        }

        public DateTime GetAppRestrictionDate(string StaffId)
        {using (var repo = new CommonRepository())
            {
                var date = repo.GetRestrictionAppDate(StaffId);
                return date;
            }
        }

        public List<SubordinateList> GetSubordinateTreeList(string StaffId)
        {using (var repo = new CommonRepository())
            {
                var lst = repo.GetSubordinateTreeList(StaffId);
                return lst;
            }
        }

        public List<HeadCountOverAll> GetHeadCountData(string GroupNo, string DeptId, string CategoryId, string GradeId, string ShiftId, string StaffId, string Date)
        {using (var repo = new CommonRepository())
            {
                var lst = repo.GetHeadCountData(GroupNo, DeptId, CategoryId, GradeId, ShiftId, StaffId, Date);
                return lst;
            }        }

        public string GetUniqueId()
        {
            using (CommonRepository commonRepository = new CommonRepository())
            { 
                return commonRepository.GetUniqueId();
            }
        }

        public List<UserList> ReqisterList(string ReportingManagerId)
        {

            using (var repo = new CommonRepository())
            { 
            var lst = repo.ReqisterList(ReportingManagerId);
            return lst;
            }
        }

        public string GetEmailIdOfEmployee()
        {
            using (CommonRepository commonRepository = new CommonRepository())
            { 
                return commonRepository.GetSettingsEmailIdOfEmployee();
            }
        }
        #region Forget Password
        public string SendPasswordDetailsToUserMail(string StaffId, string Password)
        {
            using (CommonRepository commonRepository = new CommonRepository())
            { 
                return commonRepository.SendPasswordDetailsToUserMail(StaffId, Password);
            }
        }
        public string CheckIsEmployeeExists(string StaffId)
        {using (CommonRepository commonRepository = new CommonRepository())
            {
                return commonRepository.CheckIsEmployeeExists(StaffId);
            }
        }
        public string GetOfficialEmail(string StaffId)
        {using (CommonRepository commonRepository = new CommonRepository())
            {
                return commonRepository.GetOfficialEmail(StaffId);
            }
        }
        public string GetPasswordForUserName(string StaffId)
        {
            using (CommonRepository commonRepository = new CommonRepository())
            { 
                return commonRepository.GetPasswordForUserName(StaffId);
            }
        }

        public bool CheckMobileAppEligible(string StaffId)
        {
            using (CommonRepository commonRepository = new CommonRepository())
            { 
                return commonRepository.CheckMobileAppEligible(StaffId);
            }
        }
        public string SaveUserDetails(string StaffId, string UserName, string Password, string LoggedInUser)
        {
            AtrakUserDetails AUT = new AtrakUserDetails();
            CommonRepository commonRepository = new CommonRepository();
            AUT.StaffId = StaffId;
            AUT.UserName = UserName;
            AUT.Password = Password;
            AUT.IsActive = true;
            AUT.CreatedOn = DateTime.Now;
            AUT.CreatedBy = LoggedInUser;
            Message = commonRepository.SaveUserDetails(AUT);
            return Message;
        }
        public string GetUserName(string StaffId)
        {
            using (var repo = new CommonRepository())
            {
            var Result = repo.GetUserName(StaffId);
            return Result;
            }
        }
        public ReportingDetails GetReportingDetails(string staffId)
        {
            using (CommonRepository commonRepository = new CommonRepository())
            { 
                return commonRepository.GetReportingDetails(staffId);
            }
        }
        public string UserCreationTrigger(string StaffId, string UserName, string Password)
        {
            using (var repo = new CommonRepository())
            {
                string BaseAddress = string.Empty;
                try
                {
                    BaseAddress = System.Configuration.ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
                    BaseAddress = BaseAddress + '"';
                    if (string.IsNullOrEmpty(BaseAddress) == true)
                        throw new Exception("BaseAddress parameter is blank in web.config file.");
                }
                catch (Exception) { throw; }
                var StaffEmailId = repo.GetEmailIdOfStaff(StaffId);
                var StaffName = repo.GetStaffName(StaffId);
                string senderEmailId = string.Empty;
                senderEmailId = repo.GetSenderEmailIdFromEmailSettings();
                var staffid = StaffId;
                if (string.IsNullOrEmpty(StaffEmailId).Equals(false))
                {
                    var EmailStr = string.Empty;
                    EmailStr = " <html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",</br></br>Greetings!" + " </p><p style=\"font-family:tahoma; font-size:9pt;\">Welcome to ATRAK Leave Management System.</br>" + "  Your login Id and Password has been generated by HR." + " Please find the below credentials to access Leave Management System.</br></br>UserName:&nbsp;&nbsp" + UserName + "" + "<br/>Password:&nbsp;&nbsp" + Password + "</br></br>To Login into Leave Management System please click on " + "the below link:</br></br> <a href=\"" + BaseAddress + ">" + BaseAddress + "</a> </p></body></html>";
                    repo.SendEmailMessage(senderEmailId, StaffEmailId, "", "", "ATRAK Leave Management credential of " + StaffName + " - (" + StaffId + ")", EmailStr);
                }
                return "OK";
            }
        } 
        #region ResetPassword
        //public AspnetuserFilterDetails GetAspnetuserFilters()
        //{
        //    CommonRepository commonRepository = new CommonRepository();
        //    {
        //        return commonRepository.GetAspnetuserFilters();
        //    }
        //}
        public List<ResetPasswordDetails> GetAspNetUsersList(AspnetuserFilterDetails aspnetuserFilters)
        {
            using (CommonRepository commonRepository = new CommonRepository())
            {
                return commonRepository.GetAspNetUsersList(aspnetuserFilters);
            }
        }
        #endregion
        public string UpdateAtrakUserDetails(string UserName, string NewPwd)
        {using (CommonRepository commonRepository = new CommonRepository())
            {
                return commonRepository.UpdateAtrakUserDetails(UserName, NewPwd);
            }
        }

        public BirthDayModel GetEmployeeNameAndDeaprtment(string EmployeeCode)
        {
            using (var repo = new CommonRepository())
            { 
                return repo.GetEmployeeNameAndDeaprtment(EmployeeCode);
            }
        }
        public void UpdateLastPunchType (PunchTypeHistory punchTypeHistory)
        {
            using (CommonRepository commonRepository = new CommonRepository())
            { 
                commonRepository.UpdateLastPunchType(punchTypeHistory);
            }
        }

        #endregion
        #region Attendance Policy Config
        public string GetValidOfficialEmailBusinessLogic(string StaffId, string DOJ)
        {using (var repo = new CommonRepository())
            {
                return repo.GetValidOfficialEmailRepository(StaffId, DOJ);
            }
        }
        public string GetEmailFromAdd()
        {

            using (CommonRepository commonRepository = new CommonRepository())
            {
                return commonRepository.GetEmailFromAdd();
            }

        }
        public string Decode(string Reqid)
        {using (var repo = new CommonRepository())
            {
                return repo.Decode(Reqid);
            }
        }
        public string GetPolicyValue(string PolicyId)
        {using (CommonRepository commonRepository = new CommonRepository())
            {
                return commonRepository.GetPolicyValue(PolicyId);
            }
         }
        //public List<RuleGroupTxnsList> GetRule(string id)
        //{
        //    AttendancePolicyRepository CompRepo = new AttendancePolicyRepository();
        //    return CompRepo.GetRule(id);
        //}
        #endregion


    }
}
