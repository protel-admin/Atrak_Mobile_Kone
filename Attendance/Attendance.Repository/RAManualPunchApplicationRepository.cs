﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;
using System.Data.SqlClient;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using AttendanceManagement;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class RAManualPunchApplicationRepository:IDisposable
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
        public RAManualPunchApplicationRepository()
        {
            context = new AttendanceManagementContext();
        }

        public string ValidateManualPunch(string staffId, DateTime time1, DateTime time2)
        {
            SqlParameter[] sqlParameter = new SqlParameter[3];
            sqlParameter[0] = new SqlParameter("@staffId", staffId ?? "");
            sqlParameter[1] = new SqlParameter("@time1", time1);
            sqlParameter[2] = new SqlParameter("@time2", time2);

            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append(" Select [dbo].[fnValidateManualPunch](@staffId,@time1,@time2)");
            var msg = (context.Database.SqlQuery<string>(queryString.ToString(), sqlParameter).FirstOrDefault()).ToString();
            return msg;
        }


        public string GetManualPunchApproverRepository()
        {
            return context.Settings.Where(condition => condition.Parameter == "MANUAL PUNCH APPROVER").Select(select => select.Value).FirstOrDefault();
        }
        public List<RAManualPunchApplication> GetAppliedManualPunches(string StaffId)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Append("select Top 30 ManualPunchId as Id, StaffId, Name as StaffName, ManualPunchReason as Remarks, PunchType AS[Type]," +
                    " InDateTime as StartDateTime, OutDateTime as EndDateTime, Approval1StatusName as Status1, Approval2statusName as " +
                    "Status2,IsCancelled from [View_ManualPunchApplicationHistory] where staffid = @StaffId Order by " +
                    "CONVERT (DateTime, InDateTime, 101) Desc ");
                var Obj = context.Database.SqlQuery<RAManualPunchApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).ToList();
                return Obj;
            }
            catch 
            {
                return new List<RAManualPunchApplication>();
            }
        }

        public List<RAManualPunchApplication> GetAppliedManualPunchesForMyTeam(string StaffId, string AppliedId, string userRole)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                if (userRole == "3" || userRole == "5")
                {
                    Str.Append(" select Top 30 ManualPunchId as Id ,StaffId,FirstName as StaffName,ManualPunchReason as Remarks,PunchType AS Type," +
                        "InTime as StartDateTime,OutTime as EndDateTime,Approval1StatusName as Status1, " +
                        "Approval2statusName as Status2,Convert(varchar(10),IsCancelled) as IsCancelled,Approval1Owner,Approval2Owner " +
                        "from [vwManualPunchApproval] where staffid=@StaffId   " +
                        " Order by CONVERT (DateTime, InTime, 101) Desc");
                }
                else
                {
                    Str.Append(" select Top 30 ManualPunchId as Id ,StaffId,FirstName as StaffName,ManualPunchReason as Remarks,PunchType AS Type," +
                        "InTime as StartDateTime,OutTime as EndDateTime,Approval1StatusName as Status1, Approval2statusName as Status2," +
                        "Convert(varchar(10),IsCancelled) as IsCancelled,Approval1Owner,Approval2Owner from [vwManualPunchApproval] where " +
                        "staffid=@StaffId  Order by CONVERT (DateTime, InTime, 101) Desc");
                }
                var Obj = context.Database.SqlQuery<RAManualPunchApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId),
                    new SqlParameter("@AppliedId", AppliedId)).ToList();
                return Obj;
            }
            catch 
            {
                return new List<RAManualPunchApplication>();
            }
        }

        public List<RAManualPunchApplication> GetAllManualPunchList(string StaffId, string AppliedId, string userRole)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                if (userRole == "3" || userRole == "5")
                {
                    Str.Append(" select Top 30 ManualPunchId as Id ,StaffId,FirstName as StaffName,ManualPunchReason as Remarks,PunchType AS Type," +
                        "InTime as StartDateTime,OutTime as EndDateTime,Approval1StatusName as Status1, " +
                        "Approval2statusName as Status2,Convert(varchar(10),IsCancelled) as IsCancelled,Approval1Owner,Approval2Owner " +
                        "from [vwManualPunchApproval] where staffid=@StaffId Order by CONVERT (DateTime, InTime, 101) Desc");
                }
                else
                {
                    Str.Append(" select Top 30 ManualPunchId as Id ,StaffId,FirstName as StaffName,ManualPunchReason as Remarks,PunchType AS Type," +
                        "InTime as StartDateTime,OutTime as EndDateTime,Approval1StatusName as Status1, " +
                        "Approval2statusName as Status2,Convert(varchar(10),IsCancelled) as IsCancelled,Approval1Owner,Approval2Owner " +
                        "from [vwManualPunchApproval] where staffid=@StaffId  Order by CONVERT (DateTime, InTime, 101) Desc");
                }
                var Obj = context.Database.SqlQuery<RAManualPunchApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@AppliedId", AppliedId)).Select(d => new RAManualPunchApplication
                {
                    Id = d.Id,
                    IsCancelled = d.IsCancelled,
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    AppliedBy = d.AppliedBy,
                    Remarks = d.Remarks,
                    StartDateTime = d.StartDateTime,
                    EndDateTime = d.EndDateTime,
                    Type = d.Type,
                    Status1 = d.Status1,
                    Status2 = d.Status2
                }).ToList();
                return Obj;
            }
            catch 
            {
                return new List<RAManualPunchApplication>();
            }
        }


