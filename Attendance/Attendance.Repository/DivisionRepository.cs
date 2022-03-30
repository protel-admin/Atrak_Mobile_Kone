using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;
using System.Data.Entity.Migrations;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class DivisionRepository : IDisposable
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

        public DivisionRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<DivisionList> GetAllDivisions()
        {
            var qryStr = new StringBuilder();

            qryStr.Clear();
            qryStr.Append("select Id , Name , ShortName , IsActive , CreatedOn ,CreatedBy  from Division Order By Name Asc");

            try
            {
                var lst = context.Database.SqlQuery<DivisionList>(qryStr.ToString()).Select(d => new DivisionList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    IsActive = d.IsActive , 
                    CreatedOn  = d.CreatedOn , 
                    CreatedBy = d.CreatedBy
                }).ToList();

                if (lst == null)
                {
                    return new List<DivisionList>();
                }
                else
                {
                    return lst;
                }
            }
            catch(Exception)
            {
                return new List<DivisionList>();
            }
        }

        public void SaveDivisionFileInfo(Division div)
        {
            var lastid = string.Empty;
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    if (string.IsNullOrEmpty(div.Id))
                    {
                        var mr = new MasterRepository();
                        lastid = mr.getmaxid("division", "id", "DV", "", 6,ref lastid);
                        div.Id = lastid;
                        div.ModifiedOn = DateTime.Now;
                        div.ModifiedBy = "-";
                    }
                    context.Division.AddOrUpdate(div);
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
    }
}
