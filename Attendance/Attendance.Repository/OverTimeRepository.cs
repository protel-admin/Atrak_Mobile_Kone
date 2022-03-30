using Attendance.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
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
        AttendanceManagementContext Context = null;
        public OverTimeRepository()
        {
            Context = new AttendanceManagementContext();
        }
        StringBuilder Str = new StringBuilder();
        public List<OTRequestApplicationModel> OTRequestApplication(string StaffId, string AppliedBy)
        {
            List<OTRequestApplicationModel> Obj = new List<OTRequestApplicationModel>();
            try
            {
              Str.Clear();
              Str.Append("select A.id,A.StaffId,convert(varchar, A.OTDate,106) as OTDate ,A.OTTime,convert(varchar,A.OTDuration,108) as OTDuration,A.OTReason,A.IsCancelled,convert(varchar, A.CreatedOn) as CreatedOn,A.CreatedBy,convert(varchar,A.ModifiedOn) as CancelledOn, A.ModifiedBy as CanceledBy,B.FirstName from OTApplication A inner join Staffview B on A.staffid = B.staffid where A.StaffId=@StaffId order by id desc");
              Obj = Context.Database.SqlQuery<OTRequestApplicationModel>(Str.ToString(),
              new SqlParameter("@StaffId", StaffId)).ToList();
            }
            catch
            {
                return new List<OTRequestApplicationModel>();
            }
            return Obj;
        }


        public List<OTRequestApplicationModel> GetAllOverTimeList(string StaffId)
        {
            List<OTRequestApplicationModel> Obj = new List<OTRequestApplicationModel>();
            try
            {
                Str.Clear();
                Str.Append("select A.id,A.StaffId,convert(varchar, A.OTDate,106) as OTDate ,A.OTTime,convert(varchar,A.OTDuration,108) as OTDuration,A.OTReason,A.IsCancelled,convert(varchar, A.CreatedOn) as CreatedOn,A.CreatedBy,convert(varchar,A.ModifiedOn) as CancelledOn, A.ModifiedBy as CanceledBy,B.FirstName from OTApplication A inner join Staffview B on A.staffid = B.staffid where A.StaffId=@StaffId order by id desc");
                Obj = Context.Database.SqlQuery<OTRequestApplicationModel>(Str.ToString(),
                new SqlParameter("@StaffId", StaffId)).ToList();
            }
            catch 
            {
                return new List<OTRequestApplicationModel>();
            }
            return Obj;
        }

        //public List<OTRequestApplicationModel> GetAllOverTimeList(string StaffId)
        //{
        //    List<OTRequestApplicationModel> Obj = new List<OTRequestApplicationModel>();
        //    try
        //    {
        //        Str.Clear();
        //        Str.Append("select A.id,A.StaffId,convert(varchar, A.OTDate,106) as OTDate ,A.OTTime,convert(varchar,A.OTDuration,108) as OTDuration,A.OTReason,A.IsCancelled,convert(varchar, A.CreatedOn) as CreatedOn,A.CreatedBy,convert(varchar,A.ModifiedOn) as CancelledOn, A.ModifiedBy as CanceledBy,B.FirstName from OTApplication A inner join Staffview B on A.staffid = B.staffid where A.StaffId=@StaffId order by id desc");
        //        Obj = Context.Database.SqlQuery<OTRequestApplicationModel>(Str.ToString(),
        //        new SqlParameter("@StaffId", StaffId)).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        return new List<OTRequestApplicationModel>();
        //    }
        //    return Obj;
        //}

        public string CancelApprovedApplication(string Id, string StaffId, string currentuser,DateTime OTDate)
        {
            string msg = "";
            OTRequestApplicationModel Obj = new OTRequestApplicationModel();
            
            Str.Clear();
            using (DbContextTransaction trans=Context.Database.BeginTransaction())
            {
                try
                {
                  Str.Clear();
                  Str.Append("update OTApplication set ModifiedOn=getdate(),ModifiedBy=@currentuser,IsCancelled='true' where id=@Id ");
                  Context.Database.ExecuteSqlCommand(Str.ToString(),
                  new SqlParameter("@Id", Id),
                  new SqlParameter("@currentuser", currentuser));
                  Str.Clear();
                  Str.Append("update AttendanceData set IsOTValid=0 where  staffid=@StaffId and convert(date,shiftindate)=convert(date,@OTDate) ");
                  Context.Database.ExecuteSqlCommand(Str.ToString(),
                  new SqlParameter("@OTDate", OTDate),
                  new SqlParameter("@StaffId", StaffId));
                  trans.Commit();
                  msg = "OK";
                }
                catch (Exception e)
                {
                    msg = e.Message;
                    trans.Rollback();
                }
            }
            
            return msg;
        }

    }
}
