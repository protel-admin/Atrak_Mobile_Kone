using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using Attendance.Model;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

namespace Attendance.Repository {
    public class DutyRoosterRepository : IDisposable
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

        public string GetShiftPatterns(string id)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@id", id);
            var ctx = new AttendanceManagementContext();
            var qryStr = new StringBuilder();
            var b = "";

            qryStr.Append("SELECT PARENTID + '~' + PARENTTYPE + '~' +  SAL.SHORTNAME AS [SHIFTPATTERN] " +
                          "FROM ShiftPatternTxn SPT " +
                          "INNER JOIN vwSHIFTSANDLEAVES SAL ON SPT.PARENTID  = SAL.ID " +
                          "WHERE PATTERNID = @id" +
                          " ORDER BY SPT.ID");

            var lstSP = ctx.Database.SqlQuery<string>(qryStr.ToString(), sqlParameter).ToList();

            foreach(var a in lstSP)
                b = b + a + "|";

            if (b.Trim() != string.Empty)
            {
                b = b.Substring(0, b.Length - 1);
            }

            return b;
        }

        public void SaveDutyRoosterSettings( ShiftPattern sp , string dutyRooster , string OldDutyRooster)
        {
            var ctx = new AttendanceManagementContext();
            var qryStr = new StringBuilder();

            using (var trans = ctx.Database.BeginTransaction())
            {
                var spt = new ShiftPatternTxn();
                try
                {
                    ctx.ShiftPattern.AddOrUpdate(sp);
                    ctx.SaveChanges();

                    if ( dutyRooster.Trim ( ) != string.Empty ) {
                        if ( dutyRooster.EndsWith ( "|" ) == true ) {
                            dutyRooster = dutyRooster.Substring ( 0 , dutyRooster.Length - 1 );
                        }

                    if (dutyRooster != OldDutyRooster)
                    {
                        qryStr.Clear ( );
                        qryStr.Append ( "DELETE FROM ShiftPatternTxn WHERE PATTERNID = " + sp.Id.ToString ( ) );
                        ctx.Database.ExecuteSqlCommand ( qryStr.ToString ( ) );
                            var arr = dutyRooster.Split ( '|' );
                            foreach ( var a in arr ) {
                                var b = a.Split ( '~' );

                                spt = new ShiftPatternTxn ( );
                                spt.PatternId = sp.Id;
                                spt.ParentId = b [ 0 ];
                                spt.ParentType = b [ 1 ];
                                ctx.ShiftPatternTxn.AddOrUpdate ( spt );
                                ctx.SaveChanges ( );
                            }
                        }
                    }
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public List<DutyRoosterView> GetAllDutyRoosters()
        {
            var ctx = new AttendanceManagementContext ( );
            var qryStr = new StringBuilder();

            qryStr.Clear();
            qryStr.Append("SELECT Id , Name , IsRotational , " +
                          "IsLifeTime , StartDate , " +
                          "EndDate , IsActive " +
                          "FROM ShiftPattern");

            try
            {
                //var lstDr = ctx.Database.SqlQuery<DutyRoosterView>(qryStr.ToString()).ToList();

                var lstDr = ctx.Database.SqlQuery<DutyRoosterView> ( qryStr.ToString ( ) ).ToList ( ).Select ( d => new DutyRoosterView ( ) {
                    Id = d.Id,
                    Name = d.Name ,
                    IsRotational = d.IsRotational ,
                    IsLifeTime = d.IsLifeTime ,
                    StartDate = d.StartDate ,
                    EndDate = d.EndDate ,
                    IsActive = d.IsActive
                } ).ToList ( );

                if ( lstDr == null )
                    lstDr = new List<DutyRoosterView> ( );

                return lstDr;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<LeaveView> GetAllLeaves()
        {
            //

            var ctx = new AttendanceManagementContext ( );
            var lstOffs = ctx.Database.SqlQuery<LeaveView>( "SELECT Id , Name , ShortName FROM LEAVETYPE WHERE IsActive = 1 AND ISACCOUNTABLE=0 AND ISENCASHABLE=0 AND ISPAIDLEAVE=1 AND ISCOMMON=1 AND ISPERMISSION=0").ToList ( ).Select ( d => new LeaveView ( ) {
                                                              Id = d.Id ,
                                                              Name = d.Name ,
                                                              ShortName = d.ShortName ,
                                                          } ).ToList ( );

            if ( lstOffs == null || ( lstOffs != null && lstOffs.Count == 0 ) ) {
                lstOffs = new List<LeaveView> ( );
            }
            return lstOffs;        
        }

        public List<ShiftView> GetAllShifts( )
        {
            var ctx = new AttendanceManagementContext();
            var lstShifts = ctx.Database.SqlQuery<ShiftView>("SELECT ID , NAME , SHORTNAME , " +
                                                          "LEFT ( CONVERT ( VARCHAR , STARTTIME , 108 ) , 5 ) AS [StartTime] , " +
                                                          "LEFT ( CONVERT ( VARCHAR , ENDTIME , 108) , 5 ) AS [EndTime] " +
                                                          "FROM SHIFTS").ToList().Select(d => new ShiftView()
                                                          {
                                                              Id = d.Id,
                                                              Name = d.Name,
                                                              ShortName = d.ShortName,
                                                              StartTime = d.StartTime,
                                                              EndTime = d.EndTime
                                                          }).ToList();

            if(lstShifts == null || ( lstShifts != null && lstShifts.Count ==0))
            {
                lstShifts = new List<ShiftView> ( );
            }
            return lstShifts;
        }
    }
}
