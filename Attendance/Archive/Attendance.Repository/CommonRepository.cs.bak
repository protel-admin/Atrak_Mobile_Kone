using Attendance.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

//using System.Web.mvc
namespace Attendance.Repository
{
    public class CommonRepository : IDisposable
    {
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
        private AttendanceManagementContext context = null;

        public CommonRepository()
        {
            context = new AttendanceManagementContext();
        }

        public string ValidateMOffApplication(string StaffId, string FromDate, string ToDate, string LeaveTypeId, bool IsFixed)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            var str = "0";
            if (IsFixed == true)
                str = "1";

            qryStr.Append("SELECT DBO.fnValidateMOffApplication('" + StaffId + "','" + FromDate + "','" + ToDate + "' , '" + LeaveTypeId + "' , '" + str + "')");
            var ret = (context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault()).ToString();
            return ret;
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
            qryStr.Append("exec spLeaveBalanceHandler '" + ApprovalId + "','" + ApprovalStatus + "' ");
            try
            {
                var lst = (context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault()).ToString();
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
            qryStr.Append("select dbo.fngetmastername('" + StaffId + "','" + Mastdt + "')");

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

        public string ValidateApplicationOverlaping(string staffId, DateTime startDate, int startDurationId, DateTime endDate,
            int endDurationId)
        {
            string overlappingValidationMessage = string.Empty;
            SqlParameter[] sqlParameter = new SqlParameter[5];
            sqlParameter[0] = new SqlParameter("@StaffId", staffId);
            sqlParameter[1] = new SqlParameter("@StartDurationId", startDurationId);
            sqlParameter[2] = new SqlParameter("@StartDate",System.Data.SqlDbType.Date);
            sqlParameter[2].Value = startDate.ToString("dd") + "-" + startDate.ToString("MMM") + "-" + startDate.ToString("yyyy");
            sqlParameter[3] = new SqlParameter("@EndDate", System.Data.SqlDbType.Date);
            sqlParameter[3].Value= endDate.ToString("dd") + "-" + endDate.ToString("MMM") + "-" + endDate.ToString("yyyy");
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

        public string ValidateApplicationForPayDate(DateTime startDate, DateTime endDate)
        {

            string payPeriodValidationMessage = string.Empty;
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@StartDate", startDate);
            sqlParameter[1] = new SqlParameter("@EndDate", endDate);
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append(" Select [dbo].[fnValidateApplicationForPayPeriod](@StartDate, @EndDate )");
            payPeriodValidationMessage = (context.Database.SqlQuery<string>(queryString.ToString(), sqlParameter).FirstOrDefault()).ToString();
            return payPeriodValidationMessage;
        }

        public string ValidateApplication(string StaffId, string FromDate, string ToDate, string LeaveTypeId, decimal LeaveBalance)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT DBO.fnValidateLeaveApplication('" + StaffId + "','" + FromDate + "','" + ToDate + "' , '" + LeaveTypeId + "'," + LeaveBalance + ")");
            var ret = (context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault()).ToString();
            return ret;
        }

        public string GetApprovalStatus(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select ass.Name from approvalstatus ass inner join applicationapproval ap on ap.ApprovalStatusId = ass.Id inner join staff st on  '" + staffid + "' = ap.parentid ");
            try
            {
                var bal = (context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault()).ToString();

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


        public LoggedInUserDetails GetDomainIdBasedDetails(string domainId)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();

            QryStr.Append("SELECT DomainId , StaffId , dbo.fnGetStaffName(StaffId) as [StaffFullName] , CompanyId , BranchId ," +
                " DeptId as DepartmentId  , SecurityGroupId , ApproverLevel ,REPORTINGMGRID as ApproverId , DBO.FNGETSTAFFNAME(REPORTINGMGRID)" +
                " AS ApproverName , GradeName , CompanyName , BranchName , DeptName , OfficialPhone, LocationId ,Reviewer as ReviewerId," +
                " 1ReviewerName , IsMobileAppEligible from StaffView where DomainId = @DomainId ");
            try
            {
                var UsrDet = context.Database.SqlQuery<LoggedInUserDetails>(QryStr.ToString(), new SqlParameter("@DomainId", domainId)).
                    Select(d => new LoggedInUserDetails
                {
                    DomainId = d.DomainId,
                    StaffId = d.StaffId,
                    StaffFullName = d.StaffFullName,
                    SecurityGroupId = d.SecurityGroupId,
                    ApprovalLevel = d.ApprovalLevel,
                    ApproverId = d.ApproverId,
                    ApproverName = d.ApproverName,
                    CompanyId = d.CompanyId,
                    LocationId = d.LocationId,
                    BranchId = d.BranchId,
                    DepartmentId = d.DepartmentId,
                    GradeName = d.GradeName,
                    CompanyName = d.CompanyName,
                    BranchName = d.BranchName,
                    DeptName = d.DeptName,
                    OfficialPhone = d.OfficialPhone,
                    ReviewerId = d.ReviewerId,
                        ReviewerName = d.ReviewerName,
                        IsMobileApplicationEligible = d.IsMobileApplicationEligible
                }).FirstOrDefault();

                return UsrDet;
            }
            catch (Exception)
            {
                return new LoggedInUserDetails();
            }
        }

        public LoggedInUserDetails GetUserIdBasedDetails(string staffId)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();

            QryStr.Append("SELECT DomainId ,  [DBO].[GetStaffEmailId](StaffId) as UserEmailId, [DBO].[GetStaffEmailId](Reviewer)" +
                " as ReviewerEmailId, [DBO].[GetStaffEmailId](REPORTINGMGRID) as ReportingManagerEmailId , StaffId , " +
                "DBO.FNGETSTAFFNAME(StaffId) AS StaffFullName ,  CompanyId , BranchId , DeptId as DepartmentId , LocationId," +
                "SecurityGroupId ,ApproverLevel, REPORTINGMGRID AS ApproverId , DBO.FNGETSTAFFNAME(REPORTINGMGRID) AS ApproverName , " +
                "Reviewer AS ReviewerId , DBO.FNGETSTAFFNAME(Reviewer) AS ReviewerName , GradeName , CompanyName , BranchName , " +
                "DeptName , OFFICIALPHONE , IsMobileAppEligible FROM  StaffView WHERE StaffId = @StaffId ");

            try
            {
                var UsrDet = context.Database.SqlQuery<LoggedInUserDetails>(QryStr.ToString(), new SqlParameter("@StaffId", staffId)).Select(d => new LoggedInUserDetails
                {
                    DomainId = d.DomainId,
                    StaffId = d.StaffId,
                    StaffFullName = d.StaffFullName,
                    SecurityGroupId = d.SecurityGroupId,
                    ApprovalLevel = d.ApprovalLevel,
                    ApproverId = d.ApproverId,
                    ApproverName = d.ApproverName,
                    CompanyId = d.CompanyId,
                    BranchId = d.BranchId,
                    DepartmentId = d.DepartmentId,
                    GradeName = d.GradeName,
                    CompanyName = d.CompanyName,
                    BranchName = d.BranchName,
                    DeptName = d.DeptName,
                    OfficialPhone = d.OfficialPhone,
                    UserEmailId = d.UserEmailId,
                    ApproverEmailId = d.ApproverEmailId,
                    ReviewerId = d.ReviewerId,
                    ReviewerName = d.ReviewerName,
                    ReviewerEmailId = d.ReviewerEmailId,
                    LocationId = d.LocationId,
                    IsMobileApplicationEligible = d.IsMobileApplicationEligible
                }).FirstOrDefault();

                return UsrDet;
            }
            catch (Exception)
            {
                return new LoggedInUserDetails();
            }
        }
        public List<UserList> ReqisterList()
        {
            var Qrstr = new StringBuilder();
            Qrstr.Clear();
            Qrstr.Append("Select A.StaffId,B.FirstName,A.UserName,B.CompanyName,B.DeptName,B.REPMGRFIRSTNAME from aspnetusers A inner join staffview B on A.staffid=B.StaffId");
            var Data = context.Database.SqlQuery<UserList>(Qrstr.ToString()).ToList();
            return Data;

        }

        public List<RoleList> GetRoleList()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT [Id] , [Name] FROM [SecurityGroup] Where IsActive = 1");

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
            qryStr.Append("select count ( * ) as totalcount from staffofficial where reportingmanager = '" + StaffId + "'");
            try
            {
                var count = context.Database.SqlQuery<int>(qryStr.ToString()).First();
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
            qryStr.Append("select reportingmgrid from staffview where staffid = '" + staffid + "'");

            try
            {
                reportingmanager = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
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
            qryStr.Append("Select ReportingManagerId from  [TeamHierarchy] where StaffId= '" + staffid + "'");

            try
            {
                reportingmanager = context.Database.SqlQuery<string>(qryStr.ToString()).ToArray();
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
            qryStr.Append("select dbo.fnGetStaffName('" + StaffId + "')");

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

        public string GetLocationId(string StaffId)
        {
            var StaffName = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select locationId from StaffOfficial where staffid='" + StaffId + "'");

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
            qryStr.Append("select approvalowner from ApplicationApproval where ParentId= '" + Id + "'");

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

        public string GetOTApprover(string Id)
        {
            var OTApprover = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select OTReportingManager from StaffOfficial where staffid= '" + Id + "'");

            try
            {
                OTApprover = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
                return OTApprover;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetOTReviewer(string Id)
        {
            var OTReviewer = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select OTReviewer from StaffOfficial where staffid= '" + Id + "'");

            try
            {
                OTReviewer = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
                return OTReviewer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OTApplication GetOTDetails(string Id)
        {
            return context.OTApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }


        public string GetStaffReviewer(string Id)
        {
            var StaffName = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select Reviewer from StaffOfficial where staffid= '" + Id + "'");

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

        public string GetReviewerOwner(string Id)
        {
            var StaffName = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("Select Reviewerowner from ApplicationApproval where ParentId= '" + Id + "'");

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

        public string GetLeaveName(string leaveTypeId)
        {
            string leaveName = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("Select Name from LeaveType where Id = ('" + leaveTypeId + "')");

            try
            {
                leaveName = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
                return leaveName;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetNewShiftName(string NewShiftId)
        {
            var ShiftName = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("SELECT SHORTNAME + ' - ' + NAME FROM SHIFTS WHERE ID = '" + NewShiftId + "'");

            try
            {
                ShiftName = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
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
            qryStr.Append("Select ShortName,StartTime,EndTime from [Shifts] where Id='" + ShiftId + "'");

            try
            {
                var lst = context.Database.SqlQuery<ShiftDetailsForShiftChange>(qryStr.ToString()).Select(d => new ShiftDetailsForShiftChange()
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
        public string GetEmailIdOfEmployee(string staffId)
        {
            string officialEmailId = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select [Email] from staffofficial where staffid = '" + staffId + "'");

            try
            {
                officialEmailId = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
                return officialEmailId;
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
                "[SentCounter]) VALUES ('" + pFrom + "','" + pTo + "','" + pCC + "'," +
                "'" + pBCC + "','" + EmailSubject + "','" + EmailBody + "'," +
                "getdate(),'-',0," +
                "getdate(),0,''," +
                "0)");

            try
            {
                context.Database.ExecuteSqlCommand(InsQry.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendEmailMessageForApplication(string pFrom, string pTo, string pCC, string pBCC, string EmailSubject, string EmailBody, string CreatedBy)
        {
            StringBuilder InsQry = new StringBuilder();

            InsQry.Append("INSERT INTO [EmailSendLog] ([From],[To],[CC]," +
                "[BCC],[EmailSubject],[EmailBody]," +
                "[CreatedOn],[CreatedBy],[IsSent]," +
                "[SentOn],[IsError],[ErrorDescription]," +
                "[SentCounter]) VALUES ('" + pFrom + "','" + pTo + "','" + pCC + "'," +
                "'" + pBCC + "','" + EmailSubject + "','" + EmailBody + "'," +
                "getdate(),'" + CreatedBy + "',0," +
                "getdate(),0,''," +
                "0)");

            try
            {
                context.Database.ExecuteSqlCommand(InsQry.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendEmailToStaff(string ApprovalId)
        {
            string FromEmail = string.Empty;
            string OfficialEmail = string.Empty;
            string StaffName = string.Empty;
            StringBuilder qryStr = new StringBuilder();
            string StatusCaption = string.Empty;
            qryStr.Clear();

            //get the application id and the application type from the application approval table based on the approval id.
            qryStr.Append("select ParentId , ParentType , (SELECT Name FROM ApprovalStatus where  id = A.ApprovalStatusId ) as ApprovalStatus from ApplicationApproval A where ParentID = '" + ApprovalId + "'");

            try
            {
                var data = context.Database.SqlQuery<TempData>(qryStr.ToString()).FirstOrDefault();
                //check whether the application details could be fetched.
                if (data == null) //if the application details could not be fetched then...
                {
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
                        qryStr.Append("select * from LeaveApplicationWabco where id = '" + data.ParentId + "'");
                        var dataLAW = context.Database.SqlQuery<LeaveApplicationWabco>(qryStr.ToString()).FirstOrDefault();

                        //check if the application details could be fetched.
                        if (dataLAW == null) //if the application details could not be fetched then...
                        {
                            throw new Exception("No leave application with Id " + data.ParentId + " could be found.");
                        }
                        else //if the application details could be fetched then...
                        {
                            //get the name and the official id of the staff.
                            StaffName = GetStaffName(dataLAW.StaffId);
                            FromEmail = GetEmailFromAdd();
                            OfficialEmail = GetEmailIdOfEmployee(dataLAW.StaffId);
                            if (!string.IsNullOrEmpty(OfficialEmail) == true)
                            {
                                //get the details like the LeaveStartDate and LeaveEndDate and the id of the staff who had applied 
                                //for the leave.

                                qryStr.Clear();
                                qryStr.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your leave application for leave to be taken between " + Convert.ToDateTime(dataLAW.LeaveStartDate).ToString("dd-MMM-yyyy") + " and " + Convert.ToDateTime(dataLAW.LeaveEndDate).ToString("dd-MMM-yyyy") + " has been " + data.ApprovalStatus + ".</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>");

                                //send the email to the staff intimating that the application is either approved or rejected.
                                SendEmailMessage(FromEmail, OfficialEmail, string.Empty, string.Empty, "Leave application approved", qryStr.ToString());
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
                queryString.Append(" Select StaffId , ApplicationDate as FromDate , ApplicationDate as ToDate from [RHApplication] where Id = '" + ApplicationApprovalId + "' ");

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

        public void LogIntoIntoAttendanceControlTable(string StaffId, DateTime FromDate, DateTime ToDate, string ApplicationType, string ApplicationId)
        {
            DateTime currentDate = DateTime.Now;
            string UserFullName = string.Empty;

            UserFullName = "Admin";
            AttendanceControlTable attendanceControlTable = new AttendanceControlTable();
            attendanceControlTable.StaffId = StaffId;
            attendanceControlTable.FromDate = FromDate;
            attendanceControlTable.ToDate = ToDate;
            attendanceControlTable.IsProcessed = false;
            attendanceControlTable.CreatedOn = DateTime.Now;
            attendanceControlTable.CreatedBy = UserFullName;
            attendanceControlTable.ApplicationType = ApplicationType;
            attendanceControlTable.ApplicationId = ApplicationId;
            context.AttendanceControlTable.AddOrUpdate(attendanceControlTable);
            context.SaveChanges();
        }
        
        public void SaveIntoApplicationApproval(string parentid, string parenttype, string loggedInUserId, string reportingmanager, bool SelfApproval)
        {
            var maxid = string.Empty;
            var lastid = string.Empty;
            var aa = new ApplicationApproval();

            var mr = new MasterRepository();
            maxid = mr.getmaxid("ApplicationApproval", "id", "AA", "", 10, ref lastid);
            aa.Id = maxid;
            aa.ParentId = parentid;
            aa.ApprovedOn = DateTime.Now;
            aa.ParentType = parenttype;
            if (SelfApproval == true)
            {
                aa.ApprovalStatusId = 2;
                aa.ApprovedBy = loggedInUserId;
                aa.Comment = "SELF APPROVAL";
                aa.ApprovalOwner = loggedInUserId;
            }
            else
            {
                aa.ApprovalStatusId = 1;
                aa.ApprovedBy = reportingmanager;
                aa.Comment = "--";
                aa.ApprovalOwner = reportingmanager;
                aa.ReviewerOwner = reportingmanager;
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
                var queryString = new StringBuilder();
                queryString.Clear();

                queryString.Append("Insert into [SMaxTransaction] (Tr_Date,Tr_Time,Tr_TType,Tr_Message,Tr_NodeId,Tr_OpName,Tr_CardNumber,Tr_TrackCard,Tr_Reason,Tr_LnId,Tr_IPAddress,Tr_ChId,Tr_Unit,SMAX_Id,DE_NAME,DE_READERTYPE,TR_ID,Tr_Created,IsProcessed,LocationId,Tr_SourceCreatedOn)" +
                    " Values ('" + InTime + "','" + InTime + "','20','Access Granted','1','IN','" + cardNumber + "','1','1','1','192.168.0.223','" + StaffId + "','1','','MANUAL PUNCH','IN','1',GETDATE(),0,'" + LocationId + "',GETDATE())");
                context.Database.ExecuteSqlCommand(queryString.ToString());
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public void SaveOutPunch(string OutTime, string StaffId, string LocationId)
        {
            DateTime actualOutTime = Convert.ToDateTime(OutTime);
            var SwipeType = "Out";
            DeleteDupilcateSwipes(StaffId, OutTime, OutTime, SwipeType);
            try
            {
                var cardNumber = "-";
                var queryString2 = new StringBuilder();
                queryString2.Clear();
                queryString2.Append("Insert into [SMaxTransaction] (Tr_Date,Tr_Time,Tr_TType,Tr_Message,Tr_NodeId,Tr_OpName,Tr_CardNumber,Tr_TrackCard,Tr_Reason,Tr_LnId,Tr_IPAddress,Tr_ChId,Tr_Unit,SMAX_Id,DE_NAME,DE_READERTYPE,TR_ID,Tr_Created,IsProcessed,LocationId,Tr_SourceCreatedOn) " +
                    " Values ('" + OutTime + "','" + OutTime + "','36','Access Granted','1','OUT','" + cardNumber + "','1','1','1','192.168.0.223','" + StaffId + "','1','0','MANUAL PUNCH','OUT','1',GETDATE(),0, '" + LocationId + "',GETDATE())");
                context.Database.ExecuteSqlCommand(queryString2.ToString());
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public void DeleteTrasaction(ClassesToSave CTS)
        {
            string Indatetime = DateTime.ParseExact(CTS.RA.StartDate.ToString(), "dd/MM/yyyy HH:mm:ss", null).ToString("dd/MMM/yyyy HH:mm:ss");
            string Outdatetime = DateTime.ParseExact(CTS.RA.EndDate.ToString(), "dd/MM/yyyy HH:mm:ss", null).ToString("dd/MMM/yyyy HH:mm:ss");
            try
            {
                var queryString = new StringBuilder();
                var queryString1 = new StringBuilder();
                queryString.Clear();
                queryString1.Clear();
                if (CTS.RA.PunchType == "In")
                {
                    queryString.Append("Delete from [SmaxTransaction] Where tr_chid='" + CTS.RA.StaffId + "' and tr_date= '" + Indatetime + "'  and Tr_IPAddress='192.168.0.223' and Upper(Tr_OpName)='IN'");
                    context.Database.ExecuteSqlCommand(queryString.ToString());
                }
                if (CTS.RA.PunchType == "Out")
                {
                    queryString.Append("Delete from [SmaxTransaction] Where tr_chid='" + CTS.RA.StaffId + "' and tr_date= '" + Outdatetime + "' and Tr_IPAddress='192.168.0.223' and Upper(Tr_OpName) = 'OUT' ");
                    context.Database.ExecuteSqlCommand(queryString.ToString());
                }
                if (CTS.RA.PunchType == "InOut")
                {
                    queryString.Append("Delete from [SmaxTransaction] Where tr_chid='" + CTS.RA.StaffId + "' and tr_date= '" + Indatetime + "' and Tr_IPAddress='192.168.0.223' and Upper(Tr_OpName) in ('IN','OUT') ");
                    context.Database.ExecuteSqlCommand(queryString.ToString());
                    queryString1.Append("Delete from [smaxtransaction] where tr_chid='" + CTS.RA.StaffId + "' and tr_date= '" + Outdatetime + "'  and Tr_IPAddress='192.168.0.223' and Upper(Tr_OpName) in ('IN','OUT') ");
                    context.Database.ExecuteSqlCommand(queryString1.ToString());
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public void SaveInOutPunch(string InTime, string OutTime, string StaffId, string LocationId)
        {
            var cardNumber = "-";
            DeleteDupilcateSwipes(StaffId, InTime, OutTime, "InOut");
            try
            {
                var queryString2 = new StringBuilder();
                queryString2.Clear();
                queryString2.Append("Insert into [SMaxTransaction] Values ('" + InTime + "','" + InTime + "','20','Access Granted','1','IN','" + cardNumber + "','0','0','1','192.168.0.223','" + StaffId + "','1','','MANUAL PUNCH','IN','1',GETDATE(),0, '" + LocationId + "',GETDATE())");
                context.Database.ExecuteSqlCommand(queryString2.ToString());

                var queryString3 = new StringBuilder();
                queryString3.Clear();
                queryString3.Append("Insert into [SMaxTransaction] Values ('" + OutTime + "','" + OutTime + "','36','Access Granted','1','OUT','" + cardNumber + "','1','1','1','192.168.0.223','" + StaffId + "','1','','MANUAL PUNCH','OUT','1',GETDATE(),0, '" + LocationId + "',GETDATE())");
                context.Database.ExecuteSqlCommand(queryString3.ToString());

            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public void DeleteDupilcateSwipes(string staffId, string inTime, string outTime, string swipeType)
        {
            try
            {
                var RA = new RequestApplication();
                StringBuilder stringBuilder1 = new StringBuilder();
                StringBuilder stringBuilder2 = new StringBuilder();
                string swipeTime = string.Empty;
                stringBuilder1.Clear();
                stringBuilder2.Clear();
                if (swipeType == "In" || swipeType == "Out")
                {
                    stringBuilder1.Append("Exec [DBO].[DeleteDuplicateSwipes] @StaffId ,@SwipeType ,@SwipeTime,@IpAddress");
                    context.Database.ExecuteSqlCommand(stringBuilder1.ToString(), new SqlParameter("@StaffId", staffId)
                      , new SqlParameter("@SwipeType", swipeType), new SqlParameter("@SwipeTime", swipeTime)
                      , new SqlParameter("@IpAddress", "192.168.0.223"));
                }
                else
                {
                    stringBuilder1.Append("Exec [DBO].[DeleteDuplicateSwipes] @StaffId ,@SwipeType ,@SwipeTime,@IpAddress");
                    context.Database.ExecuteSqlCommand(stringBuilder1.ToString(), new SqlParameter("@StaffId", staffId)
                    , new SqlParameter("@SwipeType", "In"), new SqlParameter("@SwipeTime", inTime)
                    , new SqlParameter("@IpAddress", "192.168.0.223"));
                    stringBuilder2.Append("Exec [DBO].[DeleteDuplicateSwipes] @StaffId ,@SwipeType ,@SwipeTime,@IpAddress");
                    context.Database.ExecuteSqlCommand(stringBuilder1.ToString(), new SqlParameter("@StaffId", staffId)
                    , new SqlParameter("@SwipeType", "Out"), new SqlParameter("@SwipeTime", outTime)
                    , new SqlParameter("@IpAddress", "192.168.0.223"));
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public string GetAccessLevel(string staffId)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("Select A.Value From RuleGroupTxn A Inner Join[Rule] B on A.RuleId = B.Id Inner Join StaffOfficial SO on " +
                " A.LocationId = SO.Locationid  Where SO.StaffId = @StaffId And B.name = 'EmployeeAccessLevel'" +
                "And A.rulegroupid = SO.PolicyId ");
            var data = context.Database.SqlQuery<string>(QryStr.ToString(), new SqlParameter("@StaffId", staffId)).FirstOrDefault();
            return data;
        }
        public DateTime GetRestrictionAppDate(string StaffId)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Value from Settings where Parameter = 'Date for Restrict the Application'");
            var date = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
            DateTime rdate = Convert.ToDateTime(date);
            return rdate;
        }

        public List<SubordinateList> GetSubordinateTreeList(string StaffId)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            // below stored procedure call has to be made by using parameters and not the way it been done now. pls remember to change.
            queryString.Append("exec [DBO].[GetSubordinateTree] '" + StaffId + "'");
            var _LST_ = context.Database.SqlQuery<SubordinateList>(queryString.ToString()).Select(d => new SubordinateList()
            {
                StaffId = d.StaffId,
                StaffName = d.StaffName,
                SubordinateCount = d.SubordinateCount
            }).ToList();
            return _LST_;
        }

        public List<HeadCountOverAll> GetHeadCountData(string GroupNo, string LocId, string DeptId, string DesgId, string GradeId, string ShiftId, string StaffId, string Date)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("exec [DBO].[GetHeadCountsNew] '" + GroupNo + "','" + LocId + "','" + DeptId + "','" + DesgId + "','" + GradeId + "','" + ShiftId + "','" + StaffId + "','" + Date + "'");
            ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 360;
            var _LST_ = context.Database.SqlQuery<HeadCountOverAll>(queryString.ToString()).Select(d => new HeadCountOverAll()
            {
                DepartmentId = d.DepartmentId,
                DesignationId = d.DesignationId,
                LocationId = d.LocationId,
                ShiftId = d.ShiftId,
                DepartmentName = d.DepartmentName,
                LocationName = d.LocationName,
                DesignationName = d.DesignationName,
                ShiftName = d.ShiftName,
                HeadCount = d.HeadCount,
                PresentCount = d.PresentCount,
                AbsentCount = d.AbsentCount,
                GroupNo = d.GroupNo,
                LocSeq = d.LocSeq,
                DeptSeq = d.DeptSeq,
                DesgSeq = d.DesgSeq,
                GradeSeq = d.GradeSeq,
                Seq = d.Seq
            }).ToList();
            return _LST_;
        }

        public List<HeadCountViewModel> GetHeadCountDashlet(string ReportingManagerId)
        {
            List<HeadCountViewModel> result = new List<HeadCountViewModel>();
            try
            {
                string Qry = string.Empty;
                Qry = $@"EXEC [DBO].[GetLiveHeadCount] '{ReportingManagerId}'";
                result = context.Database.SqlQuery<HeadCountViewModel>(Qry).Select(x => new HeadCountViewModel()
                {
                    RMId = x.RMId,
                    LocationId = x.LocationId,
                    LocationName = x.LocationName,
                    DepartmentId = x.DepartmentId
                ,
                    DepartmentName = x.DepartmentName,
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName
                ,
                    ShiftId = x.ShiftId,
                    ShifName = x.ShifName,
                    TotalHeadCount = x.TotalHeadCount,
                    ViolationCount = x.ViolationCount
                ,
                    PresentCount = x.PresentCount,
                    AbsentCount = x.AbsentCount,
                    PresentPercentage = x.PresentPercentage,
                    AbsentPercentage = x.AbsentPercentage


                }).ToList();
                return result;
            }
            catch (Exception e)
            {
                string message = e.Message;
                return result;
            }

        }

        public List<HeadCountOverAll> GetHeadCountDataNew(string GroupNo, string DeptId, string DesgId, string GradeId, string ShiftId, string StaffId, string Date)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("exec [DBO].[GetHeadCountsNew] '" + GroupNo + "','" + DeptId + "','" + DesgId + "','" + GradeId + "','" + ShiftId + "','" + StaffId + "','" + Date + "'");
            ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
            var _LST_ = context.Database.SqlQuery<HeadCountOverAll>(queryString.ToString()).Select(d => new HeadCountOverAll()
            {
                DepartmentId = d.DepartmentId,
                DesignationId = d.DesignationId,
                LocationId = d.LocationId,
                ShiftId = d.ShiftId,
                DepartmentName = d.DepartmentName,
                LocationName = d.LocationName,
                DesignationName = d.DesignationName,
                ShiftName = d.ShiftName,
                HeadCount = d.HeadCount,
                PresentCount = d.PresentCount,
                AbsentCount = d.AbsentCount,
                GroupNo = d.GroupNo,
                LocSeq = d.LocSeq,
                DeptSeq = d.DeptSeq,
                DesgSeq = d.DesgSeq,
                GradeSeq = d.GradeSeq,
                Seq = d.Seq
            }).ToList();
            return _LST_;
        }

        public ACTList GetList(string Id)
        {
            try
            {
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append(" Select StaffId ,StartDate as FromDate , EndDate as ToDate from  RequestApplication where Id = '" + Id + "' ");
                var data = context.Database.SqlQuery<ACTList>(queryString.ToString()).Select(d => new ACTList()
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

        public List<UserList> GetDetailsForUserRegistration()
        {
            var Qrstr = new StringBuilder();
            Qrstr.Clear();
            Qrstr.Append("Select A.StaffId,B.FirstName,A.UserName,B.CompanyName,B.DeptName,B.REPMGRFIRSTNAME from aspnetusers A inner join staffview B on A.staffid=B.StaffId");
            var Data = context.Database.SqlQuery<UserList>(Qrstr.ToString()).ToList();
            return Data;

        }

        public string CheckIsEmployeeExists(string StaffId)
        {
            try
            {
                string name = string.Empty;
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select FirstName from Staff Where Id = '" + StaffId + "'");
                name = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
                return name;
            }
            catch
            {
                return "";
            }

        }

        public string GetOfficialEmail(string StaffId)
        {
            try
            {
                string phoneNumber = string.Empty;
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select email from StaffOfficial Where StaffId = '" + StaffId + "'");
                phoneNumber = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
                return phoneNumber;
            }
            catch
            {
                return "";
            }

        }

        public int HolidayGroupId(string StaffID)
        {
            try
            {
                var str = new StringBuilder();
                str.Append("Select HolidayGroupId from StaffOfficial Where StaffId = '" + StaffID + "'");
                int Employeegroup = context.Database.SqlQuery<int>(str.ToString()).FirstOrDefault();
                return Employeegroup;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetUserName(string StaffId)
        {
            try
            {
                string username = string.Empty;
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select UserName from AtrakUserDetails Where StaffId = '" + StaffId + "'");
                username = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
                return username;
            }
            catch
            {
                return "";
            }

        }

        public string SendPasswordDetailsToUserMail(string StaffId, string Password)
        {
            try
            {
                string StaffName = string.Empty;
                string From = string.Empty;
                string OfficialEmail = string.Empty;
                var EmailString = new StringBuilder();


                StaffName = GetStaffName(StaffId);
                From = GetEmailFromAdd();
                OfficialEmail = GetEmailIdOfEmployee(StaffId);
                var pw = Decrypt(Password);
                if (!string.IsNullOrEmpty(OfficialEmail).Equals(true))
                {
                    EmailString.Clear();
                    EmailString.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + "  ( " + StaffId + ") <br/>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\"> Please find the below password for LMS as per your request.</p><br/><p style=\"font-family:tahoma; font-size:9pt;\"> " + pw + "</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>");
                    SendEmailMessage(From, OfficialEmail, string.Empty, string.Empty, "Forgot Password Requisition of " + StaffName + "", EmailString.ToString());
                }
                return "OK";
            }
            catch (Exception err)
            {
                throw err;
            }
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

        public string SendPasswordDetailsToHRMail(string LoggedUser, string StaffId, string Password)
        {
            try
            {
                string StaffName = string.Empty;
                string From = string.Empty;
                string OfficialEmail = string.Empty;
                string UserName = string.Empty;
                var EmailString = new StringBuilder();

                StaffName = GetStaffName(LoggedUser);
                UserName = GetStaffName(StaffId);
                From = GetEmailFromAdd();
                OfficialEmail = GetEmailIdOfEmployee(LoggedUser);
                if (!string.IsNullOrEmpty(OfficialEmail).Equals(true))
                {
                    EmailString.Clear();
                    EmailString.Append("<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + "  ( " + StaffId + ") <br/>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\"> Please find the below password for " + UserName + "(" + StaffId + ") as per your request.</p><br/><p style=\"font-family:tahoma; font-size:9pt;\">The current password is -   " + Password + "</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>");
                    SendEmailMessage(From, OfficialEmail, string.Empty, string.Empty, "Password Reset for " + StaffName + "", EmailString.ToString());
                }
                return "OK";
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public string GetPasswordForUserName(string StaffId)
        {

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    string password = string.Empty;
                    var queryString = new StringBuilder();
                    queryString.Clear();
                    //queryString.Append("Select Password from [AtrakUserDetails] Where StaffId = '" + StaffId + "'");
                    string qry = $"Select Password from [AtrakUserDetails] a , staffofficial b  Where a.StaffId = b.staffid and a.staffid = '{StaffId}' and b.IsMobileAppEligible = 1";


                    // password = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
                    password = context.Database.SqlQuery<string>(qry).FirstOrDefault();
                    return password;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public void SaveUserDetails(AtrakUserDetails AUT)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    context.AtrakUserDetails.Add(AUT);
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception err)
                {
                    trans.Rollback();
                    throw new Exception(err.Message.ToString());
                }
            }
        }

        public void SavePasswordChangeInAtrakUserDetails(ChangeDetailsPW PCD)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    var queryString1 = new StringBuilder();
                    var queryString2 = new StringBuilder();
                    queryString1.Clear();
                    queryString2.Clear();
                    queryString2.Append("Update [AtrakUserDetails] set Password='" + PCD.NewPassword + "' where StaffId = '" + PCD.StaffId + "' AND IsActive = 1");
                    context.Database.ExecuteSqlCommand(queryString2.ToString());
                    context.PasswordChangeDetails.Add(PCD);
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception err)
                {
                    trans.Rollback();
                    throw err;
                }
            }
        }
        public string GetEmailFromAdd()
        {
            try
            {
                string SendEmail = string.Empty;
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("select SenderEmail from EmailSettings");
                SendEmail = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
                return SendEmail;
            }
            catch
            {
                return "";
            }
        }

        public string GetEmailCCAdd()
        {
            try
            {
                string SendEmail = string.Empty;
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("Select Value From Settings Where Parameter = 'CC_Email_ID'");
                SendEmail = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
                return SendEmail;
            }
            catch
            {
                return "";
            }
        }

        public string GetCCAddress(string Application, string LocationId)
        {
            try
            {
                string SendEmail = string.Empty;
                var querystring = new StringBuilder();
                querystring.Append("select CCAddress from emailforwardingconfig where LocationID='" + LocationId + "' and ScreenID='" + Application + "'");
                SendEmail = context.Database.SqlQuery<string>(querystring.ToString()).FirstOrDefault();
                return SendEmail;
            }
            catch
            {
                return "";
            }
        }

        public string Decode(string Reqid)
        {
            byte[] encoded = Convert.FromBase64String(Reqid);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }

        public string GetPolicyValue(string PolicyId)
        {
            try
            {
                string Policyid = string.Empty;
                var queryString = new StringBuilder();
                queryString.Clear();
                queryString.Append("select Value from Settings where id in ('" + PolicyId + "')");
                Policyid = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
                return Policyid;
            }
            catch (Exception e)
            {
                throw;
            }
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

        #region Coff Req Availing
        public string GetEmailIdOfStaff(string StaffId)
        {
            var OfficialEmailId = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select [Email] from staffofficial where staffid = '" + StaffId + "'");
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
        #endregion

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

        public string CheckIsOTorCompOffApproved(string StaffId, string ApplicationDate)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append(" Select [dbo].[Check_Is_Coff_OT_Applied]('" + StaffId + "' , '" + ApplicationDate + "')");
            var msg = (context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault()).ToString();
            return msg;
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

    }
}
