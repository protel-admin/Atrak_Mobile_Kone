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
    public class ExecuteShiftPatternRepository : IDisposable
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

        public ExecuteShiftPatternRepository()
        {
            context = new AttendanceManagementContext();
        }

/*
FOR MOVING EMPLOYEES BETWEEN GROUPS
*/
        public void ProcessShiftPattern(DateTime startdate, DateTime enddate,string staffid, string EmployeeGroupId)
        {
            CheckIfEmployeeGroupExists(EmployeeGroupId);

            var lstEGSPT = CheckIfShiftPatternLinkedToStaffGroup(EmployeeGroupId);

            CheckIfShiftPatternExists(lstEGSPT.FirstOrDefault().ShiftPatternId);

            //var lstShiftPatterns = GetPendingShiftPatternToBeExecuted(lstEGSPT.FirstOrDefault().ShiftPatternId);
            var lstShiftPatterns = GetShiftPatternSettings(lstEGSPT.FirstOrDefault().ShiftPatternId);

            ExecuteShiftPattern(lstShiftPatterns,startdate,enddate, staffid);

        }

        public void CheckIfEmployeeGroupExists(string EmployeeGroupId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@EmployeeGroupId", EmployeeGroupId);
            var qryStr = new StringBuilder();
            //check if employee group has been configured.
            qryStr.Clear();
            qryStr.Append("SELECT COUNT(*) FROM EmployeeGroup where id = @EmployeeGroupId");
            if (context.Database.SqlQuery<int>(qryStr.ToString(), sqlParameter).FirstOrDefault<int>() == 0) //if not configured then...
            {
                //throw exception.
                throw new Exception("Employee group is not configured.");
            }
        }


        public List<EmployeeGroupShiftPatternTxn> CheckIfShiftPatternLinkedToStaffGroup(string EmployeeGroupId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@EmployeeGroupId", EmployeeGroupId);
            var qryStr = new StringBuilder();
            //check if any one shift pattern has been associated with any one employee group or not.
            qryStr.Clear();
            qryStr.Append("SELECT * FROM EmployeeGroupShiftPatternTxn where employeegroupid = @EmployeeGroupId ");

            var lst = context.Database.SqlQuery<EmployeeGroupShiftPatternTxn>(qryStr.ToString(),sqlParameter).Select(d => new EmployeeGroupShiftPatternTxn()
            {
                Id = d.Id,
                EmployeeGroupId = d.EmployeeGroupId,
                ShiftPatternId = d.ShiftPatternId,
                IsActive = d.IsActive
            }).ToList();

            if (lst == null)
                return new List<EmployeeGroupShiftPatternTxn>();

            return lst;
        }


        public void CheckIfShiftPatternExists(int ShiftPatternId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@ShiftPatternId", ShiftPatternId);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT COUNT(*) FROM ShiftPattern where id = @ShiftPatternId");

            if (context.Database.SqlQuery<int>(qryStr.ToString(),sqlParameter).FirstOrDefault<int>() == 0) //if not configured then...
            {
                //throw exception.
                throw new Exception("Shift Pattern is not configured.");
            }
        }


        public List<ShiftPatternTemp> GetShiftPatternSettings(int ShiftPatternId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@ShiftPatternId", ShiftPatternId);
            var qryStr = new StringBuilder();
            //GET A LIST OF ALL PENDING SHIFT PATTERNS TO BE EXECUTED.
            qryStr.Clear();
            qryStr.Append("SELECT ID AS ShiftPatternID, ISROTATIONAL , ISLIFETIME , STARTDATE , ENDDATE , UPDATEDUNTIL , 0 FROM ShiftPattern " +
                            "WHERE Id = @ShiftPatternId");

            var lstShiftPatterns = context.Database.SqlQuery<ShiftPatternTemp>(qryStr.ToString(),sqlParameter).ToList().Select(d => new ShiftPatternTemp()
            {
                ShiftPatternID = d.ShiftPatternID,
                IsRotational = d.IsRotational,
                IsLifeTime = d.IsLifeTime,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                UpdatedUntil = d.UpdatedUntil
            }).ToList();

            if (lstShiftPatterns == null)
                return new List<ShiftPatternTemp>();

            return lstShiftPatterns;

        }


        public List<ShiftPatternTemp> GetPendingShiftPatternToBeExecuted(int ShiftPatternId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@ShiftPatternId", ShiftPatternId);
            var qryStr = new StringBuilder();
            //GET A LIST OF ALL PENDING SHIFT PATTERNS TO BE EXECUTED.
            qryStr.Clear();
            qryStr.Append("SELECT ID AS ShiftPatternID, ISROTATIONAL , ISLIFETIME , STARTDATE , ENDDATE , UPDATEDUNTIL , 0 FROM ShiftPattern " +
                            "WHERE id = @ShiftPatternId AND ( UPDATEDUNTIL IS NULL ) OR ( ( ( DATEDIFF ( D , GETDATE() , UPDATEDUNTIL ) = 1 ) OR " +
                            "( DATEDIFF ( D , GETDATE() , UPDATEDUNTIL ) <= 0 ) ) AND ( ( ISROTATIONAL = 1 AND ( DATEDIFF ( D , GETDATE() , ENDDATE ) >=0 ) ) " +
                            "OR ISLIFETIME = 1 ) )");

            var lstShiftPatterns = context.Database.SqlQuery<ShiftPatternTemp>(qryStr.ToString(),sqlParameter).ToList().Select(d => new ShiftPatternTemp()
            {
                ShiftPatternID = d.ShiftPatternID,
                IsRotational = d.IsRotational,
                IsLifeTime = d.IsLifeTime,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                UpdatedUntil = d.UpdatedUntil
            }).ToList();

            if (lstShiftPatterns == null)
                return new List<ShiftPatternTemp>();

            return lstShiftPatterns;
        }


        public List<StaffsTemp> GetStaffForShiftPattern(int ShiftPatternId, string StaffId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            if (string.IsNullOrEmpty(StaffId) == false ){ //if not null
                if (StaffId.EndsWith(",") == true)
                { //if ends with a coma.
                    StaffId = StaffId.Substring(0, StaffId.Length - 1);
                }
                StaffId = "'" + StaffId.Replace(",", "','") + "'";
            } //remove the ending coma.


            var qryStr = new StringBuilder();
            //qryStr.Clear();
            //qryStr.Append("SELECT DISTINCT EGT.STAFFID FROM EmployeeGroupShiftPatternTxn EGSPT " +
            //                "INNER JOIN EmployeeGroup EG ON EGSPT.EMPLOYEEGROUPID = EG.ID " +
            //                "INNER JOIN EmployeeGroupTxn EGT ON EGT.EMPLOYEEGROUPID = EG.ID " +
            //                "INNER JOIN STAFF STA ON EGT.STAFFID = STA.ID WHERE EGT.STAFFID IN ("+StaffId+") AND SHIFTPATTERNID = " + ShiftPatternId);

            qryStr.Clear();
            qryStr.Append("select id as [StaffId] from staff where id in ( @StaffId )");

            var lstStaffs = context.Database.SqlQuery<StaffsTemp>(qryStr.ToString(),sqlParameter).ToList().Select(d => new StaffsTemp()
            {
                StaffId = d.StaffId
            }).ToList();

            if (lstStaffs == null)
                return new List<StaffsTemp>();

            return lstStaffs;
        }


        public void ExecuteShiftPattern(List<ShiftPatternTemp> lst, DateTime StartDate, DateTime EndDate, string StaffId)
        {
            var qryStr = new StringBuilder();
            var loopStartDate = DateTime.Now;
            var loopEndDate = DateTime.Now;
            List<DatesTemp> lstDates = new List<DatesTemp>();
            var Count = 0;
            var ctr = 0;
            //RUN LOOP TO READ THROUGH ALL PENDING SHIFT PATTERNS AND EXECUTE THEM ONE AFTER THE OTHER.
            foreach (var sp in lst)
            {

                var lstStaffs = GetStaffForShiftPattern(sp.ShiftPatternID, StaffId);

                if (lstStaffs.Count > 0)
                {
                    var lstShiftSettings = GetShiftPattern(sp.ShiftPatternID);
                    //get the count of total shifts set in the pattern.
                    Count = lstShiftSettings.Count;
                    loopStartDate = StartDate;
                    loopEndDate = EndDate;


                    if (sp.IsRotational == true && sp.IsLifeTime == false)
                    {
                        if (sp.EndDate < loopEndDate)
                        {
                            loopEndDate = Convert.ToDateTime(sp.EndDate);
                        }
                    }
                    else if (sp.IsRotational == false && sp.IsLifeTime == false)
                    {
                        loopEndDate = Convert.ToDateTime(sp.EndDate);
                    }

                    //load the dates in the list object.
                    while (loopStartDate <= loopEndDate)
                    {
                        if ((ctr % Count) == 0)
                        {
                            ctr = 0;
                        }
                        ctr += 1;
                        lstDates.Add(new DatesTemp() { ShiftDate = loopStartDate, Id = ctr });
                        loopStartDate = loopStartDate.AddDays(1);
                    }


                    var lstFinal = (from dts in lstDates
                                    from sta in lstStaffs
                                    orderby sta.StaffId, dts.ShiftDate
                                    select new ShiftsFinal()
                                    {
                                        ShiftStartDate = dts.ShiftDate,
                                        ShiftId = dts.Id.ToString(),
                                        StaffId = sta.StaffId
                                    }).ToList();

                    foreach (var item in lstFinal)
                    {
                        if (lstShiftSettings[Convert.ToInt16(item.ShiftId) - 1].ShiftId.ToString().StartsWith("SH"))
                        {
                            //if(lstShiftPatterns.Find(x => x.ShiftPatternID == item.ShiftId))
                            item.ShiftStartTime = lstShiftSettings[Convert.ToInt16(item.ShiftId) - 1].ShiftStartTime.Value;
                            item.ShiftEndTime = lstShiftSettings[Convert.ToInt16(item.ShiftId) - 1].ShiftEndTime.Value;

                            if (item.ShiftEndTime < item.ShiftStartTime)
                            {
                                item.ShiftEndDate = item.ShiftStartDate.Value.AddDays(1);
                            }
                            else
                            {
                                item.ShiftEndDate = item.ShiftStartDate;
                            }
                        }
                        item.ShiftId = lstShiftSettings[Convert.ToInt16(item.ShiftId) - 1].ShiftId.ToString();
                    }
                    StringBuilder qry = new StringBuilder();

                    using (var trans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            AttendanceData ad = null;
                            TimeSpan T1, T2, T3;
                            var LeaveId = string.Empty;
                            var ShiftId = string.Empty;
                            var LeaveShortName = string.Empty;
                            var ShiftShortName = string.Empty;

                            foreach (var item in lstFinal)
                            {
                                ad = new AttendanceData();
                                ad.StaffId = item.StaffId;
                                if (ShiftId != item.ShiftId)
                                {
                                    SqlParameter[] sqlParameter = new SqlParameter[1];
                                    sqlParameter[0] = new SqlParameter("@ShiftId", item.ShiftId);
                                    qry.Clear();
                                    qry.Append("SELECT SHORTNAME FROM SHIFTS WHERE ID = @ShiftId ");
                                    ShiftShortName =
                                        context.Database.SqlQuery<string>(
                                            qry.ToString(), sqlParameter)
                                            .FirstOrDefault();

                                    ShiftId = item.ShiftId;
                                }
                                ad.ShiftId = item.ShiftId;
                                ad.ShiftInDate = item.ShiftStartDate;
                                if (item.ShiftId.StartsWith("SH"))
                                {
                                    ad.ShiftShortName = ShiftShortName;
                                    ad.AttendanceStatus = ShiftShortName;
                                    ad.FHStatus = ShiftShortName;
                                    ad.SHStatus = ShiftShortName;
                                    ad.ShiftInTime = item.ShiftStartTime;
                                    ad.ShiftOutDate = item.ShiftEndDate;
                                    ad.ShiftOutTime = item.ShiftEndTime;
                                    T1 = new TimeSpan(item.ShiftEndTime.Value.Hour, item.ShiftEndTime.Value.Minute,
                                        item.ShiftEndTime.Value.Second);
                                    T2 = new TimeSpan(item.ShiftStartTime.Value.Hour, item.ShiftStartTime.Value.Minute,
                                        item.ShiftStartTime.Value.Second);

                                    if (T1 < T2)
                                    {
                                        T3 = new TimeSpan();
                                        T1 = new TimeSpan(23, 59, 59);
                                        T3 = T1.Subtract(T2);
                                        T1 = new TimeSpan(item.ShiftEndTime.Value.Hour, item.ShiftEndTime.Value.Minute,
                                        item.ShiftEndTime.Value.Second + 1);
                                        T3 = T3.Add(T1);
                                        ad.ExpectedWorkingHours = Convert.ToDateTime(T3.ToString());
                                    }
                                    else if (T1 > T2)
                                    {
                                        T3 = T1.Subtract(T2);
                                        ad.ExpectedWorkingHours = Convert.ToDateTime(T3.ToString());
                                    }

                                }
                                else if (item.ShiftId.StartsWith("LV"))
                                {
                                    if (LeaveId != item.ShiftId)
                                    {
                                        SqlParameter[] sqlParameter = new SqlParameter[1];
                                        sqlParameter[0] = new SqlParameter("@ShiftId", item.ShiftId);

                                        qry.Clear();
                                        qry.Append("SELECT SHORTNAME FROM LEAVETYPE WHERE ID = @ShiftId ");

                                        LeaveShortName =
                                            context.Database.SqlQuery<string>(
                                                qry.ToString(),sqlParameter)
                                                .FirstOrDefault();

                                        LeaveId = item.ShiftId;
                                    }
                                    ad.AttendanceStatus = LeaveShortName;
                                    ad.ShiftShortName = LeaveShortName;
                                    ad.FHStatus = LeaveShortName;
                                    ad.SHStatus = LeaveShortName;
                                }
                                ad.CreatedOn = DateTime.Now;
                                ad.IsManualPunch = false;
                                context.AttendanceData.Add(ad);
                            }
                            context.SaveChanges();
                            context.Database.ExecuteSqlCommand("UPDATE ShiftPattern SET UPDATEDUNTIL = '" +
                                                           loopEndDate.ToString("dd-MMM-yyyy") + "' WHERE ID = " +
                                                           sp.ShiftPatternID);
                            trans.Commit();
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                            throw;
                        }
                        finally
                        {
                            lstStaffs.Clear();
                            lstDates.Clear();
                            lstFinal.Clear();
                            //lstShiftSettings.Clear();
                            ctr = 0;
                        }
                    }
                }
            }
        }

