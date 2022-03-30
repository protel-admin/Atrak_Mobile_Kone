using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using Attendance.Model;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

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
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT DBO.FNGETSTAFFNAME(STAFFID) AS StaffName , DEPTNAME AS DepartmentName FROM STAFFVIEW WHERE STAFFID = @StaffId");
            try
            {

                var data = context.Database.SqlQuery<StaffView>(QryStr.ToString(),sqlParameter).Select(d => new StaffView()
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
                   //PatternName = d.PatternName ,
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
                    context.Database.ExecuteSqlCommand("update EmployeeShiftPlan set IsActive=0 where IsActive = 1 and staffid = '" + data.StaffId + "' ");

                    //Changes made by Aarthi on 11/03/2020
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
                    if (data.IsGeneralShift == true)
                    {
                        context.Database.ExecuteSqlCommand("EXEC [dbo].[GeneralShiftPlanV1] '" + data.StaffId + "' , '" + data.ShiftId + "' , " + data.DayPatternId + " , '" + data.WeeklyOffId + "' , " + data.UseDayPattern + " , '" + date1 + "' ");
                    }
                    else if (data.IsWeekPattern == true)
                    {

                       context.Database.ExecuteSqlCommand("EXEC [dbo].[ProcessShiftPatternV1] '" + data.StaffId + "' , '" + data.PatternId + "' , " + data.UseDayPattern + " , '" + data.WeeklyOffId + "' , '" + data.DayPatternId + "' , '" + date1 + "' , Null");
                    }
                    else if  (data.IsMonthlyPattern == true)
                    {
                        context.Database.ExecuteSqlCommand("EXEC MonthlyRoistering '" + data.StaffId + "',  '" + date1 + "' , '" + data.LastUpdatedShiftId + "','" + data.PatternId + "', '" + data.NoOfDaysShift + "', '" + data.UseDayPattern + "' ,'" + data.WeeklyOffId + "' , '" + data.DayPatternId + "' "); 

                    }
                    else if (data.IsGeneralShift == false && data.IsWeekPattern == false && data.IsMonthlyPattern == false && data.UseDayPattern == true)
                    {
                        context.Database.ExecuteSqlCommand("EXEC ShiftPostingPatternWOV1 '" + data.StaffId + "',  '" + date1 + "' , '" + data.LastUpdatedShiftId + "','" + data.PatternId + "','"+data.UseDayPattern+"' , '"+ data.DayPatternId +"' ");
                    }
                    else
                    {
                        context.Database.ExecuteSqlCommand("EXEC ShiftPostingPatternV1 '" + data.StaffId + "',  '" + date1 + "' , '" + data.LastUpdatedShiftId + "','" + data.PatternId + "' ");
                    }
                    _TRAN_.Commit();

                }
                catch (Exception)
                {
                    _TRAN_.Rollback();
                    throw;
                }
            }
        }
    }
}
