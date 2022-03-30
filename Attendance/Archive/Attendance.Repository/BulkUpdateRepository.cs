using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Web;
using System.Data.Entity.Migrations;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

namespace Attendance.Repository
{
    public class BulkUpdateRepository : IDisposable
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

        public BulkUpdateRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<FilterList> GetMasterList(string TableName)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@TableName", TableName);
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select CONVERT ( VARCHAR , Id ) AS Id , Name from @TableName where isactive = 1 ORDER BY NAME");

            try
            {
                var Lst = context.Database.SqlQuery<FilterList>(QryStr.ToString(), sqlParameter).Select(d => new FilterList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if(Lst == null)
                {
                    return new List<FilterList>();
                }
                else
                {
                    return Lst;
                }
            }
            catch(Exception)
            {
                return new List<FilterList>();
            }
        }

        public List<FilterList> GetWorkingPatternList()
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select convert ( varchar , Id ) as Id , convert ( varchar , WorkingPattern ) as Name from WorkingDayPattern where isactive = 1");

            try
            {
                var Lst = context.Database.SqlQuery<FilterList>(QryStr.ToString()).Select(d => new FilterList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (Lst == null)
                {
                    return new List<FilterList>();
                }
                else
                {
                    return Lst;
                }
            }
            catch (Exception)
            {
                return new List<FilterList>();
            }
        }

        public void UpdateEmployees(string EmpList , string Criteria)
        {
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@EmpList", EmpList);
            sqlParameter[1] = new SqlParameter("@Criteria", Criteria);

            var QryStr = new StringBuilder();
            //
            QryStr.Clear();
            //form update statement
            QryStr.Append("Update StaffOfficial Set @Criteria where StaffId in ( @EmpList )");
            //execute update statement
            context.Database.ExecuteSqlCommand(QryStr.ToString());
        }
    }
}
