using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

namespace Attendance.Repository
{
    public class StaffListRepository : IDisposable
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

        public StaffListRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<StaffList> GetAllStaffLists(string qryStr)
        {
            try
            {
                var lst = context.Database.SqlQuery<StaffList>(qryStr).Select(d => new StaffList()
                {
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    Department = d.Department,
                    Location = d.Location
                }).ToList();

                if (lst.Count > 0)
                    return lst;
                else
                    return new List<StaffList>();
            }
            catch (Exception)
            {
                return new List<StaffList>();
            }

        }

        public List<SecurityGroupList> GetAllUserRoleLists(string qryStr)
        {
            try
            {
                var lst = context.Database.SqlQuery<SecurityGroupList>(qryStr).Select(d => new SecurityGroupList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsActive = d.IsActive
                }).ToList();

                if (lst.Count > 0)
                    return lst;
                else
                    return new List<SecurityGroupList>();
            }
            catch (Exception)
            {
                return new List<SecurityGroupList>();
            }

        }

        public List<string> GetStaffsByReportingManager(string reportingManager)
        {
            List<string> Staffs = new List<string>();
            StringBuilder query = new StringBuilder();
            query.Clear();
            string allStaffs = string.Empty;
            query.Append("Select Distinct StaffId from [StaffOfficial] where  ReportingManager = @reportingManager AND StaffId in (Select Distinct Id from [Staff] where StaffStatusId = 1)  ");
            var lst = context.Database.SqlQuery<string>(query.ToString(),new SqlParameter("@reportingManager", reportingManager)).ToList();
            if (lst == null)
            {
                return Staffs;
            }
            else
            {
                foreach (var rec in lst)
                {
                    Staffs.Add(rec);
                    StringBuilder query1 = new StringBuilder();
                    query1.Clear();
                    query1.Append(" Select Distinct StaffId from [StaffOfficial] where ReportingManager = @rec");
                    var lst1 = context.Database.SqlQuery<string>(query1.ToString(),new SqlParameter("@rec",rec)).ToList();//.Select(d => new StaffsTemp()
                    if (lst1.Count > 0)
                    {
                        foreach (var rec1 in lst1)
                        {
                            Staffs.Add(rec1);
                        }

                    }
                }
                Staffs.Add(reportingManager);
                return Staffs;
            }

        }
        public List<StaffList> GetStaffBasedonrep(string criteria, string ReportingManager)
        {
            List<string> staff = GetStaffsByReportingManager(ReportingManager);

            //foreach (var st in staff) { 

            if (staff.Contains(criteria) == true)
            {
                try
                {
                    var qryStr = new StringBuilder();
                    qryStr.Clear();

                    qryStr.Append("Select staffid , DBO.FNGETSTAFFNAME(STAFFID) as staffname , DBO.FNGETMASTERNAME(STAFFID , 'DP') as Department, DBO.FNGETMASTERNAME(STAFFID , 'L') as Location from StaffOfficial a inner join Staff b on a.staffid = b.id WHERE 1=1   AND b.StaffStatusId = 1 AND  staffid = @criteria");
                    var lst = context.Database.SqlQuery<StaffList>(qryStr.ToString(),new SqlParameter("@criteria", criteria)).Select(d => new StaffList()
                    {
                        StaffId = d.StaffId,
                        StaffName = d.StaffName,
                        Department = d.Department,
                        Location = d.Location
                    }).ToList();

                    if (lst.Count > 0)
                        return lst;
                    else
                        return new List<StaffList>();
                }
                catch (Exception)
                {
                    return new List<StaffList>();
                }

            }
            else
            {
                return new List<StaffList>();
            }
        }


        public List<StaffList> GetStaffBasedonrep(string ReportingManager)
        {

            List<string> staff = GetStaffsByReportingManager(ReportingManager);

            //foreach (var st in staff) { 
            List<StaffList> lt = new List<StaffList>();

            foreach (var st in staff)
            {
                try
                {

                    var qryStr = new StringBuilder();
                    qryStr.Clear();

                    qryStr.Append("Select staffid , DBO.FNGETSTAFFNAME(STAFFID) as staffname , DBO.FNGETMASTERNAME(STAFFID , 'DP') as Department, DBO.FNGETMASTERNAME(STAFFID , 'L') as Location from StaffOfficial a inner join Staff b on a.staffid = b.id WHERE 1=1 AND b.StaffStatusId = 1 AND  staffid = @st ");
                    var lst = context.Database.SqlQuery<StaffList>(qryStr.ToString(),new SqlParameter("@st",st)).Select(d => new StaffList()
                    {
                        StaffId = d.StaffId,
                        StaffName = d.StaffName,
                        Department = d.Department,
                        Location = d.Location
                    }).FirstOrDefault();


                    lt.Add(lst);
                }

                catch (Exception)
                {
                    return new List<StaffList>();
                }
            }
            if (lt.Count != 0)
            {
                return lt;
            }
            return new List<StaffList>();
        }
    }
}

