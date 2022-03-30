using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class ReportRepository : IDisposable
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

        public ReportRepository()
        {
            context = new AttendanceManagementContext();
        }
        StringBuilder builder = new StringBuilder();
        public List<MOffApplicationReport> GetMOffApplicationReport(string beginningdate, string endingdate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetMaintenanceOffList ( '" + stafflist + "','" + beginningdate + "','" + endingdate + "' ) ORDER BY STAFFID , TXNDATE");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<MOffApplicationReport>(qryStr.ToString()).Select(d => new MOffApplicationReport()
                {
                    STAFFID = d.STAFFID,
                    STAFFNAME = d.STAFFNAME,
                    TXNDATE = d.TXNDATE,
                    LEAVESHORTNAME = d.LEAVESHORTNAME,
                    MAINTENANCEOFFREASON = d.MAINTENANCEOFFREASON,
                    APPROVALSTATUSNAME = d.APPROVALSTATUSNAME,
                    APPROVALSTAFFID = d.APPROVALSTAFFID,
                    APPROVALSTAFFNAME = d.APPROVALSTAFFNAME,
                    APPROVEDONDATE = d.APPROVEDONDATE,
                    APPROVEDONTIME = d.APPROVEDONTIME,
                    APPROVALOWNWERID = d.APPROVALOWNWERID,
                    APPROVALOWNERNAME = d.APPROVALOWNERNAME
                }).ToList();

                if (lst == null)
                {
                    return new List<MOffApplicationReport>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<MOffApplicationReport>();
            }
        }

        public List<LeaveApplicationDeatails> GetLeaveNewApplicationReport(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            context.Database.CommandTimeout = 0;
            qryStr.Append("SELECT * FROM [dbo].[fnGetLeaveApplications] ( '" + fromdate + "' , '" + todate + "'  , '" + stafflist + "'  )");
            try
            {
                var lst = context.Database.SqlQuery<LeaveApplicationDeatails>(qryStr.ToString()).Select(d => new LeaveApplicationDeatails()
                {
                    STAFFID = d.STAFFID,
                    STAFFNAME = d.STAFFNAME,
                    PLANT = d.PLANT,
                    TEAM = d.TEAM,
                    DEPARTMENTNAME = d.DEPARTMENTNAME,
                    DESIGNATION = d.DESIGNATION,
                    STARTDATE = d.STARTDATE,
                    STARTDURATION = d.STARTDURATION,
                    ENDDATE = d.ENDDATE,
                    ENDDURATION = d.ENDDURATION,
                    CANCELLED = d.CANCELLED,
                    REMARKS = d.REMARKS,
                    REASON = d.REASON,
                    TOTALDAYS = d.TOTALDAYS,
                    APPROVALSTATUSNAME = d.APPROVALSTATUSNAME,
                    APPROVEDBY = d.APPROVEDBY,
                    APPLTYPE = d.APPLTYPE,
                    APPLICATIONDATE = d.APPLICATIONDATE
                }).ToList();

                if (lst == null)
                {
                    return new List<LeaveApplicationDeatails>();
                }
                else
                {
                    return lst;
                }

            }
            catch (Exception)
            {
                return new List<LeaveApplicationDeatails>();
            }

            return null;
        }


        public List<LeaveApplicationReport> GetLeaveApplicationReport(string beginningdate, string endingdate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetLeaveApplications ( '" + stafflist + "','" + beginningdate + "','" + endingdate + "' ) ORDER BY STAFFID , TXNDATE");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<LeaveApplicationReport>(qryStr.ToString()).Select(d => new LeaveApplicationReport()
                {
                    STAFFID = d.STAFFID,
                    STAFFNAME = d.STAFFNAME,
                    TXNDATE = d.TXNDATE,
                    LEAVETYPENAME = d.LEAVETYPENAME,
                    LEAVEAPPLICATIONREASON = d.LEAVEAPPLICATIONREASON,
                    APPROVALSTATUSNAME = d.APPROVALSTATUSNAME,
                    APPROVALSTAFFID = d.APPROVALSTAFFID,
                    APPROVALSTAFFNAME = d.APPROVALSTAFFNAME,
                    APPROVEDONDATE = d.APPROVEDONDATE,
                    APPROVEDONTIME = d.APPROVEDONTIME,
                    APPROVALOWNWERID = d.APPROVALOWNWERID,
                    APPROVALOWNERNAME = d.APPROVALOWNERNAME
                }).ToList();

                if (lst == null)
                {
                    return new List<LeaveApplicationReport>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LeaveApplicationReport>();
            }
        }



        public List<CanteenReport> GetCanteenReport(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT Empcode ,Name ,Plant ,Department,Designation,Branch,Division,Grade  ,Indate ,InTime   from fnCanteenReport ('" + fromdate + "','" + todate + "')");
            //STAFFID,STAFFNAME,STARTDATE,LEAVESTARTDURATION,ENDDATE,LEAVEENDDURATION,CANCELLED,REMARKS,REASON,TOTALDAYS,APPROVALSTATUSNAME,APPROVEDBY,APPLTYPE,APPLICATIONDATE
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<CanteenReport>(qryStr.ToString()).Select(d => new CanteenReport()
                {
                    Empcode = d.Empcode,
                    Name = d.Name,
                    Plant = d.Plant,
                    Department = d.Department,
                    Designation = d.Designation,
                    Branch = d.Branch,
                    Division = d.Division,
                    Grade = d.Grade,
                    Indate = d.Indate,
                    InTime = d.InTime


                }).ToList();

                if (lst == null)
                {
                    return new List<CanteenReport>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<CanteenReport>();
            }
        }
        public List<PresentOnNFH> GetPresentOnNFH(string beginningdate, string endingdate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetPresentOnNFH ( '" + stafflist + "','" + beginningdate + "','" + endingdate + "' )");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<PresentOnNFH>(qryStr.ToString()).Select(d => new PresentOnNFH()
                {
                    STAFFID = d.STAFFID,
                    STAFFNAME = d.STAFFNAME,
                    TXNDATE = d.TXNDATE,
                    PRESENT = d.PRESENT,
                    WEEKLYOFF = d.WEEKLYOFF,
                    PAIDHOLIDAY = d.PAIDHOLIDAY,
                    COL1 = d.COL1,
                    COL2 = d.COL2,
                    COL3 = d.COL3,
                    COL4 = d.COL4,
                    COL5 = d.COL5,
                    COL6 = d.COL6,
                    COL7 = d.COL7,
                    COL8 = d.COL8,
                    COL9 = d.COL9
                }).ToList();

                if (lst == null)
                {
                    return new List<PresentOnNFH>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<PresentOnNFH>();
            }
        }

        public List<OnDutyReportModel> GetOutDoorStatement(string beginningdate, string endingdate, string stafflist)
        {
            stafflist = stafflist.Replace("','", ",");
            stafflist = stafflist.Replace("'", "");
            SqlParameter[] Param = new SqlParameter[3];
            Param[0] = new SqlParameter("@beginningdate", beginningdate);
            Param[1] = new SqlParameter("@endingdate", endingdate);
            Param[2] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Append("Exec [Dbo].[GetOnDutyRequisitionHistory] @stafflist,@beginningdate,@endingdate");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<OnDutyReportModel>(qryStr.ToString(), Param).ToList();

                if (lst == null)
                {
                    return new List<OnDutyReportModel>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<OnDutyReportModel>();
            }
        }

        public List<DailyPerformance> GetDailyPerformance(string fromdate, string todate, string stafflist)
        {
            SqlParameter[] Param = new SqlParameter[3];
            Param[0] = new SqlParameter("@fromdate", fromdate);
            Param[1] = new SqlParameter("@todate", todate);
            Param[2] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select * from fnGetDailyPerformanceReport (@stafflist,@fromdate,@todate )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<DailyPerformance>(qryStr.ToString(), Param).ToList();

                if (lst == null)
                {
                    return new List<DailyPerformance>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<DailyPerformance>();
            }
        }

        public List<PresentAndAbsentCountReport> GetPresentAndAbsentReport(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select STAFFID , FirstName , PresentCount , AbsentCount , Department , HalfDayCount " +
                          "from [dbo].[fnGetPresentAndAbsentCount] ( '" + stafflist + "' , '" + fromdate + "' , '" + todate + "' )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst =
                    context.Database.SqlQuery<PresentAndAbsentCountReport>(qryStr.ToString()).Select(d => new PresentAndAbsentCountReport()
                    {
                        StaffId = d.StaffId,
                        FirstName = d.FirstName,
                        Department = d.Department,
                        PresentCount = d.PresentCount,
                        AbsentCount = d.AbsentCount,
                        HalfDayCount = d.HalfDayCount
                    }).ToList();

                if (lst == null)
                {
                    return new List<PresentAndAbsentCountReport>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<PresentAndAbsentCountReport>();
            }
        }

        public List<ShiftAllowanceReport> GetShiftAllowanceReport(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select STAFFID , FirstName , Department ,Designation, SecondShift , NightShift " +
                          "from [dbo].[fnGetShiftAllowanceReport] ( '" + stafflist + "' , '" + fromdate + "' , '" + todate + "' )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst =
                    context.Database.SqlQuery<ShiftAllowanceReport>(qryStr.ToString()).Select(d => new ShiftAllowanceReport()
                    {
                        StaffId = d.StaffId,
                        FirstName = d.FirstName,
                        Department = d.Department,
                        Designation = d.Designation,
                        SecondShift = d.SecondShift,
                        NightShift = d.NightShift
                    }).ToList();

                if (lst == null)
                {
                    return new List<ShiftAllowanceReport>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ShiftAllowanceReport>();
            }
        }

        public List<AttendanceIncentive> GetAttendanceIncentiveReport(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select STAFFID , FirstName , DeptName , DesignationName , AbsentCount , PresentCount, HalfDayCount " +
                          "from [dbo].[fnGetAttendanceIncentiveReport] ( '" + stafflist + "' , '" + fromdate + "' , '" + todate + "' )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst =
                    context.Database.SqlQuery<AttendanceIncentive>(qryStr.ToString()).Select(d => new AttendanceIncentive()
                    {
                        StaffId = d.StaffId,
                        FirstName = d.FirstName,
                        DeptName = d.DeptName,
                        DesignationName = d.DesignationName,
                        AbsentCount = d.AbsentCount,
                        PresentCount = d.PresentCount,
                        HalfDayCount = d.HalfDayCount
                    }).ToList();

                if (lst == null)
                {
                    return new List<AttendanceIncentive>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<AttendanceIncentive>();
            }
        }


        public List<GetHeadCountAlertDetails> GetHeadCountAlertReport(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select convert(varchar,max(Tr_Date)) as TransactionDate,Tr_chid as StaffId,Tr_TType, B.FirstName,B.DeptName as DepartmentName,B.DesignationName,B.CategoryName,B.BranchName" +
                " from SMaxTransaction A inner join staffview B on A.Tr_ChId=B.StaffId" +
                " where convert(date,Tr_Date) = convert(date,getdate()) and tr_ttype=20 " +
                "group by tr_chid,Tr_ttype,B.firstname,B.DeptName,B.DesignationName,B.CategoryName,B.BranchName");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst =
                    context.Database.SqlQuery<GetHeadCountAlertDetails>(qryStr.ToString()).Select(d => new GetHeadCountAlertDetails()
                    {
                        StaffId = d.StaffId,
                        FirstName = d.FirstName,
                        DepartmentName = d.DepartmentName,
                        DesignationName = d.DesignationName,
                        TransactionDate = d.TransactionDate,
                        CategoryName = d.CategoryName,
                        BranchName = d.BranchName
                    }).ToList();

                if (lst == null)
                {
                    return new List<GetHeadCountAlertDetails>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<GetHeadCountAlertDetails>();
            }
        }

        public List<BasicDetails> GetBasicDetailsReport(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select StaffId,Firstname,DateofJoining,Gender,BranchName,DeptName,OfficialPhone, officalEmail as OfficialEmail," +
                          " (select name from MaritalStatus where id=A.MaritalStatus) as MaritalStatus,(select name from bloodgroup where id=A.BloodGroup) as BloodGroup," +
                          "DesignationName,CategoryName,LocationName,HomeLocation,HomeCity,HomeDistrict,HomeState,HomeCountry,DateofBirth,PersonalBankName," +
                          "PersonalBankAccount, PersonalBankIFSCCode,PersonalBankBranch,AadharNo from staffview A where staffid='" + stafflist + "'");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst =
                    context.Database.SqlQuery<BasicDetails>(qryStr.ToString()).Select(d => new BasicDetails()
                    {
                        StaffId = d.StaffId,
                        Firstname = d.Firstname,
                        DateofJoining = d.DateofJoining,
                        Gender = d.Gender,
                        BranchName = d.BranchName,
                        DeptName = d.DeptName,
                        OfficialPhone = d.OfficialPhone,
                        OfficialEmail = d.OfficialEmail,
                        MaritalStatus = d.MaritalStatus,
                        BloodGroup = d.BloodGroup,
                        DesignationName = d.DesignationName,
                        CategoryName = d.CategoryName,
                        LocationName = d.LocationName,
                        HomeLocation = d.HomeLocation,
                        HomeCity = d.HomeCity,
                        HomeDistrict = d.HomeDistrict,
                        HomeState = d.HomeState,
                        HomeCountry = d.HomeCountry,
                        DateofBirth = d.DateofBirth,
                        PersonalBankName = d.PersonalBankName,
                        PersonalBankAccount = d.PersonalBankAccount,
                        PersonalBankIFSCCode = d.PersonalBankIFSCCode,
                        PersonalBankBranch = d.PersonalBankBranch,
                        AadharNo = d.AadharNo
                    }).ToList();

                if (lst == null)
                {
                    return new List<BasicDetails>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<BasicDetails>();
            }
        }

        public List<PresentList> GetAttendanceLists(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select StaffId , FirstName , ShiftId , ShiftName , ShiftInDate , CompanyName , DepartmentName ,Designation," +
                           "GradeName , ActualInDate , ActualInTime , " +
                           "ActualOutDate , ActualOutTime , ActualWorkedHours , AttendanceStatus , DateOfJoining , DateOfResignation " +
                           "from fnGetPresentList ('" + fromdate + "','" + todate + "','" + stafflist + "','') ");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<PresentList>(qryStr.ToString()).Select(d => new PresentList()
                {
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    ShiftId = d.ShiftId,
                    ShiftName = d.ShiftName,
                    CompanyName = d.CompanyName,
                    DepartmentName = d.DepartmentName,
                    Designation = d.Designation,
                    GradeName = d.GradeName,
                    ShiftInDate = d.ShiftInDate,
                    ActualInDate = d.ActualInDate,
                    ActualInTime = d.ActualInTime,
                    ActualOutDate = d.ActualOutDate,
                    ActualOutTime = d.ActualOutTime,
                    ActualWorkedHours = d.ActualWorkedHours,
                    AttendanceStatus = d.AttendanceStatus,
                    DateOfJoining = d.DateOfJoining,
                    DateOfResignation = d.DateOfResignation
                }).ToList();

                if (lst == null)
                {
                    return new List<PresentList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<PresentList>();
            }
        }

        public List<PresentList> GetAbsentLists(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("select StaffId,FirstName,DepartmentName,Category,Designation,AttendanceStatus,TransactionDate as ShiftInDate from fnGetAbsentListV1 ('" + stafflist + "','" + fromdate + "','" + todate + "')");
            //qryStr.Append( "select StaffId , FirstName , ShiftId , ShiftName , ShiftInDate , CompanyName , DepartmentName ,Designation, " +
            //               "GradeName , ActualInDate , ActualInTime , " +
            //               "ActualOutDate , ActualOutTime , ActualWorkedHours , AttendanceStatus  , DateOfJoining, DateOfResignation " +
            //               "from fnGetPresentList ('" + fromdate + "','" + todate + "','" + stafflist + "','AB,HD') " );

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<PresentList>(qryStr.ToString()).Select(d => new PresentList()
                {
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    DepartmentName = d.DepartmentName,
                    Designation = d.Designation,
                    AttendanceStatus = d.AttendanceStatus,
                    ShiftInDate = d.ShiftInDate
                }).ToList();

                if (lst == null)
                {
                    return new List<PresentList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<PresentList>();
            }
        }

        public List<PresentList> GetPresentLists(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select StaffId,FirstName,DepartmentName,Category,Designation,AttendanceStatus,TransactionDate as ShiftInDate from fnGetpresentListV1 ('" + stafflist + "','" + fromdate + "','" + todate + "')");
            //qryStr.Append("select StaffId , FirstName , ShiftId , ShiftName , ShiftInDate , CompanyName , DepartmentName , Designation, " +
            //               "GradeName , ActualInDate , ActualInTime , " +
            //               "ActualOutDate , ActualOutTime , ActualWorkedHours , AttendanceStatus , DateOfJoining, DateOfResignation " +
            //               "from fnGetPresentList ('" + fromdate + "','" + todate + "','" + stafflist + "','PR') ");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<PresentList>(qryStr.ToString()).Select(d => new PresentList()
                {
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    ShiftId = d.ShiftId,
                    ShiftName = d.ShiftName,
                    CompanyName = d.CompanyName,
                    Category = d.Category,
                    DepartmentName = d.DepartmentName,
                    Designation = d.Designation,
                    GradeName = d.GradeName,
                    ShiftInDate = d.ShiftInDate,
                    ActualInDate = d.ActualInDate,
                    ActualInTime = d.ActualInTime,
                    ActualOutDate = d.ActualOutDate,
                    ActualOutTime = d.ActualOutTime,
                    ActualWorkedHours = d.ActualWorkedHours,
                    AttendanceStatus = d.AttendanceStatus,
                    DateOfJoining = d.DateOfJoining,
                    DateOfResignation = d.DateOfResignation
                }).ToList();

                if (lst == null)
                {
                    return new List<PresentList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<PresentList>();
            }
        }

        public List<PunchDetails> GetPunchDetails(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            //qryStr.Append("select StaffId , FirstName , CompanyName , BranchName , DeptName , ActualInDate , " +
            //              "ActualInTime , ActualOutTime , ActualWorkedHours , OTHours from vwAttendanceDetails " +
            //              "WHERE STAFFID IN ( "+stafflist+") " +
            //              "AND ActualInDate BETWEEN '"+fromdate+"' AND '"+todate+"'");

            stafflist = stafflist.Replace("','", ",");
            stafflist = stafflist.Replace("'", "");
            qryStr.Append("select * from  dbo.fnGetPunchDetails ( '" + stafflist + "','" + fromdate + "','" + todate + "' )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<PunchDetails>(qryStr.ToString()).Select(d => new PunchDetails()
                {
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    CompanyName = d.CompanyName,
                    BranchName = d.BranchName,
                    DeptName = d.DeptName,
                    TxnDate = d.TxnDate,
                    ActualInDate = d.ActualInDate,
                    ActualInTime = d.ActualInTime,
                    ActualOutDate = d.ActualOutDate,
                    ActualOutTime = d.ActualOutTime,
                    ActualWorkedHours = d.ActualWorkedHours,
                    OTHours = d.OTHours,
                    AbsentStatus = d.AbsentStatus
                }).ToList();

                if (lst == null)
                {
                    return new List<PunchDetails>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<PunchDetails>();
            }

        }

        public List<FirstInLastOutDiamlerNew> GetFirstInLastOuts(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            //qryStr.Append("select Id , FirstName , CompanyName , BranchName , DeptName , DivisionName , GradeName , SwipeDate , FirstInTime , InReaderName , LastOutTime , OutReaderName , TotalHoursWorked from dbo.fnGetFirstInLastOut('" + stafflist + "','" + fromdate + "','" + todate + "')");
            qryStr.Append("Select Id,STAFFID,STAFFNAME,PLANT,TEAM ,DEPARTMENT,DESIGNATION ,DIVISON ,GRADE,SHIFT,TXNDATE,INTIME ,INREADER ,OUTTIME ,OUTREADER ,LATEIN ,EARLYEXIT ,TOTALHOURSWORKED,AttendanceStatus from dbo.fnGetFirstInLastOutNewV1('" + stafflist + "','" + fromdate + "','" + todate + "')");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<FirstInLastOutDiamlerNew>(qryStr.ToString()).Select(d => new FirstInLastOutDiamlerNew()
                {
                    Id = d.Id,
                    STAFFID = d.STAFFID,
                    STAFFNAME = d.STAFFNAME,
                    PLANT = d.PLANT,
                    TEAM = d.TEAM,
                    DEPARTMENT = d.DEPARTMENT,
                    DESIGNATION = d.DESIGNATION,
                    DIVISON = d.DIVISON,
                    GRADE = d.GRADE,
                    SHIFT = d.SHIFT,
                    TXNDATE = d.TXNDATE,
                    INTIME = d.INTIME,
                    INREADER = d.INREADER,
                    OUTTIME = d.OUTTIME,
                    OUTREADER = d.OUTREADER,
                    LATEIN = d.LATEIN,
                    EARLYEXIT = d.EARLYEXIT,
                    TOTALHOURSWORKED = d.TOTALHOURSWORKED,
                    AttendanceStatus = d.AttendanceStatus
                    //Id = d.Id,
                    //FirstName = d.FirstName,
                    //CompanyName = d.CompanyName,
                    //BranchName = d.BranchName,
                    //DeptName = d.DeptName,
                    //Designation = d.Designation ,
                    //DivisionName = d.DivisionName,
                    //GradeName = d.GradeName,
                    //SwipeDate = d.SwipeDate,
                    //FirstInTime = d.FirstInTime ,
                    //InReaderName = d.InReaderName,
                    //LastOutTime = d.LastOutTime,
                    //OutReaderName = d.OutReaderName,
                    //TotalHoursWorked = d.TotalHoursWorked
                }).ToList();

                if (lst == null)
                {
                    return new List<FirstInLastOutDiamlerNew>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<FirstInLastOutDiamlerNew>();
            }
        }

        public List<RawPunchDetails> GetRawPunchDetailsReport(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select AccessType,StaffId , FirstName , DeptName ,Designation, GradeName ,Division , Volume , SwipeDate , SwipeTime , InOut , ReaderName from fnGetRawPunchDetails('" + fromdate + "','" + todate + "','" + stafflist + "')");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<RawPunchDetails>(qryStr.ToString()).Select(d => new RawPunchDetails()
                {
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    DeptName = d.DeptName,
                    Designation = d.Designation,
                    Division = d.Division,
                    Volume = d.Volume,
                    GradeName = d.GradeName,
                    SwipeDate = d.SwipeDate,
                    SwipeTime = d.SwipeTime,
                    InOut = d.InOut,
                    ReaderName = d.ReaderName,
                    AccessType = d.AccessType
                }).ToList();

                if (lst == null)
                {
                    return new List<RawPunchDetails>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<RawPunchDetails>();
            }
        }

        public List<Form25FLSmidth> GetForm25FLSmidth(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec spGetForm25_FLSmidth '" + fromdate + "' , '" + todate + "'	, '" + stafflist + "'");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<Form25FLSmidth>(qryStr.ToString()).Select(d => new Form25FLSmidth()
                {
                    RecId = d.RecId,
                    Id = d.Id,
                    StaffId = d.StaffId,
                    NameOfWorker = d.NameOfWorker,
                    FatherName = d.FatherName,
                    Designation = d.Designation,
                    BirthDay = d.BirthDay,
                    BirthMonth = d.BirthMonth,
                    BirthYear = d.BirthYear,
                    PlaceOfEmployment = d.PlaceOfEmployment,
                    GroupNo = d.GroupNo,
                    RelayNo = d.RelayNo,
                    PeriodOfEmployment = d.PeriodOfEmployment,
                    PeriodOfWork = d.PeriodOfWork,
                    Day1 = d.Day1,
                    Day2 = d.Day2,
                    Day3 = d.Day3,
                    Day4 = d.Day4,
                    Day5 = d.Day5,
                    Day6 = d.Day6,
                    Day7 = d.Day7,
                    Day8 = d.Day8,
                    Day9 = d.Day9,
                    Day10 = d.Day10,
                    Day11 = d.Day11,
                    Day12 = d.Day12,
                    Day13 = d.Day13,
                    Day14 = d.Day14,
                    Day15 = d.Day15,
                    Day16 = d.Day16,
                    Day17 = d.Day17,
                    Day18 = d.Day18,
                    Day19 = d.Day19,
                    Day20 = d.Day20,
                    Day21 = d.Day21,
                    Day22 = d.Day22,
                    Day23 = d.Day23,
                    Day24 = d.Day24,
                    Day25 = d.Day25,
                    Day26 = d.Day26,
                    Day27 = d.Day27,
                    Day28 = d.Day28,
                    Day29 = d.Day29,
                    Day30 = d.Day30,
                    Day31 = d.Day31
                    //ExemptingOrder = d.ExemptingOrder ,
                    //WeeklyRest = d.WeeklyRest ,
                    //CompensatoryHolidayDate = d.CompensatoryHolidayDate ,
                    //LostRestDays = d.LostRestDays ,
                    //NoOfDaysWorked = d.NoOfDaysWorked ,
                    //LeaveWithWages = d.LeaveWithWages ,
                    //LeaveWithOutWages = d.LeaveWithOutWages ,
                    //Remarks = d.Remarks
                }).ToList();

                if (lst == null)
                {
                    return new List<Form25FLSmidth>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<Form25FLSmidth>();
            }
        }

        public List<Form25> GetForm25(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec spGetForm25 '" + fromdate + "' , '" + todate + "'	, '" + stafflist + "'");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<Form25>(qryStr.ToString()).Select(d => new Form25()
                {
                    Id = d.Id,
                    staffid = d.staffid,
                    EmployeeDetails = d.EmployeeDetails,
                    RelayNo = d.RelayNo,
                    PeriodOfEmployment = d.PeriodOfEmployment,
                    PeriodOfWork = d.PeriodOfWork,
                    Day1 = d.Day1,
                    Day2 = d.Day2,
                    Day3 = d.Day3,
                    Day4 = d.Day4,
                    Day5 = d.Day5,
                    Day6 = d.Day6,
                    Day7 = d.Day7,
                    Day8 = d.Day8,
                    Day9 = d.Day9,
                    Day10 = d.Day10,
                    Day11 = d.Day11,
                    Day12 = d.Day12,
                    Day13 = d.Day13,
                    Day14 = d.Day14,
                    Day15 = d.Day15,
                    Day16 = d.Day16,
                    Day17 = d.Day17,
                    Day18 = d.Day18,
                    Day19 = d.Day19,
                    Day20 = d.Day20,
                    Day21 = d.Day21,
                    Day22 = d.Day22,
                    Day23 = d.Day23,
                    Day24 = d.Day24,
                    Day25 = d.Day25,
                    Day26 = d.Day26,
                    Day27 = d.Day27,
                    Day28 = d.Day28,
                    Day29 = d.Day29,
                    Day30 = d.Day30,
                    Day31 = d.Day31,
                    ExemptingOrder = d.ExemptingOrder,
                    WeeklyRest = d.WeeklyRest,
                    CompensatoryHolidayDate = d.CompensatoryHolidayDate,
                    LostRestDays = d.LostRestDays,
                    NoOfDaysWorked = d.NoOfDaysWorked,
                    LeaveWithWages = d.LeaveWithWages,
                    LeaveWithOutWages = d.LeaveWithOutWages,
                    Remarks = d.Remarks,
                }).ToList();

                if (lst == null)
                {
                    return new List<Form25>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<Form25>();
            }
        }


        public List<Form25> GetForm25SL(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec spGetForm25_SINGLE_LINE '" + fromdate + "' , '" + todate + "'	, '" + stafflist + "'");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<Form25>(qryStr.ToString()).Select(d => new Form25()
                {
                    Id = d.Id,
                    staffid = d.staffid,
                    EmployeeDetails = d.EmployeeDetails,
                    RelayNo = d.RelayNo,
                    PeriodOfEmployment = d.PeriodOfEmployment,
                    PeriodOfWork = d.PeriodOfWork,
                    Day1 = d.Day1,
                    Day2 = d.Day2,
                    Day3 = d.Day3,
                    Day4 = d.Day4,
                    Day5 = d.Day5,
                    Day6 = d.Day6,
                    Day7 = d.Day7,
                    Day8 = d.Day8,
                    Day9 = d.Day9,
                    Day10 = d.Day10,
                    Day11 = d.Day11,
                    Day12 = d.Day12,
                    Day13 = d.Day13,
                    Day14 = d.Day14,
                    Day15 = d.Day15,
                    Day16 = d.Day16,
                    Day17 = d.Day17,
                    Day18 = d.Day18,
                    Day19 = d.Day19,
                    Day20 = d.Day20,
                    Day21 = d.Day21,
                    Day22 = d.Day22,
                    Day23 = d.Day23,
                    Day24 = d.Day24,
                    Day25 = d.Day25,
                    Day26 = d.Day26,
                    Day27 = d.Day27,
                    Day28 = d.Day28,
                    Day29 = d.Day29,
                    Day30 = d.Day30,
                    Day31 = d.Day31,
                    ExemptingOrder = d.ExemptingOrder,
                    WeeklyRest = d.WeeklyRest,
                    CompensatoryHolidayDate = d.CompensatoryHolidayDate,
                    LostRestDays = d.LostRestDays,
                    NoOfDaysWorked = d.NoOfDaysWorked,
                    LeaveWithWages = d.LeaveWithWages,
                    LeaveWithOutWages = d.LeaveWithOutWages,
                    Remarks = d.Remarks,
                }).ToList();

                if (lst == null)
                {
                    return new List<Form25>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<Form25>();
            }
        }

        public List<Form25> GetForm25Payroll(string fromdate, string todate, string stafflist)
        {
            SqlParameter[] Param = new SqlParameter[3];
            Param[0] = new SqlParameter("@FromDate", fromdate);
            Param[1] = new SqlParameter("@ToDate", todate);
            Param[2] = new SqlParameter("@Stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Append("exec fnGetForm25_PAYROLL @FromDate,@ToDate,@Stafflist");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<Form25>(qryStr.ToString(), Param).ToList();

                if (lst == null)
                {
                    return new List<Form25>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<Form25>();
            }
        }


        public List<Form25> GetForm25status(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec fnGetForm25_PAYROLL '" + fromdate + "' , '" + todate + "'	, '" + stafflist + "'");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<Form25>(qryStr.ToString()).Select(d => new Form25()
                {
                    Id = d.Id,
                    staffid = d.staffid,
                    EmployeeDetails = d.EmployeeDetails,
                    RelayNo = d.RelayNo,
                    PeriodOfEmployment = d.PeriodOfEmployment,
                    PeriodOfWork = d.PeriodOfWork,
                    Day1 = d.Day1,
                    Day2 = d.Day2,
                    Day3 = d.Day3,
                    Day4 = d.Day4,
                    Day5 = d.Day5,
                    Day6 = d.Day6,
                    Day7 = d.Day7,
                    Day8 = d.Day8,
                    Day9 = d.Day9,
                    Day10 = d.Day10,
                    Day11 = d.Day11,
                    Day12 = d.Day12,
                    Day13 = d.Day13,
                    Day14 = d.Day14,
                    Day15 = d.Day15,
                    Day16 = d.Day16,
                    Day17 = d.Day17,
                    Day18 = d.Day18,
                    Day19 = d.Day19,
                    Day20 = d.Day20,
                    Day21 = d.Day21,
                    Day22 = d.Day22,
                    Day23 = d.Day23,
                    Day24 = d.Day24,
                    Day25 = d.Day25,
                    Day26 = d.Day26,
                    Day27 = d.Day27,
                    Day28 = d.Day28,
                    Day29 = d.Day29,
                    Day30 = d.Day30,
                    Day31 = d.Day31,
                    //Day1IN = d.Day1IN,
                    //Day2IN = d.Day2IN,
                    //Day3IN = d.Day3IN,
                    //Day4IN = d.Day4IN,
                    //Day5IN = d.Day5IN,
                    //Day6IN = d.Day6IN,
                    //Day7IN = d.Day7IN,
                    //Day8IN = d.Day8IN,
                    //Day9IN = d.Day9IN,
                    //Day10IN = d.Day10IN,
                    //Day11IN = d.Day11IN,
                    //Day12IN = d.Day12IN,
                    //Day13IN = d.Day13IN,
                    //Day14IN = d.Day14IN,
                    //Day15IN = d.Day15IN,
                    //Day16IN = d.Day16IN,
                    //Day17IN = d.Day17IN,
                    //Day18IN = d.Day18IN,
                    //Day19IN = d.Day19IN,
                    //Day20IN = d.Day20IN,
                    //Day21IN = d.Day21IN,
                    //Day22IN = d.Day22IN,
                    //Day23IN = d.Day23IN,
                    //Day24IN = d.Day24IN,
                    //Day25IN = d.Day25IN,
                    //Day26IN = d.Day26IN,
                    //Day27IN = d.Day27IN,
                    //Day28IN = d.Day28IN,
                    //Day29IN = d.Day29IN,
                    //Day30IN = d.Day30IN,
                    //Day31IN = d.Day31IN,
                    //Day1OUT = d.Day1OUT,
                    //Day2OUT = d.Day2OUT,
                    //Day3OUT = d.Day3OUT,
                    //Day4OUT = d.Day4OUT,
                    //Day5OUT = d.Day5OUT,
                    //Day6OUT = d.Day6OUT,
                    //Day7OUT = d.Day7OUT,
                    //Day8OUT = d.Day8OUT,
                    //Day9OUT = d.Day9OUT,
                    //Day10OUT = d.Day10OUT,
                    //Day11OUT = d.Day11OUT,
                    //Day12OUT = d.Day12OUT,
                    //Day13OUT = d.Day13OUT,
                    //Day14OUT = d.Day14OUT,
                    //Day15OUT = d.Day15OUT,
                    //Day16OUT = d.Day16OUT,
                    //Day17OUT = d.Day17OUT,
                    //Day18OUT = d.Day18OUT,
                    //Day19OUT = d.Day19OUT,
                    //Day20OUT = d.Day20OUT,
                    //Day21OUT = d.Day21OUT,
                    //Day22OUT = d.Day22OUT,
                    //Day23OUT = d.Day23OUT,
                    //Day24OUT = d.Day24OUT,
                    //Day25OUT = d.Day25OUT,
                    //Day26OUT = d.Day26OUT,
                    //Day27OUT = d.Day27OUT,
                    //Day28OUT = d.Day28OUT,
                    //Day29OUT = d.Day29OUT,
                    //Day30OUT = d.Day30OUT,
                    //Day31OUT = d.Day31OUT,
                    //Day1TotalHours = d.Day1TotalHours,
                    //Day2TotalHours = d.Day2TotalHours,
                    //Day3TotalHours = d.Day3TotalHours,
                    //Day4TotalHours = d.Day4TotalHours,
                    //Day5TotalHours = d.Day5TotalHours,
                    //Day6TotalHours = d.Day6TotalHours,
                    //Day7TotalHours = d.Day7TotalHours,
                    //Day8TotalHours = d.Day8TotalHours,
                    //Day9TotalHours = d.Day9TotalHours,
                    //Day10TotalHours = d.Day10TotalHours,
                    //Day11TotalHours = d.Day11TotalHours,
                    //Day12TotalHours = d.Day12TotalHours,
                    //Day13TotalHours = d.Day13TotalHours,
                    //Day14TotalHours = d.Day14TotalHours,
                    //Day15TotalHours = d.Day15TotalHours,
                    //Day16TotalHours = d.Day16TotalHours,
                    //Day17TotalHours = d.Day17TotalHours,
                    //Day18TotalHours = d.Day18TotalHours,
                    //Day19TotalHours = d.Day19TotalHours,
                    //Day20TotalHours = d.Day20TotalHours,
                    //Day21TotalHours = d.Day21TotalHours,
                    //Day22TotalHours = d.Day22TotalHours,
                    //Day23TotalHours = d.Day23TotalHours,
                    //Day24TotalHours = d.Day24TotalHours,
                    //Day25TotalHours = d.Day25TotalHours,
                    //Day26TotalHours = d.Day26TotalHours,
                    //Day27TotalHours = d.Day27TotalHours,
                    //Day28TotalHours = d.Day28TotalHours,
                    //Day29TotalHours = d.Day29TotalHours,
                    //Day30TotalHours = d.Day30TotalHours,
                    //Day31TotalHours = d.Day31TotalHours,
                    //Day1LateIn = d.Day1LateIn,
                    //Day2LateIn = d.Day2LateIn,
                    //Day3LateIn = d.Day3LateIn,
                    //Day4LateIn = d.Day4LateIn,
                    //Day5LateIn = d.Day5LateIn,
                    //Day6LateIn = d.Day6LateIn,
                    //Day7LateIn = d.Day7LateIn,
                    //Day8LateIn = d.Day8LateIn,
                    //Day9LateIn = d.Day9LateIn,
                    //Day10LateIn = d.Day10LateIn,
                    //Day11LateIn = d.Day11LateIn,
                    //Day12LateIn = d.Day12LateIn,
                    //Day13LateIn = d.Day13LateIn,
                    //Day14LateIn = d.Day14LateIn,
                    //Day15LateIn = d.Day15LateIn,
                    //Day16LateIn = d.Day16LateIn,
                    //Day17LateIn = d.Day17LateIn,
                    //Day18LateIn = d.Day18LateIn,
                    //Day19LateIn = d.Day19LateIn,
                    //Day20LateIn = d.Day20LateIn,
                    //Day21LateIn = d.Day11LateIn,
                    //Day22LateIn = d.Day12LateIn,
                    //Day23LateIn = d.Day13LateIn,
                    //Day24LateIn = d.Day14LateIn,
                    //Day25LateIn = d.Day15LateIn,
                    //Day26LateIn = d.Day16LateIn,
                    //Day27LateIn = d.Day17LateIn,
                    //Day28LateIn = d.Day18LateIn,
                    //Day29LateIn = d.Day19LateIn,
                    //Day30LateIn = d.Day30LateIn,
                    //Day31LateIn = d.Day31LateIn,
                    //Day1EarlyOut = d.Day1EarlyOut,
                    //Day2EarlyOut = d.Day2EarlyOut,
                    //Day3EarlyOut = d.Day3EarlyOut,
                    //Day4EarlyOut = d.Day4EarlyOut,
                    //Day5EarlyOut = d.Day5EarlyOut,
                    //Day6EarlyOut = d.Day6EarlyOut,
                    //Day7EarlyOut = d.Day7EarlyOut,
                    //Day8EarlyOut = d.Day8EarlyOut,
                    //Day9EarlyOut = d.Day9EarlyOut,
                    //Day10EarlyOut = d.Day10EarlyOut,
                    //Day11EarlyOut = d.Day11EarlyOut,
                    //Day12EarlyOut = d.Day12EarlyOut,
                    //Day13EarlyOut = d.Day13EarlyOut,
                    //Day14EarlyOut = d.Day14EarlyOut,
                    //Day15EarlyOut = d.Day15EarlyOut,
                    //Day16EarlyOut = d.Day16EarlyOut,
                    //Day17EarlyOut = d.Day17EarlyOut,
                    //Day18EarlyOut = d.Day18EarlyOut,
                    //Day19EarlyOut = d.Day19EarlyOut,
                    //Day20EarlyOut = d.Day20EarlyOut,
                    //Day21EarlyOut = d.Day21EarlyOut,
                    //Day22EarlyOut = d.Day22EarlyOut,
                    //Day23EarlyOut = d.Day23EarlyOut,
                    //Day24EarlyOut = d.Day24EarlyOut,
                    //Day25EarlyOut = d.Day25EarlyOut,
                    //Day26EarlyOut = d.Day26EarlyOut,
                    //Day27EarlyOut = d.Day27EarlyOut,
                    //Day28EarlyOut = d.Day28EarlyOut,
                    //Day29EarlyOut = d.Day29EarlyOut,
                    //Day30EarlyOut = d.Day30EarlyOut,
                    //Day31EarlyOut = d.Day31EarlyOut,
                    //Day1OT = d.Day1OT,
                    //Day2OT = d.Day2OT,
                    //Day3OT = d.Day3OT,
                    //Day4OT = d.Day4OT,
                    //Day5OT = d.Day5OT,
                    //Day6OT = d.Day6OT,
                    //Day7OT = d.Day7OT,
                    //Day8OT = d.Day8OT,
                    //Day9OT = d.Day9OT,
                    //Day10OT = d.Day10OT,
                    //Day11OT = d.Day11OT,
                    //Day12OT = d.Day12OT,
                    //Day13OT = d.Day13OT,
                    //Day14OT = d.Day14OT,
                    //Day15OT = d.Day15OT,
                    //Day16OT = d.Day16OT,
                    //Day17OT = d.Day17OT,
                    //Day18OT = d.Day18OT,
                    //Day19OT = d.Day19OT,
                    //Day20OT = d.Day20OT,
                    //Day21OT = d.Day21OT,
                    //Day22OT = d.Day22OT,
                    //Day23OT = d.Day23OT,
                    //Day24OT = d.Day24OT,
                    //Day25OT = d.Day25OT,
                    //Day26OT = d.Day26OT,
                    //Day27OT = d.Day27OT,
                    //Day28OT = d.Day28OT,
                    //Day29OT = d.Day29OT,
                    //Day30OT = d.Day30OT,
                    //Day31OT = d.Day31OT,
                    ExemptingOrder = d.ExemptingOrder,
                    WeeklyRest = d.WeeklyRest,
                    CompensatoryHolidayDate = d.CompensatoryHolidayDate,
                    LostRestDays = d.LostRestDays,
                    NoOfDaysWorked = d.NoOfDaysWorked,
                    LeaveWithWages = d.LeaveWithWages,
                    LeaveWithOutWages = d.LeaveWithOutWages,
                    Remarks = d.Remarks,
                    NAME = d.NAME,
                    PLANT = d.PLANT,
                    DEPARTMENT = d.DEPARTMENT,
                    Division = d.Division,
                    Volume = d.Volume,
                    NOOFDAYSWORKING = d.NOOFDAYSWORKING,
                    //NOOFDAYSWORKED = d.NOOFDAYSWORKED,
                    NOOFDAYSNOTWORKED = d.NOOFDAYSNOTWORKED,
                    NOOFDAYSABSENT = d.NOOFDAYSABSENT,
                    NOOFDAYSLEAVE = d.NOOFDAYSLEAVE,
                    NOOFDAYSWO = d.NOOFDAYSWO,
                    NSA1 = d.NSA1,
                    NSA2 = d.NSA2,
                    ATTINCENTIVE = d.ATTINCENTIVE,
                    LATEPENALTY = d.LATEPENALTY,
                    DOJ = d.DOJ,
                    DOR = d.DOR
                }).ToList();

                if (lst == null)
                {
                    return new List<Form25>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<Form25>();
            }
        }
        public List<ManualPunchApprovalList> GetManualPunchApprovalLists(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Append("Exec [dbo].[GetManualPunchRequisitionHistoryReport]  '" + stafflist + "','" + fromdate + "','" + todate + "' ");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ManualPunchApprovalList>(qryStr.ToString()).Select(d => new ManualPunchApprovalList()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    Company = d.Company,
                    Location = d.Location,
                    Branch = d.Branch,
                    Department = d.Department,
                    Division = d.Division,
                    Designation = d.Designation,
                    Grade = d.Grade,
                    Category = d.Category,
                    CostCentre = d.CostCentre,
                    PunchType = d.PunchType,
                    InDateTime = d.InDateTime,
                    OutDateTime = d.OutDateTime,
                    Reason = d.Reason,
                    ApprovalStatus = d.ApprovalStatus,
                    ReviewalStatus = d.ReviewalStatus,
                    ApplicationDate = d.ApplicationDate,
                    ApprovedBy = d.ApprovedBy,
                    ApprovedOn = d.ApprovedOn,
                    ReviewedBy = d.ReviewedBy,
                    ReviewedOn = d.ReviewedOn
                }).ToList();
                if (lst == null)
                {
                    return new List<ManualPunchApprovalList>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<ManualPunchApprovalList>();
            }
        }

        public List<ShiftChangeApproval> GetShiftChangeApprovalLists(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select ApplicationId as ShiftChangeId ,StaffId ,StaffName as FirstName ,StartDate as FromDate ,EndDate as ToDate ," +
                " NewShiftName ,Remarks as ShiftChangeReason , convert(varchar, Approval1StatusId) as ApprovalStatusId ," +
                "Approval1StatusName as ApprovalStatusName , ApplicationApprovalId ,Approved1On as ApprovedOnDate , Comment ,Approval1Owner as ApprovalOwner " +
                "from ShiftChangeApproval where StartDate between '" + fromdate + "' and '" + todate + "' and  StaffId In (" + stafflist + ")");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ShiftChangeApproval>(qryStr.ToString()).Select(d => new ShiftChangeApproval()
                {
                    ShiftChangeId = d.ShiftChangeId,
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    FromDate = d.FromDate,
                    ToDate = d.ToDate,
                    NewShiftId = d.NewShiftId,
                    NewShiftName = d.NewShiftName,
                    ShiftChangeReason = d.ShiftChangeReason,
                    ApprovalStatusId = d.ApprovalStaffId,
                    ApprovalStatusName = d.ApprovalStatusName,
                    ApprovalStaffId = d.ApprovalStatusId,
                    ApprovalStaffName = d.ApprovalStaffName,
                    ApplicationApprovalId = d.ApplicationApprovalId,
                    ApprovedOnDate = d.ApprovedOnDate,
                    ApprovedOnTime = d.ApprovedOnTime,
                    Comment = d.Comment,
                    ApprovalOwner = d.ApprovalOwner
                }).ToList();

                if (lst == null)
                {
                    return new List<ShiftChangeApproval>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ShiftChangeApproval>();
            }
        }

        public List<PlannedLeave> GetPlannedLeaveLists(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            //please do not get deceived by the name of the sql function being called.
            // it is actually a common function being used by both planned and unplanned leave list.
            qryStr.Append("SELECT * FROM fnGetPlannedLeaveV1 (  '" + fromdate + "' , '" + todate + "' , '" + stafflist + "')");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<PlannedLeave>(qryStr.ToString()).Select(d => new PlannedLeave()
                {
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    ShiftName = d.ShiftName,
                    CompanyName = d.CompanyName,
                    DepartmentName = d.DepartmentName,
                    GradeName = d.GradeName,
                    LeaveTypeName = d.LeaveTypeName,
                    LeaveStartDate = d.LeaveStartDate,
                    LeaveStartDurationName = d.LeaveStartDurationName,
                    LeaveEndDate = d.LeaveEndDate,
                    LeaveEndDurationName = d.LeaveEndDurationName,
                    ApprovalStatusName = d.ApprovalStatusName,
                    LeaveApplicationReason = d.LeaveApplicationReason,
                    ApprovalStaffId = d.ApprovalStaffId,
                    ApprovalStaffName = d.ApprovalStaffName,
                    ApprovedOnDate = d.ApprovedOnDate
                }).ToList();

                if (lst == null)
                {
                    return new List<PlannedLeave>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<PlannedLeave>();
            }
        }


        public List<UnPlannedLeave> GetUnPlannedLeaveLists(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            stafflist = stafflist.Replace("','", ",");
            qryStr.Append("SELECT * FROM fnGetUnPlannedLeaveNew ( '" + stafflist + "' , '" + fromdate + "' , '" + todate + "' , 0)");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<UnPlannedLeave>(qryStr.ToString()).Select(d => new UnPlannedLeave()
                {
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    ShiftName = d.ShiftName,
                    DepartmentName = d.DepartmentName,
                    GradeName = d.GradeName,
                    ShiftInDate = d.ShiftInDate,
                    ShiftInTime = d.ShiftInTime,
                    ShiftOutDate = d.ShiftOutDate,
                    ShiftOutTime = d.ShiftOutTime,
                    AttendanceStatus = d.AttendanceStatus,
                    LeaveStatus = d.LeaveStatus,
                    CompanyName = d.CompanyName,
                    LeaveTypeName = d.LeaveTypeName,
                    LeaveStartDate = d.LeaveStartDate,
                    LeaveStartDurationName = d.LeaveStartDurationName,
                    LeaveEndDate = d.LeaveEndDate,
                    LeaveEndDurationName = d.LeaveEndDurationName,
                    ApprovalStatusName = d.ApprovalStatusName,
                    LeaveApplicationReason = d.LeaveApplicationReason,
                    ApprovalStaffId = d.ApprovalStaffId,
                    ApprovalStaffName = d.ApprovalStaffName
                }).ToList();

                if (lst == null)
                {
                    return new List<UnPlannedLeave>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<UnPlannedLeave>();
            }
        }


        public List<DepartmentSummary> GetDepartmentSummaryList(string fromdate, string todate, string stafflist, string CompanyId)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            stafflist = stafflist.Replace("','", ",");
            stafflist = stafflist.Replace("'", "");
            qryStr.Append("SELECT * FROM fnGetDepartmentSummary ('" + CompanyId + "', '" + stafflist + "','" + fromdate + "' , '" + todate + "' )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<DepartmentSummary>(qryStr.ToString()).Select(d => new DepartmentSummary()
                {
                    DEPTID = d.DEPTID,
                    DEPTNAME = d.DEPTNAME,
                    TXNDATE = d.TXNDATE,
                    HEADCOUNT = d.HEADCOUNT,
                    TOTALPRESENT = d.TOTALPRESENT,
                    TOTALABSENT = d.TOTALABSENT,
                    PRESENTPAGE = d.PRESENTPAGE,
                    ABSENTPAGE = d.ABSENTPAGE
                }).ToList();
                if (lst == null)
                {
                    return new List<DepartmentSummary>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<DepartmentSummary>();
            }
        }

        //public List<ContinuousAbsent> GetContinuousAbsentList(string fromdate, string todate, string stafflist, int DayCount)
        //{
        //    var qryStr = new StringBuilder();
        //    qryStr.Clear();
        //    qryStr.Append("SELECT * FROM fnGetContinuousAbsentList ( '" + stafflist + "' , '" + fromdate + "' , '" + todate + "' , "+DayCount+" )");
        //    try
        //    {
        //        context.Database.CommandTimeout = 0;
        //        var lst = context.Database.SqlQuery<ContinuousAbsent>(qryStr.ToString()).Select(d => new ContinuousAbsent()
        //        {
        //            StaffId = d.StaffId,
        //            StaffName = d.StaffName,
        //            CompanyName = d.CompanyName,
        //            DepartmentName = d.DepartmentName,
        //            DivisionName = d.DivisionName,
        //            NoOfDays = d.NoOfDays
        //        }).ToList();
        //        if (lst == null)
        //        {
        //            return new List<ContinuousAbsent>();
        //        }
        //        else
        //        {
        //            return lst;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return new List<ContinuousAbsent>();
        //    }
        //}
        public List<ContinuousAbsent> GetContinuousAbsentList(string fromdate, string todate, string stafflist, int DayCount)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec GetContinuousDefaultersReport '" + stafflist + "' , '" + fromdate + "' , '" + todate + "' , " + DayCount + "," + 0 + ", '" + "AB" + "'");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ContinuousAbsent>(qryStr.ToString()).Select(d => new ContinuousAbsent()
                {
                    STAFFID = d.STAFFID,
                    NAME = d.NAME,
                    DEPARTMENT = d.DEPARTMENT,
                    Designation = d.Designation,
                    FROMDATE = d.FROMDATE,
                    TODATE = d.TODATE,
                    TOTALDAYS = d.TOTALDAYS
                }).ToList();
                if (lst == null)
                {
                    return new List<ContinuousAbsent>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<ContinuousAbsent>();
            }
        }

        public List<ContinuousLateComing> GetGraceTime(string fromdate, string todate, string stafflist, int DayCount)
        {
            var repo = new ReportRepository();
            var lst = repo.GetContinuousLateComing(fromdate, todate, stafflist, DayCount);
            return lst;
        }

        public List<GraceTime> GetGraceTime(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetGraceTime ( '" + stafflist + "' , '" + fromdate + "' , '" + todate + "' )");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<GraceTime>(qryStr.ToString()).Select(d => new GraceTime()
                {
                    STAFFID = d.STAFFID,
                    NAME = d.NAME,
                    SHIFTSHORTNAME = d.SHIFTSHORTNAME,
                    TXNDATE = d.TXNDATE,
                    SCHINTIME = d.SCHINTIME,
                    SCHOUTTIME = d.SCHOUTTIME,
                    INTIME = d.INTIME,
                    OUTTIME = d.OUTTIME,
                    LATECOMING = d.LATECOMING,
                    EARLYGOING = d.EARLYGOING
                }).ToList();
                if (lst == null)
                {
                    return new List<GraceTime>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<GraceTime>();
            }
        }

        public List<ContinuousLateComing> GetContinuousLateComing(string fromdate, string todate, string stafflist, int DayCount)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetContinuousLateList ( '" + stafflist + "' , '" + fromdate + "' , '" + todate + "' , " + DayCount + " )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ContinuousLateComing>(qryStr.ToString()).Select(d => new ContinuousLateComing()
                {
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    CompanyName = d.CompanyName,
                    DepartmentName = d.DepartmentName,
                    Designation = d.Designation,
                    DivisionName = d.DivisionName,
                    NoOfDays = d.NoOfDays,
                    TotalHours = d.TotalHours
                }).ToList();
                if (lst == null)
                {
                    return new List<ContinuousLateComing>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ContinuousLateComing>();
            }
        }

        public List<ContinuousEarlyGoing> GetContinuousEarlyGoing(string fromdate, string todate, string stafflist, int DayCount)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetContinuousEarlyGoingList ( '" + stafflist + "' , '" + fromdate + "' , '" + todate + "' , " + DayCount + " )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ContinuousEarlyGoing>(qryStr.ToString()).Select(d => new ContinuousEarlyGoing()
                {
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    CompanyName = d.CompanyName,
                    DepartmentName = d.DepartmentName,
                    DivisionName = d.DivisionName,
                    NoOfDays = d.NoOfDays,
                    TotalHours = d.TotalHours
                }).ToList();
                if (lst == null)
                {
                    return new List<ContinuousEarlyGoing>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ContinuousEarlyGoing>();
            }
        }


        public List<MissedPunchList> GetMissedPunchList(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetMissPunchList ( '" + stafflist + "' , '" + fromdate + "' , '" + todate + "' )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<MissedPunchList>(qryStr.ToString()).Select(d => new MissedPunchList()
                {
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    CompanyName = d.CompanyName,
                    DepartmentName = d.DepartmentName,
                    Designation = d.Designation,
                    DivisionName = d.DivisionName,
                    TxnDate = d.TxnDate,
                    MissedPunch = d.MissedPunch

                }).ToList();
                if (lst == null)
                {
                    return new List<MissedPunchList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<MissedPunchList>();
            }
        }
        public List<LateComers> GetLateComersList(string fromdate, string todate, string stafflist)
        {
            stafflist = stafflist.Replace("','", ",");
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM dbo.fnGetLateComers ( '" + fromdate + "' , '" + todate + "' , " + stafflist + " )");
            //"ApprovalStatusName , ApprovalStaffId , ApprovalStaffName , ApplicationApprovalId , ApprovedOnDate , ApprovedOnTime , Comment , ApprovalOwner " +
            //"from vwShiftChangeApproval where FromDate between '" + fromdate + "' and '" + todate + "' and  StaffId =" + stafflist + "");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<LateComers>(qryStr.ToString()).Select(d => new LateComers()
                {
                    StaffID = d.StaffID,
                    FirstName = d.FirstName,
                    CompanyName = d.CompanyName,
                    BranchName = d.BranchName,
                    DepartmentName = d.DepartmentName,
                    Designation = d.Designation,
                    DivisionName = d.DivisionName,
                    ShiftName = d.ShiftName,
                    ShiftInDate = d.ShiftInDate,
                    ShiftInTime = d.ShiftInTime,
                    ShiftOutTime = d.ShiftOutTime,
                    ActualInTime = d.ActualInTime,
                    ActualWorkedHours = d.ActualWorkedHours,
                    LateComing = d.LateComing,
                    AccountedLateComingTime = d.AccountedLateComingTime,
                    IsValid = d.IsValid,
                    TotalHoursPermission = d.TotalHoursPermission,
                    ApprovalStatusName = d.ApprovalStatusName,
                    ApprovalStaffName = d.ApprovalStaffName

                }).ToList();
                if (lst == null)
                {
                    return new List<LateComers>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LateComers>();
            }
        }

        public List<OvertimeStatement> GetOvertimeStatementList(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            stafflist = stafflist.Replace("','", ",");
            qryStr.Append("SELECT * FROM fnGetOverTimeStatement ( '" + fromdate + "' , '" + todate + "' , " + stafflist + " )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<OvertimeStatement>(qryStr.ToString()).Select(d => new OvertimeStatement()
                {
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    CompanyName = d.CompanyName,
                    DeptName = d.DeptName,
                    GradeName = d.GradeName,
                    ShiftId = d.StaffId,
                    Name = d.Name,
                    ShiftInDate = d.ShiftInDate,
                    ShiftInTime = d.ShiftInTime,
                    ShiftOutDate = d.ShiftOutDate,
                    ShiftOutTime = d.ShiftOutTime,
                    ActualInDate = d.ActualInDate,
                    ActualInTime = d.ActualInTime,
                    ActualOutDate = d.ActualOutDate,
                    ActualOutTime = d.ActualOutTime,
                    ActualOTTime = d.ActualOTTime,
                    AccountedOTTime = d.AccountedOTTime
                }).ToList();
                if (lst == null)
                {
                    return new List<OvertimeStatement>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<OvertimeStatement>();
            }
        }

        public List<ShiftChangeStatement> GetShiftChangeStatement(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetShiftChangeList ( " + stafflist + ",'" + fromdate + "' , '" + todate + "' )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ShiftChangeStatement>(qryStr.ToString()).Select(d => new ShiftChangeStatement()
                {
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    CompanyName = d.CompanyName,
                    BranchName = d.BranchName,
                    DivisionName = d.DivisionName,
                    TxnDate = d.TxnDate,
                    NewShiftName = d.NewShiftName,
                    ShiftChangeReason = d.ShiftChangeReason,
                    ApprovalStatusName = d.ApprovalStatusName,
                    ApprovalStaffName = d.ApprovalStaffName,
                    ApprovedOnDate = d.ApprovedOnDate
                }).ToList();
                if (lst == null)
                {
                    return new List<ShiftChangeStatement>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ShiftChangeStatement>();
            }
        }

        public List<ExtraHoursWorked> GetExtraHoursWorkedList(string fromdate, string todate, string stafflist)
        {
            stafflist = stafflist.Replace("','", ",");
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetExtraHoursWorkedStatement  ( " + stafflist + " , '" + fromdate + "' , '" + todate + "' )");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ExtraHoursWorked>(qryStr.ToString()).Select(d => new ExtraHoursWorked()
                {
                    STAFFID = d.STAFFID,
                    NAME = d.NAME,
                    SHIFTSHORTNAME = d.SHIFTSHORTNAME,
                    TXNDATE = d.TXNDATE,
                    SCHINTIME = d.SCHINTIME,
                    SCHOUTTIME = d.SCHOUTTIME,
                    INTIME = d.INTIME,
                    OUTTIME = d.OUTTIME,
                    EARLYCOMING = d.EARLYCOMING,
                    LATEGOING = d.LATEGOING,
                    OTHOURS = d.OTHOURS,
                    COL1 = d.COL1,
                    COL2 = d.COL2,
                    COL3 = d.COL3,
                    COL4 = d.COL4,
                    COL5 = d.COL5,
                    COL6 = d.COL6,
                    COL7 = d.COL7,
                    COL8 = d.COL8,
                    COL9 = d.COL9
                }).ToList();
                if (lst == null)
                {
                    return new List<ExtraHoursWorked>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ExtraHoursWorked>();
            }
        }


        public List<EarlyArraival> GetEarlyArraival(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetEarlyArrivalStatement  ( '" + fromdate + "' , '" + todate + "' , " + stafflist + " )");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<EarlyArraival>(qryStr.ToString()).Select(d => new EarlyArraival()
                {

                    StaffId = d.ShiftId,
                    FirstName = d.FirstName,
                    CompanyName = d.CompanyName,
                    DeptName = d.DeptName,
                    GradeName = d.GradeName,
                    ShiftId = d.ShiftId,
                    Name = d.Name,
                    ShiftInDate = d.ShiftInDate,
                    ShiftInTime = d.ShiftInTime,
                    ShiftOutDate = d.ShiftOutDate,
                    ShiftOutTime = d.ShiftOutTime,
                    ActualInDate = d.ActualInDate,
                    ActualInTime = d.ActualInTime,
                    ActualOutDate = d.ActualOutDate,
                    ActualOutTime = d.ActualOutTime,
                    ActualEarlyComingTime = d.AccountedEarlyComingTime,
                    AccountedEarlyComingTime = d.AccountedEarlyComingTime,

                }).ToList();

                if (lst == null)
                {
                    return new List<EarlyArraival>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<EarlyArraival>();
            }
        }

        public List<EarlyDeparture> GetEarlyDepartureLists(string fromdate, string todate, string stafflist)
        {
            stafflist = stafflist.Replace("','", ",");
            stafflist = stafflist.Replace("'", "");
            var qrystr = new StringBuilder();
            qrystr.Clear();
            //qrystr.Append("SELECT * FROM fnGetEarlyDepartureStatement ( '" + fromdate + "' , '" + todate + "' , " + stafflist + " )");
            qrystr.Append("SELECT * FROM fnGetEarlyDepartureStatement  ( '" + stafflist + "','" + fromdate + "' , '" + todate + "')");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<EarlyDeparture>(qrystr.ToString()).Select(d => new EarlyDeparture()
                {
                    STAFFID = d.STAFFID,
                    NAME = d.NAME,
                    SHIFTSHORTNAME = d.SHIFTSHORTNAME,
                    TXNDATE = d.TXNDATE,
                    SCHINTIME = d.SCHINTIME,
                    SCHOUTTIME = d.SCHOUTTIME,
                    INTIME = d.INTIME,
                    OUTTIME = d.OUTTIME,
                    EARLYGOING = d.EARLYGOING,
                    COL1 = d.COL1,
                    COL2 = d.COL2,
                    COL3 = d.COL3,
                    COL4 = d.COL4,
                    COL5 = d.COL5,
                    COL6 = d.COL6,
                    COL7 = d.COL7,
                    COL8 = d.COL8,
                    COL9 = d.COL9
                }).ToList();
                if (lst == null)
                {
                    return new List<EarlyDeparture>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<EarlyDeparture>();
            }
        }

        public List<NightShiftData> GetNightShiftDataRepoert(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT STAFFID ,STAFFNAME ,PLANT ,DEPARTMENT ,DESIGNATION,CATEGORY,TXNDATE,SHIFTSHORTNAME from fnNightShiftData ('" + stafflist + "','" + fromdate + "','" + todate + "')");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<NightShiftData>(qryStr.ToString()).Select(d => new NightShiftData()
                {
                    STAFFID = d.STAFFID,
                    STAFFNAME = d.STAFFNAME,
                    PLANT = d.PLANT,
                    DEPARTMENT = d.DEPARTMENT,
                    CATEGORY = d.CATEGORY,
                    TXNDATE = d.TXNDATE,
                    SHIFTSHORTNAME = d.SHIFTSHORTNAME
                }).ToList();

                if (lst == null)
                {
                    return new List<NightShiftData>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<NightShiftData>();
            }
        }

        public List<ShiftViolation> GetShiftViolation(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT STAFFID ,NAME ,PLANT ,TEAM,DEPARTMENT ,DESIGNATION, GRADE,TXNDATE,PLANNEDSHIFT,ACTUALSHIFT from fnGetShiftViolationReport ('" + stafflist + "','" + fromdate + "','" + todate + "')");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ShiftViolation>(qryStr.ToString()).Select(d => new ShiftViolation()
                {
                    STAFFID = d.STAFFID,
                    NAME = d.NAME,
                    PLANT = d.PLANT,
                    TEAM = d.TEAM,
                    DEPARTMENT = d.DEPARTMENT,
                    DESIGNATION = d.DESIGNATION,
                    GRADE = d.GRADE,
                    TXNDATE = d.TXNDATE,
                    PLANNEDSHIFT = d.PLANNEDSHIFT,
                    ACTUALSHIFT = d.ACTUALSHIFT
                }).ToList();

                if (lst == null)
                {
                    return new List<ShiftViolation>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ShiftViolation>();
            }
        }

        public Form15StaffPersonalDetails GetForm15StaffPersonalDetails(string StaffId)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append(" SELECT STAFFID , DBO.fnGetMasterName(STAFFID , 'DP') AS DEPARTMENT , ");
            QryStr.Append(" REPLACE ( CONVERT ( VARCHAR , DateOfJoining , 106 ) , ' ' , '-' ) AS DATEOFJOINING , DBO.FNGETSTAFFNAME(StaffId) AS NAME , ");
            QryStr.Append(" ( SELECT NAME FROM StaffFamily WHERE STAFFID = '" + StaffId + "' AND RELATEDAS = 1 ) AS FATHERNAME , ");
            QryStr.Append(" ( SELECT ADDR FROM STAFFPERSONAL WHERE STAFFID = '" + StaffId + "' ) AS RESADDR , ");
            QryStr.Append(" CASE WHEN YEAR ( ResignationDate ) = 2055 THEN '' ELSE REPLACE ( CONVERT ( VARCHAR , ResignationDate , 106 ) , ' ' , '-' ) END AS DATEOFLEAVING");
            QryStr.Append(" FROM STAFFOFFICIAL WHERE STAFFID = '" + StaffId + "' ");

            try
            {
                context.Database.CommandTimeout = 0;
                var data = context.Database.SqlQuery<Form15StaffPersonalDetails>(QryStr.ToString()).Select(d => new Form15StaffPersonalDetails()
                {
                    StaffId = d.StaffId,
                    Department = d.Department,
                    DateOfJoining = d.DateOfJoining,
                    Name = d.Name,
                    FatherName = d.FatherName,
                    ResAddr = d.ResAddr,
                    DateOfLeaving = d.DateOfLeaving
                }).FirstOrDefault();

                if (data == null)
                {
                    return new Form15StaffPersonalDetails();
                }
                else
                {
                    return data;
                }
            }
            catch (Exception)
            {
                return new Form15StaffPersonalDetails();
            }
        }

        public List<Form15> GetForm15(string StaffId, string FromDate, string ToDate)
        {
            var QryStr = new StringBuilder();

            QryStr.Clear();
            QryStr.Append("SELECT YearDateApplicationForLeave , NOOFDAYS , replace ( convert ( varchar , DATEFROM , 106 ) , ' ' , '-' ) as DATEFROM , replace ( convert ( varchar , DATETO , 106 ) , ' ' , '-' ) as DATETO , REASON  , NOOFWORKINGDAYS  , NOOFWORKEDDAYS  , NOOFDAYSLAYOFF  , NOOFDAYSLEAVEEARNED  , TOTALLEAVESATCREDIT  , NOOFDAYSWITHPAY  , NOOFDAYSWITHLOSSOFPAY  , LEAVEBALANCE  , SEC79  , LATEMINUTES  , ABSENCE FROM fnGetForm15 ( '" + StaffId + "' , '" + FromDate + "' , '" + ToDate + "' ) ORDER BY CONVERT ( DATETIME , DATEFROM ) ASC");

            context.Database.CommandTimeout = 0;
            var lst = context.Database.SqlQuery<Form15>(QryStr.ToString()).Select(d => new Form15()
            {
                YearDateApplicationForLeave = d.YearDateApplicationForLeave,
                NOOFDAYS = d.NOOFDAYS,
                DATEFROM = d.DATEFROM,
                DATETO = d.DATETO,
                REASON = d.REASON,
                NOOFWORKINGDAYS = d.NOOFWORKINGDAYS,
                NOOFWORKEDDAYS = d.NOOFWORKEDDAYS,
                NOOFDAYSLAYOFF = d.NOOFDAYSLAYOFF,
                NOOFDAYSLEAVEEARNED = d.NOOFDAYSLEAVEEARNED,
                TOTALLEAVESATCREDIT = d.TOTALLEAVESATCREDIT,
                NOOFDAYSWITHPAY = d.NOOFDAYSWITHPAY,
                NOOFDAYSWITHLOSSOFPAY = d.NOOFDAYSWITHLOSSOFPAY,
                LEAVEBALANCE = d.LEAVEBALANCE,
                SEC79 = d.SEC79,
                LATEMINUTES = d.LATEMINUTES,
                ABSENCE = d.ABSENCE
            }).ToList();

            if (lst == null)
            {
                return new List<Form15>();
            }
            else
            {
                return lst;
            }
        }

        public List<DailyAttendance> GetDailyAttendance(string fromdate, string todate, string stafflist, bool SUMrpt)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec [DAILYPERFORMANCEATTENDANCEREPORT] '" + fromdate + "','" + todate + "','" + stafflist + "'," + SUMrpt + "");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<DailyAttendance>(qryStr.ToString()).Select(d => new DailyAttendance()
                {
                    Id = d.Id,
                    Branch = d.Branch,
                    StaffId = d.StaffId,
                    NAME = d.NAME,
                    CostCenter = d.CostCenter,
                    DEPARTMENT = d.DEPARTMENT,
                    Designation = d.Designation,
                    ShiftName = d.ShiftName,
                    SHIFTCOUNT = d.SHIFTCOUNT,
                    ShiftInDate = d.ShiftInDate,
                    ActualInTime = d.ActualInTime,
                    ActualOutTime = d.ActualOutTime,
                    TotalWorkedHours = d.TotalWorkedHours,
                    HalfDayCount = d.HalfDayCount,
                    LeaveCount = d.LeaveCount,
                    HolidayCount = d.HolidayCount,
                    WeeklyoffCount = d.WeeklyoffCount,
                    ODCount = d.ODCount,
                    COFFCOUNT = d.COFFCOUNT,
                    AbsentCount = d.AbsentCount,
                    PresentCount = d.PresentCount,
                    EarlyArrival = d.EarlyArrival,
                    LateArrival = d.LateArrival,
                    EarlyGoing = d.EarlyGoing,
                    LateGoing = d.LateGoing
                }).ToList();

                if (lst == null)
                {
                    return new List<DailyAttendance>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<DailyAttendance>();
            }
        }

        public List<DailyExtraHoursWorkedDetails> GetExtraHoursWorkedDetails(string fromdate, string todate, string stafflist, bool flag)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec EXTRAHOUSWORKEDREPORT '" + fromdate + "' , '" + todate + "'	, '" + stafflist + "','" + flag + "' ");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<DailyExtraHoursWorkedDetails>(qryStr.ToString()).Select(d => new DailyExtraHoursWorkedDetails()
                {
                    ID = d.ID,
                    BRANCH = d.BRANCH,
                    staffid = d.staffid,
                    NAME = d.NAME,
                    COSTCENTER = d.COSTCENTER,
                    DEPARTMENT = d.DEPARTMENT,
                    DESIGNATION = d.DESIGNATION,
                    Day1 = d.Day1,
                    Day2 = d.Day2,
                    Day3 = d.Day3,
                    Day4 = d.Day4,
                    Day5 = d.Day5,
                    Day6 = d.Day6,
                    Day7 = d.Day7,
                    Day8 = d.Day8,
                    Day9 = d.Day9,
                    Day10 = d.Day10,
                    Day11 = d.Day11,
                    Day12 = d.Day12,
                    Day13 = d.Day13,
                    Day14 = d.Day14,
                    Day15 = d.Day15,
                    Day16 = d.Day16,
                    Day17 = d.Day17,
                    Day18 = d.Day18,
                    Day19 = d.Day19,
                    Day20 = d.Day20,
                    Day21 = d.Day21,
                    Day22 = d.Day22,
                    Day23 = d.Day23,
                    Day24 = d.Day24,
                    Day25 = d.Day25,
                    Day26 = d.Day26,
                    Day27 = d.Day27,
                    Day28 = d.Day28,
                    Day29 = d.Day29,
                    Day30 = d.Day30,
                    Day31 = d.Day31,
                    NORMALHOURS = d.NORMALHOURS,
                    HOLIDAYHOURS = d.HOLIDAYHOURS,
                    WOHOURS = d.WOHOURS,
                    TOTALOTHOURS = d.TOTALOTHOURS,
                    //TotalHoursWorked = d.Day1 ( d.Day1 + d.Day2 + d.Day3 +  d.Day4 + d.Day5 + d.Day6 + d.Day7 + d.Day8 + d.Day9 + d.Day10 +d.Day11 + d.Day12 + d.Day13 + d.Day14 + d.Day15 + d.Day16 + d.Day17 + d.Day18 + d.Day19 + d.Day20 + d.Day21 + d.Day22 + d.Day23 + d.Day24 + d.Day25 + d.Day26 + d.Day27 + d.Day28 + d.Day29 + d.Day30 + d.Day31 )
                }).ToList();

                if (lst == null)
                {
                    return new List<DailyExtraHoursWorkedDetails>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<DailyExtraHoursWorkedDetails>();
            }
        }

        public List<DepartmentWiseDailyAttendance> GetDepartmentWiseDailyAttendance(string fromdate, string todate, string stafflist, bool SUMrpt)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec [DEPARTMENTWISEEREPORT] '" + fromdate + "','" + todate + "','" + stafflist + "'," + SUMrpt + " , 'department' ");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<DepartmentWiseDailyAttendance>(qryStr.ToString()).Select(d => new DepartmentWiseDailyAttendance()
                {
                    Id = d.Id,
                    Staffid = d.Staffid,
                    ShiftName = d.ShiftName,
                    SHIFTCOUNT = d.SHIFTCOUNT,
                    Department = d.Department,
                    LeaveCount = d.LeaveCount,
                    PRESENTCOUNT = d.PRESENTCOUNT,
                    ABSENTCOUNT = d.ABSENTCOUNT,
                    WEEKLYOFFCOUNT = d.WEEKLYOFFCOUNT,
                    HalfDayCount = d.HalfDayCount,
                    HOLIDAYCOUNT = d.HOLIDAYCOUNT,
                    ODCount = d.ODCount,
                    COFFCOUNT = d.COFFCOUNT,
                    EarlyArrival = d.EarlyArrival,
                    LateArrival = d.LateArrival,
                    EarlyGoing = d.EarlyGoing,
                    LateGoing = d.LateGoing,

                }).ToList();

                if (lst == null)
                {
                    return new List<DepartmentWiseDailyAttendance>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<DepartmentWiseDailyAttendance>();
            }
        }


        public List<BranchWiseDailyAttendance> GetBranchWiseDailyAttendance(string fromdate, string todate, string stafflist, bool SUMrpt)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec [DEPARTMENTWISEEREPORT] '" + fromdate + "','" + todate + "','" + stafflist + "'," + SUMrpt + " , 'Branch' ");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<BranchWiseDailyAttendance>(qryStr.ToString()).Select(d => new BranchWiseDailyAttendance()
                {
                    Id = d.Id,
                    Staffid = d.Staffid,
                    ShiftName = d.ShiftName,
                    SHIFTCOUNT = d.SHIFTCOUNT,
                    Branch = d.Branch,
                    LeaveCount = d.LeaveCount,
                    PRESENTCOUNT = d.PRESENTCOUNT,
                    ABSENTCOUNT = d.ABSENTCOUNT,
                    WEEKLYOFFCOUNT = d.WEEKLYOFFCOUNT,
                    HalfDayCount = d.HalfDayCount,
                    HOLIDAYCOUNT = d.HOLIDAYCOUNT,
                    ODCount = d.ODCount,
                    COFFCOUNT = d.COFFCOUNT,
                    EarlyArrival = d.EarlyArrival,
                    LateArrival = d.LateArrival,
                    EarlyGoing = d.EarlyGoing,
                    LateGoing = d.LateGoing,

                }).ToList();

                if (lst == null)
                {
                    return new List<BranchWiseDailyAttendance>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<BranchWiseDailyAttendance>();
            }
        }

        public List<OverTime> GetOverTime(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec spGetOverTime  '" + fromdate + "' , '" + todate + "'	, '" + stafflist + "' ");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<OverTime>(qryStr.ToString()).Select(d => new OverTime()
                {
                    Id = d.Id,
                    staffid = d.staffid,
                    NAME = d.NAME,
                    Day1 = d.Day1,
                    Day2 = d.Day2,
                    Day3 = d.Day3,
                    Day4 = d.Day4,
                    Day5 = d.Day5,
                    Day6 = d.Day6,
                    Day7 = d.Day7,
                    Day8 = d.Day8,
                    Day9 = d.Day9,
                    Day10 = d.Day10,
                    Day11 = d.Day11,
                    Day12 = d.Day12,
                    Day13 = d.Day13,
                    Day14 = d.Day14,
                    Day15 = d.Day15,
                    Day16 = d.Day16,
                    Day17 = d.Day17,
                    Day18 = d.Day18,
                    Day19 = d.Day19,
                    Day20 = d.Day20,
                    Day21 = d.Day21,
                    Day22 = d.Day22,
                    Day23 = d.Day23,
                    Day24 = d.Day24,
                    Day25 = d.Day25,
                    Day26 = d.Day26,
                    Day27 = d.Day27,
                    Day28 = d.Day28,
                    Day29 = d.Day29,
                    Day30 = d.Day30,
                    Day31 = d.Day31,
                    Total = d.Total

                }).ToList();

                if (lst == null)
                {
                    return new List<OverTime>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<OverTime>();
            }
        }

        public List<AttendanceDataView> GetOTRepository(string StaffId, string fromdate, string todate)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            //qryStr.Append("SELECT STAFFID ,NAME as FirstName , TXNDATE as ShiftInDate, ShiftShortName ,ShiftInTime,ShiftOutTime, ActualInTime As InTime,  ActualOutTime as OutTime,ACTUALOTTIME As ActualOTTime FROM fnGetOT ( '" + StaffId + "','" + fromdate + "','" + todate + "','" + Category + "')");
            qryStr.Append(" Exec [dbo].[GetExtraHoursWorkedForApproval]  '" + StaffId + "','" + fromdate + "','" + todate + "'");
            try
            {
                var lstGrp = context.Database.SqlQuery<AttendanceDataView>(qryStr.ToString())
                    .Select(d => new AttendanceDataView()
                    {
                        STAFFID = d.STAFFID,
                        FirstName = d.FirstName,
                        TXNDATE = d.TXNDATE,
                        ShiftShortName = d.ShiftShortName,
                        ShiftInTime = d.ShiftInTime,
                        ShiftOutTime = d.ShiftOutTime,
                        ActualInTime = d.ActualInTime,
                        ActualOutTime = d.ActualOutTime,
                        ActualExtraHoursWorked = d.ActualExtraHoursWorked
                    }).ToList();
                if (lstGrp == null)
                {
                    return new List<AttendanceDataView>();
                }
                else
                {
                    return lstGrp;
                }
            }
            catch (Exception e)
            {
                return new List<AttendanceDataView>();
                throw e;
            }
        }
        #region Employee Master Summary Reports
        public List<EmployeeMasterSummaryReportModel> EmployeeMasterSummaryReportRepository(string staffList)
        {
            List<EmployeeMasterSummaryReportModel> lst = new List<EmployeeMasterSummaryReportModel>();
            try
            {
                builder.Append("Exec GetEmployeeMasterSummaryReportV1 @staffList");
                lst = context.Database.SqlQuery<EmployeeMasterSummaryReportModel>(builder.ToString(), new SqlParameter("@staffList", staffList)).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        public List<LeaveRequisitionHistoryModel> GetLeaveRequisitionHistoryRepot(string beginningdate, string endingdate, string stafflist)
        {
            var qryStr = new StringBuilder();
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@stafflist", stafflist);
            param[1] = new SqlParameter("@beginningdate", beginningdate);
            param[2] = new SqlParameter("@endingdate", endingdate);
            qryStr.Append("Exec GetLeaveRequisitionHistoryReport @stafflist , @beginningdate , @endingdate");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<LeaveRequisitionHistoryModel>(qryStr.ToString(), param).Select(d => new LeaveRequisitionHistoryModel()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    Department = d.Department,
                    Designation = d.Designation,
                    LeaveType = d.LeaveType,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    StartDuration = d.StartDuration,
                    EndDuration = d.EndDuration,
                    TotalDays = d.TotalDays,
                    ApprovalStatus = d.ApprovalStatus,
                    ApprovalStatusId = d.ApprovalStatusId,
                    ApplicationDate = d.ApplicationDate,
                    ApprovedBy = d.ApprovedBy,
                    ApprovedOn = d.ApprovedOn,
                    Reason = d.Reason

                }).ToList();

                if (lst == null)
                {
                    return new List<LeaveRequisitionHistoryModel>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LeaveRequisitionHistoryModel>();
            }
        }
        public List<CoffAvailingModel> GetCompOffAvailedReport(string beginningdate, string endingdate, string stafflist)
        {
            var qryStr = new StringBuilder();
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@stafflist", stafflist);
            param[1] = new SqlParameter("@beginningdate", beginningdate);
            param[2] = new SqlParameter("@endingdate", endingdate);
            qryStr.Append("Exec GetCompOffAvailedReport @stafflist , @beginningdate , @endingdate");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<CoffAvailingModel>(qryStr.ToString(), param).Select(d => new CoffAvailingModel()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    Department = d.Department,
                    Designation = d.Designation,
                    WorkedDate = d.WorkedDate,
                    StartDate = d.StartDate,
                    StartDuration = d.StartDuration,
                    EndDate = d.EndDate,
                    EndDuration = d.EndDuration,
                    AppliedDate = d.AppliedDate,
                    ApprovalStatus = d.ApprovalStatus,
                    ApprovedDate = d.ApprovedDate,
                    ApprovalOwner = d.ApprovalOwner
                }).ToList();

                if (lst == null)
                {
                    return new List<CoffAvailingModel>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<CoffAvailingModel>();
            }
        }
        public List<CurrentDayInSwipModel> GetCurrentDayInSwipeReport(string beginningdate, string endingdate, string stafflist)
        {
            var qryStr = new StringBuilder();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@stafflist", stafflist);
            qryStr.Append("Exec GetCurrentDayInSwipeReport @stafflist");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<CurrentDayInSwipModel>(qryStr.ToString(), param).ToList();
                if (lst == null)
                {
                    return new List<CurrentDayInSwipModel>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<CurrentDayInSwipModel>();
            }
        }
        #endregion
        #region Daimler
        public List<DeptWiseGenderHeadCount> GetDeptWiseGenderHeadCount(string fromdate, string todate)
        {
            List<DeptWiseGenderHeadCount> lst = new List<DeptWiseGenderHeadCount>();
            try
            {
                SqlParameter[] Param = new SqlParameter[2];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@ToDate", todate);

                builder = new StringBuilder();
                builder.Append("Exec [Dbo].[GetMaleFemaleHCSummaryReportV1]  @FromDate,@ToDate");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<DeptWiseGenderHeadCount>(builder.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }

        public List<DrawAttendanceYearlySummary> GetAttendanceYearlySummary(string fromdate, string stafflist)
        {
            List<DrawAttendanceYearlySummary> lst = new List<DrawAttendanceYearlySummary>();
            try
            {
                SqlParameter[] Param = new SqlParameter[2];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@stafflist", stafflist);

                builder = new StringBuilder();
                builder.Append("Exec [dbo].[DrawYearlyAttendanceSummary] @stafflist,@FromDate");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<DrawAttendanceYearlySummary>(builder.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }

        public List<ShiftSummaryReport> GetShiftSummaryReport(string fromdate, string todate, string stafflist)
        {
            List<ShiftSummaryReport> lst = new List<ShiftSummaryReport>();
            try
            {
                SqlParameter[] Param = new SqlParameter[3];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@todate", todate);
                Param[2] = new SqlParameter("@stafflist", stafflist);

                builder = new StringBuilder();
                builder.Append("Exec [dbo].[GetShiftSummaryReportV1] @stafflist,@FromDate,@todate ");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<ShiftSummaryReport>(builder.ToString(), Param).ToList();
            }
            catch 
            {
                return new List<ShiftSummaryReport>();
            }
            return lst;
        }
        public List<LateInEarlyOutReport> GetLateInEarlyOutReport(string fromdate, string todate, string stafflist)
        {
            List<LateInEarlyOutReport> lst = new List<LateInEarlyOutReport>();
            try
            {
                SqlParameter[] Param = new SqlParameter[3];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@todate", todate);
                Param[2] = new SqlParameter("@stafflist", stafflist);

                builder = new StringBuilder();
                builder.Append("Exec [GetLateSwipeInandEarlySwipeOutReportV1] @stafflist,@FromDate,@todate ");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<LateInEarlyOutReport>(builder.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        public List<DrawFORMS> GetFORM_S(string stafflist)
        {
            List<DrawFORMS> lst = new List<DrawFORMS>();
            try
            {
                builder = new StringBuilder();
                builder.Append("Exec [DBO].[DrawFormS] @stafflist ");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<DrawFORMS>(builder.ToString(), new SqlParameter("@stafflist", stafflist)).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        public List<DrawFORM10> GetFORM10(string fromdate, string todate, string stafflist)
        {
            List<DrawFORM10> lst = new List<DrawFORM10>();
            try
            {
                var qryStr = new StringBuilder();
                qryStr.Append("Exec [DBO].[DrawForm10] @StaffList,@FromDate,@ToDate ");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<DrawFORM10>(qryStr.ToString(), new SqlParameter("@StaffList", stafflist), new SqlParameter("@Fromdate", fromdate), new SqlParameter("@ToDate", todate)).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        public List<ExtraHoursWorked> GetExtraHoursWorkedDetails(string fromdate, string todate, string stafflist)
        {
            List<ExtraHoursWorked> lst = new List<ExtraHoursWorked>();
            try
            {
                SqlParameter[] Param = new SqlParameter[3];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@ToDate", todate);
                Param[2] = new SqlParameter("@stafflist", stafflist);

                stafflist = stafflist.Replace("','", ",");
                builder = new StringBuilder();
                builder.Append("SELECT * FROM fnGetExtraHoursWorkedStatement1  ( @stafflist, @FromDate , @todate )");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<ExtraHoursWorked>(builder.ToString(), Param).ToList();
            }
            catch (Exception)
            {
                return new List<ExtraHoursWorked>();
            }
            return lst;
        }
        public List<HWOWokingReport> GetHWOWokingReport(string beginningdate, string endingdate, string stafflist)
        {
            List<HWOWokingReport> lst = new List<HWOWokingReport>();
            try
            {
                builder = new StringBuilder();
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@Flag", 1);
                param[1] = new SqlParameter("@FilterValue", "BR0002");
                param[2] = new SqlParameter("@beginningdate", beginningdate);
                param[3] = new SqlParameter("@endingdate", endingdate);
                builder.Append("Exec GetWeekOff_HolidayWorkingReport @Flag, @FilterValue, @beginningdate ,@endingdate");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<HWOWokingReport>(builder.ToString(), param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        public List<LeaveSummaryReport> GetLeaveSummaryReport(string Year, string stafflist)
        {
            List<LeaveSummaryReport> lst = new List<LeaveSummaryReport>();
            try
            {
                builder = new StringBuilder();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@stafflist", stafflist);
                param[1] = new SqlParameter("@Year", Year);
                builder.Append("Exec GetLeaveSummaryReport @stafflist, @Year");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<LeaveSummaryReport>(builder.ToString(), param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        public List<EmployeeAdditionDeletionReport> GetEmpAdditionDeletionReport(string beginningdate, string endingdate)
        {
            List<EmployeeAdditionDeletionReport> lst = new List<EmployeeAdditionDeletionReport>();
            try
            {
                builder = new StringBuilder();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@beginningdate", beginningdate);
                param[1] = new SqlParameter("@endingdate", endingdate);
                param[2] = new SqlParameter("@FilterValue", "Both");
                builder.Append("Exec GetEmployeeAdditionDeletionReportV1 @beginningdate, @endingdate, @FilterValue ");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<EmployeeAdditionDeletionReport>(builder.ToString(), param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        #endregion
        #region Swing Stetter
        public List<SalaryOTSAfinal> GetSalaryOTSAfinalDetails(string fromdate, string todate, string stafflist)  //Changes Pending
        {
            List<SalaryOTSAfinal> lst = new List<SalaryOTSAfinal>();
            try
            {
                SqlParameter[] Param = new SqlParameter[3];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@ToDate", todate);
                Param[2] = new SqlParameter("@Stafflist", stafflist);

                builder = new StringBuilder();
                builder.Append("exec [dbo].[SALARYOTSAFINAL] @FromDate,@ToDate ,@Stafflist");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<SalaryOTSAfinal>(builder.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        public List<ShiftAllowance> GetShiftAllowance(string fromdate, string todate, string stafflist)
        {
            List<ShiftAllowance> lst = new List<ShiftAllowance>();
            try
            {
                SqlParameter[] Param = new SqlParameter[3];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@ToDate", todate);
                Param[2] = new SqlParameter("@Stafflist", stafflist);

                builder = new StringBuilder();
                builder.Append("exec SHIFTALLOWANCE  @FromDate , @ToDate,@Stafflist ");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<ShiftAllowance>(builder.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }

        public List<YearlyReportShiftDetails> GetYearlyReportShiftDetails(string fromdate, string todate, string stafflist)
        {
            List<YearlyReportShiftDetails> lst = new List<YearlyReportShiftDetails>();
            try
            {
                SqlParameter[] Param = new SqlParameter[3];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@ToDate", todate);
                Param[2] = new SqlParameter("@Stafflist", stafflist);

                builder = new StringBuilder();
                builder.Append("exec [YEARLYSHIFTDETAILS]  @FromDate , @ToDate, @Stafflist ");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<YearlyReportShiftDetails>(builder.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }

        public List<YearlyReportWorkedDays> GetYearlyReportWorkedDays(string fromdate, string todate, string stafflist)
        {
            List<YearlyReportWorkedDays> lst = new List<YearlyReportWorkedDays>();
            try
            {
                SqlParameter[] Param = new SqlParameter[3];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@ToDate", todate);
                Param[2] = new SqlParameter("@Stafflist", stafflist);

                builder = new StringBuilder();
                builder.Append("exec  [dbo].[YEARLYWORKEDDAYS]  @FromDate, @ToDate	, @Stafflist");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<YearlyReportWorkedDays>(builder.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }

        public List<YearlyOTReport> GetYearlyOTReport(string fromdate, string todate, string stafflist)
        {
            List<YearlyOTReport> lst = new List<YearlyOTReport>();
            try
            {
                SqlParameter[] Param = new SqlParameter[3];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@ToDate", todate);
                Param[2] = new SqlParameter("@Stafflist", stafflist);

                builder = new StringBuilder();
                builder.Append("exec [YEARLYREPORTOT]  @FromDate ,@ToDate, @Stafflist ");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<YearlyOTReport>(builder.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        public List<LeaveReport> GetLeaveReport(string fromdate, string todate, string stafflist)
        {
            List<LeaveReport> lst = new List<LeaveReport>();
            try
            {
                SqlParameter[] Param = new SqlParameter[3];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@ToDate", todate);
                Param[2] = new SqlParameter("@Stafflist", stafflist);

                builder = new StringBuilder();
                builder.Append("exec [LeaveReport]  @FromDate , @ToDate	, @Stafflist ");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<LeaveReport>(builder.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        public List<ShopFloorAttendance> GetShopFloorAttendance(string fromdate, string todate, string stafflist)
        {
            List<ShopFloorAttendance> lst = new List<ShopFloorAttendance>();
            try
            {
                SqlParameter[] Param = new SqlParameter[3];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@ToDate", todate);
                Param[2] = new SqlParameter("@Stafflist", stafflist);

                builder = new StringBuilder();
                builder.Append("exec [dbo].[SHOPFLOORATTENDANCE]  @FromDate , @ToDate, @Stafflist ");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<ShopFloorAttendance>(builder.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        public List<DailyAttendance_Trd_Dep_Cat> GetTradeWiseHC(string fromdate, string todate)
        {
            List<DailyAttendance_Trd_Dep_Cat> lst = new List<DailyAttendance_Trd_Dep_Cat>();
            try
            {
                SqlParameter[] Param = new SqlParameter[2];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@ToDate", todate);

                builder = new StringBuilder();
                builder.Append("exec [dbo].[DESIGWISEHC]  @FromDate , @ToDate");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<DailyAttendance_Trd_Dep_Cat>(builder.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        public List<DailyAttendance_Trd_Dep_Cat> GetDepartmentWiseHC(string fromdate, string todate)
        {
            List<DailyAttendance_Trd_Dep_Cat> lst = new List<DailyAttendance_Trd_Dep_Cat>();
            try
            {
                SqlParameter[] Param = new SqlParameter[2];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@ToDate", todate);

                builder = new StringBuilder();
                builder.Append("exec [dbo].[DEPARTMENTWISEHC]  @FromDate, @ToDate");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<DailyAttendance_Trd_Dep_Cat>(builder.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        public List<DailyAttendance_Trd_Dep_Cat> GetCategoryWiseHC(string fromdate, string todate)
        {
            List<DailyAttendance_Trd_Dep_Cat> lst = new List<DailyAttendance_Trd_Dep_Cat>();
            try
            {
                SqlParameter[] Param = new SqlParameter[2];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@ToDate", todate);

                builder = new StringBuilder();
                builder.Append("exec [dbo].[CATEGORYWISEHC]  @FromDate ,@ToDate");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<DailyAttendance_Trd_Dep_Cat>(builder.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        public List<DailyAttendance_Trd_Dep_Cat> GetCategoryWiseAbsenteeism(string fromdate, string todate)
        {
            List<DailyAttendance_Trd_Dep_Cat> lst = new List<DailyAttendance_Trd_Dep_Cat>();
            try
            {
                SqlParameter[] Param = new SqlParameter[2];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@ToDate", todate);

                builder = new StringBuilder();
                builder.Append("exec [dbo].[CATEGORYWISEHC]  @FromDate , @ToDate");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<DailyAttendance_Trd_Dep_Cat>(builder.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        public List<ShopFloorAttendanceMonthWise> GetShoopFloorAttendanceMonthWise(string fromdate, string todate, string stafflist)
        {
            List<ShopFloorAttendanceMonthWise> lst = new List<ShopFloorAttendanceMonthWise>();
            try
            {
                SqlParameter[] Param = new SqlParameter[3];
                Param[0] = new SqlParameter("@FromDate", fromdate);
                Param[1] = new SqlParameter("@ToDate", todate);
                Param[2] = new SqlParameter("@Stafflist", stafflist);

                builder = new StringBuilder();
                builder.Append("exec [dbo].[MONTHLYSHOPFLOORATTENDANCE]  @FromDate , @ToDate, @Stafflist");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 240;
                lst = context.Database.SqlQuery<ShopFloorAttendanceMonthWise>(builder.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }

        #endregion
        public List<PermissionRequisitionHistory> GetPermissionRequisitionHistory(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Exec [dbo].[GetPermissionRequisitionHistory]  '" + stafflist + "' , '" + fromdate + "' , '" + todate + "' ");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<PermissionRequisitionHistory>(qryStr.ToString()).Select(d => new PermissionRequisitionHistory()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    Department = d.Department,
                    Designation = d.Designation,
                    PermissionDate = d.PermissionDate,
                    PermissionType = d.PermissionType,
                    TotalHours = d.TotalHours,
                    Reason = d.Reason,
                    ApprovalStatus = d.ApprovalStatus,
                    ApprovedOn = d.ApprovedOn,
                    ApprovedBy = d.ApprovedBy,
                    ReviewalStatus = d.ReviewalStatus,
                    ReviewedBy = d.ReviewedBy,
                    ReviewedOn = d.ReviewedOn,
                }).ToList();
                if (lst == null)
                {
                    return new List<PermissionRequisitionHistory>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<PermissionRequisitionHistory>();
            }
        }
        public List<COffCreditReportModel> COffCreditReqReportRepository(string StaffList, string FromDate, string ToDate)
        {
            List<COffCreditReportModel> lst = new List<COffCreditReportModel>();
            try
            {
                SqlParameter[] Param = new SqlParameter[3];
                Param[0] = new SqlParameter("@StaffList", StaffList);
                Param[1] = new SqlParameter("@FromDate", FromDate);
                Param[2] = new SqlParameter("@ToDate", ToDate);

                StringBuilder builder = new StringBuilder();
                builder.Append("Exec [dbo].[GetCoffCreditRequisitionHistoryReport]  @StaffList,@FromDate,@ToDate");
                lst = context.Database.SqlQuery<COffCreditReportModel>(builder.ToString(), Param).ToList();
            }
            catch 
            {
                return new List<COffCreditReportModel>();
            }
            return lst;
        }

        public List<BusinessTravelReportModel> GetBusinessTravelReportRepository(string StaffId, string fromdate, string todate)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@StaffList", StaffId);
            param[1] = new SqlParameter("@FromDate", fromdate);
            param[2] = new SqlParameter("@ToDate", todate);


            var qryStr = new StringBuilder();
            qryStr.Append("Exec [Dbo].[GetBusinessTravelRequestHistory] @StaffList,@FromDate,@ToDate");
            try
            {
                var lstGrp = context.Database.SqlQuery<BusinessTravelReportModel>(qryStr.ToString(), param).ToList();
                if (lstGrp == null)
                {
                    return new List<BusinessTravelReportModel>();
                }
                else
                {
                    return lstGrp;
                }
            }
            catch (Exception e)
            {
                return new List<BusinessTravelReportModel>();
                throw e;
            }
        }
        public List<CommonPermissionReportModel> GetCommonPermissionReportRepository(string StaffId, string fromdate, string todate)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@StaffList", StaffId);
            param[1] = new SqlParameter("@FromDate", fromdate);
            param[2] = new SqlParameter("@ToDate", todate);

            var qryStr = new StringBuilder();
            qryStr.Append("Exec [Dbo].[GetCommonPermissionRequestHistory] @StaffList,@FromDate,@ToDate");
            try
            {
                var lstGrp = context.Database.SqlQuery<CommonPermissionReportModel>(qryStr.ToString(), param).ToList();
                if (lstGrp == null)
                {
                    return new List<CommonPermissionReportModel>();
                }
                else
                {
                    return lstGrp;
                }
            }
            catch (Exception e)
            {
                return new List<CommonPermissionReportModel>();
                throw e;
            }
        }
        public List<LeaveBalance> GetLeaveBalanceReport(string StaffId, string todate)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@StaffList", StaffId);
            param[1] = new SqlParameter("@Date", todate);

            var qryStr = new StringBuilder();
            qryStr.Append("Exec [DBO].[GetLeaveBalanceReport] @StaffList,@Date");
            try
            {
                var lstGrp = context.Database.SqlQuery<LeaveBalance>(qryStr.ToString()
                    , new SqlParameter("@StaffList", StaffId), new SqlParameter("@Date", todate)).ToList();
                return lstGrp;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<LeaveAvailedReport> GetLeaveAvailedReport(string StaffId, string fromdate, string todate)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@FROMDATE", fromdate);
            param[1] = new SqlParameter("@TODATE", todate); 
             param[2] = new SqlParameter("@STAFFLIST", StaffId); 

             var qryStr = new StringBuilder();
            qryStr.Append("EXEC LeaveTakenReport @FROMDATE,@TODATE,@STAFFLIST");
            //qryStr.Append("EXEC LeaveTakenReport '"+ fromdate + "','" + todate + "','" + StaffId + "'");
            try
            {
                var lstGrp = context.Database.SqlQuery<LeaveAvailedReport>(qryStr.ToString(), param).ToList();
                if (lstGrp == null)
                {
                    return new List<LeaveAvailedReport>();
                }
                else
                {
                    return lstGrp;
                }
            }
            catch
            {
                return new List<LeaveAvailedReport>();
            }
        }
        public List<LeaveTransactionDetails> GetLeaveTransactionDetails(string stafflist, string fromdate, string todate)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Exec [dbo].[GetLeaveTransactionDetails]  @StaffList ,'', @FromDate , @ToDate ");
            try
            {
                var list = context.Database.SqlQuery<LeaveTransactionDetails>(queryString.ToString(), new SqlParameter("@StaffList", stafflist)
                , new SqlParameter("@FromDate", fromdate), new SqlParameter("@ToDate", todate)).Select(d => new LeaveTransactionDetails()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    Location = d.Location,
                    Branch = d.Branch,
                    Department = d.Department,
                    Division = d.Division,
                    Designation = d.Designation,
                    Grade = d.Grade,
                    Category = d.Category,
                    CostCentre = d.CostCentre,
                    Volume = d.Volume,
                    LeaveType = d.LeaveType,
                    LeaveCount = d.LeaveCount,
                    TransactionType = d.TransactionType,
                    TransactionDate = d.TransactionDate,
                    Narration = d.Narration,
                    Year = d.Year,
                    Month = d.Month,
                    IsSystemAction = d.IsSystemAction,
                    TransactionBy = d.TransactionBy

                }).ToList();
                if (list == null)
                {
                    return new List<LeaveTransactionDetails>();
                }
                else
                {
                    return list;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<WorkFromHome> GetWorkFromHomeRequisition(string StaffId, string fromdate, string todate)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@StaffList", StaffId);
            param[1] = new SqlParameter("@FromDate", fromdate);
            param[2] = new SqlParameter("@ToDate", todate);


            var qryStr = new StringBuilder();
            qryStr.Append("Exec [Dbo].[GetWorkFromHomeRequisitionHistory] @StaffList,@FromDate,@ToDate");
            try
            {
                var lstGrp = context.Database.SqlQuery<WorkFromHome>(qryStr.ToString(), param).ToList();
                if (lstGrp == null)
                {
                    return new List<WorkFromHome>();
                }
                else
                {
                    return lstGrp;
                }
            }
            catch 
            {
                return new List<WorkFromHome>();
            }
        }
        public List<LeaveBalanceReport> GetLeaveBalanceHistoryReport(string stafflist, string endingdate)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Exec [spLeaveBalanceReport] '" + stafflist + "'");
            try
            {
                var list = context.Database.SqlQuery<LeaveBalanceReport>(queryString.ToString()).Select(d => new LeaveBalanceReport()
                {

                    StaffId = d.StaffId,
                    Name = d.Name,
                    Department = d.Department,
                    Designation = d.Designation,
                    Grade = d.Grade,
                    Category = d.Category,
                    LeaveTypeName = d.LeaveTypeName,
                    LapseCount = d.LapseCount,
                    Encashment = d.Encashment,
                    ClosingBalance = d.ClosingBalance,
                    OpeningCredits = d.OpeningCredits,
                    LeaveAvailed = d.LeaveAvailed,
                    CurrentBalance = d.CurrentBalance,
                }).ToList();

                if (list == null)
                {
                    return new List<LeaveBalanceReport>();
                }
                else
                {
                    return list;
                }
            }
            catch 
            {
                return new List<LeaveBalanceReport>();
            }
        }
        public List<GetCompOffLapsReport> GetCompOffLapsReport(string stafflist, string fromDate, string toDate)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Exec [Dbo].[GetCompOffLapseReport]  '" + stafflist + "' , '" + fromDate + "' , '" + toDate + "' ");
            try
            {
                var list = context.Database.SqlQuery<GetCompOffLapsReport>(queryString.ToString()).Select(d => new GetCompOffLapsReport()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    Department = d.Department,
                    Designation = d.Designation,
                    ReportingManager = d.ReportingManager,
                    WorkedDate = d.WorkedDate,
                    Credit = d.Credit,
                    ExpiryDate = d.ExpiryDate
                }).ToList();
                if (list == null)
                {
                    return new List<GetCompOffLapsReport>();
                }
                else
                {
                    return list;
                }
            }
            catch
            {
                return new List<GetCompOffLapsReport>();
            }
        }
        public List<AutoLeaveDeduction> GetLeaveAutoDeductionReport(string stafflist, string fromDate, string toDate)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Exec [dbo].[GetAutoLeaveDuductionReport]  '" + stafflist + "' , '" + fromDate + "' , '" + toDate + "' ");
            try
            {
                var list = context.Database.SqlQuery<AutoLeaveDeduction>(queryString.ToString()).Select(d => new AutoLeaveDeduction()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    Department = d.Department,
                    Designation = d.Designation,
                    ReportingManager = d.ReportingManager,
                    TxnDate = d.TxnDate,
                    LeaveType = d.LeaveType,
                    LeaveCount = d.LeaveCount,
                    DeductedOn = d.DeductedOn
                }).ToList();
                if (list == null)
                {
                    return new List<AutoLeaveDeduction>();
                }
                else
                {
                    return list;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<OffRoleAttendanceReport> GetOffRoleAttendanceReport(string staffList, string fromDate, string toDate)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Exec [dbo].[GetOffRoleAttendance] @StaffList , @FromDate , @ToDate");
            try
            {
                var list = context.Database.SqlQuery<OffRoleAttendanceReport>(queryString.ToString(), new SqlParameter("@StaffList", staffList),
                new SqlParameter("@FromDate", fromDate), new SqlParameter("@ToDate", toDate)).Select(d => new OffRoleAttendanceReport()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    CostCentre = d.CostCentre,
                    Category = d.Category,
                    Department = d.Department,
                    Designation = d.Designation,
                    ReportingManager = d.ReportingManager,
                    TotalNoOfWorkingDays = d.TotalNoOfWorkingDays,
                    NoOfDaysPresent = d.NoOfDaysPresent,
                    NoOfDaysAbsent = d.NoOfDaysAbsent,
                    NoOfDaysWeeklyOff = d.NoOfDaysWeeklyOff,
                    NoOfDaysHoliday = d.NoOfDaysHoliday
                }).ToList();
                if (list == null)
                {
                    return null;
                }
                else
                {
                    return list;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<LeaveNightShiftAllowance> GetNightShiftAllowanceDetails(string stafflist, string fromDate, string toDate)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Exec [GetNightShiftAllowanceDetails]  '" + stafflist + "' , '" + fromDate + "' , '" + toDate + "' ");
            try
            {
                var list = context.Database.SqlQuery<LeaveNightShiftAllowance>(queryString.ToString()).Select(d => new LeaveNightShiftAllowance()
                {

                    StaffId = d.StaffId,
                    Name = d.Name,
                    Plant = d.Plant,
                    Department = d.Department,
                    Division = d.Division,
                    ReportingManager = d.ReportingManager,
                    AttendanceDate = d.AttendanceDate,
                    AttendanceStatus = d.AttendanceStatus,
                    NightShiftCount = d.NightShiftCount
                }).ToList();
                if (list == null)
                {
                    return new List<LeaveNightShiftAllowance>();
                }
                else
                {
                    return list;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<FormQ> GetFormQ(string fromdate, string todate, string stafflist)
        {

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Exec [dbo].[GetFormQReport]  '" + fromdate + "' , '" + todate + "'	, '" + stafflist + "'");
            try
            {
                this.context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<FormQ>(qryStr.ToString()).Select(d => new FormQ()
                {
                    StaffId = d.StaffId,
                    NAME = d.NAME,
                    DOJ = d.DOJ,
                    DOB = d.DOB,
                    DEPARTMENT = d.DEPARTMENT,
                    Designation = d.Designation,
                    Day1 = d.Day1,
                    Day2 = d.Day2,
                    Day3 = d.Day3,
                    Day4 = d.Day4,
                    Day5 = d.Day5,
                    Day6 = d.Day6,
                    Day7 = d.Day7,
                    Day8 = d.Day8,
                    Day9 = d.Day9,
                    Day10 = d.Day10,
                    Day11 = d.Day11,
                    Day12 = d.Day12,
                    Day13 = d.Day13,
                    Day14 = d.Day14,
                    Day15 = d.Day15,
                    Day16 = d.Day16,
                    Day17 = d.Day17,
                    Day18 = d.Day18,
                    Day19 = d.Day19,
                    Day20 = d.Day20,
                    Day21 = d.Day21,
                    Day22 = d.Day22,
                    Day23 = d.Day23,
                    Day24 = d.Day24,
                    Day25 = d.Day25,
                    Day26 = d.Day26,
                    Day27 = d.Day27,
                    Day28 = d.Day28,
                    Day29 = d.Day29,
                    Day30 = d.Day30,
                    Day31 = d.Day31,
                    Day1TotalHours = d.Day1TotalHours,
                    Day2TotalHours = d.Day2TotalHours,
                    Day3TotalHours = d.Day3TotalHours,
                    Day4TotalHours = d.Day4TotalHours,
                    Day5TotalHours = d.Day5TotalHours,
                    Day6TotalHours = d.Day6TotalHours,
                    Day7TotalHours = d.Day7TotalHours,
                    Day8TotalHours = d.Day8TotalHours,
                    Day9TotalHours = d.Day9TotalHours,
                    Day10TotalHours = d.Day10TotalHours,
                    Day11TotalHours = d.Day11TotalHours,
                    Day12TotalHours = d.Day12TotalHours,
                    Day13TotalHours = d.Day13TotalHours,
                    Day14TotalHours = d.Day14TotalHours,
                    Day15TotalHours = d.Day15TotalHours,
                    Day16TotalHours = d.Day16TotalHours,
                    Day17TotalHours = d.Day17TotalHours,
                    Day18TotalHours = d.Day18TotalHours,
                    Day19TotalHours = d.Day19TotalHours,
                    Day20TotalHours = d.Day20TotalHours,
                    Day21TotalHours = d.Day21TotalHours,
                    Day22TotalHours = d.Day22TotalHours,
                    Day23TotalHours = d.Day23TotalHours,
                    Day24TotalHours = d.Day24TotalHours,
                    Day25TotalHours = d.Day25TotalHours,
                    Day26TotalHours = d.Day26TotalHours,
                    Day27TotalHours = d.Day27TotalHours,
                    Day28TotalHours = d.Day28TotalHours,
                    Day29TotalHours = d.Day29TotalHours,
                    Day30TotalHours = d.Day30TotalHours,
                    Day31TotalHours = d.Day31TotalHours,
                    Opening_CLBalance_CurrentMonth = d.Opening_CLBalance_CurrentMonth,
                    Opening_PLBalance_CurrentMonth = d.Opening_PLBalance_CurrentMonth,
                    Opening_SLBalance_CurrentMonth = d.Opening_SLBalance_CurrentMonth,
                    SumOf_CL_Availed_CurrentMonth = d.SumOf_CL_Availed_CurrentMonth,
                    SumOfSL_Availed_CurrentMonth = d.SumOfSL_Availed_CurrentMonth,
                    SumOfBL_Availed_CurrentMonth = d.SumOfBL_Availed_CurrentMonth,
                    SumOfPL_Availed_CurrentMonth = d.SumOfPL_Availed_CurrentMonth,
                    SumOfML_Availed_CurrentMonth = d.SumOfML_Availed_CurrentMonth,
                    CL_Closing_Balance = d.CL_Closing_Balance,
                    PL_Closing_Balance = d.PL_Closing_Balance,
                    SL_Closing_Balance = d.SL_Closing_Balance,
                    SumOfUnApprovedAndApprovedLeave = d.SumOfUnApprovedAndApprovedLeave,
                    TotalHours_OT_InCurrentMonth = d.TotalHours_OT_InCurrentMonth1,
                    TotalHours_Workded_InMonth = d.TotalHours_Workded_InMonth,
                }).ToList();

                if (lst == null)
                {
                    return new List<FormQ>();
                }
                else
                {
                    return lst;
                }
            }
            catch
            {
                return new List<FormQ>();
            }
        }
        public List<ManualAttendanceStatusChange> GetManualAttendanceStatusChange(string stafflist, string fromDate, string toDate)
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Exec [Dbo].[GetManualAttStatusChangeReport]  '" + stafflist + "' , '" + fromDate + "' , '" + toDate + "' ");
            try
            {
                var list = context.Database.SqlQuery<ManualAttendanceStatusChange>(queryString.ToString()).Select(d => new ManualAttendanceStatusChange()
                {

                    StaffId = d.StaffId,
                    Name = d.Name,
                    Department = d.Department,
                    Division = d.Division,
                    Designation = d.Designation,
                    TxnDate = d.TxnDate,
                    AttStatus = d.AttStatus
                }).ToList();
                if (list == null)
                {
                    return new List<ManualAttendanceStatusChange>();
                }
                else
                {
                    return list;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<ShiftExtension> GetShiftExtensionReport(string StaffId, string fromdate, string todate)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@StaffList", StaffId);
            param[1] = new SqlParameter("@FromDate", fromdate);
            param[2] = new SqlParameter("@ToDate", todate);


            var qryStr = new StringBuilder();
            qryStr.Append("Exec [Dbo].[GetShiftExtensionRequisitionHistory] @StaffList,@FromDate,@ToDate");
            try
            {
                var lstGrp = context.Database.SqlQuery<ShiftExtension>(qryStr.ToString(), param).ToList();
                if (lstGrp == null)
                {
                    return new List<ShiftExtension>();
                }
                else
                {
                    return lstGrp;
                }
            }
            catch
            {
                return new List<ShiftExtension>();
            }
        }
        public List<NightShiftCount> GetNightShiftCount(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Exec [GetNightShiftCount] '" + stafflist + "','" + fromdate + "','" + todate + "'");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<NightShiftCount>(qryStr.ToString()).Select(d => new NightShiftCount()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    Plant = d.Plant,
                    Department = d.Department,
                    Division = d.Division,
                    Designation = d.Designation,
                    ReportingManager = d.ReportingManager,
                    AttendanceDate = d.AttendanceDate,
                    TotalCount = d.TotalCount,
                }).ToList();

                if (lst == null)
                {
                    return new List<NightShiftCount>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<NightShiftCount>();
            }
        }
        public List<HolidayWorkingDetails> GetHolidayWorkingDetails(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Exec [spHolidayWorkingDetails] '" + stafflist + "','" + fromdate + "','" + todate + "'");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<HolidayWorkingDetails>(qryStr.ToString()).Select(d => new HolidayWorkingDetails()
                {

                    StaffId = d.StaffId,
                    Name = d.Name,
                    Department = d.Department,
                    Designation = d.Designation,
                    Grade = d.Grade,
                    Category = d.Category,
                    ReportingManager = d.ReportingManager,
                    TxnDate = d.TxnDate,
                    ShiftType = d.ShiftType,
                    ShiftInTime = d.ShiftInTime,
                    ShiftOutTime = d.ShiftOutTime,
                    ActualInTime = d.ActualInTime,
                    ActualOutTime = d.ActualOutTime,
                    ActualWorkedHours = d.ActualWorkedHours,
                    AttendanceStatus = d.AttendanceStatus,

                }).ToList();

                if (lst == null)
                {
                    return new List<HolidayWorkingDetails>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<HolidayWorkingDetails>();
            }
        }
        public List<HolidayWorkingRequisition> GetHolidayWorkingRequisitionHistory(string StaffId, string fromdate, string todate)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@StaffList", StaffId);
            param[1] = new SqlParameter("@FromDate", fromdate);
            param[2] = new SqlParameter("@ToDate", todate);

            var qryStr = new StringBuilder();
            qryStr.Append("Exec [Dbo].[GetHolidayWorkingRequisitionHistory] @StaffList,@FromDate,@ToDate");
            try
            {
                var lstGrp = context.Database.SqlQuery<HolidayWorkingRequisition>(qryStr.ToString(), param).ToList();
                if (lstGrp == null)
                {
                    return new List<HolidayWorkingRequisition>();
                }
                else
                {
                    return lstGrp;
                }
            }
            catch 
            {
                return new List<HolidayWorkingRequisition>();
            }
        }

        public List<AutoLeaveDeductionDryRun> GetAutoLeaveDeductionDryRun(string StaffId, string fromdate, string todate)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@StaffList", StaffId);
            param[1] = new SqlParameter("@FromDate", fromdate);
            param[2] = new SqlParameter("@ToDate", todate);

            var qryStr = new StringBuilder();
            qryStr.Append("Exec [Dbo].[AutoLeaveDeductionDryRun] @StaffList,@FromDate,@ToDate");
            try
            {
                var lstGrp = context.Database.SqlQuery<AutoLeaveDeductionDryRun>(qryStr.ToString(), param).ToList();
                if (lstGrp == null)
                {
                    return new List<AutoLeaveDeductionDryRun>();
                }
                else
                {
                    return lstGrp;
                }
            }
            catch 
            {
                return new List<AutoLeaveDeductionDryRun>();
            }
        }
    }
}
