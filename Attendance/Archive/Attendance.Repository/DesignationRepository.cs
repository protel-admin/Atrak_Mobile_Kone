using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Data.Entity.Migrations;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository {
    public class DesignationRepository : IDisposable
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

        public DesignationRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<DesignationList> GetAllDesignations()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Id , Name , ShortName , IsActive  , CreatedOn , CreatedBy from Designation Order By Name Asc");
            try
            {
                var lst =
                    context.Database.SqlQuery<DesignationList>(qryStr.ToString()).Select(d => new DesignationList()
                    {
                        Id = d.Id ,
                        Name = d.Name ,
                        ShortName = d.ShortName ,
                        IsActive = d.IsActive , 
                        CreatedOn = d.CreatedOn ,
                        CreatedBy = d.CreatedBy
                    }).ToList();

                if (lst == null)
                {
                    return new List<DesignationList>();
                }
                else
                {
                    return lst;
                }
            } catch( Exception ) {
                return new List<DesignationList>();
            }
        }

        public void SaveDesignationFileInfo( Designation dsg  ,string loggedInUserStaffId)
        {
            var lastid = string.Empty;
            using( var trans = context.Database.BeginTransaction() ) {
                try {
                    if( string.IsNullOrEmpty( dsg.Id ) )
                    {
                        var mr = new MasterRepository();
                        lastid = mr.getmaxid( "designation" , "Id" , "DG" , "" , 6 , ref lastid );
                        dsg.Id = lastid;
                        dsg.CreatedOn = DateTime.Now;
                        dsg.CreatedBy = loggedInUserStaffId;
                        dsg.ModifiedOn = null;
                        dsg.ModifiedBy = "-";
                    }
                    context.Designation.AddOrUpdate( dsg );
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
