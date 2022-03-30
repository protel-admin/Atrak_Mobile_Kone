using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using Attendance.Model;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class EmployeeShiftPlanRepository : IDisposable
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

        public EmployeeShiftPlanRepository()
        {
            context = new AttendanceManagementContext();
        }

        public StaffView GetStaffInformation(string StaffId)
        {
            SqlParameter[] Param = new SqlParameter[1];
            Param[0] = new SqlParameter("@StaffId", StaffId);
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT DBO.FNGETSTAFFNAME(STAFFID) AS StaffName , DEPTNAME AS DepartmentName FROM STAFFVIEW WHERE STAFFID = @StaffId");
            try
            {

                var data = context.Database.SqlQuery<StaffView>(QryStr.ToString(), Param).Select(d => new StaffView()
                {
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    DepartmentName = d.DepartmentName
                }).FirstOrDefault();

                if (data == null)
                {
                    throw new Exception("Employee Not Found.");
                }
                {
                    return data;
                }
            }
            catch (Exception err)
            {
                if (err.Message == "Employee Not Found.")
                {
                    throw;
                }
                return new StaffView();
            }
        }

        public List<ShiftView> GetShifts()
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT [Id] , [Name] FROM [dbo].[SHIFTS] WHERE [IsActive]=1 order by convert ( varchar ( 8 ) , StartTime , 114 ) ,convert ( varchar ( 8 ) , EndTime , 114 )  Asc");

            try
            {
                var lst = context.Database.SqlQuery<ShiftView>(QryStr.ToString()).Select(d => new ShiftView()
                {
                    Id = d.Id,
                    Name = d.Name
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

        public List<ShiftPatternList> GetShiftPattern()
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT CONVERT ( VARCHAR , [Id] ) AS Id , [Name] as PatternName FROM [dbo].[SHIFTPATTERN] "+ 
                " WHERE [IsActive]=1 AND Id in (Select Distinct PatternId from ShiftPostingPattern)");

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

        public List<ShiftPatternList> GetShiftPatternForWeeklyPosting()
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT CONVERT ( VARCHAR , [Id] ) AS Id , [Name] as PatternName FROM [dbo].[SHIFTPATTERN] "+ 
                            " WHERE [IsActive]=1 AND Id in (Select Distinct PatternId from ShiftPatterntxn)");

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

        public List<WorkingDayPatternList> GetWorkingDayPattern()
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT Id , ( CONVERT ( VARCHAR , WorkingPattern ) + ' - ' + PATTERNDESC ) AS PatternDesc FROM WORKINGDAYPATTERN");

            try
            {
                var lst = context.Database.SqlQuery<WorkingDayPatternList>(QryStr.ToString()).Select(d => new WorkingDayPatternList()
                {
                    Id = d.Id,
                    PatternDesc = d.PatternDesc
                }).ToList();

                if (lst == null)
                {
                    return new List<WorkingDayPatternList>();
                }
                else
                {
                    return lst;
                }
            }
            catch
            {
                return new List<WorkingDayPatternList>();
            }
        }

        public List<WeeklyOffList> GetWeeklyOffs()
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT Id , Name FROM WeeklyOffs");

            try
            {
                var lst = context.Database.SqlQuery<WeeklyOffList>(QryStr.ToString()).Select(d => new WeeklyOffList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<WeeklyOffList>();
                }
                else
                {
                    return lst;
                }
            }
            catch
            {
                return new List<WeeklyOffList>();
            }
        }

        public List<PermanantShiftChangeList> GetShiftChangeList()
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("Select Top 100 * from VwPermanantShiftChangeList order by Id Desc ");

            try
            {
                var lst = context.Database.SqlQuery<PermanantShiftChangeList>(QryStr.ToString()).Select(d => new PermanantShiftChangeList()
                {
                    Id = d.Id ,
                   StaffId = d.StaffId ,
                   StaffName = d.StaffName ,
                   Department = d.Department ,
                   PatternName = d.PatternName ,
                   ShiftName = d.ShiftName ,
                   WorkingDayPattern = d.WorkingDayPattern,
                   WithEffectFrom = d.WithEffectFrom ,
                   CreatedBy = d.CreatedBy ,
                   IsGeneralShift = d.IsGeneralShift ,
                   Reason = d.Reason 

                }).ToList();

                if (lst == null)
                {
                    return new List<PermanantShiftChangeList>();
                }
                else
                {
                    return lst;
                }
            }
            catch
            {
                return new List<PermanantShiftChangeList>();
            }
        }

        public void SaveInformation(EmployeeShiftPlan data , AttendanceControlTable act)
        {
            using (var _TRAN_ = context.Database.BeginTransaction())
            {
                try
                {
                    // DEACTIVATE THE  PREVIOUS SETTINGS OF THIS EMPLOYEE.
                    var Str = new StringBuilder();
                    Str.Clear();
                    Str.Append("update EmployeeShiftPlan set IsActive=0,InUse=0 where IsActive = 1 and staffid = @StaffId");
                    context.Database.ExecuteSqlCommand(Str.ToString(), new SqlParameter("@StaffId", data.StaffId));

                    //SAVE THE NEW SETTINGS FOR THIS EMPLOYEE.
                    context.EmployeeShiftPlan.Add(data);
                    context.SaveChanges();

                    /*IF THE CHANGE IS FOR PAST DATE THEN INSERT THE RECORD INTO ATTENDANCE CONTROL TABLE FOR 
                    ATTENDANCE REPROCESSING FOR THE PARTICULAR EMPLOYEE*/
                    if(act!= null)
                    {
                        context.AttendanceControlTable.AddOrUpdate(act);
                        context.SaveChanges();
                    }
                  
                    string date1 = data.LastUpdatedDate.ToString("dd-MMM-yyyy");

                    //CALL THE CORRESPONDING  SP TO EXECUTE THE SHIFT PLAN FOR THE EMPLOYEE.
                    if (data.IsAutoShift == false && data.IsFlexiShift == false && data.IsManualShift == false)
                    {
                        if (data.IsGeneralShift == true)
                        {
                            var QryStr = new StringBuilder();
                            QryStr.Clear();
                            QryStr.Append("EXEC [dbo].[GeneralShiftPlanV1] @StaffId,@ShiftId,@DayPatternId," +
                                "@WeeklyOffId,@UseDayPattern,@date1");
                            context.Database.ExecuteSqlCommand(QryStr.ToString(),new SqlParameter("@StaffId", data.StaffId)
                                , new SqlParameter("@ShiftId", data.ShiftId), new SqlParameter("@DayPatternId", data.DayPatternId)
                                , new SqlParameter("@WeeklyOffId", data.WeeklyOffId), new SqlParameter("@UseDayPattern", data.UseDayPattern)
                                , new SqlParameter("@date1", date1));
                        }
                        else if (data.IsWeekPattern == true)
                        {
                            var QryStr = new StringBuilder();
                            QryStr.Clear();
                            QryStr.Append("EXEC[dbo].[ProcessShiftPatternV1] @StaffId,@PatternId, @UseDayPattern," +
                               "@WeeklyOffId, @DayPatternId, @date1,Null");

                            context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@StaffId", data.StaffId)
                            , new SqlParameter("@PatternId", data.PatternId), new SqlParameter("@UseDayPattern", data.UseDayPattern) 
                            ,new SqlParameter("@WeeklyOffId", data.WeeklyOffId), new SqlParameter("@DayPatternId", data.DayPatternId)
                            ,new SqlParameter("@date1", date1));
                        }
                        else if (data.IsMonthlyPattern == true)
                        {
                            var QryStr = new StringBuilder();
                            QryStr.Clear();
                            QryStr.Append("EXEC MonthlyRoistering @StaffId,@date1,@LastUpdatedShiftId,@PatternId, @NoOfDaysShift," +
                                "@UseDayPattern,@WeeklyOffId, @DayPatternId");

                            context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@StaffId", data.StaffId)
                            , new SqlParameter("@date1", date1), new SqlParameter("@LastUpdatedShiftId", data.LastUpdatedShiftId)
                            , new SqlParameter("@PatternId", data.PatternId), new SqlParameter("@NoOfDaysShift", data.NoOfDaysShift)
                            , new SqlParameter("@UseDayPattern", data.UseDayPattern), new SqlParameter("@WeeklyOffId", data.WeeklyOffId)
                            , new SqlParameter("@DayPatternId", data.DayPatternId));

                        }
                        else if (data.IsGeneralShift == false && data.IsWeekPattern == false && data.IsMonthlyPattern == false && data.UseDayPattern == true)
                        {
                            var QryStr = new StringBuilder();
                            QryStr.Clear();
                            QryStr.Append("EXEC ShiftPostingPatternWOV1 @StaffId,@date1, @LastUpdatedShiftId,@PatternId,@UseDayPattern,@DayPatternId");

                            context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@StaffId", data.StaffId)
                            , new SqlParameter("@date1", date1), new SqlParameter("@LastUpdatedShiftId", data.LastUpdatedShiftId)
                            , new SqlParameter("@PatternId", data.PatternId), new SqlParameter("@UseDayPattern", data.UseDayPattern)
                            , new SqlParameter("@DayPatternId", data.DayPatternId));
                        }
                        else
                        {
                            var QryStr = new StringBuilder();
                            QryStr.Clear();
                            QryStr.Append("EXEC ShiftPostingPatternV1 @StaffId,@date1,@LastUpdatedShiftId,@PatternId");

                            context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@StaffId", data.StaffId)
                            , new SqlParameter("@date1", date1), new SqlParameter("@LastUpdatedShiftId", data.LastUpdatedShiftId)
                            , new SqlParameter("@PatternId", data.PatternId));
                        }
                    }
                    
                    _TRAN_.Commit();

                }
                catch (Exception e)
                {
                    _TRAN_.Rollback();
                    throw e;
                }
            }
        }
        public List<StaffList> LoadDataBasedOnRandomStaff(string staffid, string stafflist, bool includetermination, string beginning, string ending)
        {
            SqlParameter[] Param = new SqlParameter[5];
            Param[0] = new SqlParameter("@staffid", staffid);
            Param[1] = new SqlParameter("@stafflist", stafflist);
            Param[2] = new SqlParameter("@includetermination", includetermination);
            Param[3] = new SqlParameter("@beginning", beginning);
            Param[4] = new SqlParameter("@ending", ending);
            string fromdate;
            string todate;
            DateTime indate = new DateTime();
            DateTime outdate = new DateTime();
            if (beginning != null && beginning != "")
            {
                indate = Convert.ToDateTime(beginning);
            }
            else
            {
                indate = DateTime.Now;
            }
            if (ending != null && ending != "")
            {
                outdate = Convert.ToDateTime(ending);
            }
            else
            {
                outdate = DateTime.Now;
            }
            fromdate = indate.ToString("yyyy-MM-dd");
            todate = outdate.ToString("yyyy-MM-dd");

            try
            {
                var lst = context.Database.SqlQuery<StaffList>("Select staffid , DBO.FNGETSTAFFNAME(STAFFID) as staffname , " +
                    "DBO.FNGETMASTERNAME(STAFFID , 'DP') as Department, DBO.FNGETMASTERNAME(STAFFID , 'L') as Location from " +
                    "StaffOfficial a inner join Staff b on a.staffid = b.id WHERE 1=1 AND b.IsHidden = 0 and Staffid in ('"+ stafflist + "')").ToList();

                if (lst == null)
                {
                    return new List<StaffList>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<StaffList>();
            }
        }
    }
}
