using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

namespace Attendance.Repository
{
    public class WorkStationRepository : IDisposable
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
         public WorkStationRepository()
        {
            context = new AttendanceManagementContext();
        }

         public List<WorkStationList> GetAllWorkStation()
         {
             try
             {
                 var lstWorkstation = context.WorkStation.Select(c => new WorkStationList()
                 {
                     Id = c.Id,
                     Name = c.Name,
                     ShortName = c.ShortName,
                     IsActive = c.IsActive,
                     CreatedOn = c.CreatedOn,
                     CreatedBy = c.CreatedBy
                 }).Where(p=>p.IsActive).ToList();

                 if (lstWorkstation == null)
                 {
                     return new List<WorkStationList>();
                 }
                 else
                 {
                     return lstWorkstation;
                 }

             }
             catch (Exception)
             {
                 return new List<WorkStationList>();
             }
         }





         public void saveWorkstationHistory(WorkstationAllocation wa)
         {
             string Mess;
             
                 var qryStr = new StringBuilder();
                 qryStr.Clear();

            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@Staffid", wa.Staffid);
            sqlParameter[1] = new SqlParameter("@TransactionDate", wa.TransactionDate.ToString("MM/dd/yyyy"));


            qryStr.Append("select count(*) from WorkstationAllocation where staffid= @Staffid  and isActive=1 and convert(date,Transactiondate)=convert(date,@TransactionDate)");
                 var lst = context.Database.SqlQuery<int>(qryStr.ToString(), sqlParameter).FirstOrDefault();
                 if (lst > 0)
                 {
                     qryStr.Clear();

                     qryStr.Append("update WorkstationAllocation set isActive=0 where staffid= @Staffid and isActive=1 and convert(date,Transactiondate)=convert(date,@TransactionDate)");
                     context.Database.ExecuteSqlCommand(qryStr.ToString(), sqlParameter);
                 }

                 context.WorkStationAllocation.AddOrUpdate(wa);
                 context.SaveChanges();
               
             
         }
    }
}
