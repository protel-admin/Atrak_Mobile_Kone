using Attendance.Model;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
namespace Attendance.Repository
{
    public class RALeaveApplicationRepository : IDisposable
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
        public RALeaveApplicationRepository()
        {
            context = new AttendanceManagementContext();
        }
        string Message = string.Empty;
        StringBuilder builder = new StringBuilder();
        public void ApproveApplication(ClassesToSave CTS)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;

            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.

                    context.RequestApplication.AddOrUpdate(CTS.RA);

                    context.ApplicationApproval.AddOrUpdate(CTS.AA);

                    if (CTS.APA != null)
                    {
                        context.AlternativePersonAssign.AddOrUpdate(CTS.APA);
                    }
                    //Insert into employee leave account.
                    if (CTS.ELA != null)
                    {
                        context.EmployeeLeaveAccount.Add(CTS.ELA);
                    }
                    context.SaveChanges();
                    //Insert into attendance control table
                    fromDate = Convert.ToDateTime(CTS.RA.StartDate);
                    toDate = Convert.ToDateTime(CTS.RA.EndDate);
                    if (fromDate < currentDate && CTS.RA.IsApproved == true)
                    {
                        if (toDate >= currentDate)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        using (CommonRepository commonRepository = new CommonRepository())
                        {
                            commonRepository.LogIntoIntoAttendanceControlTable(CTS.RA.StaffId, fromDate, toDate, CTS.RA.RequestApplicationType, CTS.AA.Id);
                        }
                    }
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

        public AlternativePersonAssign GetApplicationForWorkAllocation(string ParentId)
        {
            return context.AlternativePersonAssign.Where(d => d.ParentId.Equals(ParentId)).FirstOrDefault();
        }
        public List<LeaveReasonList> GetLeaveCDReasonList(string user)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Id as ResonId,Name from LeaveCreditDebitReason where IsActive = 1");
            try
            {
                var lst = context.Database.SqlQuery<LeaveReasonList>(qryStr.ToString()).Select(d => new LeaveReasonList()
                {
                    ResonId = d.ResonId,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<LeaveReasonList>();
                }
                else
                {
                    return lst;
                }
            }
            catch
            {
                return new List<LeaveReasonList>();
            }
        }
        public List<RALeaveApplication> GetAppliedLeaves(string StaffId, string AppliedId, string userRole)
        {
            try
            {
                StringBuilder Str = new StringBuilder();

                Str.Append(" select top 30 LeaveApplicationId as Id,StaffId,FirstName as StaffName, Remarks ,LeaveStartDate as StartDate ," +
                    "LeaveEndDate as EndDate ,LeaveStartDurationName as FromDuration,LeaveEndDurationName as ToDuration ," +
                    "Convert(Varchar(10),TotalDays) as TotalDays, LeaveTypeName as Type, Approval1StatusName as Status1," +
                    "Approval2StatusName as Status2,IsCancelled FROM [View_LeaveApplicationHistory] where " +
                    "staffid=@StaffId Order by CONVERT (DateTime, LeaveStartDate, 101) Desc ");

                var Obj = context.Database.SqlQuery<RALeaveApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).ToList();
                return Obj;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string ValidateLeaveApplication(string StaffId, DateTime FromDate, DateTime ToDate, string LeaveTypeId, decimal LeaveBalance)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Clear();
            SqlParameter[] sqlParameter = new SqlParameter[5];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            sqlParameter[1] = new SqlParameter("@FromDate", FromDate);
            sqlParameter[2] = new SqlParameter("@ToDate", ToDate);
            sqlParameter[3] = new SqlParameter("@LeaveTypeId", LeaveTypeId);
            sqlParameter[4] = new SqlParameter("@LeaveBalance", LeaveBalance);
            stringBuilder.Append("EXEC DBO.[ValidateLeaveApplication] @StaffId, @FromDate," +
                "@ToDate,@LeaveBalance, @LeaveTypeId ");
            var ret = (context.Database.SqlQuery<string>(stringBuilder.ToString(), sqlParameter).FirstOrDefault()).ToString();
            return ret;
        }


