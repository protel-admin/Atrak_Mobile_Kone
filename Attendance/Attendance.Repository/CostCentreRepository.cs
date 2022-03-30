using Attendance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class CostCentreRepository : IDisposable
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

        public CostCentreRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<CostCentreList> GetAllCostCentre()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("Select Id,PeopleSoftCode, Name , ShortName , IsActive  , CreatedOn , CreatedBy from CostCentre Order By Name asc");

            try
            {
                var lst = context.Database.SqlQuery<CostCentreList>( qryStr.ToString() ).Select( d => new CostCentreList() {
                    Id=d.Id,
                    PeopleSoftCode = d.PeopleSoftCode ,
                    Name = d.Name ,
                    ShortName = d.ShortName ,
                    IsActive = d.IsActive , 
                    CreatedOn = d.CreatedOn ,
                    CreatedBy = d.CreatedBy
                } ).ToList();

                if( lst == null ) {
                    return new List<CostCentreList>();
                } else {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<CostCentreList>();
            }
        }

        public void SaveCostCentreListFileInfo(CostCentre cc, string loggedInUserStaffId)
        {
            var lastid = string.Empty;
            using( var trans = context.Database.BeginTransaction() ) {
                try {
                    if( string.IsNullOrEmpty( cc.Id ) )
                    {
                        var mr = new MasterRepository();
                        lastid = mr.getmaxid("costcentre" , "id" , "CC" , "" , 6 ,ref lastid);
                        cc.Id = lastid;
                        cc.CreatedOn = DateTime.Now;
                        cc.CreatedBy = loggedInUserStaffId;
                        cc.ModifiedOn = null;
                        cc.ModifiedBy = "-";
                    }
                    context.CostCentre.AddOrUpdate(cc);
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

