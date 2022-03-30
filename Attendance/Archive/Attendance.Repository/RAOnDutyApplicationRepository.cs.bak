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
using System.Data.Entity.Infrastructure;

namespace Attendance.Repository
{
    public class RAOnDutyApplicationRepository : IDisposable
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
        public RAOnDutyApplicationRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<RAODRequestApplication> GetAppliedODRequest(string StaffId, string ApplicationType)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Append(" SELECT ");
                Str.Append(" a.Id, ");
                Str.Append(" a.StaffId, ");
                Str.Append(" A.IsCancelled,");
                Str.Append(" A.IsReviewerCancelled,");
                Str.Append(" A.IsApproverCancelled,");
                Str.Append(" dbo.fnGetStaffName(a.StaffId) as StaffName, ");
                Str.Append(" Remarks, ");
                Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS StartDate, ");
                Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,EndDate,106),' ','-')) AS EndDate, ");
                Str.Append(" TotalDays,AppliedBy, ");
                Str.Append(" upper(replace(convert(varchar,StartDate,106),' ','-')) + ' ' + convert(varchar(8),StartDate,114) AS StartTime,");
                Str.Append(" upper(replace(convert(varchar,EndDate,106),' ','-')) + ' ' + convert(varchar(8),EndDate,114) AS EndTime,");
                Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,TotalHours,108),' ','-')) AS TotalHours, ");
                Str.Append(" ODDuration,B.ApprovalOwner, B.ReviewerOwner,");
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
                Str.Append(" END as [ReviewerStatus],B.ParentType as ApplicationType ");
                Str.Append(" FROM RequestApplication A ");
                Str.Append(" join STAFFVIEW as c on a.Staffid = c.Staffid ");
                Str.Append("inner join ApplicationApproval B on A.id=B.ParentId ");
                Str.Append(" WHERE RequestApplicationType = @ApplicationType AND a.StaffId = @StaffId order by A.ApplicationDate desc ");

                var Obj = context.Database.SqlQuery<RAODRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@ApplicationType", ApplicationType)).Select(d => new RAODRequestApplication
                {
                    ApplicationType = d.ApplicationType,
                    Id = d.Id,
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    AppliedBy = d.AppliedBy,
                    Remarks = d.Remarks,
                    ODDuration = d.ODDuration,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    TotalDays = d.TotalDays,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                    TotalHours = d.TotalHours,
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
                return new List<RAODRequestApplication>();
            }
        }

        public List<RAODRequestApplication> GetApprovedWFHRequestForMyTeam(string staffId)
        {
            //Rajesh Nov 09 for mobile
            StringBuilder Str = new StringBuilder();
            Str.Append(" SELECT ");
            Str.Append(" a.Id, ");
            Str.Append(" a.StaffId, ");
            Str.Append(" A.IsCancelled, ");
            Str.Append(" A.IsReviewerCancelled, ");
            Str.Append(" A.IsApproverCancelled, ");
            Str.Append("(c.FirstName+' '+c.LastName) as StaffName,");
            Str.Append(" Remarks, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS StartDate, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,EndDate,106),' ','-')) AS EndDate, ");
            Str.Append(" TotalDays,AppliedBy, ");
            Str.Append(" CASE WHEN A.ODDuration = 'SINGLEDAY' THEN replace(convert(varchar, StartDate,106),' ','-') +' ' +      convert(varchar(8), StartDate, 114) ELSE '-' END AS StartTime,   ");
            Str.Append(" CASE WHEN A.ODDuration = 'SINGLEDAY' THEN replace(convert(varchar, EndDate,106),  ' ','-') +' ' + convert(varchar(8), EndDate, 114) ELSE '-' END AS EndTime, ");
            Str.Append(" CASE WHEN ODDuration = 'SINGLEDAY' Then REPLACE(CONVERT(VARCHAR, TotalHours,108),' ','-') ELSE '-' END AS TotalHours, CASE WHEN ODDuration = 'SINGLEDAY' THEN 'Single Day' ELSE 'Multiple Days' END AS ODDuration,AppliedBy,B.ApprovalOwner, B.ReviewerOwner     ,  CASE ");



            Str.Append($" WHEN IsCancelled = 0 and IsApproved = 1 and ApprovedBy = '{staffId}' THEN 'Approved' ");
            Str.Append($" WHEN IsCancelled = 0 and IsReviewed = 1 and ReviewedBy = '{staffId}' THEN 'Approved' ");
            Str.Append($" WHEN IsCancelled = 0 and IsRejected = 1 and ApprovalStatusId = 3  and ApprovedBy = '{staffId}' ");
            Str.Append(" THEN 'Rejected' ");
            Str.Append($" WHEN IsCancelled = 0 and IsRejected = 1 and ReviewerstatusId = 3  and ReviewedBy = '{staffId}' ");
            Str.Append($" THEN 'Rejected' WHEN IsCancelled = 1    and CancelledBy = '{staffId}' THEN 'Cancelled' ");
            Str.Append("  END as [Status] FROM RequestApplication A join ApplicationApproval B on A.id = B.ParentId   ");

            Str.Append(" Inner Join Staff C on A.StaffId = C.Id  WHERE A.RequestApplicationType = 'WFH'    ");
            Str.Append(" and(IsApproved = 1 or IsReviewed = 1 or IsCancelled = 1 or IsRejected = 1)    ");

            Str.Append($" AND(B.ApprovedBy = '{staffId}' or B.ReviewedBy = '{staffId}') order by A.ApplicationDate desc ");




            //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
            var Obj = context.Database.SqlQuery<RAODRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", staffId)).Select(d => new RAODRequestApplication
            {
                Id = d.Id,
                StaffId = d.StaffId,
                StaffName = d.StaffName,
                AppliedBy = d.AppliedBy,
                Remarks = d.Remarks,
                //Date = d.Date,
                StartTime = d.StartTime,
                EndTime = d.EndTime,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                TotalHours = d.TotalHours,
                TotalDays = d.TotalDays,
                Type = d.ODDuration,
                ODDuration = d.ODDuration,
                ApproverStatus = d.ApproverStatus,
                ReviewerStatus = d.ReviewerStatus,
                ApprovalOwner = d.ApprovalOwner,
                ReviewerOwner = d.ReviewerOwner,
                IsCancelled = d.IsCancelled,
                IsReviewerCancelled = d.IsReviewerCancelled,
                IsApproverCancelled = d.IsApproverCancelled,
                Status = d.Status //Rajesh Sep 16- Nov 9
            }).ToList();
            return Obj;
        }

        public List<RAODRequestApplication> GetApprovedBTRequestForMyTeam(string staffId)
        {
            //Rajesh Sep18 for mobile
            StringBuilder Str = new StringBuilder();
            Str.Append(" SELECT ");
            Str.Append(" a.Id, ");
            Str.Append(" a.StaffId, ");
            Str.Append(" A.IsCancelled, ");
            Str.Append(" A.IsReviewerCancelled, ");
            Str.Append(" A.IsApproverCancelled, ");
            Str.Append("(c.FirstName+' '+c.LastName) as StaffName,");
            Str.Append(" Remarks, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS StartDate, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,EndDate,106),' ','-')) AS EndDate, ");
            Str.Append(" TotalDays,AppliedBy, ");
            Str.Append(" CASE WHEN A.ODDuration = 'SINGLEDAY' THEN replace(convert(varchar, StartDate,106),' ','-') +' ' +      convert(varchar(8), StartDate, 114) ELSE '-' END AS StartTime,   ");
            Str.Append(" CASE WHEN A.ODDuration = 'SINGLEDAY' THEN replace(convert(varchar, EndDate,106),  ' ','-') +' ' + convert(varchar(8), EndDate, 114) ELSE '-' END AS EndTime, ");
            Str.Append(" CASE WHEN ODDuration = 'SINGLEDAY' Then REPLACE(CONVERT(VARCHAR, TotalHours,108),' ','-') ELSE '-' END AS TotalHours, CASE WHEN ODDuration = 'SINGLEDAY' THEN 'Single Day' ELSE 'Multiple Days' END AS ODDuration,AppliedBy,B.ApprovalOwner, B.ReviewerOwner     ,  CASE ");



            Str.Append($" WHEN IsCancelled = 0 and IsApproved = 1 and ApprovedBy = '{staffId}' THEN 'Approved' ");
            Str.Append($" WHEN IsCancelled = 0 and IsReviewed = 1 and ReviewedBy = '{staffId}' THEN 'Approved' ");
            Str.Append($" WHEN IsCancelled = 0 and IsRejected = 1 and ApprovalStatusId = 3  and ApprovedBy = '{staffId}' ");
            Str.Append(" THEN 'Rejected' ");
            Str.Append($" WHEN IsCancelled = 0 and IsRejected = 1 and ReviewerstatusId = 3  and ReviewedBy = '{staffId}' ");
            Str.Append($" THEN 'Rejected' WHEN IsCancelled = 1    and CancelledBy = '{staffId}' THEN 'Cancelled' ");
            Str.Append("  END as [Status] FROM RequestApplication A join ApplicationApproval B on A.id = B.ParentId   ");

            Str.Append(" Inner Join Staff C on A.StaffId = C.Id  WHERE A.RequestApplicationType = 'BT'    ");
            Str.Append(" and(IsApproved = 1 or IsReviewed = 1 or IsCancelled = 1 or IsRejected = 1)    ");

            Str.Append($" AND(B.ApprovedBy = '{staffId}' or B.ReviewedBy = '{staffId}') order by A.ApplicationDate desc ");




            //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
            var Obj = context.Database.SqlQuery<RAODRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", staffId)).Select(d => new RAODRequestApplication
            {
                Id = d.Id,
                StaffId = d.StaffId,
                StaffName = d.StaffName,
                AppliedBy = d.AppliedBy,
                Remarks = d.Remarks,
                //Date = d.Date,
                StartTime = d.StartTime,
                EndTime = d.EndTime,
                StartDate=d.StartDate,
                EndDate=d.EndDate,
                TotalHours = d.TotalHours,
                TotalDays=d.TotalDays,
                Type = d.ODDuration,
                ODDuration=d.ODDuration,
                ApproverStatus = d.ApproverStatus,
                ReviewerStatus = d.ReviewerStatus,
                ApprovalOwner = d.ApprovalOwner,
                ReviewerOwner = d.ReviewerOwner,
                IsCancelled = d.IsCancelled,
                IsReviewerCancelled = d.IsReviewerCancelled,
                IsApproverCancelled = d.IsApproverCancelled,
                Status = d.Status //Rajesh Sep 16
            }).ToList();
            return Obj;
        }
    

        public List<RAODRequestApplication> GetAppliedODRequestForMyTeam(string StaffId, string AppliedBy, string Role, string ApplicationType)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                if (Role == "3" || Role == "5" || Role == "6")
                {
                    Str.Append(" SELECT ");
                    Str.Append(" a.Id, ");
                    Str.Append(" a.StaffId, ");
                    Str.Append(" A.IsCancelled, ");
                    Str.Append(" A.IsReviewerCancelled, ");
                    Str.Append(" A.IsApproverCancelled, ");
                    Str.Append("(c.FirstName+' '+c.LastName) as StaffName,");
                    Str.Append(" Remarks, ");
                    Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS StartDate, ");
                    Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,EndDate,106),' ','-')) AS EndDate, ");
                    Str.Append(" TotalDays,AppliedBy, ");
                    Str.Append(" upper(replace(convert(varchar,StartDate,106),' ','-')) + ' ' + convert(varchar(8),StartDate,114) AS StartTime,");
                    Str.Append(" upper(replace(convert(varchar,EndDate,106),' ','-')) + ' ' + convert(varchar(8),EndDate,114) AS EndTime,");
                    Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,TotalHours,108),' ','-')) AS TotalHours, ");
                    Str.Append(" ODDuration,AppliedBy,B.ApprovalOwner, B.ReviewerOwner, ");
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
                    Str.Append(" END as [ReviewerStatus],B.ParentType as ApplicationType ");
                    Str.Append("FROM RequestApplication A ");
                    Str.Append("join ApplicationApproval as B on a.id= b.parentid ");
                    Str.Append("join STAFFVIEW as c on a.Staffid = c.Staffid ");
                    Str.Append("WHERE a.RequestApplicationType = @ApplicationType AND a.StaffId=@StaffId order by a.ApplicationDate desc ");
                }
                else
                {
                    Str.Append(" SELECT ");
                    Str.Append(" a.Id, ");
                    Str.Append(" a.StaffId, ");
                    Str.Append(" A.IsCancelled, ");
                    Str.Append(" A.IsReviewerCancelled, ");
                    Str.Append(" A.IsApproverCancelled, ");
                    Str.Append("(c.FirstName+' '+c.LastName) as StaffName,");
                    Str.Append(" Remarks, ");
                    Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS StartDate, ");
                    Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,EndDate,106),' ','-')) AS EndDate, ");
                    Str.Append(" TotalDays,AppliedBy, ");
                    Str.Append(" upper(replace(convert(varchar,StartDate,106),' ','-')) + ' ' + convert(varchar(8),StartDate,114) AS StartTime,");
                    Str.Append(" upper(replace(convert(varchar,EndDate,106),' ','-')) + ' ' + convert(varchar(8),EndDate,114) AS EndTime,");
                    Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,TotalHours,108),' ','-')) AS TotalHours, ");
                    Str.Append(" ODDuration,AppliedBy,B.ApprovalOwner, B.ReviewerOwner, ");
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
                    Str.Append(" END as [ReviewerStatus],B.ParentType as ApplicationType ");
                    Str.Append("FROM RequestApplication A ");
                    Str.Append("join ApplicationApproval as B on a.id= b.parentid ");
                    Str.Append("join STAFFVIEW as c on a.Staffid = c.Staffid ");
                    Str.Append("WHERE a.RequestApplicationType = @ApplicationType AND a.StaffId=@StaffId AND (a.AppliedBy = @AppliedId or b.ApprovalOwner=@AppliedId or b.ReviewerOwner=@AppliedId) order by a.ApplicationDate desc ");
                }
                var Obj = context.Database.SqlQuery<RAODRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@AppliedId", AppliedBy), new SqlParameter("@ApplicationType", ApplicationType)).Select(d => new RAODRequestApplication
                {
                    ApplicationType = d.ApplicationType,
                    Id = d.Id,
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    AppliedBy = d.AppliedBy,
                    Remarks = d.Remarks,
                    ODDuration = d.ODDuration,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    TotalDays = d.TotalDays,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                    TotalHours = d.TotalHours,
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
                return new List<RAODRequestApplication>();
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

        public List<OnDutyDuration> GetOnDutyDurations()
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Id , Name , IsActive From OnDutyDuration Where IsActive = 1");
            try
            {
                var onDutyDurationList = context.Database.SqlQuery<OnDutyDuration>
                    (queryString.ToString()).Select(d => new OnDutyDuration()
                    {
                        Id = d.Id,
                        Name = d.Name,
                        IsActive = d.IsActive
                    }
                    ).ToList();
                if (onDutyDurationList == null)
                {
                    return new List<OnDutyDuration>();
                }
                else
                {
                    return onDutyDurationList;
                }
            }
            catch (Exception err)
            {
                return new List<OnDutyDuration>();
            }
        }
        public List<LeaveView> GetODList()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Id,Name from LeaveType where ID in ('LV0012','LV0038')");
            //qryStr.Append("SELECT CONVERT ( VARCHAR , Id ) AS Id , Name FROM LeaveType WHERE ISACTIVE = 1");

            try
            {
                var lst = context.Database.SqlQuery<LeaveView>(qryStr.ToString()).Select(d => new LeaveView()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<LeaveView>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception e)
            {
                return new List<LeaveView>();
            }
        }
        public void SaveRequestApplication(ClassesToSave DataToSave, bool isFinalLevelApproval)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            CommonRepository commonRepository = new CommonRepository();
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
                        fromDate = Convert.ToDateTime(DataToSave.RA.StartDate);
                        toDate = Convert.ToDateTime(DataToSave.RA.EndDate);
                        if (fromDate < currentDate)
                        {
                            if (toDate >= currentDate)
                            {
                                toDate = DateTime.Now.AddDays(-1);
                            }
                            commonRepository.LogIntoIntoAttendanceControlTable(DataToSave.RA.StaffId, fromDate, toDate, DataToSave.RA.RequestApplicationType, DataToSave.AA.Id);
                        }
                    }

                    context.SaveChanges();
                    Trans.Commit();
                    if (DataToSave.ESL != null)
                    {
                        foreach (var l in DataToSave.ESL)
                            if (l.To != "-")
                                commonRepository.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException err)
                {
                    Trans.Rollback();
                    if (err.Message != null)
                    {
                        var message = "\nValidation Errors: ";
                        foreach (var error in err.EntityValidationErrors.SelectMany(entity => entity.ValidationErrors))
                        {
                            message += $"\n * Field name: {error.PropertyName}, Error message: {error.ErrorMessage}";
                        }
                        throw new ApplicationException( message);
                    }
                    else
                    {
                    throw;
                    }
                }
                catch (DbUpdateException e)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                    foreach (var eve in e.Entries)
                    {
                        sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    }

                    throw new ApplicationException(sb.ToString());
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message);
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
                    context.SaveChanges();
                    Trans.Commit();
                    if (CTS.ESL != null)
                    {
                    foreach (var l in CTS.ESL)
                        if (l.To != "-")
                                using (CommonRepository commonRepository = new CommonRepository()) {
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

        public void ApproveApplication(ClassesToSave CTS, bool isFinalLevelApproval)
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
                    //context.Entry(CTS.AA).Property("ReviewerstatusId").IsModified = true;
                    //context.Entry(CTS.AA).Property("ReviewedBy").IsModified = true;
                    //context.Entry(CTS.AA).Property("ReviewedOn").IsModified = true;
                    //context.Entry(CTS.AA).Property("Comment").IsModified = true;

                    context.ApplicationApproval.AddOrUpdate(CTS.AA);

                    //Insert into attendance control table
                    fromDate = Convert.ToDateTime(CTS.RA.StartDate);
                    toDate = Convert.ToDateTime(CTS.RA.EndDate);
                    if (isFinalLevelApproval == true)
                    {
                        if (fromDate < currentDate)
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

        public void CancelApplication(ClassesToSave CTS)
        {
            CommonRepository commonRepository = new CommonRepository();
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //context.Entry(CTS.RA).Property("IsCancelled").IsModified = true;
                    //context.Entry(CTS.RA).Property("CancelledDate").IsModified = true;
                    //context.Entry(CTS.RA).Property("CancelledBy").IsModified = true;

                    context.RequestApplication.AddOrUpdate(CTS.RA);

                    if (CTS.RA.IsReviewed == true)
                    {
                        if (fromDate < currentDate)
                        {
                            if (toDate >= currentDate)
                            {
                                toDate = DateTime.Now.AddDays(-1);
                            }
                            commonRepository.LogIntoIntoAttendanceControlTable(CTS.RA.StaffId, fromDate, toDate,
                                CTS.RA.RequestApplicationType, CTS.RA.Id);
                        }
                    }

                    context.SaveChanges();
                    Trans.Commit();
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                            if (l.To != "-")
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

        public RequestApplication GetRequestApplicationDetails(string Id)
        {
            return context.RequestApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }

        public ApplicationApproval GetApplicationApproval(string ParentId, string ApplicationType)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(ParentId) && d.ParentType.Equals(ApplicationType)).FirstOrDefault();
        }

        public string ValidateBeforeSave(string StaffId, string FromDate, string ToDate, string Duration)
        {

            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@StaffId", StaffId ?? "");
            param[1] = new SqlParameter("@FromDate", Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd HH:mm:ss") ?? "");
            param[2] = new SqlParameter("@ToDate", Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd HH:mm:ss") ?? "");
            param[3] = new SqlParameter("@Duration", Duration);
            
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT DBO.fnValidateODBeforeSave ( @StaffId,@FromDate,@ToDate,@Duration)");

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
        public string ValidateApplicationOverlaping(string StaffId, string FromDate, int FromDurationiD, string ToDate, int ToDurationiD)
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@StaffId", StaffId ?? "");
            param[1] = new SqlParameter("@FromDate", Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd HH:mm:ss") ?? "");
            param[2] = new SqlParameter("@ToDate", Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd HH:mm:ss") ?? "");
            param[3] = new SqlParameter("@FromDurationiD", FromDurationiD);
            param[4] = new SqlParameter("@ToDurationiD", ToDurationiD);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Exec  [dbo].[IsApplicationOverLapping]  @StaffId,@FromDurationiD,@FromDate,@ToDate,@ToDurationiD");
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



        public List<RAODRequestApplication> GetApprovedOnDutyForMyTeam(string staffId)
        {                                       //Rajesh Sep18 for mobile
            StringBuilder Str = new StringBuilder();
            Str.Append(" SELECT ");
            Str.Append(" a.Id, ");
            Str.Append(" a.StaffId, ");
            Str.Append(" A.IsCancelled, ");
            Str.Append(" A.RequestApplicationType as ApplicationType ,");
            Str.Append(" A.IsReviewerCancelled, ");
            Str.Append(" A.IsApproverCancelled, ");
            Str.Append("(c.FirstName+' '+c.LastName) as StaffName,");
            Str.Append(" Remarks, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS StartDate, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,EndDate,106),' ','-')) AS EndDate, ");
            Str.Append(" TotalDays,AppliedBy, ");
            Str.Append(" CASE WHEN A.ODDuration = 'SINGLEDAY' THEN replace(convert(varchar, StartDate,106),' ','-') +' ' +      convert(varchar(8), StartDate, 114) ELSE '-' END AS StartTime,   ");
            Str.Append(" CASE WHEN A.ODDuration = 'SINGLEDAY' THEN replace(convert(varchar, EndDate,106),  ' ','-') +' ' + convert(varchar(8), EndDate, 114) ELSE '-' END AS EndTime, ");
            Str.Append(" CASE WHEN ODDuration = 'SINGLEDAY' Then REPLACE(CONVERT(VARCHAR, TotalHours,108),' ','-') ELSE '-' END AS TotalHours, CASE WHEN ODDuration = 'SINGLEDAY' THEN 'Single Day' ELSE 'Multiple Days' END AS ODDuration,AppliedBy,B.ApprovalOwner, B.ReviewerOwner     ,  CASE ");



            Str.Append($" WHEN IsCancelled = 0 and IsApproved = 1 and ApprovedBy = '{staffId}' THEN 'Approved' ");
            Str.Append($" WHEN IsCancelled = 0 and IsReviewed = 1 and ReviewedBy = '{staffId}' THEN 'Approved' ");
            Str.Append($" WHEN IsCancelled = 0 and IsRejected = 1 and ApprovalStatusId = 3  and ApprovedBy = '{staffId}' ");
            Str.Append(" THEN 'Rejected' ");
            Str.Append($" WHEN IsCancelled = 0 and IsRejected = 1 and ReviewerstatusId = 3  and ReviewedBy = '{staffId}' ");
            Str.Append($" THEN 'Rejected' WHEN IsCancelled = 1    and CancelledBy = '{staffId}' THEN 'Cancelled' ");
            Str.Append("  END as [Status] FROM RequestApplication A join ApplicationApproval B on A.id = B.ParentId   ");

            Str.Append(" Inner Join Staff C on A.StaffId = C.Id  WHERE A.RequestApplicationType = 'OD'    ");
            Str.Append(" and(IsApproved = 1 or IsReviewed = 1 or IsCancelled = 1 or IsRejected = 1)    ");

            Str.Append($" AND(B.ApprovedBy = '{staffId}' or B.ReviewedBy = '{staffId}') order by A.ApplicationDate desc ");




            //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
            var Obj = context.Database.SqlQuery<RAODRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", staffId)).Select(d => new RAODRequestApplication
            {
                Id = d.Id,
                ParentType=d.ApplicationType,
                StaffId = d.StaffId,
                StaffName = d.StaffName,
                AppliedBy = d.AppliedBy,
                Remarks = d.Remarks,
                ODDuration=d.ODDuration,
                //Date = d.Date,
                StartTime = d.StartTime,
                EndTime = d.EndTime,
                TotalHours = d.TotalHours,
                TotalDays=d.TotalDays,
                Type = d.Type,

                StartDate=d.StartDate,
                EndDate=d.EndDate,


                ApproverStatus = d.ApproverStatus,
                ReviewerStatus = d.ReviewerStatus,
                ApprovalOwner = d.ApprovalOwner,
                ReviewerOwner = d.ReviewerOwner,
                IsCancelled = d.IsCancelled,
                IsReviewerCancelled = d.IsReviewerCancelled,
                IsApproverCancelled = d.IsApproverCancelled,
                Status = d.Status //Rajesh Sep 16
            }).ToList();
            return Obj;
        }
    }
}
