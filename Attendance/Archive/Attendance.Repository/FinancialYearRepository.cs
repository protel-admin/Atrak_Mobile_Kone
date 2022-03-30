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

namespace Attendance.Repository
{
    public class FinancialYearRepository : IDisposable
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
        public FinancialYearRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<FinancialYearList> GetFinancialYears()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Convert ( varchar, Id ) as Id, Name, Replace( Convert(varchar, [From], 106),' ', '-' ) as [From],Replace( Convert ( varchar, [To], 106), ' ', '-' ) as [To], case when [IsActive] = 1 then 'Yes' else 'No' end as [IsActive] from FinancialYear");

            try
            {
                var lst = context.Database.SqlQuery<FinancialYearList>(qryStr.ToString()).Select(d => new FinancialYearList()
                {
                    Id = d.Id ,
                    Name = d.Name ,
                    From = d.From ,
                    To = d.To ,
                    IsActive = d.IsActive 

                }).ToList();

                if(lst == null)
                {
                    return new List<FinancialYearList>();
                }
                else
                {
                    return lst;
                }
            }
            catch(Exception)
            {
                return new List<FinancialYearList>();
            }
            //return null;
        }

        public void SaveInformation( string id , string name , string startdate , string enddate , string isactive ) {
            FinancialYear fy = new FinancialYear();

            if (string.IsNullOrEmpty(id)== false ){
                fy.Id = Convert.ToInt16(id);}

            fy.Name = name;
            fy.From = Convert.ToDateTime(startdate);
            fy.To = Convert.ToDateTime( enddate );

            if (isactive.Trim().ToUpper() == "YES"){
                fy.IsActive = true;}
            else if (isactive.Trim().ToUpper() == "NO"){
                fy.IsActive = false;}

            context.FinancialYear.AddOrUpdate(fy);
            context.SaveChanges();
        }
    }
}
