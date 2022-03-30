using Attendance.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class RACoffAvailingApplicationRepository : IDisposable
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
        AttendanceManagementContext context = null;
        public RACoffAvailingApplicationRepository()
        {
            context = new AttendanceManagementContext();
        }
        string Message = string.Empty;
        StringBuilder builder = new StringBuilder();
        public void SaveRequestApplication(ClassesToSave DataToSave, bool isFinalLevelApproval)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            CommonRepository CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    // save to request application table.
                    context.RequestApplication.Add(DataToSave.RA);
                    // save to application application table.
                    context.ApplicationApproval.Add(DataToSave.AA);
                    if (isFinalLevelApproval == true)
                    {
                        if (DataToSave.ELA != null)
                        {
                            //Insert into employee leave account.
                            context.EmployeeLeaveAccount.Add(DataToSave.ELA);
                        }
                        if (DataToSave.RA.StartDate != null && DataToSave.RA.EndDate != null)
                        {
                            //Insert into attendance control table
                            fromDate = Convert.ToDateTime(DataToSave.RA.StartDate);
                            toDate = Convert.ToDateTime(DataToSave.RA.EndDate);
                            if (fromDate.Date < currentDate.Date)
                            {
                                if (toDate.Date >= currentDate.Date)
                                {
                                    toDate = DateTime.Now.AddDays(-1);
                                }
                                CR.LogIntoIntoAttendanceControlTable(DataToSave.RA.StaffId, fromDate.Date, toDate.Date, DataToSave.RA.RequestApplicationType, DataToSave.AA.Id);
                            }
                        }
                    }
                    context.SaveChanges();
                    Trans.Commit();
                    if (DataToSave.ESL != null)
                    {
                        foreach (var l in DataToSave.ESL)
                            if (l.To != "-" && string.IsNullOrEmpty(l.To).Equals(false))
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException err)
                {
                    Trans.Rollback();
                    throw err.InnerException;
                }
            }
        }
        public string GetCoffReqPeriodRepository()
        {
            Message = context.Settings.Where(condition => condition.Parameter == "COffReqPeriod").Select(select => select.Value).FirstOrDefault();
            return Message;
        }
        public string ValidateCoffAvailing(string StaffId, string COffFromDate, string COffToDate, decimal TotalDays, string WorkedDate)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@StaffId", StaffId);
                param[1] = new SqlParameter("@COffFromDate", COffFromDate);
                param[2] = new SqlParameter("@COffToDate", COffToDate);
                param[3] = new SqlParameter("@TotalDays", TotalDays);
                param[4] = new SqlParameter("@COffReqDate", WorkedDate);
                builder = new StringBuilder();
                builder.Append("select [dbo].[fnValidateCOFFApplication] (@StaffId,@COffFromDate,@COffToDate,@TotalDays,@COffReqDate)");

