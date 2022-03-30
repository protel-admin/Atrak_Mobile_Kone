using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Data.Entity.Migrations;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class DepartmentRepository : TrackChangeRepository, IDisposable
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

        public DepartmentRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<DepartmentList> GetAllDepartments()
        {

            var qryStr = new StringBuilder();
            qryStr.Append("select * from Department Order By Name Asc");

            try
            {
                var lstDept = context.Database.SqlQuery<DepartmentList>(qryStr.ToString()).Select(c => new DepartmentList()
                {
                    Id = c.Id,
                    Name = c.Name,
                    ShortName = c.ShortName,
                    Phone = c.Phone,
                    Fax = c.Fax,
                    Email = c.Email,
                    IsActive = c.IsActive,
                    CreatedOn = c.CreatedOn,
                    CreatedBy = c.CreatedBy
                }
                ).ToList();

                if (lstDept == null)
                {
                    return new List<DepartmentList>();
                }
                else
                {
                    return lstDept;
                }
            }
            catch (Exception)
            {
                return new List<DepartmentList>();
            }
        }
    }
}
