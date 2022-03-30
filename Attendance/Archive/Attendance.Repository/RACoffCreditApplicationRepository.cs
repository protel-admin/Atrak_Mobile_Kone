using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;
using System.Data.SqlClient;
using System.Data.Entity.Migrations;
using AttendanceManagement;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class RACoffCreditApplicationRepository : IDisposable
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
        public RACoffCreditApplicationRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<RACoffCreditRequestApplication> GetAppliedCoffCreditRequest(string StaffId)
        {
          
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Append("SELECT ");
                Str.Append("a.Id, ");
                Str.Append("a.StaffId, ");
                Str.Append("A.IsCancelled,");
                Str.Append("A.IsReviewerCancelled,");
                Str.Append("A.IsApproverCancelled,");
                Str.Append("(c.FirstName+' '+c.LastName) as StaffName,");
                Str.Append("Remarks, ");
                Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS StartDate, ");
                Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,EndDate,106),' ','-')) AS EndDate, ");
                Str.Append("TotalDays,AppliedBy,B.ApprovalOwner, B.ReviewerOwner,  ");

                Str.Append(" CASE ");
                Str.Append(" WHEN IsapproverCancelled = 0 AND ApprovalStatusId = 1 THEN 'PENDING' ");
                Str.Append(" WHEN IsapproverCancelled = 1 AND ApprovalStatusId = 1 THEN 'CANCELLED' ");
                Str.Append(" WHEN IsapproverCancelled = 0 and ApprovalStatusId = 2 THEN 'APPROVED'  ");
                Str.Append(" WHEN IsapproverCancelled = 0 and ApprovalStatusId = 3  THEN 'REJECTED' ");
                Str.Append(" WHEN IsapproverCancelled = 1 and ApprovalStatusId = 1 and ReviewerstatusId = 2  THEN 'CANCELLED'  ");
                Str.Append(" WHEN IsapproverCancelled = 1 and ApprovalStatusId = 2 THEN 'APPROVED BUT CANCELLED' ");
                Str.Append(" WHEN IsapproverCancelled = 1 and ApprovalStatusId = 3 THEN 'REJECTED BUT CANCELLED' ");
                Str.Append(" END as [ApproverStatus], ");


                Str.Append(" CASE ");
                Str.Append(" WHEN IsReviewerCancelled = 0 and B.ReviewerstatusId =1	 THEN 'PENDING' ");
                Str.Append(" WHEN IsReviewerCancelled = 0 and B.ReviewerstatusId =2	THEN 'REVIEWED'   ");
                Str.Append(" WHEN IsReviewerCancelled = 0 and B.ReviewerstatusId =3  THEN 'REJECTED' ");
                Str.Append(" WHEN IsReviewerCancelled = 1 and  B.ReviewerstatusId =1	 THEN 'CANCELLED' ");
                Str.Append(" WHEN IsReviewerCancelled = 1 and B.ReviewerstatusId =2	and ApprovalStatusId=1 THEN 'REVIEWED BUT CANCELLED' ");
                Str.Append(" WHEN IsReviewerCancelled = 1 and  B.ReviewerstatusId =3 and ApprovalStatusId=1 THEN 'REJECTED BUT CANCELLED' ");
                Str.Append(" END as [ReviewerStatus]");

                Str.Append("FROM RequestApplication A ");
                Str.Append("join STAFFVIEW as c on a.Staffid = c.Staffid ");
                Str.Append("inner join ApplicationApproval B on A.id=B.ParentId ");
                Str.Append("WHERE a.RequestApplicationType = 'CR' AND a.StaffId = @StaffId order by A.ApplicationDate desc ");

                var Obj = context.Database.SqlQuery<RACoffCreditRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).Select(d => new RACoffCreditRequestApplication
                {
                    Id = d.Id,
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    AppliedBy = d.AppliedBy,
                    Remarks = d.Remarks,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    TotalDays = d.TotalDays,
                    ApproverStatus = d.ApproverStatus,
                    ReviewerStatus = d.ReviewerStatus,
                    ApprovalOwner = d.ApprovalOwner,
                    ReviewerOwner = d.ReviewerOwner,
                    IsCancelled = d.IsCancelled,
                    IsReviewerCancelled = d.IsReviewerCancelled,
                    IsApproverCancelled = d.IsApproverCancelled
                }).ToList();
                return Obj;
            }
            catch (Exception e)
            {
                return new List<RACoffCreditRequestApplication>();
            }
        }

        public List<RACoffCreditRequestApplication> GetAppliedCoffCreditRequestForMyTeam(string StaffId, string AppliedBy, string Role)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                if (Role == "3" || Role == "5" || Role == "6")
                {
                    Str.Append("SELECT ");
                    Str.Append("a.Id, ");
                    Str.Append("a.StaffId, ");
                    Str.Append("A.IsCancelled,");
                    Str.Append("A.IsReviewerCancelled,");
                    Str.Append("A.IsApproverCancelled,");
                    Str.Append("(c.FirstName+' '+c.LastName) as StaffName,");
                    Str.Append("Remarks, ");
                    Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS StartDate, ");
                    Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,EndDate,106),' ','-')) AS EndDate, ");
                    Str.Append("TotalDays,AppliedBy,B.ApprovalOwner, B.ReviewerOwner, ");

                    Str.Append(" CASE ");
                    Str.Append(" WHEN IsapproverCancelled = 0 AND ApprovalStatusId = 1 THEN 'PENDING' ");
                    Str.Append(" WHEN IsapproverCancelled = 1 AND ApprovalStatusId = 1 THEN 'CANCELLED' ");
                    Str.Append(" WHEN IsapproverCancelled = 0 and ApprovalStatusId = 2 THEN 'APPROVED'  ");
                    Str.Append(" WHEN IsapproverCancelled = 0 and ApprovalStatusId = 3  THEN 'REJECTED' ");
                    Str.Append(" WHEN IsapproverCancelled = 1 and ApprovalStatusId = 1 and ReviewerstatusId = 2  THEN 'CANCELLED'  ");
                    Str.Append(" WHEN IsapproverCancelled = 1 and ApprovalStatusId = 2 THEN 'APPROVED BUT CANCELLED' ");
                    Str.Append(" WHEN IsapproverCancelled = 1 and ApprovalStatusId = 3 THEN 'REJECTED BUT CANCELLED' ");
                    Str.Append(" END as [ApproverStatus], ");

                    Str.Append(" CASE ");
                    Str.Append(" WHEN IsReviewerCancelled = 0 and B.ReviewerstatusId =1	 THEN 'PENDING' ");
                    Str.Append(" WHEN IsReviewerCancelled = 0 and B.ReviewerstatusId =2	THEN 'REVIEWED'   ");
                    Str.Append(" WHEN IsReviewerCancelled = 0 and B.ReviewerstatusId =3  THEN 'REJECTED' ");
                    Str.Append(" WHEN IsReviewerCancelled = 1 and  B.ReviewerstatusId =1	 THEN 'CANCELLED' ");
                    Str.Append(" WHEN IsReviewerCancelled = 1 and B.ReviewerstatusId =2	and ApprovalStatusId=1 THEN 'REVIEWED BUT CANCELLED' ");
                    Str.Append(" WHEN IsReviewerCancelled = 1 and  B.ReviewerstatusId =3 and ApprovalStatusId=1 THEN 'REJECTED BUT CANCELLED' ");
                    Str.Append(" END as [ReviewerStatus]");

                    Str.Append(" FROM RequestApplication A ");
                    Str.Append(" join ApplicationApproval as B on a.id= b.parentid ");
                    Str.Append(" join STAFFVIEW as c on a.Staffid = c.Staffid ");
                    Str.Append(" WHERE a.RequestApplicationType = 'CR' AND a.StaffId=@StaffId order by a.ApplicationDate desc ");
                }
                else
                {
                    Str.Append("SELECT ");
                    Str.Append("a.Id, ");
                    Str.Append("a.StaffId, ");
                    Str.Append("A.IsCancelled,");
                    Str.Append("A.IsReviewerCancelled,");
                    Str.Append("A.IsApproverCancelled,");
                    Str.Append("(c.FirstName+' '+c.LastName) as StaffName,");
                    Str.Append("Remarks, ");
                    Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS StartDate, ");
                    Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,EndDate,106),' ','-')) AS EndDate, ");
                    Str.Append("TotalDays,AppliedBy,B.ApprovalOwner, B.ReviewerOwner, ");

                    Str.Append(" CASE ");
                    Str.Append(" WHEN IsapproverCancelled = 0 AND ApprovalStatusId = 1 THEN 'PENDING' ");
                    Str.Append(" WHEN IsapproverCancelled = 1 AND ApprovalStatusId = 1 THEN 'CANCELLED' ");
                    Str.Append(" WHEN IsapproverCancelled = 0 and ApprovalStatusId = 2 THEN 'APPROVED'  ");
                    Str.Append(" WHEN IsapproverCancelled = 0 and ApprovalStatusId = 3  THEN 'REJECTED' ");
                    Str.Append(" WHEN IsapproverCancelled = 1 and ApprovalStatusId = 1 and ReviewerstatusId = 2  THEN 'CANCELLED'  ");
                    Str.Append(" WHEN IsapproverCancelled = 1 and ApprovalStatusId = 2 THEN 'APPROVED BUT CANCELLED' ");
                    Str.Append(" WHEN IsapproverCancelled = 1 and ApprovalStatusId = 3 THEN 'REJECTED BUT CANCELLED' ");
                    Str.Append(" END as [ApproverStatus], ");

                    Str.Append(" CASE ");
                    Str.Append(" WHEN IsReviewerCancelled = 0 and B.ReviewerstatusId =1	 THEN 'PENDING' ");
                    Str.Append(" WHEN IsReviewerCancelled = 0 and B.ReviewerstatusId =2	THEN 'REVIEWED'   ");
                    Str.Append(" WHEN IsReviewerCancelled = 0 and B.ReviewerstatusId =3  THEN 'REJECTED' ");
                    Str.Append(" WHEN IsReviewerCancelled = 1 and  B.ReviewerstatusId =1	 THEN 'CANCELLED' ");
                    Str.Append(" WHEN IsReviewerCancelled = 1 and B.ReviewerstatusId =2	and ApprovalStatusId=1 THEN 'REVIEWED BUT CANCELLED' ");
                    Str.Append(" WHEN IsReviewerCancelled = 1 and  B.ReviewerstatusId =3 and ApprovalStatusId=1 THEN 'REJECTED BUT CANCELLED' ");
                    Str.Append(" END as [ReviewerStatus]");

                    Str.Append(" FROM RequestApplication A ");
                    Str.Append(" join ApplicationApproval as B on a.id= b.parentid ");
                    Str.Append(" join STAFFVIEW as c on a.Staffid = c.Staffid ");
                    Str.Append(" WHERE a.RequestApplicationType = 'CR' AND a.StaffId=@StaffId AND (a.AppliedBy = @AppliedId or b.ApprovalOwner=@AppliedId or b.ReviewerOwner=@AppliedId) order by a.ApplicationDate desc ");
                }
                //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
                var Obj = context.Database.SqlQuery<RACoffCreditRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@AppliedId", AppliedBy)).Select(d => new RACoffCreditRequestApplication
                {
                    Id = d.Id,
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    AppliedBy = d.AppliedBy,
                    Remarks = d.Remarks,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    TotalDays = d.TotalDays,
                    IsCancelled = d.IsCancelled,
                    ReviewerStatus = d.ReviewerStatus,
                    ApproverStatus = d.ApproverStatus,
                    ApprovalOwner = d.ApprovalOwner,
                    ReviewerOwner = d.ReviewerOwner,
                    IsReviewerCancelled = d.IsReviewerCancelled,
                    IsApproverCancelled = d.IsApproverCancelled,
                }).ToList();
                return Obj;
            }
            catch (Exception e)
            {
                return new List<RACoffCreditRequestApplication>();
            }
        }

        public List<CoffReqDates> GetAllOTDates(string Staffid, string FromDate, string ToDate)
        {
            List<CoffReqDates> data = new List<CoffReqDates>();
            try
            {
                SqlParameter[] Param = new SqlParameter[3];
                Param[0] = new SqlParameter("@StaffId", Staffid ?? "");
                Param[1] = new SqlParameter("@FromDate", FromDate.Trim() ?? "");
                Param[2] = new SqlParameter("@ToDate", ToDate.Trim() ?? "");

                var qrystr = new StringBuilder();
                qrystr.Clear();
                qrystr.Append("EXEC [DBO].[GETCOFFDATES] @StaffId, @FromDate, @ToDate");
                data = context.Database.SqlQuery<CoffReqDates>(qrystr.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return data;
        }

        public string GetUniqueId()
        {
            return context.Database.SqlQuery<string>("select convert(varchar,getdate(),112) + replace(convert(varchar,getdate(),114),':','')").First();
        }


        public void SaveRequestApplication(ClassesToSave DataToSave, bool IsFinalLevelApproval)
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
                    // save to application approval table.
                    context.ApplicationApproval.Add(DataToSave.AA);
                  // Save the Employee Leave Account table
                    if (IsFinalLevelApproval == true)
                    {
                        context.EmployeeLeaveAccount.Add(DataToSave.ELA);
                    }
                    context.SaveChanges();
                    Trans.Commit();

                    // save to email send log table.
                    if (DataToSave.ESL != null)
                    {
                    foreach (var l in DataToSave.ESL)
                        if (l.To != "-")
                            CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException err)
                {
                    Trans.Rollback();
                    if (err.Message != null)
                    {
                        throw new Exception(err.Message);
                    }
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
                    //context.Entry(CTS.AA).Property("ReviewerstatusId").IsModified = true;
                    //context.Entry(CTS.AA).Property("ReviewedBy").IsModified = true;
                    //context.Entry(CTS.AA).Property("ReviewedOn").IsModified = true;
                    //context.Entry(CTS.AA).Property("Comment").IsModified = true;
                    context.ApplicationApproval.AddOrUpdate(CTS.AA);

                    if (CTS.ESL != null)
                    {
                    foreach (var l in CTS.ESL)
                        if (l.To != "-")
                                using (CommonRepository commonRepository = new CommonRepository())
                                {

                                    commonRepository.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                                }
                    }
                    context.SaveChanges();
                    Trans.Commit();
                }
                catch
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }

        public void ApproveApplication(ClassesToSave CTS, bool IsFinalLevelApproval)
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
                    //context.Entry(CTS.RA).State = System.Data.Entity.EntityState.Modified;
                    //context.Entry(CTS.RA).Property("IsApproved").IsModified = true;
                    //context.Entry(CTS.RA).Property("IsReviewed").IsModified = true;


                    context.RequestApplication.AddOrUpdate(CTS.RA);

                    //Update the application approval table for approver.
                    //context.Entry(CTS.AA).State = System.Data.Entity.EntityState.Modified;
                    //context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    //context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    //context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    //context.Entry(CTS.AA).Property("Comment").IsModified = true;

                    //Update the application approval table for Reviewer.
                    //context.Entry(CTS.AA).State = System.Data.Entity.EntityState.Modified;
                    //context.Entry(CTS.AA).Property("ReviewerstatusId").IsModified = true;
                    //context.Entry(CTS.AA).Property("ReviewedBy").IsModified = true;
                    //context.Entry(CTS.AA).Property("ReviewedOn").IsModified = true;
                    //context.Entry(CTS.AA).Property("Comment").IsModified = true;

                    context.ApplicationApproval.AddOrUpdate(CTS.AA);

                    //Insert into employee leave account.
                    if (CTS.ELA != null)
                    {
                        CTS.ELA.TransactionDate = CTS.RA.ApplicationDate;
                        CTS.ELA.Narration = "Approved the C-Off application.";
                        CTS.ELA.TransactionFlag = 1;
                        context.EmployeeLeaveAccount.AddOrUpdate(CTS.ELA);
                    }
                    context.SaveChanges();
                    Trans.Commit();
                    if (CTS.ESL != null)
                    {
                    foreach (var l in CTS.ESL)
                        if (l.To != "-")
                                using (CommonRepository commonRepository = new CommonRepository())
                    {

                                    commonRepository.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
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
            var CR = new CommonRepository();
                using (var Trans = context.Database.BeginTransaction())
                {
                    try
                    {
                    //context.Entry(CTS.RA).Property("IsCancelled").IsModified = true;
                    //context.Entry(CTS.RA).Property("CancelledDate").IsModified = true;
                    //context.Entry(CTS.RA).Property("CancelledBy").IsModified = true;
                    //context.Entry(CTS.RA).Property("IsReviewerCancelled").IsModified = true;
                    //context.Entry(CTS.RA).Property("ReviewerCancelledDate").IsModified = true;
                    //context.Entry(CTS.RA).Property("ReviewerCancelledBy").IsModified = true;

                    //context.Entry(CTS.RA).Property("IsApproverCancelled").IsModified = true;
                    //context.Entry(CTS.RA).Property("ApproverCancelledDate").IsModified = true;
                    //context.Entry(CTS.RA).Property("ApproverCancelledBy").IsModified = true;
                    context.RequestApplication.AddOrUpdate(CTS.RA);

                    if (CTS.ELA != null)
                    {
                        //set the Employee Leave Account class to add.
                        context.EmployeeLeaveAccount.Add(CTS.ELA);
                    }

                    context.SaveChanges();
                    Trans.Commit();
                    if (CTS.ESL != null)
                {
                        foreach (var l in CTS.ESL)
                            if (l.To != "-")
                                using (CommonRepository commonRepository = new CommonRepository())
                                {

                                    commonRepository.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
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

        public RequestApplication GetRequestApplicationDetails(string Id)
        {
            return context.RequestApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }

        public ApplicationApproval GetApplicationApproval(string ParentId)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(ParentId) && d.ParentType.Equals("CR")).FirstOrDefault();
        }

        #region Coff Req Availing
        public string ValidateApplicationOverlaping(string StaffId, string CoffStartDate, string FromDurationId, string CoffEndDate, string ToDurationId)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Exec  [dbo].[IsApplicationOverLapping]  '" + StaffId + "'," + FromDurationId + ",'" + Convert.ToDateTime(CoffStartDate).ToString("yyyy-MM-dd HH:mm:ss") + "','" + Convert.ToDateTime(CoffEndDate).ToString("yyyy-MM-dd HH:mm:ss") + "'," + ToDurationId + "");
            try
            {
                var str = (context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault()).ToString();
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
        public List<COffReqAvailModel> GetWorkedDatesForCompOffCreditRequest(string StaffId)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Append("Select StaffId , Replace(Convert(Varchar,StartDate,106),' ','-') as WorkedDate," +
                    " cast (TotalDays as decimal(10,2)) as Balance,Replace(Convert(Varchar,ExpiryDate,106),' ','-') as ExpiryDate " +
                    " From RequestApplication Where StaffId = @StaffId And IsCancelled = 0 and IsApproved = 1 and IsRejected = 0 and " +
                    " RequestApplicationType = 'CR' and Convert(Date,ExpiryDate) >= Convert(Date,GetDate()) and WorkedDate" +
                    " not in (Select WorkedDate From RequestApplication Where StaffId = @StaffId And RequestApplicationType = 'CO'" +
                    " and IsCancelled = 0 and IsRejected = 0) Order by StartDate Asc");

                var Obj = context.Database.SqlQuery<COffReqAvailModel>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).Select(d => new COffReqAvailModel
                {
                    StaffId = d.StaffId,
                    WorkedDate = d.WorkedDate,
                    Balance = d.Balance,
                    ExpiryDate = d.ExpiryDate,

                }).ToList();
                return Obj;
            }
            catch (Exception e)
            {
                return new List<COffReqAvailModel>();
            }
        }
        public void SaveRequestApplication(ClassesToSave DataToSave)
        {
            CommonRepository CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    // save to request application table.
                    context.RequestApplication.Add(DataToSave.RA);
                    // save to application application table.
                    context.ApplicationApproval.Add(DataToSave.AA);
                    if (DataToSave.AA.ReviewerOwner == DataToSave.RA.AppliedBy)
                    {
                        if (DataToSave.ELA != null)
                        {
                            //Insert into employee leave account.
                            context.EmployeeLeaveAccount.Add(DataToSave.ELA);
                        }
                        if (DataToSave.RA.StartDate != null && DataToSave.RA.EndDate != null)
                        {
                            //Insert into attendance control table
                            DateTime currentDate = DateTime.Now;
                            DateTime fromDate = Convert.ToDateTime(DataToSave.RA.StartDate);
                            DateTime toDate = Convert.ToDateTime(DataToSave.RA.EndDate);
                            if (fromDate < currentDate)
                            {
                                if (toDate >= currentDate)
                                {
                                    toDate = DateTime.Now.AddDays(-1);
                                }
                                CR.LogIntoIntoAttendanceControlTable(DataToSave.RA.StaffId, fromDate, toDate, DataToSave.RA.RequestApplicationType, DataToSave.AA.Id);
                            }
                        }
                    }
                    if (DataToSave.ESL != null)
                    {
                        foreach (var l in DataToSave.ESL)
                            if (l.To != "-" && l.To != "")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                    context.SaveChanges();
                    Trans.Commit();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException err)
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }
        public List<RACoffAvailingRequestApplication> RenderAppliedCompAvailingList(string StaffId, string AppliedBy, string userRole)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Append(" select COffId as Id,StaffId,FirstName as StaffName,COffReason as Remarks,COffReqDate as StartDate," +
                    "COffAvailDate as EndDate,convert(varchar(10),TotalDays) as TotalDays,AppliedBy, Approval1StatusName as Status1," +
                    " Approval2statusName as Status2, IsCancelled as IsCancelled, Approval1Owner, Approval2Owner," +
                    "format(Workeddate,'dd-MMM-yyyy') as Workeddate,ExpiryDate from vwCOffAvailingApproval where staffid = @StaffId  ");
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
                    Str.Append(" select COffId as Id,StaffId,FirstName as StaffName,COffReason as Remarks,COffReqDate as StartDate,COffAvailDate as EndDate,convert(varchar(10),TotalDays) as TotalDays,AppliedBy, Approval1StatusName as Status1, Approval2statusName as Status2, IsCancelled as IsCancelled, Approval1Owner, Approval2Owner,convert(varchar,Workeddate,110) as Workeddate from vwCOffAvailApproval where staffid = @StaffId and(AppliedBy = '" + AppliedBy + "' or Approval1Owner = '" + AppliedBy + "' or Approval2Owner = '" + AppliedBy + "') ");
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
        public ApplicationApproval GetApplicationApprovalForCoffAvailing(string ParentId)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(ParentId) && d.ParentType.Equals("CO")).FirstOrDefault();
        }
        public void ApproveApplicationCoffAvail(ClassesToSave CTS, string loggedInUser)
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
                    context.Entry(CTS.AA).Property("ReviewerstatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ReviewedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ReviewedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;
                    if (CTS.AA.ReviewerOwner == loggedInUser)
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
                            if (fromDate < currentDate)
                            {
                                if (toDate >= currentDate)
                                {
                                    toDate = DateTime.Now.AddDays(-1);
                                }
                                CR.LogIntoIntoAttendanceControlTable(CTS.RA.StaffId, fromDate, toDate, CTS.RA.RequestApplicationType, CTS.AA.Id);
                            }
                        }
                    }
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                            //context.EmailSendLog.Add(l);
                            if (l.To != "-" && l.To != "")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                    context.SaveChanges();
                    Trans.Commit();
                }
                catch (Exception e)
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }
        public void RejectApplicationCoffAvail(ClassesToSave CTS)
        {
            var CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    //context.Entry(CTS.RA).State = System.Data.Entity.EntityState.Modified;
                    context.Entry(CTS.RA).Property("IsRejected").IsModified = true;

                    //Update the application approval table.
                    //context.Entry(CTS.AA).State = System.Data.Entity.EntityState.Modified;
                    context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2statusId").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2By").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2On").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                            //context.EmailSendLog.Add(l);
                            if (l.To != "-" && l.To != "")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                    //
                    context.SaveChanges();
                    Trans.Commit();
                }
                catch
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }
        #endregion

        // Changes Made by aarthi on 28/2/2020 for CompOff Availing
        // Changes made by Karuppiah on 07-06-2020 to fine tune the query 
        public int GetCompOffLapsePeriod(string LocationId, string StaffId)
        {
            var qryStr = new StringBuilder();
            SqlParameter[] Params = new SqlParameter[2];
            Params[0] = new SqlParameter("@LocationId", LocationId);
            Params[1] = new SqlParameter("@StaffId", StaffId);
            qryStr.Append(" Select Top 1 Coalesce(A.Value,30) from [RuleGroupTxn] A Inner Join [Rule] B On A.ruleid = B.id Inner Join [StaffOfficial] C " +
                          "On A.rulegroupid = C.PolicyId and A.LocationID = C.LocationId Where B.name = 'LapsePeriodForCompOff' " +
                          "and  C.LocationId = @LocationId and C.staffId = @StaffId ");
            try
            {
                var lapsePeriod = context.Database.SqlQuery<int>(qryStr.ToString(), Params).FirstOrDefault();
                return lapsePeriod;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string ValidateCoffCreditApplication(string StaffId, DateTime FromDate, DateTime ToDate, string ApplicationType)
        {
            StringBuilder builder = new StringBuilder();
            string Message = string.Empty;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@StaffId", StaffId);
                param[1] = new SqlParameter("@COffFromDate", FromDate);
                param[2] = new SqlParameter("@COffToDate", ToDate);
                param[3] = new SqlParameter("@ApplicationType", ApplicationType);
                builder = new StringBuilder();
                builder.Append("Select [dbo].[ValidateCOFF_Credit_Application] (@StaffId,@COffFromDate,@COffToDate)");
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

        public bool CheckIsCompOffAvailed(string StaffId, DateTime StartDate)
        {
            bool isCompOffAvailed = false;
            string applicationId = string.Empty;
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Top 1 Id From RequestApplication Where StaffId = @StaffId and StartDate " +
                " = @StartDate and IsCancelled = 0 and IsRejected = 0 and RequestApplicationType = 'CO' ");
            applicationId = context.Database.SqlQuery<string>(queryString.ToString(), new SqlParameter("@StaffId", StaffId),
                new SqlParameter("@StartDate", StartDate)).FirstOrDefault();
            if (string.IsNullOrEmpty(applicationId).Equals(false))
            {
                isCompOffAvailed = true;
            }
            return isCompOffAvailed;
        }

    }
}
