using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
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
        StringBuilder builder = new StringBuilder();
        public string GetDashBoardSettings(string settingsName, int PolicyId)
        {
            try
            {
                var qryStr = new StringBuilder();
                qryStr.Clear();
                qryStr.Append("SELECT Value FROM [RULEGROUPTXN] WHERE rulegroupid = @PolicyId AND [RuleId] = (SELECT Top 1 Id " +
                    "FROM [Rule] WHERE [NAME] = @settingsName)");
                var value = context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@PolicyId", PolicyId)
                    , new SqlParameter("@settingsName", settingsName)).FirstOrDefault();
                if (string.IsNullOrEmpty(value) == true)
                {
                    return "false";
                }
                else
                {
                    return value;
                }
            }
            catch (Exception)
            {
                return "false";
            }
        }

        public void ApproveApplication(string ApprovalId, int ApprovalStatusId, string ApproverId)
        {
            var repo1 = new CommonRepository();
            bool AFlag = false;
            if (ApprovalStatusId == 2)
            {
                AFlag = true;
            }
            else
            {
                AFlag = false;
            }
            repo1.ApplicationApprovalRejection(ApproverId, ApprovalId, AFlag);
        }



        public List<HolidayGroupTxn1> GetAllHolidays(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("Select H.Name as HolidayName,Replace(Convert(Varchar,HolidayDateFrom, 106),' ','-' )as HolidayDateFrom," +
                "DATENAME(DW, HolidayDateFrom) as DayName , Replace(Convert(Varchar, HolidayDateTo, 106), ' ', '-') as HolidayDateTo" +
                " from HolidayGroupTxn G inner join Holiday H on H.id = G.HolidayId " +
                "Where Datepart(Year, G.HolidayDateFrom) = DATEPART(Year, GetDate())" +
                " And H.Id in (Select Distinct HolidayId From HolidayZoneTxn Where HolidayZoneId in " +
                "(Select HolidayGroupId From StaffOfficial Where StaffId = @staffid)) order by G.HolidayDateFrom Asc");

            try
            {
                var lst = context.Database.SqlQuery<HolidayGroupTxn1>(qryStr.ToString(), new SqlParameter("@staffid", staffid)).Select(d => new HolidayGroupTxn1()
                {
                    HolidayName = d.HolidayName,
                    HolidayDateFrom = d.HolidayDateFrom,
                    HolidayDateTo = d.HolidayDateTo,
                    DayName = d.DayName

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

        public List<GetPlannedLeave> GetPlannedLeave()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select StaffId , dbo.fngetstaffName(staffid) as StaffName ,dbo.fngetMasterName ( staffid , 'DP' ) " +
                "as Department, RequestApplicationType as LeaveTypes , convert (varchar, Startdate) as StartDate ," +
                " convert (Varchar, EndDate) as EndDate, TotalDays, Remarks as Reason from RequestApplication	where " +
                "convert(date,getdate()) between convert(date,startdate) and " +
                "convert(date,enddate) and RequestApplicationType not in ('MP','RH') and isapproved=1");

            try
            {
                var lst =
                    context.Database.SqlQuery<GetPlannedLeave>(qryStr.ToString())
                        .Select(d => new GetPlannedLeave()
                        {

                            StaffId = d.StaffId,
                            StaffName = d.StaffName,
                            Department = d.Department,
                            LeaveTypes = d.LeaveTypes,
                            StartDate = d.StartDate,
                            EndDate = d.EndDate,
                            TotalDays = d.TotalDays,
                            Reason = d.Reason
                        }).ToList();
                if (lst == null)
                {
                    return new List<GetPlannedLeave>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<GetPlannedLeave>();
            }
        }

        public List<GetHeadCountAlertDetails> GetHeadCountAlertDetails()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select convert(varchar,max(Tr_Date)) as TransactionDate,Tr_chid as StaffId,Tr_TType, " +
                "B.FirstName,B.DeptName as DepartmentName,B.DesignationName,B.CategoryName,B.BranchName" +
                " from SMaxTransaction A inner join staffview B on A.Tr_ChId=B.StaffId" +
                " where convert(date,Tr_Date) = convert(date,getdate()) and tr_ttype=20 " +
                "group by tr_chid,Tr_ttype,B.firstname,B.DeptName,B.DesignationName,B.CategoryName,B.BranchName");

            try
            {
                var lst =
                    context.Database.SqlQuery<GetHeadCountAlertDetails>(qryStr.ToString())
                        .Select(d => new GetHeadCountAlertDetails()
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

        public List<GetBirthdayAlert> GetBirthdayAlertList(string StaffId)
        {
            List<GetBirthdayAlert> lst = new List<GetBirthdayAlert>();
            try
            {
                builder = new StringBuilder();
                builder.Append("select B.OfficalEmail,convert(varchar,A.DateOfBirth, 106) as DateOfBirth , A.StaffId, " +
                    "B.FirstName,B.DeptName as DepartmentName,B.DesignationName,B.CategoryName,B.BranchName" +
                    " from StaffPersonal A inner join staffview B on A.StaffId=B.StaffId where convert(varchar(5),A.DateOfBirth, 110) = convert(varchar(5), getdate(), 110) and A.StaffId not in(@StaffId)");
                lst = context.Database.SqlQuery<GetBirthdayAlert>(builder.ToString(), new SqlParameter("@StaffId", StaffId)).ToList();
            }
            catch (Exception e)
            {
                throw e; ;
            }
            return lst;
        }

        public int GetPolicyId(string StaffId)
        {
            try
            {
                builder = new StringBuilder();
                builder.Append("SELECT POLICYID FROM STAFFOFFICIAL WHERE STAFFID = @StaffId");
                var value = context.Database.SqlQuery<int>(builder.ToString(), new SqlParameter("@StaffId", StaffId)).FirstOrDefault();
                return value;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<ShiftWiseHeadCount> GetShiftWiseHeadCount(string reportingmanagerid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select CompanyName , ShiftName , convert ( varchar , TotalHeadCount ) as TotalHeadCount , " +
                           "convert ( varchar , TotalPresent ) as TotalPresent , convert ( varchar , TotalAbsent ) as TotalAbsent , " +
                           "convert ( varchar , TotalLate ) as TotalLate , convert ( varchar , PresentPercentage ) as PresentPercentage , " +
                           "convert ( varchar , AbsentPercentage ) as AbsentPercentage from " +
                           "fnGetShiftWiseHeadCount(@reportingmanagerid)");
            try
            {
                var lst =
                    context.Database.SqlQuery<ShiftWiseHeadCount>(qryStr.ToString(), new SqlParameter("@reportingmanagerid", reportingmanagerid))
                        .Select(d => new ShiftWiseHeadCount()
                        {
                            CompanyName = d.CompanyName,
                            ShiftName = d.ShiftName,
                            TotalHeadCount = d.TotalHeadCount,
                            TotalPresent = d.TotalPresent,
                            TotalAbsent = d.TotalAbsent,
                            TotalLate = d.TotalLate,
                            PresentPercentage = d.PresentPercentage,
                            AbsentPercentage = d.AbsentPercentage
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

        public List<ShiftWiseHeadCount> GetShiftWiseCount(string LoggedInUserId, string ShiftName, string HeadCountType, string company)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();

            if (HeadCountType == "Present")
            {
                QryStr.Append("select StaffId,Name as StaffName,Department,Designation,Category,Category,Coalesce(substring(Convert(varchar(8),InTime ,114),11,18),'-')" +
                    "as InTime,Coalesce(substring(Convert(varchar(8),OutTime ,114),11,18),'-') as OutTime, " +
                    "coalesce(substring(Convert(varchar(16),LateBy ,114),11,18),'-') as LateBy,WorkStationId" +
                    " from fnGetDashBoardShiftCountDetails(@LoggedInUserId,@ShiftName,@HeadCountType,@company)");
            }
            else if (HeadCountType == "Absent")
            {
                QryStr.Append("select StaffId,Name as StaffName,Department,Designation,Category,Coalesce(substring(Convert(varchar(8),InTime ,114),11,18),'-')" +
                    "as InTime,Category,Coalesce(substring(Convert(varchar(8),OutTime ,114),11,18),'-') as OutTime, coalesce(substring(Convert(Varchar(16),LateBy,114),11,18),'-') as LateBy,WorkStationId" +
                    " from fnGetDashBoardShiftCountDetails(@LoggedInUserId,@ShiftName,@HeadCountType,@company)");
            }
            else if (HeadCountType == "Late")
            {
                QryStr.Append("select StaffId,Name as StaffName,Department,Designation,Category,coalesce(substring(Convert(varchar(8) ,InTime,114),11,18),'-')" +
                " as InTime,coalesce(substring(Convert(varchar(8),OutTime,114),11,18),'-') as OutTime,substring(Convert(varchar(16) ,LateBy,114 ),11,18) as LateBy,WorkStationId" +
               " from fnGetDashBoardShiftCountDetails(@LoggedInUserId,@ShiftName,@HeadCountType,@company)");
            }
            try
            {
                var lst = context.Database.SqlQuery<ShiftWiseHeadCount>(QryStr.ToString(), new SqlParameter("@LoggedInUserId", LoggedInUserId)
                    , new SqlParameter("@ShiftName", ShiftName), new SqlParameter("@HeadCountType", HeadCountType)
                    , new SqlParameter("@company", company)).Select(d => new ShiftWiseHeadCount()
                    {
                        StaffId = d.StaffId,
                        StaffName = d.StaffName,
                        Department = d.Department,
                        Designation = d.Designation,
                        Category = d.Category,
                        InTime = d.InTime,
                        OutTime = d.OutTime,
                        LateBy = d.LateBy,
                        WorkStationId = d.WorkStationId
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

        public List<RALeaveDonation> GetLeaveDonationRequest(string reportingmanagerid, string ApplicationType)
        {
            List<RALeaveDonation> lst = new List<RALeaveDonation>();
            var qryStr = new StringBuilder();
            qryStr.Clear();
            try
            {
                //qryStr.Append("Select RA.TotalDays as LeaveCount,AA.ParentId as LeaveDonationApplicationId,RA.StaffId as DonarStaffId, RA.Remarks as Narration," +
                //  " RA.AppliedBy as ApprovalOwner, RA.ReceiverStaffId From RequestApplication RA inner join ApplicationApproval AA on" +
                //  " AA.ParentId = RA.ID Where ApprovalOwner = @reportingmanagerid and ApprovalStatusId = 1 and RA.RequestApplicationType = '" + ApplicationType + "'");
                //lst = context.Database.SqlQuery<RALeaveDonation>(qryStr.ToString(), new SqlParameter("@StaffId", reportingmanagerid)).ToList();

                qryStr.Append("Select RA.TotalDays as LeaveCount,AA.ParentId as LeaveDonationApplicationId,RA.StaffId as DonarStaffId, RA.Remarks as Narration," +
                   " RA.AppliedBy as ApprovalOwner, RA.ReceiverStaffId From RequestApplication RA inner join ApplicationApproval AA on" +
                   " AA.ParentId = RA.ID Where ApprovalOwner = @ReportingManagerId and ApprovalStatusId = 1 and RA.RequestApplicationType = '" + ApplicationType + "'");
                lst = context.Database.SqlQuery<RALeaveDonation>(qryStr.ToString(), new SqlParameter("@ReportingManagerId", reportingmanagerid)).ToList();

                if (lst != null)
                {
                    return lst;
                }
                else
                {
                    return new List<RALeaveDonation>();
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }


        public List<AllApplicationHistory> GetAllApplicationHistory(string staffId, int numberOfRec, string applnType)
        {
            var qryStr = new StringBuilder();



            qryStr.Append("EXEC [DBO].[GetApplicationsHistoryForMobileApp] '" + staffId + "'," + numberOfRec + ",'" + applnType + "'");

            try
            {
                var lst =
                    context.Database.SqlQuery<AllApplicationHistory>(qryStr.ToString()).Select(d => new AllApplicationHistory()
                    {
                        TotalDays = d.TotalDays,
                        Id = d.Id,
                        StaffId = d.StaffId,
                        StaffName = d.StaffName,
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


        public List<AllPendingApprovals> GetAllPendingApplications(string staffId, string applnType = "")
        {
            var qryStr = new StringBuilder();
            qryStr.Append("EXEC [DBO].[GetPendingApprovals] '" + staffId + "','" + applnType + "'");
            try
            {
                var lst =
                    context.Database.SqlQuery<AllPendingApprovals>(qryStr.ToString()).Select(d => new AllPendingApprovals()
                    {
                        Id = d.Id,
                        ApplicationId = d.ApplicationId,
                        ParentType = d.ParentType,
                        StaffId = d.StaffId,
                        FirstName = d.FirstName,
                        LeaveTypeName = d.LeaveTypeName,
                        StartDurationName = d.StartDurationName,
                        StartDate = d.StartDate,
                        EndDate = d.EndDate,
                        EndDurationName = d.EndDurationName,
                        TotalDays = (applnType == "OD" || applnType == "BT" || applnType == "WFH") ? d.OD : d.TotalDays,
                        Reason = d.Reason,
                        FromTime = d.FromTime,
                        TimeTo = d.TimeTo,
                        Name = d.Name,
                        TotalHours = (applnType == "OD" || applnType == "BT" || applnType == "WFH") ? d.OD : d.TotalHours,
                        PunchType = d.PunchType,
                        InDate = d.InDate,
                        InTime = d.InTime,
                        OutDate = d.OutDate,
                        OutTime = d.OutTime,
                        WorkedDate = d.WorkedDate,
                        COffAvailDate = d.COffAvailDate,
                        ApplicantName = d.ApplicantName,
                        ODDuration = d.ODDuration,
                        ODFromTime = d.ODFromTime,
                        ODFromDate = d.ODFromDate,
                        ODToDate = d.ODToDate,
                        ODToTime = d.ODToTime,
                        OD = d.OD,
                        ContactNumber = d.ContactNumber,
                        ApprovalStatusName = d.ApprovalStatusName,
                        ReviewerStatusName = d.ReviewerStatusName,
                        ApprovalStaffId = d.ApprovalStaffId,
                        ReviewerStaffId = d.ReviewerStaffId,
                        ApprovalOwnerName = d.ApprovalOwnerName,
                        ReviewerOwnerName = d.ReviewerOwnerName,
                        ApprovedOnDate = d.ApprovedOnDate,
                        ReviewedOnDate = d.ReviewedOnDate,
                        Comment = d.Comment

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
            qryStr.Append("select  DepartmentName , convert ( varchar , TotalHeadCount ) as TotalHeadCount , " +
                           "convert ( varchar , TotalPresent ) as TotalPresent , convert ( varchar , TotalAbsent ) as TotalAbsent , " +
                           "convert ( varchar , TotalLate ) as TotalLate , convert ( varchar , PresentPercentage ) as PresentPercentage , " +
                           "convert ( varchar , AbsentPercentage ) as AbsentPercentage from " +
                           "fnGetDepartWiseHeadCount(@reportingmanagerid)");

            try
            {
                var lst =
                    context.Database.SqlQuery<DepartmentWiseHeadCount>(qryStr.ToString(), new SqlParameter("@StaffId", reportingmanagerid))
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
                    " from fnGetDashBoardDepCountDetails(@LoggedInUserId,@DepartmentName,@HeadCountType)");
            }
            else if (HeadCountType == "Absent")
            {
                QryStr.Append("select StaffId,Name as StaffName,Department,Designation,Category,Coalesce(substring(Convert(varchar(8),InTime ,114),11,18),'-')" +
                    "as InTime,Category,Coalesce(substring(Convert(varchar(8),OutTime ,114),11,18),'-') as OutTime, coalesce(substring(Convert(Varchar(16),LateBy,114),11,18),'-') as LateBy" +
                    " from fnGetDashBoardDepCountDetails(@LoggedInUserId,@DepartmentName,@HeadCountType)");
            }
            else if (HeadCountType == "Late")
            {
                QryStr.Append("select StaffId,Name as StaffName,Department,Designation,Category,coalesce(substring(Convert(varchar(8) ,InTime,114),11,18),'-')" +
                " as InTime,coalesce(substring(Convert(varchar(8),OutTime,114),11,18),'-') as OutTime,substring(Convert(varchar(16) ,LateBy,114 ),11,18) as LateBy" +
               " from fnGetDashBoardDepCountDetails(@LoggedInUserId,@DepartmentName,@HeadCountType)");
            }
            try
            {
                var lst = context.Database.SqlQuery<DepartmentWiseHeadCount>(QryStr.ToString(), new SqlParameter("@LoggedInUserId", LoggedInUserId)
                    , new SqlParameter("@DepartmentName", DepartmentName), new SqlParameter("@HeadCountType", HeadCountType)).Select(d => new DepartmentWiseHeadCount()
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

        public List<LeaveRequestList> GetLeaveRequests(string reportingmanagerid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("EXEC [DBO].[ApprovalListForLeaveApplication] @reportingmanagerid");
            try
            {
                var lst =
                    context.Database.SqlQuery<LeaveRequestList>(qryStr.ToString(), new SqlParameter("@reportingmanagerid", reportingmanagerid)).Select(d => new LeaveRequestList()
                    {
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
                        LeaveApplicationReason = d.Remarks,
                        ContactNumber = d.ContactNumber,
                        ApprovalStatusName = d.ApprovalStatusName,
                        ApprovalStaffName = d.ApprovalStaffName,
                        ApprovedOnDate = d.ApprovedOnDate,
                        ApprovedOnTime = d.ApprovedOnTime,
                        //Comment = d.Remarks,
                        ApprovalOwnerName = d.ApprovalOwnerName,
                        ReviewerOwnerName = d.ReviewerOwnerName,
                        ReviewedOnDate = d.ReviewedOnDate,
                        ReviewerstatusName = d.ReviewerstatusName,
                        IsDocumentAvailable = d.IsDocumentAvailable,
                        ParentType = d.ParentType,
                        TotalDays = d.TotalDays
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

        public List<ManualPunchRequest> GetManualPunchRequests(string reportingmanagerid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("EXEC [DBO].[ApprovalListForManualPunchApplication] @reportingmanagerid");
            try
            {
                var lst =
                    context.Database.SqlQuery<ManualPunchRequest>(qryStr.ToString(), new SqlParameter("@reportingmanagerid", reportingmanagerid))
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
                            ApprovalStaffName = d.ApprovalStaffName,
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
            catch 
            {
                return new List<ManualPunchRequest>();
            }
        }

        public List<ShiftChangeRequest> GetShiftChangeRequests(string reportingmanagerid)
        {
            try
            {
                var qryStr = new StringBuilder();
                qryStr.Append("select ApplicationId as ShiftChangeId , StaffId , StaffName as FirstName , StartDate as FromDate , EndDate as ToDate , NewShiftName ," +
                    "Remarks as ShiftChangeReason from ShiftChangeApproval Where  (Approval1StatusId = 1 OR Approval2statusId = 1 OR " +
                    "Approval2statusId = 0) AND ISCANCELLED = 'NO' and(Approval1Owner = @reportingmanagerid OR " +
                    "Approval2Owner = @reportingmanagerid)");

                var lst = context.Database.SqlQuery<ShiftChangeRequest>(qryStr.ToString(), new SqlParameter("@reportingmanagerid", reportingmanagerid))
                        .Select(d => new ShiftChangeRequest()
                        {
                            Approval2Owner = d.Approval2Owner,
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
                            Comment = d.Comment
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
            catch 
            {
                return new List<ShiftChangeRequest>();
            }
        }

        public List<PermissionRequest> GetPermissionRequests(string reportingmanagerid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("EXEC [DBO].[ApprovalListForPermissionApplication] @reportingmanagerid");

            try
            {
                var lst =
                    context.Database.SqlQuery<PermissionRequest>(qryStr.ToString(), new SqlParameter("@reportingmanagerid", reportingmanagerid)).Select(d => new PermissionRequest()
                    {
                        PermissionId = d.PermissionId,
                        StaffId = d.StaffId,
                        FirstName = d.FirstName,
                        PermissionDate = d.PermissionDate,
                        FromTime = d.FromTime,
                        TimeTo = d.TimeTo,
                        PermissionOffReason = d.PermissionOffReason,
                        ContactNumber = d.ContactNumber,
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

                if (lst == null)
                {
                    return new List<PermissionRequest>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<PermissionRequest>();
            }
        }
        public List<COffRequest> GetCOffRequests(string reportingmanagerid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append(" EXEC [DBO].[ApprovalListForCOFFApplication] @reportingmanagerid");

            try
            {
                var lst = context.Database.SqlQuery<COffRequest>(qryStr.ToString(), new SqlParameter("@reportingmanagerid", reportingmanagerid)).Select(d => new COffRequest()
                {
                    COffId = d.COffId,
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    COffReqDate = d.COffReqDate,
                    TotalDays = d.TotalDays,
                    COffReason = d.COffReason,
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

                if (lst == null)
                {
                    return new List<COffRequest>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<COffRequest>();
            }
        }
        public List<COffAvailling> GetCOffAvaillingRequests(string reportingmanagerid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("EXEC [DBO].[ApprovalListForCOFFAvailingApplication] @reportingmanagerid");
            try
            {
                var lst = context.Database.SqlQuery<COffAvailling>(qryStr.ToString(), new SqlParameter("@reportingmanagerid", reportingmanagerid)).Select(d => new COffAvailling()
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
            catch 
            {
                return new List<COffAvailling>();
            }
        }

        public List<LaterOffRequest> GetLaterOffRequests(string reportingmanagerid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select COffId , StaffId , FirstName , LaterOffReqDate , LaterOffAvailDate , " +
                          "LaterOffReason , convert ( varchar , ApprovalStatusId ) as ApprovalStatusId , " +
                          "ApprovalStatusName , ApprovalStaffId , ApprovalStaffName , ApplicationApprovalId , " +
                          "ApprovedOnDate , ApprovedOnTime , Comment , ApprovalOwner from vwLaterOffApproval " +
                          "where ApprovalOwner = @reportingmanagerid and ApprovalStatusId = 1 AND ISCANCELLED = 'NO'");
            try
            {
                var lst =
                    context.Database.SqlQuery<LaterOffRequest>(qryStr.ToString(), new SqlParameter("@reportingmanagerid", reportingmanagerid)).Select(d => new LaterOffRequest()
                    {
                        COffId = d.COffId,
                        StaffId = d.StaffId,
                        FirstName = d.FirstName,
                        LaterOffReqDate = d.LaterOffReqDate,
                        LaterOffAvailDate = d.LaterOffAvailDate,
                        LaterOffReason = d.LaterOffReason,
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

        public List<MaintenanceOffRequest> GetMaintenanceOffRequests(string reportingmanagerid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select MaintenanceOffId , StaffId , FirstName , FromDate , ToDate , " +
                          "MaintenanceOffReason , ContactNumber , convert ( varchar , ApprovalStatusId ) as ApprovalStatusId , " +
                          "ApprovalStatusName , ApprovalStaffId , ApprovalStaffName , ApplicationApprovalId , ApprovedOnDate , " +
                          "ApprovedOnTime , Comment , ApprovalOwner from vwMaintenanceOffApproval where ApprovalStatusId = 1 AND ISCANCELLED = 'NO' " +
                          "and ApprovalOwner = @reportingmanagerid");

            try
            {
                var lst =
                    context.Database.SqlQuery<MaintenanceOffRequest>(qryStr.ToString(), new SqlParameter("@reportingmanagerid", reportingmanagerid))
                        .Select(d => new MaintenanceOffRequest()
                        {
                            MaintenanceOffId = d.MaintenanceOffId,
                            StaffId = d.StaffId,
                            FirstName = d.FirstName,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate,
                            MaintenanceOffReason = d.MaintenanceOffReason,
                            ContactNumber = d.ContactNumber,
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
            qrySty.Append("SELECT OTApplicationId ,StaffId ,FirstName ,OTDate ,OTTime ," +
                           "OTDuration ,OTReason ,convert ( varchar , ApprovalStatusId ) as ApprovalStatusId  ,ApprovalStatusName ," +
                           "ApprovalStaffId ,ApprovalStaffName ,ApplicationApprovalId ,ApprovedOnDate ," +
                           "ApprovedOnTime ,Comment ,ApprovalOwner FROM vwOTApproval WHERE ApprovalOwner = @reportingmanagerid " +
                           "AND ApprovalStatusId = 1 AND ISCANCELLED = 'NO'");

            try
            {
                var lst = context.Database.SqlQuery<OTRequest>(qrySty.ToString(), new SqlParameter("@reportingmanagerid", reportingmanagerid)).Select(d => new OTRequest()
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

        public void ApproveApplication(string ApprovalId, int ApprovalStatusId, string ApproverId, string ParentType, string LocationId)
        {
            var repo1 = new CommonRepository();
            bool AFlag = false;
            if (ApprovalStatusId == 2)
            {
                AFlag = true;
            }
            else
            {
                AFlag = false;
            }
            repo1.ApplicationApprovalRejection(ApproverId, ApprovalId, AFlag, ParentType, LocationId);
        }

        public void SendEmailToRequester(string ApprovalId)
        {
            StringBuilder qryStr = new StringBuilder();

            qryStr.Clear();
            qryStr.Append("select ParentId, ParentType from ApplicationApproval where [Id] = @ApprovalId");
            var lst = context.Database.SqlQuery<RALeaveDonation>(qryStr.ToString(), new SqlParameter("@ApprovalId", ApprovalId)).ToList();
            //get the application id and the application type based on the approval id.
            //check the type of application.
            //if the type of application is LeaveApplication then...
            //get the leave application details based on the leave application id.
        }

        public List<ODRequest> GetOD_OR_BTRequest(string ReportingManagerId, string ApplicationType)
        {
            SqlParameter[] Param = new SqlParameter[2];
            Param[0] = new SqlParameter("@StaffId", ReportingManagerId);
            Param[1] = new SqlParameter("@ApplicationType", ApplicationType);
            var QryStr = new StringBuilder();
            QryStr.Append("EXEC [DBO].[ApprovalListForOnDutyApplication] @StaffId,@ApplicationType");
            try
            {
                var lst = context.Database.SqlQuery<ODRequest>(QryStr.ToString(), Param).Select(d => new ODRequest()
                {
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

                if (lst == null)
                {
                    return new List<ODRequest>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<ODRequest>();
            }
        }

        public List<CompleteHeadCount> ShowCompleteHeadCount(string id, string TxnDate)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("exec [DBO].[GetHeadCounts] 5,NULL,NULL,NULL,NULL,@id,@TxnDate");
            try
            {
                var lst = context.Database.SqlQuery<CompleteHeadCount>(qryStr.ToString(), new SqlParameter("@id", id)
                    , new SqlParameter("@TxnDate", TxnDate)).Select(d => new CompleteHeadCount
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

        public List<Remainder> RemainderDetails(string StaffId)
        {
            List<Remainder> lst = new List<Remainder>();
            try
            {
                var qryStr = new StringBuilder();
                qryStr.Append("select (select value from settings where id=15) as LastAttendanceprocessed, " +
                    "(Select convert(varchar,max(Tr_Created)) from smaxtransaction) as LasttransactionSyncDate, " +
                    "(select count(*) from RequestApplication where Requestapplicationtype not in ('MP','RH') and " +
                    "convert(date,getdate()) between convert(Date,StartDate) and convert(Date,EndDate) and isapproved=1)  " +
                    "as PlannedLeaveCount,(select count(t.TransactionDate) as HeadCount from ( select max(Tr_Date) as " +
                    "TransactionDate,Tr_chid as EmpId,Tr_TType from SMaxTransaction A where convert(date,Tr_Date) = " +
                    "convert(date,getdate()) and tr_ttype=20 group by tr_chid,Tr_ttype) as t) as HeadCount,(select count(*) from " +
                    "staffpersonal where convert(varchar(5),DateOfBirth, 110) = convert(varchar(5), getdate(), 110) and " +
                    "StaffId not in (@StaffId)) as BirthdayAlert");

                lst = context.Database.SqlQuery<Remainder>(qryStr.ToString(), new SqlParameter("@StaffId", StaffId)).ToList();
            }
            catch (Exception e)
            {
                throw e; ;
            }
            return lst;
        }

        public List<CompleteHeadCount> ShowLiveHeadCount()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select count(t.TransactionDate) as HeadCount from ( select max(Tr_Date) as TransactionDate," +
                "Tr_chid as EmpId,Tr_TType from SMaxTransaction A where convert(date,Tr_Date) between convert(date,getdate()-1) " +
                "and convert(date,getdate()) and tr_ttype=20 group by tr_chid,Tr_ttype) as t");
            try
            {
                var lst = context.Database.SqlQuery<CompleteHeadCount>(qryStr.ToString()).Select(d => new CompleteHeadCount
                {
                    HeadCount = d.HeadCount
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
            qryStr.Append("SELECT * FROM fnGetFirstInLastOutNewV1(@LoggedInUserId,@SelectedDate,@SelectedDate)");

            try
            {
                var lst = context.Database.SqlQuery<FirstInLastOutDiamlerNew>(qryStr.ToString(),
                    new SqlParameter("@LoggedInUserId", LoggedInUserId), new SqlParameter("@SelectedDate", SelectedDate)).Select(d => new FirstInLastOutDiamlerNew()
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
            catch 
            {
                return new FirstInLastOutDiamlerNew();
            }
        }

        public DocumentData GetDocumentData(string Id)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT ParentId as LeaveApplicationId, FileContent,TypeOfDocument FROM DocumentUpload " +
                "where ParentId=@Id");

            try
            {
                var lst = context.Database.SqlQuery<DocumentData>(qryStr.ToString(), new SqlParameter("@Id", Id)).Select(d => new DocumentData()
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
            catch 
            {
                return new DocumentData();
            }
        }

        public GetEmpRoleModel GetEmpRole(string Staffid)
        {
            GetEmpRoleModel ObjM = new GetEmpRoleModel();
            var qryStr = new StringBuilder();
            try
            {
                qryStr.Append("select Roles,Responsibilities,Authorities from RolesAndResponsibilities where staffid=@Staffid");
                ObjM = context.Database.SqlQuery<GetEmpRoleModel>(qryStr.ToString(), new SqlParameter("@Staffid", Staffid)).FirstOrDefault();
            }
            catch 
            {
                return new GetEmpRoleModel();
            }
            return ObjM;
        }

        //public void Dispose()
        //{
        //    ((IDisposable)context).Dispose();
        //}
    }
}
