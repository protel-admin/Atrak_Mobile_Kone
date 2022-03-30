using Attendance.Model;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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

        public List<RAODRequestApplication> GetAppliedODRequest(string StaffId, string AppliedId, string userRole, string ApplicationType)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Append("select Top 30 ODApplicationId as Id,StaffId,APPLICANTNAME as StaffName,ODReason as Remarks,ODDuration," +
                    "FromDate as StartDate,ToDate as EndDate, FromTime as StartTime, ODToTime as EndTime, OD as TotalHours," +
                    " Approval1StatusName as Status1,APPROVAL2STATUSNAME as Status2, ISCANCELLED as IsCancelled, ApplicationType " +
                    "from[View_OD_BT_WFH_ApplicationHistory] where StaffId = @StaffId and ApplicationType = @ApplicationType " +
                    "Order by CONVERT(DateTime, FromDate, 101) Desc");
                var Obj = context.Database.SqlQuery<RAODRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@ApplicationType", ApplicationType)).ToList();
                return Obj;
            }
            catch (Exception)
            {
                return new List<RAODRequestApplication>();
            }
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
            catch (Exception)
            {
                return new List<RAODRequestApplication>();
            }
        }

        //public List<RAODRequestApplication> GetAppliedODRequestForMyTeam(string StaffId, string AppliedBy, string userRole, string ApplicationType)
        //{
        //    try
        //    {
        //        StringBuilder Str = new StringBuilder();
        //        if (userRole == "3" || userRole == "5")
        //        {
        //            Str.Append(" select Top 30 ODApplicationId as Id, StaffId,APPLICANTNAME as StaffName,ODReason as Remarks,ODDuration," +
        //                " FromDate as StartDate, ToDate as EndDate, FromTime as StartTime, ODToTime as EndTime,OD as TotalHours, " +
        //                "Approval1StatusName as Status1, APPROVAL2STATUSNAME as Status2,ISCANCELLED,ApplicationType, ApprovalOwner as Approval1Owner, " +
        //                "Approval2Owner as Approval2Owner from [View_OD_BT_WFH_ApplicationHistory] where ApplicationType=@ApplicationType" +
        //                " and StaffId=@StaffId Order by Convert (Date,FromDate) desc");
        //        }
        //        else
        //        {
        //            Str.Append("select Top 30 ODApplicationId as Id, StaffId,APPLICANTNAME as StaffName,ODReason as Remarks,ODDuration," +
        //                " FromDate as StartDate, ToDate as EndDate, FromTime as StartTime, ODToTime as EndTime,OD as TotalHours, " +
        //                "Approval1StatusName as Status1, APPROVAL2STATUSNAME as Status2,ISCANCELLED,ApplicationType, ApprovalOwner as Approval1Owner, " +
        //                "Approval2Owner as Approval2Owner from [View_OD_BT_WFH_ApplicationHistory] where ApplicationType=@ApplicationType " +
        //                "and StaffId=@StaffId and (AppliedBy=@AppliedBy or ApprovalOwner=@AppliedBy or " +
        //                "Approval2Owner=@AppliedBy) Order by Convert (Date,FromDate) desc");
        //        }
        //        var Obj = context.Database.SqlQuery<RAODRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId),
        //            new SqlParameter("@AppliedBy", AppliedBy), new SqlParameter("@ApplicationType", ApplicationType)).ToList();
        //        return Obj;
        //    }
        //    catch (Exception)
        //    {
        //        return new List<RAODRequestApplication>();
        //    }
        //}


        public List<RAODRequestApplication> GetApprovedWFHRequestForMyTeam(string staffId)
        {
            //Rajesh Nov 09 for mobile , Nov 29 2021 changed column names of IsReviewer and its clan
            StringBuilder Str = new StringBuilder();
            Str.Append(" SELECT ");
            Str.Append(" a.Id, ");
            Str.Append(" a.StaffId, ");
            Str.Append(" A.IsCancelled, ");
            Str.Append(" A.IsCancelled as IsReviewerCancelled, ");
            Str.Append(" A.IsCancelled as IsApproverCancelled, ");
            Str.Append("(c.FirstName+' '+c.LastName) as StaffName,");
            Str.Append(" Remarks, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS StartDate, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,EndDate,106),' ','-')) AS EndDate, ");
            Str.Append(" REPLACE(CONVERT(VARCHAR, TotalDays,108),' ','-') as TotalDays,AppliedBy, ");
            Str.Append(" CASE WHEN A.ODDuration = 'SINGLEDAY' THEN replace(convert(varchar, StartDate,106),' ','-') +' ' +      convert(varchar(8), StartDate, 114) ELSE '-' END AS StartTime,   ");
            Str.Append(" CASE WHEN A.ODDuration = 'SINGLEDAY' THEN replace(convert(varchar, EndDate,106),  ' ','-') +' ' + convert(varchar(8), EndDate, 114) ELSE '-' END AS EndTime, ");
            Str.Append(" CASE WHEN ODDuration = 'SINGLEDAY' Then REPLACE(CONVERT(VARCHAR, TotalHours,108),' ','-') ELSE '-' END AS TotalHours, CASE WHEN ODDuration = 'SINGLEDAY' THEN 'Single Day' ELSE 'Multiple Days' END AS ODDuration,AppliedBy,B.ApprovalOwner, B.Approval2Owner     ,  CASE ");



            Str.Append($" WHEN IsCancelled = 0 and IsApproved = 1 and ApprovedBy = '{staffId}' THEN 'Approved' ");
            Str.Append($" WHEN IsCancelled = 0 and IsApproved = 1 and Approval2By = '{staffId}' THEN 'Approved' ");
            Str.Append($" WHEN IsCancelled = 0 and IsRejected = 1 and ApprovalStatusId = 3  and ApprovedBy = '{staffId}' ");
            Str.Append(" THEN 'Rejected' ");
            Str.Append($" WHEN IsCancelled = 0 and IsRejected = 1 and Approval2statusId = 3  and Approval2By = '{staffId}' ");
            Str.Append($" THEN 'Rejected' WHEN IsCancelled = 1    and CancelledBy = '{staffId}' THEN 'Cancelled' ");
            Str.Append("  END as [Status] FROM RequestApplication A join ApplicationApproval B on A.id = B.ParentId   ");

            Str.Append(" Inner Join Staff C on A.StaffId = C.Id  WHERE A.RequestApplicationType = 'WFH'    ");
            Str.Append(" and(IsApproved = 1  or IsCancelled = 1 or IsRejected = 1)    ");

            Str.Append($" AND(B.ApprovedBy = '{staffId}' or B.Approval2By = '{staffId}') order by A.ApplicationDate desc ");




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



        public List<RAODRequestApplication> GetAllODList(string StaffId, string AppliedBy, string userRole, string ApplicationType)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                if (userRole == "3" || userRole == "5")
                {
                    Str.Append(" select top 30 ODApplicationId as Id,StaffId,APPLICANTNAME as StaffName,ODReason as Remarks,ODDuration," +
                        "FromDate as StartDate, ToDate as EndDate, FromTime as StartTime, ODToTime as EndTime, OD as TotalHours, " +
                        "Approval1StatusName as Status1, APPROVAL2STATUSNAME as Status2, ISCANCELLED,ApprovalOwner as Approval1Owner, " +
                        "Approval2Owner as Approval2Owner,ApplicationType from[View_OD_BT_WFH_ApplicationHistory] where StaffId=@StaffId and " +
                        "ApplicationType=@ApplicationType Order by Convert(Date,FromDate) desc");
                }
                else
                {
                    Str.Append(" select Top 30 ODApplicationId as Id,StaffId,APPLICANTNAME as StaffName,ODReason as Remarks,ODDuration, " +
                        "FromDate as StartDate, ToDate as EndDate, FromTime as StartTime, ODToTime as EndTime, OD as TotalHours, " +
                        "Approval1StatusName as Status1, APPROVAL2STATUSNAME as Status2, IsCancelled, ApprovalOwner as Approval1Owner, " +
                        "Approval2Owner as Approval2Owner, ApplicationType from [View_OD_BT_WFH_ApplicationHistory] where StaffId= @StaffId and " +
                        "(AppliedBy=@AppliedBy or ApprovalOwner=@AppliedBy or Approval2Owner=@AppliedBy) and " +
                        "ApplicationType=@ApplicationType Order by Convert(Date,FromDate) desc");
                }
                var Obj = context.Database.SqlQuery<RAODRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId),
                    new SqlParameter("@ApplicationType", ApplicationType), new SqlParameter("@AppliedBy", AppliedBy)).ToList();
                return Obj;
            }
            catch (Exception)
            {
                return new List<RAODRequestApplication>();
            }
        }



        public List<RAODRequestApplication> GetApprovedBTRequestForMyTeam(string staffId)
        {
            //Rajesh Sep18 for mobile
            StringBuilder Str = new StringBuilder();
            Str.Append(" SELECT ");
            Str.Append(" a.Id, ");
            Str.Append(" a.StaffId, ");
            Str.Append(" A.IsCancelled, ");
            Str.Append(" A.IsCancelled as IsReviewerCancelled, ");
            Str.Append(" A.IsCancelled as IsApproverCancelled, ");
            Str.Append("(c.FirstName+' '+c.LastName) as StaffName,");
            Str.Append(" Remarks, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS StartDate, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,EndDate,106),' ','-')) AS EndDate, ");
            Str.Append(" TotalDays,AppliedBy, ");
            Str.Append(" CASE WHEN A.ODDuration = 'SINGLEDAY' THEN replace(convert(varchar, StartDate,106),' ','-') +' ' +      convert(varchar(8), StartDate, 114) ELSE '-' END AS StartTime,   ");
            Str.Append(" CASE WHEN A.ODDuration = 'SINGLEDAY' THEN replace(convert(varchar, EndDate,106),  ' ','-') +' ' + convert(varchar(8), EndDate, 114) ELSE '-' END AS EndTime, ");
            Str.Append(" CASE WHEN ODDuration = 'SINGLEDAY' Then REPLACE(CONVERT(VARCHAR, TotalHours,108),' ','-') ELSE '-' END AS TotalHours, CASE WHEN ODDuration = 'SINGLEDAY' THEN 'Single Day' ELSE 'Multiple Days' END AS ODDuration,AppliedBy,B.ApprovalOwner, B.Approval2Owner     ,  CASE ");



            Str.Append($" WHEN IsCancelled = 0 and IsApproved = 1 and ApprovedBy = '{staffId}' THEN 'Approved' ");
            Str.Append($" WHEN IsCancelled = 0 and IsApproved = 1 and Approval2By = '{staffId}' THEN 'Approved' ");
            Str.Append($" WHEN IsCancelled = 0 and IsRejected = 1 and ApprovalStatusId = 3  and ApprovedBy = '{staffId}' ");
            Str.Append(" THEN 'Rejected' ");
            Str.Append($" WHEN IsCancelled = 0 and IsRejected = 1 and Approval2StatusId = 3  and Approval2By = '{staffId}' ");
            Str.Append($" THEN 'Rejected' WHEN IsCancelled = 1    and CancelledBy = '{staffId}' THEN 'Cancelled' ");
            Str.Append("  END as [Status] FROM RequestApplication A join ApplicationApproval B on A.id = B.ParentId   ");

            Str.Append(" Inner Join Staff C on A.StaffId = C.Id  WHERE A.RequestApplicationType = 'BT'    ");
            Str.Append(" and(IsApproved = 1 or  IsCancelled = 1 or IsRejected = 1)    ");

            Str.Append($" AND(B.ApprovedBy = '{staffId}' or B.Approval2By = '{staffId}') order by A.ApplicationDate desc ");




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
            catch (Exception)
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
                    //Save AttendancecontrolTable For past date application
                    if (isFinalLevelApproval == true)
                    {
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
                    // save to email send log table.
                    if (DataToSave.ESL != null)
                    {
                        foreach (var l in DataToSave.ESL)
                            if (l.To != "" && l.To != "-")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException)
                {
                    Trans.Rollback();
                    throw;
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
                    foreach (var l in CTS.ESL)
                        if (l.To != "-")
                            CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
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
            CommonRepository CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    context.Entry(CTS.RA).Property("IsApproved").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2statusId").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2By").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2On").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;

                    if (isFinalLevelApproval == true)
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
                    context.SaveChanges();
                    Trans.Commit();
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                            if (l.To != "" && l.To != "-")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                }
                catch (Exception)
                {
                    Trans.Rollback();
                    throw;
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
                    //save the changes.
                    context.SaveChanges();
                    Trans.Commit();
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                            if (l.To != "-")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                }
                catch (Exception)
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
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT DBO.fnValidateODBeforeSave ( @StaffId,'" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd HH:mm:ss") + "','" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd HH:mm:ss") + "','" + Duration + "' )");

            try
            {
                var str = (context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@StaffId", StaffId)).FirstOrDefault()).ToString();
                return str;
            }
            catch (Exception err)
            {
                return "ERROR!" + err.Message;
            }
        }
        public string ValidateApplicationOverlaping(string StaffId, string FromDate, int FromDurationiD, string ToDate, int ToDurationiD)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Exec  [dbo].[IsApplicationOverLapping]  @StaffId,@FromDurationiD,'" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd HH:mm:ss") + "','" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd HH:mm:ss") + "',@ToDurationiD");
            try
            {
                var str = (context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@StaffId", StaffId)
                    , new SqlParameter("@FromDurationiD", FromDurationiD), new SqlParameter("@ToDurationiD", ToDurationiD)).FirstOrDefault()).ToString();
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
            Str.Append(" A.IsCancelled as IsReviewerCancelled, ");
            Str.Append(" A.IsCancelled as IsApproverCancelled, ");
            Str.Append("(c.FirstName+' '+c.LastName) as StaffName,");
            Str.Append(" Remarks, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS StartDate, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,EndDate,106),' ','-')) AS EndDate, ");
            Str.Append(" REPLACE(CONVERT(VARCHAR, TotalDays,108),' ','-') as TotalDays,AppliedBy, ");
            Str.Append(" CASE WHEN A.ODDuration = 'SINGLEDAY' THEN replace(convert(varchar, StartDate,106),' ','-') +' ' +      convert(varchar(8), StartDate, 114) ELSE '-' END AS StartTime,   ");
            Str.Append(" CASE WHEN A.ODDuration = 'SINGLEDAY' THEN replace(convert(varchar, EndDate,106),  ' ','-') +' ' + convert(varchar(8), EndDate, 114) ELSE '-' END AS EndTime, ");
            Str.Append(" CASE WHEN ODDuration = 'SINGLEDAY' Then REPLACE(CONVERT(VARCHAR, TotalHours,108),' ','-') ELSE '-' END AS TotalHours, CASE WHEN ODDuration = 'SINGLEDAY' THEN 'Single Day' ELSE 'Multiple Days' END AS ODDuration,AppliedBy,B.ApprovalOwner, B.Approval2Owner     ,  CASE ");



            Str.Append($" WHEN IsCancelled = 0 and IsApproved = 1 and ApprovedBy = '{staffId}' THEN 'Approved' ");
            Str.Append($" WHEN IsCancelled = 0 and IsApproved = 1 and Approval2By = '{staffId}' THEN 'Approved' ");
            Str.Append($" WHEN IsCancelled = 0 and IsRejected = 1 and ApprovalStatusId = 3  and ApprovedBy = '{staffId}' ");
            Str.Append(" THEN 'Rejected' ");
            Str.Append($" WHEN IsCancelled = 0 and IsRejected = 1 and Approval2statusId = 3  and Approval2By = '{staffId}' ");
            Str.Append($" THEN 'Rejected' WHEN IsCancelled = 1    and CancelledBy = '{staffId}' THEN 'Cancelled' ");
            Str.Append("  END as [Status] FROM RequestApplication A join ApplicationApproval B on A.id = B.ParentId   ");

            Str.Append(" Inner Join Staff C on A.StaffId = C.Id  WHERE A.RequestApplicationType = 'OD'    ");
            Str.Append(" and(IsApproved = 1  or IsCancelled = 1 or IsRejected = 1)    ");

            Str.Append($" AND(B.ApprovedBy = '{staffId}' or B.Approval2By = '{staffId}') order by A.ApplicationDate desc ");



            try
            {
                //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
                var Obj = context.Database.SqlQuery<RAODRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", staffId)).Select(d => new RAODRequestApplication
                {
                    Id = d.Id,
                    ParentType = d.ApplicationType,
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    AppliedBy = d.AppliedBy,
                    Remarks = d.Remarks,
                    ODDuration = d.ODDuration,
                    //Date = d.Date,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                    TotalHours =  d.TotalHours  ,
                    TotalDays =  d.TotalDays ,
                    Type = d.Type,

                    StartDate = d.StartDate,
                    EndDate = d.EndDate,


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
            catch (Exception e)
            {
                throw e;
            }
        }
    }


}

