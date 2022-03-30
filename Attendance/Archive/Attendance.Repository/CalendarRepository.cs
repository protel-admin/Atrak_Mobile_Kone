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
    public class CalendarRepository : IDisposable
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
        public List<CalendarDays> GetShiftCalendar(string month, int year, string StaffId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[3];
            sqlParameter[0] = new SqlParameter("@month", month);
            sqlParameter[1] = new SqlParameter("@year", year);
            sqlParameter[2] = new SqlParameter("@StaffId", StaffId);

            var ctx = new AttendanceManagementContext();
            var qryStr = new StringBuilder();

            qryStr.Append("SELECT DATENAME ( WK , ShiftInDate ) WEEKNUMBER , DATENAME ( D , ShiftInDate ) DAY , UPPER(LEFT(DATENAME ( DW , ShiftInDate ) ,2)) [DayName] , case when convert(date,shiftindate) = convert(date, getdate()) then convert(bit,1) else convert(bit,0) end as IsCurrentDay," +
                         "ShiftShortName AS SHORTNAME , LEFT ( CONVERT ( VARCHAR , ShiftInTime , 114 ) , 5 ) AS INTIME , " +
                         "LEFT ( CONVERT ( VARCHAR , ShiftOutTime , 114 ) , 5 ) AS OUTTIME , CONVERT  ( VARCHAR ,  ShiftInDate , 106 ) as ActualDate,  AttendanceStatus , LEFT ( CONVERT ( VARCHAR , ActualInTime , 114 ) , 5) AS ActualInTime , " +
                         " LEFT (CONVERT ( VARCHAR , ActualOutTime , 114 ) , 5) AS ActualOutTime , LEFT (CONVERT ( VARCHAR , ActualWorkedHours , 114 ) , 5) AS ActualWorkedHours,FHStatus,SHStatus FROM ATTENDANCEDATA WHERE STAFFID = @StaffId " +
                         "AND UPPER ( DATENAME ( M , ShiftInDate ) ) = @month AND DATENAME ( Year , ShiftInDate ) = @year");
            var lst = ctx.Database.SqlQuery<CalendarDays>(qryStr.ToString(), sqlParameter).ToList();

            if(lst.Count > 0)
                return lst;
            else
                return new List<CalendarDays>();

        }
    }
}
