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

namespace Attendance.Repository {
    public class WeeklyOffRepository : IDisposable
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

        public WeeklyOffRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<WeeklyOffList> GetAllWeeklyOffs()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("Select Id , Name , Settings , IsActive , CreatedOn , CreatedBy from Weeklyoffs");

            try
            {
                var lst = context.Database.SqlQuery<WeeklyOffList>(qryStr.ToString()).Select(d => new WeeklyOffList()
                {
                    Id = d.Id ,
                    Name = d.Name ,
                    Settings = d.Settings ,
                    IsActive = d.IsActive ,
                    CreatedOn = d.CreatedOn ,
                    CreatedBy = d.CreatedBy
                } ).ToList();

                if( lst == null ) {
                    return new List<WeeklyOffList>();
                } else {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<WeeklyOffList>();
            }
        }

     
        public void SaveWeeklyOffInfo( WeeklyOffs wo ) {
            var lastid = string.Empty;

            using( var trans = context.Database.BeginTransaction() ) {
                try {
                    if( string.IsNullOrEmpty( wo.Id ) ) {
                        var mr = new MasterRepository();
                        lastid = mr.getmaxid( "weeklyoffs" , "id" , "WO" , "" , 10 , ref lastid );
                        wo.Id = lastid;
                    }
                    context.WeeklyOffs.AddOrUpdate(wo);
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
