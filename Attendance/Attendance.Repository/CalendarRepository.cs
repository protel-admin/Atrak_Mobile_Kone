using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository {
    public class CalendarRepository :IDisposable
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

        public CalendarRepository()
        {
            context = new AttendanceManagementContext();
        }
        public List<CalendarDays> GetShiftCalendar(int month, int year, string StaffId)
        {
            var ctx = new AttendanceManagementContext();
            StringBuilder stringBuilder = new StringBuilder();
            SqlParameter[] sqlParameter = new SqlParameter[3];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            sqlParameter[1] = new SqlParameter("@month", month);
            sqlParameter[2] = new SqlParameter("@year", year);

            stringBuilder.Append("SELECT Replace(Convert(Varchar,ShiftInDate,106) , ' ','-') as  ActualDate ,FHStatus,SHStatus,DATENAME ( WK , ShiftInDate ) WEEKNUMBER ," +
                " DATENAME ( D , ShiftInDate ) DAY , UPPER(LEFT(DATENAME ( DW , ShiftInDate ) ,2)) [DayName] , " +
                "case when convert(date,shiftindate) = convert(date, getdate()) then convert(bit,1) else " +
                "convert(bit,0) end as IsCurrentDay,ShiftShortName AS SHORTNAME , LEFT ( CONVERT ( VARCHAR(5) ," +
                " ShiftInTime , 114 ) , 5) AS INTIME , LEFT ( CONVERT ( VARCHAR(5) , ShiftOutTime , 114 ) , 5 ) AS OUTTIME," +
                " AttendanceStatus , ISNULL(CONVERT ( VARCHAR(5) , ACTUALINTIME , 114 ),'-') AS ActualInTime ,  " +
                "ISNULL(CONVERT ( VARCHAR(5) , ACTUALOUTTIME , 114 ),'-') AS ActualOutTime,ISNULL(CONVERT ( VARCHAR(5) , " +
                "ActualWorkedHours , 114 ),'-') AS ActualWorkedHours, iif(latecoming is null , '-'," +
                " iif(CONVERT ( VARCHAR(5) , LateComing , 114 )='00:00','-',CONVERT ( VARCHAR(5) , LateComing , 114 )))" +
                " as LateComing, iif(EarlyGoing is null , '-', iif(CONVERT ( VARCHAR(5) , EarlyGoing , 114 )='00:00'," +
                "'-',CONVERT ( VARCHAR(5) , EarlyGoing , 114 ))) as EarlyGoing, " +
                " (select top 1 FHColorCode from AttendanceStatus where StatusShortName = FHStatus) as AttendanceStatusCode," +
                " (select top 1 ColorCode from AttendanceStatus where StatusShortName=FHStatus) as FHStatusCode," +
                " (select top 1 ColorCode from AttendanceStatus where StatusShortName=SHStatus) as SHStatusCode" +
                " FROM ATTENDANCEDATA WHERE STAFFID = @StaffId AND " +
                "UPPER(DATEPART(MONTH,SHIFTINDATE)) = @month AND YEAR(SHIFTINDATE) = @year");
            var lst = ctx.Database.SqlQuery<CalendarDays>(stringBuilder.ToString(), sqlParameter).ToList();
            return lst;
        }

        public List<CalendarDays> GetMyAttendanceForMobile(string StaffId)
        {
            var ctx = new AttendanceManagementContext();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" Exec [Dbo].[GetMyAttendanceForMobile] @StaffId");
            var lst = ctx.Database.SqlQuery<CalendarDays>(stringBuilder.ToString(), new SqlParameter("@StaffId",StaffId)).ToList();
            return lst;
        }
        public List<CalendarColorModel> GetCalendarColor()
        {
            List<CalendarColorModel> lst = new List<CalendarColorModel>();
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("select StatusName as AttendanceStatus,ColorCode from AttendanceStatus" +
                    " where IsActive=1 order by StatusName");
                lst = context.Database.SqlQuery<CalendarColorModel>(builder.ToString()).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return lst;
        }
        public string GetCategoryById(string StaffId)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Select CategoryId from StaffOfficial Where StaffId = @StaffId");
                var Result = context.Database.SqlQuery<string>(builder.ToString(),new SqlParameter("@StaffId", StaffId)).FirstOrDefault();    
                return Result;
            }
            catch
            {
                return "False";
            }
        }
    }
}
