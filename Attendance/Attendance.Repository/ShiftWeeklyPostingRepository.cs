using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class ShiftWeeklyPostingRepository:TrackChangeRepository,IDisposable
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

        public ShiftWeeklyPostingRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<ShiftPostingPatternList> GetShiftPostingPatternList(int PatternId)
        {
           SqlParameter[] Param = new SqlParameter[3];
            Param[0] = new SqlParameter("@PatternId", PatternId);
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT CONVERT ( VARCHAR , Id ) AS Id , CONVERT ( VARCHAR , PatternId ) as PatternId , ");
            QryStr.Append(" Sunday , ");
            QryStr.Append(" Monday , ");
            QryStr.Append(" Tuesday , ");
            QryStr.Append(" Wednesday , ");
            QryStr.Append(" Thursday , ");
            QryStr.Append(" Friday , ");
            QryStr.Append(" Saturday ,");
            QryStr.Append(" CreatedOn ,");
            QryStr.Append(" CreatedBy ");
            QryStr.Append("FROM SHIFTPOSTINGPATTERN A WHERE PATTERNID = @PatternId");

            try
            {
                var lst = context.Database.SqlQuery<ShiftPostingPatternList>(QryStr.ToString()).Select(d => new ShiftPostingPatternList()
                {
                    Id = d.Id,
                    PatternId = d.PatternId,
                    Sunday = d.Sunday,
                    Monday = d.Monday,
                    Tuesday = d.Tuesday,
                    Wednesday = d.Wednesday,
                    Thursday = d.Thursday,
                    Friday = d.Friday,
                    Saturday = d.Saturday,
                    CreatedOn = d.CreatedOn,
                    CreatedBy = d.CreatedBy
                }).ToList();

                if (lst == null)
                {
                    return new List<ShiftPostingPatternList>();
                }
                else
                {
                    return lst;
                }
            }
            catch
            {
                return new List<ShiftPostingPatternList>();
            }
        }

        public List<ShiftView> GetAllShifts()
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();

            QryStr.Append("SELECT Id , ShortName + ' - ('+convert ( varchar (5), StartTime , 114) + ' - ' + " +
                "convert ( varchar (5), EndTime , 114) +')'as ShortName FROM SHIFTS where isactive = 1" +
                "order by convert ( varchar ( 8 ) , StartTime , 114 ),convert ( varchar ( 8 ) , EndTime , 114 )  Asc");

            try
            {

                var lst = context.Database.SqlQuery<ShiftView>(QryStr.ToString()).Select(d => new ShiftView()
                {
                    Id = d.Id,
                    ShortName = d.ShortName,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime
                }).ToList();

                if (lst == null)
                {
                    return new List<ShiftView>();
                }
                else
                {
                    return lst;
                }
            }
            catch
            {
                return new List<ShiftView>();
            }
        }

        public void SavePostingInformation(ShiftWeeklyPosting data)
        {

            var lst = from c in data.ShiftWeekList.Where(d => d.Id != 0).ToList() select c;
            var db = from d in context.ShiftPostingPattern.Where(d => d.PatternId == data.PatternId).ToList() select d;

            var bal = from d in db where !(from l in lst select l.Id).Contains(d.Id) select d;

            foreach (var l in bal)
            {
                var dat = context.ShiftPostingPattern.First(d => d.Id == l.Id);
                context.ShiftPostingPattern.Remove(dat);
                context.SaveChanges();
            }

            foreach (var l in data.ShiftWeekList)
            {
                var d = new ShiftPostingPattern();
                if (l.Id != 0)
                {
                    d.Id = l.Id;
                    d.PatternId = data.PatternId;
                    d.Sunday = l.Sunday;
                    d.Monday = l.Monday;
                    d.Tuesday = l.Tuesday;
                    d.Wednesday = l.Wednesday;
                    d.Thursday = l.Thursday;
                    d.Friday = l.Friday;
                    d.Saturday = l.Saturday;
                    d.CreatedOn = l.CreatedOn;
                    d.CreatedBy = l.CreatedBy;
                    d.ModifiedOn = l.ModifiedOn;
                    d.ModifiedBy = l.ModifiedBy;

                    context.ShiftPostingPattern.AddOrUpdate(d);
                    string ActionType = string.Empty;
                    string _ChangeLog = string.Empty;
                    string _PrimaryKeyValue = string.Empty;
                    GetChangeLogString(d, context, ref _ChangeLog, ref ActionType, ref _PrimaryKeyValue);
                    context.SaveChanges();
                    if (string.IsNullOrEmpty(_ChangeLog.ToString()) == false)
                    {
                        RecordChangeLog(context, d.CreatedBy, "SHIFTPOSTINGPATTERN", _ChangeLog, ActionType, _PrimaryKeyValue);
                    }
                }
                else
                {
                    d.Id = l.Id;
                    d.PatternId = data.PatternId;
                    d.Sunday = l.Sunday;
                    d.Monday = l.Monday;
                    d.Tuesday = l.Tuesday;
                    d.Wednesday = l.Wednesday;
                    d.Thursday = l.Thursday;
                    d.Friday = l.Friday;
                    d.Saturday = l.Saturday;
                    d.CreatedOn = l.CreatedOn;
                    d.CreatedBy = l.CreatedBy;

                    context.ShiftPostingPattern.AddOrUpdate(d);
                    context.SaveChanges();
                }
            }
        }

        public List<ShiftPatternList> GetShiftPattern()
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT convert ( varchar , Id ) as Id , UPPER ( Name ) as PatternName FROM SHIFTPATTERN WHERE ISACTIVE = 1");
            try
            {
                var lst = context.Database.SqlQuery<ShiftPatternList>(QryStr.ToString()).Select(d => new ShiftPatternList()
                {
                    Id = d.Id,
                    PatternName = d.PatternName
                }).ToList();

                if (lst == null)
                {
                    return new List<ShiftPatternList>();
                }
                else
                {
                    return lst;
                }
            }
            catch
            {
                return new List<ShiftPatternList>();
            }
        }
    }
}