public List<RAManualPunchApplication> GetApprovedManualPunchesForMyTeam(string staffId)
        {              //Rajesh Sep 18
            try
            {
                StringBuilder Str = new StringBuilder();

                Str.Append("SELECT ");
                Str.Append(" a.Id, ");
                Str.Append(" a.StaffId, ");
                Str.Append(" A.IsCancelled,");
                Str.Append(" A.IsReviewerCancelled,");
                Str.Append(" A.IsApproverCancelled,");
                Str.Append(" (c.FirstName+' '+c.LastName) as StaffName,");
                Str.Append(" Remarks, ");
                Str.Append(" upper(replace(convert(varchar,StartDate,106),' ','-')) + ' ' + convert(varchar(8),StartDate,114) AS StartDateTime,");
                Str.Append(" upper(replace(convert(varchar,EndDate,106),' ','-')) + ' ' + convert(varchar(8),EndDate,114) AS EndDateTime,");
                Str.Append(" PunchType as [Type],AppliedBy,B.ApprovalOwner, B.ReviewerOwner, ");
                Str.Append($" CASE WHEN IsCancelled = 0 and IsApproved = 1 and ApprovedBy = '{staffId}' THEN 'Approved'       ");
                Str.Append($" WHEN IsCancelled = 0 and IsReviewed = 1 and ReviewedBy = '{staffId}' THEN 'Approved'     ");
                Str.Append($" WHEN IsCancelled = 0 and IsRejected = 1 and ApprovalStatusId = 3  and ApprovedBy = '{staffId}'          ");
                Str.Append(" THEN 'Rejected' ");
                Str.Append($" WHEN IsCancelled = 0 and IsRejected = 1 and ReviewerstatusId = 3  and ReviewedBy = '{staffId}' ");
                Str.Append($" THEN 'Rejected' WHEN IsCancelled = 1    and CancelledBy = '{staffId}' THEN 'Cancelled'    ");
                Str.Append(" END as [Status] FROM RequestApplication A join ApplicationApproval B on A.id = B.ParentId   ");
                Str.Append(" Inner Join Staff C on A.StaffId = C.Id  WHERE A.RequestApplicationType = 'MP'      ");
                Str.Append(" and(IsApproved = 1 or IsReviewed = 1 or IsCancelled = 1 or IsRejected = 1)                ");
                Str.Append($" AND(B.ApprovedBy = '{staffId}' or B.ReviewedBy = '{staffId}') order by A.ApplicationDate desc             ");

                //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
                var Obj = context.Database.SqlQuery<RAManualPunchApplication>(Str.ToString(), new SqlParameter("@StaffId", staffId), new SqlParameter("@AppliedId", "")).Select(d => new RAManualPunchApplication
                {
                    Id = d.Id,
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    AppliedBy = d.AppliedBy,
                    Remarks = d.Remarks,
                    StartDateTime = d.StartDateTime,
                    EndDateTime = d.EndDateTime,
                    Type = d.Type,
                    IsCancelled = d.IsCancelled,
                    ReviewerStatus = d.ReviewerStatus,
                    ApproverStatus = d.ApproverStatus,
                    ApprovalOwner = d.ApprovalOwner,
                    ReviewerOwner = d.ReviewerOwner,
                    IsReviewerCancelled = d.IsReviewerCancelled,
                    IsApproverCancelled = d.IsApproverCancelled,
                    Status=d.Status
                }).ToList();
                return Obj;
            }
            catch
            {
                return new List<RAManualPunchApplication>();
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
        public string ValidateExistanceManualPunch(string StaffId, string DatetoBeChecked)
        {
            var Manual = string.Empty;
            var Manual1 = string.Empty;
            var qryStr = new StringBuilder();
            var qryStr1 = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select Top 1 Tr_chId From SmaxTransaction where Tr_ChId = @StaffId And " +
                "Convert(DateTime, Replace(Convert(Varchar, Tr_Date, 106), ' ', '-') + ' ' + Convert(Varchar(8), TR_Time, 114))  = " +
                "Convert(DateTime, @DatetoBeChecked)");
            Manual = context.Database.SqlQuery<string>(qryStr.ToString(),new SqlParameter("@StaffId", StaffId)
                , new SqlParameter("@DatetoBeChecked", DatetoBeChecked)).FirstOrDefault();
            if (string.IsNullOrEmpty(Manual).Equals(true))
            {
                qryStr1.Append(" Select Top 1 StaffId From RequestApplication  where StaffId = @StaffId And  Convert(DateTime, StartDate) =" +
                   "Convert(DateTime, @DatetoBeChecked) And RequestApplicationType = 'MP' And IsCancelled = 0 And IsRejected = 0");
                Manual1 = context.Database.SqlQuery<string>(qryStr1.ToString(), new SqlParameter("@StaffId", StaffId)
                , new SqlParameter("@DatetoBeChecked", DatetoBeChecked)).FirstOrDefault();
                return Manual1;
            }
            return Manual;
        }
        public void SaveRequestApplication(ClassesToSave DataToSave, string SecurityGroupId, bool isFinalLevelApproval, string LocationId)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            var CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    if (isFinalLevelApproval == true)
                    {
                        string Indatetime;
                        string outdatetime;
                        Indatetime = DateTime.ParseExact(DataToSave.RA.StartDate.ToString(), "dd/MM/yyyy HH:mm:ss", null).ToString("dd/MMM/yyyy HH:mm:ss");
                        outdatetime = DateTime.ParseExact(DataToSave.RA.EndDate.ToString(), "dd/MM/yyyy HH:mm:ss", null).ToString("dd/MMM/yyyy HH:mm:ss");
                        if (DataToSave.RA.PunchType.ToUpper() == "IN")
                        {
                            CR.SaveInPunch(Indatetime, DataToSave.RA.StaffId, LocationId);
                        }
                        else if (DataToSave.RA.PunchType.ToUpper() == "OUT")
                        {
                            CR.SaveOutPunch(outdatetime, DataToSave.RA.StaffId, LocationId);
                        }
                        else
                        {
                            CR.SaveInOutPunch(Indatetime, outdatetime, DataToSave.RA.StaffId,LocationId);
                        }
                        //Insert into attendance control table
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
                    // save to request application table.
                    context.RequestApplication.Add(DataToSave.RA);
                    // save to application approval table.
                    context.ApplicationApproval.Add(DataToSave.AA);
                    context.SaveChanges();
                    Trans.Commit();
                    if (DataToSave.ESL != null)
                    {
                        foreach (var l in DataToSave.ESL)
                            if (l.To != "" && l.To != "-")
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

        public void RejectApplication(ClassesToSave CTS,string LocationId)
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

        public void ApproveApplication(ClassesToSave CTS, string ReportingManagerId, bool isFinalLevelApproval, string LocationId)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            var CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    string Indatetime;
                    string outdatetime;

                    if (isFinalLevelApproval == true)
                    {
                        Indatetime = DateTime.ParseExact(CTS.RA.StartDate.ToString(), "dd-MM-yyyy HH:mm:ss", null).ToString("dd/MMM/yyyy HH:mm:ss");
                        outdatetime = DateTime.ParseExact(CTS.RA.EndDate.ToString(), "dd-MM-yyyy HH:mm:ss", null).ToString("dd/MMM/yyyy HH:mm:ss");
                        if (CTS.RA.PunchType.ToUpper() == "IN")
                        {
                            CR.SaveInPunch(Indatetime, CTS.RA.StaffId,LocationId);
                        }
                        else if (CTS.RA.PunchType.ToUpper() == "OUT")
                        {
                            CR.SaveOutPunch(outdatetime, CTS.RA.StaffId, LocationId);
                        }
                        else
                        {
                            CR.SaveInOutPunch(Indatetime, outdatetime, CTS.RA.StaffId, LocationId);
                        }
                        //Update the request application table.
                        context.Entry(CTS.RA).Property("IsApproved").IsModified = true;
                        context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                        context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                        context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                        context.Entry(CTS.AA).Property("Approval2statusId").IsModified = true;
                        context.Entry(CTS.AA).Property("Approval2By").IsModified = true;
                        context.Entry(CTS.AA).Property("Approval2On").IsModified = true;
                        context.Entry(CTS.AA).Property("Comment").IsModified = true;
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
                    //CTS.RA.CancelledBy = CTS.RA.StaffId;
                    context.Entry(CTS.RA).Property("IsCancelled").IsModified = true;
                    context.Entry(CTS.RA).Property("CancelledDate").IsModified = true;
                    context.Entry(CTS.RA).Property("CancelledBy").IsModified = true;
                    if (CTS.RA.StaffId != null && CTS.RA.StartDate != null && CTS.RA.EndDate != null && CTS.RA.IsApproved == true)
                    {
                        RemoveManualPunch(CTS);
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
                            if (l.To != "-")
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

        public void RemoveManualPunch(ClassesToSave CTS)
        {
            ApplicationEntryRepository cmr = new ApplicationEntryRepository();
            StringBuilder QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select staffid  from RequestApplication where id = @CTS.RA.Id");
            var rtnstaffid = context.Database.SqlQuery<string>(QryStr.ToString(),new SqlParameter("@CTS.RA.Id", CTS.RA.Id)).FirstOrDefault();
            cmr.RemovePunchesFromSmax(CTS.RA.Id, rtnstaffid);
        }

        public RequestApplication GetRequestApplicationDetails(string Id)
        {
            return context.RequestApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }

        public ApplicationApproval GetApplicationApproval(string ParentId)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(ParentId) && d.ParentType.Equals("MP")).FirstOrDefault();
        }


        public void SaveRequestApplication(ClassesToSave DataToSave, string LocationId, bool IsFinalLevelApproval)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;

            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    // save to request application table.
                    context.RequestApplication.Add(DataToSave.RA);
                    // save to application approval table.
                    context.ApplicationApproval.Add(DataToSave.AA);
                    if (IsFinalLevelApproval == true)
                    {
                        string Indatetime;
                        string outdatetime;
                        Indatetime = DateTime.ParseExact(DataToSave.RA.StartDate.ToString(), "dd/MM/yyyy HH:mm:ss", null).ToString("dd/MMM/yyyy HH:mm:ss");
                        outdatetime = DateTime.ParseExact(DataToSave.RA.EndDate.ToString(), "dd/MM/yyyy HH:mm:ss", null).ToString("dd/MMM/yyyy HH:mm:ss");
                        if (DataToSave.RA.PunchType == "In")
                        {
                            using (CommonRepository commonRepository = new CommonRepository())
                            {
                                commonRepository.SaveInPunch(Indatetime, DataToSave.RA.StaffId, LocationId);
                            }
                        }
                        else if (DataToSave.RA.PunchType == "Out")
                        {
                            using (CommonRepository commonRepository = new CommonRepository())
                            {
                                commonRepository.SaveOutPunch(outdatetime, DataToSave.RA.StaffId, LocationId);
                            }
                        }
                        else
                        {
                            using (CommonRepository commonRepository = new CommonRepository())
                            {
                                commonRepository.SaveInOutPunch(Indatetime, outdatetime, DataToSave.RA.StaffId, LocationId);
                            }
                        }
                        //Insert into attendance control table
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
                        {
                            if (l.To != "-")
                            {
                                using (CommonRepository commonRepository = new CommonRepository())
                                {
                                    commonRepository.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                                }
                            }
                        }
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException err)
                {
                    Trans.Rollback();
                    if (err != null)
                    {
                        throw new Exception(err.Message);
                    }
                    throw;
                }
            }
        }

    }
}
