using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class ShiftPatternNewRepository : IDisposable
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
        public ShiftPatternNewRepository()
        {
            context = new AttendanceManagementContext();
        }

        public void SaveInformation(ShiftPatternNewList spnl)
        {
          SqlParameter[] Param = new SqlParameter[1];
            Param[0] = new SqlParameter("@spnl", spnl);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("delete from shiftpatterntxn where PatternId = @spnl");
            using (DbContextTransaction Trans = context.Database.BeginTransaction())
            {
                try
                {
                    ShiftPattern sp = new ShiftPattern();
                    if (string.IsNullOrEmpty(spnl.Id) == false)
                    {
                        sp.Id = Convert.ToInt16(spnl.Id);
                    }

                    sp.Name = spnl.Name;
                    sp.IsRotational = spnl.IsRotational == "Yes" ? true : false;
                    sp.IsLifeTime = spnl.IsLifeTime == "Yes" ? true : false;
                    sp.StartDate = Convert.ToDateTime(spnl.StartDate);
                    sp.EndDate = Convert.ToDateTime(spnl.EndDate);
                    sp.DayPattern = 7;
                    sp.WOStartDate = Convert.ToDateTime(spnl.WOStartDate);
                    sp.WODayOffSet = 20;
                    //sp.LastShiftUpdated = System.DateTime.UtcNow.ToString();
                    sp.IsActive = spnl.IsActive == "Yes" ? true : false;
                    context.ShiftPattern.AddOrUpdate(sp);
                    context.SaveChanges();

                    context.Database.ExecuteSqlCommand("delete from shiftpatterntxn where PatternId = @spnl");
                    ShiftPatternTxn spt = null;
                    foreach (var l in spnl.ShiftList)
                    {
                        spt = new ShiftPatternTxn();
                        spt.ParentId = l.ShiftId;
                        spt.ParentType = "S";
                        spt.PatternId = sp.Id;
                        context.ShiftPatternTxn.AddOrUpdate(spt);
                        context.SaveChanges();
                    }
                    Trans.Commit();
                }
                catch (Exception)
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }

        public List<ShiftPatternNewList> LoadShiftPattern()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("select ");
            qryStr.Append("convert ( varchar, Id) as Id,  ");
            qryStr.Append("Name,  ");
            qryStr.Append("replace ( convert ( varchar, StartDate, 106 ), ' ', '-' ) as StartDate,  ");
            qryStr.Append("replace ( convert ( varchar, EndDate, 106 ), ' ', '-' ) as EndDate,  ");
            qryStr.Append("convert ( varchar, DayPattern ) as DayPattern,  ");
            qryStr.Append("replace ( convert ( varchar, WOStartDate, 106 ), ' ', '-' ) as WOStartDate,  ");
            qryStr.Append("convert ( varchar, WODayOffset ) as WODayOffSet,");
            qryStr.Append("case when IsActive = 1 then 'Yes' else 'No' end as IsActive ,");
            qryStr.Append("case when IsRotational = 1 then 'Yes' else 'No' end as IsRotational,");
            qryStr.Append("case when IsLifeTime = 1 then 'Yes' else 'No' end as IsLifeTime, ");
            qryStr.Append("replace ( convert ( varchar, UpdatedUntil, 106 ), ' ', '-' ) as UpdatedUntil,");
            qryStr.Append("replace ( convert ( varchar, WOLastUpdatedDate, 106 ), ' ', '-' ) as WOLastUpdatedDate ");
            qryStr.Append("from shiftpattern ");

            try
            {
                var lst =
                    context.Database.SqlQuery<ShiftPatternNewList>(qryStr.ToString())
                        .Select(d => new ShiftPatternNewList()
                        {
                            Id = d.Id,
                            Name = d.Name,
                            IsRotational = d.IsRotational,
                            IsLifeTime = d.IsLifeTime,
                            StartDate = d.StartDate,
                            EndDate = d.EndDate,
                            UpdatedUntil = d.UpdatedUntil,
                            DayPattern = d.DayPattern,
                            WOStartDate = d.WOStartDate,
                            WODayOffSet = d.WODayOffSet,
                            WOLastUpdatedDate = d.WOLastUpdatedDate,
                            IsActive = d.IsActive
                        }).ToList();

                if (lst == null)
                {
                    return new List<ShiftPatternNewList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ShiftPatternNewList>();
            }

        }


        public List<ShiftList> GetShiftList(int Id)
        {
            SqlParameter[] Param = new SqlParameter[1];
            Param[0] = new SqlParameter("@Id", Id);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select ParentId as ShiftId, b.ShortName as ShiftShortName, b.Name as ShiftName, " +
                           "convert ( varchar (5), b.starttime, 114 ) as ShiftIn, " +
                           "convert ( varchar (5), b.endtime, 114 ) as ShiftOut " +
                           "from shiftpatterntxn a inner join shifts b on a.parentid = b.id where PatternId = @Id");

            try
            {
                var lst = context.Database.SqlQuery<ShiftList>(qryStr.ToString(), Param).Select(d => new ShiftList()
                {
                    ShiftId = d.ShiftId,
                    ShiftShortName = d.ShiftShortName,
                    ShiftName = d.ShiftName,
                    ShiftIn = d.ShiftIn,
                    ShiftOut = d.ShiftOut
                }).ToList();

                if (lst == null)
                {
                    return new List<ShiftList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ShiftList>();
            }
        }
    }
}
