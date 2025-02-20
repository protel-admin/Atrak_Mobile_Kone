﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;
using System.Data.SqlClient;
using System.Data.Entity.Migrations;
using System.Data.Entity;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class RAPermissionApplicationRepository: IDisposable
    {
        AttendanceManagementContext context = null;
        
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
        public RAPermissionApplicationRepository()
        {
            context = new AttendanceManagementContext();
        }

        //public string ValidatePermissionOffApplication(string StaffId, string ToDate, string TotalHours)
        public string ValidatePermissionOffApplication(string StaffId, DateTime PermissionStartDate, string Duration, string TimeFrom, string TimeTo, DateTime TotalHours )
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@StaffId", StaffId ?? "");
            param[1] = new SqlParameter("@PermissionDate", PermissionStartDate.ToString("dd-MMM-yyyy") );
            param[2] = new SqlParameter("@Duration", Duration );
            param[3] = new SqlParameter("@TimeFrom", TimeFrom);
            param[4] = new SqlParameter("@TimeTo", TimeTo);
            param[5] = new SqlParameter("@TotalHours", TotalHours.ToString("HH:mm:ss"));

            context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            //param[4] = new SqlParameter("@TimeTo", TimeTo);
            qryStr.Append("SELECT dbo.[fnValidatePermission] (@StaffId,@PermissionDate,@Duration,@TimeFrom,@TimeTo,@TotalHours)");
            var str = (context.Database.SqlQuery<string>(qryStr.ToString(), param).FirstOrDefault()).ToString();
            return str;
        }


        public List<RAPermissionApplication> GetAppliedPermissions(string StaffId)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Append(" select Top 30 Permissionid as Id ,StaffId,FirstName as StaffName,PermissionOffReason as Remarks ," +
                    " PermissionDate as Date, FromTime as StartTime, TimeTo as EndTime, Convert(Varchar(10),TotalHours) TotalHours, " +
                    "PermissionType as Type,   Approval1StatusName as Status1, Approval2statusName as Status2,convert(varchar,IsCancelled) as " +
                    "IsCancelled from [View_PermissionApplicationHistory] where staffid = @StaffId Order by CONVERT (DateTime, PermissionDate, 101) Desc");
                var Obj = context.Database.SqlQuery<RAPermissionApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).ToList();
                return Obj;
            }
            catch
            {
                return new List<RAPermissionApplication>();
            }
        }

        public List<RAPermissionApplication> GetAppliedPermissionsForMyTeam(string StaffId, string userRole, string AppliedId)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                if (userRole == "3" || userRole == "5")
                {
                    Str.Append(" select Permissionid as Id ,StaffId,FirstName as StaffName,PermissionOffReason as Remarks , PermissionDate as Date, FromTime as StartTime, TimeTo as EndTime,Convert(Varchar(10),TotalHours) TotalHours, Name as Type,   Approval1StatusName as Status1, Approval2statusName as Status2,Approval1Owner,Approval2Owner,convert(varchar(10),IsCancelled) as IsCancelled,Approval1Owner,Approval2Owner from [vwPermissionApproval] where staffid = @StaffId and (AppliedBy=@AppliedId or Approval1Owner=@AppliedId or Approval2Owner=@AppliedId)");
                }
                else
                {
                    Str.Append(" select Permissionid as Id ,StaffId,FirstName as StaffName,PermissionOffReason as Remarks , PermissionDate as Date, " +
                        "FromTime as StartTime, TimeTo as EndTime,Convert(Varchar(10),TotalHours) TotalHours, Name as Type,   " +
                        "Approval1StatusName as Status1, Approval2statusName as Status2,Approval1Owner,Approval2Owner," +
                        "convert(varchar(10),IsCancelled) as IsCancelled,Approval1Owner,Approval2Owner from [vwPermissionApproval] where " +
                        "staffid = @StaffId");
                }

                //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
                var Obj = context.Database.SqlQuery<RAPermissionApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@AppliedId", AppliedId)).ToList();
                return Obj;
            }
            catch 
            {
                return new List<RAPermissionApplication>();
            }
        }

        public List<RAPermissionApplication> GetAllEmployeesPermissionList(string StaffId, string userRole, string AppliedId)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                if (userRole == "3" || userRole == "5")
                {
                    Str.Append(" select Top 30 Permissionid as Id ,StaffId,FirstName as StaffName,PermissionOffReason as Remarks ," +
                    " PermissionDate as Date, FromTime as StartTime, TimeTo as EndTime, Convert(Varchar(10),TotalHours) TotalHours, " +
                    "PermissionType as Type,   Approval1StatusName as Status1, Approval2statusName as Status2,convert(varchar,IsCancelled) as " +
                    "IsCancelled from [View_PermissionApplicationHistory] where staffid = @StaffId and (AppliedBy=@AppliedId or " +
                    "ApprovalOwner=@AppliedId or Approval2Owner=@AppliedId) Order by CONVERT (DateTime, PermissionDate, 101) Desc");
                }
                else
                {
                    Str.Append(" select Top 30 Permissionid as Id ,StaffId,FirstName as StaffName,PermissionOffReason as Remarks ," +
                    " PermissionDate as Date, FromTime as StartTime, TimeTo as EndTime, Convert(Varchar(10),TotalHours) TotalHours, " +
                    "PermissionType as Type,   Approval1StatusName as Status1, Approval2statusName as Status2,convert(varchar,IsCancelled) as " +
                    "IsCancelled from [View_PermissionApplicationHistory] where staffid = @StaffId");
                }
                var Obj = context.Database.SqlQuery<RAPermissionApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)
                    , new SqlParameter("@AppliedId", AppliedId)).ToList();
                return Obj;
            }
            catch 
            {
                return new List<RAPermissionApplication>();
            }
        }

        public List<PermissionType> GetPermissionTypes()
        {
            var Obj = context.PermissionType.Where(d => d.IsActive.Equals(true)).ToList();
            return Obj;
        }

        public string GetPermissionTypeById(string PermissionTypeId)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select Name from PermissionType Where Id = @PermissionTypeId");
            var Result = (context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@PermissionTypeId", PermissionTypeId)).FirstOrDefault()).ToString();
            return Result;
        }

        public string GetUniqueId()
        {
            return context.Database.SqlQuery<string>("select convert(varchar,getdate(),112) + replace(convert(varchar,getdate(),114),':','')").First();
        }

        public string ValidatePermissionOffApplication(string StaffId, string ToDate, string TotalHours, string Duration,string PermissionStartTime,string PermissionEndTime)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT dbo.[fnValidatePermission] (@StaffId,@ToDate, @Duration," +
                "@PermissionStartTime,@PermissionEndTime,@TotalHours)");
            var str = (context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@StaffId", StaffId)
                , new SqlParameter("@ToDate", ToDate), new SqlParameter("@Duration", Duration)
                , new SqlParameter("@PermissionStartTime", PermissionStartTime), new SqlParameter("@PermissionEndTime", PermissionEndTime)
                , new SqlParameter("@TotalHours", TotalHours)).FirstOrDefault()).ToString();
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
                        if (DataToSave.RA.StartDate != null && DataToSave.RA.StartDate != null)
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
                        {
                            if (l.To != "-" && l.To != "")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
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
                    {
                        if (l.To != "-")
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
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2statusId").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2By").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2On").IsModified = true;

                    if (isFinalLevelApproval == true)
                    {
                        if (CTS.RA.StartDate != null)
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
                        {
                            if (l.To != "-" && l.To != "")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
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

                    if (CTS.RA.IsApproved == true && CTS.RA.StaffId != null && CTS.RA.StartDate != null && CTS.RA.EndDate != null)
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
                        {
                            if (l.To != "-" && l.To != "")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
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
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(ParentId) && d.ParentType.Equals("PO")).FirstOrDefault();
        }
        #region Common Permission
        public string BulkSaveCommonPermissionRepository(CommonPermissionModel model, string StaffList, string CreatedBy)
        {
            string Result = string.Empty;
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    string[] StaffId = StaffList.Split(',');
                    for (int i = 0; i < StaffId.Length; i++)
                    {
                        RequestApplication reqApp = new RequestApplication();
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
                        reqApp.TotalHours = Convert.ToDateTime(model.TotalHours);
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
                        appAppr.Approval2statusId = 2;
                        appAppr.Approval2By = CreatedBy;
                        appAppr.Approval2On = reqApp.ApplicationDate;
                        appAppr.Approval2Owner = CreatedBy;
                        context.ApplicationApproval.Add(appAppr);
                        context.SaveChanges();
                    }
                    //  context.SaveChanges();
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
            Str.Append(" B.Approval2Owner, ");
            Str.Append($" CASE WHEN IsApproved = 1 and ApprovedBy = '{staffId}' THEN 'Approved' ");
            Str.Append($" WHEN IsApproved = 1 and Approval2By = '{staffId}' THEN 'Approved'  ");
            Str.Append($" WHEN IsRejected = 1 and ApprovalStatusId = 3  and ApprovedBy = '{staffId}'   ");
            Str.Append(" THEN 'Rejected'    ");
            Str.Append($" WHEN IsRejected = 1 and Approval2statusId = 3  and Approval2By = '{staffId}'");
            Str.Append($" THEN 'Rejected' WHEN IsCancelled = 1 and CancelledBy = '{staffId}' THEN 'Cancelled' ");
            Str.Append(" END as [Status] FROM RequestApplication A join ApplicationApproval B on A.id = B.ParentId  ");
            Str.Append(" Inner Join Staff C on A.StaffId = C.Id  WHERE A.RequestApplicationType = 'PO'  ");
            Str.Append(" and(IsApproved = 1 or IsCancelled = 1 or IsRejected = 1) ");
            Str.Append($" AND(B.ApprovedBy = '{staffId}' or B.Approval2By = '{staffId}') order by A.ApplicationDate desc ");

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
                    if (CTS.AA.Approval2statusId == 2)
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
                    throw e;
                }
            }
        }



    }
}
