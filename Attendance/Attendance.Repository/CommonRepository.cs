﻿using Attendance.Model;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
//using System.Web.mvc
namespace Attendance.Repository
{
    public class CommonRepository : IDisposable
    {
        private AttendanceManagementContext context = null;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }
            disposed = true;
        }
        public CommonRepository()
        {
            context = new AttendanceManagementContext();
        }
        StringBuilder builder = new StringBuilder();
        string Message = string.Empty;

        public OTApplication GetOTDetails(string Id)
        {
            return context.OTApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }

        public string GetUniqueId()
        {
            return context.Database.SqlQuery<string>("select convert(varchar,getdate(),112) + replace(convert(varchar,getdate(),114),':','')").First();
        }
        public string GetOTReviewer(string Id)
        {
            var OTReviewer = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select OTReviewer from StaffOfficial where staffid = @StaffId");

            try
            {
                OTReviewer = context.Database.SqlQuery<string>(qryStr.ToString() , new SqlParameter("@StaffId", Id)).FirstOrDefault();
                return OTReviewer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetOTApprover(string Id)
        {
            var OTApprover = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select OTReportingManager from StaffOfficial where staffid= @StaffId");

            try
            {
                OTApprover = context.Database.SqlQuery<string>(qryStr.ToString() , new SqlParameter("@StaffId",Id)).FirstOrDefault();
                return OTApprover;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetCCAddress(string Application, string LocationId)
        {
            try
            {
                string SendEmail = string.Empty;
                var querystring = new StringBuilder();
                querystring.Append("select CCAddress from emailforwardingconfig where LocationID = @LocationId and ScreenID = @Application");
                SendEmail = context.Database.SqlQuery<string>(querystring.ToString() , new SqlParameter("@LocationId" , LocationId) , 
                    new SqlParameter("@Application", Application)).FirstOrDefault();
                return SendEmail;
            }
            catch
            {
                return "";
            }
        }



        public int ValidateUserAccountRepository(string StaffId)
        {
            int ValidateAccount = 0;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Count(StaffId) from AspNetUsers where StaffId=@StaffId and IsActive=1");
                ValidateAccount = context.Database.SqlQuery<int>(builder.ToString(), new SqlParameter("@StaffId", StaffId)).FirstOrDefault();
            }
            catch 
            {
                ValidateAccount = 0;
            }
            return ValidateAccount;
        }

        public int CheckPunchExistOrNotRepository(string StaffId, string InDateTime, string OutDateTime)
        {
            int CheckCount = 0;
            try
            {
                if (string.IsNullOrEmpty(InDateTime).Equals(false))
                {
                    builder = new StringBuilder();
                    builder.Append("select Count(1) from RequestApplication where RequestApplicationType='MP' and" +
                        " StaffId=@StaffId and StartDate=Convert(datetime,@InDateTime) ");
                    CheckCount = context.Database.SqlQuery<int>(builder.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@InDateTime", InDateTime ?? "")).FirstOrDefault();
                    if (CheckCount > 0)
                    {
                        throw new Exception("Already exist data for the request type");
                    }

                    builder = new StringBuilder();
                    builder.Append("select count(1) from SmaxTransaction where Tr_ChId=@StaffId and " +
                        "(Format(Tr_Date,'yyyy-MM-dd') +' ' +Format(Tr_Time,'HH:mm'))=@InDateTime ");
                    CheckCount = context.Database.SqlQuery<int>(builder.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@InDateTime", InDateTime ?? "")).FirstOrDefault();
                    if (CheckCount > 0)
                    {
                        throw new Exception("Already exist for the punch");
                    }
                }
                if (string.IsNullOrEmpty(OutDateTime).Equals(false))
                {
                    builder = new StringBuilder();
                    builder.Append("select Count(1) from RequestApplication where RequestApplicationType='MP' and " +
                        "StaffId=@StaffId and EndDate=Convert(datetime,@OutDateTime) ");
                    CheckCount = context.Database.SqlQuery<int>(builder.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@OutDateTime", OutDateTime ?? "")).FirstOrDefault();
                    if (CheckCount > 0)
                    {
                        throw new Exception("Already exist data for the request type");
                    }

                    builder = new StringBuilder();
                    builder.Append("select count(1) from SmaxTransaction where Tr_ChId=@StaffId and " +
                        "(Format(Tr_Date,'yyyy-MM-dd') +' ' +Format(Tr_Time,'HH:mm'))=@OutDateTime ");
                    CheckCount = context.Database.SqlQuery<int>(builder.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@OutDateTime", OutDateTime ?? "")).FirstOrDefault();
                    if (CheckCount > 0)
                    {
                        throw new Exception("Already exist for the punch");
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return CheckCount;
        }
        public string ValidateMOffApplication(string StaffId, string FromDate, string ToDate, string LeaveTypeId, bool IsFixed)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            var str = "0";
            if (IsFixed == true)
                str = "1";

            qryStr.Append("SELECT DBO.fnValidateMOffApplication(@StaffId,@FromDate,@ToDate,@LeaveTypeId,@str)");
            var ret = (context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@StaffId", StaffId)
                , new SqlParameter("@FromDate", FromDate), new SqlParameter("@ToDate", ToDate)
                , new SqlParameter("@LeaveTypeId", LeaveTypeId), new SqlParameter("@str", str)).FirstOrDefault()).ToString();
            return ret;
        }

        public string GetLeaveName(string LeaveTypeID)
        {
            var StaffName = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select Name from LeaveType where id = (@LeaveTypeID)");

            try
            {
                StaffName = context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@LeaveTypeID", LeaveTypeID)).FirstOrDefault();
                return StaffName;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ResetPassword(string UserName, string Staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Append("Update AspNetUsers Set PasswordHash = 'ANvPX2ErIY6JQLxQZbVmVLrtgfEyFmj9MbBE7qYyQRwufSxattpdzLQpx1w5CFytyA=='," +
                " SecurityStamp ='eb195c15-3d71-45b9-82fd-6178ebff28a9' " +
                "Where UserName = @UserName and StaffId = @Staffid");
            context.Database.ExecuteSqlCommand(qryStr.ToString(), new SqlParameter("@Staffid", Staffid)
                , new SqlParameter("@UserName", UserName));
        }
        public void DeleteAspNetUsers(string UserName, string Staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Append("Delete from AspNetUsers Where StaffId = @Staffid");
            context.Database.ExecuteSqlCommand(qryStr.ToString(), new SqlParameter("@Staffid", Staffid));
        }
        public void DeleteAtrakUserDetails(string UserName, string Staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Append("Delete from AtrakUserDetails Where StaffId = @Staffid");
            context.Database.ExecuteSqlCommand(qryStr.ToString(), new SqlParameter("@Staffid", Staffid));
        }

        public void ApproveRejectVisitAppointment(string ApproverId, string ApplicationApprovalId, bool Approve)
        {
            string ConStr = string.Empty;
            StringBuilder QryStr = new StringBuilder();
            try
            {
                ConStr = ConfigurationManager.ConnectionStrings["ConVizitas"].ToString();

                if (string.IsNullOrEmpty(ConStr) == true)
                {
                    throw new Exception("Connection string has not been configured.");
                }
            }
            catch (Exception)
            {
                throw;
            }
            QryStr.Clear();
            if (Approve == true)
            {
                QryStr.Append("UPDATE VISITTRANSACTION SET VISITSTATUS = 'open' WHERE SLNO = '" + ApplicationApprovalId + "'");
                VisitAppointment Model = new VisitAppointment();

                //SendEmail(Model, _CMD_);
            }
            else if (Approve == false)
            {
                QryStr.Append("UPDATE VISITTRANSACTION SET VISITSTATUS = 'REJECTED' WHERE SLNO = '" + ApplicationApprovalId + "'");
            }

            using (SqlConnection _CON_ = new SqlConnection(ConStr.ToString()))
            {
                using (SqlCommand _CMD_ = new SqlCommand(QryStr.ToString()))
                {
                    _CMD_.CommandTimeout = 0;
                    _CMD_.CommandType = System.Data.CommandType.Text;
                    _CMD_.Connection = _CON_;
                    _CON_.Open();

                    _CMD_.ExecuteNonQuery();
                }
            }
        }

        public string LeaveBalanceHandler(string ApprovalId, string ApprovalStatus)
        {
            // get the first two letters from the approvalid.
            // if the first two letters are LA then...
            // get the leave details from LeaveApplicationWabco table based on the ApprovalId
            // check if the leave details has been found or not...
            // if the leave details have not been found then...
            // throw exception.
            // check the flag of ApprovalStatus
            // if approved then...
            // get staffid, total number of days, assign leave type id as LV0003 and credit leaves into employee accounting table.
            // if cancelled then...
            // check whether the leave has been debited based on the application id
            // if the application id has been debited then...h
            // get staffid, total number of days, assign leave type id as LV0003 and debit leaves into employee accounting table.
            // if the first two letters are MO then...
            // 
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec spLeaveBalanceHandler @ApprovalId,@ApprovalStatus");
            try
            {
                var lst = (context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@ApprovalId", ApprovalId)
                    , new SqlParameter("@ApprovalStatus", ApprovalStatus)).FirstOrDefault()).ToString();
                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetMasterName(string StaffId, string Mastdt)
        {
            var StaffName = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select dbo.fngetmastername(@StaffId, @Mastdt)");

            try
            {
                StaffName = context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@StaffId", StaffId)
                    , new SqlParameter("@Mastdt", Mastdt)).FirstOrDefault();
                return StaffName;
            }
            catch (Exception)
            {
                throw;
            }
        }

  
        public string ValidateShiftExtension(string StaffId, string Date, string Duration, string BeforeShift, string AfterShift)
        {
            var queryString = new StringBuilder();
            queryString.Append(" Select [Dbo].[ValidateShiftExtension](@StaffId,@Date,@Duration,@BeforeShift,@AfterShift)");
            var msg = (context.Database.SqlQuery<string>(queryString.ToString(), new SqlParameter("@StaffId", StaffId),
                new SqlParameter("@Date", Date), new SqlParameter("@Duration", Duration),
                new SqlParameter("@BeforeShift", BeforeShift), new SqlParameter("@AfterShift", AfterShift)).FirstOrDefault()).ToString();
            return msg;
        }

        public string ValidateApplicationOverlaping(string staffId, DateTime startDate, int startDurationId, DateTime endDate,
           int endDurationId)
        {
            string overlappingValidationMessage = string.Empty;
            SqlParameter[] sqlParameter = new SqlParameter[5];
            sqlParameter[0] = new SqlParameter("@StaffId", staffId);
            sqlParameter[1] = new SqlParameter("@StartDurationId", startDurationId);
            sqlParameter[2] = new SqlParameter("@StartDate", System.Data.SqlDbType.Date);
            sqlParameter[2].Value = startDate.ToString("dd") + "-" + startDate.ToString("MMM") + "-" + startDate.ToString("yyyy");
            sqlParameter[3] = new SqlParameter("@EndDate", System.Data.SqlDbType.Date);
            sqlParameter[3].Value = endDate.ToString("dd") + "-" + endDate.ToString("MMM") + "-" + endDate.ToString("yyyy");
            sqlParameter[4] = new SqlParameter("@EndDurationId", endDurationId);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Clear();
            stringBuilder.Append("Exec [dbo].[IsApplicationOverLapping] @StaffId , @StartDurationId , @StartDate , @EndDate , @EndDurationId  ");
            try
            {
                overlappingValidationMessage = (context.Database.SqlQuery<string>(stringBuilder.ToString(), sqlParameter).FirstOrDefault()).ToString();
                return overlappingValidationMessage;
            }
            catch (Exception err)
            {
                return "ERROR!" + err.Message;
            }
        }

        public string ValidateApplication(string StaffId, string FromDate, string ToDate, string TotalDays, string LeaveTypeId)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("EXEC DBO.[ValidateLeaveApplication] @StaffId, @FromDate,@ToDate,@TotalDays,@LeaveTypeId");
            var ret = (context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@StaffId", StaffId),
                new SqlParameter("@FromDate", FromDate), new SqlParameter("@ToDate", ToDate),
                new SqlParameter("@TotalDays", TotalDays), new SqlParameter("@LeaveTypeId", LeaveTypeId)).FirstOrDefault()).ToString();
            return ret;
        }

        public string ValidateApplicationForPayDate(string StaffId, DateTime startDate, DateTime endDate)
        {

            string payPeriodValidationMessage = string.Empty;
            SqlParameter[] sqlParameter = new SqlParameter[3];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            //sqlParameter[1] = new SqlParameter("@StartDate", startDate);
            //sqlParameter[2] = new SqlParameter("@EndDate", endDate);
            sqlParameter[1] = new SqlParameter("@AppcationStartDate", startDate);
            sqlParameter[2] = new SqlParameter("@AppcationEndDate", endDate);
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append(" Select [dbo].[fnValidateApplicationForPayPeriod](@StaffId,@AppcationStartDate, @AppcationEndDate )");
            payPeriodValidationMessage = (context.Database.SqlQuery<string>(queryString.ToString(), sqlParameter).FirstOrDefault()).ToString();
            return payPeriodValidationMessage;
        }

        public string ValidateDonationApplication(string DonorStaffID, string ReceiverStaffID, string LeaveCount)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT DBO.fnValidateLeaveDonation(@DonorStaffID,@ReceiverStaffID,@LeaveCount)");
            var ret = (context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@DonorStaffID", DonorStaffID)
                , new SqlParameter("@ReceiverStaffID", ReceiverStaffID), new SqlParameter("@LeaveCount", LeaveCount)).FirstOrDefault()).ToString();
            return ret;
        }

        public string GetApprovalStatus(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select ass.Name from approvalstatus ass inner join applicationapproval ap on ap.ApprovalStatusId = ass.Id " +
                "inner join staff st on  @staffid = ap.parentid ");

            try
            {
                var bal = (context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@staffid", staffid)).FirstOrDefault()).ToString();

                if (string.IsNullOrEmpty(bal) == false)
                {
                    return bal;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetScreens(int UserRoleId)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT [dbo].fnGetScreens(" + UserRoleId + ") AS Screens");
            try
            {
                var str = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
                if (str == null)
                {
                    return string.Empty;
                }
                else
                {
                    return str;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public LoggedInUserDetails GetDomainIdBasedDetails(string DomainId)
        {
            LoggedInUserDetails UsrDet = new LoggedInUserDetails();
            try
            {
                var QryStr = new StringBuilder();
                QryStr.Append("SELECT A.UserName,S.Gender,S.DomainId , S.StaffId , dbo.fnGetStaffName(S.StaffId) as " +
                    "[StaffFullName] , S.CompanyId , S.BranchId , S.DeptId as DepartmentId  , " +
                    "S.SecurityGroupId , S.REPORTINGMGRID , DBO.FNGETSTAFFNAME(REPORTINGMGRID) AS REPORTINGMANAGERNAME , " +
                    "S.GradeName , S.CompanyName , S.BranchName , S.DeptName ,S.CategoryId, S.OfficialPhone  FROM  StaffView S left join " +
                    "AtrakUserDetails A on A.StaffId=S.StaffId WHERE S.DomainId = @DomainId");
                UsrDet = context.Database.SqlQuery<LoggedInUserDetails>(QryStr.ToString(), new SqlParameter("@DomainId", DomainId)).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e; ;
            }
            return UsrDet;
        }

        public LoggedInUserDetails GetUserIdBasedDetails(string StaffId)
        {
            LoggedInUserDetails UsrDet = new LoggedInUserDetails();
            try
            {
                var QryStr = new StringBuilder();
                QryStr.Append("SELECT A.UserName,S.Gender, S.DomainId ,  [DBO].[GetStaffEmailId](S.StaffId) as UserEmailId,  " +
                    "[DBO].[GetStaffEmailId](REPORTINGMGRID) as ReportingManagerEmailId , S.StaffId , " +
                    "DBO.FNGETSTAFFNAME(S.StaffId) AS StaffFullName ,  S.CompanyId , S.BranchId , S.DeptId as DepartmentId ,S.LocationId, " +
                    "S.SecurityGroupId , S.REPORTINGMGRID AS ReportingManagerId , DBO.FNGETSTAFFNAME(REPORTINGMGRID) AS ReportingManagerName , " +
                    "S.GradeName , S.CompanyName , S.BranchName , S.DeptName ,S.CategoryId,S.ApproverLevel as ApprovalLevel, S.OFFICIALPHONE ,Approver2 as Approver2Id  FROM  StaffView S " +
                    "left join AtrakUserDetails A on A.StaffId=S.StaffId WHERE S.StaffId = @StaffId");
                UsrDet = context.Database.SqlQuery<LoggedInUserDetails>(QryStr.ToString(), new SqlParameter("@StaffId", StaffId)).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
            return UsrDet;
        }

        public List<RoleList> GetRoleList()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT [Id] , [Name] FROM [SecurityGroup]");

            try
            {
                var lst = context.Database.SqlQuery<RoleList>(qryStr.ToString()).Select(d => new RoleList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<RoleList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<RoleList>();
            }
        }

        public bool IfUserHasLeafs(string StaffId)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select count ( * ) as totalcount from staffofficial where reportingmanager = @StaffId");
            try
            {
                var count = context.Database.SqlQuery<int>(qryStr.ToString(), new SqlParameter("@StaffId", StaffId)).First();
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetReportingManager(string staffid)
        {
            var reportingmanager = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select reportingmgrid from staffview where staffid = @staffid");

            try
            {
                reportingmanager = context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@staffid", staffid)).FirstOrDefault();
                return reportingmanager;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string[] GetAllReportingManager(string staffid)
        {
            string[] reportingmanager = new string[] { };
            var qryStr = new StringBuilder();
            qryStr.Append("Select ReportingManagerId from  [TeamHierarchy] where StaffId= @staffid");

            try
            {
                reportingmanager = context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@staffid", staffid)).ToArray();
                return reportingmanager;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetStaffName(string StaffId)
        {
            var StaffName = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select dbo.fnGetStaffName(@StaffId)");

            try
            {
                StaffName = context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@StaffId", StaffId)).FirstOrDefault();
                return StaffName;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public string GetNewShiftName(string NewShiftId)
        {
            var ShiftName = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("SELECT SHORTNAME + ' - ' + NAME FROM SHIFTS WHERE ID = @NewShiftId");

            try
            {
                ShiftName = context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@NewShiftId", NewShiftId)).FirstOrDefault();
                return ShiftName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ShiftDetailsForShiftChange> GetShiftDetailsForShiftChange(string ShiftId)
        {
            var qryStr = new StringBuilder();

            qryStr.Clear();
            qryStr.Append("Select ShortName,StartTime,EndTime from [Shifts] where Id=@ShiftId");

            try
            {
                var lst = context.Database.SqlQuery<ShiftDetailsForShiftChange>(qryStr.ToString(), new SqlParameter("@ShiftId", ShiftId)).Select(d => new ShiftDetailsForShiftChange()
                {
                    ShortName = d.ShortName,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                }).ToList();

                if (lst == null)
                {
                    return new List<ShiftDetailsForShiftChange>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ShiftDetailsForShiftChange>();
            }
        }
        public string GetSettingsEmailIdOfEmployee()
        {
            var OfficialEmailId = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select SenderEmail from emailsettings");

            try
            {
                OfficialEmailId = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
                return OfficialEmailId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetEmailIdOfEmployee(string StaffId)
        {
            var OfficialEmailId = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select [Email] from staffofficial where staffid = @StaffId");

            try
            {
                OfficialEmailId = context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@StaffId", StaffId)).FirstOrDefault();
                return OfficialEmailId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetEmailIdOfStaff(string StaffId)
        {
            var OfficialEmailId = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select [Email] from staffofficial where staffid = @StaffId");
            try
            {
                OfficialEmailId = context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@StaffId", StaffId)).FirstOrDefault();
                return OfficialEmailId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendEmailMessage(string pFrom, string pTo, string pCC, string pBCC, string EmailSubject, string EmailBody)
        {
            StringBuilder InsQry = new StringBuilder();

            InsQry.Append("INSERT INTO [EmailSendLog] ([From],[To],[CC]," +
                "[BCC],[EmailSubject],[EmailBody]," +
                "[CreatedOn],[CreatedBy],[IsSent]," +
                "[SentOn],[IsError],[ErrorDescription]," +
                "[SentCounter]) VALUES (@pFrom,@pTo,@pCC," +
                "@pBCC,@EmailSubject,@EmailBody," +
                "getdate(),'-',0," +
                "getdate(),0,''," +
                "0)");

            try
            {
                context.Database.ExecuteSqlCommand(InsQry.ToString(), new SqlParameter("@pFrom", pFrom)
                    , new SqlParameter("@pTo", pTo), new SqlParameter("@pCC", pCC)
                    , new SqlParameter("@pBCC", pBCC), new SqlParameter("@EmailSubject", EmailSubject)
                    , new SqlParameter("@EmailBody", EmailBody));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendEmailMessageForApplication(string pFrom, string pTo, string pCC, string pBCC, string EmailSubject, string EmailBody, string CreatedBy)
        {
            try
            {
                EmailSendLog tbl = new EmailSendLog();
                tbl.From = pFrom;
                tbl.To = pTo;
                tbl.CC = pCC;
                tbl.BCC = pBCC;
                tbl.EmailSubject = EmailSubject;
                tbl.EmailBody = EmailBody;
                tbl.CreatedOn = DateTime.Now;
                tbl.CreatedBy = CreatedBy;
                tbl.IsSent = false;
                tbl.SentOn = DateTime.Now;
                tbl.IsError = false;
                tbl.ErrorDescription = "";
                tbl.SentCounter = 0;
                context.EmailSendLog.Add(tbl);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SendEmailToStaff(string ApprovalId)
        {
            string OfficialEmail = string.Empty;
            string StaffName = string.Empty;
            StringBuilder qryStr = new StringBuilder();
            string StatusCaption = string.Empty;
            qryStr.Clear();

            //get the application id and the application type from the application approval table based on the approval id.
            qryStr.Append("select ParentId , ParentType , (SELECT Name FROM ApprovalStatus where  id = A.ApprovalStatusId ) as " +
                "ApprovalStatus from ApplicationApproval A where ParentID = @ApprovalId");

            try
            {
                var data = context.Database.SqlQuery<TempData>(qryStr.ToString(), new SqlParameter("@ApprovalId", ApprovalId)).FirstOrDefault();
                //check whether the application details could be fetched.
                if (data == null) //if the application details could not be fetched then...
                {
                    //throw exception.
                    throw new Exception("No record with id " + ApprovalId + "in application approval could be found.");
                }
                else //if the application details is fetched then...
                {
                    //check the value of application type.
                    if (data.ParentType == "LA") //if the application type is a leave application then...
                    {
                        //get the application details from leaveapplication table based on the 
                        //parent id fetched from appliation approval table.
                        qryStr.Clear();
                        qryStr.Append("select * from RequestApplication where id = @dataParentId");
                        var dataLAW = context.Database.SqlQuery<RequestApplication>(qryStr.ToString(), new SqlParameter("@dataParentId", data.ParentId)).FirstOrDefault();

                        //check if the application details could be fetched.
                        if (dataLAW == null) //if the application details could not be fetched then...
                        {
                            //throw exception.
                            throw new Exception("No leave application with Id " + data.ParentId + " could be found.");
                        }
                        else //if the application details could be fetched then...
                        {
                            //get the name and the official id of the staff.
                            StaffName = GetStaffName(dataLAW.StaffId);
                            OfficialEmail = GetEmailIdOfEmployee(dataLAW.StaffId);
                            if (!string.IsNullOrEmpty(OfficialEmail) == true)
                            {
                                //get the details like the LeaveStartDate and LeaveEndDate and the id of the staff who had applied 
                                //for the leave.

                                qryStr.Clear();
                                qryStr.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your leave application for leave to be taken between " + Convert.ToDateTime(dataLAW.StartDate).ToString("dd-MMM-yyyy") + " and " + Convert.ToDateTime(dataLAW.EndDate).ToString("dd-MMM-yyyy") + " has been " + data.ApprovalStatus + ".</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>");

                                //send the email to the staff intimating that the application is either approved or rejected.
                                SendEmailMessage(string.Empty, OfficialEmail, string.Empty, string.Empty, "Leave application approved", qryStr.ToString());
                            }
                        }
                    }
                    else if (data.ParentType == "RH")
                    {
                        //qryStr.Clear();
                        //qryStr.Append("SELECT A.ID AS RHAPPLICATIONID , STAFFID , RHID , APPLICATIONDATE , ISCANCELLED , NAME , RHDATE , RHYEAR , COMPANYID , LEAVEID FROM RHAPPLICATION A INNER JOIN RESTRICTEDHOLIDAYS B ON A.RHID = B.ID where a.id = '" + data.ParentId + "'");
                        //var dat = context.Database.SqlQuery<RHApplication>(qryStr.ToString()).FirstOrDefault();

                        ////check if the application details could be fetched.
                        //if (dat == null) //if the application details could not be fetched then...
                        //{
                        //    //throw exception.
                        //    throw new Exception("No leave application with Id " + data.ParentId + " could be found.");
                        //}
                        //else //if the application details could be fetched then...
                        //{
                        //    //get the name and the official id of the staff.
                        //    StaffName = GetStaffName(dat.StaffId);
                        //    OfficialEmail = GetEmailIdOfEmployee(dat.StaffId);

                        //    //get the details like the LeaveStartDate and LeaveEndDate and the id of the staff who had applied 
                        //    //for the leave.

                        //    qryStr.Clear();
                        //    qryStr.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your leave application for leave to be taken between " + Convert.ToDateTime(data.LeaveStartDate).ToString("dd-MMM-yyyy") + " and " + Convert.ToDateTime(dataLAW.LeaveEndDate).ToString("dd-MMM-yyyy") + " has been " + data.ApprovalStatus + ".</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>");

                        //    //send the email to the staff intimating that the application is either approved or rejected.
                        //    SendEmailMessage(string.Empty, OfficialEmail, string.Empty, string.Empty, "Leave application approved", qryStr.ToString());
                        //}
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        public string ValidateApplicationForPayDate(string StaffId, string ApplicationStartDate, string ApplicationEndDate)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();

                SqlParameter[] sqlParameter = new SqlParameter[3];
                sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
                sqlParameter[1] = new SqlParameter("@ApplicationStartDate", ApplicationStartDate);
                sqlParameter[2] = new SqlParameter("@ApplicationEndDate", ApplicationEndDate);
                stringBuilder.Append(" Select [dbo].[fnValidateApplicationForPayPeriod]" +
                    "(@StaffId ,@ApplicationStartDate, @ApplicationEndDate)");
                var msg = (context.Database.SqlQuery<string>(stringBuilder.ToString(), sqlParameter).FirstOrDefault()).ToString();
                return msg;
            }
            catch (Exception err)
            {
                throw err;
            }
        }


        public void ApplicationApprovalRejection(string ApproverId, string ApplicationApprovalId, bool Approve)
        {
            var queryString = new StringBuilder();
            string LocationId = "LO0001";
            queryString.Clear();
            var QryStr = new StringBuilder();
            string ReferenceTable = string.Empty;
            string staffId = string.Empty;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            string applicationId = string.Empty;
            string applicationType = string.Empty;

            if (ApplicationApprovalId.StartsWith("MO"))
            {
                ReferenceTable = "MaintenanceOff";
            }

            else if (ApplicationApprovalId.StartsWith("LO"))
            {
                ReferenceTable = "LATEROFF";
            }
            else if (ApplicationApprovalId.StartsWith("RH"))
            {
                ReferenceTable = "RHAPPLICATION";
                queryString.Append(" Select StaffId , ApplicationDate as FromDate , ApplicationDate as ToDate from [RHApplication] where Id =  @ApplicationApprovalId  ");

                try
                {
                    var data = context.Database.SqlQuery<ACTList>(queryString.ToString() , new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).Select(d => new ACTList()
                    {
                        StaffId = d.StaffId,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate
                    }).FirstOrDefault();

                    staffId = data.StaffId;
                    fromDate = data.FromDate;
                    toDate = data.ToDate;
                    applicationType = "RH";
                }
                catch (Exception err)
                {
                    throw err;
                }
                if (fromDate < currentDate)
                {
                    if (toDate >= currentDate)
                    {
                        toDate = DateTime.Now.AddDays(-1);
                    }
                    LogIntoIntoAttendanceControlTable(staffId, fromDate, toDate, applicationType, ApplicationApprovalId);
                }
            }
            else if (ApplicationApprovalId.StartsWith("OT"))
            {
                ReferenceTable = "OTApplication";
                queryString.Append(" Select StaffId , OTDate as FromDate , OTDate as ToDate from [OTApplication] where Id = '" + ApplicationApprovalId + "' ");

                try
                {

                    var data = context.Database.SqlQuery<ACTList>(queryString.ToString()).Select(d => new ACTList()
                    {
                        StaffId = d.StaffId,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate
                    }).FirstOrDefault();

                    staffId = data.StaffId;
                    fromDate = data.FromDate;
                    toDate = data.ToDate;
                    applicationType = "OT";
                }
                catch (Exception err)
                {
                    throw err;
                }
                if (fromDate < currentDate)
                {
                    if (toDate >= currentDate)
                    {
                        toDate = DateTime.Now.AddDays(-1);
                    }
                    LogIntoIntoAttendanceControlTable(staffId, fromDate, toDate, applicationType, ApplicationApprovalId);
                }

            }
            else if (ApplicationApprovalId.StartsWith("SC"))
            {

                ReferenceTable = "ShiftChangeApplication";
                queryString.Append(" Select StaffId , FromDate , ToDate from [ShiftChangeApplication] where Id = '" + ApplicationApprovalId + "' ");

                try
                {

                    var data = context.Database.SqlQuery<ACTList>(queryString.ToString()).Select(d => new ACTList()
                    {
                        StaffId = d.StaffId,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate
                    }).FirstOrDefault();

                    staffId = data.StaffId;
                    fromDate = data.FromDate;
                    toDate = data.ToDate;
                    applicationType = "SC";
                }
                catch (Exception err)
                {
                    throw err;
                }
                if (fromDate < currentDate)
                {
                    if (toDate >= currentDate)
                    {
                        toDate = DateTime.Now.AddDays(-1);
                    }
                    LogIntoIntoAttendanceControlTable(staffId, fromDate, toDate, applicationType, ApplicationApprovalId);
                }
            }


            QryStr.Clear();
            //check the approval status
            if (Approve == false) // if the approval has to be rejected then...
            {
                if (!ApplicationApprovalId.StartsWith("V") && !ApplicationApprovalId.StartsWith("SR"))
                {
                    QryStr.Clear();
                    QryStr.Append("SELECT CONVERT ( VARCHAR , ISCANCELLED ) AS ISCANCELLED FROM " + ReferenceTable + " WHERE id = '" + ApplicationApprovalId + "'");
                    var res1 = context.Database.SqlQuery<string>(QryStr.ToString()).FirstOrDefault();

                    if (res1.Equals("1"))
                    {
                        throw new Exception("Cannot reject a cancelled application.");
                    }
                }
                //get the current status of the application.
                QryStr.Clear();
                QryStr.Append("SELECT CONVERT ( VARCHAR , APPROVALSTATUSID ) AS APPROVALSTATUSID FROM APPLICATIONAPPROVAL WHERE PARENTID = '" + ApplicationApprovalId + "'");
                var res2 = context.Database.SqlQuery<string>(QryStr.ToString()).FirstOrDefault();
                //check the current status of the application.
                if (res2.ToString() == "1") //if the current status is pending then...
                {
                    //reject the application
                    //set the approval status.
                    QryStr.Clear();
                    QryStr.Append("update ApplicationApproval Set ApprovalStatusId = 3 , ApprovedBy = '" + ApproverId + "' , ApprovedOn = GetDate() , Comment = 'REJECTED' where ParentId = '" + ApplicationApprovalId + "'");
                    context.Database.ExecuteSqlCommand(QryStr.ToString());
                }
                else if (res2.ToString() == "2")
                {
                    throw new Exception("Cannot reject already approved application.");
                }
                else if (res2.ToString() == "3")
                {
                    throw new Exception("Cannot reject already rejected application.");
                }
            }
            else if (Approve == true)
            {
                using (var Trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        var str = "OK.";

                        if (!ApplicationApprovalId.StartsWith("V") && !ApplicationApprovalId.StartsWith("SR"))
                        {
                            QryStr.Clear();
                            QryStr.Append("SELECT CONVERT ( VARCHAR , ISCANCELLED ) AS ISCANCELLED FROM  " + ReferenceTable + "  WHERE id = '" + ApplicationApprovalId + "'");
                            var res1 = context.Database.SqlQuery<string>(QryStr.ToString()).FirstOrDefault();

                            if (res1.Equals("1"))
                            {
                                throw new Exception("Cannot approve a cancelled application.");
                            }
                        }

                        //get the current status of the application.
                        QryStr.Clear();
                        QryStr.Append("SELECT CONVERT ( VARCHAR , APPROVALSTATUSID ) AS APPROVALSTATUSID FROM APPLICATIONAPPROVAL  WHERE PARENTID = '" + ApplicationApprovalId + "'");
                        var res2 = context.Database.SqlQuery<string>(QryStr.ToString()).FirstOrDefault();
                        //check the current status of the application.
                        if (res2.ToString() == "1") //if the current status is pending then...
                        {
                            if (ApplicationApprovalId.StartsWith("MO") == true)
                            {
                                str = LeaveBalanceHandler(ApplicationApprovalId, "Approve");
                            }
                            else if (ApplicationApprovalId.StartsWith("V") == true)
                            {
                                ApproveRejectVisitAppointment(ApproverId, ApplicationApprovalId, Approve);
                            }
                            else if (ApplicationApprovalId.StartsWith("SR") == true)
                            {
                                StaffRepository sr = new StaffRepository();
                                sr.UpdateStaffInformation(ApplicationApprovalId);
                            }


                            //check if the above function returned "OK." flag.
                            if (str != "OK.") //if it does not return OK flag then...
                            {
                                //throw exception which lands the program control in exception block.
                                throw new Exception(str);
                            }

                            QryStr.Append("update ApplicationApproval Set ApprovalStatusId = 2 , ApprovedBy = '" + ApproverId + "' , ApprovedOn = GetDate() , Comment = 'APPROVED' where ParentId = '" + ApplicationApprovalId + "'");
                            context.Database.ExecuteSqlCommand(QryStr.ToString());
                            //Manulpunchdatasendtosmax
                            if (ApplicationApprovalId.StartsWith("MP") == true)
                            {
                                queryString.Clear();
                                queryString.Append("Select Id,StaffId,indatetime,outdatetime,punchtype from ManualPunch where Id = '" + ApplicationApprovalId + "' ");
                                try
                                {

                                    var data = context.Database.SqlQuery<Manualpunchforsmax>(queryString.ToString()).Select(d => new Manualpunchforsmax()
                                    {

                                        StaffId = d.StaffId,
                                        Indatetime = d.Indatetime,
                                        Outdatetime = d.Outdatetime,
                                        PunchType = d.PunchType

                                    }).FirstOrDefault();

                                    string Indatetime;
                                    string outdatetime;
                                    Indatetime = DateTime.ParseExact(data.Indatetime.ToString(), "dd/MM/yyyy HH:mm:ss", null).ToString("dd/MMM/yyyy HH:mm:ss");
                                    outdatetime = DateTime.ParseExact(data.Outdatetime.ToString(), "dd/MM/yyyy HH:mm:ss", null).ToString("dd/MMM/yyyy HH:mm:ss");

                                    if (data.PunchType == "IN")
                                    {

                                        SaveInPunch(Indatetime, data.StaffId, LocationId);
                                    }
                                    else if (data.PunchType == "OUT")
                                    {
                                        SaveOutPunch(outdatetime, data.StaffId, LocationId);
                                    }
                                    else
                                    {
                                        SaveInOutPunch(Indatetime, outdatetime, data.StaffId, LocationId);
                                    }

                                }
                                catch (Exception e)
                                {
                                    throw e;
                                }
                            }
                            if (ApplicationApprovalId.StartsWith("SC") == true)
                            {

                                var ShiftName = string.Empty;
                                var ShiftShortName = string.Empty;
                                DateTime StartTime = DateTime.Now;
                                DateTime EndTime = DateTime.Now;
                                TimeSpan ExpectedWorkingHours;
                                Int64 Id = 0;
                                int IsWeeklyOff = 0;

                                var queryStringSC = new StringBuilder();
                                queryStringSC.Clear();
                                queryStringSC.Append("Select *  from ShiftChangeApplication where Id = '" + ApplicationApprovalId + "' ");
                                try
                                {

                                    var data = context.Database.SqlQuery<ShiftChangeApplication>(queryStringSC.ToString()).Select(d => new ShiftChangeApplication()
                                    {
                                        Id = d.Id,
                                        StaffId = d.StaffId,
                                        FromDate = d.FromDate,
                                        ToDate = d.ToDate,
                                        NewShiftId = d.NewShiftId,
                                        CreatedOn = d.CreatedOn,
                                        CreatedBy = d.CreatedBy,
                                        Reason = d.Reason,
                                        IsCancelled = d.IsCancelled
                                    }).FirstOrDefault();
                                    if (data.NewShiftId == "LV0011")
                                    {
                                        ShiftName = "WO";
                                        ShiftShortName = "WO";
                                        StartTime = Convert.ToDateTime("1900-01-01 00:00:00.000");
                                        EndTime = Convert.ToDateTime("1900-01-01 00:00:00.000");
                                        ExpectedWorkingHours = EndTime - StartTime;
                                        IsWeeklyOff = 1;
                                    }
                                    else
                                    {

                                        var lst = new List<ShiftDetailsForShiftChange>();
                                        lst = GetShiftDetailsForShiftChange(data.NewShiftId).ToList();

                                        if (lst != null)
                                        {
                                            foreach (var rec in lst)
                                            {
                                                ShiftName = rec.Name;
                                                ShiftShortName = rec.ShortName;
                                                StartTime = rec.StartTime;
                                                EndTime = rec.EndTime;
                                            }
                                        }

                                        ExpectedWorkingHours = EndTime - StartTime;
                                        if (EndTime > StartTime)
                                        {
                                            ExpectedWorkingHours = EndTime - StartTime;
                                        }
                                        else
                                        {
                                            var EndTime1 = EndTime.AddDays(1);
                                            ExpectedWorkingHours = EndTime1 - StartTime;
                                        }
                                        var startTime = StartTime.ToString("hh:mm:tt");
                                        var endTime = EndTime.ToString("hh:mm:tt");
                                    }

                                    var repo1 = new ShiftChangeRepository();
                                    double count = (Convert.ToDateTime(data.ToDate) - Convert.ToDateTime(data.FromDate)).TotalDays;
                                    var dates = new List<DateTime>();
                                    DateTime defaultToDate = DateTime.Now.AddDays(-1);
                                    DateTime expectedWorkingHours = Convert.ToDateTime("1900-01-01 00:00:00.000");

                                    for (var dt = Convert.ToDateTime(data.FromDate); dt <= Convert.ToDateTime(data.ToDate); dt = dt.AddDays(1))
                                    {
                                        dates.Add(dt);
                                    }
                                    foreach (var date in dates)
                                    {

                                        DateTime ShiftOutDate = date;
                                        if (EndTime < StartTime)
                                        {
                                            ShiftOutDate = date.AddDays(1);
                                        }
                                        var QryStr1 = new StringBuilder();
                                        QryStr1.Clear();
                                        QryStr1.Append("Select Id from AttendanceData where StaffId='" + data.StaffId + "' AND CONVERT ( DATETIME , CONVERT ( VARCHAR , ShiftInDate , 106 ) )='" + date.ToString("dd-MMM-yyyy") + "' ");

                                        try
                                        {
                                            Id = context.Database.SqlQuery<Int64>(QryStr1.ToString()).FirstOrDefault();
                                        }
                                        catch (Exception)
                                        {
                                            throw;
                                        }
                                        if (Id != 0)
                                        {
                                            var QryStr2 = new StringBuilder();
                                            QryStr2.Clear();
                                            QryStr2.Append("Update AttendanceData set ShiftId='" + data.NewShiftId + "' , ShiftShortName='" + ShiftShortName + "' ," +
                                                " ShiftInDate = '" + date.ToString("yyyy-MM-dd") + "', ShiftInTime = '" + StartTime.ToString("yyyy-MM-dd HH:mm:ss:ms") + "' ," +
                                                " ShiftOutDate = '" + ShiftOutDate.ToString("yyyy-MM-dd") + "', IsWeeklyOff='" + IsWeeklyOff + "'," +
                                                "ShiftOutTime = '" + EndTime.ToString("yyyy-MM-dd HH:mm:ss:ms") + "', ExpectedWorkingHours = '" + ExpectedWorkingHours + "', " +
                                                " [IsProcessed] = 0 where  Id= '" + Id + "' AND  StaffId='" + data.StaffId + "'");
                                            context.Database.ExecuteSqlCommand(QryStr2.ToString());
                                        }

                                        if (Id == 0)
                                        {
                                            var QryStr3 = new StringBuilder();
                                            QryStr3.Clear();
                                            QryStr3.Append(" Insert into AttendanceData([StaffId],[ShiftId],[ShiftShortName],[ShiftInDate],[ShiftInTime]," +
                                            " [ShiftOutDate],  [ShiftOutTime],[ExpectedWorkingHours],[IsEarlyComing],[IsEarlyComingValid],[IsLateComing]," +
                                           "  [IsLateComingValid],[IsEarlyGoing], [IsEarlyGoingValid],[IsLateGoing],[IsLateGoingValid],[IsOT],[IsOTValid]," +
                                            " [IsManualPunch],[IsSinglePunch],[IsIncorrectPunches],[IsDisputed],[OverRideEarlyComing]," +
                                            " [OverRideLateComing],[OverRideEarlyGoing],[OverRideLateGoing],[OverRideOT],[AttendanceStatus]," +
                                            " [FHStatus],[SHStatus],[AbsentCount],[DayAccount],[IsLeave],[IsLeaveValid],[IsLeaveWithWages],[IsAutoShift]," +
                                            " [IsWeeklyOff],[IsPaidHoliday],[IsProcessed]) VALUES ('" + data.StaffId + "','" + data.NewShiftId + "','" + ShiftShortName + "'," +
                                            " '" + date.ToString("yyyy-MM-dd") + "','" + StartTime.ToString("yyyy-MM-dd HH:mm:ss:ms") + "','" + ShiftOutDate.ToString("yyyy-MM-dd") + "'," +
                                            " '" + EndTime.ToString("yyyy-MM-dd HH:mm:ss:ms") + "' , '" + ExpectedWorkingHours + "',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0," +
                                            " 0,0,0,0,'-','-','-',0,0,0,0,0,0,'" + IsWeeklyOff + "',0,0)");

                                            context.Database.ExecuteSqlCommand(QryStr3.ToString());
                                        }
                                    }

                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                                //SEND APPROVAL/REJECTION EMAIL TO THE REQUESTER.

                            }
                            SendEmailToStaff(ApplicationApprovalId);

                        }
                        else if (res2.ToString() == "2")
                        {
                            throw new Exception("Cannot approve already approved application.");
                        }
                        else if (res2.ToString() == "3")
                        {
                            throw new Exception("Cannot approve already rejected application.");
                        }

                        Trans.Commit();
                    }

                    catch (Exception)
                    {
                        Trans.Rollback();
                        throw;
                    }
                }
            }
        }


        public void ApplicationApprovalRejection(string ApproverId, string ApplicationApprovalId, bool Approve, string ParentType, string LocationId)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            var QryStr = new StringBuilder();
            var QryStr1 = new StringBuilder();
            string ReferenceTable = string.Empty;
            string staffId = string.Empty;
            string actionuser = string.Empty;
            string LeaveCount = string.Empty;
            string Narration = string.Empty;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            string applicationId = string.Empty;
            string applicationType = string.Empty;
            string WorkedDate = string.Empty;
            string ReprocessAttendanceForPriorApplications = string.Empty;
            if (ParentType.StartsWith("LA"))
            {
                ParentType = "LA";
                ReferenceTable = "RequestApplication";
                queryString.Append("Select StaffId ,StartDate as FromDate , EndDate as ToDate from RequestApplication" +
                    " where Id = @ApplicationApprovalId");
                try
                {

                    var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).Select(d => new ACTList()
                    {
                        StaffId = d.StaffId,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate
                    }).FirstOrDefault();

                    staffId = data.StaffId;
                    fromDate = data.FromDate;
                    toDate = data.ToDate;
                    applicationType = "LA";
                    applicationId = ApplicationApprovalId;
                    if (Approve == true)
                    {
                        string payValidationMessage = string.Empty;

                        payValidationMessage = ValidateApplicationForPayDate(data.StaffId, Convert.ToString(data.FromDate.ToString("yyyy-MM-dd")), Convert.ToString(data.ToDate.ToString("yyyy-MM-dd")));
                        if (payValidationMessage == "INVALID")
                        {
                            throw new Exception("You cannot approve the applications of past pay period");
                        }
                    }
                }
                catch (Exception err)
                {
                    throw err;
                }

                try
                {
                    ReprocessAttendanceForPriorApplications = ConfigurationManager.AppSettings["ReprocessAttendanceForPriorApplications"].ToString();
                }
                catch
                {
                    ReprocessAttendanceForPriorApplications = "No";
                }
                if (ReprocessAttendanceForPriorApplications.Trim().ToUpper().Equals("YES"))
                {

                    LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationApprovalId);
                }
                else
                {
                    if (fromDate.Date < currentDate.Date)
                    {
                        if (toDate.Date >= currentDate.Date)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationApprovalId);
                    }
                }
            }
            else if (ApplicationApprovalId.StartsWith("MO"))
            {
                ParentType = "MO";
                ReferenceTable = "MaintenanceOff";
            }
            else if (ParentType.StartsWith("MP"))
            {
                ReferenceTable = "RequestApplication";
                ParentType = "MP";
                queryString.Append(" Select StaffId , StartDate as FromDate , EndDate as ToDate from RequestApplication" +
                    " where Id = @ApplicationApprovalId");
                try
                {
                    var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).Select(d => new ACTList()
                    {
                        StaffId = d.StaffId,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate
                    }).FirstOrDefault();
                    staffId = data.StaffId;
                    fromDate = data.FromDate;
                    toDate = data.ToDate;
                    applicationType = "MP";
                    if (Approve == true)
                    {
                        string payValidationMessage = string.Empty;
                        payValidationMessage = ValidateApplicationForPayDate(data.StaffId, Convert.ToString(data.FromDate.ToString("yyyy-MM-dd")), Convert.ToString(data.ToDate.ToString("yyyy-MM-dd")));
                        if (payValidationMessage == "INVALID")
                        {
                            throw new Exception("You cannot approve the applications of past pay period");
                        }
                    }
                }
                catch (Exception err)
                {
                    throw err;
                }
                if (fromDate.Date < currentDate.Date)
                {
                    if (toDate.Date >= currentDate.Date)
                    {
                        toDate = DateTime.Now.AddDays(-1);
                    }
                    LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationApprovalId);
                }
            }
            else if (ParentType.StartsWith("PO"))
            {
                ReferenceTable = "RequestApplication";
                ParentType = "PO";
                queryString.Append(" Select StaffId , StartDate as FromDate , EndDate as ToDate from" +
                    " RequestApplication where Id  = @ApplicationApprovalId");
                try
                {
                    var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).Select(d => new ACTList()
                    {
                        StaffId = d.StaffId,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate
                    }).FirstOrDefault();

                    staffId = data.StaffId;
                    fromDate = data.FromDate;
                    toDate = data.ToDate;
                    applicationType = "PO";
                    if (Approve == true)
                    {
                        string payValidationMessage = string.Empty;
                        payValidationMessage = ValidateApplicationForPayDate(data.StaffId, Convert.ToString(data.FromDate.ToString("yyyy-MM-dd")), Convert.ToString(data.ToDate.ToString("yyyy-MM-dd")));
                        if (payValidationMessage == "INVALID")
                        {
                            throw new Exception("You cannot approve the applications of past pay period");
                        }
                    }
                }
                catch (Exception err)
                {
                    throw err;
                }
                if (fromDate.Date < currentDate.Date)
                {
                    if (toDate.Date >= currentDate.Date)
                    {
                        toDate = DateTime.Now.AddDays(-1);
                    }
                    LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationApprovalId);
                }


            }
            else if (ApplicationApprovalId.StartsWith("LO"))
            {
                ReferenceTable = "LATEROFF";
                ParentType = "LO";
            }
            else if (ParentType.StartsWith("OD") || ParentType.StartsWith("BT") || ParentType.StartsWith("WFH"))
            {
                ReferenceTable = "RequestApplication";
                queryString.Append(" Select StaffId , StartDate as FromDate , EndDate as ToDate from RequestApplication" +
                    " where Id = @ApplicationApprovalId");
                try
                {
                    var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).Select(d => new ACTList()
                    {
                        StaffId = d.StaffId,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate
                    }).FirstOrDefault();

                    staffId = data.StaffId;
                    fromDate = data.FromDate;
                    toDate = data.ToDate;
                    applicationType = "OD";
                    if (Approve == true)
                    {
                        string payValidationMessage = string.Empty;
                        payValidationMessage = ValidateApplicationForPayDate(data.StaffId, Convert.ToString(data.FromDate.ToString("yyyy-MM-dd")), Convert.ToString(data.ToDate.ToString("yyyy-MM-dd")));
                        if (payValidationMessage == "INVALID")
                        {
                            throw new Exception("You cannot approve the applications of past pay period");
                        }
                    }
                }
                catch (Exception err)
                {
                    throw err;
                }
                try
                {
                    ReprocessAttendanceForPriorApplications = ConfigurationManager.AppSettings["ReprocessAttendanceForPriorApplications"].ToString();
                }
                catch
                {
                    ReprocessAttendanceForPriorApplications = "No";
                }
                if (ReprocessAttendanceForPriorApplications.Trim().ToUpper().Equals("YES"))
                {

                    LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationApprovalId);
                }
                else
                {
                    if (fromDate.Date < currentDate.Date)
                    {
                        if (toDate.Date >= currentDate.Date)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationApprovalId);
                    }
                }

            }
            else if (ParentType.StartsWith("CR"))
            {
                ReferenceTable = "RequestApplication";
                ParentType = "CR";
                queryString.Append(" Select Id,StaffId ,StartDate as FromDate , EndDate as ToDate,TotalDays from RequestApplication" +
                    " where Id = @ApplicationApprovalId ");
                try
                {
                    var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).Select(d => new ACTList()
                    {
                        StaffId = d.StaffId,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate,
                        //TotalDays = d.TotalDays,
                        //Id = d.Id
                    }).FirstOrDefault();

                    staffId = data.StaffId;
                    fromDate = data.FromDate;
                    toDate = data.ToDate;
                    applicationType = "CR";
                    applicationId = ApplicationApprovalId;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
            else if (ParentType.StartsWith("CO"))
            {
                ReferenceTable = "RequestApplication";
                ParentType = "CO";
                queryString.Append(" Select Id,StaffId , StartDate as FromDate , EndDate as ToDate ,WorkedDate from " +
                    "RequestApplication where Id = @ApplicationApprovalId ");
                try
                {
                    var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).Select(d => new ACTList()
                    {
                        StaffId = d.StaffId,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate,
                        //WorkedDate = d.WorkedDate,
                        //Id = d.Id
                    }).FirstOrDefault();

                    staffId = data.StaffId;
                    fromDate = data.FromDate;
                    toDate = data.ToDate;
                    // WorkedDate = data.WorkedDate;
                    applicationType = "CO";
                    if (Approve == true)
                    {
                        string payValidationMessage = string.Empty;
                        payValidationMessage = ValidateApplicationForPayDate(data.StaffId, Convert.ToString(data.FromDate.ToString("yyyy-MM-dd")), Convert.ToString(data.ToDate.ToString("yyyy-MM-dd")));
                        if (payValidationMessage == "INVALID")
                        {
                            throw new Exception("You cannot approve the applications of past pay period");
                        }
                    }
          
                }
                catch (Exception err)
                {
                    throw err;
                }
                try
                {
                    ReprocessAttendanceForPriorApplications = ConfigurationManager.AppSettings["ReprocessAttendanceForPriorApplications"].ToString();
                }
                catch
                {
                    ReprocessAttendanceForPriorApplications = "No";
                }
                if (ReprocessAttendanceForPriorApplications.Trim().ToUpper().Equals("YES"))
                {

                    LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationApprovalId);
                }
                else
                {
                    if (fromDate.Date < currentDate.Date)
                    {
                        if (toDate.Date >= currentDate.Date)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationApprovalId);
                    }
                }
            }

            else if (ParentType.StartsWith("SC"))
            {
                ReferenceTable = "RequestApplication";
                ParentType = "SC";
                queryString.Append(" Select StaffId , StartDate as FromDate , EndDate as ToDate from " +
                    "RequestApplication where Id = @ApplicationApprovalId");

                try
                {
                    var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).Select(d => new ACTList()
                    {
                        StaffId = d.StaffId,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate
                    }).FirstOrDefault();

                    staffId = data.StaffId;
                    fromDate = data.FromDate;
                    toDate = data.ToDate;
                    applicationType = "SC";
                    if (Approve == true)
                    {
                        string payValidationMessage = string.Empty;
                        payValidationMessage = ValidateApplicationForPayDate(data.StaffId, Convert.ToString(data.FromDate.ToString("yyyy-MM-dd")), Convert.ToString(data.ToDate.ToString("yyyy-MM-dd")));
                        if (payValidationMessage == "INVALID")
                        {
                            throw new Exception("You cannot approve the applications of past pay period");
                        }
                    }
                }
                catch (Exception err)
                {
                    throw err;
                }
                if (fromDate.Date < currentDate.Date)
                {
                    if (toDate.Date >= currentDate.Date)
                    {
                        toDate = DateTime.Now.AddDays(-1);
                    }
                    LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationApprovalId);
                }
            }
            if (string.IsNullOrEmpty(ParentType).Equals(false) && ParentType.Equals("SE"))
            {

                ReferenceTable = "RequestApplication";
                queryString.Append(" Select StaffId , StartDate as FromDate , EndDate as ToDate from " +
                    "RequestApplication where Id = @ApplicationApprovalId");

                try
                {

                    var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).Select(d => new ACTList()
                    {
                        StaffId = d.StaffId,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate
                    }).FirstOrDefault();

                    staffId = data.StaffId;
                    fromDate = data.FromDate;
                    toDate = data.ToDate;
                    applicationType = "SE";
                    if (Approve == true)
                    {
                        string payValidationMessage = string.Empty;
                        payValidationMessage = ValidateApplicationForPayDate(data.StaffId, Convert.ToString(data.FromDate.ToString("yyyy-MM-dd")), Convert.ToString(data.ToDate.ToString("yyyy-MM-dd")));
                        if (payValidationMessage == "INVALID")
                        {
                            throw new Exception("You cannot approve the applications of past pay period");
                        }
                    }
                }
                catch (Exception err)
                {
                    throw err;
                }
                if (fromDate.Date < currentDate.Date)
                {
                    if (toDate.Date >= currentDate.Date)
                    {
                        toDate = DateTime.Now.AddDays(-1);
                    }
                    LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationApprovalId);
                }
            }
            else if (string.IsNullOrEmpty(ParentType).Equals(false) && ParentType.Equals("HW"))
            {
                ReferenceTable = "RequestApplication";
                queryString.Append(" Select StaffId , StartDate as FromDate , EndDate as ToDate from " +
                    "RequestApplication where Id = @ApplicationApprovalId");
                try
                {
                    var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).Select(d => new ACTList()
                    {
                        StaffId = d.StaffId,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate
                    }).FirstOrDefault();

                    staffId = data.StaffId;
                    fromDate = data.FromDate;
                    toDate = data.ToDate;
                    applicationType = "HW";
                    if (Approve == true)
                    {
                        string payValidationMessage = string.Empty;
                        payValidationMessage = ValidateApplicationForPayDate(data.StaffId, Convert.ToString(data.FromDate.ToString("yyyy-MM-dd")), Convert.ToString(data.ToDate.ToString("yyyy-MM-dd")));
                        if (payValidationMessage == "INVALID")
                        {
                            throw new Exception("You cannot approve the applications of past pay period");
                        }
                    }
                }
                catch (Exception err)
                {
                    throw err;
                }
                if (fromDate.Date < currentDate.Date)
                {
                    if (toDate.Date >= currentDate.Date)
                    {
                        toDate = DateTime.Now.AddDays(-1);
                    }
                    LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationApprovalId);
                }
            }
            QryStr.Clear();
            //check the approval status
            if (Approve == false) // if the approval has to be rejected then...
            {
                if (!ApplicationApprovalId.StartsWith("V") && !ApplicationApprovalId.StartsWith("SR") && ParentType != "SE" && ParentType != "HW")
                {
                    QryStr.Clear();
                    QryStr.Append("SELECT CONVERT ( VARCHAR , ISCANCELLED ) AS ISCANCELLED FROM " + ReferenceTable + " WHERE id = @ApplicationApprovalId");
                    var res1 = context.Database.SqlQuery<string>(QryStr.ToString(), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).FirstOrDefault();

                    if (res1.Equals("1"))
                    {
                        throw new Exception("Cannot reject a cancelled application.");
                    }

                }
                //get the current status of the application.
                QryStr.Clear();
                QryStr.Append("SELECT CONVERT ( VARCHAR , APPROVALSTATUSID ) AS APPROVALSTATUSID FROM APPLICATIONAPPROVAL WHERE PARENTID = @ApplicationApprovalId");
                var res2 = context.Database.SqlQuery<string>(QryStr.ToString(), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).FirstOrDefault();
                //check the current status of the application.
                if (res2.ToString().Equals("1"))//if the current status is pending then...
                {
                    //reject the application
                    //set the approval status.
                    QryStr.Clear();
                    if (ParentType.Equals("SE") || ParentType.Equals("HW"))
                    {
                        QryStr.Append("update ApplicationApproval Set ApprovalStatusId = 3 , ApprovedBy = @ApproverId , ApprovedOn = GetDate() ," +
                                       " Comment = 'REJECTED' where ParentType = @ParentType And ParentId = @ApplicationApprovalId");

                    }
                    else
                    {
                        QryStr.Append("update ApplicationApproval Set ApprovalStatusId = 3 , ApprovedBy = @ApproverId , ApprovedOn = GetDate() ," +
                                      " Comment = 'REJECTED' where ParentId = @ApplicationApprovalId");
                    }
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@ApproverId", ApproverId)
                        , new SqlParameter("@ParentType", ParentType), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId));
                }
                else if (res2.ToString().Equals("2"))
                {
                    throw new Exception("Cannot reject already approved application.");
                }
                else if (res2.ToString().Equals("3"))
                {
                    throw new Exception("Cannot reject already rejected application.");
                }
                SendEmailToStaff(ApplicationApprovalId);
            }
            else if (Approve == true)
            {
                using (var Trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        var str = "OK.";

                        if (!ApplicationApprovalId.StartsWith("V") && !ApplicationApprovalId.StartsWith("SR") && ParentType != "SE" && ParentType != "HW")
                        {
                            QryStr.Clear();
                            QryStr.Append("SELECT CONVERT ( VARCHAR , ISCANCELLED ) AS ISCANCELLED FROM  " + ReferenceTable + "  WHERE id = @ApplicationApprovalId");
                            var res1 = context.Database.SqlQuery<string>(QryStr.ToString(), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).FirstOrDefault();

                            if (res1.Equals("1"))
                            {
                                throw new Exception("Cannot approve a cancelled application.");
                            }
                        }

                        //get the current status of the application.
                        QryStr.Clear();
                        QryStr.Append("SELECT CONVERT ( VARCHAR , APPROVALSTATUSID ) AS APPROVALSTATUSID FROM APPLICATIONAPPROVAL  WHERE PARENTID = @ApplicationApprovalId");
                        var res2 = context.Database.SqlQuery<string>(QryStr.ToString(), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).FirstOrDefault();

                        QryStr.Clear();
                        QryStr.Append("SELECT CONVERT ( VARCHAR , Approval2statusId ) AS Approval2statusId FROM APPLICATIONAPPROVAL  WHERE PARENTID = @ApplicationApprovalId");
                        var res3 = context.Database.SqlQuery<string>(QryStr.ToString(), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).FirstOrDefault();
                        //check the current status of the application.
                        if (res2.ToString() == "1" || res3.ToString() == "1") //if the current status is pending then...
                        {
                            if (ParentType.StartsWith("LA") == true)
                            {
                                //function call to handle the leave balances.
                                str = LeaveBalanceHandler(ApplicationApprovalId, "Approve");
                            }
                            else if (ParentType.StartsWith("MO") == true)
                            {
                                str = LeaveBalanceHandler(ApplicationApprovalId, "Approve");
                            }
                            else if (ParentType.StartsWith("V") == true)
                            {
                                ApproveRejectVisitAppointment(ApproverId, ApplicationApprovalId, Approve);
                            }

                            else if (ParentType.StartsWith("SR") == true)
                            {
                                StaffRepository sr = new StaffRepository();
                                sr.UpdateStaffInformation(ApplicationApprovalId);
                            }
                            else if (ParentType.StartsWith("CR") == true)
                            {
                                str = LeaveBalanceHandler(ApplicationApprovalId, "Approve");
                            }
                            else if (ParentType.StartsWith("CO") == true)
                            {
                                str = LeaveBalanceHandler(ApplicationApprovalId, "Approve");
                            }
                            //check if the above function returned "OK." flag.
                            if (str != "OK.") //if it does not return OK flag then...
                            {
                                //throw exception which lands the program control in exception block.
                                throw new Exception(str);
                            }

                            if (ParentType.Equals("LA") || ParentType.Equals("PO") || ParentType.Equals("MP") || ParentType.Equals("OD")
                                || ParentType.Equals("BT") || ParentType.Equals("WFH") || ParentType.Equals("CR") || ParentType.Equals("CO")
                                || ParentType.Equals("HW") || ParentType.Equals("SC") || ParentType.Equals("SE"))
                            {
                                QryStr.Clear();
                                QryStr.Append("Update ApplicationApproval Set ApprovalStatusId = 2 , ApprovedBy = @ApproverId , ApprovedOn = GetDate() , " +
                                "Comment = 'APPROVED', Approval2statusId = 2 , Approval2By = @ApproverId , Approval2On = GetDate() where ParentId = @ApplicationApprovalId AND ParentType = @ParentType");
                                QryStr1.Append("Update RequestApplication Set IsApproved = 1 where Id = @ApplicationApprovalId ");
                            }
                            else
                            {
                                QryStr.Clear();
                                QryStr.Append("update ApplicationApproval Set ApprovalStatusId = 2 , ApprovedBy = @ApproverId , ApprovedOn = GetDate() ," +
                                    " Comment = 'APPROVED' where ParentId = @ApplicationApprovalId ");

                            }
                            context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@ApproverId", ApproverId)
                                , new SqlParameter("@ParentType", ParentType), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId));
                            context.Database.ExecuteSqlCommand(QryStr1.ToString(), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId));
                            //Manulpunchdatasendtosmax
                            if (ParentType.StartsWith("MP") == true)
                            {
                                queryString.Clear();
                                queryString.Append("Select Id,StaffId,StartDate as indatetime,EndDate as outdatetime,punchtype from " +
                                    "RequestApplication where Id = @ApplicationApprovalId");
                                try
                                {

                                    var data = context.Database.SqlQuery<Manualpunchforsmax>(queryString.ToString(), new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).Select(d => new Manualpunchforsmax()
                                    {

                                        StaffId = d.StaffId,
                                        Indatetime = d.Indatetime,
                                        Outdatetime = d.Outdatetime,
                                        PunchType = d.PunchType

                                    }).FirstOrDefault();

                                    string Indatetime;
                                    string outdatetime;
                                    Indatetime = DateTime.ParseExact(data.Indatetime.ToString(), "dd/MM/yyyy HH:mm:ss", null).ToString("dd/MMM/yyyy HH:mm:ss");
                                    outdatetime = DateTime.ParseExact(data.Outdatetime.ToString(), "dd/MM/yyyy HH:mm:ss", null).ToString("dd/MMM/yyyy HH:mm:ss");

                                    if (data.PunchType.ToUpper() == "IN")
                                    {
                                        SaveInPunch(Indatetime, data.StaffId, LocationId);
                                    }
                                    else if (data.PunchType.ToUpper() == "OUT")
                                    {
                                        SaveOutPunch(outdatetime, data.StaffId, LocationId);
                                    }
                                    else
                                    {
                                        SaveInOutPunch(Indatetime, outdatetime, data.StaffId, LocationId);
                                    }

                                }
                                catch (Exception e)
                                {
                                    throw e;
                                }
                            }

                            SendEmailToStaff(ApplicationApprovalId);

                        }
                        else if (res2.ToString() == "2" || res3.ToString() == "2")
                        {
                            throw new Exception("Cannot approve already approved application.");
                        }
                        else if (res2.ToString() == "3" || res3.ToString() == "3")
                        {
                            throw new Exception("Cannot approve already rejected application.");
                        }

                        Trans.Commit();
                    }

                    catch (Exception err)
                    {
                        Trans.Rollback();
                        throw err;
                    }
                }
            }
        }

        public void LogIntoIntoAttendanceControlTable(string StaffId, DateTime FromDate, DateTime ToDate, string ApplicationType, string ApplicationId)
        {
            DateTime currentDate = DateTime.Now;

            var act = new AttendanceControlTable();
            act.StaffId = StaffId;
            act.FromDate = FromDate.Date;
            act.ToDate = ToDate.Date;
            act.IsProcessed = false;
            act.CreatedOn = DateTime.Now;
            act.CreatedBy = StaffId;
            act.ApplicationType = ApplicationType;
            act.ApplicationId = ApplicationId;
            context.AttendanceControlTable.AddOrUpdate(act);
            context.SaveChanges();
        }

        public void SaveIntoApplicationApproval(string parentid, string parenttype, string loggedInUserId, string reportingmanager, bool SelfApproval)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ReportingManager as Approver1,Approver2,ApproverLevel from StaffOfficial where " +
                "StaffId=@StaffId");
            ShiftChangeApprvalModel ApproverData = context.Database.SqlQuery<ShiftChangeApprvalModel>(builder.ToString(), new SqlParameter("@StaffId", loggedInUserId)).FirstOrDefault();

            bool AutoApprove = false;
            if (ApproverData.Approver1 == loggedInUserId && ApproverData.Approver2 == loggedInUserId)
            {
                AutoApprove = true;
            }

            var maxid = string.Empty;
            var lastid = string.Empty;
            ApplicationApproval aa = new ApplicationApproval();
            RALeaveApplicationRepository RAL = new RALeaveApplicationRepository();

            var mr = new MasterRepository();
            //maxid = mr.getmaxid("ApplicationApproval", "id", "AA", "", 10, ref lastid);
            maxid = RAL.GetUniqueId();
            aa.Id = maxid;
            aa.ParentId = parentid;
            aa.ApprovedOn = DateTime.Now;
            aa.ParentType = parenttype;
            if (SelfApproval == true)
            {
                aa.ApprovalStatusId = 2;
                aa.ApprovedBy = reportingmanager;
                aa.Comment = "SELF APPROVAL";
                aa.ApprovalOwner = reportingmanager;
            }
            else
            {
                aa.ApprovalStatusId = 1;
                aa.ApprovedBy = reportingmanager;
                aa.Comment = "--";
                aa.ApprovalOwner = reportingmanager;
            }
            if (AutoApprove == true)
            {
                aa.Approval2statusId = 2;
                aa.Approval2By = reportingmanager;
                aa.Approval2Owner = reportingmanager;
                aa.Approval2On = DateTime.Now;
            }
            aa.ForwardCounter = 1;
            aa.ApplicationDate = DateTime.Now;

            context.ApplicationApproval.AddOrUpdate(aa);
            context.SaveChanges();
        }



        public void SaveInPunch(string InTime, string StaffId, string LocationId)
        {
            DateTime actuailInTime = Convert.ToDateTime(InTime);
            var SwipeType = "In";
            DeleteDupilcateSwipes(StaffId, InTime, InTime, SwipeType);
            try
            {
                var cardNumber = "-";
                var queryString1 = new StringBuilder();
                queryString1.Clear();
                queryString1.Append(" Delete from SmaxTransaction where Convert(datetime,Tr_Date)= " +
                    "Convert(datetime,'" + actuailInTime.ToString("yyyy-MM-dd HH:mm:ss") + "') And " +
                    "Tr_ChId = @StaffId");
                context.Database.ExecuteSqlCommand(queryString1.ToString(), new SqlParameter("@StaffId", StaffId));
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Insert into [SMaxTransaction] Values (@InTime,@InTime,'20','Access Granted'," +
                    "'1','IN',@cardNumber,'1','1','1','192.168.0.223',@StaffId,'1','','MANUAL PUNCH','IN','1',GETDATE(),0,@LocationId)");
                context.Database.ExecuteSqlCommand(queryString.ToString(), new SqlParameter("@InTime", InTime)
                    , new SqlParameter("@cardNumber", cardNumber), new SqlParameter("@StaffId", StaffId), new SqlParameter("@LocationId", LocationId));
            }
            catch (Exception err)
            {
                throw err;
            }
        }


        public void SaveMobileInPunch(string InTime, string StaffId, string LocationId)
        {
            DateTime actuailInTime = Convert.ToDateTime(InTime);
            var SwipeType = "In";
            DeleteDupilcateSwipes(StaffId, InTime, InTime, SwipeType);
            try
            {
                var cardNumber = "-";
                var queryString1 = new StringBuilder();
                queryString1.Clear();
                queryString1.Append(" Delete from SmaxTransaction where Convert(datetime,Tr_Date)= " +
                    "Convert(datetime,'" + actuailInTime.ToString("yyyy-MM-dd HH:mm:ss") + "') And " +
                    "Tr_ChId = @StaffId");
                context.Database.ExecuteSqlCommand(queryString1.ToString(), new SqlParameter("@StaffId", StaffId));
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Insert into [SMaxTransaction] Values (@InTime,@InTime,'20','Access Granted'," +
                    "'1','IN',@cardNumber,'1','1','1','66.66.66.66',@StaffId,'1','','MOBILE SWIPE','IN','1',GETDATE(),0,@LocationId)");
                context.Database.ExecuteSqlCommand(queryString.ToString(), new SqlParameter("@InTime", InTime)
                    , new SqlParameter("@cardNumber", cardNumber), new SqlParameter("@StaffId", StaffId), new SqlParameter("@LocationId", LocationId));
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public void SaveOutPunch(string OutTime, string StaffId, string LocationId)
        {
            DateTime actualOutTime = Convert.ToDateTime(OutTime);
            DateTime ActualOutTime = DateTime.Parse(OutTime);
            var SwipeType = "Out";
            DeleteDupilcateSwipes(StaffId, OutTime, OutTime, SwipeType);
            try
            {
                var cardNumber = "-";
                var queryString1 = new StringBuilder();
                queryString1.Clear();
                queryString1.Append(" Delete from SmaxTransaction where Convert(datetime,Tr_Date)= Convert(DateTime,'" + ActualOutTime.ToString("yyyy-MM-dd HH:mm:ss") + "') And Tr_ChId = '" + StaffId + "' ");
                context.Database.ExecuteSqlCommand(queryString1.ToString());
                var queryString2 = new StringBuilder();
                queryString2.Clear();
                queryString2.Append("Insert into [SMaxTransaction] Values (@OutTime,@OutTime,'36','Access Granted','1','OUT',@cardNumber,'1'," +
                    "'1','1','192.168.0.223',@StaffId,'1','0','MANUAL PUNCH','OUT','1',GETDATE(),0,@LocationId)");
                context.Database.ExecuteSqlCommand(queryString2.ToString(), new SqlParameter("@OutTime", OutTime)
                    , new SqlParameter("@cardNumber", cardNumber), new SqlParameter("@StaffId", StaffId), new SqlParameter("@LocationId", LocationId));
            }
            catch (Exception err)
            {
                throw err;
            }
        }


        public void SaveMobileOutPunch(string OutTime, string StaffId, string LocationId)
        {
            DateTime actualOutTime = Convert.ToDateTime(OutTime);
            DateTime ActualOutTime = DateTime.Parse(OutTime);
            var SwipeType = "Out";
            DeleteDupilcateSwipes(StaffId, OutTime, OutTime, SwipeType);
            try
            {
                var cardNumber = "-";
                var queryString1 = new StringBuilder();
                queryString1.Clear();
                queryString1.Append(" Delete from SmaxTransaction where Convert(datetime,Tr_Date)= Convert(DateTime,'" + ActualOutTime.ToString("yyyy-MM-dd HH:mm:ss") + "') And Tr_ChId = '" + StaffId + "' ");
                context.Database.ExecuteSqlCommand(queryString1.ToString());
                var queryString2 = new StringBuilder();
                queryString2.Clear();
                queryString2.Append("Insert into [SMaxTransaction] Values (@OutTime,@OutTime,'36','Access Granted','1','OUT',@cardNumber,'1'," +
                    "'1','1','66.66.66.66',@StaffId,'1','0','MOBILE SWIPE','OUT','1',GETDATE(),0,@LocationId)");
                context.Database.ExecuteSqlCommand(queryString2.ToString(), new SqlParameter("@OutTime", OutTime)
                    , new SqlParameter("@cardNumber", cardNumber), new SqlParameter("@StaffId", StaffId), new SqlParameter("@LocationId", LocationId));
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public void SaveInOutPunch(string InTime, string OutTime, string StaffId, string LocationId)
        {
            DateTime actuailInTime = Convert.ToDateTime(InTime);
            DateTime actualOutTime = Convert.ToDateTime(OutTime);
            var cardNumber = "-";
            DeleteDupilcateSwipes(StaffId, InTime, OutTime, "InOut");
            try
            {
                var queryString1 = new StringBuilder();
                queryString1.Clear();
                queryString1.Append(" Delete from SmaxTransaction where Convert(datetime,Tr_Date)= " +
                    "Convert(datetime,'" + actuailInTime.ToString("yyyy-MM-dd HH:mm:ss") + "') And Tr_ChId = @StaffId ");
                context.Database.ExecuteSqlCommand(queryString1.ToString(), new SqlParameter("@StaffId", StaffId));

                var queryString4 = new StringBuilder();
                queryString4.Clear();
                queryString4.Append(" Delete from SmaxTransaction where Convert(datetime,Tr_Date)= Convert(DateTime,'" + actualOutTime.ToString("yyyy-MM-dd HH:mm:ss") + "') And Tr_ChId = @StaffId ");
                context.Database.ExecuteSqlCommand(queryString4.ToString(), new SqlParameter("@StaffId", StaffId));

                var queryString2 = new StringBuilder();
                //var ipIn = context.AttendanceReaders.Where(s => s.Id == 1).Select(d => d.IpAddress).FirstOrDefault();
                queryString2.Clear();
                queryString2.Append("Insert into [SMaxTransaction] Values (@InTime,@InTime,'20','Access Granted'," +
                    "'1','IN',@cardNumber,'0','0','1','192.168.0.223',@StaffId,'1','','MANUAL PUNCH','IN','1'" +
                    ",GETDATE(),0,@LocationId)");
                context.Database.ExecuteSqlCommand(queryString2.ToString(), new SqlParameter("@InTime", InTime)
                    , new SqlParameter("@cardNumber", cardNumber), new SqlParameter("@StaffId", StaffId)
                    , new SqlParameter("@LocationId", LocationId));

                var queryString3 = new StringBuilder();
                queryString3.Clear();
                queryString3.Append("Insert into [SMaxTransaction] Values (@OutTime,@OutTime,'36'," +
                    "'Access Granted','1','OUT',@cardNumber,'1','1','1','192.168.0.223',@StaffId,'1'," +
                    "'','MANUAL PUNCH','OUT','1',GETDATE(),0,@LocationId)");
                context.Database.ExecuteSqlCommand(queryString3.ToString(), new SqlParameter("@OutTime", OutTime)
                    , new SqlParameter("@cardNumber", cardNumber), new SqlParameter("@StaffId", StaffId)
                    , new SqlParameter("@LocationId", LocationId));
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public void DeleteDupilcateSwipes(string StaffId, string InTime, string OutTime, string SwipeType)
        {
            try
            {
                var RA = new RequestApplication();
                var DuplicateSwipe = new StringBuilder();
                var DuplicateSwipe1 = new StringBuilder();
                DuplicateSwipe.Clear();
                DuplicateSwipe1.Clear();
                if (SwipeType == "In")
                {
                    DuplicateSwipe.Append("Exec [DBO].[DeleteDuplicateSwipes]@StaffId,'In',@InTime,'192.168.0.223'");
                    context.Database.ExecuteSqlCommand(DuplicateSwipe.ToString(), new SqlParameter("@InTime", InTime)
                        , new SqlParameter("@StaffId", StaffId));
                }
                if (SwipeType == "Out")
                {
                    DuplicateSwipe.Append("Exec [DBO].[DeleteDuplicateSwipes]@StaffId,'Out',@OutTime,'192.168.0.223'");
                    context.Database.ExecuteSqlCommand(DuplicateSwipe.ToString(), new SqlParameter("@StaffId", StaffId)
                        , new SqlParameter("@OutTime", OutTime));
                }
                if (SwipeType == "InOut")
                {
                    DuplicateSwipe.Append("Exec [DBO].[DeleteDuplicateSwipes]@StaffId,'In',@InTime,'192.168.0.223'");
                    context.Database.ExecuteSqlCommand(DuplicateSwipe.ToString(), new SqlParameter("@InTime", InTime)
                        , new SqlParameter("@StaffId", StaffId));
                    DuplicateSwipe1.Append("Exec [DBO].[DeleteDuplicateSwipes]@StaffId,'Out',@OutTime,'192.168.0.223'");
                    context.Database.ExecuteSqlCommand(DuplicateSwipe1.ToString(), new SqlParameter("@StaffId", StaffId)
                        , new SqlParameter("@OutTime", OutTime));
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public string GetAccessLevel(string Staffid)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select  VALUE from rulegrouptxn where rulegroupid = ( ");
            QryStr.Append("select TOP 1 policyid from staffofficial where staffid = @Staffid)");
            QryStr.Append("AND RULEID = ( SELECT ID FROM [DBO].[RULE] WHERE NAME = 'EmployeeAccessLevel')");
            var data = context.Database.SqlQuery<string>(QryStr.ToString(), new SqlParameter("@Staffid", Staffid)).FirstOrDefault();
            return data;
        }

        public DateTime GetRestrictionAppDate(string StaffId)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Value from Settings where Parameter = 'Date for Restrict the Application'");
            var date = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
            DateTime rdate = Convert.ToDateTime(date);
            //date = Convert.ToDateTime(date);
            return rdate;
        }


        public List<SubordinateList> GetSubordinateTreeList(string StaffId)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            // below stored procedure call has to be made by using parameters and not the way it been done now. pls remember to change.
            queryString.Append("exec [DBO].[GetSubordinateTree] @StaffId");
            var _LST_ = context.Database.SqlQuery<SubordinateList>(queryString.ToString(), new SqlParameter("@StaffId", StaffId)).Select(d => new SubordinateList()
            {
                StaffId = d.StaffId,
                StaffName = d.StaffName,
                SubordinateCount = d.SubordinateCount
            }).ToList();
            return _LST_;
        }

        public List<HeadCountOverAll> GetHeadCountData(string GroupNo, string DeptId, string CategoryId, string GradeId, string ShiftId, string StaffId, string Date)
        {
            var queryString = new StringBuilder();
            queryString.Append("exec [DBO].[GetHeadCounts] @GroupNo , @DeptId , @CategoryId , @GradeId , @ShiftId , @StaffId, @Date");
            ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 0;
            var _LST_ = context.Database.SqlQuery<HeadCountOverAll>(queryString.ToString() , new SqlParameter("@GroupNo", GroupNo) , 
                new SqlParameter("@DeptId", DeptId) , new SqlParameter("@CategoryId", CategoryId) , new SqlParameter("@GradeId", GradeId) ,
                new SqlParameter("@ShiftId" , ShiftId) , new SqlParameter("@StaffId", StaffId) , new SqlParameter("@Date" , Date)).Select(d => new HeadCountOverAll()
            {
                DepartmentId = d.DepartmentId,
                DesignationId = d.DesignationId,
                ShiftId = d.ShiftId,
                DepartmentName = d.DepartmentName,
                CategoryName = d.CategoryName,
                ShiftName = d.ShiftName,
                HeadCount = d.HeadCount,
                PresentCount = d.PresentCount,
                AbsentCount = d.AbsentCount,
                GroupNo = d.GroupNo,
                DeptSeq = d.DeptSeq,
                DesgSeq = d.DesgSeq,
                GradeSeq = d.GradeSeq,
                Seq = d.Seq,
                TotalHeadCount = d.TotalHeadCount,
                TotalPresentCount = d.TotalPresentCount,
                TotalAbsentCount = d.TotalAbsentCount
            }).ToList();
            return _LST_;
        }

        public ACTList GetList(string Id)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append(" Select StaffId ,StartDate as FromDate , EndDate as ToDate from  " +
                    "RequestApplication where Id = @Id");
                var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@Id", Id)).Select(d => new ACTList()
                {
                    StaffId = d.StaffId,
                    FromDate = d.FromDate,
                    ToDate = d.ToDate
                }).FirstOrDefault();
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<UserList> ReqisterList(string ID)
        {
            var Qrstr = new StringBuilder();
            Qrstr.Clear();
            SqlParameter par = new SqlParameter();
            par.ParameterName = "@ID";
            par.Value = ID;
            par.SqlDbType = System.Data.SqlDbType.NVarChar;

            Qrstr.Append("Select A.StaffId,B.FirstName,A.UserName,B.CompanyName,B.DeptName,B.REPMGRFIRSTNAME " +
                "from aspnetusers A inner join staffview B on A.staffid=B.StaffId");
            var Data = context.Database.SqlQuery<UserList>(Qrstr.ToString(), par).ToList();
            return Data;
        }

        public string GetApprover(string StaffId)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select ReportingManager from staffofficial where staffId = @StaffId");
            var Approver = context.Database.SqlQuery<string>(queryString.ToString(), new SqlParameter("@StaffId", StaffId)).FirstOrDefault();
            return Approver;
        }

        public string GetHR()
        {
            var HRManager = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("Select value from [Settings] where parameter = 'HRAPPROVER'");

            try
            {
                HRManager = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
                return HRManager;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Forgot Password
        public string GetValidOfficialEmailRepository(string StaffId, string DOJ)
        {
            try
            {
                ValidateForgotPwdModel data = context.StaffOfficial.Where(condition => condition.StaffId == StaffId).Select(select => new ValidateForgotPwdModel()
                {
                    StaffId = select.StaffId,
                    DOJ = select.DateOfJoining,
                    Email = select.Email
                }).FirstOrDefault();

                if (data == null)
                {
                    throw new Exception("Employee does not exists..Please enter the proper employee code");
                }
                else if (Convert.ToDateTime(DOJ) != data.DOJ)
                {
                    throw new Exception("Invalid Date Of Joining");
                }
                else if (string.IsNullOrEmpty(data.Email).Equals(true) || data.Email == "-")
                {
                    throw new Exception("Your official e-mail Id has not been updated");
                }
                else
                {
                    Message = "OK" + data.Email;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Message;
        }
        public string SendPasswordDetailsToUserMail(string StaffId, string Password)
        {
            using (DbContextTransaction trans = context.Database.BeginTransaction())
            {
                try
                {
                    string StaffName = string.Empty;
                    string commonSenderEmailId = string.Empty;
                    string OfficialEmail = string.Empty;
                    StringBuilder EmailString = new StringBuilder();
                    StaffName = GetStaffName(StaffId);
                    OfficialEmail = GetEmailIdOfStaff(StaffId);
                    commonSenderEmailId = GetSenderEmailIdFromEmailSettings();
                    string pw = Decrypt(Password);
                    if (!string.IsNullOrEmpty(OfficialEmail).Equals(true))
                    {
                        EmailString.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + "  ( " + StaffId + ") <br/>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\"> Please find the below password for LMS as per your request.</p><br/><p style=\"font-family:tahoma; font-size:9pt;\"> " + pw + "</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>");
                        SendEmailMessage(commonSenderEmailId, OfficialEmail, string.Empty, string.Empty, "Forgot Password Requisition of " + StaffName + "", EmailString.ToString());
                    }
                    trans.Commit();
                    Message = "OK";
                }
                catch (Exception err)
                {
                    trans.Rollback();
                    throw err;
                }
            }
            return Message;
        }
        public string CheckIsEmployeeExists(string StaffId)
        {
            try
            {
                Message = context.Staff.Where(condition => condition.Id == StaffId).Select(select => select.FirstName).FirstOrDefault();
            }
            catch
            {
                return "";
            }
            return Message;
        }
        public string GetOfficialEmail(string StaffId)
        {
            try
            {
                Message = context.StaffOfficial.Where(condition => condition.StaffId == StaffId).Select(select => select.Email).FirstOrDefault();
            }
            catch
            {
                return "";
            }
            return Message;
        }
        public string GetPasswordForUserName(string StaffId)
        {
            try
            {
                Message = context.AtrakUserDetails.OrderByDescending(s => s.Id).Where(condition => condition.StaffId == StaffId).Select(select => select.Password).FirstOrDefault();
            }
            catch (Exception err)
            {
                throw err;
            }
            return Message;
        }

        public bool CheckMobileAppEligible(string StaffId)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Select IsMobileAppEligible From StaffOfficial Where StaffId = @StaffId");
                bool isMobileAppEligible = context.Database.SqlQuery<bool>(stringBuilder.ToString(),
                    new SqlParameter("@StaffId", StaffId)).First();
                return isMobileAppEligible;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string SaveUserDetails(AtrakUserDetails AUT)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    context.AtrakUserDetails.Add(AUT);
                    context.SaveChanges();
                    trans.Commit();
                    Message = "OK";
                }
                catch (Exception err)
                {
                    trans.Rollback();
                    Message = err.Message;
                }
            }
            return Message;
        }
        public string Decrypt(string str)
        {
            str = str.Replace(" ", "+");
            string DecryptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] inputByteArray = new byte[str.Length];

            byKey = System.Text.Encoding.UTF8.GetBytes(DecryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(str.Replace(" ", "+"));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }
        public string UpdateAtrakUserDetails(string UserName, string NewPwd)
        {
            using (DbContextTransaction trans = context.Database.BeginTransaction())
            {
                try
                {
                    SqlParameter[] Param = new SqlParameter[2];
                    Param[0] = new SqlParameter("@UserName", UserName);
                    Param[1] = new SqlParameter("@NewPwd", NewPwd);

                    StringBuilder builder = new StringBuilder();
                    builder.Append("Update AtrakUserDetails set Password= @NewPwd where " +
                        "StaffId = @UserName");
                    context.Database.ExecuteSqlCommand(builder.ToString(), Param);
                    trans.Commit();
                    Message = "success";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }
            return Message;
        }
        #endregion
        public string GetCompOffExpDateValue()
        {
            var qryStr = new StringBuilder();
            qryStr.Append("Select value from RuleGroupTxn where RuleId = 121");
            try
            {
                string HRManager = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
                return HRManager;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #region Attendance Policy Config
        public string GetPolicyValue(string PolicyId)
        {
            string Policyid = string.Empty;
            try
            {
                var queryString = new StringBuilder();
                queryString.Append("select Value from Settings where Parameter=@PolicyId");
                Policyid = context.Database.SqlQuery<string>(queryString.ToString(), new SqlParameter("@PolicyId", PolicyId)).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
            return Policyid;
        }
        public string Decode(string Reqid)
        {
            byte[] encoded = Convert.FromBase64String(Reqid);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }

        public string GetReviewerOwner(string Id)
        {
            var StaffName = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("Select Approval2Owner from ApplicationApproval where ParentId= '" + Id + "'");

            try
            {
                StaffName = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
                return StaffName;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetApproverOwner(string Id)
        {
            var StaffName = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("Select  approvalowner from ApplicationApproval where ParentId= '" + Id + "'");

            try
            {
                StaffName = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
                return StaffName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetEmailFromAdd()
        {
            try
            {
                string SendEmail = string.Empty;
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select SenderEmail from EmailSettings");
                SendEmail = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
                return SendEmail;
            }
            catch
            {
                return "";
            }

        }
        #endregion

        #region BulkImport
        public int ValidateEmpCode(string Staffid)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select count(*)  from StaffOfficial where staffid='" + Staffid + "'");
            var StaffId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
            return StaffId;
        }
        public int GetBloodgroupId(string Blood)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select count(*)  from bloodgroup where name='" + Blood + "'");
            var BloodId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
            return BloodId;
        }
        public string GetCompanyLocationId(string Location)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Id from Location where Replace(Name,Char(09),'')='" + Location + "'");
            var CompanyLocationId = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
            return CompanyLocationId;
        }
        public string GetCompanyId(string company)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Id from company where name='" + company + "'");
            var CompanyId = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
            return CompanyId;
        }
        public string GetBranchId(string Branch)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Id from Branch where name='" + Branch + "'");
            var BranchId = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
            return BranchId;
        }
        public string GetDepartmentId(string Department)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Id from department where name='" + Department + "'");
            var DepartmentId = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
            return DepartmentId;
        }
        public string GetDesignationId(string Designation)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Id from Designation where name='" + Designation + "'");
            var DesignationId = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
            return DesignationId;
        }
        public string GetGradeId(string Grade)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Id from Grade where name='" + Grade + "'");
            var GradeId = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
            return GradeId;
        }
        public string GetCategaryId(string GetCategary)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Id from category where name='" + GetCategary + "'");
            var CategoryId = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
            return CategoryId;
        }
        public string GetcostcentreId(string costcentre)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Id from costcentre where name='" + costcentre + "'");
            var costcentreId = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
            return costcentreId;
        }
        public int GetRoleId(string Role)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Id from SecurityGroup where name='" + Role + "'");
            var GetRoleId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
            return GetRoleId;
        }
        public int GetMaritalStatusId(string MaritalStatus)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Id from MaritalStatus where name='" + MaritalStatus + "'");
            var MaritalStatusId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
            return MaritalStatusId;
        }
        public int GetPolicyId(string GetPolicy)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Id from RuleGroup where name='" + GetPolicy + "'");
            var GetPolicyId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
            return GetPolicyId;
        }
        public int GetWorkingDayPatternId(string WorkingDayPattern)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Id from WorkingDayPattern where PatternDesc='" + WorkingDayPattern + "'");
            var WorkingDayPatternId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
            return WorkingDayPatternId;
        }
        public string GetWeeklyOffId(string GetWeeklyOff)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Id from WeeklyOffs where Name='" + GetWeeklyOff + "'");
            var GetWeeklyOffId = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
            return GetWeeklyOffId;
        }
        public string GetDivisionId(string Division)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select Id from Division where Name='" + Division + "'");
                var DivisionId = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
                return DivisionId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string GetLocation(string Locationid)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select Id from location where Name='" + Locationid + "'");
                var LocationId = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
                return LocationId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string Workingdaypattern(string WorkingDayPattern)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select Id from WorkingDayPattern where Name='" + WorkingDayPattern + "'");
                var WorkingdaypatternId = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
                return WorkingdaypatternId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string weeklyoffId(string WeeklyOffs)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select Id from WeeklyOffs where Name='" + WeeklyOffs + "'");
                var WeeklyOffsId = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
                return WeeklyOffsId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string GetCompLocationId(string GetLocation)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select Id from location where Name='" + GetLocation + "'");
                var LocationId = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
                return LocationId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int ValidateId(string ReportingManagerId)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select count(*) from StaffOfficial where staffid='" + ReportingManagerId + "'");
                var LocationId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
                return LocationId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int ValidateShiftPatternTypeId(string shiftpattern)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select count(*) from shiftpattern where name='" + shiftpattern + "'");
                var sftpatternId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
                return sftpatternId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int ValidateGeneralShiftId(string General)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select count(*) from shifts where name='" + General + "'");
                var GenId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
                return GenId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //public int ValidateFlexishiftId(string Flexi)
        //{
        //    try
        //    {
        //        var queryString = new StringBuilder();
        //        queryString.Clear();
        //        queryString.Append("select count(*) from shiftpattern where staffid='" + Flexi + "'");
        //        var FlexiId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
        //        return FlexiId;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
        public int ValidateWorkingdaypatternId(string Workingdaypattern)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select count(*) from WorkingDayPattern where PatternDesc='" + Workingdaypattern + "'");
                var Flexi = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
                return Flexi;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int ValidateweeklyoffId(string Validateweeklyoff)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select count(*) from WeeklyOffs where name='" + Validateweeklyoff + "'");
                var ValidateweeklyoffId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
                return ValidateweeklyoffId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int ValidateSecurityGroupId(string SecurityGroup)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select count(*) from securitygroup where name='" + SecurityGroup + "'");
                var SecurityGroupId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
                return SecurityGroupId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int ValidatePolicygroupId(string Policygroup)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select count(*) from RuleGroup where name='" + Policygroup + "'");
                var PolicygroupId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
                return PolicygroupId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int ValidateCompany(string ValidateCompany)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select count(*) from Company where Name='" + ValidateCompany + "'");
                var ValidateCompanyId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
                return ValidateCompanyId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int ValidateBranch(string ValidateBranch)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select count(*) from branch where Name='" + ValidateBranch + "'");
                var ValidateBranchId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
                return ValidateBranchId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int ValidateDepartment(string Department)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select count(*) from department where Name='" + Department + "'");
                var DepartmentId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
                return DepartmentId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int ValidateDivision(string Division)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select count(*) from Division where Name='" + Division + "'");
                var DivisionId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
                return DivisionId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int ValidateDesignation(string ValidateDesignation)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select count(*) from Designation where Name='" + ValidateDesignation + "'");
                var ValidateDesignationId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
                return ValidateDesignationId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public string GetEmailFromAdd()
        //  {
        //      try
        //      {
        //          string SendEmail = string.Empty;
        //          var queryString = new StringBuilder();
        //          queryString.Clear();
        //          queryString.Append("select SenderEmail from EmailSettings");
        //          SendEmail = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
        //          return SendEmail;
        //      }
        //      catch
        //      {
        //          return "";
        //      }
        //  }






        public int ValidateGrade(string ValidateGrade)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select count(*) from Grade where Name='" + ValidateGrade + "'");
                var GradeId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
                return GradeId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int ValidateCategory(string Category)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select count(*) from Category where Name='" + Category + "'");
                var CategoryId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
                return CategoryId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int ValidateCostCentre(string CostCentre)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select count(*) from CostCentre where Name='" + CostCentre + "'");
                var CostCentreId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
                return CostCentreId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int ValidateLocation(string Location)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select count(*) from Location where Name='" + Location + "'");
                var LocationId = context.Database.SqlQuery<int>(queryString.ToString()).FirstOrDefault();
                return LocationId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int CheckEmployeeExistingOrNot(string StfId)
        {
            var qryStr = new StringBuilder();
            qryStr.Append("Select count(Staffid) from staffofficial where staffid = @StaffId");
            try
            {
                int HRManager = context.Database.SqlQuery<int>(qryStr.ToString(), new SqlParameter("@StaffId", StfId)).FirstOrDefault();
                return HRManager;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void DeleteShiftPlanAfterEffectDate(DateTime EffectFromDate, string staffid)
        {
            try
            {
                var qryStr = new StringBuilder();
                qryStr.Append("Delete from AttendanceData where ShiftInDate >= @EffectFromDate and staffid = @Staffid");
                context.Database.ExecuteSqlCommand(qryStr.ToString(), new SqlParameter("@EffectFromDate", EffectFromDate), new SqlParameter("@staffid", staffid));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
        public string GetUserName(string StaffId)
        {
            try
            {
                var Qrystr = new StringBuilder();
                Qrystr.Append("Select FirstName from Staff Where Id = @StaffId");
                var Result = context.Database.SqlQuery<string>(Qrystr.ToString(), new SqlParameter("@StaffId", StaffId)).FirstOrDefault();
                return Result;

            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public string GetSenderEmailIdFromEmailSettings()
        {
            var OfficialEmailId = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("Select SenderEmail from Emailsettings");

            try
            {
                OfficialEmailId = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
                return OfficialEmailId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public BirthDayModel GetEmployeeNameAndDeaprtment(string EmployeeCode)
        {
            var Qrystr = new StringBuilder();
            SqlParameter Param = new SqlParameter();
            Param = new SqlParameter("@EmployeeCode", EmployeeCode);
            Qrystr.Append("Select FirstName as StaffName, DeptName from StaffView Where StaffId =  @EmployeeCode");
            try
            {
                var EmployeeDetails = context.Database.SqlQuery<BirthDayModel>(Qrystr.ToString(), Param).FirstOrDefault();
                return EmployeeDetails;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        public string GetHRName(string HrId)
        {
            var HRName = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("Select FirstName from StaffView Where StaffId = @HrId");

            try
            {
                HRName = context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@HrId", HrId)).FirstOrDefault();
                return HRName;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ApplicationforcancellationList> GetApplicationsForOnBehalfApproval(string staffId, string applicationType,
            string fromDate, string toDate, string reportingManager)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            if (applicationType.ToUpper().Equals("LA"))
            {
                qryStr.Append("Select LeaveApplicationId as ApplicationId, StaffId ,'LA' as ApplicationShortname,LeaveTypeId , LeaveStartDurationName as LeaveStartDuration , " +
                  " LeaveEndDurationName as LeaveEndDuration  , LeaveTypeName , Replace(Convert(Varchar,LeaveStartDate,106) , ' ','-') as FromDate , " +
                  " Replace(Convert(Varchar,LeaveEndDate,106) , ' ','-') as ToDate ,Cast(TotalDays as Varchar) as TotalDays, LeaveApplicationReason as Reason, IsDocumentAvailable " +
                  " from vwLeaveApplicationApproval where Approval1StatusId = 1 AND ISCANCELLED = 'NO' and CONVERT ( Date, LeaveStartDate )   Between " +
                  " convert(Date,@fromDate) AND convert(Date,@toDate) AND CONVERT " +
                  " ( Date, LeaveEndDate ) Between convert(Date,@fromDate) AND  convert" +
                  " (Date,@toDate) and ParentType = @ApplicationType");

                if (string.IsNullOrEmpty(staffId).Equals(false) && string.IsNullOrEmpty(reportingManager).Equals(true))
                {
                    qryStr.Append(" and StaffId = @StaffId");
                }
                else if (string.IsNullOrEmpty(reportingManager).Equals(false))
                {
                    qryStr.Append(" and ((Approval1Owner = @ReportingManager AND Approval1StatusId = 1) OR " +
                        " (Approval2Owner = @ReportingManager AND Approval2statusId = 1))");
                }
            }

            if (applicationType.ToUpper().Equals("PO"))
            {
                qryStr.Append("Select PermissionId as ApplicationId , StaffId ,'PO' as ApplicationShortname, FirstName as Name," +
                    "  Replace(Convert(Varchar,PermissionDate,106) , ' ','-') as PermissionDate, FromTime as TimeFrom, TimeTo , " +
                    " TotalHours ,PermissionOffReason as Reason, ContactNumber ,  Approval1StatusName ," +
                    " Approval1StaffId , Approval1StaffName from vwPermissionApproval where IsCancelled= 0 and Approval1StatusId = 1 " +
                    " and Convert ( Date, PermissionDate ) Between  Convert(Date,@fromDate) " +
                    " AND Convert (Date,@toDate) and ParentType = @ApplicationType");
                if (string.IsNullOrEmpty(staffId).Equals(false) && string.IsNullOrEmpty(reportingManager).Equals(true))
                {
                    qryStr.Append(" and StaffId = @StaffId");
                }
                else if (string.IsNullOrEmpty(reportingManager).Equals(false))
                {
                    qryStr.Append(" and ((Approval1Owner = @ReportingManager AND Approval1StatusId = 1) OR " +
                        " (Approval2Owner = @ReportingManager AND Approval2statusId = 1))");
                }
            }
            if (applicationType.ToUpper().Equals("MP"))
            {
                qryStr.Append("Select A.Id as ApplicationId , A.StaffId ,B.ApprovalStatusId ,'MP' as ApplicationShortname , A.StartDate " +
                    " as InDateTime, A.EndDate As OutDateTime, A.PunchType, A.Remarks as Reason from RequestApplication A inner join ApplicationApproval" +
                    " B on B.ParentId = A.Id where IsCancelled = 0 and ApprovalStatusId = 1 " +
                    " and CONVERT ( Date, StartDate) Between convert(Date,@fromDate)  AND convert" +
                    " (Date,@toDate) AND CONVERT ( Date, EndDate ) Between  convert" +
                    " (Date,@fromDate) AND  convert(Date,@toDate) and ParentType = @ApplicationType");
                if (string.IsNullOrEmpty(staffId).Equals(false) && string.IsNullOrEmpty(reportingManager).Equals(true))
                {
                    qryStr.Append(" and StaffId = @StaffId");
                }
                else if (string.IsNullOrEmpty(reportingManager).Equals(false))
                {
                    qryStr.Append(" and ((ApprovalOwner = @ReportingManager AND ApprovalStatusId = 1) OR " +
                         " (Approval2Owner = @ReportingManager AND Approval2statusId = 1))");
                }
            }
            if (applicationType.ToUpper().Equals("CO"))
            {
                qryStr.Append("Select A.Id as ApplicationId ,  A.StaffId ,B.ApprovalStatusId , 'CO' as ApplicationShortname ," +
                    " Replace(convert(Varchar, A.StartDate, 106), ' ', '-') as CoffFrom," +
                    " Replace(convert(Varchar, A.EndDate, 106), ' ', '-') as CoffTo, " +
                    " Replace(convert(Varchar, A.WorkedDate, 106), ' ', '-') as WorkedDate, A.Remarks as Reason from " +
                    " RequestApplication A inner join ApplicationApproval B on" +
                    "  B.ParentId = A.Id where IsCancelled= 0  " +
                    " and ApprovalStatusId = 1 and CONVERT ( Date, StartDate )   Between " +
                    " convert(Date,@fromDate) AND convert(Date,@toDate) AND CONVERT " +
                    " ( Date, EndDate ) Between convert(Date,@fromDate) AND  convert" +
                    " (Date,@toDate) and ParentType = @ApplicationType");
                if (string.IsNullOrEmpty(staffId).Equals(false) && string.IsNullOrEmpty(reportingManager).Equals(true))
                {
                    qryStr.Append(" and StaffId = @StaffId");
                }
                else if (string.IsNullOrEmpty(reportingManager).Equals(false))
                {
                    qryStr.Append(" and ((ApprovalOwner = @ReportingManager AND ApprovalStatusId = 1) OR " +
                        " (Approval2Owner = @ReportingManager  AND Approval2statusId = 1))");
                }
            }
            if (applicationType.ToUpper().Equals("OD") || applicationType.ToUpper().Equals("BT") || applicationType.ToUpper().Equals("WFH"))
            {
                qryStr.Append(" Select A.Id as ApplicationId , A.StaffId , B.ApprovalStatusId , 'OD' as ApplicationShortname , A.ODDuration as Duration, " +
               " Case when A.ODDuration = 'SINGLEDAY' then Convert ( Varchar , Replace(Convert ( varchar , A.[StartDate] , 106 ),' ','-')" +
               " + ' ' + Convert ( varchar ( 8 ) , A.[StartDate] , 114 ) )else REPLACE ( Convert ( VARCHAR , A.[StartDate], 106 ) , ' ' , '-' ) " +
               " end as  FromDate  , case when A.ODDuration = 'SINGLEDAY' then Convert ( Varchar ,Replace" +
               " ( Convert ( varchar , A.[EndDate] , 106 ),' ','-') + ' ' + Convert ( varchar ( 8 ) , A.[EndDate] , 114 ) )else " +
               " REPLACE ( Convert ( VARCHAR , A.[EndDate], 106 ) , ' ' , '-' ) end as  ToDate  , A.Remarks as Reason  , @ApplicationType as ODType from " +
               " RequestApplication A inner join ApplicationApproval B on B.ParentId = A.Id where " +
               " Convert ( Date, [StartDate]) Between  Convert(Date,@fromDate)  AND Convert(Date,@toDate) " +
               " AND  Convert ( Date, [EndDate] ) Between  Convert(Date,@fromDate) AND Convert(Date,@toDate) AND " +
               " IsCancelled = 0 AND ApprovalStatusId = 1 and ParentType = @ApplicationType");
                if (string.IsNullOrEmpty(staffId).Equals(false) && string.IsNullOrEmpty(reportingManager).Equals(true))
                {
                    qryStr.Append(" and StaffId = @StaffId");
                }
                else if (string.IsNullOrEmpty(reportingManager).Equals(false))
                {
                    qryStr.Append(" and ((ApprovalOwner = @ReportingManager AND ApprovalStatusId = 1) OR " +
                        " (Approval2Owner = @ReportingManager AND Approval2statusId = 1))");
                }
            }
            if (applicationType.ToUpper().Equals("CR"))
            {
                qryStr.Append(" Select RA.Id as ApplicationId,'CR' as ApplicationShortname , RA.StaffId , " +
                " Replace(convert(Varchar, RA.StartDate, 106), ' ', '-') as FromDate, " +
                " Replace(convert(Varchar, RA.EndDate, 106), ' ', '-') as ToDate, " +
                " Replace(convert(Varchar, RA.TotalDays, 106), ' ', '-') as Credit, RA.Remarks as Reason from " +
                " RequestApplication RA inner join ApplicationApproval AA on " +
                " AA.ParentId = RA.Id where Convert ( Date, [StartDate]) Between  " +
                " Convert(Date,@fromDate)  AND Convert(Date,@toDate)  AND  Convert ( Date, [EndDate] ) Between  " +
                "Convert(Date,@fromDate) AND Convert(Date,@toDate) AND  IsCancelled = 0 AND ApprovalStatusId = 1 " +
                "and ParentType = @ApplicationType");
                if (string.IsNullOrEmpty(staffId).Equals(false) && string.IsNullOrEmpty(reportingManager).Equals(true))
                {
                    qryStr.Append(" and StaffId = @StaffId");
                }
                else if (string.IsNullOrEmpty(reportingManager).Equals(false))
                {
                    qryStr.Append(" and ((ApprovalOwner =  @ReportingManager  AND ApprovalStatusId = 1) OR " +
                        " (Approval2Owner =  @ReportingManager AND Approval2statusId = 1))");
                }
            }

            if (applicationType.ToUpper().Equals("SC"))
            {
                qryStr.Append("Select ApplicationId,'SC' as ApplicationShortname ,  StaffId , StaffName as Name," +
                    " StartDate as FromDate , EndDate as ToDate, NewShiftName , Remarks as Reason from ShiftChangeApproval where" +
                    " Convert(Date,StartDate) Between Convert(Date,@fromDate)  AND Convert(Date,@toDate) AND Approval1StatusId = 1 " +
                    "AND ISCANCELLED = 'NO' and ParentType = @ApplicationType");
                if (string.IsNullOrEmpty(staffId).Equals(false) && string.IsNullOrEmpty(reportingManager).Equals(true))
                {
                    qryStr.Append(" and StaffId = @StaffId");
                }
                else if (string.IsNullOrEmpty(reportingManager).Equals(false))
                {
                    qryStr.Append(" and Approval1Owner = @ReportingManager AND Approval1StatusId = 1");
                }
            }

            if (applicationType.ToUpper().Equals("HW"))
            {
                qryStr.Append(" Select Convert(Varchar,HolidayWorkingId) as ApplicationId,'HW' as ApplicationShortname ,StaffId,Name," +
                    "TransactionDate,InTime as FromDate,OutTime as ToDate from HolidayWorkingApproval where Convert(Date,TransactionDate) Between  " +
                    " Convert(Date,@fromDate)  AND Convert(Date,@toDate) AND ApprovalStatusId = 1 and ParentType = @ApplicationType");
                if (string.IsNullOrEmpty(staffId).Equals(false) && string.IsNullOrEmpty(reportingManager).Equals(true))
                {
                    qryStr.Append(" and StaffId = @StaffId");
                }
                else if (string.IsNullOrEmpty(reportingManager).Equals(false))
                {
                    qryStr.Append(" and ApprovalOwner =  @ReportingManager   AND ApprovalStatusId = 1 ");
                }
            }
            if (applicationType.ToUpper().Equals("SE"))
            {
                qryStr.Append(" Select Convert(Varchar,ShiftExtensionId) as ApplicationId,'SE' as ApplicationShortname ,StaffId , " +
                    "FirstName as Name, ExtensionDate as TxnDate,DurationOfHoursExtension as Duration,HoursBeforeShift as BeforeShiftHours,HoursAfterShift as AfterShiftHours  " +
                    " from ShiftExtensionApproval where Convert(Date,ExtensionDate) Between  Convert(Date,@fromDate)  AND " +
                    " Convert(Date,@toDate) AND ApprovalStatusId = 1 and ParentType = @ApplicationType");
                if (string.IsNullOrEmpty(staffId).Equals(false) && string.IsNullOrEmpty(reportingManager).Equals(true))
                {
                    qryStr.Append(" and StaffId = @StaffId");
                }
                else if (string.IsNullOrEmpty(reportingManager).Equals(false))
                {
                    qryStr.Append(" and ApprovalOwner = @ReportingManager AND ApprovalStatusId = 1");
                }
            }
            try
            {
                var lst = context.Database.SqlQuery<ApplicationforcancellationList>(qryStr.ToString(), new SqlParameter("@StaffId", staffId)
                    , new SqlParameter("@ReportingManager", reportingManager), new SqlParameter("@fromDate", fromDate)
                    , new SqlParameter("@toDate", toDate), new SqlParameter("@ApplicationType", applicationType)).
                    Select(d => new ApplicationforcancellationList()
                    {
                        StaffId = d.StaffId,
                        Name = d.Name,
                        LeaveType = d.LeaveType,
                        LeaveTypename = d.LeaveTypename,
                        LeaveStartDate = d.LeaveStartDate,
                        LeaveEndDate = d.LeaveEndDate,
                        ApplicationId = d.ApplicationId,
                        ApplicationShortname = d.ApplicationShortname,
                        TxnDate = d.TxnDate,
                        FromDate = d.FromDate,
                        IsDocumentAvailable = d.IsDocumentAvailable,
                        ToDate = d.ToDate,
                        TimeFrom = d.TimeFrom,
                        TimeTo = d.TimeTo,
                        InDateTime = d.InDateTime,
                        OutDateTime = d.OutDateTime,
                        PunchType = d.PunchType,
                        PermissionDate = d.PermissionDate,
                        TotalHours = d.TotalHours,
                        TotalDays = d.TotalDays,
                        Credit = d.Credit,
                        CoffFrom = d.CoffFrom,
                        CoffTo = d.CoffTo,
                        WorkedDate = d.WorkedDate,
                        Duration = d.Duration,
                        ODType = d.ODType,
                        Reason = d.Reason,
                        NewShiftName = d.NewShiftName,
                        BeforeShiftHours = d.BeforeShiftHours,
                        AfterShiftHours = d.AfterShiftHours,
                    }).ToList();

                if (lst == null)
                {
                    return new List<ApplicationforcancellationList>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<ApplicationforcancellationList>();
            }
        }
        public void RemovePunchesFromSmax(string id, string StaffId)
        {

            string staffid = string.Empty;
            DateTime fromDate = DateTime.Now;
            DateTime Indatetime = DateTime.Now;
            DateTime Outdatetime = DateTime.Now;
            string punchtype = string.Empty;

            // Get the Date value from Manual Punch Table fro deleting the Punch
            var QryStr1 = new StringBuilder();
            QryStr1.Clear();
            QryStr1.Append("Select staffid,InDateTime as Indatetime,OutDateTime,PunchType from  [ManualPunch]" +
                " where Id = '" + id + "' AND StaffId = @StaffId");

            try
            {
                var data = context.Database.SqlQuery<Manualpunchforsmax>(QryStr1.ToString(), new SqlParameter("@StaffId", StaffId)).Select(d => new Manualpunchforsmax()
                {
                    StaffId = d.StaffId,
                    Indatetime = d.Indatetime,
                    Outdatetime = d.Outdatetime,
                    PunchType = d.PunchType
                }).FirstOrDefault();
                staffid = data.StaffId;
                Indatetime = data.Indatetime;
                Outdatetime = data.Outdatetime;
                punchtype = data.PunchType;
            }
            catch (Exception)
            {
                throw;
            }

            if (punchtype == "OUT")
            {
                string dateForDelete = string.Empty;
                var day = Outdatetime.Day;
                var monthName = Outdatetime.ToString("MMM");
                var year = Outdatetime.Year;
                dateForDelete = day + "-" + monthName + "-" + year;

                var QryStr2 = new StringBuilder();
                QryStr2.Clear();
                QryStr2.Append(" Delete from [SmaxTransaction]  where Tr_ChId = @StaffId AND  Convert(Date,Tr_Date)= " +
                    "Convert(Date,@dateForDelete) AND Tr_IpAddress = '192.168.0.223'  AND Tr_OpName = 'OUT'");
                context.Database.ExecuteSqlCommand(QryStr2.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@dateForDelete", dateForDelete));
            }
            else if (punchtype == "IN")
            {
                string dateForDelete = string.Empty;
                var day = Indatetime.Day;
                var monthName = Indatetime.ToString("MMM");
                var year = Indatetime.Year;
                dateForDelete = day + "-" + monthName + "-" + year;

                var QryStr3 = new StringBuilder();
                QryStr3.Clear();
                QryStr3.Append(" Delete from [SmaxTransaction]  where Tr_ChId = @StaffId" +
                    " AND  Convert(Date,Tr_Date)= Convert(Date,@dateForDelete) AND Tr_IpAddress = '192.168.0.223' AND Tr_OpName = 'IN'");
                context.Database.ExecuteSqlCommand(QryStr3.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@dateForDelete", dateForDelete));
            }
            else if (punchtype == "INOUT")
            {
                if (Outdatetime.Date > Indatetime.Date)
                {
                    string inDateForDelete = string.Empty;
                    var day = Indatetime.Day;
                    var monthName = Indatetime.ToString("MMM");
                    var year = Indatetime.Year;
                    inDateForDelete = day + "-" + monthName + "-" + year;

                    string outDateForDelete = string.Empty;
                    var day1 = Outdatetime.Day;
                    var monthName1 = Outdatetime.ToString("MMM");
                    var year1 = Outdatetime.Year;
                    outDateForDelete = day1 + "-" + monthName1 + "-" + year1;

                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append(" Delete from [SmaxTransaction]  where Tr_ChId = @StaffId AND  " +
                        "Convert(Date,Tr_Date)= Convert(Date,@inDateForDelete) AND Tr_IpAddress = '192.168.0.223'");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@StaffId", StaffId)
                        , new SqlParameter("@inDateForDelete", inDateForDelete));

                    var QryStr2 = new StringBuilder();
                    QryStr2.Clear();
                    QryStr2.Append(" Delete from [SmaxTransaction]  where Tr_ChId = @StaffId AND  " +
                        "Convert(Date,Tr_Date)= Convert(Date,@outDateForDelete) AND Tr_IpAddress = '192.168.0.223'");
                    context.Database.ExecuteSqlCommand(QryStr2.ToString(), new SqlParameter("@StaffId", StaffId)
                        , new SqlParameter("@outDateForDelete", outDateForDelete));
                }
                else
                {
                    string dateForDelete = string.Empty;
                    var day = Indatetime.Day;
                    var monthName = Indatetime.ToString("MMM");
                    var year = Indatetime.Year;
                    dateForDelete = day + "-" + monthName + "-" + year;
                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append(" Delete from [SmaxTransaction]  where Tr_ChId = @StaffId AND " +
                        " Convert(Date,Tr_Date)= Convert(Date,@dateForDelete) AND Tr_IpAddress = '192.168.0.223'");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@dateForDelete", dateForDelete)
                        , new SqlParameter("@StaffId", StaffId));
                }

            }
        }
        public string Cancel(string ApplicationId, string ApplicationShortname, string UserID)
        {
            string staffId = string.Empty;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            string applicationId = string.Empty;
            string applicationType = string.Empty;
            var queryString = new StringBuilder();
            CommonRepository obj = new CommonRepository();

            queryString.Clear();
            try
            {
                if (ApplicationShortname == "LA")
                {
                    var bl = new CommonRepository();
                    bl.LeaveBalanceHandler(ApplicationId, "CANCEL");

                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("Update LeaveApplicationWabco Set IsCancelled=1 where id =@ApplicationId");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@ApplicationId", ApplicationId));
                    bl.OnBehalfApplcationCancellationEmailTrigger(ApplicationId, UserID);
                }
                else if (ApplicationShortname == "PO")
                {
                    var bl = new CommonRepository();
                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("Update PermissionOff Set IsCancelled=1 where id =ApplicationId");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@ApplicationId", ApplicationId));
                    bl.OnBehalfApplcationCancellationEmailTrigger(ApplicationId, UserID);
                }

                else if (ApplicationShortname == "CO")
                {
                    var bl = new CommonRepository();
                    bl.LeaveBalanceHandler(ApplicationId, "CANCEL");
                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("Update CompensatoryOff Set IsCancelled=1 where id =@ApplicationId");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@ApplicationId", ApplicationId));

                    QryStr.Clear();
                    QryStr.Append("Select * From EmployeeLeaveAccount where refid=@ApplicationId and transactionflag=2");
                    var data = context.Database.SqlQuery<EmployeeLeaveAccount>(QryStr.ToString(), new SqlParameter("@ApplicationId", ApplicationId)).ToList();

                    foreach (var dt in data)
                    {
                        EmployeeLeaveAccount ela = new EmployeeLeaveAccount();
                        string value = dt.LeaveCount.ToString();
                        var val = value.Split('-');
                        ela.StaffId = dt.StaffId;
                        ela.LeaveTypeId = "LV0005";
                        ela.TransactionFlag = 1;
                        ela.TransactionDate = dt.TransactionDate;
                        ela.LeaveCount = Convert.ToDecimal(val[1]);
                        ela.Narration = "CANCELLED COFF - " + ApplicationId;
                        ela.LeaveCreditDebitReasonId = 23;
                        ela.Year = DateTime.Now.Year;
                        ela.Month = DateTime.Now.Month;
                        ela.IsLapsed = false;
                        ela.IsSystemAction = false;
                        ela.RefId = ApplicationId;
                        context.EmployeeLeaveAccount.Add(ela);
                        context.SaveChanges();
                    }
                    bl.OnBehalfApplcationCancellationEmailTrigger(ApplicationId, UserID);



                }
                else if (ApplicationShortname == "MP")
                {
                    var bl = new CommonRepository();
                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("Update ManualPunch Set IsCancelled=1 where id =@ApplicationId");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@ApplicationId", ApplicationId));

                    QryStr.Clear();
                    QryStr.Append("select staffid  from ManualPunch where id =@ApplicationId");
                    var rtnstaffid = context.Database.SqlQuery<string>(QryStr.ToString(), new SqlParameter("@ApplicationId", ApplicationId)).FirstOrDefault();
                    RemovePunchesFromSmax(ApplicationId, rtnstaffid);
                    bl.OnBehalfApplcationCancellationEmailTrigger(ApplicationId, UserID);

                }
                else if (ApplicationShortname == "OD")
                {
                    var bl = new CommonRepository();
                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("Update ODApplication Set IsCancelled=1 where id=@ApplicationId");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@ApplicationId", ApplicationId));
                    bl.OnBehalfApplcationCancellationEmailTrigger(ApplicationId, UserID);
                }
                else if (ApplicationShortname == "RH")
                {
                    var bl = new CommonRepository();
                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("Update RHApplication Set IsCancelled=1 where id =@ApplicationId");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@ApplicationId", ApplicationId));
                    bl.OnBehalfApplcationCancellationEmailTrigger(ApplicationId, UserID);
                }
                else if (ApplicationShortname == "LO")
                {
                    var bl = new CommonRepository();
                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("Update LaterOff Set IsCancelled=1 where id =@ApplicationId");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@ApplicationId", ApplicationId));
                    bl.OnBehalfApplcationCancellationEmailTrigger(ApplicationId, UserID);
                }
                else if (ApplicationShortname == "MO")
                {
                    var bl = new CommonRepository();
                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("Update MaintenanceOff Set IsCancelled=1 where id =@ApplicationId");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@ApplicationId", ApplicationId));
                    bl.OnBehalfApplcationCancellationEmailTrigger(ApplicationId, UserID);
                }
                if (ApplicationId.StartsWith("LA"))
                {

                    queryString.Append(" Select StaffId ,LeaveStartDate as FromDate , LeaveEndDate as ToDate from  [LeaveApplicationWabco]" +
                        " where Id = @ApplicationId");
                    try
                    {

                        var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@ApplicationId", ApplicationId)).Select(d => new ACTList()
                        {
                            StaffId = d.StaffId,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate
                        }).FirstOrDefault();

                        staffId = data.StaffId;
                        fromDate = data.FromDate;
                        toDate = data.ToDate;
                        applicationType = "LA";
                        applicationId = ApplicationId;
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                    if (fromDate.Date < currentDate.Date)
                    {
                        if (toDate.Date >= currentDate.Date)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        obj.LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationId);
                    }

                }

                else if (ApplicationId.StartsWith("MP"))
                {

                    queryString.Append(" Select StaffId , InDateTime as FromDate , OutDateTime as ToDate from ManualPunch where " +
                        "Id = @ApplicationId");
                    try
                    {

                        var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@ApplicationId", ApplicationId)).Select(d => new ACTList()
                        {
                            StaffId = d.StaffId,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate
                        }).FirstOrDefault();

                        staffId = data.StaffId;
                        fromDate = data.FromDate;
                        toDate = data.ToDate;
                        applicationType = "MP";
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                    if (fromDate.Date < currentDate.Date)
                    {
                        if (toDate.Date >= currentDate.Date)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        obj.LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationId);
                    }
                }
                else if (ApplicationId.StartsWith("PO"))
                {

                    queryString.Append(" Select StaffId , PermissionDate as FromDate , PermissionDate as ToDate from PermissionOff " +
                        "where Id  = @ApplicationId ");
                    try
                    {

                        var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@ApplicationId", ApplicationId)).Select(d => new ACTList()
                        {
                            StaffId = d.StaffId,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate
                        }).FirstOrDefault();

                        staffId = data.StaffId;
                        fromDate = data.FromDate;
                        toDate = data.ToDate;
                        applicationType = "PO";
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                    if (fromDate.Date < currentDate.Date)
                    {
                        if (toDate.Date >= currentDate.Date)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        obj.LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationId);
                    }


                }
                else if (ApplicationId.StartsWith("CO"))
                {

                    queryString.Append(" Select StaffId , COffAvailDate as FromDate , COffAvailDate as ToDate from [COMPENSATORYOFF] " +
                        "where Id = @ApplicationId ");
                    try
                    {

                        var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@ApplicationId", ApplicationId)).Select(d => new ACTList()
                        {
                            StaffId = d.StaffId,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate
                        }).FirstOrDefault();

                        staffId = data.StaffId;
                        fromDate = data.FromDate;
                        toDate = data.ToDate;
                        applicationType = "CO";
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                    if (fromDate.Date < currentDate.Date)
                    {
                        if (toDate.Date >= currentDate.Date)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        obj.LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationId);
                    }
                }

                else if (ApplicationId.StartsWith("OD"))
                {
                    queryString.Append(" Select StaffId , [From] as FromDate , [To] as ToDate from [ODApplication] where " +
                        "Id = @ApplicationId ");

                    try
                    {

                        var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@ApplicationId", ApplicationId)).Select(d => new ACTList()
                        {
                            StaffId = d.StaffId,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate
                        }).FirstOrDefault();

                        staffId = data.StaffId;
                        fromDate = data.FromDate;
                        toDate = data.ToDate;
                        applicationType = "OD";
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                    if (fromDate.Date < currentDate.Date)
                    {
                        if (toDate.Date >= currentDate.Date)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        obj.LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationId);
                    }

                }
                else if (ApplicationId.StartsWith("RH"))
                {

                    queryString.Append(" Select StaffId , ApplicationDate as FromDate , ApplicationDate as ToDate from [RHApplication]" +
                        " where Id = @ApplicationId ");

                    try
                    {

                        var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@ApplicationId", ApplicationId)).Select(d => new ACTList()
                        {
                            StaffId = d.StaffId,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate
                        }).FirstOrDefault();

                        staffId = data.StaffId;
                        fromDate = data.FromDate;
                        toDate = data.ToDate;
                        applicationType = "RH";
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                    if (fromDate.Date < currentDate.Date)
                    {
                        if (toDate.Date >= currentDate.Date)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        obj.LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationId);
                    }


                }
                else if (ApplicationId.StartsWith("OT"))
                {

                    queryString.Append(" Select StaffId , OTDate as FromDate , OTDate as ToDate from [OTApplication] where Id = @ApplicationId ");

                    try
                    {

                        var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), new SqlParameter("@ApplicationId", ApplicationId)).Select(d => new ACTList()
                        {
                            StaffId = d.StaffId,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate
                        }).FirstOrDefault();

                        staffId = data.StaffId;
                        fromDate = data.FromDate;
                        toDate = data.ToDate;
                        applicationType = "OT";
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                    if (fromDate.Date < currentDate.Date)
                    {
                        if (toDate.Date >= currentDate.Date)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        obj.LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, ApplicationId);
                    }

                }


                return "OK";
            }
            catch (Exception)
            {
                return "NOT SAVED";
            }
        }

        public void OnBehalfApplcationCancellationEmailTrigger(string ApplicationId, string UserID)
        {
            string LeaveTypeName = string.Empty;
            string OfficialEmail = string.Empty;
            string StaffName = string.Empty;
            string UserName = string.Empty;
            StringBuilder qryStr = new StringBuilder();
            string StatusCaption = string.Empty;
            UserName = GetStaffName(UserID);
            qryStr.Clear();

            //get the application id and the application type from the application approval table based on the approval id.
            qryStr.Append("select ParentId , ParentType , (SELECT Name FROM ApprovalStatus where  id = A.ApprovalStatusId )" +
                " as ApprovalStatus from ApplicationApproval A where ParentID = @ApplicationId");

            try
            {
                var data = context.Database.SqlQuery<TempData>(qryStr.ToString(), new SqlParameter("@ApplicationId", ApplicationId)).FirstOrDefault();
                //check whether the application details could be fetched.
                if (data == null) //if the application details could not be fetched then...
                {
                    //throw exception.
                    throw new Exception("No record with id " + ApplicationId + "in application approval could be found.");
                }
                else //if the application details is fetched then...
                {
                    //check the value of application type.
                    if (data.ParentType == "LA") //if the application type is a leave application then...
                    {
                        //get the application details from leaveapplication table based on the 
                        //parent id fetched from appliation approval table.
                        qryStr.Clear();
                        qryStr.Append("Select * from LeaveApplicationWabco where id = @ApplicationId ");
                        var dataLAW = context.Database.SqlQuery<LeaveApplicationWabco>(qryStr.ToString(), new SqlParameter("@ApplicationId", ApplicationId)).FirstOrDefault();

                        //check if the application details could be fetched.
                        if (dataLAW == null) //if the application details could not be fetched then...
                        {
                            //throw exception.
                            throw new Exception("No leave application with Id " + ApplicationId + " could be found.");
                        }
                        else //if the application details could be fetched then...
                        {

                            //get the name and the official id of the staff.
                            StaffName = GetStaffName(dataLAW.StaffId);
                            UserName = GetStaffName(UserID);
                            LeaveTypeName = GetLeaveName(dataLAW.LeaveTypeId);
                            OfficialEmail = GetEmailIdOfEmployee(dataLAW.StaffId);
                            if (!string.IsNullOrEmpty(OfficialEmail) == true)
                            {
                                //get the details like the LeaveStartDate and LeaveEndDate and the id of the staff who had applied 
                                //for the leave.

                                qryStr.Clear();
                                qryStr.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your " + LeaveTypeName + "  application from " + Convert.ToDateTime(dataLAW.LeaveStartDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(dataLAW.LeaveEndDate).ToString("dd-MMM-yyyy") + " has been cancelled by " + UserName + " (" + UserID + ").</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>");

                                //send the email to the staff intimating that the application is either approved or rejected.
                                SendEmailMessage(string.Empty, OfficialEmail, string.Empty, string.Empty, "Leave Application - Cancelled.", qryStr.ToString());
                            }
                        }
                    }
                    else if (data.ParentType == "LD") //if the application type is a leave application then...
                    {
                        //get the application details from leaveapplication table based on the 
                        //parent id fetched from appliation approval table.
                        qryStr.Clear();
                        qryStr.Append("Select * from LeaveDonation where Id = @data.ParentId");
                        var dataLAW = context.Database.SqlQuery<RALeaveDonation>(qryStr.ToString(), new SqlParameter("@data.ParentId", data.ParentId)).FirstOrDefault();

                        //check if the application details could be fetched.
                        if (dataLAW == null) //if the application details could not be fetched then...
                        {
                            //throw exception.
                            throw new Exception("No leave application with Id " + data.ParentId + " could be found.");
                        }
                        else //if the application details could be fetched then...
                        {
                            //get the name and the official id of the staff.
                            var repo = new CommonRepository();
                            StaffName = GetStaffName(dataLAW.DonarStaffID);
                            OfficialEmail = GetEmailIdOfEmployee(dataLAW.DonarStaffID);
                            var ReceiverStaffName = repo.GetStaffName(dataLAW.ReceiverStaffID);
                            if (!string.IsNullOrEmpty(OfficialEmail) == true)
                            {
                                //get the details like the LeaveStartDate and LeaveEndDate and the id of the staff who had applied 
                                //for the leave.

                                qryStr.Clear();
                                qryStr.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your Leave Donation application applied for " + ReceiverStaffName + " (" + dataLAW.ReceiverStaffID + ") on " + Convert.ToDateTime(dataLAW.TransactionDate).ToString("dd-MMM-yyyy") + " has been cancelled by " + UserName + " (" + UserID + ").</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>");

                                //send the email to the staff intimating that the application is either approved or rejected.
                                SendEmailMessage(string.Empty, OfficialEmail, string.Empty, string.Empty, "Leave Donation application- Cancelled", qryStr.ToString());
                            }

                        }

                    }
                    else if (data.ParentType == "RH")
                    {
                        //qryStr.Clear();
                        //qryStr.Append("SELECT A.ID AS RHAPPLICATIONID , STAFFID , RHID , APPLICATIONDATE , ISCANCELLED , NAME , RHDATE , RHYEAR , COMPANYID , LEAVEID FROM RHAPPLICATION A INNER JOIN RESTRICTEDHOLIDAYS B ON A.RHID = B.ID where a.id = '" + data.ParentId + "'");
                        //var dat = context.Database.SqlQuery<RHApplication>(qryStr.ToString()).FirstOrDefault();

                        ////check if the application details could be fetched.
                        //if (dat == null) //if the application details could not be fetched then...
                        //{
                        //    //throw exception.
                        //    throw new Exception("No leave application with Id " + data.ParentId + " could be found.");
                        //}
                        //else //if the application details could be fetched then...
                        //{
                        //    //get the name and the official id of the staff.
                        //    StaffName = GetStaffName(dat.StaffId);
                        //    OfficialEmail = GetEmailIdOfEmployee(dat.StaffId);

                        //    //get the details like the LeaveStartDate and LeaveEndDate and the id of the staff who had applied 
                        //    //for the leave.

                        //    qryStr.Clear();
                        //    qryStr.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your leave application for leave to be taken between " + Convert.ToDateTime(data.LeaveStartDate).ToString("dd-MMM-yyyy") + " and " + Convert.ToDateTime(dataLAW.LeaveEndDate).ToString("dd-MMM-yyyy") + " has been " + data.ApprovalStatus + ".</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>");

                        //    //send the email to the staff intimating that the application is either approved or rejected.
                        //    SendEmailMessage(string.Empty, OfficialEmail, string.Empty, string.Empty, "Leave application approved", qryStr.ToString());
                        //}
                    }

                    else if (data.ParentType == "CR")
                    {


                        qryStr.Clear();
                        qryStr.Append("select * from CoffReq where id = @data.ParentId");
                        var dataLAW = context.Database.SqlQuery<CoffReq>(qryStr.ToString(), new SqlParameter("@data.ParentId", data.ParentId)).FirstOrDefault();

                        //check if the application details could be fetched.
                        if (dataLAW == null) //if the application details could not be fetched then...
                        {
                            //throw exception.
                            throw new Exception("No Coff credit application with Id " + data.ParentId + " could be found.");
                        }
                        else //if the application details could be fetched then...
                        {
                            //get the name and the official id of the staff.
                            StaffName = GetStaffName(dataLAW.Staffid);
                            OfficialEmail = GetEmailIdOfEmployee(dataLAW.Staffid);
                            if (!string.IsNullOrEmpty(OfficialEmail) == true)
                            {
                                //get the details like the LeaveStartDate and LeaveEndDate and the id of the staff who had applied 
                                //for the leave.

                                qryStr.Clear();
                                qryStr.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your Compensatory Off Credit Requisition for the date " + Convert.ToDateTime(dataLAW.CoffReqFrom).ToString("dd-MMM-yyyy") + " has been cancelled by " + UserName + " (" + UserID + ").</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>");

                                //send the email to the staff intimating that the application is either approved or rejected.
                                SendEmailMessage(string.Empty, OfficialEmail, string.Empty, string.Empty, "Compensatory Off Credit Requisition - Cancelled", qryStr.ToString());
                            }

                        }
                    }
                    else if (data.ParentType == "PO")
                    {


                        qryStr.Clear();
                        qryStr.Append("select * from permissionoff where id = @data.ParentId");
                        var dataLAW = context.Database.SqlQuery<PermissionOff>(qryStr.ToString(), new SqlParameter("@data.ParentId", data.ParentId)).FirstOrDefault();

                        //check if the application details could be fetched.
                        if (dataLAW == null) //if the application details could not be fetched then...
                        {
                            //throw exception.
                            throw new Exception("No Permissionoff  application with Id " + data.ParentId + " could be found.");
                        }
                        else //if the application details could be fetched then...
                        {
                            //get the name and the official id of the staff.
                            StaffName = GetStaffName(dataLAW.StaffId);
                            OfficialEmail = GetEmailIdOfEmployee(dataLAW.StaffId);
                            if (!string.IsNullOrEmpty(OfficialEmail) == true)
                            {
                                //get the details like the LeaveStartDate and LeaveEndDate and the id of the staff who had applied 
                                //for the leave.

                                qryStr.Clear();
                                qryStr.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your permission  application for the date " + Convert.ToDateTime(dataLAW.PermissionDate).ToString("dd-MMM-yyyy") + " has been cancelled by " + UserName + " (" + UserID + ").</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>");

                                //send the email to the staff intimating that the application is either approved or rejected.
                                SendEmailMessage(string.Empty, OfficialEmail, string.Empty, string.Empty, "Permission Application - Cancelled.", qryStr.ToString());
                            }

                        }
                    }
                    else if (data.ParentType == "MP")
                    {


                        qryStr.Clear();
                        qryStr.Append("select * from manualpunch where id = @data.ParentId");
                        var dataLAW = context.Database.SqlQuery<ManualPunch>(qryStr.ToString(), new SqlParameter("@data.ParentId", data.ParentId)).FirstOrDefault();


                        //check if the application details could be fetched.
                        if (dataLAW == null) //if the application details could not be fetched then...
                        {
                            //throw exception.
                            throw new Exception("No Punch Regularization  application with Id " + data.ParentId + " could be found.");
                        }
                        else //if the application details could be fetched then...
                        {
                            //get the name and the official id of the staff.
                            StaffName = GetStaffName(dataLAW.StaffId);
                            OfficialEmail = GetEmailIdOfEmployee(dataLAW.StaffId);
                            if (!string.IsNullOrEmpty(OfficialEmail) == true)
                            {
                                //get the details like the LeaveStartDate and LeaveEndDate and the id of the staff who had applied 
                                //for the leave.
                                if (dataLAW.PunchType == "IN")
                                {
                                    qryStr.Clear();
                                    qryStr.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your Punch Regularization  application for the Punch Type " + dataLAW.PunchType + " on " + dataLAW.InDateTime + " has been cancelled by " + UserName + " (" + UserID + ").</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>");

                                    //send the email to the staff intimating that the application is either approved or rejected.
                                    SendEmailMessage(string.Empty, OfficialEmail, string.Empty, string.Empty, " Punch Regularization  application - Cancelled", qryStr.ToString());
                                }
                                else if (dataLAW.PunchType == "OUT")
                                {
                                    qryStr.Clear();
                                    qryStr.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your Punch Regularization  application for the Punch Type " + dataLAW.PunchType + " on " + dataLAW.OutDateTime + " has been cancelled by " + UserName + " (" + UserID + ").</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>");

                                    //send the email to the staff intimating that the application is either approved or rejected.
                                    SendEmailMessage(string.Empty, OfficialEmail, string.Empty, string.Empty, " Punch Regularization  application - Cancelled", qryStr.ToString());
                                }
                                else
                                {
                                    qryStr.Clear();
                                    qryStr.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your Punch Regularization  application for the Punch Type " + dataLAW.PunchType + " from " + dataLAW.InDateTime + " to " + dataLAW.OutDateTime + " has been cancelled by " + UserName + " (" + UserID + ").</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>");

                                    //send the email to the staff intimating that the application is either approved or rejected.
                                    SendEmailMessage(string.Empty, OfficialEmail, string.Empty, string.Empty, " Punch Regularization  application - Cancelled", qryStr.ToString());
                                }

                            }

                        }
                    }
                    else if (data.ParentType == "SC")
                    {


                        qryStr.Clear();
                        qryStr.Append("select * from ShiftChangeApplication where id = @data.ParentId");
                        var dataLAW = context.Database.SqlQuery<ShiftChangeApplication>(qryStr.ToString(), new SqlParameter("@data.ParentId", data.ParentId)).FirstOrDefault();

                        //check if the application details could be fetched.
                        if (dataLAW == null) //if the application details could not be fetched then...
                        {
                            //throw exception.
                            throw new Exception("No Shift Change application with Id " + data.ParentId + " could be found.");
                        }
                        else //if the application details could be fetched then...
                        {
                            //get the name and the official id of the staff.
                            StaffName = GetStaffName(dataLAW.StaffId);
                            OfficialEmail = GetEmailIdOfEmployee(dataLAW.StaffId);
                            if (!string.IsNullOrEmpty(OfficialEmail) == true)
                            {
                                //get the details like the LeaveStartDate and LeaveEndDate and the id of the staff who had applied 
                                //for the leave.

                                qryStr.Clear();
                                qryStr.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your Shift Change  application from " + Convert.ToDateTime(dataLAW.FromDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(dataLAW.ToDate).ToString("dd-MMM-yyyy") + " has been cancelled by " + UserName + " (" + UserID + ").</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>");

                                //send the email to the staff intimating that the application is either approved or rejected.
                                SendEmailMessage(string.Empty, OfficialEmail, string.Empty, string.Empty, " Shift Change  application - Cancelled", qryStr.ToString());
                            }

                        }
                    }
                    else if (data.ParentType == "OD")
                    {


                        qryStr.Clear();
                        qryStr.Append("select * from ODapplication where id = @data.ParentId");
                        var dataLAW = context.Database.SqlQuery<ODApplication>(qryStr.ToString(), new SqlParameter("@data.ParentId", data.ParentId)).FirstOrDefault();

                        //check if the application details could be fetched.
                        if (dataLAW == null) //if the application details could not be fetched then...
                        {
                            //throw exception.
                            throw new Exception("No On Duty application with Id " + data.ParentId + " could be found.");
                        }
                        else //if the application details could be fetched then...
                        {
                            //get the name and the official id of the staff.
                            StaffName = GetStaffName(dataLAW.StaffId);
                            OfficialEmail = GetEmailIdOfEmployee(dataLAW.StaffId);
                            if (!string.IsNullOrEmpty(OfficialEmail) == true)
                            {
                                //get the details like the LeaveStartDate and LeaveEndDate and the id of the staff who had applied 
                                //for the leave.
                                if (dataLAW.ODDuration == "MULTIPLEDAYS")
                                {
                                    qryStr.Clear();
                                    qryStr.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your On Duty application from " + Convert.ToDateTime(dataLAW.From).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(dataLAW.To).ToString("dd-MMM-yyyy") + " for " + dataLAW.ODDuration + " has been cancelled by " + UserName + " (" + UserID + ").</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>");
                                }
                                else
                                {
                                    qryStr.Clear();
                                    qryStr.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your On Duty application on " + Convert.ToDateTime(dataLAW.From).ToString("dd-MMM-yyyy") + " for " + dataLAW.ODDuration + " has been cancelled by " + UserName + " (" + UserID + ").</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>");

                                }

                                //send the email to the staff intimating that the application is either approved or rejected.
                                SendEmailMessage(string.Empty, OfficialEmail, string.Empty, string.Empty, " On Duty application - Cancelled", qryStr.ToString());
                            }

                        }
                    }
                    else if (data.ParentType == "CO")
                    {
                        qryStr.Clear();
                        qryStr.Append("select * from compensatoryoff where id = @data.ParentId345");
                        var dataLAW = context.Database.SqlQuery<CompensatoryOff>(qryStr.ToString(), new SqlParameter("@data.ParentId", data.ParentId)).FirstOrDefault();

                        //check if the application details could be fetched.
                        if (dataLAW == null) //if the application details could not be fetched then...
                        {
                            //throw exception.
                            throw new Exception("No Coff  application with Id " + data.ParentId + " could be found.");
                        }
                        else //if the application details could be fetched then...
                        {
                            //get the name and the official id of the staff.
                            StaffName = GetStaffName(dataLAW.StaffId);
                            OfficialEmail = GetEmailIdOfEmployee(dataLAW.StaffId);
                            if (!string.IsNullOrEmpty(OfficialEmail) == true)
                            {
                                //get the details like the LeaveStartDate and LeaveEndDate and the id of the staff who had applied 
                                //for the leave.

                                qryStr.Clear();
                                qryStr.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br/>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your Compensatory Off Requisition from " + Convert.ToDateTime(dataLAW.COffReqDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(dataLAW.COffAvailDate).ToString("dd-MMM-yyyy") + " has been cancelled by " + UserName + " (" + UserID + ").</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>");

                                //send the email to the staff intimating that the application is either approved or rejected.
                                SendEmailMessage(string.Empty, OfficialEmail, string.Empty, string.Empty, "Compensatory Off Requisition - Cancelled", qryStr.ToString());
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ReportingDetails GetReportingDetails(string staffId)
        {
            try
            {
                string qry = string.Empty;
                qry = @" Select ReportingManager as ApprovalOwner1 , Approver2 as ApprovalOwner2 from StaffOfficial 
                 Where StaffId = @StaffId";
                var data = context.Database.SqlQuery<ReportingDetails>(qry, new SqlParameter("@StaffId", staffId)).Select(d => new ReportingDetails()
                {
                    ApprovalOwner1 = d.ApprovalOwner1,
                    ApprovalOwner2 = d.ApprovalOwner2,
                }
                ).FirstOrDefault();
                return data;
            }
            catch
            {
                throw;
            }
        }
        //public AspnetuserFilterDetails GetAspnetuserFilters()
        //{
        //    try
        //    {
        //        string Qry = string.Empty;
        //        AspnetuserFilterDetails model = new AspnetuserFilterDetails();
        //        Qry = @"select v.DeptId Id,v.DeptName Name from AspNetUsers as u
        //        inner  join  STAFFVIEW V ON u.StaffId=v.StaffId     
        //        where v.DeptId is not null and v.DeptName is not null group by v.DeptId,v.DeptName";
        //        model.Department = context.Database.SqlQuery<DropDownStrModel>(Qry).ToList();

        //        Qry = @"Select v.DesignationId Id,v.DesignationName  Name
        //        from AspNetUsers as u
        //        inner  join  STAFFVIEW V ON u.StaffId=v.StaffId
        //        where v.DesignationId is not null and v.DesignationName is not null 
        //        group by v.DesignationId,v.DesignationName ";
        //        model.Designation = context.Database.SqlQuery<DropDownStrModel>(Qry).ToList();

        //        return model;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        public List<ResetPasswordDetails> GetAspNetUsersList(AspnetuserFilterDetails model)
        {
            try
            {
                string Qry = string.Empty;
                List<ResetPasswordDetails> ReturnModel = new List<ResetPasswordDetails>();
                Qry = @"with cte as (select u.Id,u.UserName ,v.StaffId,u.UserFullName,dbo.fnGetStaffName(v.StaffId) StaffName,v.DeptName,v.DesignationName 
                from AspNetUsers as u inner  join  STAFFVIEW V ON u.StaffId=v.StaffId where 1=1 ";

                if (string.IsNullOrEmpty(model.UserId) == false)
                {
                    Qry = string.Concat(Qry, $" and u.UserName='{model.UserId}'");
                }

                if (string.IsNullOrEmpty(model.DepartmentId) == false)
                {
                    Qry = string.Concat(Qry, $" and v.DeptId='{model.DepartmentId}'");
                }

                if (string.IsNullOrEmpty(model.DesignationId) == false)
                {
                    Qry = string.Concat(Qry, $" and v.DesignationId='{model.DesignationId}'");
                }

                Qry = string.Concat(Qry, " ) select * from cte v where 1=1 ");

                if (string.IsNullOrEmpty(model.StaffId) == false)
                {
                    Qry = string.Concat(Qry, $"  and(v.StaffId like '%{model.StaffId}%' or v.StaffName like '%{model.StaffId}%')");
                }
                ReturnModel = context.Database.SqlQuery<ResetPasswordDetails>(Qry).ToList();
                return ReturnModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Changes Made by aarthi on 28/2/2020 for CompOff Availing
        public string GetCommonSenderEmailIdFromEmailSettings()
        {
            var OfficialEmailId = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("Select SenderEmail from EmailSettings");

            try
            {
                OfficialEmailId = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
                return OfficialEmailId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveMobilePunch(DashboardSwipes ds)
        {
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    context.DashboardSwipes.Add(ds);
                    if (ds.TransactionTypeId == 20)
                    {
                        SaveMobileInPunch(Convert.ToDateTime(ds.TransactionTime).ToString("yyyy-MM-dd HH:mm:ss"), ds.StaffId, "");
                    }
                    else
                    {
                        SaveMobileOutPunch(Convert.ToDateTime(ds.TransactionTime).ToString("yyyy-MM-dd HH:mm:ss"), ds.StaffId, "");
                    }
                    context.SaveChanges();
                    Trans.Commit();
                }
                catch (Exception err)
                {
                    Trans.Rollback();
                    throw err;
                }
            }
        }

        public void UpdateLastPunchType(PunchTypeHistory punchTypeHistory)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Update PunchTypeHistory Set LastPunchType = @PunchType Where StaffId = @StaffId ");
            context.Database.ExecuteSqlCommand(stringBuilder.ToString(), new SqlParameter("@PunchType", punchTypeHistory.LastPunchType),
                new SqlParameter("@StaffId", punchTypeHistory.StaffId));
            // context.PunchTypeHistory.AddOrUpdate(punchTypeHistory);
            //context.SaveChanges();
        }

    }
}