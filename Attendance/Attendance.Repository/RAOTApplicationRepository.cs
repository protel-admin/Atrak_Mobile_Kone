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
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class RAOTApplicationRepository :IDisposable
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

        public List<RAOTRequestApplication> GetAppliedPermissions(string StaffId)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Append(" SELECT ");
                Str.Append(" Id, ");
                Str.Append(" StaffId, ");
                Str.Append(" Remarks, ");
                Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS OTDate, ");
                Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,ApplicationDate,108),' ','-')) AS AppliedDate, ");
                Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,TotalHours,108),' ','-')) AS TotalHours, ");
                Str.Append(" OTRange as OTDuration, ");
                Str.Append(" CASE  ");
                Str.Append(" WHEN IsCancelled = 0	and IsApproved = 0	and IsRejected = 0 THEN 'PENDING' ");
                Str.Append(" WHEN IsCancelled = 0	and IsApproved = 1	and IsRejected = 0 THEN 'APPROVED' ");
                Str.Append(" WHEN IsCancelled = 0	and IsApproved = 0	and IsRejected = 1 THEN 'REJECTED' ");
                Str.Append(" WHEN IsCancelled = 1	and IsApproved = 0	and IsRejected = 0 THEN 'CANCELLED' ");
                Str.Append(" WHEN IsCancelled = 1	and IsApproved = 1	and IsRejected = 0 THEN 'APPROVED BUT CANCELLED' ");
                Str.Append(" WHEN IsCancelled = 1	and IsApproved = 0	and IsRejected = 1 THEN 'REJECTED BUT CANCELLED' ");
                Str.Append(" END as [Status] ");
                Str.Append(" FROM RequestApplication A WHERE RequestApplicationType = 'OT' AND StaffId = @StaffId order by ApplicationDate desc ");

                //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
                var Obj = context.Database.SqlQuery<RAOTRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).Select(d => new RAOTRequestApplication
                {
                    Id = d.Id,
                    StaffId = d.StaffId,
                    Remarks = d.Remarks,
                    OTDate = d.OTDate,
                    OTDuration = d.OTDuration,
                    TotalHours = d.TotalHours,
                    AppliedDate = d.AppliedDate,
                    Status = d.Status
                }).ToList();
                return Obj;
            }
            catch 
            {
                return new List<RAOTRequestApplication>();
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
                    //context.Entry(CTS.RA).State = System.Data.Entity.EntityState.Modified;
                    context.Entry(CTS.RA).Property("IsRejected").IsModified = true;

                    //Update the application approval table.
                    //context.Entry(CTS.AA).State = System.Data.Entity.EntityState.Modified;
                    context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;

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

        public void ApproveApplication(ClassesToSave CTS)
        {
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
                    context.SaveChanges();
                    Trans.Commit();
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
            if (CTS.ELA == null)
            {
                //context.Entry(CTS.RA).State = System.Data.Entity.EntityState.Modified;
                CTS.RA.CancelledBy = CTS.RA.StaffId;
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
                        CTS.RA.CancelledBy = CTS.RA.StaffId;
                        context.Entry(CTS.RA).Property("IsCancelled").IsModified = true;
                        context.Entry(CTS.RA).Property("CancelledDate").IsModified = true;
                        context.Entry(CTS.RA).Property("CancelledBy").IsModified = true;

                        //set the Employee Leave Account class to add.
                        //context.EmployeeLeaveAccount.Add(CTS.ELA);

                        //save the changes.
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


        public RequestApplication GetRequestApplicationDetails(string Id)
        {
            return context.RequestApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }

        public ApplicationApproval GetApplicationApproval(string ParentId)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(ParentId) && d.ParentType.Equals("OT")).FirstOrDefault();
        }

        //public void Dispose()
        //{
        //    ((IDisposable)context).Dispose();
        //}
    }
}
