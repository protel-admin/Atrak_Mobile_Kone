using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository {
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

        public ReportRepository() {
            context = new AttendanceManagementContext();
        }

        public List<MOffApplicationReport> GetMOffApplicationReport(string beginningdate, string endingdate, string stafflist)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@beginningdate", beginningdate);
            param[1] = new SqlParameter("@endingdate", endingdate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetMaintenanceOffList (@stafflist,@beginningdate@,@endingdate) ORDER BY STAFFID , TXNDATE");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<MOffApplicationReport>(qryStr.ToString(),param).Select(d => new MOffApplicationReport()
                {
                    STAFFID=d.STAFFID,
                    STAFFNAME=d.STAFFNAME,
                    TXNDATE=d.TXNDATE,
                    LEAVESHORTNAME=d.LEAVESHORTNAME,
                    MAINTENANCEOFFREASON=d.MAINTENANCEOFFREASON,
                    APPROVALSTATUSNAME=d.APPROVALSTATUSNAME,
                    APPROVALSTAFFID=d.APPROVALSTAFFID,
                    APPROVALSTAFFNAME=d.APPROVALSTAFFNAME,
                    APPROVEDONDATE=d.APPROVEDONDATE,
                    APPROVEDONTIME=d.APPROVEDONTIME,
                    APPROVALOWNWERID = d.APPROVALOWNWERID ,
                    APPROVALOWNERNAME=d.APPROVALOWNERNAME
                }).ToList();

                if(lst == null)
                {
                    return new List<MOffApplicationReport>();
                }
                else
                {
                    return lst;
                }
            }
            catch(Exception)
            {
                return new List<MOffApplicationReport>();
            }
        }

        public List<LeaveApplicationListNew> GetLeaveNewApplicationReport(string fromdate, string todate, string stafflist)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            context.Database.CommandTimeout = 0;
            qryStr.Append("SELECT * FROM [dbo].[fnGetLeaveApplicationsList] ( @stafflist )");
            try
            {
                var lst = context.Database.SqlQuery<LeaveApplicationListNew>(qryStr.ToString(),param).Select(d => new LeaveApplicationListNew()
                {
                    StaffId = d.StaffId,          
                    FirstName = d.FirstName,
                    LeaveTypeId = d.LeaveTypeId,
                    LeaveTypeName = d.LeaveTypeName,
                    LeaveStartDate = d.LeaveStartDate,
                    LeaveStartDurationId = d.LeaveStartDurationId,
                    LeaveStartDurationName = d.LeaveStartDurationName,
                    LeaveEndDate = d.LeaveEndDate,
                    LeaveEndDurationId = d.LeaveEndDurationId,
                    LeaveEndDurationName = d.LeaveEndDurationName,
                    Remarks = d.Remarks,
                    LeaveApplicationReason = d.LeaveApplicationReason,
                    ContactNumber = d.ContactNumber,
                    ApprovalStatusName = d.ApprovalStatusName,
                    ReviewerStatusName = d.ReviewerStatusName,
                    ApprovalStaffId = d.ApprovalStaffId,
                    ApprovalStaffName = d.ApprovalStaffName,
                    ApprovedOn = d.ApprovedOn,
                    ApprovalOwner = d.ApprovalOwner,
                    ReviewerOwner = d.ReviewerOwner,
                    ReviewerOwnerName = d.ReviewerOwnerName,
                    ApprovalOwnerName = d.ApprovalOwnerName,
                    IsUserCancelled = d.IsUserCancelled,
                    ApplicationDate = d.ApplicationDate,
                    TotalDays = d.TotalDays,
                    ReviewedOn = d.ReviewedOn,
                    IsReviewerCancelled = d.IsReviewerCancelled,
                    IsApproverCancelled = d.IsApproverCancelled

                   
                }).ToList();

                if(lst == null)
                {
                    return new List<LeaveApplicationListNew>();
                }
                else
                {
                    return lst;
                }

            }
            catch(Exception)
            {
                return new List<LeaveApplicationListNew>();
            }

            return null;
        }

        public List<PermissionOffReport> GetPermissionOff(string fromdate, string todate, string stafflist)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            context.Database.CommandTimeout = 0;
            qryStr.Append("select StaffId,FirstName,FromTime,TimeTo,TotalHours,Name,PermissionDate,PermissionOffReason,ApprovalStatusName,ApprovalStaffId,ApprovalStaffName as ApprovalOwner,concat(ApprovedOnDate,' ',ApprovedOnTime) as ApprovedOn,Comment,ParentType, ApprovalOwner,ISCANCELLEDWORD as IsUserCancelled,ReviewerStatusName,ReviewedBy,ReviewedOn, ReviewerOwner,ReviewerOwnerName,PermissionType,IsReviewerCancelled as IsReviewerCancelled ,IsApproverCancelled as IsApproverCancelled from vwPermissionApproval where staffid in (select * from dbo.split(@stafflist,','))");
            try
            {
                var lst = context.Database.SqlQuery<PermissionOffReport>(qryStr.ToString(),param).Select(d => new PermissionOffReport()
                {
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    FromTime = d.FromTime,
                    TimeTo = d.TimeTo,
                    TotalHours = d.TotalHours,
                    Name = d.Name,
                    PermissionDate = d.PermissionDate,
                    PermissionOffReason = d.PermissionOffReason,
                    ContactNumber = d.ContactNumber,
                    ApprovalStatusName = d.ApprovalStatusName,
                    ApprovalStaffId = d.ApprovalStaffId,
                    ApprovalStaffName = d.ApprovalStaffName,
                    ApprovedOn = d.ApprovedOn,
                    ApprovalOwner = d.ApprovalOwner,
                    IsUserCancelled = d.IsUserCancelled,
                    ReviewerStatusName = d.ReviewerStatusName,
                    ReviewedBy = d.ReviewedBy,
                    ReviewedOn = d.ReviewedOn,
                    ReviewerOwner = d.ReviewerOwner,
                    ReviewerOwnerName = d.ReviewerOwnerName,

                }).ToList();

                if (lst == null)
                {
                    return new List<PermissionOffReport>();
                }
                else
                {
                    return lst;
                }

            }
            catch (Exception e)
            {
                return new List<PermissionOffReport>();
            }
        }


        public List<LeaveApplicationReport> GetLeaveApplicationReport(string beginningdate, string endingdate, string stafflist)
        {


            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@beginningdate", beginningdate);
            param[1] = new SqlParameter("@endingdate", endingdate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetLeaveApplications ( @stafflist,@beginningdate,@endingdate) ORDER BY STAFFID , TXNDATE");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<LeaveApplicationReport>(qryStr.ToString(),param).Select(d => new LeaveApplicationReport()
                {
                    STAFFID=d.STAFFID,
                    STAFFNAME=d.STAFFNAME,
                    TXNDATE=d.TXNDATE,
                    LEAVETYPENAME=d.LEAVETYPENAME,
                    LEAVEAPPLICATIONREASON=d.LEAVEAPPLICATIONREASON,
                    APPROVALSTATUSNAME=d.APPROVALSTATUSNAME,
                    APPROVALSTAFFID=d.APPROVALSTAFFID,
                    APPROVALSTAFFNAME=d.APPROVALSTAFFNAME,
                    APPROVEDONDATE=d.APPROVEDONDATE,
                    APPROVEDONTIME=d.APPROVEDONTIME,
                    APPROVALOWNWERID = d.APPROVALOWNWERID ,
                    APPROVALOWNERNAME=d.APPROVALOWNERNAME
                }).ToList();

                if(lst == null)
                {
                    return new List<LeaveApplicationReport>();
                }
                else
                {
                    return lst;
                }
            }
            catch(Exception)
            {
                return new List<LeaveApplicationReport>();
            }
        }



        public List<CanteenReport> GetCanteenReport( string fromdate , string todate , string stafflist )
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
  

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append( "SELECT Empcode ,Name ,Plant ,Department ,Branch,Division,Grade  ,Indate ,InTime   from fnCanteenReport (@fromdate,@todate)" );
            //STAFFID,STAFFNAME,STARTDATE,LEAVESTARTDURATION,ENDDATE,LEAVEENDDURATION,CANCELLED,REMARKS,REASON,TOTALDAYS,APPROVALSTATUSNAME,APPROVEDBY,APPLTYPE,APPLICATIONDATE
            try {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<CanteenReport>( qryStr.ToString(),param).Select( d => new CanteenReport() {
                    Empcode = d.Empcode ,
                    Name = d.Name ,
                    Plant = d.Plant ,
                    Department = d.Department ,
                    Branch = d.Branch ,
                    Division = d.Division ,
                    Grade = d.Grade ,
                    Indate = d.Indate ,
                    InTime = d.InTime


                } ).ToList();

                if( lst == null ) {
                    return new List<CanteenReport>();
                } else {
                    return lst;
                }
            } catch( Exception ) {
                return new List<CanteenReport>();
            }
        }
        public List<PresentOnNFH> GetPresentOnNFH(string beginningdate, string endingdate, string stafflist)
        {

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@beginningdate", beginningdate);
            param[1] = new SqlParameter("@endingdate", endingdate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetPresentOnNFH ( @stafflist,@beginningdate,@endingdate)");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<PresentOnNFH>(qryStr.ToString(),param).Select(d => new PresentOnNFH() 
                {
                    STAFFID=d.STAFFID,
                    STAFFNAME=d.STAFFNAME,
                    TXNDATE=d.TXNDATE,
                    PRESENT=d.PRESENT,
                    WEEKLYOFF=d.WEEKLYOFF,
                    PAIDHOLIDAY=d.PAIDHOLIDAY,
                    COL1=d.COL1,
                    COL2=d.COL2,
                    COL3=d.COL3,
                    COL4=d.COL4,
                    COL5=d.COL5,
                    COL6=d.COL6,
                    COL7=d.COL7,
                    COL8=d.COL8,
                    COL9=d.COL9
                }).ToList();

                if(lst == null)
                {
                    return new List<PresentOnNFH>();
                }
                else
                {
                    return lst;
                }
            }
            catch(Exception)
            {
                return new List<PresentOnNFH>();
            }
        }

        public List<ODApplicationListNew> GetOutDoorStatement(string beginningdate, string endingdate, string stafflist)
        {
            stafflist = stafflist.Replace("','", ",");
            stafflist = stafflist.Replace("'", "");
            SqlParameter[] param = new SqlParameter[3]; 
            param[0] = new SqlParameter("@beginningdate", beginningdate);
            param[1] = new SqlParameter("@endingdate", endingdate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            //qryStr.Append("select StaffId,APPLICANTNAME,ODDuration,case when ODDuration='SINGLEDAY' then ODFromTime else ODFromDate end as ODFrom, "+ 
            //    "case when ODDuration = 'SINGLEDAY' then ODToTime else ODToDate end as ODTo, "+
            //    " OD,ODReason,ApprovalStatusName,	ApprovalStaffId,concat(ApprovedOnDate,' ',ApprovedOnTime) as ApprovedOn, ApprovalOwner,	APPROVALOWNERNAME, " +
            //    " ParentType,ISCANCELLED as IsUserCancelled,ReviewerstatusId,ReviewerstatusName,ReviewedBy,ReviewedOn,ReviewerOwner,ReviewerOwnerName,IsReviewerCancelled, " +
            //    " IsApproverCancelled from vwODApproval " +
            //    " where staffid in (select * from dbo.split('" + stafflist + "',','))");

            qryStr.Append("Exec [Dbo].[GetOnDutyRequisitionHistory] @stafflist,@beginningdate,@endingdate");


            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ODApplicationListNew>(qryStr.ToString(),param).Select(d => new ODApplicationListNew()
                {
                    StaffId = d.StaffId,
                    NAME = d.NAME,
                    ODDuration = d.ODDuration,
                    ODFromDate = d.ODFromDate,
                    ODToDate = d.ODToDate,
                    ODFromTime = d.ODFromTime,
                    ODToTime = d.ODToTime,
                    OD = d.OD,
                    Reason = d.Reason,
                    ApprovalStatus = d.ApprovalStatus,
                    ApprovalStaffId = d.ApprovalStaffId,
                    APPROVALSTAFFNAME = d.APPROVALSTAFFNAME,
                    ApprovedOn = d.ApprovedOn,
                    ApprovalOwner = d.ApprovalOwner,
                    ApprovedBy = d.ApprovedBy,
                    IsUserCancelled = d.IsUserCancelled,
                    ReviewerstatusId = d.ReviewerstatusId,
                    ReviewalStatus = d.ReviewalStatus,
                    ReviewedBy = d.ReviewedBy,
                    ReviewedOn = d.ReviewedOn,
                    ReviewerOwner = d.ReviewerOwner,
                    ReviewerOwnerName = d.ReviewerOwnerName,
                    IsReviewerCancelled = d.IsReviewerCancelled,
                    IsApproverCancelled = d.IsApproverCancelled
                }).ToList();

                if (lst == null)
                {
                    return new List<ODApplicationListNew>();
                }
                else
                {
                    return lst;
                }
            }
            catch(Exception)
            {
                return new List<ODApplicationListNew>();
            }
        }

        public List<DailyPerformance> GetDailyPerformance( string fromdate , string todate , string stafflist )
        {
           SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist); 

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Exec [dbo].[GetDailyPerformanceReport] @stafflist,@fromdate,@todate");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst =
                    context.Database.SqlQuery<DailyPerformance>(qryStr.ToString(),param).Select(d => new DailyPerformance()
                    {
                        StaffId  = d.StaffId,
                        CardCode = d.CardCode ,
                        FirstName = d.FirstName ,
                        ShiftName = d.ShiftName ,
                        DeptName = d.DeptName ,
                        GradeName = d.GradeName ,
                        DivisionName = d.DivisionName ,
                        VolumeName = d.VolumeName ,
                        ShiftInDate = d.ShiftInDate ,
                        ActualInDate = d.ActualInDate ,
                        ActualInTime = d.ActualInTime ,
                        ActualOutDate = d.ActualOutDate ,
                        ActualOutTime = d.ActualOutTime ,
                        ActualWorkedHours = d.ActualWorkedHours ,
                        AttendanceStatus = d.AttendanceStatus ,
                        AccountedEarlyComingTime = d.AccountedEarlyComingTime ,
                        AccountedLateComingTime = d.AccountedLateComingTime ,
                        AccountedEarlyGoingTime = d.AccountedEarlyGoingTime ,
                        AccountedLateGoingTime = d.AccountedLateGoingTime ,
                        AccountedOTHours = d.AccountedOTHours,
                        SystemMessage = d.SystemMessage,
                        IsDisputed = d.IsDisputed
                    }).ToList();

                if (lst == null)
                {
                    return new List<DailyPerformance>();
                }
                else
                {
                    return lst;
                }
            }
                catch (Exception E)
            {
                return new List<DailyPerformance>();
            }
        }

        public List<StaffExport> GetStaffDetails(string fromdate, string todate, string stafflist)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            if (stafflist=="1")
            { 
            qryStr.Append("select StaffId,StatusName,CardCode,FirstName,ShortName,Gender,DateOfJoining,DateOfResignation,OfficialPhone, " +
                        "OfficalEmail,CompanyName,BranchName,DeptName,DivisionName,VolumeName,DesignationName,GradeName,CategoryName,CostCentreName, "+
                        "LocationName,SecurityGroupName,LeaveGroupName,WeeklyOffName,convert(varchar,WeeklyOffIsActive) as WeeklyOffIsActive,HolidayGroupName, "+
                        "convert(varchar,HolidayGroupIsActive) as HolidayGroupIsActive,(select name from BloodGroup where id= BloodGroup) as BloodGroup," +
                        "(select name from MaritalStatus where id=MaritalStatus) as MaritalStatus,HomeAddress,HomeLocation,HomeCity,DateOfBirth,MarriageDate, "+
                        "REPORTINGMGRID,REPMGRFIRSTNAME,MedicalClaimNumber, " +
                        "Reviewer,OTReviewer,OTReportingManager,ReviewerName,OTReviewerName,OTReportingManagerName from staffview where StatusId=  @stafflist ");
            }
            else
            {
                qryStr.Append("select StaffId,StatusName,CardCode,FirstName,ShortName,Gender,DateOfJoining,DateOfResignation,OfficialPhone, " +
                        "OfficalEmail,CompanyName,BranchName,DeptName,DivisionName,VolumeName,DesignationName,GradeName,CategoryName,CostCentreName, " +
                        "LocationName,SecurityGroupName,LeaveGroupName,WeeklyOffName,convert(varchar,WeeklyOffIsActive) as WeeklyOffIsActive,HolidayGroupName, " +
                        "convert(varchar,HolidayGroupIsActive) as HolidayGroupIsActive,(select name from BloodGroup where id= BloodGroup) as BloodGroup," +
                        "(select name from MaritalStatus where id=MaritalStatus) as MaritalStatus,HomeAddress,HomeLocation,HomeCity,DateOfBirth,MarriageDate, " +
                        "REPORTINGMGRID,REPMGRFIRSTNAME,MedicalClaimNumber, " +
                        "Reviewer,OTReviewer,OTReportingManager,ReviewerName,OTReviewerName,OTReportingManagerName from staffview ");
            }
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<StaffExport>(qryStr.ToString(),param).ToList();

                if (lst == null)
                {
                    return new List<StaffExport>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception e)
            {
                return new List<StaffExport>();
            }
        }

        public List<PresentList> GetAttendanceLists( string fromdate , string todate , string stafflist )
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append( "select StaffId , FirstName , ShiftId , ShiftName , ShiftInDate , CompanyName , DepartmentName ,Designation," +
                           "GradeName , ActualInDate , ActualInTime , " +
                           "ActualOutDate , ActualOutTime , ActualWorkedHours , AttendanceStatus , DateOfJoining , DateOfResignation " +
                           "from fnGetPresentList (@fromdate,@todate,@stafflist,'') " );

            try {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<PresentList>( qryStr.ToString(),param).Select( d => new PresentList() {
                    StaffId = d.StaffId ,
                    FirstName = d.FirstName ,
                    ShiftId = d.ShiftId ,
                    ShiftName = d.ShiftName ,
                    CompanyName = d.CompanyName ,
                    DepartmentName = d.DepartmentName ,
                    Designation = d.Designation ,
                    GradeName = d.GradeName ,
                    ShiftInDate = d.ShiftInDate ,
                    ActualInDate = d.ActualInDate ,
                    ActualInTime = d.ActualInTime ,
                    ActualOutDate = d.ActualOutDate ,
                    ActualOutTime = d.ActualOutTime ,
                    ActualWorkedHours = d.ActualWorkedHours ,
                    AttendanceStatus = d.AttendanceStatus , 
                    DateOfJoining= d.DateOfJoining,
                    DateOfResignation = d.DateOfResignation
                } ).ToList();

                if( lst == null ) {
                    return new List<PresentList>();
                } else {
                    return lst;
                }
            } catch( Exception ) {
                return new List<PresentList>();
            }
        }

        public List<PresentList> GetAbsentLists( string fromdate , string todate , string stafflist ) {

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append( "select StaffId , FirstName , ShiftId , ShiftName , ShiftInDate , CompanyName , DepartmentName ,Designation, " +
                           "GradeName , ActualInDate , ActualInTime , " +
                           "ActualOutDate , ActualOutTime , ActualWorkedHours , AttendanceStatus  , DateOfJoining, DateOfResignation " +
                           "from fnGetPresentList (@fromdate,@todate,@stafflist,'AB,HD') " );

            try {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<PresentList>( qryStr.ToString(),param).Select( d => new PresentList() {
                    StaffId = d.StaffId ,
                    FirstName = d.FirstName ,
                    ShiftId = d.ShiftId ,
                    ShiftName = d.ShiftName ,
                    CompanyName = d.CompanyName ,
                    DepartmentName = d.DepartmentName ,
                    Designation = d.Designation,
                    GradeName = d.GradeName ,
                    ShiftInDate = d.ShiftInDate ,
                    ActualInDate = d.ActualInDate ,
                    ActualInTime = d.ActualInTime ,
                    ActualOutDate = d.ActualOutDate ,
                    ActualOutTime = d.ActualOutTime ,
                    ActualWorkedHours = d.ActualWorkedHours ,
                    AttendanceStatus = d.AttendanceStatus,
                    DateOfJoining = d.DateOfJoining,
                    DateOfResignation = d.DateOfResignation
                } ).ToList();

                if( lst == null ) {
                    return new List<PresentList>();
                } else {
                    return lst;
                }
            } catch( Exception ) {
                return new List<PresentList>();
            }
        }

        public List<PresentList> GetPresentLists( string fromdate , string todate , string stafflist )
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append( "select StaffId , FirstName , ShiftId , ShiftName , ShiftInDate , CompanyName , DepartmentName , Designation, " +
                           "GradeName , ActualInDate , ActualInTime , " +
                           "ActualOutDate , ActualOutTime , ActualWorkedHours , AttendanceStatus , DateOfJoining, DateOfResignation " +
                           "from fnGetPresentList (@fromdate,@todate,@stafflist,'PR') " );

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<PresentList>(qryStr.ToString(),param).Select(d => new PresentList()
                {
                    StaffId=d.StaffId,
                    FirstName = d.FirstName ,
                    ShiftId = d.ShiftId ,
                    ShiftName = d.ShiftName ,
                    CompanyName = d.CompanyName ,
                    DepartmentName = d.DepartmentName ,
                    Designation = d.Designation ,
                    GradeName = d.GradeName ,
                    ShiftInDate = d.ShiftInDate ,
                    ActualInDate = d.ActualInDate ,
                    ActualInTime = d.ActualInTime ,
                    ActualOutDate = d.ActualOutDate ,
                    ActualOutTime = d.ActualOutTime ,
                    ActualWorkedHours = d.ActualWorkedHours ,
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

        public List<PunchDetails> GetPunchDetails( string fromdate , string todate , string stafflist )
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            //qryStr.Append("select StaffId , FirstName , CompanyName , BranchName , DeptName , ActualInDate , " +
            //              "ActualInTime , ActualOutTime , ActualWorkedHours , OTHours from vwAttendanceDetails " +
            //              "WHERE STAFFID IN ( "+stafflist+") " +
            //              "AND ActualInDate BETWEEN '"+fromdate+"' AND '"+todate+"'");

            stafflist = stafflist.Replace("','", ",");
            stafflist = stafflist.Replace("'", "");
            qryStr.Append("select * from  dbo.fnGetPunchDetails (@stafflist,@fromdate,@todate )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<PunchDetails>(qryStr.ToString(),param).Select(d => new PunchDetails()
                {
                    StaffId = d.StaffId,
                    FirstName = d.FirstName ,
                    CompanyName = d.CompanyName ,
                    BranchName = d.BranchName ,
                    DeptName = d.DeptName ,
                    TxnDate = d.TxnDate,
                    ActualInDate = d.ActualInDate ,
                    ActualInTime = d.ActualInTime ,
                    ActualOutDate = d.ActualOutDate,
                    ActualOutTime = d.ActualOutTime ,
                    ActualWorkedHours = d.ActualWorkedHours ,
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

        public List<FirstInLastOutDiamlerNew> GetFirstInLastOuts( string fromdate , string todate , string stafflist )
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            //qryStr.Append("select Id , FirstName , CompanyName , BranchName , DeptName , DivisionName , GradeName , SwipeDate , FirstInTime , InReaderName , LastOutTime , OutReaderName , TotalHoursWorked from dbo.fnGetFirstInLastOut('" + stafflist + "','" + fromdate + "','" + todate + "')");
            qryStr.Append("select Id,STAFFID,STAFFNAME,PLANT,TEAM ,DEPARTMENT,DESIGNATION ,DIVISON ,GRADE ,SHIFT,TXNDATE  ,INTIME ,INREADER ,OUTTIME ,OUTREADER ,LATEIN ,EARLYEXIT ,TOTALHOURSWORKED,AttendanceStatus from dbo.fnGetFirstInLastOutNew(@stafflist,@fromdate,@todate)");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<FirstInLastOutDiamlerNew>(qryStr.ToString(),param).Select(d => new FirstInLastOutDiamlerNew()
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
                    TXNDATE  = d.TXNDATE,
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

        public List<RawPunchDetails> GetRawPunchDetailsReport( string fromdate , string todate , string stafflist ) {

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append( "select StaffId , FirstName , DeptName ,Designation, GradeName ,Division , Volume , SwipeDate , SwipeTime , InOut , ReaderName from fnGetRawPunchDetails(@fromdate,@todate,@stafflist)" );

            try {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<RawPunchDetails>( qryStr.ToString(),param ).Select( d => new RawPunchDetails() {
                    StaffId = d.StaffId ,
                    FirstName = d.FirstName ,
                    DeptName = d.DeptName ,
                    Designation = d.Designation ,
                    Division = d.Division ,
                    Volume = d.Volume ,
                    GradeName = d.GradeName ,
                    SwipeDate = d.SwipeDate ,
                    SwipeTime = d.SwipeTime ,
                    InOut = d.InOut , 
                    ReaderName = d.ReaderName
                } ).ToList();

                if( lst == null ) {
                    return new List<RawPunchDetails>();
                } else {
                    return lst;
                }
            } catch( Exception ) {
                return new List<RawPunchDetails>();
            }
        }

        public List<Form25FLSmidth> GetForm25FLSmidth( string fromdate , string todate , string stafflist ) {

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append( "exec spGetForm25_FLSmidth @fromdate ,@todate	,@stafflist" );

            try {


                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<Form25FLSmidth>( qryStr.ToString(),param).Select( d => new Form25FLSmidth() {
                    RecId = d.RecId,
                    Id = d.Id,
                    StaffId = d.StaffId,
                    NameOfWorker= d.NameOfWorker,
                    FatherName= d.FatherName,
                    Designation= d.Designation,
                    BirthDay= d.BirthDay,
                    BirthMonth= d.BirthMonth,
                    BirthYear= d.BirthYear,
                    PlaceOfEmployment= d.PlaceOfEmployment,
                    GroupNo = d.GroupNo,
                    RelayNo = d.RelayNo ,
                    PeriodOfEmployment = d.PeriodOfEmployment ,
                    PeriodOfWork = d.PeriodOfWork,
                    Day1 = d.Day1 ,
                    Day2 = d.Day2 ,
                    Day3 = d.Day3 ,
                    Day4 = d.Day4 ,
                    Day5 = d.Day5 ,
                    Day6 = d.Day6 ,
                    Day7 = d.Day7 ,
                    Day8 = d.Day8 ,
                    Day9 = d.Day9 ,
                    Day10 = d.Day10 ,
                    Day11 = d.Day11 ,
                    Day12 = d.Day12 ,
                    Day13 = d.Day13 ,
                    Day14 = d.Day14 ,
                    Day15 = d.Day15 ,
                    Day16 = d.Day16 ,
                    Day17 = d.Day17 ,
                    Day18 = d.Day18 ,
                    Day19 = d.Day19 ,
                    Day20 = d.Day20 ,
                    Day21 = d.Day21 ,
                    Day22 = d.Day22 ,
                    Day23 = d.Day23 ,
                    Day24 = d.Day24 ,
                    Day25 = d.Day25 ,
                    Day26 = d.Day26 ,
                    Day27 = d.Day27 ,
                    Day28 = d.Day28 ,
                    Day29 = d.Day29 ,
                    Day30 = d.Day30 ,
                    Day31 = d.Day31 
                    //ExemptingOrder = d.ExemptingOrder ,
                    //WeeklyRest = d.WeeklyRest ,
                    //CompensatoryHolidayDate = d.CompensatoryHolidayDate ,
                    //LostRestDays = d.LostRestDays ,
                    //NoOfDaysWorked = d.NoOfDaysWorked ,
                    //LeaveWithWages = d.LeaveWithWages ,
                    //LeaveWithOutWages = d.LeaveWithOutWages ,
                    //Remarks = d.Remarks
                } ).ToList();

                if( lst == null ) {
                    return new List<Form25FLSmidth>();
                } else {
                    return lst;
                }
            } catch( Exception ) {
                return new List<Form25FLSmidth>();
            }
        }

        public List<Form25> GetForm25(string fromdate, string todate, string stafflist)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append( "exec spGetForm25 @fromdate,@todate,@stafflist");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<Form25>(qryStr.ToString(),param).Select(d => new Form25()
                {
                    Id	=	d.Id,
                    staffid	=	d.staffid,
                    EmployeeDetails	=	d.EmployeeDetails,
                    RelayNo	=	d.RelayNo,
                    PeriodOfEmployment	=	d.PeriodOfEmployment,
                    PeriodOfWork	=	d.PeriodOfWork,
                    Day1	=	d.Day1,
                    Day2	=	d.Day2,
                    Day3	=	d.Day3,
                    Day4	=	d.Day4,
                    Day5	=	d.Day5,
                    Day6	=	d.Day6,
                    Day7	=	d.Day7,
                    Day8	=	d.Day8,
                    Day9	=	d.Day9,
                    Day10	=	d.Day10,
                    Day11	=	d.Day11,
                    Day12	=	d.Day12,
                    Day13	=	d.Day13,
                    Day14	=	d.Day14,
                    Day15	=	d.Day15,
                    Day16	=	d.Day16,
                    Day17	=	d.Day17,
                    Day18	=	d.Day18,
                    Day19	=	d.Day19,
                    Day20	=	d.Day20,
                    Day21	=	d.Day21,
                    Day22	=	d.Day22,
                    Day23	=	d.Day23,
                    Day24	=	d.Day24,
                    Day25	=	d.Day25,
                    Day26	=	d.Day26,
                    Day27	=	d.Day27,
                    Day28	=	d.Day28,
                    Day29	=	d.Day29,
                    Day30	=	d.Day30,
                    Day31	=	d.Day31,
                    ExemptingOrder	=	d.ExemptingOrder,
                    WeeklyRest	=	d.WeeklyRest,
                    CompensatoryHolidayDate	=	d.CompensatoryHolidayDate,
                    LostRestDays	=	d.LostRestDays,
                    NoOfDaysWorked	=	d.NoOfDaysWorked,
                    LeaveWithWages	=	d.LeaveWithWages,
                    LeaveWithOutWages	=	d.LeaveWithOutWages,
                    Remarks	=	d.Remarks,
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
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec spGetForm25_SINGLE_LINE @fromdate,@todate,@stafflist");

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
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec fnGetForm25_PAYROLL @fromdate, @todate,@stafflist");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<Form25>(qryStr.ToString(),param).Select(d => new Form25()
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
                    Day1IN = d.Day1IN,
                    Day2IN = d.Day2IN,
                    Day3IN = d.Day3IN,
                    Day4IN = d.Day4IN,
                    Day5IN = d.Day5IN,
                    Day6IN = d.Day6IN,
                    Day7IN = d.Day7IN,
                    Day8IN = d.Day8IN,
                    Day9IN = d.Day9IN,
                    Day10IN = d.Day10IN,
                    Day11IN = d.Day11IN,
                    Day12IN = d.Day12IN,
                    Day13IN = d.Day13IN,
                    Day14IN = d.Day14IN,
                    Day15IN = d.Day15IN,
                    Day16IN = d.Day16IN,
                    Day17IN = d.Day17IN,
                    Day18IN = d.Day18IN,
                    Day19IN = d.Day19IN,
                    Day20IN = d.Day20IN,
                    Day21IN = d.Day21IN,
                    Day22IN = d.Day22IN,
                    Day23IN = d.Day23IN,
                    Day24IN = d.Day24IN,
                    Day25IN = d.Day25IN,
                    Day26IN = d.Day26IN,
                    Day27IN = d.Day27IN,
                    Day28IN = d.Day28IN,
                    Day29IN = d.Day29IN,
                    Day30IN = d.Day30IN,
                    Day31IN = d.Day31IN,
                    Day1OUT = d.Day1OUT,
                    Day2OUT = d.Day2OUT,
                    Day3OUT = d.Day3OUT,
                    Day4OUT = d.Day4OUT,
                    Day5OUT = d.Day5OUT,
                    Day6OUT = d.Day6OUT,
                    Day7OUT = d.Day7OUT,
                    Day8OUT = d.Day8OUT,
                    Day9OUT = d.Day9OUT,
                    Day10OUT = d.Day10OUT,
                    Day11OUT = d.Day11OUT,
                    Day12OUT = d.Day12OUT,
                    Day13OUT = d.Day13OUT,
                    Day14OUT = d.Day14OUT,
                    Day15OUT = d.Day15OUT,
                    Day16OUT = d.Day16OUT,
                    Day17OUT = d.Day17OUT,
                    Day18OUT = d.Day18OUT,
                    Day19OUT = d.Day19OUT,
                    Day20OUT = d.Day20OUT,
                    Day21OUT = d.Day21OUT,
                    Day22OUT = d.Day22OUT,
                    Day23OUT = d.Day23OUT,
                    Day24OUT = d.Day24OUT,
                    Day25OUT = d.Day25OUT,
                    Day26OUT = d.Day26OUT,
                    Day27OUT = d.Day27OUT,
                    Day28OUT = d.Day28OUT,
                    Day29OUT = d.Day29OUT,
                    Day30OUT = d.Day30OUT,
                    Day31OUT = d.Day31OUT,
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
            qryStr.Clear();

            //qryStr.Append("select ManualPunchId, StaffId, InDate, InTime, OutDate, OutTime, PunchType,ManualPunchReason, FirstName," +
            //    "convert(varchar, ApprovalStatusId) as ApprovalStatusId,ApprovalStatusName, case when  ApprovalStatusId = 1 then '-' else ApprovalStaffId end as ApprovalStaffId," +
            //    " case when  ApprovalStatusId = 1 then '-' else ApprovalStaffName end as ApprovalStaffName, ApplicationApprovalId," +
            //    " case when  ApprovalStatusId = 1 then '-' else ApprovedOnDate end as ApprovedOnDate , case when  ApprovalStatusId = 1 then '-' else ApprovedOnTime " +
            //    "end as ApprovedOnTime, case when  ApprovalStatusId = 1 then '-' else Comment end as Comment, ApprovalOwner,ReviewerStatusName, " +
            //    "ReviewerStaffId,ReviewerStaffName, ReviewedOnDate,	ReviewedOnTime,	ReviewerOwner " +
            //    "from fnGetManualPunchApproval('" + stafflist + "','" + fromdate + "','" + todate + "')" );
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            qryStr.Append(" Exec[dbo].[GetManualPunchRequisitionHistoryReport] @stafflist,@fromdate,@todate");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ManualPunchApprovalList>(qryStr.ToString(),param).Select(d => new ManualPunchApprovalList()
                {
                    ManualPunchId = d.ManualPunchId,
                    StaffId = d.StaffId,
                    Department = d.Department,
                    Designation = d.Designation,
                    InDateTime = d.InDateTime,
                    OutDateTime = d.OutDateTime,
                    PunchType = d.PunchType,
                    ApprovedBy = d.ApprovedBy,
                    ApprovedOn = d.ApprovedOn,
                    ReviewedBy = d.ReviewedBy,
                    ReviewedOn = d.ReviewedOn,
                    ManualPunchReason = d.ManualPunchReason,
                    Name = d.Name,
                    ApprovalStatusId = d.ApprovalStatusId,
                    ApprovalStatus = d.ApprovalStatus,
                    ApprovalStaffId = d.ApprovalStaffId,
                    ApprovalStaffName = d.ApprovalStaffName,
                    ApplicationApprovalId = d.ApplicationApprovalId,
                    ApprovedOnDate = d.ApprovedOnDate,
                    ApprovedOnTime = d.ApprovedOnTime,
                    Comment = d.Comment,
                    ApprovalOwner = d.ApprovalOwner,
                    ReviewalStatus = d.ReviewalStatus,
                    ReviewalStatusId = d.ReviewalStatusId,
                    ReviewerStaffName = d.ReviewerStaffName,
                    ReviewedOnDate = d.ReviewedOnDate,
                    ReviewedOnTime = d.ReviewedOnTime,
                    ReviewerOwner = d.ReviewerOwner,

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
            catch (Exception e)
            {
                return new List<ManualPunchApprovalList>();
            }
        }

        public List<ShiftChangeApproval> GetShiftChangeApprovalLists(string fromdate, string todate, string stafflist)
        {

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select ShiftChangeId ,StaffId , FirstName , FromDate ,ToDate ,NewShiftId , NewShiftName , ShiftChangeReason , convert(varchar, ApprovalStatusId) as ApprovalStatusId ," +
                          "ApprovalStatusName , ApprovalStaffId , ApprovalStaffName , ApplicationApprovalId , ApprovedOnDate , ApprovedOnTime , Comment , ApprovalOwner " +
                          "from vwShiftChangeApproval where FromDate between @fromdate and @todate and  StaffId = @stafflist");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ShiftChangeApproval>(qryStr.ToString(),param).Select(d => new ShiftChangeApproval()
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

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            //please do not get deceived by the name of the sql function being called.
            // it is actually a common function being used by both planned and unplanned leave list.
            qryStr.Append("SELECT * FROM fnGetPlannedLeave (  @fromdate , @todate ,@stafflist )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<PlannedLeave>(qryStr.ToString(),param).Select(d => new PlannedLeave()
                {
                    StaffId = d.StaffId,
                    FirstName = d.FirstName ,
                    ShiftName = d.ShiftName,
                    CompanyName = d.CompanyName,
                    DepartmentName = d.DepartmentName,
                    GradeName = d.GradeName ,
                    LeaveTypeName = d.LeaveTypeName ,
                    LeaveStartDate = d.LeaveStartDate ,
                    LeaveStartDurationName = d.LeaveStartDurationName ,
                    LeaveEndDate = d.LeaveEndDate ,
                    LeaveEndDurationName = d.LeaveEndDurationName ,
                    ApprovalStatusName = d.ApprovalStatusName ,
                    ReviewerStatusName = d.ReviewerStatusName,
                    ISCANCELLED = d.ISCANCELLED,
                    LeaveApplicationReason = d.LeaveApplicationReason ,
                    ApprovalStaffId = d.ApprovalStaffId ,
                    ApprovalStaffName = d.ApprovalStaffName ,
                    ApprovalDate = d.ApprovalDate,
                    TxnDate = d.TxnDate
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


        public List<UnPlannedLeave> GetUnPlannedLeaveLists( string fromdate , string todate , string stafflist )
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            

            var qryStr = new StringBuilder();
            qryStr.Clear();
            stafflist = stafflist.Replace("','", ",");
            param[2] = new SqlParameter("@stafflist", stafflist);

            qryStr.Append("SELECT * FROM fnGetUnPlannedLeaveList ( @fromdate,@todate,@stafflist)");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<UnPlannedLeave>(qryStr.ToString(),param).Select(d => new UnPlannedLeave()
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
                    ApprovalStaffName = d.ApprovalStaffName,
                    TxnDate = d.TxnDate

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
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
          

            var qryStr = new StringBuilder();
            qryStr.Clear();
            stafflist = stafflist.Replace("','", ",");
            stafflist = stafflist.Replace("'", "");
            param[2] = new SqlParameter("@stafflist", stafflist);
            param[3] = new SqlParameter("@CompanyId", CompanyId);

            qryStr.Append("SELECT * FROM fnGetDepartmentSummary (@CompanyId,@stafflist,@fromdate,@todate )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<DepartmentSummary>(qryStr.ToString(),param).Select(d => new DepartmentSummary()
                {
                    DEPTID = d.DEPTID,
                    DEPTNAME= d.DEPTNAME,
                    TXNDATE= d.TXNDATE,
                    HEADCOUNT= d.HEADCOUNT,
                    TOTALPRESENT= d.TOTALPRESENT,
                    TOTALABSENT= d.TOTALABSENT,
                    PRESENTPAGE= d.PRESENTPAGE,
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

        public List<ContinuousAbsent> GetContinuousAbsentList(string fromdate, string todate, string stafflist, int DayCount)
        {

            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
            param[3] = new SqlParameter("@DayCount", DayCount);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetContinuousAbsentList ( @stafflist ,@fromdate@ ,@todate ,@DayCount )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ContinuousAbsent>(qryStr.ToString(),param).Select(d => new ContinuousAbsent()
                {
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    CompanyName = d.CompanyName,
                    DepartmentName = d.DepartmentName,
                    DivisionName = d.DivisionName,
                    NoOfDays = d.NoOfDays
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
            catch (Exception)
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
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
      
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetGraceTime ( @stafflist,@fromdate,@todate )");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<GraceTime>(qryStr.ToString(),param).Select(d => new GraceTime()
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
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
            param[3] = new SqlParameter("@DayCount", DayCount);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetContinuousLateList ( @stafflist ,@fromdate,@todate , @DayCount )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ContinuousLateComing>(qryStr.ToString(),param).Select(d => new ContinuousLateComing()
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

            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
            param[3] = new SqlParameter("@DayCount", DayCount);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetContinuousEarlyGoingList ( @stafflist,@fromdate,@todate,@DayCount )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ContinuousEarlyGoing>(qryStr.ToString(),param).Select(d => new ContinuousEarlyGoing()
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
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
      

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetMissPunchList ( @stafflist,@fromdate,@todate )");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<MissedPunchList>(qryStr.ToString(),param).Select(d => new MissedPunchList()
                {
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    CompanyName = d.CompanyName,
                    DepartmentName = d.DepartmentName,
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


            stafflist = stafflist.Replace("','",",");
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);


            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM dbo.fnGetLateComers ( @fromdate,@todate,@stafflist)");
            //"ApprovalStatusName , ApprovalStaffId , ApprovalStaffName , ApplicationApprovalId , ApprovedOnDate , ApprovedOnTime , Comment , ApprovalOwner " +
            //"from vwShiftChangeApproval where FromDate between '" + fromdate + "' and '" + todate + "' and  StaffId =" + stafflist + "");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<LateComers>(qryStr.ToString(),param).Select(d => new LateComers()
                {
                    StaffID = d.StaffID,
	                FirstName = d.FirstName,
	                CompanyName = d.CompanyName,
                    BranchName = d.BranchName,
                    DepartmentName = d.DepartmentName ,
                    Designation = d.Designation ,
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
                    ApprovalStaffName = d.ApprovalStaffName,
                    IsDisputed = d.IsDisputed

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
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
            
            qryStr.Append("SELECT * FROM fnGetOverTimeStatement (@fromdate,@todate,@stafflist)");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<OvertimeStatement>(qryStr.ToString(),param).Select(d => new OvertimeStatement()
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
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
       

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetShiftChangeList ( @stafflist,@fromdate,@todate)");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ShiftChangeStatement>(qryStr.ToString(),param).Select(d => new ShiftChangeStatement()
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
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
            
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetExtraHoursWorkedStatement  (@stafflist,@fromdate,@todate)");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ExtraHoursWorked>(qryStr.ToString(),param).Select(d => new ExtraHoursWorked()
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
            stafflist = stafflist.Replace("','", ",");
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
            

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetEarlyArrivalStatement  ( @fromdate,@todate,@stafflist)");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<EarlyArraival>(qryStr.ToString(),param).Select(d => new EarlyArraival()
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
                    ActualEarlyComingTime = d.ActualEarlyComingTime,
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
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
            
            var qrystr = new StringBuilder();
            qrystr.Clear();
            //qrystr.Append("SELECT * FROM fnGetEarlyDepartureStatement ( '" + fromdate + "' , '" + todate + "' , " + stafflist + " )");
            qrystr.Append("SELECT * FROM fnGetEarlyDepartureStatement  ( @stafflist,@fromdate,@todate)");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<EarlyDeparture>(qrystr.ToString(),param).Select(d => new EarlyDeparture()
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
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
            

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT STAFFID ,STAFFNAME ,PLANT ,DEPARTMENT ,CATEGORY,TXNDATE,SHIFTSHORTNAME     from fnNightShiftData (@stafflist,@fromdate,@todate)");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<NightShiftData>(qryStr.ToString(),param).Select(d => new NightShiftData()
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
    
        public List<ShiftViolation> GetShiftViolation(string fromdate , string todate , string stafflist)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
          
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Exec  [dbo].[GetShiftViolationReport] @stafflist,@fromdate,@todate");
           // qryStr.Append("SELECT STAFFID ,NAME ,PLANT ,TEAM,DEPARTMENT , GRADE,TXNDATE,PLANNEDSHIFT,ACTUALSHIFT from fnGetShiftViolationReport ('" + stafflist + "','" + fromdate + "','" + todate + "')");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ShiftViolation>(qryStr.ToString(),param).Select(d => new ShiftViolation()
                {
                    STAFFID = d.STAFFID,
                    NAME = d.NAME ,
                    DEPARTMENT = d.DEPARTMENT,
                    TXNDATE = d.TXNDATE,
                    PLANNEDSHIFT = d.PLANNEDSHIFT,
                    ACTUALSHIFT = d.ACTUALSHIFT,
                    ACTUALINTIME = d.ACTUALINTIME
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
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@StaffId", StaffId);
          

            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append(" SELECT STAFFID , DBO.fnGetMasterName(STAFFID , 'DP') AS DEPARTMENT , ");
            QryStr.Append(" REPLACE ( CONVERT ( VARCHAR , DateOfJoining , 106 ) , ' ' , '-' ) AS DATEOFJOINING , DBO.FNGETSTAFFNAME(StaffId) AS NAME , ");
            QryStr.Append(" ( SELECT NAME FROM StaffFamily WHERE STAFFID =  @StaffId AND RELATEDAS = 1 ) AS FATHERNAME , ");
            QryStr.Append(" ( SELECT ADDR FROM STAFFPERSONAL WHERE STAFFID = @StaffId ) AS RESADDR , ");
            QryStr.Append(" CASE WHEN YEAR ( ResignationDate ) = 2055 THEN '' ELSE REPLACE ( CONVERT ( VARCHAR , ResignationDate , 106 ) , ' ' , '-' ) END AS DATEOFLEAVING");
            QryStr.Append(" FROM STAFFOFFICIAL WHERE STAFFID = @StaffId ");

            try
            {
                context.Database.CommandTimeout = 0;
                var data = context.Database.SqlQuery<Form15StaffPersonalDetails>(QryStr.ToString(),param).Select(d => new Form15StaffPersonalDetails()
                {
                    StaffId = d.StaffId,
                    Department = d.Department,
                    DateOfJoining = d.DateOfJoining,
                    Name = d.Name,
                    FatherName = d.FatherName,
                    ResAddr = d.ResAddr,
                    DateOfLeaving = d.DateOfLeaving
                }).FirstOrDefault();

                if(data == null)
                {
                    return new Form15StaffPersonalDetails();
                }
                else
                {
                    return data;
                }
            }
            catch(Exception)
            {
                return new Form15StaffPersonalDetails();
            }
        }

        public List<Form15> GetForm15(string StaffId , string FromDate , string ToDate)
        {

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@FromDate", FromDate);
            param[1] = new SqlParameter("@ToDate", ToDate);
            param[2] = new SqlParameter("@StaffId", StaffId);

            var QryStr = new StringBuilder();

            QryStr.Clear();
            QryStr.Append("SELECT YearDateApplicationForLeave , NOOFDAYS , replace ( convert ( varchar , DATEFROM , 106 ) , ' ' , '-' ) as DATEFROM , replace ( convert ( varchar , DATETO , 106 ) , ' ' , '-' ) as DATETO , REASON  , NOOFWORKINGDAYS  , NOOFWORKEDDAYS  , NOOFDAYSLAYOFF  , NOOFDAYSLEAVEEARNED  , TOTALLEAVESATCREDIT  , NOOFDAYSWITHPAY  , NOOFDAYSWITHLOSSOFPAY  , LEAVEBALANCE  , SEC79  , LATEMINUTES  , ABSENCE FROM fnGetForm15 ( @StaffId, @FromDate,@ToDate ) ORDER BY CONVERT ( DATETIME , DATEFROM ) ASC");

            context.Database.CommandTimeout = 0;
            var lst = context.Database.SqlQuery<Form15>(QryStr.ToString(),param).Select(d => new Form15()
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
                LATEMINUTES = d.LATEMINUTES , 
                ABSENCE = d.ABSENCE
            }).ToList();

            if(lst == null)
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
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
            param[3] = new SqlParameter("@SUMrpt", SUMrpt);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec [DAILYPERFORMANCEATTENDANCEREPORT] @fromdate,@todate,@stafflist,@SUMrpt");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<DailyAttendance>(qryStr.ToString(),param).Select(d => new DailyAttendance()
                {
                    Id = d.Id,
                    Branch = d.Branch,
                    StaffId = d.StaffId,
                    NAME = d.NAME,
                    CostCenter = d.CostCenter,
                    DEPARTMENT = d.DEPARTMENT,
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

        public List<DailyExtraHoursWorkedDetails> GetExtraHoursWorkedDetails(string fromdate, string todate, string stafflist,bool flag)
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
            param[3] = new SqlParameter("@flag", flag);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec EXTRAHOUSWORKEDREPORT @fromdate,@todate,@stafflist,@flag");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<DailyExtraHoursWorkedDetails>(qryStr.ToString(),param).Select(d => new DailyExtraHoursWorkedDetails()
                {
                    ID = d.ID,
                    BRANCH = d.BRANCH,
                    staffid = d.staffid,
                    NAME = d.NAME,
                    COSTCENTER = d.COSTCENTER,
                    DEPARTMENT = d.DEPARTMENT,
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
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
            param[3] = new SqlParameter("@SUMrpt", SUMrpt);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec [DEPARTMENTWISEEREPORT] @fromdate,@todate,@stafflist,@SUMrpt , 'department' ");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<DepartmentWiseDailyAttendance>(qryStr.ToString(),param).Select(d => new DepartmentWiseDailyAttendance()
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

            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
            param[3] = new SqlParameter("@SUMrpt", SUMrpt);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec [DEPARTMENTWISEEREPORT] @fromdate,@todate,@stafflist,@SUMrpt , 'Branch' ");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<BranchWiseDailyAttendance>(qryStr.ToString(),param).Select(d => new BranchWiseDailyAttendance()
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
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);
   

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec spGetOverTime  @fromdate,@todate,@stafflist");

            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<OverTime>(qryStr.ToString(),param).Select(d => new OverTime()
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
        #region New Forms
        public List<DrawFORM1> GetFORM1(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Exec [dbo].[DrawForm1] @StaffList ");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<DrawFORM1>(qryStr.ToString(),new SqlParameter("@StaffList",stafflist)).Select(d => new DrawFORM1()
                {
                     Id =d.Id ,
                     EmpCode =d.EmpCode ,
                     Name =d.Name ,
                     Designation =d.Designation ,
                     ApprenticeAct =d.ApprenticeAct ,
                     DOJ =d.DOJ ,
                     Complete480Days =d.Complete480Days, 
                     DOJWords =d.DOJWords ,
                     Complete480DaysWords =d.Complete480DaysWords ,
                     Remarks=d.Remarks,
                     Signature =d.Signature
                }).ToList();

                if (lst == null)
                {
                    return new List<DrawFORM1>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception e)
            {
                return new List<DrawFORM1>();
            }
        }
        public List<DrawFORM10> GetFORM10(string stafflist ,string fromdate, string todate)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Exec  [DBO].[DrawForm10] @StaffList,@FromDate,@ToDate ");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<DrawFORM10>(qryStr.ToString(), new SqlParameter("@StaffList", stafflist), new SqlParameter("@Fromdate", fromdate), new SqlParameter("@ToDate", todate)).Select(d => new DrawFORM10()
                {
                    Id = d.Id,
                    PayrollId = d.StaffId,
                    EmpName = d.EmpName,
                    Department = d.Department,
                    OTDate = d.OTDate,
                    ExtentOfOT = d.ExtentOfOT,
                    PieceWorkers = d.PieceWorkers,
                    NormalHours = d.NormalHours,
                    NormalRate = d.NormalRate,
                    OverTimeRate = d.OverTimeRate,
                    NormalEarnings = d.NormalEarnings,
                    OverTimeEarning = d.OverTimeEarning,
                    CashEquivalent = d.CashEquivalent,
                    TotalEarnings = d.TotalEarnings,
                    PaymentOn = d.PaymentOn

                }).ToList();

                if (lst == null)
                {
                    return new List<DrawFORM10>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception e)
            {
                return new List<DrawFORM10>();
            }
        }
        public List<DrawFORM12> GetFORM12(string fromdate, string todate, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Exec [dbo].[DrawForm12] @StaffList ");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<DrawFORM12>(qryStr.ToString(), new SqlParameter("@StaffList", stafflist)).Select(d => new DrawFORM12()
                {
                    Id = d.Id,
                    EmpCode = d.EmpCode,
                    Name = d.Name,
                    FatherName = d.FatherName,
                    Designation = d.Designation,
                    LetterOfGroup = d.LetterOfGroup,
                    NoOfRelays = d.NoOfRelays,
                    NoOfCertificate = d.NoOfCertificate,
                    TokenNo = d.TokenNo,
                    Remarks = d.Remarks
                }).ToList();

                if (lst == null)
                {
                    return new List<DrawFORM12>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<DrawFORM12>();
            }
        }
        public List<FormVI> GetFormVI(string fromdate, string todate, string stafflist)
        {
            stafflist = stafflist.Replace("','", ",");
            stafflist = stafflist.Replace("'", "");
            var qrystr = new StringBuilder();
            qrystr.Clear();
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            qrystr.Append("exec DrawFormVI @fromdate,@todate,@stafflist");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<FormVI>(qrystr.ToString(),param).Select(d => new FormVI()
                {
                    StaffId = d.StaffId,
                    NAME = d.NAME,
                    FUNCTION = d.FUNCTION,
                    SUBFUNCTION = d.SUBFUNCTION,
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
                    Day1Name = d.Day1Name,
                    Day2Name = d.Day2Name,
                    Day3Name = d.Day3Name,
                    Day4Name = d.Day4Name,
                    Day5Name = d.Day5Name,
                    Day6Name = d.Day6Name,
                    Day7Name = d.Day7Name,
                    Day8Name = d.Day8Name,
                    Day9Name = d.Day9Name,
                    Day10Name = d.Day10Name,
                    Day11Name = d.Day11Name,
                    Day12Name = d.Day12Name,
                    Day1DATE = d.Day1DATE,
                    Day2DATE = d.Day2DATE,
                    Day3DATE = d.Day3DATE,
                    Day4DATE = d.Day4DATE,
                    Day5DATE = d.Day5DATE,
                    Day6DATE = d.Day6DATE,
                    Day7DATE = d.Day7DATE,
                    Day8DATE = d.Day8DATE,
                    Day9DATE = d.Day9DATE,
                    Day10DATE = d.Day10DATE,
                    Day11DATE = d.Day11DATE,
                    Day12DATE = d.Day12DATE

                }).ToList();

                if (lst == null)
                {
                    return new List<FormVI>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception e)
            {
                return new List<FormVI>();
            }
        }
        public List<FormQ> GetFormQ(string fromdate, string todate, string stafflist)
        {
            stafflist = stafflist.Replace("','", ",");
            stafflist = stafflist.Replace("'", "");
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@stafflist", stafflist);

            var qrystr = new StringBuilder();


            qrystr.Clear();


            qrystr.Append("Exec [DrawFormQV1] 3 , @stafflist,@fromdate,@todate");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<FormQ>(qrystr.ToString(),param).Select(d => new FormQ()
                {
                    StaffId = d.StaffId,
                    NAME = d.NAME,
                    DOJ = d.DOJ,
                    DOB = d.DOB,
                    PLANT = d.PLANT,
                    DEPARTMENT = d.DEPARTMENT,
                    DESIGNATION = d.DESIGNATION,
                    REPORTINGMANAGER = d.REPORTINGMANAGER,
                    Division = d.Division,
                    Volume = d.Volume,
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
                    TotalHours_Workded_InMonth = d.TotalHours_Workded_InMonth,
                    TotalHours_OT_InCurrentMonth = d.TotalHours_OT_InCurrentMonth,
                    Opening_CLBalance_CurrentMonth = d.Opening_CLBalance_CurrentMonth,
                    Opening_ELBalance_CurrentMonth = d.Opening_ELBalance_CurrentMonth,
                    Opening_SLBalance_CurrentMonth = d.Opening_SLBalance_CurrentMonth,
                    SumOf_CL_Availed_CurrentMonth = d.SumOf_CL_Availed_CurrentMonth,
                    SumOfSL_Availed_CurrentMonth = d.SumOfSL_Availed_CurrentMonth,
                    SumOfEL_Availed_CurrentMonth = d.SumOfEL_Availed_CurrentMonth,
                    SumOfML_Availed_CurrentMonth = d.SumOfML_Availed_CurrentMonth,
                    CL_Closing_Balance = d.CL_Closing_Balance,
                    EL_Closing_Balance = d.EL_Closing_Balance,
                    SL_Closing_Balance = d.SL_Closing_Balance

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
            catch (Exception e)
            {
                return new List<FormQ>();
            }
        }

        public List<DrawFORMS> GetFORM_S(string fromdate, string todate, string stafflist)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@stafflist", stafflist);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Exec [DBO].[DrawFormS] @stafflist");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<DrawFORMS>(qryStr.ToString(),param).Select(d => new DrawFORMS()
                {
                    SRNO = d.SRNO,
                    PayrollId = d.PayrollId,
                    EmployeeName = d.EmployeeName,
                    Sex = d.Sex,
                    FatherName = d.FatherName,
                    Designation = d.Designation,
                    DOJ = d.DOJ,
                    AdultAdolocence = d.AdultAdolocence,
                    ShiftNumber = d.ShiftNumber,
                    TimeOfCommencementOfWork = d.TimeOfCommencementOfWork,
                    TimeOfRestInterval = d.TimeOfRestInterval,
                    TimeAtWhichWorkCeases = d.TimeAtWhichWorkCeases,
                    WeeklyHoliday = d.WeeklyHoliday,
                    ClassOfWorkers = d.ClassOfWorkers,
                    MaxRatesOfWages = d.MaxRatesOfWages,
                    MinRatesOfWages = d.MinRatesOfWages
                }).ToList();

                if (lst == null)
                {
                    return new List<DrawFORMS>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception E)
            {
                return new List<DrawFORMS>();
            }
        }
        public List<LeaveSummary> LeaveSummary(string fromdate, string todate, string stafflist)
        {
            string Department = string.Empty;
            string Flag = "2";
            string MasterName = "CATEGORY";
            DateTime FromDate = Convert.ToDateTime(fromdate);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append(" Exec [leavesummary] @stafflist,@MasterName,@fromdate,2 ");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<LeaveSummary>(qryStr.ToString(),new SqlParameter("@stafflist", stafflist),new SqlParameter("@fromdate", FromDate), new SqlParameter("@MasterName", MasterName),new SqlParameter("@Flag", Flag)).Select(d => new LeaveSummary()
                {
                   StaffId   =d.StaffId,  
                   FirstName =d.FirstName,
                   PFNo      =d.PFNo,     
                   OB_CL     =d.OB_CL,    
                   OB_SL     =d.OB_SL,    
                   OB_PL     =d.OB_PL,    
                   AV_CL     =d.AV_CL,    
                   AV_SL     =d.AV_SL,    
                   AV_PL     =d.AV_PL,    
                   BAL_CL    =d.BAL_CL,   
                   BAL_SL    =d.BAL_SL,   
                   BAL_PL    =d.BAL_PL,
                   CR_CL     =d.CR_CL,
                   CR_SL     =d.CR_SL,
                   CR_PL     =d.CR_PL,
                   DR_CL     =d.DR_CL,
                   DR_SL     =d.DR_SL,
                   DR_PL     =d.DR_PL
                }).ToList();

                if (lst == null)
                {
                    return new List<LeaveSummary>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception E)
            {
                return new List<LeaveSummary>();
            }
        }
        public List<ShiftExtension> ShiftExtension(string fromdate, string todate, string stafflist)
        {
            string Department = string.Empty;
            string Flag = "2";
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append(" EXEC [DBO].[GetCandAReport] @FilterList , @FromDate , @ToDate , '' , @Flag  ");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<ShiftExtension>(qryStr.ToString(),new SqlParameter("@FilterList", stafflist),new SqlParameter("@FromDate", fromdate),new SqlParameter("@ToDate", todate),new SqlParameter("@Flag", Flag)).Select(d => new ShiftExtension()
                {
                     StaffId              =d.StaffId,
                     ShiftId              =d.ShiftId,
                     ShiftOutTime         =d.ShiftOutTime,
                     Date                 =d.ShiftOutTime.Date,
                     OutPunch             =d.OutPunch,
                     ShiftOutTime4Hours   =d.ShiftOutTime4Hours,
                     ShiftOutTime12Hours  =d.ShiftOutTime12Hours,
                     OverStayHours        =d.OverStayHours
                }).ToList();

                if (lst == null)
                {
                    return new List<ShiftExtension>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception E)
            {
                return new List<ShiftExtension>();
            }
        }
        public List<CandAReport> CandAReport(string fromdate, string todate, string stafflist)
        {
            string Department = string.Empty;
            string Flag = "2";
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append(" EXEC [DBO].[GetCandAReport] @FilterList , @FromDate , @ToDate , '' , @Flag  ");
            try
            {
                context.Database.CommandTimeout = 0;
                var lst = context.Database.SqlQuery<CandAReport>(qryStr.ToString(),new SqlParameter("@FilterList", stafflist),new SqlParameter("@FromDate", fromdate),new SqlParameter("@ToDate", todate),new SqlParameter("@Flag", Flag)).Select(d => new CandAReport()
                {
                     StaffId              =d.StaffId,
                     ShiftName            =d.ShiftName,
                     ShiftOutTime         =d.ShiftOutTime,
                   //  Date                 =d.ShiftOutTime.Date,
                     OutPunch             =d.OutPunch,
                     //ShiftOutTime4Hours   =d.ShiftOutTime4Hours,
                     //ShiftOutTime12Hours  =d.ShiftOutTime12Hours,
                     OverStayHours        =d.OverStayHours
                }).ToList();

                if (lst == null)
                {
                    return new List<CandAReport>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception E)
            {
                return new List<CandAReport>();
            }
        }
        public List<AttendanceDataView> GetOTRepository(string StaffId, string fromdate, string todate, string LogedInUser)
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@fromdate", fromdate);
            param[1] = new SqlParameter("@todate", todate);
            param[2] = new SqlParameter("@StaffId", StaffId);
            param[3] = new SqlParameter("@LogedInUser", LogedInUser);

            var qryStr = new StringBuilder();
            qryStr.Append(" Exec  [dbo].[GetExtraHoursWorkedForApproval]  @StaffId,@fromdate,@todate,@LogedInUser");
            try
            {
                context.Database.CommandTimeout = 0;
                var lstGrp = context.Database.SqlQuery<AttendanceDataView>(qryStr.ToString(),param)
                 .Select(d => new AttendanceDataView()
                 {
                   STAFFID                =d.STAFFID,                
                   FirstName              =d.FirstName,              
                   TXNDATE                =d.TXNDATE,                
                   ShiftShortName         =d.ShiftShortName,         
                   ShiftInTime            =d.ShiftInTime,            
                   ShiftOutTime           =d.ShiftOutTime,           
                   ActualInTime           =d.ActualInTime,           
                   ActualOutTime          =d.ActualOutTime,          
                   ActualExtraHoursWorked =d.ActualExtraHoursWorked
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
                throw;
            }
        }
        #endregion
        #region Business Travel
        public List<BusinessTravelReportModel> GetBusinessTravelReportRepository(string StaffId, string fromdate, string todate)
        {
           
           SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@staffList", StaffId);
            param[1] = new SqlParameter("@fromdate", fromdate);
            param[2] = new SqlParameter("@todate", todate);
            
            
          var qryStr = new StringBuilder();
          //  qryStr.Append("Exec [Dbo].[GetBusinessTravelRequestHistory] @StaffList,@FromDate,@ToDate");
            qryStr.Append("Exec [Dbo].[GetBusinessTravelRequestHistory] @staffList,@fromdate,@todate");
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
                throw;
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
            }
        }
        #endregion
        public List<CompOffRequistionModel> GetCompOffAvailingRequisitionRepository(string StaffId, string fromdate, string todate)
        {
            SqlParameter[] Param = new SqlParameter[3];
            Param[0] = new SqlParameter("@StaffList", StaffId);
            Param[1] = new SqlParameter("@FromDate", fromdate);
            Param[2] = new SqlParameter("@ToDate", todate);


            var qryStr = new StringBuilder();
            qryStr.Append("Exec [dbo].[GetComp_Off_Availing_RequisitionHistoryReport]  @StaffList,@FromDate,@ToDate");
            try
            {
                var lstGrp = context.Database.SqlQuery<CompOffRequistionModel>(qryStr.ToString(), Param).ToList();
                if (lstGrp == null)
                {
                    return new List<CompOffRequistionModel>();
                }
                else
                {
                    return lstGrp;
                }

            }
            catch(Exception err)
            {
                return new List<CompOffRequistionModel>();
            }

        }

        // Changes made by Aarthi on 18/03/2020
        public List<LeaveDeduction> GetLeaveDeductionReport(string StaffId, string FromDate, string ToDate)
        {
            string Type = "Debit";
            SqlParameter[] Param = new SqlParameter[4];
            Param[0] = new SqlParameter("@StaffId", StaffId);
            Param[1] = new SqlParameter("@FromDate", FromDate);
            Param[2] = new SqlParameter("@ToDate", ToDate);
            Param[3] = new SqlParameter("@Type", Type);

            var qryStr = new StringBuilder();
            qryStr.Append("Exec [dbo].[GetLeaveCreditDebitDetails]  @StaffId,@FromDate,@ToDate,@Type");
            try
            {
                context.Database.CommandTimeout = 0;
                var lstGrp = context.Database.SqlQuery<LeaveDeduction>(qryStr.ToString(), Param)
                 .Select(d => new LeaveDeduction()
                 {
                     StaffId = d.StaffId,
                     Name = d.Name,
                     Location = d.Location,
                     Department = d.Department,
                     LeaveType = d.LeaveType,
                     FromDate = d.FromDate,
                     ToDate = d.ToDate,
                     LeaveCount = d.LeaveCount,
                     TransactionType = d.TransactionType,
                     TransactionBy = d.TransactionBy,
                     LeaveCreditDebitReason = d.LeaveCreditDebitReason
                 }).ToList();
                if (lstGrp == null)
                {
                    return new List<LeaveDeduction>();
                }
                else
                {
                    return lstGrp;
                }

            }
            catch (Exception err)
            {
                return new List<LeaveDeduction>();
            }
        }
    }
}
