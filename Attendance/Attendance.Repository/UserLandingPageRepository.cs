using Attendance.Model;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Attendance.Repository
{
    public class UserLandingPageRepository : IDisposable
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
        public List<LeaveBalanceList> ShowLeaveBalanceTable(string StaffId)
        {
            var QryStr = new StringBuilder();
            QryStr.Append("Exec [Dbo].[GetAllLeaveBalance] @StaffId");

            try
            {
                var lst = context.Database.SqlQuery<LeaveBalanceList>(QryStr.ToString() , new SqlParameter("@StaffId", StaffId)).Select(d => new LeaveBalanceList()
                {
                    LeaveTypeId = d.LeaveTypeId, //Rajesh for mobile
                    LeaveTypeName = d.LeaveTypeName,
                    LeaveBalance = d.LeaveBalance < 0 ? 0 : d.LeaveBalance
                }).ToList();

                if (lst == null)
                {
                    return new List<LeaveBalanceList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LeaveBalanceList>();
            }

        }

        private AttendanceManagementContext context = null;


        public UserLandingPageRepository()
        {
            context = new AttendanceManagementContext();
        }
        StringBuilder builder = new StringBuilder();
        #region Birthday Remainder
        public int GetSelfBirthDayRepository(string StaffId)
        {
            int BDayCount = 0;
            try
            {
                builder = new StringBuilder();
                builder.Append("Select Count(Isnull(SP.DateOfBirth,'0')) from StaffPersonal SP inner join Staff S on " +
                    "S.Id=SP.StaffId Where StaffStatusId=1 and format(DateOfBirth,'MM-dd')=format(getdate(),'MM-dd') and " +
                    "StaffId=@StaffId");
                BDayCount = context.Database.SqlQuery<int>(builder.ToString(), new SqlParameter("@StaffId", StaffId)).FirstOrDefault();
            }
            catch (Exception e)
            {
                string Message = e.Message;
            }
            return BDayCount;
        }
        #endregion
        public List<TeamAttendance> GetTeamAttendanceFor(string reportingManagerId, string fromDate, string toDate)
        {
            string qryStr = string.Empty;
            try
            {
                qryStr = "Exec [DBO].[GetTeamAttendance] @ReportimgManagerId , @FromDate , @ToDate";
                var lst = context.Database.SqlQuery<TeamAttendance>(qryStr , new SqlParameter("@ReportimgManagerId", reportingManagerId) , 
                    new SqlParameter("@FromDate",fromDate) , new SqlParameter("@ToDate", toDate)).ToList();
                return lst; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<FirstInLastOutNew> GetFirstInLastOutDashBoard(string id, string FromDate, string Todate, bool Default)
        {
            if (Default == true)
            {
                Todate = DateTime.Now.ToString("dd-MMM-yyyy");
                FromDate = Convert.ToDateTime(Todate).AddDays(-7).ToString("dd-MMM-yyyy");
            }

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM dbo.fnGetFirstInLastOutNew ( @id, @FromDate, @Todate) ");

            try
            {
                var lst = context.Database.SqlQuery<FirstInLastOutNew>(qryStr.ToString(), new SqlParameter("@id", id)
                    , new SqlParameter("@FromDate", FromDate), new SqlParameter("@Todate", Todate)).Select(d => new FirstInLastOutNew()
                    {
                        Id = d.Id,
                        StaffName = d.StaffName,
                        Team = d.Team,
                        Plant = d.Plant,
                        Department = d.Department,
                        Designation = d.Designation,
                        Grade = d.Grade,
                        TxnDate = d.TxnDate,
                        Shift = d.Shift,
                        InTime = d.InTime,
                        InReader = d.InReader,
                        OutTime = d.OutTime,
                        OutReader = d.OutReader,
                        TotalHoursWorked = d.TotalHoursWorked,
                        AttendanceStatus = d.AttendanceStatus
                    }).ToList();

                if (lst == null)
                {
                    return new List<FirstInLastOutNew>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<FirstInLastOutNew>();
            }

        }

        public List<LeaveBalanceList> ShowLeaveBalanceTable(string id, string Gender)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT LeaveTypeId , LeaveTypeName , LeaveBalance FROM ALLOWEDLEAVES WHERE STAFFID = @id ");
            if (Gender == "M")
            {
                QryStr.Append(" and LeaveTypeId not in('LV0006')");
            }
            else
            {
                QryStr.Append(" and LeaveTypeId not in('LV0007')");
            }
            try
            {
                var lst = context.Database.SqlQuery<LeaveBalanceList>(QryStr.ToString(), new SqlParameter("@id", id)).Select(d => new LeaveBalanceList()
                {
                    LeaveTypeName = d.LeaveTypeName,
                    LeaveBalance = d.LeaveBalance
                }).ToList();

                if (lst == null)
                {
                    return new List<LeaveBalanceList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LeaveBalanceList>();
            }

        }

        public List<TodaysPunchesDashBoardForMobile> GetTodaysPunchDashBoard(string id)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM DBO.fnGetTodaysPunchDashBoard(@id)");

            try
            {
                var lst = context.Database.SqlQuery<TodaysPunchesDashBoardForMobile>(qryStr.ToString(), new SqlParameter("@id", id)).Select(d => new TodaysPunchesDashBoardForMobile()
                {
                    StaffId = d.StaffId,
                    ShiftIn = d.ShiftIn,
                    ShiftOut = d.ShiftOut,
                    SwipeIn = d.SwipeIn,
                    SwipeOut = d.SwipeOut,
                    LateIn = d.LateIn,
                    EarlyOut = d.EarlyOut,
                    InReaderName = d.InReaderName,
                    OutReaderName = d.OutReaderName
                }).ToList();

                if (lst == null)
                {
                    return new List<TodaysPunchesDashBoardForMobile>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<TodaysPunchesDashBoardForMobile>();
            }
        }
        public TodaysPunchesDashBoardForMobile GetTodaysPunchDashBoardForMobile(string id, DateTime punchDate)
        {
            string qryStr = string.Empty;

            var txnDate = punchDate.ToString("yyyy-MMM-dd");
            qryStr = $@"SELECT * FROM  dbo.fnGetTodaysPunchDashBoard_Mobile('{id}','{txnDate}')";

            try
            {
                var lst = context.Database.SqlQuery<TodaysPunchesDashBoardForMobile>(qryStr).Select(d => new TodaysPunchesDashBoardForMobile()
                {
                    StaffId = d.StaffId,
                    ShiftIn = d.ShiftIn,
                    ShiftOut = d.ShiftOut,
                    SwipeIn = d.SwipeIn,
                    SwipeOut = d.SwipeOut,
                    LateIn = d.LateIn,
                    EarlyOut = d.EarlyOut,
                    InReaderName = d.InReaderName,
                    OutReaderName = d.OutReaderName,
                    SlideMode = d.SlideMode
                });
                return lst.First();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public void Dispose()
        //{
        //    ((IDisposable)handle).Dispose();
        //}
    }
}
