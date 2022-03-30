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
    public class LaterOffDateRepository : IDisposable
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
        public AttendanceManagementContext context = null;

        public LaterOffDateRepository()
        {
            context = new AttendanceManagementContext();
        }

        public void SaveLaterOffDate(LaterOffDate data)
        {
            context.LaterOffDate.AddOrUpdate(data);
            context.SaveChanges();
        }

        public void DeleteLaterOffDate(string Id)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("DELETE FROM LATEROFFDATE WHERE ID = " + Id);

            try
            {
                context.Database.ExecuteSqlCommand(QryStr.ToString());
            }
            catch (Exception)
            {
                throw;
            }

        }

        public LaterOffDate GetData(string Id)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT Id , ActionDate , Validity , CompanyId , CreatedOn , CreatedBy , ModifiedOn , ModifiedBy FROM LATEROFFDATE WHERE ID = " + Id);

            try
            {
                var data = context.Database.SqlQuery<LaterOffDate>(QryStr.ToString()).Select(d => new LaterOffDate() { 
                    Id = d.Id,
                    ActionDate = d.ActionDate,
                    Validity = d.Validity,
                    CompanyId = d.CompanyId,
                    CreatedBy = d.CreatedBy,
                    CreatedOn = d.CreatedOn,
                    ModifiedBy = d.ModifiedBy,
                    ModifiedOn = d.ModifiedOn
                }).FirstOrDefault();

                return data;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public int LaterOffAlreadyApplied(string LaterOffReqDate)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT COUNT(*) AS TotalDate FROM LATEROFF WHERE CONVERT ( DATETIME , CONVERT ( VARCHAR , LATEROFFREQDATE , 106 ) ) = CONVERT ( DATETIME , '" + LaterOffReqDate + "' )");
            return context.Database.SqlQuery<int>(QryStr.ToString()).FirstOrDefault();
        }

        public int LaterOffAlreadyApplied(int Id)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT COUNT(*) AS TotalDate FROM LATEROFF WHERE CONVERT ( DATETIME , CONVERT ( VARCHAR , LATEROFFREQDATE , 106 ) ) = ( SELECT CONVERT ( DATETIME , CONVERT ( VARCHAR , ACTIONDATE , 106 ) ) FROM LATEROFFDATE WHERE ID = "+Id+") ");
            return context.Database.SqlQuery<int>(QryStr.ToString()).FirstOrDefault();
        }

        public List<CompanyList> GetCompanyList()
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT ID , NAME FROM COMPANY WHERE ISACTIVE = 1");

            try
            {
                var lst = context.Database.SqlQuery<CompanyList>(QryStr.ToString()).Select(d => new CompanyList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if(lst == null)
                {
                    return new List<CompanyList>();
                }
                else
                {
                    return lst;
                }
            }
            catch(Exception)
            {
                return new List<CompanyList>();
            }
        }

        public List<LaterOffDateList> GetLaterOffDateList()
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select convert ( varchar , a.Id ) as Id , REPLACE ( CONVERT ( VARCHAR , ActionDate , 106 ) , ' ' , '-' ) AS ActionDate, CONVERT ( VARCHAR , Validity ) AS Validity , B.ID AS CompanyId, B.NAME as CompanyName from LaterOffDate A INNER JOIN COMPANY B ON A.COMPANYID = B.ID ");

            try
            {
                var lst = context.Database.SqlQuery<LaterOffDateList>(QryStr.ToString()).Select(d => new LaterOffDateList()
                {
                    Id = d.Id,
                    ActionDate = d.ActionDate,
                    Validity = d.Validity , 
                    CompanyId = d.CompanyId,
                    CompanyName = d.CompanyName
                }).ToList();

                if (lst == null)
                {
                    return new List<LaterOffDateList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LaterOffDateList>();
            }
        }
    }
}
