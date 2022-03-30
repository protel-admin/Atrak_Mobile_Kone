using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class GradeRepository : IDisposable
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

        private AttendanceManagementContext context = null;

        public GradeRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<GradeList> GetAllGrades()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("Select Id , Name , ShortName , IsActive , CreatedOn , CreatedBy from Grade Order by Name asc");

            try
            {
                var lst = context.Database.SqlQuery<GradeList>( qryStr.ToString() ).Select( d => new GradeList() {
                    Id = d.Id ,
                    Name = d.Name ,
                    ShortName = d.ShortName ,
                    IsActive = d.IsActive ,
                    CreatedOn = d.CreatedOn ,
                    CreatedBy = d.CreatedBy
                } ).ToList();

                if( lst == null ) {
                    return new List<GradeList>();
                } else {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<GradeList>();
            }
        }

        public void SaveGradeFileInfo(Grade grd, string loggedInUserStaffId)
        {
            var lastid = string.Empty;
            using( var trans = context.Database.BeginTransaction() ) {
                try {
                    if( string.IsNullOrEmpty( grd.Id ) )
                    {
                        var mr = new MasterRepository();
                        lastid = mr.getmaxid("grade" , "id" , "GD" , "" , 6 ,ref lastid);
                        grd.Id = lastid;
                        grd.CreatedOn = DateTime.Now;
                        grd.CreatedBy = loggedInUserStaffId;
                        grd.ModifiedOn = null;
                        grd.ModifiedBy = "-";
                    }
                    context.Grade.AddOrUpdate( grd );
                    context.SaveChanges();
                    trans.Commit();
                } catch( Exception ) {
                    trans.Rollback();
                    throw;
                }
            }
        }
    }
}
