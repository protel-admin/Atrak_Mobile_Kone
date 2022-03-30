using Attendance.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class OverTimeRepository : IDisposable
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
        public OverTimeRepository()
        {
            context = new AttendanceManagementContext();
        }
        StringBuilder Str = new StringBuilder();

        public List<RAOTRequestApplication> OTRequestApplication(string StaffId)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Clear();
                Str.Append(" SELECT ");
                Str.Append(" A.Id, ");
                Str.Append(" StaffId, ");
                Str.Append(" dbo.fnGetStaffName(staffId) as FirstName, ");
                Str.Append(" A.IsCancelled, ");
                Str.Append(" B.approvalstatusid, ");      
                Str.Append(" B.ReviewerstatusId, ");
                Str.Append(" B.approvalowner, ");
                Str.Append(" Convert(varchar,OTDate,106) AS OTDate, ");
                Str.Append(" OTTime as ApprovedOTHours, ");
                Str.Append(" Convert(varchar,OTDuration,108) AS ActualOtHours, ");

                Str.Append(" CASE ");
                Str.Append(" when B.ReviewerstatusId=1 THEN 'PENDING' ");
                Str.Append(" when B.ReviewerstatusId=2 THEN 'APPROVED' ");
                Str.Append(" when B.ReviewerstatusId=3 THEN 'REJECT'  ");
                Str.Append(" End as [ReviewerStatus], ");

                Str.Append(" CASE ");
                Str.Append(" when A.iscancelled=0 and B.ApprovalStatusId=1 THEN 'PENDING' ");
                Str.Append(" when A.iscancelled=0 and B.ApprovalStatusId=2 THEN 'APPROVED'   ");
                Str.Append(" when A.iscancelled=0 and B.ApprovalStatusId=3 THEN 'REJECT' ");
                Str.Append(" when A.isCancelled=1 and B.ApprovalStatusId=2 Then 'APPROVED BUT CANCELLED' ");
                Str.Append(" when A.isCancelled=1 and B.ApprovalStatusId=1 Then 'PENDING BUT CANCELLED' ");
                Str.Append(" End as [ApproverStatus] ");

                Str.Append(" from otapplication A ");
                Str.Append(" inner join ApplicationApproval B on A. Id= B.ParentId where staffid=@StaffId ");
                Str.Append(" order by a.OTDate desc ");

                //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
                var Obj = context.Database.SqlQuery<RAOTRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).Select(d => new RAOTRequestApplication
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    IsCancelled = d.IsCancelled,
                    OTDate          = d.OTDate,
                    ApprovedOTHours = d.ApprovedOTHours,
                    ActualOtHours   = d.ActualOtHours,
                    ReviewerStatus  = d.ReviewerStatus,
                    ApproverStatus  = d.ApproverStatus,
                    ApprovalOwner   = d.ApprovalOwner

                }).ToList();
                return Obj;
            }
            catch (Exception e)
            {
                return new List<RAOTRequestApplication>();
            }
        }
        public string CancelApplication(string id)
        {
            string msg = "";
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@id", id);
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    StringBuilder QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("UPDATE OTApplication SET IsCancelled = 1 WHERE Id = @id");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(),sqlParameter);
                    context.SaveChanges();
                    Trans.Commit();
                    //OTApplication TBL = new OTApplication();
                    //if (TBL.Id != "")
                    //{
                    //    TBL.Id = id;
                    //}
                    //TBL.IsCancelled = true;                 
                    //context.OTApplication.AddOrUpdate(TBL);
                    //context.SaveChanges();
                    //trans.Commit();
                    //msg = "OK";
                    //return (Vendorid).ToString();
                }
                catch (Exception e)
                {
                    msg = e.Message;
                    Trans.Rollback();

                }

            }
            return msg;
          
        }
    }
}
