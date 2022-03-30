using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using Attendance.Model;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class ShiftPatternRepository : IDisposable
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
        public ShiftPatternRepository()
        {
            context = new AttendanceManagementContext();
        }

        public void SaveInformation(ShiftPattern model)
        {
            context.ShiftPattern.AddOrUpdate(model);
            context.SaveChanges();
        }

        public List<ShiftPatternList> GetShiftPatternList()
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT convert ( varchar , Id ) as Id , Name as PatternName , UsedAsGeneralShift , REPLACE ( CONVERT ( VARCHAR , CreatedOn , 106 ) , ' ' , '-' ) as CreatedOn, CreatedBy  FROM SHIFTPATTERN");

            try
            {
                var lst = context.Database.SqlQuery<ShiftPatternList>(QryStr.ToString()).Select(d => new ShiftPatternList()
                {
                    Id = d.Id,
                    PatternName = d.PatternName,
                    UsedAsGeneralShift = d.UsedAsGeneralShift,
                    CreatedOn = d.CreatedOn,
                    CreatedBy = d.CreatedBy
                }).ToList();

                if (lst == null)
                {
                    return new List<ShiftPatternList>();
                }
                else
                {
                    return lst;
                }
            }
            catch
            {
                return new List<ShiftPatternList>();
            }

        }
    }
}
