using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;
using System.Data.Entity.Migrations;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class RHEntryRepository : IDisposable
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

        public RHEntryRepository()
        {
            context = new AttendanceManagementContext();
        }

        public void SaveRestrictedHolidayEntry(RestrictedHolidays data)
        {
            context.RestrictedHolidays.AddOrUpdate(data);
            context.SaveChanges();
        }

        public List<RestrictedHolidayList> GetRestrictedHolidayList()
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT convert ( varchar , A.Id ) as Id , upper ( A.Name ) as Name, REPLACE ( CONVERT ( VARCHAR , RHDate , 106 ) , ' ' ,'-' ) AS RHDate , CONVERT ( VARCHAR , RHYear ) AS RHYear , A.CompanyId , B.NAME as CompanyName FROM RESTRICTEDHOLIDAYS A INNER JOIN COMPANY B ON A.CompanyId = B.Id WHERE RHYear = YEAR(GETDATE())");

            try
            {
                var lst = context.Database.SqlQuery<RestrictedHolidayList>(QryStr.ToString()).Select(d => new RestrictedHolidayList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    RHDate = d.RHDate,
                    RHYear  = d.RHYear,
                    CompanyId = d.CompanyId,
                    CompanyName = d.CompanyName
                }).ToList();

                if(lst == null)
                {
                    return new List<RestrictedHolidayList>(); 
                }
                else
                {
                    return lst;
                }
            }
            catch
            {
                return new List<RestrictedHolidayList>();
            }
        }
    }
}
