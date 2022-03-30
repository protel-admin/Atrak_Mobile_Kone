using Attendance.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class LandingPageRepository : IDisposable
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

        public LandingPageRepository()
        {
            context = new AttendanceManagementContext();
        }

        #region Coff Req Availing
        public List<COffAvailling> GetCOffAvaillingRequests(string reportingmanagerid)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@reportingmanagerid", reportingmanagerid);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("EXEC [DBO].[ApprovalListForCOFFAvailingApplication] @reportingmanagerid");
            try
            {
                var lst = context.Database.SqlQuery<COffAvailling>(qryStr.ToString(), sqlParameter).Select(d => new COffAvailling()
                {
                    COffId = d.COffId,
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    COffAvailDate = d.COffAvailDate,
                    COffReqDate = d.COffReqDate,
                    TotalDays = d.TotalDays,
                    COffReason = d.COffReason
                }).ToList();
                if (lst == null)
                {
                    return new List<COffAvailling>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception e)
            {
                return new List<COffAvailling>();
            }
        }
        #endregion

        public string GetDashBoardSettings(string settingsName, int PolicyId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@PolicyId", PolicyId);
            sqlParameter[1] = new SqlParameter("@settingsName", settingsName);

            var qry = new StringBuilder();
            try
            {
                qry.Clear();
                qry.Append("SELECT Value FROM [RULEGROUPTXN] WHERE rulegroupid = @PolicyId AND [RuleId] = (SELECT Top 1 Id FROM [Rule] WHERE [NAME] = @settingsName)");

                var value = context.Database.SqlQuery<string>(qry.ToString(), sqlParameter).FirstOrDefault();
                if (string.IsNullOrEmpty(value)==true)
                {
                    return "false";
                }
                else
                {
                    return value;
                }
                
            }
            catch(Exception)
            {
                return "false";
            }
            
        }

        // my self
        public List<HolidayGroupTxn1> GetAllHolidays(string StaffID)
        {
            SqlParameter[] sqlParameter = new SqlParameter[2];
     
            CommonRepository cm = new CommonRepository();
            var Hdayid = cm.HolidayGroupId(StaffID);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select Id from HolidayGroup where IsCurrent=1 and IsActive=1");
            var Employeegroup = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
            sqlParameter[0] = new SqlParameter("@Hdayid", Hdayid);
            sqlParameter[1] = new SqlParameter("@Employeegroup", Employeegroup);
            qryStr.Clear();
            qryStr.Append("select convert ( varchar , HolidayId ) as HolidayId , convert ( varchar , LeaveYear ) as LeaveYear , " +
                        "LeaveTypeId , HolidayName , replace ( convert ( varchar , HolidayDateFrom , 106 ) , ' ' , '-' ) as HolidayDateFrom , " +
                        "replace ( convert ( varchar , HolidayDateTo , 106 ) , ' ' , '-' ) as HolidayDateTo , " +
                        "convert ( varchar , IsChecked ) as IsChecked, HolidayDateFrom as Test  from fnGetHolidayZoneWiseHolidayList ( @Hdayid,@Employeegroup ) where IsChecked=1 order by Test asc");

            try
            {
                var lst = context.Database.SqlQuery<HolidayGroupTxn1>(qryStr.ToString(),sqlParameter).Select(d => new HolidayGroupTxn1()
                {
                    HolidayName = d.HolidayName,
                    HolidayDateFrom = d.HolidayDateFrom,
                    HolidayDateTo = d.HolidayDateTo

                }).ToList();

                if (lst.Count == 0)
                {
                    return new List<HolidayGroupTxn1>();
                }
                else
                {
                    return lst;
                }

            }
            catch (Exception)
            {
                return new List<HolidayGroupTxn1>();
            }
        }

        public int GetPolicyId(string StaffId)
        {

            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);

            try
            {
                var value = context.Database.SqlQuery<int>("SELECT POLICYID FROM STAFFOFFICIAL WHERE STAFFID = @StaffId ",sqlParameter).FirstOrDefault();
                return value;
            }
            catch (Exception)
            {
                return 0;
            }
        }



        public List<ApplicationApprovalpendingCountModel> GetApplicationApprovalpendingCounts(string ReportingManagerId)
        {
            List<ApplicationApprovalpendingCountModel> result = new List<ApplicationApprovalpendingCountModel>();
            try
            {
                string Qry = string.Empty;
                Qry = $@"select * From dbo.GetApplicationApprovalPendingCount('{ReportingManagerId}')";
                result = context.Database.SqlQuery<ApplicationApprovalpendingCountModel>(Qry).ToList();
                return result;
            }
            catch (Exception)
            {
                return result;

            }

        }



        public List<ShiftWiseHeadCount> GetShiftWiseHeadCount( string reportingmanagerid ) {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append( "select CompanyName , ShiftName , convert ( varchar , TotalHeadCount ) as TotalHeadCount , " +
                           "convert ( varchar , TotalPresent ) as TotalPresent , convert ( varchar , TotalAbsent ) as TotalAbsent , " +
                           "convert ( varchar , TotalLate ) as TotalLate , convert ( varchar , PresentPercentage ) as PresentPercentage , " +
                           "convert ( varchar , AbsentPercentage ) as AbsentPercentage from " +
                           "fnGetShiftWiseHeadCount('" + reportingmanagerid + "')" );
            try {
                var lst =
                    context.Database.SqlQuery<ShiftWiseHeadCount>( qryStr.ToString() )
                        .Select( d => new ShiftWiseHeadCount() {
                            CompanyName = d.CompanyName ,
                            ShiftName = d.ShiftName ,
                            TotalHeadCount = d.TotalHeadCount ,
                            TotalPresent = d.TotalPresent ,
                            TotalAbsent = d.TotalAbsent ,
                            TotalLate = d.TotalLate ,
                            PresentPercentage = d.PresentPercentage ,
                            AbsentPercentage = d.AbsentPercentage
                        } ).ToList();

                if( lst == null )
                {
                    return new List<ShiftWiseHeadCount>();
                }
                else
                {
                    return lst;
                }
            } 
            catch( Exception )
            {
                return new List<ShiftWiseHeadCount>();
            }
        }

        public List<ShiftWiseHeadCount> GetShiftWiseCount(string LoggedInUserId, string ShiftName, string HeadCountType,string company)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();

            if (HeadCountType == "Present")
            {
                QryStr.Append("select StaffId,Name as StaffName,Department,Designation,Category,Category,Coalesce(substring(Convert(varchar(8),InTime ,114),11,18),'-')" +
                    "as InTime,Coalesce(substring(Convert(varchar(8),OutTime ,114),11,18),'-') as OutTime, coalesce(substring(Convert(varchar(16),LateBy ,114),11,18),'-') as LateBy,WorkStationId" +
                    " from fnGetDashBoardShiftCountDetails('" + LoggedInUserId + "','" + ShiftName + "','" + HeadCountType + "','"+ company+"')");
            }
            else if (HeadCountType == "Absent")
            {
                QryStr.Append("select StaffId,Name as StaffName,Department,Designation,Category,Coalesce(substring(Convert(varchar(8),InTime ,114),11,18),'-')" +
                    "as InTime,Category,Coalesce(substring(Convert(varchar(8),OutTime ,114),11,18),'-') as OutTime, coalesce(substring(Convert(Varchar(16),LateBy,114),11,18),'-') as LateBy,WorkStationId" +
                    " from fnGetDashBoardShiftCountDetails('" + LoggedInUserId + "','" + ShiftName + "','" + HeadCountType + "','" + company + "')");
            }
            else if (HeadCountType == "Late")
            {
                QryStr.Append("select StaffId,Name as StaffName,Department,Designation,Category,coalesce(substring(Convert(varchar(8) ,InTime,114),11,18),'-')" +
                " as InTime,coalesce(substring(Convert(varchar(8),OutTime,114),11,18),'-') as OutTime,substring(Convert(varchar(16) ,LateBy,114 ),11,18) as LateBy,WorkStationId" +
               " from fnGetDashBoardShiftCountDetails('" + LoggedInUserId + "','" + ShiftName + "','" + HeadCountType + "','" + company + "')");
            }
            try
            {
                var lst = context.Database.SqlQuery<ShiftWiseHeadCount>(QryStr.ToString()).Select(d => new ShiftWiseHeadCount()
                {
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    Department = d.Department,
                    Designation = d.Designation,
                    Category = d.Category,
                    InTime = d.InTime,
                    OutTime = d.OutTime,
                    LateBy = d.LateBy,
                    WorkStationId=d.WorkStationId
                }).ToList();

                if (lst == null)
                {
                    return new List<ShiftWiseHeadCount>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ShiftWiseHeadCount>();
            }
        }

        public List<AllPendingApprovals> GetAllPendingApplications(string staffId,string applnType="")
        {
            var qryStr = new StringBuilder();

             

            qryStr.Append("EXEC [DBO].[GetPendingApprovals] '" + staffId + "','" + applnType+"'" );

            try
            {
                var lst =
                    context.Database.SqlQuery<AllPendingApprovals>(qryStr.ToString()).Select(d => new AllPendingApprovals()
                    {
                       Id = d.Id ,
                       ApplicationId=d.ApplicationId    ,
                       ParentType=d.ParentType,
                       StaffId =d.StaffId, 
                       FirstName =d.FirstName ,  
                       LeaveTypeName = d.LeaveTypeName ,
                       StartDurationName= d.StartDurationName, 
                       StartDate = d.StartDate ,
                       EndDate=d.EndDate, 
                       EndDurationName=d.EndDurationName, 
                       TotalDays= (applnType=="OD" || applnType=="BT" || applnType=="WFH")? d.OD :d.TotalDays, 
                       Reason=d.Reason,
                       FromTime=d.FromTime ,
                       TimeTo=d.TimeTo,
                       Name = d.Name,  
                       TotalHours= (applnType == "OD" || applnType == "BT" || applnType=="WFH") ? d.OD : d.TotalHours, 
                       PunchType= d.PunchType ,
                       InDate=d.InDate, 
                       InTime=d.InTime, 
                       OutDate=d.OutDate,
                       OutTime=d.OutTime, 
                       WorkedDate=d.WorkedDate, 
                       COffAvailDate=d.COffAvailDate,   
                       ApplicantName=d.ApplicantName,   
                       ODDuration= d.ODDuration, 
                       ODFromTime= d.ODFromTime ,
                       ODFromDate =d.ODFromDate,
                       ODToDate =d.ODToDate,
                       ODToTime=d.ODToTime, 
                       OD=d.OD, 
                       ContactNumber= d.ContactNumber, 
                       ApprovalStatusName = d.ApprovalStatusName, 
                       ReviewerStatusName=d.ReviewerStatusName,
                       ApprovalStaffId = d.ApprovalStaffId ,
                       ReviewerStaffId = d.ReviewerStaffId,
                       ApprovalOwnerName = d.ApprovalOwnerName,  
                       ReviewerOwnerName = d.ReviewerOwnerName ,  
                       ApprovedOnDate= d.ApprovedOnDate,  
                       ReviewedOnDate= d.ReviewedOnDate, 
                       Comment= d.Comment 

                    }).ToList();


                return lst;

            }
            catch (Exception e)
            {
                //return new List<AllApplicationHistory>();
                throw e;
            }
        }

        public List<AllApplicationHistory> GetAllApplicationHistory(string staffId, int numberOfRec,string applnType)
        {
            var qryStr = new StringBuilder();
           
            

            qryStr.Append("EXEC [DBO].[GetApplicationsHistoryForMobileApp] '" + staffId + "',"+numberOfRec + ",'"+applnType+"'");

            try
            {
                var lst = 
                    context.Database.SqlQuery<AllApplicationHistory>(qryStr.ToString()).Select(d => new AllApplicationHistory()
                    {
                        TotalDays = d.TotalDays,
                        Id = d.Id,
                        StaffId = d.StaffId,
                        StaffName=d.StaffName,
                        Type = d.Type,
                        //StartDate=d.StartDate + " " + d.StartTime,
                        //EndDate=d.EndDate + " "+ d.EndTime,
                        StartDate = d.StartDate,  
                        EndDate = d.EndDate, 
                        Remarks = d.Remarks,
                        FromDuration = d.FromDuration,
                        ToDuration = d.ToDuration,
                        StartTime = d.StartTime,
                        EndTime = d.EndTime,
                        TotalHours = d.TotalHours,
                        AppliedBy = d.AppliedBy,
                        PunchType = d.PunchType,
                        ODDuration = d.ODDuration,
                        ExpiryDate = d.ExpiryDate,
                        WorkedDate = d.WorkedDate,
                        ApprovalOwner = d.ApprovalOwner,
                        ReviewerOwner = d.ReviewerOwner,
                        ApproverStatus = d.ApproverStatus,
                        ReviewerStatus = d.ReviewerStatus,
                        IsCancelled = d.IsCancelled,
                        IsReviewerCancelled = d.IsReviewerCancelled,
                        IsApproverCancelled = d.IsApproverCancelled,
                        RequestApplicationType = d.RequestApplicationType,
                        ApplicationDate = d.ApplicationDate,

                    }).ToList();
                    
              
                    return lst;
                
            }
            catch (Exception e)
            {
                //return new List<AllApplicationHistory>();
                throw e;
            }
        }

        public List<DepartmentWiseHeadCount> GetDepartmentWiseHeadCount(string reportingmanagerid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append( "select  DepartmentName , convert ( varchar , TotalHeadCount ) as TotalHeadCount , " +
                           "convert ( varchar , TotalPresent ) as TotalPresent , convert ( varchar , TotalAbsent ) as TotalAbsent , " +
                           "convert ( varchar , TotalLate ) as TotalLate , convert ( varchar , PresentPercentage ) as PresentPercentage , " +
                           "convert ( varchar , AbsentPercentage ) as AbsentPercentage from " +
                           "fnGetDepartWiseHeadCount('" + reportingmanagerid + "')" );

            try
            {
                var lst =
                    context.Database.SqlQuery<DepartmentWiseHeadCount>(qryStr.ToString())
                        .Select(d => new DepartmentWiseHeadCount()
                        {
                            CompanyName = d.CompanyName,
                            DepartmentName = d.DepartmentName,
                            TotalHeadCount = d.TotalHeadCount,
                            TotalPresent = d.TotalPresent,
                            TotalAbsent = d.TotalAbsent,
                            TotalLate = d.TotalLate,
                            PresentPercentage = d.PresentPercentage,
                            AbsentPercentage = d.AbsentPercentage
                        }).ToList();

                if (lst == null)
                {
                    return new List<DepartmentWiseHeadCount>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<DepartmentWiseHeadCount>();
            }
        }

        public List<DepartmentWiseHeadCount> GetDepartmentWiseCount(string LoggedInUserId, string DepartmentName, string HeadCountType)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();

            if (HeadCountType == "Present")
            {
                QryStr.Append("select StaffId,Name as StaffName,Department,Designation,Category,Category,Coalesce(substring(Convert(varchar(8),InTime ,114),11,18),'-')" +
                    "as InTime,Coalesce(substring(Convert(varchar(8),OutTime ,114),11,18),'-') as OutTime, coalesce(substring(Convert(varchar(16),LateBy ,114),11,18),'-') as LateBy" +
                    " from fnGetDashBoardDepCountDetails('" + LoggedInUserId + "','" + DepartmentName + "','" + HeadCountType + "')");
            }
            else if (HeadCountType == "Absent")
            {
                QryStr.Append("select StaffId,Name as StaffName,Department,Designation,Category,Coalesce(substring(Convert(varchar(8),InTime ,114),11,18),'-')" +
                    "as InTime,Category,Coalesce(substring(Convert(varchar(8),OutTime ,114),11,18),'-') as OutTime, coalesce(substring(Convert(Varchar(16),LateBy,114),11,18),'-') as LateBy" +
                    " from fnGetDashBoardDepCountDetails('" + LoggedInUserId + "','" + DepartmentName + "','" + HeadCountType + "')");
            }
            else if (HeadCountType == "Late")
            {
                QryStr.Append("select StaffId,Name as StaffName,Department,Designation,Category,coalesce(substring(Convert(varchar(8) ,InTime,114),11,18),'-')" +
                " as InTime,coalesce(substring(Convert(varchar(8),OutTime,114),11,18),'-') as OutTime,substring(Convert(varchar(16) ,LateBy,114 ),11,18) as LateBy" +
               " from fnGetDashBoardDepCountDetails('" + LoggedInUserId + "','" + DepartmentName + "','" + HeadCountType + "')");
            }
            try
            {
                var lst = context.Database.SqlQuery<DepartmentWiseHeadCount>(QryStr.ToString()).Select(d => new DepartmentWiseHeadCount()
                {
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    Department = d.Department,
                    Designation = d.Designation,
                    Category = d.Category,
                    InTime = d.InTime,
                    OutTime = d.OutTime,
                    LateBy = d.LateBy
                   // WorkStationId=d.WorkStationId
                }).ToList();

                if (lst == null)
                {
                    return new List<DepartmentWiseHeadCount>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<DepartmentWiseHeadCount>();
            }
        }

        public List<LeaveRequestList> GetLeaveRequests( string reportingmanagerid )
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("EXEC [DBO].[ApprovalListForLeaveApplication] '" + reportingmanagerid + "'");

            try
            {
                var lst =
                    context.Database.SqlQuery<LeaveRequestList>(qryStr.ToString()).Select(d => new LeaveRequestList()
                    {
                        TotalDays=d.TotalDays,
                        LeaveApplicationId = d.LeaveApplicationId,
                        StaffId = d.StaffId,
                        LeaveTypeId = d.LeaveTypeId,
                        LeaveStartDurationId = d.LeaveStartDurationId,
                        LeaveEndDurationId = d.LeaveEndDurationId,
                        ApprovalStatusId = d.ApprovalStatusId,
                        ApprovalStaffId = d.ApprovalStaffId,
                        ApplicationApprovalId = d.ApplicationApprovalId,
                        FirstName = d.FirstName,
                        LeaveTypeName = d.LeaveTypeName,
                        LeaveStartDate = d.LeaveStartDate,
                        LeaveStartDurationName = d.LeaveStartDurationName,
                        LeaveEndDate = d.LeaveEndDate,
                        LeaveEndDurationName = d.LeaveEndDurationName,
                        LeaveApplicationReason = d.LeaveApplicationReason,
                        ContactNumber = d.ContactNumber,
                        ApprovalStatusName = d.ApprovalStatusName,
                        ApprovalStaffName = d.ApprovalStaffName,
                        ApprovedOnDate = d.ApprovedOnDate,
                        ApprovedOnTime = d.ApprovedOnTime,
                        Remarks = d.Remarks,
                        ApprovalOwnerName = d.ApprovalOwnerName,
                        ReviewerOwnerName = d.ReviewerOwnerName,
                        ReviewedOnDate = d.ReviewedOnDate,
                        ReviewerstatusName = d.ReviewerstatusName,
                        IsDocumentAvailable = d.IsDocumentAvailable,
                        ParentType = d.ParentType
                    }).ToList();

                if (lst == null)
                {
                    return new List<LeaveRequestList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LeaveRequestList>();
                throw;
            }
        }

        public List<ManualPunchRequest> GetManualPunchRequests( string reportingmanagerid )
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("EXEC [DBO].[ApprovalListForManualPunchApplication] '" + reportingmanagerid + "'");
            //qryStr.Append("select ManualPunchId , StaffId ,PunchType, convert ( varchar , ApprovalStatusId ) as ApprovalStatusId , ApprovalStaffId , ApplicationApprovalId , FirstName , InDate , InTime , OutDate , OutTime , ManualPunchReason , ApprovalStatusName , ApprovalStaffName , ApprovedOnDate , ApprovedOnTime , Comment , ApprovalOwner,ParentType from vwManualPunchApproval where ApprovalOwner = '" + reportingmanagerid + "' and ApprovalStatusId = 1" );

            try
            {
                var lst =
                    context.Database.SqlQuery<ManualPunchRequest>(qryStr.ToString())
                        .Select(d => new ManualPunchRequest()
                        {
                            ManualPunchId = d.ManualPunchId,
                            StaffId = d.StaffId,
                            PunchType = d.PunchType,
                            InDate = d.InDate,
                            InTime = d.InTime,
                            OutDate = d.OutDate, 
                            OutTime = d.OutTime,
                            ManualPunchReason = d.ManualPunchReason,
                            FirstName = d.FirstName,
                            ApprovalStatusId = d.ApprovalStaffId,
                            ApprovalStatusName = d.ApprovalStatusName,
                            ApprovalStaffId = d.ApprovalStaffId,
                            ApprovalStaffName  = d.ApprovalStaffName,
                            ApplicationApprovalId = d.ApplicationApprovalId,
                            ApprovedOnDate = d.ApprovedOnDate,
                            ApprovedOnTime = d.ApprovedOnTime,
                            Comment = d.Comment,
                            ApprovalOwner = d.ApprovalOwner,
                            ParentType = d.ParentType
                        }).ToList();

                if (lst == null)
                {
                    return new List<ManualPunchRequest>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception e)
            {
                return new List<ManualPunchRequest>();
            }
        }

        public List<ShiftChangeRequest> GetShiftChangeRequests( string reportingmanagerid )
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select ShiftChangeId , StaffId , FirstName , " +
                          "FromDate , ToDate , NewShiftId , NewShiftName , ShiftChangeReason , " +
                          "convert ( varchar , ApprovalStatusId ) as ApprovalStatusId , ApprovalStatusName , " +
                          "ApprovalStaffId , ApprovalStaffName , ApplicationApprovalId , ApprovedOnDate , " +
                          "ApprovedOnTime , Comment , ApprovalOwner from vwShiftChangeApproval " +
                          "where ApprovalStatusId = 1 AND ISCANCELLED = 'NO' and ApprovalOwner = '" + reportingmanagerid + "'" );

            try
            {
                var lst =
                    context.Database.SqlQuery<ShiftChangeRequest>(qryStr.ToString())
                        .Select(d => new ShiftChangeRequest()
                        {
                            ShiftChangeId = d.ShiftChangeId, 
                            StaffId = d.StaffId,
                            FirstName = d.FirstName,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate,
                            NewShiftId = d.NewShiftId, 
                            NewShiftName = d.NewShiftName,
                            ShiftChangeReason = d.ShiftChangeReason,
                            ApprovalStatusId = d.ApprovalStatusId,
                            ApprovalStatusName = d.ApprovalStatusName,
                            ApprovalStaffId = d.ApprovalStaffId,
                            ApprovalStaffName = d.ApprovalStaffName,
                            ApplicationApprovalId = d.ApplicationApprovalId,
                            ApprovedOnDate = d.ApprovedOnDate,
                            ApprovedOnTime = d.ApprovedOnTime, 
                            Comment = d.Comment,
                            ApprovalOwner = d.ApprovalOwner
                        }).ToList();
                if (lst == null)
                {
                    return new List<ShiftChangeRequest>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ShiftChangeRequest>();
            }
        }

        public List<PermissionRequest> GetPermissionRequests( string reportingmanagerid )
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("EXEC [DBO].[ApprovalListForPermissionApplication] '" + reportingmanagerid + "'");
            //qryStr.Append("select PermissionId , StaffId , FirstName ,PermissionDate, FromTime , TimeTo , PermissionOffReason , ContactNumber , convert ( varchar , ApprovalStatusId ) as ApprovalStatusId , ApprovalStatusName , ApprovalStaffId , ApprovalStaffName , ApplicationApprovalId , ApprovedOnDate , ApprovedOnTime , Comment , ApprovalOwner,ParentType from vwPermissionApproval where ApprovalOwner = '" + reportingmanagerid + "' and ApprovalStatusId = 1 AND ISCANCELLED = 0 ");

            try
            {
                var lst =
                    context.Database.SqlQuery<PermissionRequest>(qryStr.ToString()).Select(d => new PermissionRequest()
                    {
                        PermissionId = d.PermissionId,
                        StaffId = d.StaffId ,
                        FirstName = d.FirstName ,
                        PermissionDate = d.PermissionDate ,
                        FromTime = d.FromTime ,
                        TimeTo = d.TimeTo ,
                        PermissionOffReason = d.PermissionOffReason ,
                        ContactNumber = d.ContactNumber ,
                        ApprovalStatusId = d.ApprovalStatusId ,
                        ApprovalStatusName = d.ApprovalStatusName ,
                        ApprovalStaffId = d.ApprovalStaffId ,
                        ApprovalStaffName = d.ApprovalStaffName ,
                        ApplicationApprovalId = d.ApplicationApprovalId ,
                        ApprovedOnDate = d.ApprovedOnDate ,
                        ApprovedOnTime = d.ApprovedOnTime ,
                        Comment = d.Comment ,
                        ApprovalOwner = d.ApprovalOwner,
                        ParentType = d.ParentType
                    }).ToList();

                if (lst == null)
                {
                    return new List<PermissionRequest>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception e)
            {
                return new List<PermissionRequest>();
            }
        }

        public List<COffRequest> GetCOffRequests( string reportingmanagerid )
        
