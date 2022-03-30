using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class EmployeeListRepository : IDisposable
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

        public EmployeeListRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<EmployeeList> LoadListOfEmployees()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT StaffId ,FirstName ,DeptName ,DesignationName "+
                ", ISNULL(GradeName,'-')  AS GradeName,StatusName ,convert ( varchar ,WorkingPattern) AS WorkingPattern FROM [StaffView] ORDER BY [FirstName]");

            try
            {
                var lst = context.Database.SqlQuery<EmployeeList>(qryStr.ToString()).Select(d => new EmployeeList()
                {
                    StaffId =d.StaffId,
                    FirstName =d.FirstName ,
                    DeptName =d.DeptName ,
                    DesignationName =d.DesignationName ,
                    GradeName =d.GradeName ,
                    StatusName =d.StatusName ,
                    WorkingPattern = d.WorkingPattern
                }).ToList();

                if(lst == null)
                {
                    return new List<EmployeeList>();
                }
                else
                {
                    return lst;
                }
            }
            catch(Exception)
            {
                return new List<EmployeeList>();
            }
        }

        public List<EmployeeList> LoadCriteriaWiseListOfEmployees(string criteriastring)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT StaffId ,FirstName ,DeptName ,DesignationName " +
                ",GradeName ,StatusName ,convert ( varchar ,WorkingPattern) AS WorkingPattern " +
                "FROM [StaffView] WHERE 1=1 " + criteriastring);

            try
            {
                var lst = context.Database.SqlQuery<EmployeeList>(qryStr.ToString()).Select(d => new EmployeeList()
                {
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    DeptName = d.DeptName,
                    DesignationName = d.DesignationName,
                    GradeName = d.GradeName,
                    StatusName = d.StatusName,
                    WorkingPattern = d.WorkingPattern
                }).ToList();

                if (lst == null)
                {
                    return new List<EmployeeList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<EmployeeList>();
            }
        }

        public List<EmployeeList> LoadAlphabetWiseListOfEmployees(char alphabet)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT StaffId ,FirstName ,DeptName ,DesignationName " +
                ",GradeName ,StatusName ,convert ( varchar ,WorkingPattern) AS WorkingPattern "+
                "FROM [StaffView] WHERE [FirstName] like '" + alphabet + "%' ORDER BY [FirstName]");

            try
            {
                var lst = context.Database.SqlQuery<EmployeeList>(qryStr.ToString()).Select(d => new EmployeeList()
                {
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    DeptName = d.DeptName,
                    DesignationName = d.DesignationName,
                    GradeName = d.GradeName,
                    StatusName = d.StatusName,
                    WorkingPattern = d.WorkingPattern
                }).ToList();

                if (lst == null)
                {
                    return new List<EmployeeList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<EmployeeList>();
            }
        }

    
    }
}
