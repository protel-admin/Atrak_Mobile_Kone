using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

namespace Attendance.Repository
{
    public class AttendanceProcessingRepository : IDisposable
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

         public AttendanceProcessingRepository()
        {
            context = new AttendanceManagementContext();
        }

         //public string GenerateAttendance(string StaffId, DateTime? FromDate, DateTime? ToDate)
         //{
         //    var qryStr = new StringBuilder();
         //    qryStr.Clear();
         //    qryStr.Append("exec [GenerateAttendance] '" + StaffId + "','" + FromDate + "','" + ToDate + "'");
         //    var str = (context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault()).ToString();
         //    return str;
         //}

        public List<ShiftList1> GetDurationList()
        {

            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("select Id , Name , isactive from Shifts where isactive = 1");

            try
            {
                var lst = context.Database.SqlQuery<ShiftList1>(qryStr.ToString()).Select(d => new ShiftList1()
                {
                    Id = d.Id,
                    Name = d.Name,
                    isactive = d.isactive
                }).ToList();

                if (lst == null)
                {
                    return new List<ShiftList1>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ShiftList1>();
            }
        }

        public void ProcessAttendance(string StaffId, string FromDate, string ToDate)
        {

            SqlParameter[] sqlParameter = new SqlParameter[3];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            sqlParameter[1] = new SqlParameter("@FromDate", FromDate);
            sqlParameter[2] = new SqlParameter("@ToDate", ToDate);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append(" exec [AttendanceProcessingV1A] @StaffId,@FromDate,@ToDate ");
            var Result = context.Database.ExecuteSqlCommand(qryStr.ToString(), sqlParameter);
            //////////try
            //////////{
            //////////    var Result = context.Database.ExecuteSqlCommand(qryStr.ToString());// SqlQuery<string>(qryStr.ToString()).FirstOrDefault().ToString();

            //////////    return string.Empty;
            //////////}
            //////////catch (Exception e)
            //////////{
            //////////    throw e;
            //////////}
        }

        public string ProcessBacklogAttendance()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append(" exec [BackLogAttendanceProcessing]  ");
            try
            {
                var Result = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault().ToString();

                return Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