        //public string ValidateLeaveApplication(string StaffId, DateTime Startdate, int LeaveStartDurationId, DateTime EndDate, int LeaveEndDurationId, string LeaveTypeId, decimal TotalDays)
        //{
        //    string Result = "";
        //    try
        //    {

        //        SqlParameter[] Param = new SqlParameter[7];
        //        Param[0] = new SqlParameter("@StaffId", StaffId);
        //        Param[1] = new SqlParameter("@StartDate", System.Data.SqlDbType.Date);
        //        Param[1].Value = Startdate.ToString("dd") + "-" + Startdate.ToString("MMM") + "-" + Startdate.ToString("yyyy");

        //        Param[2] = new SqlParameter("@LeaveTypeId", LeaveTypeId);
        //        Param[3] = new SqlParameter("@TotalDays", TotalDays);
        //        Param[4] = new SqlParameter("@EndDate", System.Data.SqlDbType.Date);
        //        Param[4].Value = EndDate.ToString("dd") + "-" + EndDate.ToString("MMM") + "-" + EndDate.ToString("yyyy");
        //        Param[5] = new SqlParameter("@LeaveStartDurationId", LeaveStartDurationId);
        //        Param[6] = new SqlParameter("@LeaveEndDurationId", LeaveEndDurationId);

        //        StringBuilder builder = new StringBuilder();
        //        builder.Append("Exec Dbo.[ValidateLeaveApplication] @StaffId,@StartDate,@LeaveStartDurationId,@EndDate,@LeaveEndDurationId,@LeaveTypeId,@TotalDays");
        //        Result = context.Database.SqlQuery<string>(builder.ToString(), Param).FirstOrDefault();
        //    }
        //    catch (Exception e)
        //    {
        //        Result = e.Message;
        //    }
        //    return Result;
        //}


