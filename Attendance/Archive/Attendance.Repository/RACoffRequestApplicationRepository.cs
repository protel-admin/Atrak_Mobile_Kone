using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;
using System.Data.SqlClient;
using System.Data.Entity.Migrations;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class RACoffRequestApplicationRepository : IDisposable
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
        public RACoffRequestApplicationRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<RACoffRequestApplication> GetAppliedCoffRequest(string StaffId)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Append("SELECT ");
                Str.Append("Id, ");
                Str.Append("StaffId, ");
                Str.Append("Remarks, ");
                Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS StartDate, ");
                Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,EndDate,106),' ','-')) AS EndDate, ");
                Str.Append("TotalDays, ");
                Str.Append("CASE  ");
                Str.Append("WHEN IsCancelled = 0	and IsApproved = 0	and IsRejected = 0 THEN 'PENDING' ");
                Str.Append("WHEN IsCancelled = 0	and IsApproved = 1	and IsRejected = 0 THEN 'APPROVED' ");
                Str.Append("WHEN IsCancelled = 0	and IsApproved = 0	and IsRejected = 1 THEN 'REJECTED' ");
                Str.Append("WHEN IsCancelled = 1	and IsApproved = 0	and IsRejected = 0 THEN 'CANCELLED' ");
                Str.Append("WHEN IsCancelled = 1	and IsApproved = 1	and IsRejected = 0 THEN 'APPROVED BUT CANCELLED' ");
                Str.Append("WHEN IsCancelled = 1	and IsApproved = 0	and IsRejected = 1 THEN 'REJECTED BUT CANCELLED' ");
                Str.Append("END as [Status] ");
                Str.Append("FROM RequestApplication A WHERE RequestApplicationType = 'CO' AND StaffId = @StaffId order by ApplicationDate desc ");

                //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
                var Obj = context.Database.SqlQuery<RACoffRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).Select(d => new RACoffRequestApplication
                {
                    Id = d.Id,
                    StaffId = d.StaffId,
                    Remarks = d.Remarks,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    TotalDays = d.TotalDays,
                    Status = d.Status
                }).ToList();
                return Obj;
            }
            catch (Exception e)
            {
                return new List<RACoffRequestApplication>();
            }
        }

        public LeaveTypeAndBalance GetLeaveTypeAndBalance(string StaffId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId ?? "");

            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select leavetypeid,CAST(LEAVEBALANCE as varchar) as LeaveBalance, LeaveName from leavebalance where StaffId=@StaffId and IsActive='1' and LeavetypeId='LV0005'");
            try
            {
                var data = context.Database.SqlQuery<LeaveTypeAndBalance>(QryStr.ToString(), sqlParameter).Select(d => new LeaveTypeAndBalance()
                {
                    LeaveBalance = d.LeaveBalance,
                    LeaveTypeId = d.LeaveTypeId,
                    LeaveName = d.LeaveName
                }).FirstOrDefault();

                if (data == null)
                {
                    return new LeaveTypeAndBalance();
                }
                else
                {
                    //if (string.IsNullOrEmpty(data.) == true)
                    //{
                    //    throw new Exception("Employee does not exists.");
                    //}
                    return data;
                }
            }
            catch (Exception e)
            {
                throw e;
                //return new IndividualLeaveCreditDebit_EmpDetails();
            }
        }

        public List<PermissionType> GetPermissionTypes()
        {
            var Obj = context.PermissionType.Where(d => d.IsActive.Equals(true)).ToList();
            return Obj;
        }

        public string GetUniqueId()
        {
            return context.Database.SqlQuery<string>("select convert(varchar,getdate(),112) + replace(convert(varchar,getdate(),114),':','')").First();
        }

        public void SaveRequestApplication(ClassesToSave DataToSave, bool IsFinalLevelApproval)
        {
            //CommonRepository commonRepository = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    // save to request application table.
                    context.RequestApplication.Add(DataToSave.RA);
                    // save to application approval table.
                    context.ApplicationApproval.Add(DataToSave.AA);
                    // save to email send log table.

                    if (IsFinalLevelApproval == true)
                    {
                        context.EmployeeLeaveAccount.Add(DataToSave.ELA);
                        if (DataToSave.RA.StartDate.Date < DateTime.Now.Date)
                        {
                           DateTime fromDate = Convert.ToDateTime(DataToSave.RA.StartDate);
                            DateTime toDate = Convert.ToDateTime(DataToSave.RA.EndDate);
                            if (fromDate.Date < DateTime.Now.Date)
                            {
                                if (toDate.Date >= DateTime.Now.Date)
                                {
                                    toDate = DateTime.Now.AddDays(-1);
                                }
                                using (CommonRepository commonRepository = new CommonRepository())
                                {
                                    commonRepository.LogIntoIntoAttendanceControlTable(DataToSave.RA.StaffId, fromDate, toDate,
                                        DataToSave.RA.RequestApplicationType, DataToSave.RA.Id);
                                }
                            }
                        }
                    }
                    context.SaveChanges();
                    Trans.Commit();
                    if (DataToSave.ESL != null)
                    {
                        foreach (var l in DataToSave.ESL)
                        {
                            if (l.To != "-")
                                using (CommonRepository commonRepository = new CommonRepository())
                                {
                                    commonRepository.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                                }
                        }
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException err)
                {
                    Trans.Rollback();
                    throw err;
                }
            }
        }


        public void RejectApplication(ClassesToSave CTS)
        {
            
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    //context.Entry(CTS.RA).Property("IsRejected").IsModified = true;

                    context.RequestApplication.AddOrUpdate(CTS.RA);

                    //Update the application approval table.
                    //context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    //context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    //context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    //context.Entry(CTS.AA).Property("Comment").IsModified = true;


                    context.ApplicationApproval.AddOrUpdate(CTS.AA);

                    context.SaveChanges();
                    Trans.Commit();

                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                        {
                            if (l.To != "-")

                                using (CommonRepository commonRepository = new CommonRepository())
                                {
                                    commonRepository.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                                }
                        }
                    }
                }
                catch
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }

        public void ApproveApplication(ClassesToSave CTS, bool isFinalLevelApproval)
        {
            
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    //context.Entry(CTS.RA).Property("IsApproved").IsModified = true;
                    context.RequestApplication.AddOrUpdate(CTS.RA);

                    //Update the application approval table.
                    //context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    //context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    //context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    //context.Entry(CTS.AA).Property("Comment").IsModified = true;
                    context.ApplicationApproval.AddOrUpdate(CTS.AA);

                    if (isFinalLevelApproval == true)
                    {
                        if (CTS.ELA != null)
                        {
                            context.EmployeeLeaveAccount.Add(CTS.ELA);
                        }
                        if (CTS.RA.StartDate.Date < DateTime.Now.Date)
                        {
                            DateTime fromDate = Convert.ToDateTime(CTS.RA.StartDate);
                            DateTime toDate = Convert.ToDateTime(CTS.RA.EndDate);
                            if (fromDate.Date < DateTime.Now.Date)
                            {
                                if (toDate.Date >= DateTime.Now.Date)
                                {
                                    toDate = DateTime.Now.AddDays(-1);
                                }
                                using (CommonRepository commonRepository = new CommonRepository())
                                {
                                    commonRepository.LogIntoIntoAttendanceControlTable(CTS.RA.StaffId, fromDate, toDate,
                                        CTS.RA.RequestApplicationType, CTS.RA.Id);
                                }
                            }
                        }
                    }
                    context.SaveChanges();
                    Trans.Commit();
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                        {
                            if (l.To != "-")
                                using (CommonRepository commonRepository = new CommonRepository())
                                {
                                    commonRepository.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                                }
                        }
                    }
                }
                catch (Exception e)
                {
                    Trans.Rollback();
                    throw e;
                }
            }
        }

        public void CancelApplication(ClassesToSave CTS)
        {
            
                using (var Trans = context.Database.BeginTransaction())
                {
                    try
                    {
                    //context.Entry(CTS.RA).Property("IsCancelled").IsModified = true;
                    //context.Entry(CTS.RA).Property("CancelledDate").IsModified = true;
                    //context.Entry(CTS.RA).Property("CancelledBy").IsModified = true;

                    context.RequestApplication.AddOrUpdate(CTS.RA);

                    if (CTS.AA.ReviewerstatusId == 2)
                    {
                        if (CTS.ELA != null)
            {
                            context.EmployeeLeaveAccount.Add(CTS.ELA);
            }
                        if (CTS.RA.StartDate.Date < DateTime.Now.Date)
                        {
                            DateTime fromDate = Convert.ToDateTime(CTS.RA.StartDate);
                            DateTime toDate = Convert.ToDateTime(CTS.RA.EndDate);
                            if (fromDate.Date < DateTime.Now.Date)
            {
                                if (toDate.Date >= DateTime.Now.Date)
                {
                                    toDate = DateTime.Now.AddDays(-1);
                                }
                                using (CommonRepository commonRepository = new CommonRepository())
                    {

                                    commonRepository.LogIntoIntoAttendanceControlTable(CTS.RA.StaffId, fromDate, toDate,
                                        CTS.RA.RequestApplicationType, CTS.RA.Id);
                                }
                            }
                        }
                    }
                        //save the changes.
                        context.SaveChanges();
                        Trans.Commit();
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                        {
                            if (l.To != "-")
                                using (CommonRepository commonRepository = new CommonRepository())
                                {
                                    commonRepository.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                        }
                    }
                    }
                catch(Exception e)
                    {
                        Trans.Rollback();
                    throw e;
                }
            }
        }

        public RequestApplication GetRequestApplicationDetails(string Id)
        {
            return context.RequestApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }

        public ApplicationApproval GetApplicationApproval(string ParentId)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(ParentId) && d.ParentType.Equals("CO")).FirstOrDefault();
        }

        public string ValidateApplicationOverlaping(string StaffId, string CoffStartDate, string FromDurationId, string CoffEndDate, string ToDurationId)
        {

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@StaffId", StaffId ?? "");
            param[1] = new SqlParameter("@CoffStartDate", Convert.ToDateTime(CoffStartDate).ToString("yyyy-MM-dd HH:mm:ss") ?? "");
            param[2] = new SqlParameter("@CoffEndDate", Convert.ToDateTime(CoffEndDate).ToString("yyyy-MM-dd HH:mm:ss") ?? "");
            param[3] = new SqlParameter("@FromDurationId", FromDurationId ?? "");
            param[4] = new SqlParameter("@ToDurationId", ToDurationId ?? "");


            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Exec  [dbo].[IsApplicationOverLapping]  @StaffId,@FromDurationId,@CoffStartDate,@CoffEndDate,@ToDurationId");
            try
            {
                var str = (context.Database.SqlQuery<string>(qryStr.ToString(),param).FirstOrDefault()).ToString();
                return str;
            }
            catch (Exception err)
            {
                return "ERROR!" + err.Message;
            }
        }
        public string ValidateCoffAvailing(string StaffId, string COffFromDate, string COffToDate, decimal TotalDays, string WorkedDate, string LeaveType)
        {
            string Message = string.Empty;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@StaffId", StaffId ?? "");
                param[1] = new SqlParameter("@COffFromDate", COffFromDate ?? "");
                param[2] = new SqlParameter("@COffToDate", COffToDate ?? "");
                param[3] = new SqlParameter("@TotalDays", TotalDays);
                param[4] = new SqlParameter("@COffReqDate", WorkedDate ?? "");
                StringBuilder builder = new StringBuilder();
                builder.Append("select [dbo].[fnValidateCOFFApplication] (@StaffId,@COffFromDate,@COffToDate,@TotalDays,@COffReqDate)");
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


        #region Coff Req Availing
        public List<LeaveDuration> GetDurationListRepository()
        {
            var qryStr = new StringBuilder();
            qryStr.Append("select Id , Name , isactive from leaveduration where isactive = 1");
            try
            {
                var lst = context.Database.SqlQuery<LeaveDuration>(qryStr.ToString()).Select(d => new LeaveDuration()
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<LeaveDuration>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LeaveDuration>();
            }
        }
        public List<RACoffAvailingRequestApplication> RenderAppliedCompAvailingList(string StaffId, string AppliedBy, string userRole)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Append(" select COffId as Id,StaffId,FirstName as StaffName,COffReason as Remarks,COffReqDate as StartDate,COffAvailDate as EndDate,convert(varchar(10),TotalDays) as TotalDays,AppliedBy, Approval1StatusName as Status1, Approval2statusName as Status2, IsCancelled as IsCancelled, Approval1Owner, Approval2Owner,format(Workeddate,'dd-MMM-yyyy') as Workeddate,ExpiryDate from vwCOffAvailingApproval where staffid = @StaffId  ");
                var Obj = context.Database.SqlQuery<RACoffAvailingRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).ToList();
                return Obj;
            }
            catch (Exception e)
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
                    Str.Append(" select COffId as Id,StaffId,FirstName as StaffName,COffReason as Remarks,COffReqDate as StartDate,COffAvailDate as EndDate,convert(varchar(10),TotalDays) as TotalDays,AppliedBy, Approval1StatusName as Status1, Approval2statusName as Status2, IsCancelled as IsCancelled, Approval1Owner, Approval2Owner,convert(varchar,Workeddate,110) as Workeddate from vwCOffAvailApproval where staffid = @StaffId  ");
                }
                else
                {
                    Str.Append(" select COffId as Id,StaffId,FirstName as StaffName,COffReason as Remarks,COffReqDate as StartDate,COffAvailDate as EndDate,convert(varchar(10),TotalDays) as TotalDays,AppliedBy, Approval1StatusName as Status1, Approval2statusName as Status2, IsCancelled as IsCancelled, Approval1Owner, Approval2Owner,convert(varchar,Workeddate,110) as Workeddate from vwCOffAvailApproval where staffid = @StaffId and(AppliedBy = @AppliedBy or Approval1Owner = @AppliedBy or Approval2Owner = @AppliedBy) ");
                }
                //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
                var Obj = context.Database.SqlQuery<RACoffAvailingRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@AppliedId", AppliedBy)).ToList();
                return Obj;
            }
            catch (Exception e)
            {
                return new List<RACoffAvailingRequestApplication>();
            }
        }
        #endregion

    }
}
