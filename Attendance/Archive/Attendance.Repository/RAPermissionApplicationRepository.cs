using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;
using System.Data.SqlClient;
using System.Data.Entity.Migrations;
using System.Data.Entity;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class RAPermissionApplicationRepository : IDisposable
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
        public RAPermissionApplicationRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<RAPermissionApplication> GetAppliedPermissions(string StaffId)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Clear();
                Str.Append("SELECT ");
                Str.Append("a.Id, ");
                Str.Append("a.StaffId, ");
                Str.Append("A.IsCancelled,");
                Str.Append("A.IsReviewerCancelled,");
                Str.Append("A.IsApproverCancelled,");
                Str.Append("(c.FirstName+' '+c.LastName) as StaffName,");
                Str.Append("Remarks,AppliedBy, ");
                Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS Date, ");
                Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,StartDate,108),' ','-')) AS StartTime, ");
                Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,EndDate,108),' ','-')) AS EndTime, ");
                Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,TotalHours,108),' ','-')) AS TotalHours, ");
                Str.Append("( select name from permissiontype where id = A.PermissionType ) as [Type],B.ApprovalOwner, B.ReviewerOwner, ");

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

                Str.Append("WHERE a.RequestApplicationType = 'PO' AND a.StaffId = @StaffId order by A.ApplicationDate desc ");
                //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
                var Obj = context.Database.SqlQuery<RAPermissionApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).Select(d => new RAPermissionApplication
                {
                    Id = d.Id,
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    AppliedBy = d.AppliedBy,
                    Remarks = d.Remarks,
                    Date = d.Date,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                    TotalHours = d.TotalHours,
                    Type = d.Type,
                    ApproverStatus = d.ApproverStatus,
                    ReviewerStatus = d.ReviewerStatus,
                    ApprovalOwner = d.ApprovalOwner,
                    ReviewerOwner = d.ReviewerOwner,
                    IsCancelled = d.IsCancelled,
                    IsReviewerCancelled = d.IsReviewerCancelled,
                    IsApproverCancelled = d.IsApproverCancelled ,
                    Status= d.Status //Rajesh Sep 16
                }).ToList();
                return Obj;
            }
            catch (Exception e)
            {
                return new List<RAPermissionApplication>();
            }
        }

        public List<RAPermissionApplication> GetAppliedPermissionsForMyTeam(string StaffId, string AppliedBy, string Role)
        {
            try
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
                Str.Append("Remarks,AppliedBy, ");
                Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS Date, ");
                Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,StartDate,108),' ','-')) AS StartTime, ");
                Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,EndDate,108),' ','-')) AS EndTime, ");
                Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,TotalHours,108),' ','-')) AS TotalHours, ");
                Str.Append("( select name from permissiontype where id = A.PermissionType ) as [Type],B.ApprovalOwner, B.ReviewerOwner, ");

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
                Str.Append("join ApplicationApproval as B on a.id= b.parentid ");
                Str.Append("join STAFFVIEW as c on a.Staffid = c.Staffid ");
                Str.Append("WHERE a.RequestApplicationType = 'PO' AND a.StaffId=@StaffId order by a.ApplicationDate desc ");
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
                    Str.Append("Remarks,AppliedBy, ");
                    Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS Date, ");
                    Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,StartDate,108),' ','-')) AS StartTime, ");
                    Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,EndDate,108),' ','-')) AS EndTime, ");
                    Str.Append("UPPER(REPLACE(CONVERT(VARCHAR,TotalHours,108),' ','-')) AS TotalHours, ");
                    Str.Append("( select name from permissiontype where id = A.PermissionType ) as [Type],B.ApprovalOwner, B.ReviewerOwner, ");

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
                    Str.Append("join ApplicationApproval as B on a.id= b.parentid ");
                    Str.Append("join STAFFVIEW as c on a.Staffid = c.Staffid ");
                    Str.Append("WHERE a.RequestApplicationType = 'PO' AND a.StaffId=@StaffId AND (a.AppliedBy = @AppliedId or b.ApprovalOwner=@AppliedId or b.ReviewerOwner=@AppliedId) order by a.ApplicationDate desc ");
                }
                //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
                var Obj = context.Database.SqlQuery<RAPermissionApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@AppliedId", AppliedBy)).Select(d => new RAPermissionApplication
                {
                    Id = d.Id,
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    AppliedBy = d.AppliedBy,
                    Remarks = d.Remarks,
                    Date = d.Date,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                    TotalHours = d.TotalHours,
                    Type = d.Type,
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
                return new List<RAPermissionApplication>();
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

        public string ValidatePermissionOffApplication(string StaffId, string ToDate, string TotalHours)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@StaffId", StaffId ?? "");
            param[1] = new SqlParameter("@ToDate", ToDate ?? "");
            param[2] = new SqlParameter("@TotalHours", TotalHours ??"");

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT dbo.[fnValidatePermission] (@StaffId,@ToDate,@TotalHours)");
            var str = (context.Database.SqlQuery<string>(qryStr.ToString(),param).FirstOrDefault()).ToString();
            return str;
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
                            using (CommonRepository commonRepository = new CommonRepository())
                            {
                                commonRepository.LogIntoIntoAttendanceControlTable(DataToSave.RA.StaffId, fromDate, toDate, DataToSave.RA.RequestApplicationType, DataToSave.AA.Id);
                            }
                        }
                    }
                    context.SaveChanges();
                    Trans.Commit();
                    if (DataToSave.ESL != null)
                    {
                        foreach (var l in DataToSave.ESL)
                            if (l.To != "-")
                                using (CommonRepository commonRepository = new CommonRepository())
                                {
                                    commonRepository.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
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
                    //context.Entry(CTS.AA).Property("ReviewerstatusId").IsModified = true;
                    //context.Entry(CTS.AA).Property("ReviewedBy").IsModified = true;
                    //context.Entry(CTS.AA).Property("ReviewedOn").IsModified = true;
                    //context.Entry(CTS.AA).Property("Comment").IsModified = true;

                    context.ApplicationApproval.AddOrUpdate(CTS.AA);

                    //Insert into attendance control table
                    if (CTS.AA.ReviewerstatusId == 2)
                    {
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
                                commonRepository.LogIntoIntoAttendanceControlTable(CTS.RA.StaffId, fromDate, toDate, CTS.RA.RequestApplicationType, CTS.AA.Id);
                    }
                        }
                    }

                    context.SaveChanges();
                    Trans.Commit();
                    foreach (var l in CTS.ESL)
                    {
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
                    throw;
                }
            }
        }

        public void CancelApplication(ClassesToSave CTS)
        {
            
                using (var Trans = context.Database.BeginTransaction())
                {
                    try
                    {
                //    CTS.RA.CancelledBy = CTS.RA.CancelledBy;
                //    context.Entry(CTS.RA).Property("IsCancelled").IsModified = true;
                //    context.Entry(CTS.RA).Property("CancelledDate").IsModified = true;
                //    context.Entry(CTS.RA).Property("CancelledBy").IsModified = true;

                //    context.Entry(CTS.RA).Property("IsReviewerCancelled").IsModified = true;
                //    context.Entry(CTS.RA).Property("ReviewerCancelledDate").IsModified = true;
                //    context.Entry(CTS.RA).Property("ReviewerCancelledBy").IsModified = true;

                //    context.Entry(CTS.RA).Property("IsApproverCancelled").IsModified = true;
                //    context.Entry(CTS.RA).Property("ApproverCancelledDate").IsModified = true;
                //    context.Entry(CTS.RA).Property("ApproverCancelledBy").IsModified = true;

                context.RequestApplication.AddOrUpdate(CTS.RA);

                        //save the changes.
                        context.SaveChanges();
                        Trans.Commit();
                    if (CTS.ESL == null)
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
        

        public RequestApplication GetRequestApplicationDetails(string Id)
        {
            return context.RequestApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }

        public ApplicationApproval GetApplicationApproval(string ParentId)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(ParentId) && d.ParentType.Equals("PO")).FirstOrDefault();
        }
        #region Common Permission
        public string BulkSaveCommonPermissionRepository(CommonPermissionModel model, string StaffList, string CreatedBy)
        {
            string Result = string.Empty;
            using (DbContextTransaction trans = context.Database.BeginTransaction())
            {
                try
                {
                    string[] StaffId = StaffList.Split(',');
                    for (int i = 0; i < StaffId.Length; i++)
                    {
                        RequestApplication reqApp = new RequestApplication();
                        //Changes made by Aarthi on 29/02/2020
                        Int64 tempId = 0;
                        //Thread.Sleep(2);
                        reqApp.Id = GetUniqueId();
                        tempId = Convert.ToInt64(reqApp.Id) + i;
                        if (tempId == 0)
                        {
                            reqApp.Id = GetUniqueId();
                            tempId = Convert.ToInt64(reqApp.Id) + i;
                        }

                        reqApp.Id = tempId.ToString();
                        reqApp.StaffId = StaffId[i];
                        reqApp.LeaveStartDurationId = 1;
                        reqApp.LeaveEndDurationId = 1;
                        reqApp.StartDate = Convert.ToDateTime(model.PermissionDate + " " + model.StartTime);
                        reqApp.EndDate = Convert.ToDateTime(model.PermissionDate + " " + model.EndTime);
                        reqApp.PermissionType = model.PermissionTypeId;
                        reqApp.RHId = 0;
                        reqApp.TotalHours = Convert.ToDateTime(model.PermissionDate + " " + model.TotalHours);
                        reqApp.Remarks = model.Remarks;
                        reqApp.ReasonId = 0;
                        reqApp.ApplicationDate = DateTime.Now;
                        reqApp.AppliedBy = CreatedBy;
                        reqApp.IsCancelled = false;
                        reqApp.IsApproved = true;
                        reqApp.IsRejected = false;
                        reqApp.RequestApplicationType = "CP";
                        reqApp.IsCancelApprovalRequired = false;
                        reqApp.IsCancelApproved = false;
                        reqApp.IsCancelRejected = false;
                        reqApp.IsReviewed = true;
                        reqApp.IsReviewerCancelled = false;
                        reqApp.IsApproverCancelled = false;
                        context.RequestApplication.Add(reqApp);
                        context.SaveChanges();

                        ApplicationApproval appAppr = new ApplicationApproval();
                        Int64 tempId1 = 0;
                        appAppr.Id = GetUniqueId();
                        tempId1 = Convert.ToInt64(reqApp.Id) + i;
                        if (tempId == 0)
                        {
                            reqApp.Id = GetUniqueId();
                            tempId1 = Convert.ToInt64(reqApp.Id) + i;
                        }
                        appAppr.Id = Convert.ToString(tempId1);
                        appAppr.ParentId = reqApp.Id;
                        appAppr.ApprovalStatusId = 2;
                        appAppr.ApprovedBy = CreatedBy;
                        appAppr.ApprovedOn = reqApp.ApplicationDate;
                        appAppr.Comment = "AUTO APPROVED THE COMMON PERMISSION";
                        appAppr.ApprovalOwner = CreatedBy;
                        appAppr.ParentType = "CP";
                        appAppr.ForwardCounter = 1;
                        appAppr.ApplicationDate = reqApp.ApplicationDate;
                        appAppr.ReviewerstatusId = 2;
                        appAppr.ReviewedBy = CreatedBy;
                        appAppr.ReviewedOn = reqApp.ApplicationDate;
                        appAppr.ReviewerOwner = CreatedBy;
                        context.ApplicationApproval.Add(appAppr);
                        context.SaveChanges();
                    }

                    trans.Commit();
                    Result = "OK";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }
            return Result;
        }
        #endregion




        public List<RAPermissionApplication> GetApprovedPermissionsForMyTeam(string staffId)
        {      //Rajesh Sep18
            StringBuilder Str = new StringBuilder();
            Str.Append("SELECT ");
            Str.Append(" A.Id, ");
            Str.Append(" A.StaffId, "); //Rajesh Aug 20
            Str.Append(" C.FirstName as StaffName , "); //Rajesh Aug 20
            Str.Append(" Remarks, ");
            Str.Append("  Remarks,AppliedBy, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR, StartDate, 106), ' ', '-')) AS Date,  ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR, StartDate, 108), ' ', '-')) AS StartTime, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR, EndDate, 108), ' ', '-')) AS EndTime, ");
            Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR, TotalHours, 108), ' ', '-')) AS TotalHours,    ");
            Str.Append(" ( select name from permissiontype where id = A.PermissionType ) as [Type],B.ApprovalOwner,  ");
            Str.Append(" B.ReviewerOwner, ");
            Str.Append($" CASE WHEN IsApproved = 1 and ApprovedBy = '{staffId}' THEN 'Approved' ");
            Str.Append($" WHEN IsReviewed = 1 and ReviewedBy = '{staffId}' THEN 'Approved'  ");
            Str.Append($" WHEN IsRejected = 1 and ApprovalStatusId = 3  and ApprovedBy = '{staffId}'   ");
            Str.Append(" THEN 'Rejected'    ");
            Str.Append($" WHEN IsRejected = 1 and ReviewerstatusId = 3  and ReviewedBy = '{staffId}'");
            Str.Append($" THEN 'Rejected' WHEN IsCancelled = 1 and CancelledBy = '{staffId}' THEN 'Cancelled' ");
            Str.Append(" END as [Status] FROM RequestApplication A join ApplicationApproval B on A.id = B.ParentId  ");
            Str.Append(" Inner Join Staff C on A.StaffId = C.Id  WHERE A.RequestApplicationType = 'PO'  ");
            Str.Append(" and(IsApproved = 1 or IsReviewed = 1 or IsCancelled = 1 or IsRejected = 1) ");
            Str.Append($" AND(B.ApprovedBy = '{staffId}' or B.ReviewedBy = '{staffId}') order by A.ApplicationDate desc ");

            //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
            var Obj = context.Database.SqlQuery<RAPermissionApplication>(Str.ToString(), new SqlParameter("@StaffId", staffId)).Select(d => new RAPermissionApplication
            {
                Id = d.Id,
                StaffId = d.StaffId,
                StaffName = d.StaffName,
                AppliedBy = d.AppliedBy,
                Remarks = d.Remarks,
                Date = d.Date,
                StartTime = d.StartTime,
                EndTime = d.EndTime,
                TotalHours = d.TotalHours,
                Type = d.Type,
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
