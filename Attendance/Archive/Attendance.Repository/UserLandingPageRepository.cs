using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

namespace Attendance.Repository
{
    public class UserLandingPageRepository : IDisposable
    {
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        bool disposed = false;
        

        //rajesh
        public List<LeaveBalanceList> ShowLeaveBalanceTable(string id)
        {
            var QryStr = new StringBuilder();
            QryStr.Append("SELECT LeaveTypeId , LEAVETYPENAME , LEAVEBALANCE , ISCOMMON , ISPERMISSION FROM ALLOWEDLEAVES WHERE leavetypeid not in('LV0036','LV0039') and STAFFID = '" + id + "'");

            try
            {
                var lst = context.Database.SqlQuery<LeaveBalanceList>(QryStr.ToString()).Select(d => new LeaveBalanceList()
                {
                    LeaveTypeId = d.LeaveTypeId, //Rajesh for mobile
                    LeaveTypeName = d.LeaveTypeName,
                    LeaveBalance = d.LeaveBalance <0 ? 0: d.LeaveBalance
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

        public TodaysPunchesDashBoard_DAIMLER GetTodaysPunchDashBoardForMobile(string id, DateTime punchDate)
        {
            string qryStr = string.Empty;

            var txnDate = punchDate.ToString("yyyy-MMM-dd");
            qryStr = $@"SELECT * FROM  dbo.fnGetTodaysPunchDashBoard_Mobile('{id}','{txnDate}')";

            try
            {
                var lst = context.Database.SqlQuery<TodaysPunchesDashBoard_DAIMLER>(qryStr).Select(d => new TodaysPunchesDashBoard_DAIMLER()
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
                });

                return lst.First();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<TeamAttendance> GetTeamAttendanceFor(string reportingManagerId,string workedFromDate,string workedToDate)
        {
            string qryStr = string.Empty;

          

            try
            {
                qryStr = "exec [DBO].[GetTeamAttendance] '" + reportingManagerId + "','" + @workedFromDate + "','" + @workedToDate + "'";
                var lst = context.Database.SqlQuery<TeamAttendance>(qryStr); ;
                return lst.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
           
        

        public UserLandingPageRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<FirstInLastOutNew> GetFirstInLastOutDashBoard(string id, string FromDate , string Todate, bool Default)
        {
            if(Default == true)
            {
                Todate = DateTime.Now.ToString("dd-MMM-yyyy");
                FromDate = Convert.ToDateTime(Todate).AddDays(-7).ToString("dd-MMM-yyyy");
            }

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@id", id);
            param[1] = new SqlParameter("@FromDate", FromDate);
            param[2] = new SqlParameter("@Todate", Todate);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM dbo.fnGetFirstInLastOutNew ( @id, @FromDate, @Todate) ");

            try
            {
                var lst = context.Database.SqlQuery<FirstInLastOutNew>(qryStr.ToString(), param).Select(d => new FirstInLastOutNew()
                {
                    Id = d.Id,
                    StaffName = d.StaffName ,
                    Team = d.Team ,
                    Plant = d.Plant ,
                    Department = d.Department ,
                    Designation = d.Designation ,
                    Grade = d.Grade ,
                    TxnDate = d.TxnDate ,
                    Shift = d.Shift,
                    InTime = d.InTime ,
                    InReader = d.InReader ,
                    OutTime = d.OutTime ,
                    OutReader = d.OutReader ,
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
            catch(Exception)
            {
                return new List<FirstInLastOutNew>();
            }

        }
        //Rajesh
        /**
        public List<LeaveBalanceList> ShowLeaveBalanceTable(string id)
        {
            var QryStr = new StringBuilder();
            QryStr.Append("SELECT LeaveTypeId , LEAVETYPENAME , LEAVEBALANCE , ISCOMMON , ISPERMISSION FROM ALLOWEDLEAVES WHERE leavetypeid not in('LV0036','LV0039') and STAFFID = @id");

            try
            {
                var lst = context.Database.SqlQuery<LeaveBalanceList>(QryStr.ToString(),new SqlParameter("@id", id)).Select(d => new LeaveBalanceList()
                {
                    LeaveTypeName = d.LeaveTypeName,
                    LeaveBalance = d.LeaveBalance
                }).ToList();

                if(lst == null)
                {
                    return new List<LeaveBalanceList>();
                }
                else
                {
                    return lst;
                }
            }
            catch(Exception)
            {
                return new List<LeaveBalanceList>();
            }

        }
        **/
        public List<TodaysPunchesDashBoard_DAIMLER> GetTodaysPunchDashBoard(string id)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM DBO.fnGetTodaysPunchDashBoard(@id)");

            try
            {
                var lst = context.Database.SqlQuery<TodaysPunchesDashBoard_DAIMLER>(qryStr.ToString(),new SqlParameter("@id", id)).Select(d => new TodaysPunchesDashBoard_DAIMLER()
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

                if(lst == null)
                {
                    return new List<TodaysPunchesDashBoard_DAIMLER>();
                }
                else
                {
                    return lst;
                }
            }
            catch(Exception)
            {
                return new List<TodaysPunchesDashBoard_DAIMLER>();
            }
        }

        public void Dispose()
        {
            ((IDisposable)handle).Dispose();
        }
    }
}
