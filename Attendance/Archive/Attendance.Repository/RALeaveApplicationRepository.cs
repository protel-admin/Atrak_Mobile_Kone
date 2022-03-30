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

        public List<RALeaveApplication> GetAppliedLeavesForMyTeam(string StaffId, string AppliedId, string Role)
        {
            StringBuilder Str = new StringBuilder();
            Str.Clear();
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
                Str.Append("(Select Name From LeaveDuration where Id=LeaveStartDurationId) as FromDuration,");
                Str.Append("(Select Name From LeaveDuration where Id=LeaveEndDurationId) as ToDuration,");
                //Str.Append("(Select * from ApplicationApproval where ApprovedBy=@StaffId) as  ");
                Str.Append("TotalDays,AppliedBy,");
                Str.Append("( select name from leavetype where id = A.LeaveTypeId ) as [Type],B.ApprovalOwner, B.ReviewerOwner, ");

                Str.Append(" CASE ");
                Str.Append("WHEN IsapproverCancelled = 0 AND ApprovalStatusId = 1 THEN 'PENDING' ");
                Str.Append("  WHEN IsapproverCancelled = 1 AND ApprovalStatusId = 1 THEN 'CANCELLED' ");
                Str.Append("  WHEN IsapproverCancelled = 0 and ApprovalStatusId = 2 THEN 'APPROVED'  ");
                Str.Append("  WHEN IsapproverCancelled = 0 and ApprovalStatusId = 3  THEN 'REJECTED'  ");
                Str.Append("  WHEN IsapproverCancelled = 1 and ApprovalStatusId = 1 and ReviewerstatusId = 2  THEN 'CANCELLED'  ");
                Str.Append("  WHEN IsapproverCancelled = 1 and ApprovalStatusId = 2 THEN 'APPROVED BUT CANCELLED' ");
                Str.Append("  WHEN IsapproverCancelled = 1 and ApprovalStatusId = 3 THEN 'REJECTED BUT CANCELLED' ");
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
                Str.Append("join ApplicationApproval as B on a.id= b.parentid ");
                Str.Append("join STAFFVIEW as c on a.Staffid = c.Staffid ");
                Str.Append("WHERE a.RequestApplicationType = 'LA' AND a.StaffId=@StaffId  order by a.ApplicationDate desc ");
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
                Str.Append("(Select Name From LeaveDuration where Id=LeaveStartDurationId) as FromDuration,");
                Str.Append("(Select Name From LeaveDuration where Id=LeaveEndDurationId) as ToDuration,");
                //Str.Append("(Select * from ApplicationApproval where ApprovedBy=@StaffId) as  ");
                Str.Append("TotalDays,AppliedBy,");
                Str.Append("( select name from leavetype where id = A.LeaveTypeId ) as [Type],B.ApprovalOwner, B.ReviewerOwner, ");

                Str.Append(" CASE ");
                Str.Append("WHEN IsapproverCancelled = 0 AND ApprovalStatusId = 1 THEN 'PENDING' ");
                Str.Append("  WHEN IsapproverCancelled = 1 AND ApprovalStatusId = 1 THEN 'CANCELLED' ");
                Str.Append("  WHEN IsapproverCancelled = 0 and ApprovalStatusId = 2 THEN 'APPROVED'  ");
                Str.Append("  WHEN IsapproverCancelled = 0 and ApprovalStatusId = 3  THEN 'REJECTED'  ");
                Str.Append("  WHEN IsapproverCancelled = 1 and ApprovalStatusId = 1 and ReviewerstatusId = 2  THEN 'CANCELLED'  ");
                Str.Append("  WHEN IsapproverCancelled = 1 and ApprovalStatusId = 2 THEN 'APPROVED BUT CANCELLED' ");
                Str.Append("  WHEN IsapproverCancelled = 1 and ApprovalStatusId = 3 THEN 'REJECTED BUT CANCELLED' ");
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
                Str.Append("join ApplicationApproval as B on a.id= b.parentid ");
                Str.Append("join STAFFVIEW as c on a.Staffid = c.Staffid ");
                Str.Append("WHERE a.RequestApplicationType = 'LA' AND a.StaffId=@StaffId AND (a.AppliedBy = @AppliedId or b.ApprovalOwner=@AppliedId or b.ReviewerOwner=@AppliedId)  order by a.ApplicationDate desc ");
            }
            //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
            var Obj = context.Database.SqlQuery<RALeaveApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@AppliedId", AppliedId)).Select(d => new RALeaveApplication
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
                IsCancelled = d.IsCancelled,
                IsReviewerCancelled = d.IsReviewerCancelled,
                IsApproverCancelled = d.IsApproverCancelled,
                ReviewerStatus = d.ReviewerStatus,
                ApproverStatus = d.ApproverStatus,
                ApprovalOwner = d.ApprovalOwner,
                ReviewerOwner = d.ReviewerOwner

            }).ToList();
            return Obj;
        }
         
        public List<RALeaveApplication> GetApprovedLeavesForMyTeam(string staffId)
        {
            StringBuilder Str = new StringBuilder();
            Str.Append("SELECT ");
            Str.Append(" A.Id, ");
            Str.Append(" A.StaffId, "); //Rajesh Aug 20
            Str.Append(" C.FirstName as StaffName , "); //Rajesh Aug 20
            Str.Append(" Remarks, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS StartDate, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,EndDate,106),' ','-')) AS EndDate, ");
            Str.Append(" (Select Name From LeaveDuration where Id=LeaveStartDurationId) as FromDuration,");
            Str.Append(" (Select Name From LeaveDuration where Id=LeaveEndDurationId) as ToDuration,");
            Str.Append(" TotalDays, ");
            Str.Append("( select name from leavetype where id = A.LeaveTypeId ) as [Type], ");
            Str.Append(" CASE  ");
            Str.Append($" WHEN IsCancelled = 0 and IsApproved = 1 and ApprovedBy = '{staffId}' THEN 'APPROVED'");
            Str.Append($" WHEN IsCancelled = 0 and IsReviewed = 1 and ReviewedBy = '{staffId}' THEN 'APPROVED' ");
            Str.Append($" WHEN IsCancelled = 0 and IsRejected = 1 and ApprovalStatusId = 3  and ApprovedBy = '{staffId}'  THEN 'REJECTED' ");
            Str.Append($" WHEN IsCancelled = 0 and IsRejected = 1 and ReviewerstatusId = 3  and ReviewedBy = '{staffId}' ");
            Str.Append($" THEN 'REJECTED' WHEN IsCancelled =1 and CancelledBy = '{staffId}' THEN 'CANCELLED' ");
            
            Str.Append(" END as [Status] ");
            Str.Append(" FROM RequestApplication A ");
            Str.Append(" join ApplicationApproval B on A.id = B.ParentId");
            Str.Append(" join Staff C  on A.StaffId = C.Id ");    //Rajesh Aug 20
            Str.Append(" WHERE RequestApplicationType = 'LA' and ");
            Str.Append(" ( IsApproved = 1 or IsReviewed = 1 or IsCancelled = 1 or IsRejected = 1 ) ");
            Str.Append( $" AND (B.ApprovedBy = '{staffId}' or B.ReviewedBy='{staffId}') order by A.ApplicationDate desc  ");
            //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
            var Obj = context.Database.SqlQuery<RALeaveApplication>(Str.ToString(), new SqlParameter("@StaffId", staffId)).Select(d => new RALeaveApplication
            {
                Id = d.Id,
                StaffId = d.StaffId,
                StaffName=d.StaffName,      //Rajesh Aug 20
                Remarks = d.Remarks,
                FromDuration = d.FromDuration,
                StartDate = d.StartDate,
                ToDuration = d.ToDuration,
                EndDate = d.EndDate,
                TotalDays = d.TotalDays,
                Type = d.Type,
                ApprovalOwner=d.ApprovalOwner,
                ReviewerOwner=d.ReviewerOwner,
                ApproverStatus=d.ApproverStatus,
                ReviewerStatus=d.ReviewerStatus,
                IsCancelled = d.IsCancelled,
                IsReviewerCancelled = d.IsReviewerCancelled,
                IsApproverCancelled = d.IsApproverCancelled,
                Status = d.Status
            }).ToList();
            return Obj;
        }

        public List<LeaveReasonList> GetLeaveTypes(string user)
        {
            //var Obj = context.LeaveType.Where(d => d.IsAccountable.Equals(true) && d.IsActive.Equals(true) && d.IsCommon.Equals(false)).ToList();
            //return Obj;

            var qryStr = new StringBuilder();
            qryStr.Clear();
            //qryStr.Append("select a.LeaveTypeId as Id,b.Name from LeaveGroupTxn as a join LeaveType as b on b.id = a.LeaveTypeId where a.LeaveGroupId = (select leavegroupid from StaffOfficial where StaffId = '" + user + "') and a.IsActive = 1");
            qryStr.Append("select Id, Name from ( select a.LeaveTypeId as Id,b.Name, (select case when Id = 'LV0003' then 0 else 1 end from" +
                " LeaveType where Id = B.Id ) as [Level] from LeaveGroupTxn as a join LeaveType as b on b.id = a.LeaveTypeId " +
                "where a.LeaveGroupId = (select leavegroupid from StaffOfficial where StaffId = '" + user + "')  and a.IsActive = 1 ) x" +
                " Where Id not in ('LV0005','LV0039')  order by X.Level");
            //qryStr.Append("SELECT CONVERT ( VARCHAR , Id ) AS Id , Name FROM LeaveType WHERE ISACTIVE = 1");

            try
            {
                var lst = context.Database.SqlQuery<LeaveReasonList>(qryStr.ToString()).Select(d => new LeaveReasonList()
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
            //qryStr.Append("select a.LeaveTypeId as Id,b.Name from LeaveGroupTxn as a join LeaveType as b on b.id = a.LeaveTypeId where a.LeaveGroupId = (select leavegroupid from StaffOfficial where StaffId = '" + user + "') and a.IsActive = 1");
            qryStr.Append("select Id, Name from ( select a.LeaveTypeId as Id,b.Name, (select case when Id = 'LV0003' then 0 else 1 end from LeaveType where Id = B.Id ) as [Level] from LeaveGroupTxn as a join LeaveType as b on b.id = a.LeaveTypeId where a.LeaveGroupId = (select leavegroupid from StaffOfficial where StaffId = '" + user + "') and a.IsActive = 1 ) x order by X.Level");
            //qryStr.Append("SELECT CONVERT ( VARCHAR , Id ) AS Id , Name FROM LeaveType WHERE ISACTIVE = 1");

            try
            {
                var lst = context.Database.SqlQuery<LeaveReasonList>(qryStr.ToString()).Select(d => new LeaveReasonList()
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

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@StaffId", StaffId ?? "");
            param[1] = new SqlParameter("@FromDate", FromDate ?? "");
            param[2] = new SqlParameter("@ToDate", ToDate ?? "");
            param[3] = new SqlParameter("@LeaveStartDurationId", LeaveStartDurationId);
            param[4] = new SqlParameter("@LeaveEndDurationId", LeaveEndDurationId);
            param[5] = new SqlParameter("@LeaveType", LeaveType);

            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select dbo.fnGetTotalDaysLeave(@StaffId,@LeaveStartDurationId,@FromDate,@ToDate,@LeaveEndDurationId,@LeaveType)");

            try
            {
                var data = context.Database.SqlQuery<string>(QryStr.ToString(),param).FirstOrDefault();

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

        public string ValidateLeaveApplication(string StaffId, DateTime Startdate, int LeaveStartDurationId, DateTime EndDate, int LeaveEndDurationId, string LeaveTypeId, decimal TotalDays)
        {
            string Result = "";
            try
            {

                SqlParameter[] Param = new SqlParameter[7];
                Param[0] = new SqlParameter("@StaffId", StaffId);
                Param[1] = new SqlParameter("@StartDate", System.Data.SqlDbType.Date);
                Param[1].Value = Startdate.ToString("dd") + "-" + Startdate.ToString("MMM") + "-" + Startdate.ToString("yyyy");

                Param[2] = new SqlParameter("@LeaveTypeId", LeaveTypeId);
                Param[3] = new SqlParameter("@TotalDays", TotalDays);
                Param[4] = new SqlParameter("@EndDate", System.Data.SqlDbType.Date);
                Param[4].Value = EndDate.ToString("dd") + "-" + EndDate.ToString("MMM") + "-" + EndDate.ToString("yyyy");
                Param[5] = new SqlParameter("@LeaveStartDurationId", LeaveStartDurationId);
                Param[6] = new SqlParameter("@LeaveEndDurationId", LeaveEndDurationId);

                StringBuilder builder = new StringBuilder();
                builder.Append("Select DBO.[fnValidateLeaveApplication] ( @StaffId,@StartDate,@LeaveStartDurationId,@EndDate,@LeaveEndDurationId,@LeaveTypeId,@TotalDays)");
                Result = context.Database.SqlQuery<string>(builder.ToString(), Param).FirstOrDefault();
            }
            catch (Exception e)
            {
                Result = e.Message;
            }
            return Result;
        }

        public List<DropDownListString> GetAllStaffForWorkAllocation(string StaffId)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select b.staffid as Id, a.firstname+''+a.lastname as Text from StaffOfficial b " +
                           "join staff a on b.StaffId = a.Id " +
                           "where b.LocationId = (select locationId from staffofficial where staffid=@StaffId) and b.DepartmentId = (select departmentid from staffofficial where staffid = @StaffId) and b.staffid !=@StaffId");
            var lst = context.Database.SqlQuery<DropDownListString>(QryStr.ToString(), new SqlParameter("@StaffId", StaffId)).ToList();
            return lst;
        }

        public void SaveRequestApplication(ClassesToSave DataToSave, string SecurityGroupId, bool isFinalLevelApproval)
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

                    if (isFinalLevelApproval == true)
                    {
                        context.EmployeeLeaveAccount.Add(DataToSave.ELA);
                        fromDate = Convert.ToDateTime(DataToSave.RA.StartDate);
                        toDate = Convert.ToDateTime(DataToSave.RA.EndDate);
                        if (fromDate < currentDate)
                        {
                            if (toDate >= currentDate)
                            {
                                toDate = DateTime.Now.AddDays(-1);
                            }
                            CR.LogIntoIntoAttendanceControlTable(DataToSave.RA.StaffId, fromDate, toDate, DataToSave.RA.RequestApplicationType, DataToSave.AA.Id);
                        }
                    }

                    context.SaveChanges();
                    Trans.Commit(); 
                    // save to email send log table.
                    if (DataToSave.ESL != null)
                    {
                    foreach (var l in DataToSave.ESL)
                        {
                        if (l.To != "-")
                            CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);

                        }
                    }
                }

                catch (System.Data.Entity.Validation.DbEntityValidationException err)
                {
                    Trans.Rollback();
                    throw;
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
            var CR = new CommonRepository();
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

                    if (CTS.APA != null)
                    {
                        //context.Entry(CTS.APA).Property("IsRejected").IsModified = true;
                        //context.Entry(CTS.APA).Property("RejectMailSent").IsModified = true;

                        context.AlternativePersonAssign.AddOrUpdate(CTS.APA);


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
                    //context.Entry(CTS.RA).Property("IsApproved").IsModified = true;
                    //context.Entry(CTS.RA).Property("IsReviewed").IsModified = true;

                    context.RequestApplication.AddOrUpdate(CTS.RA);

                    //Update the application approval table.
                    //context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    //context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    //context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    //context.Entry(CTS.AA).Property("Comment").IsModified = true;
                    //context.Entry(CTS.AA).Property("ReviewerstatusId").IsModified = true;
                    //context.Entry(CTS.AA).Property("ReviewedBy").IsModified = true;
                    //context.Entry(CTS.AA).Property("ReviewedOn").IsModified = true;

                    context.ApplicationApproval.AddOrUpdate(CTS.AA);

                    if (CTS.APA != null)
                    {
                        //context.Entry(CTS.APA).Property("IsApproved").IsModified = true;
                        //context.Entry(CTS.APA).Property("ConfirmationMailSent").IsModified = true;
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
                    if (fromDate < currentDate && CTS.RA.IsReviewed == true)
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

        public void CancelApplication(ClassesToSave CTS)
        {

            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;

                using (var Trans = context.Database.BeginTransaction())
                {
                    try
                    {
                    //context.RequestApplication.Attach(CTS.RA);
                    //var entry = context.Entry(CTS.RA);
                    //entry.Property(x => x.IsCancelled).IsModified = true;
                    //CTS.RA.CancelledBy = CTS.RA.CancelledBy;

                    //entry.Property(x => x.IsCancelled).IsModified = true;
                    //entry.Property(x => x.CancelledDate).IsModified = true;
                    //entry.Property(x => x.CancelledBy).IsModified = true;

                    //entry.Property(x => x.IsReviewerCancelled).IsModified = true;
                    //entry.Property(x => x.ReviewerCancelledDate).IsModified = true;
                    //entry.Property(x => x.ReviewerCancelledBy).IsModified = true;

                    //entry.Property(x => x.IsApproverCancelled).IsModified = true;
                    //entry.Property(x => x.ApproverCancelledDate).IsModified = true;
                    //entry.Property(x => x.ApproverCancelledBy).IsModified = true;


                    //CTS.RA.CancelledBy = CTS.RA.CancelledBy;
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

                        //set the Employee Leave Account class to add.
                    if (CTS.ELA != null)
                    {
                        CTS.ELA.TransactionDate = CTS.RA.ApplicationDate;
                        CTS.ELA.Narration = "Approved the leave application.";
                        context.EmployeeLeaveAccount.Add(CTS.ELA);
                    }

                    fromDate = Convert.ToDateTime(CTS.RA.StartDate);
                    toDate = Convert.ToDateTime(CTS.RA.EndDate);
                    if (fromDate < currentDate && CTS.RA.IsReviewed == true)
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

        public AlternativePersonAssign GetApplicationForWorkAllocation(string ParentId)
        {
            return context.AlternativePersonAssign.Where(d => d.ParentId.Equals(ParentId)).FirstOrDefault();
        }

        public decimal GetLeaveBalance(string StaffId, string LeaveTypeId)
        {
            return context.Database.SqlQuery<decimal>("SELECT LeaveBalance FROM LeaveBalance where StaffId = @StaffId AND LeaveTypeId = @LeaveTypeId",
                new SqlParameter("@StaffId", StaffId),
                new SqlParameter("@LeaveTypeId", LeaveTypeId)).FirstOrDefault();
        }

        public RequestApplication GetRequestApplicationDetails(string Id)
        {
            return context.RequestApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }
        public string GetValidationForYear(string Id)
        {
            var CurrentYear = DateTime.Now.Year;
            var obj = GetRequestApplicationDetails(Id);
            int LeaveTypeYear = obj.StartDate.Year;
            //DateTime LeaveDate = (RA.StartDate).Year;
            //var PreviousYear = CurrentYear - yr;
            try
            {
                if (LeaveTypeYear < CurrentYear)
                {
                    throw new Exception("You can not approve leave for previous year");
                }
                return LeaveTypeYear.ToString();
            }
            catch (Exception err)
            {
                throw err;
            }

        }

        #region Coff Req Avail
        public void CancelApplication(ClassesToSave CTS, string user)
        {
           
            if (CTS.ELA == null)
            {
                //context.Entry(CTS.RA).State = System.Data.Entity.EntityState.Modified;
                //CTS.RA.CancelledBy = CTS.RA.StaffId;
                context.Entry(CTS.RA).Property("IsCancelled").IsModified = true;
                context.Entry(CTS.RA).Property("CancelledDate").IsModified = true;
                context.Entry(CTS.RA).Property("CancelledBy").IsModified = true;
                context.SaveChanges();
            }
            else
            {
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
                            context.EmployeeLeaveAccount.Add(CTS.ELA);
                        }
                        if (CTS.RA.StaffId != null && CTS.RA.StartDate != null && CTS.RA.EndDate != null)
                        {
                            DateTime fromDate = DateTime.Now;
                            DateTime toDate = DateTime.Now;
                            DateTime currentDate = DateTime.Now;
                            fromDate = Convert.ToDateTime(CTS.RA.StartDate);
                            toDate = Convert.ToDateTime(CTS.RA.EndDate);
                            if (fromDate < currentDate)
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
                        }
                        if (CTS.ESL != null)
                        {
                            foreach (var l in CTS.ESL)
                                //context.EmailSendLog.Add(l);
                                if (l.To != "-" && l.To != "")
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
        }
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
        public ApplicationApproval GetApplicationApprovalForCoffAvailing(string ParentId)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(ParentId) && d.ParentType.Equals("CO")).FirstOrDefault();
        }
        #endregion
    }
}