{
            var qryStr = new StringBuilder();
            qryStr.Clear();
            //qryStr.Append("select COffId , StaffId , FirstName , COffReqDate , TotalDays , COffReason , convert ( varchar , ApprovalStatusId ) as ApprovalStatusId , ApprovalStatusName , ApprovalStaffId , ApprovalStaffName , ApplicationApprovalId , ApprovedOnDate , ApprovedOnTime , Comment , ApprovalOwner,ParentType from vwCOffApproval where ApprovalOwner = '" + reportingmanagerid+"' and ApprovalStatusId = 1 AND ISCANCELLED = 'NO'");
            qryStr.Append("EXEC [DBO].[ApprovalListForCOffApplication] '" + reportingmanagerid + "'");

            try
            {
                var lst = context.Database.SqlQuery<COffRequest>(qryStr.ToString()).Select(d => new COffRequest()
                {
                    COffId = d.COffId,
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    COffReqDate = d.COffReqDate,
                    TotalDays = d.TotalDays,
                    COffReason = d.COffReason,
                    ApprovalStatusId = d.ApprovalStatusId,
                    ApprovalStatusName = d.ApprovalStatusName,
                    ReviewerstatusName = d.ReviewerstatusName,
                    ApprovalStaffId = d.ApprovalStaffId,
                    ReviewerStaffId = d.ReviewerStaffId,
                    ApprovalStaffName = d.ApprovalStaffName,
                    ReviewerName = d.ReviewerName,
                    ApplicationApprovalId = d.ApplicationApprovalId,
                    ApprovedOnDate = d.ApprovedOnDate,
                    ReviewedOnDate = d.ReviewedOnDate,
                    ApprovedOnTime =d.ApprovedOnTime,
                    Comment = d.Comment,
                    ApprovalOwner = d.ApprovalOwner,
                    ParentType = d.ParentType
                }).ToList();

                if (lst == null)
                {
                    return new List<COffRequest>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<COffRequest>();
            }
        }

        public List<LaterOffRequest> GetLaterOffRequests( string reportingmanagerid )
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select COffId , StaffId , FirstName , LaterOffReqDate , LaterOffAvailDate , " +
                          "LaterOffReason , convert ( varchar , ApprovalStatusId ) as ApprovalStatusId , " +
                          "ApprovalStatusName , ApprovalStaffId , ApprovalStaffName , ApplicationApprovalId , " +
                          "ApprovedOnDate , ApprovedOnTime , Comment , ApprovalOwner from vwLaterOffApproval " +
                          "where ApprovalOwner = '"+reportingmanagerid+"' and ApprovalStatusId = 1 AND ISCANCELLED = 'NO'");

            try
            {
                var lst =
                    context.Database.SqlQuery<LaterOffRequest>(qryStr.ToString()).Select(d => new LaterOffRequest()
                    {
                        COffId = d.COffId,
                        StaffId = d.StaffId ,
                        FirstName = d.FirstName ,
                        LaterOffReqDate = d.LaterOffReqDate ,
                        LaterOffAvailDate = d.LaterOffAvailDate ,
                        LaterOffReason = d.LaterOffReason ,
                        ApprovalStatusId = d.ApprovalStatusId ,
                        ApprovalStatusName = d.ApprovalStatusName ,
                        ApprovalStaffId = d.ApprovalStaffId ,
                        ApprovalStaffName = d.ApprovalStaffName ,
                        ApplicationApprovalId = d.ApplicationApprovalId ,
                        ApprovedOnDate = d.ApprovedOnDate ,
                        ApprovedOnTime = d.ApprovedOnTime ,
                        Comment = d.Comment ,
                        ApprovalOwner = d.ApprovalOwner 
                    }).ToList();

                if (lst == null)
                {
                    return new List<LaterOffRequest>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LaterOffRequest>();
            }
        }

        public List<MaintenanceOffRequest> GetMaintenanceOffRequests( string reportingmanagerid )
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select MaintenanceOffId , StaffId , FirstName , FromDate , ToDate , " +
                          "MaintenanceOffReason , ContactNumber , convert ( varchar , ApprovalStatusId ) as ApprovalStatusId , " +
                          "ApprovalStatusName , ApprovalStaffId , ApprovalStaffName , ApplicationApprovalId , ApprovedOnDate , " +
                          "ApprovedOnTime , Comment , ApprovalOwner from vwMaintenanceOffApproval where ApprovalStatusId = 1 AND ISCANCELLED = 'NO' " +
                          "and ApprovalOwner = '" +reportingmanagerid + "' ");

            try
            {
                var lst =
                    context.Database.SqlQuery<MaintenanceOffRequest>(qryStr.ToString())
                        .Select(d => new MaintenanceOffRequest()
                        {
                            MaintenanceOffId = d.MaintenanceOffId,
                            StaffId = d.StaffId ,
                            FirstName = d.FirstName ,
                            FromDate = d.FromDate ,
                            ToDate = d.ToDate ,
                            MaintenanceOffReason = d.MaintenanceOffReason ,
                            ContactNumber = d.ContactNumber ,
                            ApprovalStatusId = d.ApprovalStatusId ,
                            ApprovalStatusName = d.ApprovalStatusName ,
                            ApprovalStaffId = d.ApprovalStaffId ,
                            ApprovalStaffName = d.ApprovalStaffName ,
                            ApplicationApprovalId = d.ApplicationApprovalId ,
                            ApprovedOnDate = d.ApprovedOnDate ,
                            ApprovedOnTime = d.ApprovedOnTime ,
                            Comment = d.Comment ,
                            ApprovalOwner = d.ApprovalOwner
                        }).ToList();

                if (lst == null)
                {
                    return new List<MaintenanceOffRequest>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<MaintenanceOffRequest>();
            }        
        }

        public List<OTRequest> GetOTRequests(string reportingmanagerid)
        {
            var qrySty = new StringBuilder();
            qrySty.Clear();
            qrySty.Append("EXEC [DBO].[ApprovalListForOTApplication] '" + reportingmanagerid + "'");

            try
            {
                var lst = context.Database.SqlQuery<OTRequest>(qrySty.ToString()).Select(d => new OTRequest()
                {
                    OTApplicationId = d.OTApplicationId,
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    OTDate = d.OTDate,
                    OTTime = d.OTTime,
                    OTDuration = d.OTDuration,
                    OTReason = d.OTReason,
                    ApprovalStatusId = d.ApprovalStatusId,
                    ApprovalStatusName = d.ApprovalStatusName,
                    ApprovalStaffId = d.ApprovalStaffId,
                    ApprovalStaffName = d.ApprovalStaffName,
                    ApplicationApprovalId = d.ApplicationApprovalId,
                    ApprovedOnDate = d.ApprovedOnDate,
                    ApprovedOnTime = d.ApprovedOnTime,
                    Comment = d.Comment,
                    ApprovalOwner = d.ApprovalOwner
                }).ToList();

                if (lst == null)
                {
                    return new List<OTRequest>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<OTRequest>();
            }
        }

        public void ApproveApplication(string ApprovalId, int ApprovalStatusId, string ApproverId)
        {
            var repo1 = new CommonRepository();
            bool AFlag = false;
            if(ApprovalStatusId == 2)
            {
                AFlag = true;
            }
            else
            {
                AFlag = false;
            }
            repo1.ApplicationApprovalRejection(ApproverId, ApprovalId, AFlag);
        }

        public void SendEmailToRequester(string ApprovalId)
        {
            StringBuilder qryStr = new StringBuilder();
            qryStr.Append("select ParentId, ParentType from ApplicationApproval where [Id] = '" + ApprovalId + "'");
            //get the application id and the application type based on the approval id.
            //check the type of application.
            //if the type of application is LeaveApplication then...
            //get the leave application details based on the leave application id.
        }

        public List<ODRequest> GetOD_OR_BTRequest(string ReportingManagerId,string ApplicationType)
        {
            SqlParameter[] Param = new SqlParameter[2];
            Param[0] = new SqlParameter("@StaffId", ReportingManagerId);
            Param[1] = new SqlParameter("@ApplicationType", ApplicationType);

            var QryStr = new StringBuilder();
            QryStr.Append("EXEC [DBO].[ApprovalListForOnDutyApplication] @StaffId,@ApplicationType");

            try
            {
                var lst = context.Database.SqlQuery<ODRequest>(QryStr.ToString(), Param).Select(d => new ODRequest() { 
                ODApplicationId = d.ODApplicationId,
                StaffId = d.StaffId,
                ApplicantName = d.ApplicantName,
                ODDuration = d.ODDuration,
                ODFromDate = d.ODFromDate,
                ODFromTime = d.ODFromTime,
                ODToDate = d.ODToDate,
                ODToTime = d.ODToTime,
                OD = d.OD,
                ODReason = d.ODReason,
                ApprovalStatusId = d.ApprovalStatusId,
                ApprovalStatusName = d.ApprovalStatusName,
                ApprovalStaffId = d.ApprovalStaffId,
                ApprovalStaffName = d.ApprovalStaffName,
                ApplicationApprovalId = d.ApplicationApprovalId,
                ApprovedOnDate = d.ApprovedOnDate,
                ApprovedOnTime = d.ApprovedOnTime,
                Comment = d.Comment,
                ApprovalOwner = d.ApprovalOwner,
                ParentType = d.ParentType
                }).ToList();

                if(lst == null)
                {
                    return new List<ODRequest>();
                }
                else
                {
                    return lst;
                }
            }
            catch(Exception err)
            {
                return new List<ODRequest>();
            }
        }

        public List<CompleteHeadCount> ShowCompleteHeadCount(string id, string TxnDate)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec [DBO].[GetHeadCounts] 5,NULL,NULL,NULL,NULL,'" + id + "','" + TxnDate + "' ");
            try
            {
                var lst = context.Database.SqlQuery<CompleteHeadCount>(qryStr.ToString()).Select(d => new CompleteHeadCount
                {
                    DepartmentName = d.DepartmentName,
                    DesignationName = d.DesignationName,
                    GradeName = d.GradeName,
                    ShiftName = d.ShiftName,
                    HeadCount = d.HeadCount,
                    PresentCount = d.PresentCount,
                    AbsentCount = d.AbsentCount
                }).ToList();

                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public FirstInLastOutDiamlerNew GetMyPunch(string LoggedInUserId, string SelectedDate)
        {

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT * FROM fnGetFirstInLastOutNew('" + LoggedInUserId + "' ,'" + SelectedDate + "','" + SelectedDate + "') ");

            try
            {
                var lst = context.Database.SqlQuery<FirstInLastOutDiamlerNew>(qryStr.ToString()).Select(d => new FirstInLastOutDiamlerNew()
                {
                    SHIFT = d.SHIFT,
                    TXNDATE = d.TXNDATE,
                    INTIME = d.INTIME,
                    OUTTIME = d.OUTTIME,
                    LATEIN = d.LATEIN,
                    EARLYEXIT = d.EARLYEXIT,
                    TOTALHOURSWORKED = d.TOTALHOURSWORKED
                }).FirstOrDefault();
                if (lst == null)
                    return new FirstInLastOutDiamlerNew();
                else
                    return lst;
            }
            catch (Exception err)
            {
                return new FirstInLastOutDiamlerNew();
            }
        }

        public DocumentData GetDocumentData(string Id)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT ParentId as LeaveApplicationId, FileContent,TypeOfDocument FROM DocumentUpload where ParentId='" + Id + "'");

            try
            {
                var lst = context.Database.SqlQuery<DocumentData>(qryStr.ToString()).Select(d => new DocumentData()
                {
                    LeaveApplicationId = d.LeaveApplicationId,
                    FileContent = d.FileContent,
                    TypeOfDocument = d.TypeOfDocument
                }).FirstOrDefault();
                if (lst == null)
                    return new DocumentData();
                else
                    return lst;
            }
            catch (Exception err)
            {
                return new DocumentData();
            }

        }

        public List<MonthlyLeavePlanner> GetMonthlyLeavePlanner(string StartDate, string EndDate, string Reporting)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("EXEC [DBO].[MonthlyLeavePlanner] @StartDate,@EndDate,@RepMgr");
            if (Reporting == null)
                Reporting = string.Empty;
            try
            {
                var lst = context.Database.SqlQuery<MonthlyLeavePlanner>(qryStr.ToString(), new SqlParameter("@StartDate", StartDate), new SqlParameter("@EndDate", EndDate), new SqlParameter("@RepMgr", Reporting)).Select(d => new MonthlyLeavePlanner()
                {
                    //Id= d.Id,
                    DeptId = d.DeptId,
                    GradeId = d.GradeId,
                    EmpCode = d.EmpCode,
                    DepartmentName = d.DepartmentName,
                    GradeName = d.GradeName,
                    EmpName = d.EmpName,
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
                    DepSeq = d.DepSeq,
                    GradeSeq = d.GradeSeq,
                    Seq = d.Seq
                }).ToList();
                if (lst == null)
                    return null;
                else
                    return lst;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //public List<Remainder> RemainderDetails()
        //{
        //    var qryStr = new StringBuilder();
        //    qryStr.Clear();
        //    qryStr.Append("select (select value from settings where id=15) as LastAttendanceprocessed, " +
        //        "(Select convert(varchar,max(Tr_Created)) from smaxtransaction) as LasttransactionSyncDate, (select count(*) from RequestApplication where " +
        //        "Requestapplicationtype not in ('MP','RH') and convert(date,getdate()) between convert(Date,StartDate) and convert(Date,EndDate) and isapproved=1)  as PlannedLeaveCount," +
        //        "(select count(t.TransactionDate) as HeadCount from ( select max(Tr_Date) as TransactionDate,Tr_chid as EmpId,Tr_TType" +
        //          " from SMaxTransaction A where convert(date,Tr_Date) = convert(date,getdate()) and tr_ttype=20" +
        //           "group by tr_chid,Tr_ttype) as t) as HeadCount,(select count(*) from staffpersonal where convert(varchar(5),DateOfBirth, 110) = convert(varchar(5), getdate(), 110)) as BirthdayAlert");

        //    try
        //    {
        //        var lst =
        //            context.Database.SqlQuery<Remainder>(qryStr.ToString())
        //                .Select(d => new Remainder()
        //                {
        //                    Lastattendanceprocessed = d.Lastattendanceprocessed,
        //                    ShiftRoisteringAlertCount = d.ShiftRoisteringAlertCount,
        //                    LasttransactionSyncDate = d.LasttransactionSyncDate,
        //                    HeadCount = d.HeadCount,
        //                    BirthdayAlert = d.BirthdayAlert,
        //                    PlannedLeaveCount = d.PlannedLeaveCount
        //                }).ToList();
        //        if (lst == null)
        //        {
        //            return new List<Remainder>();
        //        }
        //        else
        //        {
        //            return lst;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return new List<Remainder>();
        //    }
        //}

    }
}