/*
FOR SHIFT PATTERN EXECUTION.  
 */

        public void CheckIfShiftPatternExists()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT COUNT(*) FROM ShiftPattern");

            if (context.Database.SqlQuery<int>(qryStr.ToString()).FirstOrDefault<int>() == 0) //if not configured then...
            {
                //throw exception.
                throw new Exception("Shift Pattern is not configured.");
            }
        }

        public void CheckIfEmployeeGroupExists()
        {
            var qryStr = new StringBuilder();
            //check if employee group has been configured.
            qryStr.Clear();
            qryStr.Append("SELECT COUNT(*) FROM EmployeeGroup");
            if (context.Database.SqlQuery<int>(qryStr.ToString()).FirstOrDefault<int>() == 0) //if not configured then...
            {
                //throw exception.
                throw new Exception("Employee group is not configured.");
            }
        }

        public void CheckIfShiftPatternLinkedToStaffGroup()
        {
            var qryStr = new StringBuilder();
            //check if any one shift pattern has been associated with any one employee group or not.
            qryStr.Clear();
            qryStr.Append("SELECT COUNT(*) FROM EmployeeGroupShiftPatternTxn");
            if (context.Database.SqlQuery<int>(qryStr.ToString()).FirstOrDefault<int>() == 0) //if not associated then...
            {
                //throw exception.
                throw new Exception("Shift pattern is not associated to any employee group.");
            }
        }



        public List<ShiftPatternTemp> GetPendingShiftPatternToBeExecuted()
        {
            var qryStr = new StringBuilder();
            //GET A LIST OF ALL PENDING SHIFT PATTERNS TO BE EXECUTED.
            qryStr.Clear();
            qryStr.Append("SELECT ID AS ShiftPatternID, ISROTATIONAL , ISLIFETIME , STARTDATE , ENDDATE , UPDATEDUNTIL , 0 FROM ShiftPattern " +
                            "WHERE ( UPDATEDUNTIL IS NULL ) OR ( ( ( DATEDIFF ( D , GETDATE() , UPDATEDUNTIL ) = 1 ) OR " +
                            "( DATEDIFF ( D , GETDATE() , UPDATEDUNTIL ) <= 0 ) ) AND ( ( ISROTATIONAL = 1 AND ( DATEDIFF ( D , GETDATE() , ENDDATE ) >=0 ) ) " +
                            "OR ISLIFETIME = 1 ) )");

            var lstShiftPatterns = context.Database.SqlQuery<ShiftPatternTemp>(qryStr.ToString()).ToList().Select(d => new ShiftPatternTemp()
            {
                ShiftPatternID = d.ShiftPatternID,
                IsRotational = d.IsRotational,
                IsLifeTime = d.IsLifeTime,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                UpdatedUntil = d.UpdatedUntil
            }).ToList();

            if(lstShiftPatterns == null)
                return new List<ShiftPatternTemp>();

            return lstShiftPatterns;
        }




        public List<StaffsTemp> GetStaffForShiftPattern(int ShiftPatternId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@ShiftPatternId", ShiftPatternId);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT DISTINCT EGT.STAFFID FROM EmployeeGroupShiftPatternTxn EGSPT " +
                            "INNER JOIN EmployeeGroup EG ON EGSPT.EMPLOYEEGROUPID = EG.ID " +
                            "INNER JOIN EmployeeGroupTxn EGT ON EGT.EMPLOYEEGROUPID = EG.ID " +
                            "INNER JOIN STAFF STA ON EGT.STAFFID = STA.ID WHERE SHIFTPATTERNID = @ShiftPatternId");

            var lstStaffs = context.Database.SqlQuery<StaffsTemp>(qryStr.ToString(),sqlParameter).ToList().Select(d => new StaffsTemp()
            {
                StaffId = d.StaffId
            }).ToList();

            if (lstStaffs == null)
                return new List<StaffsTemp>();

            return lstStaffs;
        }

        public List<ShiftSettings> GetShiftPattern(int ShiftPatternId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@ShiftPatternId", ShiftPatternId);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT PARENTID AS [ShiftId] , STARTTIME AS [ShiftStartTime] , ENDTIME AS [ShiftEndTime] FROM ShiftPatternTxn SPT " +
                            "LEFT JOIN SHIFTS SFT ON SPT.PARENTID = SFT.ID WHERE PatternId = @ShiftPatternId");

            var lstShiftSettings = context.Database.SqlQuery<ShiftSettings>(qryStr.ToString(),sqlParameter).ToList().Select(d => new ShiftSettings()
            {
                ShiftId = d.ShiftId,
                ShiftStartTime = d.ShiftStartTime,
                ShiftEndTime = d.ShiftEndTime
            }).ToList();

            if (lstShiftSettings == null)
                return new List<ShiftSettings>();

            return lstShiftSettings;
        }

        public void ExecuteShiftPattern(List<ShiftPatternTemp> lst)
        {
            var qryStr = new StringBuilder();
            var loopStartDate = DateTime.Now;
            var loopEndDate = DateTime.Now;
            List<DatesTemp> lstDates = new List<DatesTemp>();
            var Count = 0;
            var ctr = 0;
            //RUN LOOP TO READ THROUGH ALL PENDING SHIFT PATTERNS AND EXECUTE THEM ONE AFTER THE OTHER.
            foreach (var sp in lst)
            {

                var lstStaffs = GetStaffForShiftPattern(sp.ShiftPatternID);

                if (lstStaffs.Count > 0)
                {

                    var lstShiftSettings = GetShiftPattern(sp.ShiftPatternID);
                    //get the count of total shifts set in the pattern.
                    Count = lstShiftSettings.Count;

                    //get the last updated date.
                    //check if the last updated date is having a date or is null.
                    //START DATE WILL BE EITHER FROM THE CURRENT DATE OR FROM THE DATE OF LAST UPDATED.
                    if (sp.UpdatedUntil == null) //if null then...
                    {
                        //set the loop start date to 1 day advance from current date.
                        loopStartDate = Convert.ToDateTime(sp.StartDate);
                        if (loopStartDate < DateTime.Now)
                        {
                            loopStartDate = DateTime.Now.AddDays(1);
                        }
                    }
                    else //if not null then...
                    {
                        //set the loop start date to last updated date.
                        loopStartDate = Convert.ToDateTime(sp.UpdatedUntil).AddDays(1);
                    }


                    //DECISION HAS TO BE TAKEN ON HOW TO SET THE END DATE.
                    //check if the shift pattern is lifetime or rotational
                    loopEndDate = loopStartDate.AddMonths(1);
                    if (sp.IsRotational == true && sp.IsLifeTime == false)
                    {
                        if (sp.EndDate < loopEndDate)
                        {
                            loopEndDate = Convert.ToDateTime(sp.EndDate);
                        }
                    }
                    else if (sp.IsRotational == false && sp.IsLifeTime == false)
                    {
                        loopEndDate = Convert.ToDateTime(sp.EndDate);
                    }

                    //load the dates in the list object.
                    while (loopStartDate <= loopEndDate)
                    {
                        if ((ctr % Count) == 0)
                        {
                            ctr = 0;
                        }
                        ctr += 1;
                        lstDates.Add(new DatesTemp() { ShiftDate = loopStartDate, Id = ctr });
                        loopStartDate = loopStartDate.AddDays(1);
                    }


                    var lstFinal = (from dts in lstDates
                                    from sta in lstStaffs
                                    orderby sta.StaffId, dts.ShiftDate
                                    select new ShiftsFinal()
                                    {
                                        ShiftStartDate = dts.ShiftDate,
                                        ShiftId = dts.Id.ToString(),
                                        StaffId = sta.StaffId
                                    }).ToList();

                    foreach (var item in lstFinal)
                    {
                        if (lstShiftSettings[Convert.ToInt16(item.ShiftId) - 1].ShiftId.ToString().StartsWith("SH"))
                        {
                            //if(lstShiftPatterns.Find(x => x.ShiftPatternID == item.ShiftId))
                            item.ShiftStartTime = lstShiftSettings[Convert.ToInt16(item.ShiftId) - 1].ShiftStartTime.Value;
                            item.ShiftEndTime = lstShiftSettings[Convert.ToInt16(item.ShiftId) - 1].ShiftEndTime.Value;

                            if (item.ShiftEndTime < item.ShiftStartTime)
                            {
                                item.ShiftEndDate = item.ShiftStartDate.Value.AddDays(1);
                            }
                            else
                            {
                                item.ShiftEndDate = item.ShiftStartDate;
                            }
                        }
                        item.ShiftId = lstShiftSettings[Convert.ToInt16(item.ShiftId) - 1].ShiftId.ToString();
                    }

                    using (var trans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            AttendanceData ad = null;
                            TimeSpan T1, T2, T3;
                            var LeaveId = string.Empty;
                            var ShiftId = string.Empty;
                            var LeaveShortName = string.Empty;
                            var ShiftShortName = string.Empty;
                            StringBuilder qry = new StringBuilder();

                            foreach (var item in lstFinal)
                            {
                                ad = new AttendanceData();
                                ad.StaffId = item.StaffId;
                                if (ShiftId != item.ShiftId)
                                {
                                    SqlParameter[] sqlParameter = new SqlParameter[1];
                                    sqlParameter[0] = new SqlParameter("@ShiftId", item.ShiftId);
                                   
                                    qry.Clear();
                                    qry.Append("SELECT SHORTNAME FROM SHIFTS WHERE ID = @ShiftId ");
                                    ShiftShortName =
                                        context.Database.SqlQuery<string>(
                                           qry.ToString(), sqlParameter)
                                            .FirstOrDefault();

                                    ShiftId = item.ShiftId;
                                }
                                ad.ShiftId = item.ShiftId;
                                ad.ShiftInDate = item.ShiftStartDate;
                                if (item.ShiftId.StartsWith("SH"))
                                {
                                    ad.ShiftShortName = ShiftShortName;
                                    ad.AttendanceStatus = ShiftShortName;
                                    ad.FHStatus = ShiftShortName;
                                    ad.SHStatus = ShiftShortName;
                                    ad.ShiftInTime = item.ShiftStartTime;
                                    ad.ShiftOutDate = item.ShiftEndDate;
                                    ad.ShiftOutTime = item.ShiftEndTime;
                                    T1 = new TimeSpan(item.ShiftEndTime.Value.Hour, item.ShiftEndTime.Value.Minute,
                                        item.ShiftEndTime.Value.Second);
                                    T2 = new TimeSpan(item.ShiftStartTime.Value.Hour, item.ShiftStartTime.Value.Minute,
                                        item.ShiftStartTime.Value.Second);

                                    if (T1 < T2)
                                    {
                                        T3 = new TimeSpan();
                                        T1 = new TimeSpan(23, 59, 59);
                                        T3 = T1.Subtract(T2);
                                        T1 = new TimeSpan(item.ShiftEndTime.Value.Hour, item.ShiftEndTime.Value.Minute,
                                        item.ShiftEndTime.Value.Second + 1);
                                        T3 = T3.Add(T1);
                                        ad.ExpectedWorkingHours = Convert.ToDateTime(T3.ToString());
                                    }
                                    else if (T1 > T2)
                                    {
                                        T3 = T1.Subtract(T2);
                                        ad.ExpectedWorkingHours = Convert.ToDateTime(T3.ToString());
                                    }

                                }
                                else if (item.ShiftId.StartsWith("LV"))
                                {
                                    if (LeaveId != item.ShiftId)
                                    {
                                        SqlParameter[] sqlParameter = new SqlParameter[1];
                                        sqlParameter[0] = new SqlParameter("@ShiftId", item.ShiftId);

                                        qry.Clear();
                                        qry.Append("SELECT SHORTNAME FROM LEAVETYPE WHERE ID = @ShiftId ");

                                        LeaveShortName =
                                            context.Database.SqlQuery<string>(
                                                qry.ToString(), sqlParameter)
                                                .FirstOrDefault();

                                        LeaveId = item.ShiftId;
                                    }
                                    ad.AttendanceStatus = LeaveShortName;
                                    ad.ShiftShortName = LeaveShortName;
                                    ad.FHStatus = LeaveShortName;
                                    ad.SHStatus = LeaveShortName;
                                }
                                ad.CreatedOn = DateTime.Now;
                                ad.IsManualPunch = false;
                                context.AttendanceData.Add(ad);
                            }
                            context.SaveChanges();
                            context.Database.ExecuteSqlCommand("UPDATE ShiftPattern SET UPDATEDUNTIL = '" +
                                                           loopEndDate.ToString("dd-MMM-yyyy") + "' WHERE ID = " +
                                                           sp.ShiftPatternID);
                            trans.Commit();
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                            throw;
                        }
                        finally
                        {
                            lstStaffs.Clear();
                            lstDates.Clear();
                            lstFinal.Clear();
                            //lstShiftSettings.Clear();
                            ctr = 0;
                        }
                    }
                }
            }
        }


        //THIS FUNCTION IS TO EXECUTE ALL THE PENDING SHIFT PATTERNS
        public void ProcessShiftPattern()
        {
            CheckIfShiftPatternExists();

            CheckIfEmployeeGroupExists();

            CheckIfShiftPatternLinkedToStaffGroup();

            var lstShiftPatterns = GetPendingShiftPatternToBeExecuted();

            ExecuteShiftPattern(lstShiftPatterns);
        }
    }
}
