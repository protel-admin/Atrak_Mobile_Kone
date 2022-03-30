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
    public class RAOTApplicationRepository : IDisposable
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
        public RAOTApplicationRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<RAOTRequestApplication> GetAppliedOverTimeRequestForMyTeam(string StaffId)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Clear();
                Str.Append(" SELECT ");
                Str.Append(" A.Id, ");
                Str.Append(" A.StaffId, ");
                Str.Append(" dbo.fnGetStaffName(staffId) as FirstName, ");
                Str.Append(" A.IsCancelled, ");
                Str.Append(" B.approvalstatusid, ");
                Str.Append(" B.ReviewerstatusId, ");
                Str.Append(" B.approvalowner, ");
                Str.Append(" B.ReviewerOwner, ");
                Str.Append(" Convert(varchar,OTDate,106) AS OTDate, ");
                Str.Append(" OTTime as ApprovedOTHours, ");
                Str.Append(" Convert(varchar,OTDuration,108) AS ActualOtHours, ");

                Str.Append(" CASE ");
                Str.Append(" when B.ReviewerstatusId=1 THEN 'PENDING' ");
                Str.Append(" when B.ReviewerstatusId=2 THEN 'APPROVED' ");
                Str.Append(" when B.ReviewerstatusId=3 THEN 'REJECT'  ");
                Str.Append(" when B.ReviewerstatusId=4 THEN 'CANCELLED'  ");
                Str.Append(" End as [ReviewerStatus], ");

                Str.Append(" CASE ");
                Str.Append(" when A.iscancelled=0 and B.ApprovalStatusId=1 THEN 'PENDING' ");
                Str.Append(" when A.iscancelled=0 and B.ApprovalStatusId=2 THEN 'APPROVED'   ");
                Str.Append(" when A.iscancelled=0 and B.ApprovalStatusId=3 THEN 'REJECT' ");
                Str.Append(" when A.isCancelled=1 and B.ApprovalStatusId=2 Then 'APPROVED BUT CANCELLED' ");
                Str.Append(" when A.isCancelled=1 and B.ApprovalStatusId=1 Then 'PENDING BUT CANCELLED' ");
                Str.Append(" when A.isCancelled=1 and B.ApprovalStatusId=4 Then 'PENDING BUT CANCELLED' ");
                Str.Append(" End as [ApproverStatus] ");

                Str.Append(" from otapplication A ");
                Str.Append(" inner join ApplicationApproval B on A. Id= B.ParentId where staffid=@StaffId ");
                Str.Append(" order by a.OTDate desc ");

                //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
                var Obj = context.Database.SqlQuery<RAOTRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).Select(d => new RAOTRequestApplication
                {
                    Id = d.Id,
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    IsCancelled = d.IsCancelled,
                    OTDate = d.OTDate,
                    ApprovedOTHours =  d.ApprovedOTHours,
                    ActualOtHours   =  d.ActualOtHours,
                    ReviewerStatus  =  d.ReviewerStatus,
                    ApproverStatus  =  d.ApproverStatus,
                    ApprovalOwner   =  d.ApprovalOwner,
                    ReviewerOwner   =  d.ReviewerOwner

                }).ToList();
                return Obj;
            }
            catch (Exception e)
            {
                return new List<RAOTRequestApplication>();
            }
        }

        public List<PermissionType> GetOTTypes()
        {
            var Obj = context.PermissionType.Where(d => d.IsActive.Equals(true)).ToList();
            return Obj;
        }

        public string GetUniqueId()
        {
            return context.Database.SqlQuery<string>("select convert(varchar,getdate(),112) + replace(convert(varchar,getdate(),114),':','')").First();
        }

        public void SaveRequestApplication(ClassesToSave DataToSave)
        {
            //context.Entry(DataToSave).State = System.Data.Entity.EntityState.Added;

            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    // save to request application table.
                    context.RequestApplication.Add(DataToSave.RA);
                    // save to application approval table.
                    context.ApplicationApproval.Add(DataToSave.AA);
                    // save to email send log table.
                    if (DataToSave.RA.StaffId == DataToSave.RA.AppliedBy)
                    {
                        foreach (var l in DataToSave.ESL)
                            context.EmailSendLog.Add(l);
                    }

                    //context.Testing.Add(DataToSave.Test);

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


        public void RejectApplication(ClassesToSave CTS)
        {
            CommonRepository CR = new CommonRepository();
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
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;

                    //
                    foreach (var l in CTS.ESL)
                        //context.EmailSendLog.Add(l);
                        if (l.To != "-")
                            CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
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

        public void ApproveApplication(ClassesToSave CTS)
        {
            CommonRepository CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    //context.Entry(CTS.RA).State = System.Data.Entity.EntityState.Modified;
                    context.Entry(CTS.RA).Property("IsApproved").IsModified = true;

                    //Update the application approval table.
                    //context.Entry(CTS.AA).State = System.Data.Entity.EntityState.Modified;
                    context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;

                    //Insert into employee leave account.
                    //context.EmployeeLeaveAccount.Add(CTS.ELA);
                    //
                    foreach (var l in CTS.ESL)
                     //context.EmailSendLog.Add(l);
                     if (l.To != "-")
                     CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
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

        public void CancelApplication(ClassesToSave CTS)
        {
            CommonRepository CR = new CommonRepository();
                using (var Trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        //CTS.RA.CancelledBy = CTS.RA.StaffId;
                        context.Entry(CTS.OTA).Property("IsCancelled").IsModified = true; //IsCancelled CreatedOn   CreatedBy
                        context.Entry(CTS.OTA).Property("CreatedOn").IsModified = true;
                        context.Entry(CTS.OTA).Property("CreatedBy").IsModified = true;
                        context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                        foreach (var l in CTS.ESL)
                          //context.EmailSendLog.Add(l);
                          if (l.To != "-")
                          CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                        //set the Employee Leave Account class to add.
                        //context.EmployeeLeaveAccount.Add(CTS.ELA);

                        //save the changes.
                        context.SaveChanges();
                        Trans.Commit();
                    }
                    catch(Exception e)
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
          public OTApplication GetOTApplicationDetails(string Id)
        {
            return context.OTApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }

        public ApplicationApproval GetApplicationApproval(string ParentId)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(ParentId) && d.ParentType.Equals("OT")).FirstOrDefault();
        }
    }
}
