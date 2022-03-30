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

namespace Attendance.Repository {
    public class PrefixSuffixRepository : IDisposable
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

        public PrefixSuffixRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<LeaveView> LoadLeaves() {
            var qryStr  = new StringBuilder();
            qryStr.Clear();
            qryStr.Append( "select Id , Name from leavetype" );

            try
            {
                var lst = context.Database.SqlQuery<LeaveView>( qryStr.ToString() ).Select( d => new LeaveView()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if(lst == null)
                {
                    return new List<LeaveView>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LeaveView>();
            }
        }

        public void SavePrefixSuffix(PrefixSuffixSetting pss) {
            context.PrefixSuffixSetting.AddOrUpdate(pss);
            context.SaveChanges();
        }

        public List<PrefixSuffixList> GetAllPrefixSuffix()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select convert ( varchar , a.id ) as Id , LeaveTypeId , " +
                          "lt1.Name as LeaveName , PrefixLeaveTypeId , " +
                          "lt2.Name  as PrefixLeaveName , SuffixLeaveTypeId , " +
                          "lt3.name as SuffixLeavename , convert ( varchar , a.IsActive ) as IsActive from prefixsuffixsetting a " +
                          "inner join leavetype lt1 on lt1.id = a.LeaveTypeId " +
                          "inner join leavetype lt2 on lt2.id = a.PrefixLeaveTypeId " +
                          "inner join leavetype lt3 on lt3.id = a.SuffixLeaveTypeId");

            try
            {
                var lst = context.Database.SqlQuery<PrefixSuffixList>(qryStr.ToString()).Select(d => new PrefixSuffixList()
                    {
                        Id = d.Id,
                        LeaveTypeId = d.LeaveTypeId,
                        LeaveName = d.LeaveName,
                        PrefixLeaveTypeId = d.PrefixLeaveTypeId,
                        PrefixLeaveName = d.PrefixLeaveName,
                        SuffixLeaveTypeId = d.SuffixLeaveTypeId,
                        SuffixLeavename = d.SuffixLeavename,
                        IsActive = d.IsActive
                    }).ToList();

                if (lst == null)
                {
                    return new List<PrefixSuffixList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<PrefixSuffixList>();
            }
        }
    }
}