        public List<RALeaveApplication> GetAppliedLeavesForMyTeam(string StaffId, string AppliedId, string userRole)
        {
            StringBuilder Str = new StringBuilder();
            if (userRole == "3" || userRole == "5")
            {
                Str.Append(" select LeaveApplicationId as Id,StaffId,FirstName as StaffName, Remarks ," +
                    "LeaveStartDate as StartDate ,LeaveEndDate as EndDate ,LeaveStartDurationName as FromDuration," +
                    "LeaveEndDurationName as ToDuration ,Convert(Varchar(10),TotalDays) as TotalDays, " +
                    "LeaveTypeName as Type, Approval1StatusName as Status1,Approval2StatusName as Status2,IsCancelled," +
                    "Approval1Owner,Approval2Owner  FROM [vwLeaveApplicationApproval] where staffid= @StaffId");
            }
            else
            {
                Str.Append(" select LeaveApplicationId as Id,StaffId,FirstName as StaffName, Remarks ,LeaveStartDate as " +
                    "StartDate ,LeaveEndDate as EndDate ,LeaveStartDurationName as FromDuration,LeaveEndDurationName as " +
                    "ToDuration ,Convert(Varchar(10),TotalDays) as TotalDays, LeaveTypeName as Type, Approval1StatusName " +
                    "as Status1,Approval2StatusName as Status2,IsCancelled,Approval1Owner,Approval2Owner  FROM " +
                    "[vwLeaveApplicationApproval] where staffid=@StaffId and (AppliedBy=@AppliedId or " +
                    "Approval1Owner=@AppliedId or Approval2Owner=@AppliedId)");
            }
            var Obj = context.Database.SqlQuery<RALeaveApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId),
                new SqlParameter("@AppliedId", AppliedId)).ToList();
            return Obj;
        }


        public List<RALeaveApplication> GetAppliedLeaves(string StaffId)
        {
            StringBuilder Str = new StringBuilder();
            Str.Append("SELECT ");
            Str.Append("A.Id, ");
            Str.Append("A.StaffId, ");
            Str.Append("A.IsCancelled,");
            Str.Append("A.IsReviewerCancelled,");
            Str.Append("A.IsApproverCancelled,");
            Str.Append("(c.FirstName+' '+c.LastName) as StaffName,");
            Str.Append("Remarks, ");
            Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS StartDate, ");
            Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,EndDate,106),' ','-')) AS EndDate, ");
            Str.Append("(Select Name From LeaveDuration where Id=LeaveStartDurationId) as FromDuration,");
            Str.Append("(Select Name From LeaveDuration where Id=LeaveEndDurationId) as ToDuration,");
            Str.Append("TotalDays,AppliedBy ,");
            Str.Append("( select name from leavetype where id = A.LeaveTypeId ) as [Type],B.ApprovalOwner, B.ReviewerOwner, ");

            Str.Append(" CASE ");
            Str.Append("  WHEN IsapproverCancelled = 0 AND ApprovalStatusId = 1 THEN 'PENDING' ");
            Str.Append("  WHEN IsapproverCancelled = 1 AND ApprovalStatusId = 1 THEN 'CANCELLED' ");
            Str.Append("  WHEN IsapproverCancelled = 0 and ApprovalStatusId = 2 THEN 'APPROVED'  ");
            Str.Append("  WHEN IsapproverCancelled = 0 and ApprovalStatusId = 3  THEN 'REJECTED' ");
            Str.Append("  WHEN IsapproverCancelled = 1 and ApprovalStatusId = 1 and ReviewerstatusId = 2  THEN 'CANCELLED'  ");
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

            Str.Append(" FROM RequestApplication as A ");
            Str.Append(" join STAFFVIEW as c on a.StaffId = c.StaffId ");

            Str.Append(" inner join ApplicationApproval B on A.id=B.ParentId ");

            Str.Append(" WHERE A.RequestApplicationType = 'LA' AND A.StaffId = @StaffId ");
            Str.Append(" order by A.ApplicationDate desc ");

            //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
            var Obj = context.Database.SqlQuery<RALeaveApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).Select(d => new RALeaveApplication
            {
                Id = d.Id,
                StaffId = d.StaffId,
                StaffName = d.StaffName,
                AppliedBy = d.AppliedBy,
                Remarks = d.Remarks,
                FromDuration = d.FromDuration,
                StartDate = d.StartDate,
                ToDuration = d.ToDuration,
                EndDate = d.EndDate,
                TotalDays = d.TotalDays,
                Type = d.Type,
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




        public List<RALeaveApplication> GetAllEmployeesLeaveList(string StaffId, string userRole, string AppliedId)
        {
            StringBuilder Str = new StringBuilder();
            if (userRole == "3" || userRole == "5")
            {
                Str.Append("select top 30 LeaveApplicationId as Id,StaffId,FirstName as StaffName, Remarks ,LeaveStartDate as StartDate ," +
                    "LeaveEndDate as EndDate ,LeaveStartDurationName as FromDuration,LeaveEndDurationName as ToDuration ," +
                    "Convert(Varchar(10),TotalDays) as TotalDays, LeaveTypeName as Type, Approval1StatusName as Status1," +
                    "Approval2StatusName as Status2,IsCancelled FROM [View_LeaveApplicationHistory] " +
                    " where staffid=@StaffId Order by CONVERT (DateTime, LeaveStartDate, 101) Desc");
            }
            else
            {
                Str.Append(" select top 30 LeaveApplicationId as Id,StaffId,FirstName as StaffName, Remarks ,LeaveStartDate as StartDate ," +
                    "LeaveEndDate as EndDate ,LeaveStartDurationName as FromDuration,LeaveEndDurationName as ToDuration ," +
                    "Convert(Varchar(10),TotalDays) as TotalDays, LeaveTypeName as Type, Approval1StatusName as Status1," +
                    "Approval2StatusName as Status2,IsCancelled FROM [View_LeaveApplicationHistory] where staffid=@StaffId and " +
                    "(AppliedBy=@AppliedId or ApprovalOwner=@AppliedId or Approval2Owner=@AppliedId)" +
                    " Order by CONVERT (DateTime, LeaveStartDate, 101) Desc");
            }
            var Obj = context.Database.SqlQuery<RALeaveApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId),
                new SqlParameter("@AppliedId", AppliedId)).ToList();
            return Obj;
        }

        public List<RALeaveApplication> GetApprovedLeavesForMyTeam(string StaffId)
        {
            StringBuilder Str = new StringBuilder();
            Str.Append(" select LeaveApplicationId as Id,StaffId,FirstName as StaffName, Remarks ,LeaveStartDate as StartDate ," +
                "LeaveEndDate as EndDate ,LeaveStartDurationName as FromDuration,LeaveEndDurationName as ToDuration ," +
                "Convert(Varchar(10),TotalDays) as TotalDays, LeaveTypeName as Type, Approval1StatusName as Status1," +
                "Approval2StatusName as Status2,IsCancelled,Approval1Owner,Approval2Owner FROM [vwLeaveApplicationApproval] where " +
                "staffid=@StaffId ");
            var Obj = context.Database.SqlQuery<RALeaveApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).ToList();
            return Obj;
        }

        public List<LeaveReasonList> GetLeaveTypes(string user, string Gender)
        {
            var qryStr = new StringBuilder();
            Gender = context.Staff.Where(condition => condition.Id == user).Select(select => select.Gender).FirstOrDefault();
            qryStr.Append("Select '0' as Id , '-- Select LeaveType --' as Name Union All Select a.LeaveTypeId as Id,b.Name from LeaveGroupTxn as a join LeaveType as b on b.id = a.LeaveTypeId where a.LeaveGroupId = (select leavegroupid from StaffOfficial where StaffId = @user) and a.IsActive = 1 ");
            if (Gender == "M")
            {
                qryStr.Append("and a.LeaveTypeId  not in('LV0005','LV0006')");
            }
            else
            {
                qryStr.Append("and a.LeaveTypeId  not in( 'LV0005','LV0039')");
            }
            try
            {
                var lst = context.Database.SqlQuery<LeaveReasonList>(qryStr.ToString(), new SqlParameter("@user", user)).Select(d => new LeaveReasonList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<LeaveReasonList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LeaveReasonList>();
            }
        }

        public List<LeaveReasonList> GetLeaveReasonList(string user)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select a.LeaveTypeId as Id,b.Name from LeaveGroupTxn as a join LeaveType as b on b.id = a.LeaveTypeId where a.LeaveGroupId = (select leavegroupid from StaffOfficial where StaffId = @user) and a.IsActive = 1");
            //qryStr.Append("SELECT CONVERT ( VARCHAR , Id ) AS Id , Name FROM LeaveType WHERE ISACTIVE = 1");

            try
            {
                var lst = context.Database.SqlQuery<LeaveReasonList>(qryStr.ToString(), new SqlParameter("@user", user)).Select(d => new LeaveReasonList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<LeaveReasonList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LeaveReasonList>();
            }
        }

        public List<LeaveDuration> GetLeaveDurations()
        {
            var Obj = context.LeaveDuration.ToList();
            return Obj;
        }

        public string GetTotalDaysLeave(string StaffId, string LeaveStartDurationId, string FromDate, string ToDate, string LeaveEndDurationId, string LeaveType)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select dbo.fnGetTotalDaysLeave(@StaffId,@LeaveStartDurationId,@FromDate,@ToDate,@LeaveEndDurationId,@LeaveType)");

            try
            {
                var data = context.Database.SqlQuery<string>(QryStr.ToString(), new SqlParameter("@StaffId", StaffId)
                    , new SqlParameter("@LeaveStartDurationId", LeaveStartDurationId), new SqlParameter("@FromDate", FromDate)
                    , new SqlParameter("@ToDate", ToDate), new SqlParameter("@LeaveEndDurationId", LeaveEndDurationId)
                    , new SqlParameter("@LeaveType", LeaveType)).FirstOrDefault();

                if (string.IsNullOrEmpty(data) == true)
                {
                    return "EMPTY";
                }
                else
                {
                    return data;
                }
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }

        public string GetUniqueId()
        {
            return context.Database.SqlQuery<string>("select convert(varchar,getdate(),112) + replace(convert(varchar,getdate(),114),':','')").First();
        }

        public string RequestApplicationMustNotOverLapWithTheOther(string StaffId, string LeaveStartDurationId, string LeaveStartDate, string LeaveEndDate, string LeaveEndDurationId)
        {
            return context.Database.SqlQuery<string>("Exec DBO.[IsApplicationOverLapping] @StaffId,@LeaveStartDurationId," +
                "@LeaveStartDate,@LeaveEndDate,@LeaveEndDurationId ",
                new SqlParameter("@StaffId", StaffId),
                new SqlParameter("@LeaveStartDurationId", LeaveStartDurationId),
                new SqlParameter("@LeaveStartDate", LeaveStartDate),
                new SqlParameter("@LeaveEndDate", LeaveEndDate),
                new SqlParameter("@LeaveEndDurationId", LeaveEndDurationId)).First();
        }

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
                    // save to application approval table.
                    context.ApplicationApproval.Add(DataToSave.AA);
                    // save to email send log table.
                    if (isFinalLevelApproval == true)
                    {
                        if (DataToSave.ELA != null)
                        {
                            SqlParameter[] sqlParameter = new SqlParameter[4];
                            sqlParameter[0] = new SqlParameter("@StaffId", DataToSave.RA.StaffId);
                            sqlParameter[1] = new SqlParameter("@RefId", DataToSave.ELA.RefId);
                            sqlParameter[2] = new SqlParameter("@TransactionFlag", DataToSave.ELA.TransactionFlag);
                            sqlParameter[3] = new SqlParameter("@LeaveCreditDebitReason", DataToSave.ELA.LeaveCreditDebitReasonId);
                            context.Database.ExecuteSqlCommand("Delete from EmployeeLeaveAccount Where StaffId = @StaffId And" +
                                " RefId = @RefId And TransactionFlag = @TransactionFlag and LeaveCreditDebitReasonId = @LeaveCreditDebitReason", sqlParameter);

                            context.EmployeeLeaveAccount.Add(DataToSave.ELA);
                        }
                        if (DataToSave.RA.StartDate != null && DataToSave.RA.EndDate != null)
                        {
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
                            if (l.To != "-" && l.To != "")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                }
                catch (DbEntityValidationException err)
                {
                    Trans.Rollback();
                    throw err;
                }
            }
        }

        public void SaveDocumentInformation(DocumentUpload LawDocument)
        {
            context.DocumentUpload.Add(LawDocument);
            context.SaveChanges();
        }

        public void RejectApplication(ClassesToSave CTS)
        {
            CommonRepository commonRepository = new CommonRepository();
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
                                commonRepository.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                }
                catch
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }

        public void ApproveApplication(ClassesToSave CTS, bool IsFianlLevelApproval)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            CommonRepository commonRepository = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    context.Entry(CTS.RA).Property("IsApproved").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2statusId").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2By").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2On").IsModified = true;
                    if (IsFianlLevelApproval == true)
                    {
                        // Delete the duplicate debit entry if exist before insert
                        if (CTS.ELA != null)
                        {
                            SqlParameter[] sqlParameter = new SqlParameter[4];
                            sqlParameter[0] = new SqlParameter("@StaffId", CTS.RA.StaffId);
                            sqlParameter[1] = new SqlParameter("@RefId", CTS.ELA.RefId);
                            sqlParameter[2] = new SqlParameter("@TransactionFlag", CTS.ELA.TransactionFlag);
                            sqlParameter[3] = new SqlParameter("@LeaveCreditDebitReason", CTS.ELA.LeaveCreditDebitReasonId);
                            context.Database.ExecuteSqlCommand("Delete from EmployeeLeaveAccount Where StaffId = @StaffId And" +
                                " RefId = @RefId And TransactionFlag = @TransactionFlag and " +
                                "LeaveCreditDebitReasonId = @LeaveCreditDebitReason", sqlParameter);
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
                                commonRepository.LogIntoIntoAttendanceControlTable(CTS.RA.StaffId, fromDate.Date, toDate.Date, CTS.RA.RequestApplicationType, CTS.AA.Id);
                            }
                        }
                    }
                    context.SaveChanges();
                    Trans.Commit();
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                            if (l.To != "-" && l.To != "")
                                commonRepository.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                }
                catch (Exception e)
                {
                    Trans.Rollback();
                    throw e;
                }
            }
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
                        //set the Employee Leave Account class to add.
                        SqlParameter[] sqlParameter = new SqlParameter[4];
                        sqlParameter[0] = new SqlParameter("@StaffId", CTS.RA.StaffId);
                        sqlParameter[1] = new SqlParameter("@RefId", CTS.ELA.RefId);
                        sqlParameter[2] = new SqlParameter("@TransactionFlag", CTS.ELA.TransactionFlag);
                        sqlParameter[3] = new SqlParameter("@LeaveCreditDebitReason", CTS.ELA.LeaveCreditDebitReasonId);
                        context.Database.ExecuteSqlCommand("Delete from EmployeeLeaveAccount Where StaffId = @StaffId And" +
                            " RefId = @RefId And TransactionFlag = @TransactionFlag and " +
                            "LeaveCreditDebitReasonId = @LeaveCreditDebitReason", sqlParameter);
                        context.EmployeeLeaveAccount.Add(CTS.ELA);
                    }
                    if (CTS.RA.IsApproved == true)
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

        public void CancelApplication(ClassesToSave CTS)
        {

            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;

            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    context.RequestApplication.AddOrUpdate(CTS.RA);

                    //set the Employee Leave Account class to add.
                    if (CTS.ELA != null)
                    {
                        CTS.ELA.TransactionDate = CTS.RA.ApplicationDate;
                        CTS.ELA.Narration = "Approved the leave application.";
                        context.EmployeeLeaveAccount.Add(CTS.ELA);
                    }

                    fromDate = Convert.ToDateTime(CTS.RA.StartDate);
                    toDate = Convert.ToDateTime(CTS.RA.EndDate);
                    if (fromDate < currentDate && CTS.RA.IsApproved == true)
                    {
                        if (toDate >= currentDate)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        using (CommonRepository commonRepository = new CommonRepository())
                        {
                            commonRepository.LogIntoIntoAttendanceControlTable(CTS.RA.StaffId, fromDate, toDate, CTS.RA.RequestApplicationType, CTS.RA.Id);
                        }
                    }

                    //save the changes.
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


        public ApplicationApproval GetApplicationApproval(string ParentId)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(ParentId) && d.ParentType.Equals("LA")).FirstOrDefault();
        }

        public decimal GetLeaveBalance(string StaffId, string LeaveTypeId)
        {
            return context.Database.SqlQuery<decimal>("SELECT convert(decimal(5,2),availablebalance) as LeaveBalance FROM LeaveBalance where StaffId = @StaffId AND LeaveTypeId = @LeaveTypeId",
                new SqlParameter("@StaffId", StaffId),
                new SqlParameter("@LeaveTypeId", LeaveTypeId)).FirstOrDefault();
        }
        public decimal GetLeaveBalanceForApprovalStage(string StaffId, string LeaveTypeId)
        {
            return context.Database.SqlQuery<decimal>("SELECT convert(decimal(5,2),LEAVEBALANCE) as LeaveBalance FROM LeaveBalance where StaffId = @StaffId AND LeaveTypeId = @LeaveTypeId",
                new SqlParameter("@StaffId", StaffId),
                new SqlParameter("@LeaveTypeId", LeaveTypeId)).FirstOrDefault();
        }

        public RequestApplication GetRequestApplicationDetails(string Id)
        {
            return context.RequestApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }
        public string SaveCompensatoryWorkingRepository(CompensatoryWorkingModel model)
        {
            using (DbContextTransaction trans = context.Database.BeginTransaction())
            {
                try
                {
                    CompensatoryWorking tbl = new CompensatoryWorking();
                    tbl.CompensatoryWorkingDate = model.CompensatoryWorkingDate;
                    tbl.LeaveDate = model.LeaveDate;
                    tbl.Reason = model.Reason;
                    tbl.IsActive = true;
                    tbl.CreatedOn = DateTime.Now;
                    tbl.CreatedBy = model.CreatedBy;
                    context.CompensatoryWorking.Add(tbl);
                    context.SaveChanges();

                    trans.Commit();
                    Message = "success";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    Message = e.Message;
                }
            }
            return Message;
        }
        public string NewValidateLeavePrefixSuffix(string StaffId, DateTime LeaveStartDate, DateTime LeaveEndDate, string FromDuration, string ToDuration, string LeaveTypeId, decimal TotalDays)
        {
            string Message = string.Empty;
            try
            {
                SqlParameter[] Param = new SqlParameter[7];
                Param[0] = new SqlParameter("@StaffId", StaffId ?? "");
                Param[1] = new SqlParameter("@LeaveStartDate", LeaveStartDate.Date);
                Param[2] = new SqlParameter("@LeaveEndDate", LeaveEndDate.Date);
                Param[3] = new SqlParameter("@FromDuration", Convert.ToInt32(FromDuration));
                Param[4] = new SqlParameter("@ToDuration", Convert.ToInt32(ToDuration));
                Param[5] = new SqlParameter("@LeaveTypeId", LeaveTypeId ?? "");
                Param[6] = new SqlParameter("@TotalDays", TotalDays);

                StringBuilder builder = new StringBuilder();
                builder.Append("Select DBO.[ValidateLeavePrefixSuffix] (@StaffId,@LeaveStartDate,@LeaveEndDate,@FromDuration,@ToDuration,@LeaveTypeId,@TotalDays)");
                Message = context.Database.SqlQuery<string>(builder.ToString(), Param).FirstOrDefault();
            }
            catch (Exception e)
            {
                Message = e.GetBaseException().Message;
            }
            return Message;
        }
        public string GetStartduration(int LeaveStartDurationId)
        {
            var LeaveStartDuration = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select [Name] from leaveduration where id = @LeaveStartDurationId");

            try
            {
                LeaveStartDuration = context.Database.SqlQuery<string>(qryStr.ToString() , new SqlParameter("@LeaveStartDurationId", LeaveStartDurationId)).FirstOrDefault();
                return LeaveStartDuration;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetEndduration(int LeaveEndDurationId)
        {
            var LeaveStartDuration = string.Empty;
            var qryStr = new StringBuilder();
            qryStr.Append("select [Name] from leaveduration where id =  @LeaveEndDurationId ");

            try
            {
                LeaveStartDuration = context.Database.SqlQuery<string>(qryStr.ToString() , new SqlParameter("@LeaveEndDurationId", LeaveEndDurationId)).FirstOrDefault();
                return LeaveStartDuration;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public void Dispose()
        //{
        //    ((IDisposable)context).Dispose();
        //}
    }
}