//                Select[dbo].[fnValidateCOFFApplication]('30124', '09-Aug-2019', '09-Aug-2019',
//0.5, '09-Jun-2019')

                Message = context.Database.SqlQuery<string>(builder.ToString(), param).FirstOrDefault();
                if (!Message.Equals("OK."))
                {
                    throw new Exception(Message);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Message;
        }
        public string GetUniqueId()
        {
            return context.Database.SqlQuery<string>("select convert(varchar,getdate(),112) + replace(convert(varchar,getdate(),114),':','')").First();
        }
        //Coff Avaling SelfApplication List
        public List<RACoffAvailingRequestApplication> RenderAppliedCompAvailingList(string StaffId, string AppliedBy, string userRole)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Append("select top 30 COffId as Id,StaffId,Name as StaffName, Workeddate,FromDate as StartDate,ToDate as EndDate," +
                    "convert(varchar(10), TotalDays) as TotalDays, COffReason as Remarks, Approval1StatusName as Status1, Approval2statusName as " +
                    "Status2, IsCancelled as IsCancelled from View_COffAvailing_ApplicationHistory where staffid = @StaffId Order by " +
                    "CONVERT(DateTime, FromDate, 101) Desc "
);
                var Obj = context.Database.SqlQuery<RACoffAvailingRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).ToList();
                return Obj;
            }
            catch 
            {
                return new List<RACoffAvailingRequestApplication>();
            }
        }

        //Coff Avaling TeamApplication List
        public List<RACoffAvailingRequestApplication> RenderAppliedCompAvailingListMyteam(string StaffId, string AppliedBy, string userRole)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                if (userRole == "3" || userRole == "5")
                {
                    Str.Append(" select Top 30 COffId as Id,StaffId,Name as StaffName,COffReason as Remarks,FromDate as StartDate, " +
                        "ToDate as EndDate, convert(varchar(10), TotalDays) as TotalDays, AppliedBy, Approval1StatusName as " +
                        "Status1, Approval2statusName as Status2, IsCancelled as IsCancelled,ApprovalOwner, Approval2Owner, " +
                        "convert(varchar, Workeddate, 110) as Workeddate from View_COffAvailing_ApplicationHistory " +
                        "where staffid = @StaffId Order by CONVERT(DateTime, FromDate, 101) Desc ");
                }
                else
                {
                    Str.Append("select top 30 COffId as Id,StaffId,Name as StaffName,COffReason as Remarks,FromDate as StartDate, " +
                        "ToDate as EndDate, convert(varchar(10), TotalDays) as TotalDays, AppliedBy, Approval1StatusName as " +
                        "Status1, Approval2statusName as Status2, IsCancelled as IsCancelled,ApprovalOwner, Approval2Owner, " +
                        "convert(varchar, Workeddate, 110) as Workeddate from View_COffAvailing_ApplicationHistory " +
                        "where staffid = @StaffId and(AppliedBy = @AppliedBy or ApprovalOwner = @AppliedBy " +
                        "or Approval2Owner = @AppliedBy) Order by CONVERT(DateTime, FromDate, 101) Desc ");
                }
                //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
                var Obj = context.Database.SqlQuery<RACoffAvailingRequestApplication>(Str.ToString(),
                    new SqlParameter("@StaffId", StaffId), new SqlParameter("@AppliedBy", AppliedBy)).ToList();
                return Obj;
            }
            catch 
            {
                return new List<RACoffAvailingRequestApplication>();
            }
        }


        //   To get the C-Off availing request mapped under me
        public List<RACoffAvailingRequestApplication> GetCoffAvailRequestMappedUnderMe(string loggedInUser)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("select top 30 COffId as Id,StaffId,Name as StaffName,COffReason as Remarks,FromDate as StartDate, " +
                    "ToDate as EndDate, convert(varchar(10), TotalDays) as TotalDays, AppliedBy, Approval1StatusName as Status1, Approval2statusName as Status2, IsCancelled as IsCancelled,ApprovalOwner, Approval2Owner, convert(varchar, Workeddate, 110) as Workeddate from View_COffAvailing_ApplicationHistory where ApprovalOwner = @LoggedInUser " +
                    " or Approval2Owner = @LoggedInUser) Order by CONVERT(DateTime, FromDate, 101) Desc ");
                var Obj = context.Database.SqlQuery<RACoffAvailingRequestApplication>(stringBuilder.ToString(),
                    new SqlParameter("@LoggedInUser", loggedInUser)).ToList();
                return Obj;
            }
            catch 
            {
                return new List<RACoffAvailingRequestApplication>();
            }
        }

        public RequestApplication GetRequestApplicationDetails(string Id)
        {
            return context.RequestApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }
        public void CancelApplication(ClassesToSave CTS, string user)
        {
            var CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    context.Entry(CTS.RA).Property("IsCancelled").IsModified = true;
                    context.Entry(CTS.RA).Property("CancelledDate").IsModified = true;
                    context.Entry(CTS.RA).Property("CancelledBy").IsModified = true;
                    if (CTS.ELA != null)
                    {
                        //Insert into employee leave account.
                        context.EmployeeLeaveAccount.Add(CTS.ELA);
                    }
                    if (CTS.AA.Approval2statusId == 2)
                    {
                        if (CTS.RA.StaffId != null && CTS.RA.StartDate != null && CTS.RA.EndDate != null)
                        {
                            DateTime fromDate = DateTime.Now;
                            DateTime toDate = DateTime.Now;
                            DateTime currentDate = DateTime.Now;
                            fromDate = Convert.ToDateTime(CTS.RA.StartDate);
                            toDate = Convert.ToDateTime(CTS.RA.EndDate);
                            if (fromDate.Date < currentDate.Date)
                            {
                                if (toDate.Date >= currentDate.Date)
                                {
                                    toDate = DateTime.Now.AddDays(-1);
                                }
                                CR.LogIntoIntoAttendanceControlTable(CTS.RA.StaffId, fromDate.Date, toDate.Date, CTS.RA.RequestApplicationType, CTS.RA.Id);
                            }
                        }
                    }
                    context.SaveChanges();
                    Trans.Commit();
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                            if (l.To != "-" && l.To != "")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                }
                catch (Exception err)
                {
                    Trans.Rollback();
                    throw err;
                }
            }
        }
        public void ApproveApplication(ClassesToSave CTS, string loggedInUser, bool isFinalLevelApproval)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            CommonRepository CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    context.Entry(CTS.RA).State = System.Data.Entity.EntityState.Modified;
                    context.Entry(CTS.RA).Property("IsApproved").IsModified = true;
                    //Update the application approval table.
                    context.Entry(CTS.AA).State = System.Data.Entity.EntityState.Modified;
                    context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2statusId").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2By").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2On").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;
                    if (isFinalLevelApproval == true)
                    {
                        if (CTS.ELA != null)
                        {
                            //Insert into employee leave account.
                            context.EmployeeLeaveAccount.Add(CTS.ELA);
                        }
                        if (CTS.RA.StartDate != null && CTS.RA.EndDate != null)
                        {
                            //Insert into attendance control table
                            fromDate = Convert.ToDateTime(CTS.RA.StartDate);
                            toDate = Convert.ToDateTime(CTS.RA.EndDate);
                            if (fromDate.Date < currentDate.Date)
                            {
                                if (toDate.Date >= currentDate.Date)
                                {
                                    toDate = DateTime.Now.AddDays(-1);
                                }
                                CR.LogIntoIntoAttendanceControlTable(CTS.RA.StaffId, fromDate.Date, toDate.Date, CTS.RA.RequestApplicationType, CTS.AA.Id);
                            }
                        }
                    }
                    context.SaveChanges();
                    Trans.Commit();
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                            if (l.To != "-" && l.To != "")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                }
                catch (Exception e)
                {
                    Trans.Rollback();
                    throw e;
                }
            }
        }
        public void RejectApplication(ClassesToSave CTS)
        {
            var CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    context.Entry(CTS.RA).Property("IsRejected").IsModified = true;

                    //Update the application approval table.
                    context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2statusId").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2By").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2On").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;
                    context.SaveChanges();
                    Trans.Commit();
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                            if (l.To != "-" && l.To != "")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                }
                catch
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }
        public ApplicationApproval GetApplicationApprovalForCoffAvailing(string ParentId)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(ParentId) && d.ParentType.Equals("CO")).FirstOrDefault();
        }
        public string ValidateApplicationOverlaping(string StaffId, string CoffStartDate, string FromDurationId, string CoffEndDate, string ToDurationId)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Exec  [dbo].[IsApplicationOverLapping] @StaffId,@FromDurationId,'" + Convert.ToDateTime(CoffStartDate).ToString("yyyy-MM-dd HH:mm:ss") + "','" + Convert.ToDateTime(CoffEndDate).ToString("yyyy-MM-dd HH:mm:ss") + "',@ToDurationId");
            try
            {
                var str = (context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@StaffId", StaffId)
                    , new SqlParameter("@FromDurationId", FromDurationId), new SqlParameter("@ToDurationiD", ToDurationId)).FirstOrDefault()).ToString();
                return str;
            }
            catch (Exception err)
            {
                return "ERROR!" + err.Message;
            }
        }

    }
}
