using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;
using System.Configuration;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

namespace Attendance.Repository
{
    public class ApplicationEntryRepository : IDisposable
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

        public ApplicationEntryRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<ApplicationEntryList> GetApplicationEntry()
        {

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT top 200 CONVERT ( VARCHAR , A.Id ) AS Id ,A.IsCancelled ,A.CancelledOn,A.CancelledBy , B.StaffId , DBO.FNGETSTAFFNAME(B.STAFFID) AS StaffName , B.DEPTNAME as DepartmentName,B.DesignationName,B.GradeName, ApplicationId , Reason ,D.Name as LeaveType ,D.Id as " +
                "LeaveTypeId , C.LeaveStartDurationId , C.LeaveEndDurationid , CASE WHEN REASON = 'LA' THEN REPLACE ( CONVERT ( VARCHAR , FROMDATE , 106 ) , ' ' , '-' ) " +
                //"WHEN REASON = 'PO' THEN REPLACE ( CONVERT ( VARCHAR , FROMDATE , 106 ) , ' ' , '-' ) + ' ' + CONVERT ( VARCHAR(5) , FROMDATE , 114 ) " +
                "WHEN REASON = 'PO' THEN (select  REPLACE ( CONVERT ( VARCHAR , Permissiondate , 106 ) , ' ' , '-' ) + CONVERT ( VARCHAR(5) , FROMDATE , 114 )  from PermissionOff where ApplicationId = id   ) " +
                "WHEN REASON = 'CO' THEN REPLACE ( CONVERT ( VARCHAR , FROMDATE , 106 ) , ' ' , '-' )" +
                " WHEN REASON = 'MP' THEN REPLACE ( CONVERT ( VARCHAR , FROMDATE , 106 ) , ' ' , '-' ) + ' ' + CONVERT ( VARCHAR(5) , FROMDATE , 114 ) " +
                "WHEN REASON = 'OD' THEN REPLACE ( CONVERT ( VARCHAR , FROMDATE , 106 ) , ' ' , '-' ) " +
                "WHEN REASON = 'RH' THEN REPLACE ( CONVERT ( VARCHAR , FROMDATE , 106 ) , ' ' , '-' ) " +
                "WHEN REASON = 'LO' THEN REPLACE ( CONVERT ( VARCHAR , FROMDATE , 106 ) , ' ' , '-' ) " +
                "WHEN REASON = 'MO' THEN REPLACE ( CONVERT ( VARCHAR , FROMDATE , 106 ) , ' ' , '-' ) END AS FromDate , " +
                "CASE WHEN REASON = 'LA' THEN REPLACE ( CONVERT ( VARCHAR , TODATE , 106 ) , ' ' , '-' ) " +
                "WHEN REASON = 'PO' THEN (select  REPLACE ( CONVERT ( VARCHAR , Permissiondate , 106 ) , ' ' , '-' ) + CONVERT ( VARCHAR(5) , Todate , 114 )  from PermissionOff where ApplicationId = id   ) " +
                "WHEN REASON = 'CO' THEN REPLACE ( CONVERT ( VARCHAR , TODATE , 106 ) , ' ' , '-' ) " +
                "WHEN REASON = 'MP' THEN REPLACE ( CONVERT ( VARCHAR , TODATE , 106 ) , ' ' , '-' ) + ' ' + CONVERT ( VARCHAR(5) , TODATE , 114 ) " +
                "WHEN REASON = 'OD' THEN REPLACE ( CONVERT ( VARCHAR , TODATE , 106 ) , ' ' , '-' ) " +
                "WHEN REASON = 'RH' THEN REPLACE ( CONVERT ( VARCHAR , TODATE , 106 ) , ' ' , '-' ) " +
                "WHEN REASON = 'LO' THEN REPLACE ( CONVERT ( VARCHAR , TODATE , 106 ) , ' ' , '-' ) " +
                "WHEN REASON = 'MO' THEN REPLACE ( CONVERT ( VARCHAR , TODATE , 106 ) , ' ' , '-' ) END AS ToDate , " +
                "A.TotalDays , A.Remarks FROM APPLICATIONENTRY A INNER JOIN STAFFVIEW B   ON A.STAFFID = B.STAFFID LEFT JOIN LeaveApplicationWabco C " +
                 " ON A.ApplicationId = C.Id LEFT JOIN LeaveType D ON C.LeaveTypeId = D.Id INNER JOIN ApplicationApproval AA ON A.ApplicationId = AA.ParentId where AA.ApprovalStatusId = 2 ORDER BY A.ID DESC");

            try
            {
                var lst = context.Database.SqlQuery<ApplicationEntryList>(qryStr.ToString()).Select(d => new ApplicationEntryList()
                {
                    Id = d.Id,
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    DepartmentName = d.DepartmentName,
                    DesignationName = d.DesignationName,
                    GradeName = d.GradeName,
                    ApplicationId = d.ApplicationId,
                    Reason = d.Reason,
                    FromDate = d.FromDate,
                    ToDate = d.ToDate,
                    Remarks = d.Remarks,
                    CreatedOn = d.CreatedOn,
                    CreatedBy = d.CreatedBy,
                    ModifiedOn = d.ModifiedOn,
                    ModifiedBy = d.ModifiedBy,
                    IsCancelled = d.IsCancelled,
                    CancelledOn = d.CancelledOn,
                    //  Convert.ToString( d.CancelledOn ,"yy-M-dd") ,
                    CancelledBy = d.CancelledBy,
                    TotalDays = d.TotalDays,
                    LeaveType = d.LeaveType,
                    LeaveStartDurationId = d.LeaveStartDurationId,
                    LeaveEndDurationId = d.LeaveEndDurationId,
                    LeaveTypeId = d.LeaveTypeId,
                }).ToList();

                if (lst == null)
                {
                    return new List<ApplicationEntryList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ApplicationEntryList>();
            }
        }


        public List<LeaveTypeList> GetAllLeaves()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select '0'  as id , '-- Select Leave --' as Name union select id , Name from LeaveType where IsActive = 1 order by Name Asc");
            try
            {
                var leaveList = context.Database.SqlQuery<LeaveTypeList>(qryStr.ToString()).Select(d => new LeaveTypeList()
                {
                    Id = d.Id,
                    Name = d.Name

                }).ToList();
                if (leaveList == null)
                {
                    return new List<LeaveTypeList>();
                }
                else
                {
                    return leaveList;
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        //#############################
        //DELETE CODING
        // #############################
        public string Delete(string id, string Type, string StaffId, string TotalDays, string cancelledBy)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@Id", id);

            if (Type == "LA")
            {
                var QryStr = new StringBuilder();
                QryStr.Clear();
                var QryString = new StringBuilder();
                string LeaveTypeId = string.Empty;
                QryString.Append("Select LeaveTypeId from LeaveApplicationWabco where Id=@Id");

                QryStr.Append("Update LeaveApplicationWabco Set IsCancelled=1 where id=@Id");

                try
                {
                    LeaveTypeId = context.Database.SqlQuery<string>(QryString.ToString(), sqlParameter).FirstOrDefault();
                }
                catch (Exception)
                {
                    throw;
                }
                context.Database.ExecuteSqlCommand(QryStr.ToString());
                CancelLeaveEmployeeLeaveAccount(id, Type, StaffId, TotalDays, LeaveTypeId);
            }
            else if (Type == "PO")
            {
                var QryStr = new StringBuilder();
                QryStr.Clear();
                QryStr.Append("Update PermissionOff Set IsCancelled=1 where id=@Id");
                context.Database.ExecuteSqlCommand(QryStr.ToString(), sqlParameter);
            }

            else if (Type == "CO")
            {
                var QryStr = new StringBuilder();
                QryStr.Clear();
                QryStr.Append("Update CompensatoryOff Set IsCancelled=1 where id=@Id");
                context.Database.ExecuteSqlCommand(QryStr.ToString(),sqlParameter);
            }
            else if (Type == "MP")
            {
                RemovePunchesFromSmaxTransanction(id, StaffId);
                var QryStr = new StringBuilder();
                QryStr.Clear();
                QryStr.Append("Update ManualPunch Set IsCancelled=1 where id=@Id");
                context.Database.ExecuteSqlCommand(QryStr.ToString(), sqlParameter);
            }
            else if (Type == "OD")
            {
                var QryStr = new StringBuilder();
                QryStr.Clear();
                QryStr.Append("Update ODApplication Set IsCancelled=1 where id=@Id");
                context.Database.ExecuteSqlCommand(QryStr.ToString(), sqlParameter);
            }
            else if (Type == "RH")
            {
                var QryStr = new StringBuilder();
                QryStr.Clear();
                QryStr.Append("Update RHApplication Set IsCancelled=1 where id=@Id");
                context.Database.ExecuteSqlCommand(QryStr.ToString(), sqlParameter);
            }
            else if (Type == "LO")
            {
                var QryStr = new StringBuilder();
                QryStr.Clear();
                QryStr.Append("Update LaterOff Set IsCancelled=1 where id=@Id");
                context.Database.ExecuteSqlCommand(QryStr.ToString(), sqlParameter);
            }
            else if (Type == "MO")
            {
                var QryStr = new StringBuilder();
                QryStr.Clear();
                QryStr.Append("Update MaintenanceOff Set IsCancelled=1 where id=@Id");
                context.Database.ExecuteSqlCommand(QryStr.ToString(), sqlParameter);
            }
            var QryStr1 = new StringBuilder();
            QryStr1.Clear();
            QryStr1.Append("Update applicationEntry Set IsCancelled=1 ,CancelledOn = GetDate() ," +
                " CancelledBy = '" + cancelledBy + "' where ApplicationId=@Id");
            context.Database.ExecuteSqlCommand(QryStr1.ToString(), sqlParameter);
            return "Canceled";
        }


        public string ValidateLeaveApplication(string StaffId, string FromDate, string ToDate, string LeaveType, string TotalDays)
        {
            SqlParameter[] sqlParameter = new SqlParameter[5];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            sqlParameter[1] = new SqlParameter("@FromDate", FromDate);
            sqlParameter[2] = new SqlParameter("@ToDate", ToDate);
            sqlParameter[3] = new SqlParameter("@LeaveType", LeaveType);
            sqlParameter[4] = new SqlParameter("@TotalDays", TotalDays);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT dbo.[fnValidateLeaveApplication] (@StaffId,@FromDate,@ToDate , @LeaveType, @TotalDays)");
            var str = (context.Database.SqlQuery<string>(qryStr.ToString(), sqlParameter).FirstOrDefault()).ToString();
            return str;
        }
        public string ValidatePermissionOffApplication(string StaffId, string FromDate, string ToDate, string TotalHours)
        {
            SqlParameter[] sqlParameter = new SqlParameter[4];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            sqlParameter[1] = new SqlParameter("@FromDate", FromDate);
            sqlParameter[2] = new SqlParameter("@ToDate", ToDate);
            sqlParameter[3] = new SqlParameter("@TotalHours", TotalHours);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT dbo.[fnValidatePermission] (@StaffId,@ToDate , @TotalHours,'SHIFTSTART')");
            var str = (context.Database.SqlQuery<string>(qryStr.ToString(), sqlParameter).FirstOrDefault()).ToString();
            return str;
        }


        public void RemovePunchesFromSmaxTransanction(string id, string StaffId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            sqlParameter[1] = new SqlParameter("@Id", id);
            DateTime fromDate = DateTime.Now;

            // Get the Date value from Manual Punch Table fro deleting the Punch
            var QryStr1 = new StringBuilder();
            QryStr1.Clear();
            QryStr1.Append("Select InDateTime from  [ManualPunch] where Id = @Id AND StaffId = @StaffId ");

            try
            {
                fromDate = context.Database.SqlQuery<DateTime>(QryStr1.ToString(), sqlParameter).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            
            fromDate = fromDate.Date;
            string fromDate1 = string.Empty;
            fromDate1 = fromDate.ToString();
            int index1 = fromDate1.IndexOf(' ');
            string result = string.Empty;
            if (index1 != -1)
            {
                result = fromDate1.Remove(index1, 9); // Use integer from IndexOf.
            }
            string[] Dates = result.Split('-');
            string NewDate = Dates[2] + '-' + Dates[1] + '-' + Dates[0] + " " + "00:00:00.000";
            SqlParameter[] sqlParameter2 = new SqlParameter[2];
            sqlParameter2[0] = new SqlParameter("@StaffId", StaffId);
            sqlParameter2[1] = new SqlParameter("@NewDate", NewDate);
            // Delete the Manual punches from SmaxTransaction Table for the partcular employee 
            var QryStr2 = new StringBuilder();
            QryStr2.Clear();
            QryStr2.Append(" Delete from [SmaxTransaction]  where Tr_ChId = @StaffId AND  Convert(Date,Tr_Date)= Convert(Date,@NewDate) AND Tr_IpAddress = '192.168.0.223'");
            context.Database.ExecuteSqlCommand(QryStr2.ToString(), sqlParameter2);

        }


        public void CancelLeaveEmployeeLeaveAccount(string id, string Type, string StaffId, string TotalDays, string LeaveTypeId)
        {
            EmployeeLeaveAccount ela = new EmployeeLeaveAccount();

            ela.StaffId = StaffId;
            ela.LeaveTypeId = LeaveTypeId;
            ela.TransactionFlag = 1;
            ela.TransactionDate = DateTime.Now;
            ela.LeaveCount = Convert.ToDecimal(TotalDays);
            ela.Narration = "CANCELLED LEAVE - " + id;
            context.EmployeeLeaveAccount.Add(ela);
            context.SaveChanges();
        }

        //#############################
        //SEARCH CODING
        // #############################

        public List<ApplicationEntryList> Search(string StaffId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT top 100 CONVERT ( VARCHAR , A.Id ) AS Id ,A.IsCancelled,A.CancelledOn ,  B.StaffId , DBO.FNGETSTAFFNAME(B.STAFFID) AS StaffName , B.DEPTNAME as DepartmentName, B.DesignationName, B.GradeName,ApplicationId , Reason , D.Name as LeaveType , " +
                 " D.Id as LeaveTypeid , C.LeaveStartDurationId , LeaveEndDurationId , CASE WHEN REASON = 'LA' THEN REPLACE ( CONVERT ( VARCHAR , FROMDATE , 106 ) , ' ' , '-' ) " +
                //"WHEN REASON = 'PO' THEN REPLACE ( CONVERT ( VARCHAR , FROMDATE , 106 ) , ' ' , '-' ) + ' ' + CONVERT ( VARCHAR(5) , FROMDATE , 114 ) " +
                 "WHEN REASON = 'PO' THEN (select  REPLACE ( CONVERT ( VARCHAR , Permissiondate , 106 ) , ' ' , '-' ) + CONVERT ( VARCHAR(5) , FROMDATE , 114 )  from PermissionOff where ApplicationId = id   ) " +
                 "WHEN REASON = 'CO' THEN REPLACE ( CONVERT ( VARCHAR , FROMDATE , 106 ) , ' ' , '-' )" +
                 " WHEN REASON = 'MP' THEN REPLACE ( CONVERT ( VARCHAR , FROMDATE , 106 ) , ' ' , '-' ) + ' ' + CONVERT ( VARCHAR(5) , FROMDATE , 114 ) " +
                 "WHEN REASON = 'OD' THEN REPLACE ( CONVERT ( VARCHAR , FROMDATE , 106 ) , ' ' , '-' ) " +
                 "WHEN REASON = 'RH' THEN REPLACE ( CONVERT ( VARCHAR , FROMDATE , 106 ) , ' ' , '-' ) " +
                 "WHEN REASON = 'LO' THEN REPLACE ( CONVERT ( VARCHAR , FROMDATE , 106 ) , ' ' , '-' ) " +
                 "WHEN REASON = 'MO' THEN REPLACE ( CONVERT ( VARCHAR , FROMDATE , 106 ) , ' ' , '-' ) END AS FromDate , " +
                 "CASE WHEN REASON = 'LA' THEN REPLACE ( CONVERT ( VARCHAR , TODATE , 106 ) , ' ' , '-' ) " +
                 "WHEN REASON = 'PO' THEN (select  REPLACE ( CONVERT ( VARCHAR , Permissiondate , 106 ) , ' ' , '-' ) + CONVERT ( VARCHAR(5) , Todate , 114 )  from PermissionOff where ApplicationId = id   ) " +
                 "WHEN REASON = 'CO' THEN REPLACE ( CONVERT ( VARCHAR , TODATE , 106 ) , ' ' , '-' ) " +
                 "WHEN REASON = 'MP' THEN REPLACE ( CONVERT ( VARCHAR , TODATE , 106 ) , ' ' , '-' ) + ' ' + CONVERT ( VARCHAR(5) , TODATE , 114 ) " +
                 "WHEN REASON = 'OD' THEN REPLACE ( CONVERT ( VARCHAR , TODATE , 106 ) , ' ' , '-' ) " +
                 "WHEN REASON = 'RH' THEN REPLACE ( CONVERT ( VARCHAR , TODATE , 106 ) , ' ' , '-' ) " +
                 "WHEN REASON = 'LO' THEN REPLACE ( CONVERT ( VARCHAR , TODATE , 106 ) , ' ' , '-' ) " +
                 "WHEN REASON = 'MO' THEN REPLACE ( CONVERT ( VARCHAR , TODATE , 106 ) , ' ' , '-' ) END AS ToDate , " +
                 "A.TotalDays , A.Remarks FROM APPLICATIONENTRY A INNER JOIN STAFFVIEW B   ON A.STAFFID = B.STAFFID  LEFT JOIN LeaveApplicationWabco C " +
                 " ON A.ApplicationId = C.Id LEFT JOIN LeaveType D ON C.LeaveTypeId = D.Id where A.STAFFID = @StaffId ORDER BY A.ID DESC");
            try
            {
                var lst = context.Database.SqlQuery<ApplicationEntryList>(qryStr.ToString(), sqlParameter).Select(d => new ApplicationEntryList()
                {
                    Id = d.Id,
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    DepartmentName = d.DepartmentName,
                    DesignationName = d.DesignationName,
                    GradeName = d.GradeName,
                    ApplicationId = d.ApplicationId,
                    Reason = d.Reason,
                    FromDate = d.FromDate,
                    ToDate = d.ToDate,
                    Remarks = d.Remarks,
                    LeaveType = d.LeaveType,
                    CreatedOn = d.CreatedOn,
                    CreatedBy = d.CreatedBy,
                    ModifiedOn = d.ModifiedOn,
                    ModifiedBy = d.ModifiedBy,
                    TotalDays = d.TotalDays,
                    LeaveTypeId = d.LeaveTypeId,
                    LeaveStartDurationId = d.LeaveStartDurationId,
                    LeaveEndDurationId = d.LeaveEndDurationId,
                    IsCancelled = d.IsCancelled,
                    CancelledOn = d.CancelledOn,
                    CancelledBy = d.CancelledBy
                }).ToList();

                if (lst == null)
                {
                    return new List<ApplicationEntryList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ApplicationEntryList>();
            }
        }



        public EmpData GetEmpData(string StaffId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT DBO.FNGETSTAFFNAME(STAFFID) AS StaffName , DEPTNAME AS DepartmentName, DesignationName,GradeName FROM STAFFVIEW WHERE STAFFID = @StaffId");

            var data = context.Database.SqlQuery<EmpData>(qryStr.ToString(), sqlParameter).FirstOrDefault();

            try
            {
                if (data == null)
                {
                    return null;
                }
                else
                {
                    return data;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public void ApprovedLeaveEmployeeLeaveAccount(string id, string StaffId1, string TotalDays1, string LeaveTypeId)
        {
            EmployeeLeaveAccount ela = new EmployeeLeaveAccount();

            ela.StaffId = StaffId1;

            if (LeaveTypeId == "LV0004")
            {
                ela.LeaveTypeId = "LV0020";
                ela.LeaveCount = Convert.ToDecimal(TotalDays1) * (-1) * 2;
            }
            else
            {
                ela.LeaveTypeId = LeaveTypeId;
                ela.LeaveCount = Convert.ToDecimal(TotalDays1) * (-1);
            }

            ela.TransactionFlag = 2;
            ela.TransactionDate = DateTime.Now;

            ela.Narration = "APPROVED LEAVE - " + id;


            context.EmployeeLeaveAccount.AddOrUpdate(ela);
            context.SaveChanges();
        }

        public string EditEmployeeLeaveAccount(string id, string StaffId1, string TotalDays1)
        {
            //var totalDaysValue = "";

            //totalDaysValue = Convert.ToDecimal(TotalDays1) * (-1);
            SqlParameter[] sqlParameter = new SqlParameter[3];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId1);
            sqlParameter[1] = new SqlParameter("@Id", id);
            sqlParameter[2] = new SqlParameter("@TotalDays1", Convert.ToDecimal(TotalDays1) * (-1));

            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("update employeeleaveaccount set LeaveCount=@TotalDays1 where staffid=@StaffId and Narration= '" + "APPROVED LEAVE - @Id");
            context.Database.ExecuteSqlCommand(QryStr.ToString(), sqlParameter);
            return "Canceled";
        }
        public void SaveApplicationEntry(ApplicationEntryList _AE_, string UserFullName)
        {
            var AE = new ApplicationEntry();
            string ApplicationId = string.Empty;
            string StaffId1 = string.Empty;
            string TotalDays1 = string.Empty;
            DateTime currentDate = DateTime.Now;
            DateTime toDate = DateTime.Now.AddDays(-1);
            DateTime toDate1 = DateTime.Now;
            var applicationDate = DateTime.Now;
            var fromDate = _AE_.FromDate;
            string ReportingManager = string.Empty;
            bool selfapproval = true;
            var repo = new CommonRepository();
            string BaseAddress = string.Empty;
            var crepo = new CommonRepository();
            var enableApplicationApproval = ConfigurationManager.AppSettings["EnableApplicationApproval"].ToString().Trim();
            if (enableApplicationApproval == "true")
            {
                ReportingManager = crepo.GetReportingManager(_AE_.StaffId);
                if (string.IsNullOrEmpty(ReportingManager) == true)
                {
                    ReportingManager = _AE_.StaffId;
                    selfapproval = false;
                }
                else
                {
                    selfapproval = false;
                }
            }
            else
            {
                selfapproval = true;
            }

            if (fromDate == "1900-01-01 00:00:00")
            {
                applicationDate = Convert.ToDateTime(_AE_.ToDate);
            }
            if (fromDate == null)
            {
                applicationDate = Convert.ToDateTime(_AE_.ToDate);
            }
            if (fromDate != null)
            {
                applicationDate = Convert.ToDateTime(_AE_.FromDate);
            }

            if (_AE_.ToDate == "1900-01-01 00:00:00")
            {
                toDate1 = Convert.ToDateTime(_AE_.FromDate);
            }
            if (_AE_.ToDate != null)
            {
                toDate1 = Convert.ToDateTime(_AE_.ToDate);
            }

            if (_AE_.Reason == "CO")
            {
                applicationDate = Convert.ToDateTime(_AE_.ToDate);
                toDate1 = Convert.ToDateTime(_AE_.ToDate);

            }

            if (_AE_.Reason == "PO")
            {
                applicationDate = Convert.ToDateTime(_AE_.PermissionDate);
                toDate1 = Convert.ToDateTime(_AE_.PermissionDate);
            }
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    if (enableApplicationApproval == "false")
                    {
                        if (applicationDate.Date < currentDate.Date)
                        {
                            if (_AE_.ToDate == null)
                            {
                                toDate1 = Convert.ToDateTime(_AE_.FromDate);
                            }
                            if (toDate1 < toDate)
                            {
                                toDate = toDate1;
                            }
                            var act = new AttendanceControlTable();
                            act.StaffId = _AE_.StaffId;
                            act.FromDate = applicationDate;
                            act.ToDate = toDate;
                            act.IsProcessed = false;
                            act.CreatedOn = DateTime.Now;
                            act.CreatedBy = UserFullName;
                            act.ApplicationType = _AE_.Reason;
                            context.AttendanceControlTable.AddOrUpdate(act);
                            context.SaveChanges();
                        }
                    }
                    if (_AE_.Reason == "LA")
                    {

                        
                        var law = new LeaveApplicationWabco();
                        var str = string.Empty;
                        var LeaveId = string.Empty;
                        var LeaveTypeId = string.Empty;
                        string leaveBalance = string.Empty;
                        SqlParameter[] sqlParameter = new SqlParameter[1];
                        sqlParameter[0] = new SqlParameter("@LeaveType", _AE_.LeaveType);

                        var qryString = new StringBuilder();
                        qryString.Clear();
                        qryString.Append(" Select Name from LeaveType where Id=@LeaveType");
                        string LeaveName = string.Empty;
                        Int32 id = 0;
                        try
                        {
                            LeaveName = context.Database.SqlQuery<string>(qryString.ToString(), sqlParameter).FirstOrDefault();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        SqlParameter[] sqlParameter2 = new SqlParameter[1];
                        sqlParameter2[0] = new SqlParameter("@LeaveName", LeaveName);
                        var qryString1 = new StringBuilder();
                        qryString1.Clear();
                        qryString1.Append(" Select Id from LeaveReason where Name  = @LeaveName");
                        try
                        {
                            id = context.Database.SqlQuery<Int32>(qryString1.ToString(), sqlParameter2).FirstOrDefault();
                        }
                        catch (Exception)
                        {
                            throw;
                        }

                        law.Id = _AE_.Id;
                        law.StaffId = _AE_.StaffId;
                        law.LeaveTypeId = _AE_.LeaveType;
                        law.LeaveStartDate = Convert.ToDateTime(_AE_.FromDate);
                        law.LeaveStartDurationId = Convert.ToInt16(_AE_.LeaveStartDurationId);
                        law.LeaveEndDate = Convert.ToDateTime(_AE_.ToDate);
                        law.LeaveEndDurationId = Convert.ToInt16(_AE_.LeaveEndDurationId);
                        law.ContactNumber = string.Empty;
                        law.IsCancelled = false;
                        law.Remarks = _AE_.Remarks;
                        law.ReasonId = id;
                        law.TotalDays = Convert.ToDecimal(_AE_.Total);
                        SaveLeaveApplicationDetails(law);
                        LeaveId = _AE_.Id;

                        ApplicationId = law.Id;
                        StaffId1 = law.StaffId;
                        TotalDays1 = _AE_.Total;
                        LeaveTypeId = _AE_.LeaveType;

                        if (LeaveId == null)
                        {
                            repo.SaveIntoApplicationApproval(law.Id, "LA", law.StaffId, ReportingManager, selfapproval);

                            if (selfapproval == true)
                            {
                                ApprovedLeaveEmployeeLeaveAccount(ApplicationId, StaffId1, TotalDays1, LeaveTypeId);
                            }

                        }
                        else
                        {
                            EditEmployeeLeaveAccount(ApplicationId, StaffId1, TotalDays1);
                        }


                        //MAIL SEND

                        try
                        {
                            //try to get the server ip from the web.config file.
                            BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
                            //check if the server ip address has been given or not.
                            if (string.IsNullOrEmpty(BaseAddress) == true) //if the server ip address has not been given then...
                                //throw exception.
                                throw new Exception("BaseAddress parameter is blank in web.config file.");
                        }
                        catch (Exception)
                        {
                            throw;
                        }

                        //get the emailid of the reporting manager.
                        var ReportingManagerEmailId = repo.GetEmailIdOfEmployee(ReportingManager);
                        //get the emailid of the staff who raises the leave application.
                        var StaffEmailId = repo.GetEmailIdOfEmployee(law.StaffId);
                        //get the name of the staff.
                        var StaffName = repo.GetStaffName(law.StaffId);
                        //get the name of the reporting manager.
                        var ReportingManagerName = repo.GetStaffName(ReportingManager);

                        //check if the reporting manager has an email id.
                        //if (string.IsNullOrEmpty(ReportingManagerEmailId) == true) //if the reporting manager does not have an email id then...
                        //{
                        //    //check if the staff has an email id.
                        //    if (string.IsNullOrEmpty(StaffEmailId) == true) //if the staff does not have an email id then...
                        //    {
                        //        //do not take any action.
                        //    }
                        //    else //if the staff has an email id then...
                        //    {
                        //        var EmailStr = string.Empty;
                        //        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your leave application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(law.LeaveStartDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(law.LeaveEndDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + law.Remarks + "</td></tr></table></p><p>But an intimation email could not be sent to your reporting manager because of a missing email id.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //        //send intimation to the staff stating that his/her leave application has been acknowleged 
                        //        //function call to get the name of the staff and the reporting manager.
                        //        //  but the reporting manager does not have a email id so no intimation has been sent to him.
                        //        repo.SendEmailMessage("", StaffEmailId, "", "", "Leave application of " + law.StaffId + " - " + StaffName, EmailStr);
                        //    }
                        //}
                        //else // if the reporting manager has an email id then...
                        //{
                        //    var EmailStr = string.Empty;
                        //    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a leave. Leave details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">LeaveType:</td><td style=\"width:80%;\">" + LeaveName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(law.LeaveStartDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(law.LeaveEndDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + law.Remarks + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this application.</p><p><a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + law.Id + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + law.Id + "&Approve=false\">Reject</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //    // send intimation to the reporting manager about the leave application.
                        //    repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "Leave application of " + StaffName, EmailStr);

                        //    // send acknowledgement to the staff who raised the leave application.
                        //    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your leave application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">LeaveType:</td><td style=\"width:80%;\">" + LeaveName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(law.LeaveStartDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(law.LeaveEndDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + law.Remarks + "</td></tr></table></p><p>This application has been sent also to your reporting manager.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //    repo.SendEmailMessage("", StaffEmailId, "", "", "Leave application sent to " + ReportingManagerName, EmailStr);
                        //}

                    }
                    else if (_AE_.Reason == "PO")
                    {
                       
                        var po = new PermissionOff();
                        var PermissionOffId = string.Empty;
                        po.Id = _AE_.Id;
                        po.StaffId = _AE_.StaffId;
                        po.PermissionDate = Convert.ToDateTime(_AE_.PermissionDate);
                        po.TimeFrom = Convert.ToDateTime(_AE_.FromDate);
                        po.TimeTo = Convert.ToDateTime(_AE_.ToDate);
                        po.Reason = _AE_.Remarks;
                        po.ContactNumber = string.Empty;
                        po.IsCancelled = false;
                        po.PermissionType = "SHIFTSTART";
                        po.TotalHours = Convert.ToDateTime(_AE_.TotalHours);

                        SavePermissionOffDetails(po);
                        // ReportingManager = string.Empty;
                        // selfapproval = true;
                        ApplicationId = po.Id;

                        PermissionOffId = _AE_.Id;
                        if (PermissionOffId == null)
                        {
                            repo.SaveIntoApplicationApproval(po.Id, "PO", po.StaffId, ReportingManager, selfapproval);
                        }

                        //try
                        //{
                        //    //try to get the server ip from the web.config file.
                        //    BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
                        //    //check if the server ip address has been given or not.
                        //    if (string.IsNullOrEmpty(BaseAddress) == true) //if the server ip address has not been given then...
                        //        //throw exception.
                        //        throw new Exception("BaseAddress parameter is blank in web.config file.");
                        //}
                        //catch (Exception)
                        //{
                        //    throw;
                        //}

                        ////get the emailid of the reporting manager.
                        //var ReportingManagerEmailId = repo.GetEmailIdOfEmployee(ReportingManager);
                        ////get the emailid of the staff who raises the Permission application.
                        //var StaffEmailId = repo.GetEmailIdOfEmployee(po.StaffId);
                        ////get the name of the staff.
                        //var StaffName = repo.GetStaffName(po.StaffId);
                        ////get the name of the reporting manager.
                        //var ReportingManagerName = repo.GetStaffName(ReportingManager);

                        ////check if the reporting manager has an email id.
                        //if (string.IsNullOrEmpty(ReportingManagerEmailId) == true) //if the reporting manager does not have an email id then...
                        //{
                        //    //check if the staff has an email id.
                        //    if (string.IsNullOrEmpty(StaffEmailId) == true) //if the staff does not have an email id then...
                        //    {
                        //        //do not take any action.
                        //    }
                        //    else //if the staff has an email id then...
                        //    {
                        //        var EmailStr = string.Empty;
                        //        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your permission application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">Hours:</td><td style=\"width:80%;\">" + Convert.ToDateTime(po.TotalHours).ToString("HH:mm") + "</td></tr><tr><td style=\"width:20%;\">Type:</td><td style=\"width:80%;\">" + po.PermissionType + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + po.Reason + "</td></tr></table></p><p>But an intimation email could not be sent to your reporting manager because of a missing email id.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //        //send intimation to the staff stating that his/her Permission application has been acknowleged 
                        //        //function call to get the name of the staff and the reporting manager.
                        //        //  but the reporting manager does not have a email id so no intimation has been sent to him.
                        //        repo.SendEmailMessage("", StaffEmailId, "", "", "Permission application of " + po.StaffId + " - " + StaffName, EmailStr);
                        //    }
                        //}
                        //else // if the reporting manager has an email id then...
                        //{
                        //    var EmailStr = string.Empty;
                        //    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a permission. Permission details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(po.PermissionDate).ToString("dd/MM/yyyy") + "</td></tr><tr><td style=\"width:20%;\">Time from:</td><td style=\"width:80%;\">" + Convert.ToDateTime(po.TimeFrom).ToString("HH:mm") + "</td></tr><tr><td style=\"width:20%;\">Time To:</td><td style=\"width:80%;\">" + Convert.ToDateTime(po.TimeTo).ToString("HH:mm") + "</td></tr><tr><td style=\"width:20%;\">Total Hours:</td><td style=\"width:80%;\">" + Convert.ToDateTime(po.TotalHours).ToString("HH:mm") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + po.Reason + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this application.</p><p><a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + po.Id + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + po.Id + "&Approve=false\">Reject</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //    // send intimation to the reporting manager about the Permission application.
                        //    repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "Permission application of " + StaffName, EmailStr);

                        //    // send acknowledgement to the staff who raised the Permission application.
                        //    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Permission application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(po.PermissionDate).ToString("dd/MM/yyyy") + "</td></tr><tr><td style=\"width:20%;\">Hours:</td><td style=\"width:80%;\">" + Convert.ToDateTime(po.TotalHours).ToString("HH:mm") + "</td></tr><tr><td style=\"width:20%;\">Type:</td><td style=\"width:80%;\">" + po.PermissionType + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + po.Reason + "</td></tr></table></p><p>This application has been sent also to your reporting manager.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //    repo.SendEmailMessage("", StaffEmailId, "", "", "Permission application sent to " + ReportingManagerName, EmailStr);
                        //}

                    }
                    else if (_AE_.Reason == "CO")
                    {
                        
                        var co = new CompensatoryOff();
                        var COffId = string.Empty;

                        co.Id = _AE_.Id;
                        co.StaffId = _AE_.StaffId;
                        co.COffReqDate = Convert.ToDateTime(_AE_.FromDate);
                        co.COffAvailDate = Convert.ToDateTime(_AE_.ToDate);
                        co.Reason = _AE_.Remarks;
                        co.IsCancelled = false;

                        SaveCOffDetails(co);
                        //    ReportingManager = string.Empty;
                        //  selfapproval = true;
                        ApplicationId = co.Id;
                        COffId = _AE_.Id;
                        if (COffId == null)
                        {
                            repo.SaveIntoApplicationApproval(co.Id, "CO", co.StaffId, ReportingManager, selfapproval);
                        }


                        //try
                        //{
                        //    //try to get the server ip from the web.config file.
                        //    BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
                        //    //check if the server ip address has been given or not.
                        //    if (string.IsNullOrEmpty(BaseAddress) == true) //if the server ip address has not been given then...
                        //        //throw exception.
                        //        throw new Exception("BaseAddress parameter is blank in web.config file.");
                        //}
                        //catch (Exception)
                        //{
                        //    throw;
                        //}

                        ////get the emailid of the reporting manager.
                        //var ReportingManagerEmailId = repo.GetEmailIdOfEmployee(ReportingManager);
                        ////get the emailid of the staff who raises the leave application.
                        //var StaffEmailId = repo.GetEmailIdOfEmployee(co.StaffId);
                        ////get the name of the staff.
                        //var StaffName = repo.GetStaffName(co.StaffId);
                        ////get the name of the reporting manager.
                        //var ReportingManagerName = repo.GetStaffName(ReportingManager);

                        ////check if the reporting manager has an email id.
                        //if (string.IsNullOrEmpty(ReportingManagerEmailId) == true) //if the reporting manager does not have an email id then...
                        //{
                        //    //check if the staff has an email id.
                        //    if (string.IsNullOrEmpty(StaffEmailId) == true) //if the staff does not have an email id then...
                        //    {
                        //        //do not take any action.
                        //    }
                        //    else //if the staff has an email id then...
                        //    {
                        //        var EmailStr = string.Empty;
                        //        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Compensatory Off application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">Req Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.COffReqDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Avail Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.COffAvailDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + co.Reason + "</td></tr></table></p><p>But an intimation email could not be sent to your reporting manager because of a missing email id.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //        //send intimation to the staff stating that his/her Compensatory Off application has been acknowleged 
                        //        //function call to get the name of the staff and the reporting manager.
                        //        //  but the reporting manager does not have a email id so no intimation has been sent to him.
                        //        repo.SendEmailMessage("", StaffEmailId, "", "", "Compensatory Off application of " + co.StaffId + " - " + StaffName, EmailStr);
                        //    }
                        //}
                        //else // if the reporting manager has an email id then...
                        //{
                        //    var EmailStr = string.Empty;
                        //    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a Compensatory Off. Compensatory Off details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">Req Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.COffReqDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Avail Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.COffAvailDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + co.Reason + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this application.</p><p><a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + co.Id + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + co.Id + "&Approve=false\">Reject</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //    // send intimation to the reporting manager about the Compensatory Off application.
                        //    repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "Compensatory Off application of " + StaffName, EmailStr);

                        //    // send acknowledgement to the staff who raised the leave application.
                        //    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Compensatory Off application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">Req Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.COffReqDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Avail Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.COffAvailDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + co.Reason + "</td></tr></table></p><p>This application has been sent also to your reporting manager.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //    repo.SendEmailMessage("", StaffEmailId, "", "", "Compensatory Off application sent to " + ReportingManagerName, EmailStr);
                        //}

                    }
                    else if (_AE_.Reason == "MP")
                    {

                        var MpId = string.Empty;
                        var aa = new CommonRepository();
                        var mp = new ManualPunch();
                        mp.Id = _AE_.Id;
                        mp.StaffId = _AE_.StaffId;
                        if (_AE_.FromDate != null && _AE_.ToDate != null)
                        {
                            mp.InDateTime = Convert.ToDateTime(_AE_.FromDate);
                            mp.OutDateTime = Convert.ToDateTime(_AE_.ToDate);
                            mp.PunchType = "INOUT";
                            //repo.SaveInOutPunch(_AE_.FromDate, _AE_.ToDate, _AE_.StaffId);

                        }
                        else if (_AE_.ToDate == null)
                        {
                            mp.InDateTime = Convert.ToDateTime(_AE_.FromDate);
                            mp.OutDateTime = Convert.ToDateTime("1900-01-01 00:00:00");
                            mp.PunchType = "IN";
                            //repo.SaveInPunch(_AE_.FromDate, _AE_.StaffId);
                        }
                        else
                        {
                            mp.InDateTime = Convert.ToDateTime("1900-01-01 00:00:00");
                            mp.OutDateTime = Convert.ToDateTime(_AE_.ToDate);
                            mp.PunchType = "OUT";
                            //repo.SaveOutPunch(_AE_.ToDate, _AE_.StaffId);
                        }
                        mp.Reason = _AE_.Reason;
                        SaveManualPunchDetails(mp);
                        //   ReportingManager = string.Empty;
                        ApplicationId = mp.Id;
                        MpId = _AE_.Id;
                        if (MpId == null)
                        {
                            repo.SaveIntoApplicationApproval(ApplicationId, "MP", _AE_.StaffId, ReportingManager, selfapproval);
                        }

                        //try
                        //{
                        //    //try to get the server ip from the web.config file.
                        //    BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
                        //    //check if the server ip address has been given or not.
                        //    if (string.IsNullOrEmpty(BaseAddress) == true) //if the server ip address has not been given then...
                        //        //throw exception.
                        //        throw new Exception("BaseAddress parameter is blank in web.config file.");
                        //}
                        //catch (Exception)
                        //{
                        //    throw;
                        //}

                        ////get the emailid of the reporting manager.
                        //var ReportingManagerEmailId = aa.GetEmailIdOfEmployee(ReportingManager);
                        ////get the emailid of the staff who raises the leave application.
                        //var StaffEmailId = aa.GetEmailIdOfEmployee(mp.StaffId);
                        ////get the name of the staff.
                        //var StaffName = aa.GetStaffName(mp.StaffId);
                        ////get the name of the reporting manager.
                        //var ReportingManagerName = aa.GetStaffName(ReportingManager);

                        ////check if the reporting manager has an email id.
                        //if (string.IsNullOrEmpty(ReportingManagerEmailId) == true) //if the reporting manager does not have an email id then...
                        //{
                        //    //check if the staff has an email id.
                        //    if (string.IsNullOrEmpty(StaffEmailId) == true) //if the staff does not have an email id then...
                        //    {
                        //        //do not take any action.
                        //    }
                        //    else //if the staff has an email id then...
                        //    {
                        //        var EmailStr = string.Empty;
                        //        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Forgot to punch application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(mp.InDateTime).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(mp.OutDateTime).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + mp.Reason + "</td></tr></table></p><p>But an intimation email could not be sent to your reporting manager because of a missing email id.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //        //send intimation to the staff stating that his/her leave application has been acknowleged 
                        //        //function call to get the name of the staff and the reporting manager.
                        //        //  but the reporting manager does not have a email id so no intimation has been sent to him.
                        //        aa.SendEmailMessage("", StaffEmailId, "", "", "Manual punch application of " + mp.StaffId + " - " + StaffName, EmailStr);
                        //    }
                        //}
                        //else // if the reporting manager has an email id then...
                        //{
                        //    var EmailStr = string.Empty;
                        //    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a Forgot to punch application. Forgot to punch details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + mp.InDateTime + "</td></tr><tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + mp.OutDateTime + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + mp.Reason + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this application.</p><p><a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + mp.Id + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + mp.Id + "&Approve=false\">Reject</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //    // send intimation to the reporting manager about the leave application.
                        //    aa.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "Manual punch application of " + StaffName, EmailStr);

                        //    // send acknowledgement to the staff who raised the leave application.
                        //    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Forgot to punch application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + mp.InDateTime + "</td></tr><tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + mp.OutDateTime + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + mp.Reason + "</td></tr></table></p><p>This application has been sent also to your reporting manager.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //    aa.SendEmailMessage("", StaffEmailId, "", "", "Manual punch application sent to " + ReportingManagerName, EmailStr);
                        //}

                    }
                    else if (_AE_.Reason == "OD")
                    {
                        var ODId = string.Empty;

                       
                        var od = new ODApplication();

                        od.Id = _AE_.Id;
                        od.StaffId = _AE_.StaffId;
                        if (_AE_.FromDate == _AE_.ToDate)
                        {
                            od.ODDuration = "SINGLEDAY";
                        }
                        else
                        {
                            od.ODDuration = "MULTIPLEDAY";

                        }

                        od.From = Convert.ToDateTime(_AE_.FromDate);
                        od.To = Convert.ToDateTime(_AE_.ToDate);
                        od.ODReason = _AE_.Remarks;
                        od.IsCancelled = false;
                        od.CreatedOn = DateTime.Now;
                        od.CreatedBy = "-";
                        od.ModifiedOn = DateTime.Now;
                        od.ModifiedBy = "-";

                        SaveODApplication(od);
                        ApplicationId = od.Id;

                        ODId = _AE_.Id;
                        if (ODId == null)
                        {
                            repo.SaveIntoApplicationApproval(od.Id, "OD", _AE_.StaffId, ReportingManager, selfapproval);

                        }

                        //try
                        //{
                        //    //try to get the server ip from the web.config file.
                        //    BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
                        //    //check if the server ip address has been given or not.
                        //    if (string.IsNullOrEmpty(BaseAddress) == true) //if the server ip address has not been given then...
                        //        //throw exception.
                        //        throw new Exception("BaseAddress parameter is blank in web.config file.");
                        //}
                        //catch (Exception)
                        //{
                        //    throw;
                        //}

                        ////get the emailid of the reporting manager.
                        //var ReportingManagerEmailId = repo.GetEmailIdOfEmployee(ReportingManager);
                        ////get the emailid of the staff who raises the leave application.
                        //var StaffEmailId = repo.GetEmailIdOfEmployee(od.StaffId);
                        ////get the name of the staff.
                        //var StaffName = repo.GetStaffName(od.StaffId);
                        ////get the name of the reporting manager.
                        //var ReportingManagerName = repo.GetStaffName(ReportingManager);

                        ////check if the reporting manager has an email id.
                        //if (string.IsNullOrEmpty(ReportingManagerEmailId) == true) //if the reporting manager does not have an email id then...
                        //{
                        //    //check if the staff has an email id.
                        //    if (string.IsNullOrEmpty(StaffEmailId) == true) //if the staff does not have an email id then...
                        //    {
                        //        //do not take any action.
                        //    }
                        //    else //if the staff has an email id then...
                        //    {
                        //        var EmailStr = string.Empty;
                        //        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Out Door application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(od.From).ToString("dd-MMM-yyyy HH:mm") + "</td></tr><tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(od.To).ToString("dd-MMM-yyyy HH:mm") + "</td></tr><tr><td style=\"width:20%;\">Duration:</td><td style=\"width:80%;\">" + od.ODDuration + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + od.ODReason + "</td></tr></table></p><p>But an intimation email could not be sent to your reporting manager because of a missing email id.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //        //send intimation to the staff stating that his/her Out Door application has been acknowleged 
                        //        //function call to get the name of the staff and the reporting manager.
                        //        //  but the reporting manager does not have a email id so no intimation has been sent to him.
                        //        repo.SendEmailMessage("", StaffEmailId, "", "", "Out Door application of " + od.StaffId + " - " + StaffName, EmailStr);
                        //    }
                        //}
                        //else // if the reporting manager has an email id then...
                        //{
                        //    var EmailStr = string.Empty;
                        //    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a Out Door. Out Door details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(od.From).ToString("dd-MMM-yyyy HH:mm") + "</td></tr><tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(od.To).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Duration:</td><td style=\"width:80%;\">" + od.ODDuration + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + od.ODReason + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this application.</p><p><a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + od.Id + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + od.Id + "&Approve=false\">Reject</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //    // send intimation to the reporting manager about the Out Door application.
                        //    repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "Out Door application of " + StaffName, EmailStr);

                        //    // send acknowledgement to the staff who raised the leave application.
                        //    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Out Door application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(od.From).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(od.To).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Duration:</td><td style=\"width:80%;\">" + od.ODDuration + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + od.ODReason + "</td></tr></table></p><p>This application has been sent also to your reporting manager.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //    repo.SendEmailMessage("", StaffEmailId, "", "", "Out Door application sent to " + ReportingManagerName, EmailStr);
                        //}




                    }
                    else if (_AE_.Reason == "RH")
                    {
                        var remarks = _AE_.Remarks;
                        var RHId = string.Empty;
                        
                        var rh = new RHApplication();
                        var str = Microsoft.VisualBasic.Strings.Split(remarks, "@");

                        rh.Id = _AE_.Id;
                        rh.StaffId = _AE_.StaffId;
                        rh.RHId = Convert.ToInt16(str[1]);
                        rh.ApplicationDate = DateTime.Now;
                        rh.IsCancelled = false;
                        rh.CreatedOn = DateTime.Now;
                        rh.CreatedBy = "-";
                        rh.ModifiedOn = DateTime.Now;
                        rh.ModifiedBy = "-";

                        SaveRHApplication(rh);
                        //  ReportingManager = string.Empty;
                        ApplicationId = rh.Id;

                        RHId = _AE_.Id;

                        if (RHId == null)
                        {
                            repo.SaveIntoApplicationApproval(rh.Id, "RH", _AE_.StaffId, ReportingManager, selfapproval);

                        }

                        //try
                        //{
                        //    //try to get the server ip from the web.config file.
                        //    BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
                        //    //check if the server ip address has been given or not.
                        //    if (string.IsNullOrEmpty(BaseAddress) == true) //if the server ip address has not been given then...
                        //        //throw exception.
                        //        throw new Exception("BaseAddress parameter is blank in web.config file.");
                        //}
                        //catch (Exception)
                        //{
                        //    throw;
                        //}

                        //RestrictedHolidays RH = null;
                        //try
                        //{
                        //    rh.RestrictedHolidays = context.Database.SqlQuery<RestrictedHolidays>("SELECT * FROM RESTRICTEDHOLIDAYS WHERE Id = " + Convert.ToString(rh.RHId)).FirstOrDefault();
                        //}
                        //catch (Exception)
                        //{
                        //    throw;
                        //}


                        ////get the emailid of the reporting manager.
                        //var ReportingManagerEmailId = repo.GetEmailIdOfEmployee(ReportingManager);
                        ////get the emailid of the staff who raises the leave application.
                        //var StaffEmailId = repo.GetEmailIdOfEmployee(rh.StaffId);
                        ////get the name of the staff.
                        //var StaffName = repo.GetStaffName(rh.StaffId);
                        ////get the name of the reporting manager.
                        //var ReportingManagerName = repo.GetStaffName(ReportingManager);

                        ////check if the reporting manager has an email id.
                        //if (string.IsNullOrEmpty(ReportingManagerEmailId) == true) //if the reporting manager does not have an email id then...
                        //{
                        //    //check if the staff has an email id.
                        //    if (string.IsNullOrEmpty(StaffEmailId) == true) //if the staff does not have an email id then...
                        //    {
                        //        //do not take any action.
                        //    }
                        //    else //if the staff has an email id then...
                        //    {
                        //        var EmailStr = string.Empty;
                        //        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your restricted holiday application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">RH Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(rh.RestrictedHolidays.RHDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">RH Reason:</td><td style=\"width:80%;\">" + rh.RestrictedHolidays.Name + "</td></tr></table></p><p>But an intimation email could not be sent to your reporting manager because of a missing email id.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //        //send intimation to the staff stating that his/her leave application has been acknowleged 
                        //        //function call to get the name of the staff and the reporting manager.
                        //        //  but the reporting manager does not have a email id so no intimation has been sent to him.
                        //        repo.SendEmailMessage("", StaffEmailId, "", "", "restricted holiday application of " + rh.StaffId + " - " + StaffName, EmailStr);
                        //    }
                        //}
                        //else // if the reporting manager has an email id then...
                        //{
                        //    var EmailStr = string.Empty;
                        //    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a restricted holiday. Restricted holiday details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">RH Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(rh.RestrictedHolidays.RHDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">RH Reason:</td><td style=\"width:80%;\">" + rh.RestrictedHolidays.Name + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this application.</p><p><a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + rh.Id + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + rh.Id + "&Approve=false\">Reject</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //    // send intimation to the reporting manager about the restricted holiday application.
                        //    repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "restricted holiday application of " + StaffName, EmailStr);

                        //    // send acknowledgement to the staff who raised the leave application.
                        //    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your restricted holiday application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">RH Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(rh.RestrictedHolidays.RHDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">RH Reason:</td><td style=\"width:80%;\">" + rh.RestrictedHolidays.Name + "</td></tr></table></p><p>This application has been sent also to your reporting manager.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //    repo.SendEmailMessage("", StaffEmailId, "", "", "restricted holiday application sent to " + ReportingManagerName, EmailStr);
                        //}






                    }
                    else if (_AE_.Reason == "LO")
                    {
                        var LOId = string.Empty;
                       
                        var lo = new LaterOff();

                        lo.Id = _AE_.Id;
                        lo.StaffId = _AE_.StaffId;
                        lo.LaterOffReqDate = Convert.ToDateTime(_AE_.FromDate);
                        lo.LaterOffAvailDate = Convert.ToDateTime(_AE_.ToDate);
                        lo.Reason = _AE_.Remarks;
                        lo.IsCancelled = false;

                        SaveLaterOffDetails(lo);
                        //  ReportingManager = string.Empty;
                        ApplicationId = lo.Id;

                        LOId = _AE_.Id;

                        if (LOId == null)
                        {
                            repo.SaveIntoApplicationApproval(lo.Id, "LO", lo.StaffId, ReportingManager, selfapproval);
                        }

                        //try
                        //{
                        //    //try to get the server ip from the web.config file.
                        //    BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
                        //    //check if the server ip address has been given or not.
                        //    if (string.IsNullOrEmpty(BaseAddress) == true) //if the server ip address has not been given then...
                        //        //throw exception.
                        //        throw new Exception("BaseAddress parameter is blank in web.config file.");
                        //}
                        //catch (Exception)
                        //{
                        //    throw;
                        //}

                        ////get the emailid of the reporting manager.
                        //var ReportingManagerEmailId = repo.GetEmailIdOfEmployee(ReportingManager);
                        ////get the emailid of the staff who raises the leave application.
                        //var StaffEmailId = repo.GetEmailIdOfEmployee(lo.StaffId);
                        ////get the name of the staff.
                        //var StaffName = repo.GetStaffName(lo.StaffId);
                        ////get the name of the reporting manager.
                        //var ReportingManagerName = repo.GetStaffName(ReportingManager);

                        ////check if the reporting manager has an email id.
                        //if (string.IsNullOrEmpty(ReportingManagerEmailId) == true) //if the reporting manager does not have an email id then...
                        //{
                        //    //check if the staff has an email id.
                        //    if (string.IsNullOrEmpty(StaffEmailId) == true) //if the staff does not have an email id then...
                        //    {
                        //        //do not take any action.
                        //    }
                        //    else //if the staff has an email id then...
                        //    {
                        //        var EmailStr = string.Empty;
                        //        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Later Off application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">Req Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(lo.LaterOffReqDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Avail Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(lo.LaterOffAvailDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + lo.Reason + "</td></tr></table></p><p>But an intimation email could not be sent to your reporting manager because of a missing email id.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //        //send intimation to the staff stating that his/her Later Off application has been acknowleged 
                        //        //function call to get the name of the staff and the reporting manager.
                        //        //  but the reporting manager does not have a email id so no intimation has been sent to him.
                        //        repo.SendEmailMessage("", StaffEmailId, "", "", "Later Off application of " + lo.StaffId + " - " + StaffName, EmailStr);
                        //    }
                        //}
                        //else // if the reporting manager has an email id then...
                        //{
                        //    var EmailStr = string.Empty;
                        //    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a Later Off. Later Off details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">Req Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(lo.LaterOffReqDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Avail Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(lo.LaterOffAvailDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + lo.Reason + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this application.</p><p><a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + lo.Id + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + lo.Id + "&Approve=false\">Reject</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //    // send intimation to the reporting manager about the Later Off application.
                        //    repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "Later Off application of " + StaffName, EmailStr);

                        //    // send acknowledgement to the staff who raised the leave application.
                        //    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Later Off application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">Req Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(lo.LaterOffReqDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Avail Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(lo.LaterOffAvailDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + lo.Reason + "</td></tr></table></p><p>This application has been sent also to your reporting manager.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        //    repo.SendEmailMessage("", StaffEmailId, "", "", "Later Off application sent to " + ReportingManagerName, EmailStr);
                        //}





                    }
                    else if (_AE_.Reason == "MO")
                    {
                        var MOId = string.Empty;
                        
                        var mo = new MaintenanceOff();

                        mo.Id = _AE_.Id;
                        mo.StaffId = _AE_.StaffId;
                        mo.DateFrom = Convert.ToDateTime(_AE_.FromDate);
                        mo.DateTo = Convert.ToDateTime(_AE_.ToDate);
                        mo.Reason = _AE_.Remarks;
                        mo.IsCancelled = false;

                        SaveMaintenanceOffDetails(mo);
                        //  ReportingManager = string.Empty;
                        ApplicationId = mo.Id;

                        MOId = _AE_.Id;

                        if (MOId == null)
                        {
                            repo.SaveIntoApplicationApproval(mo.Id, "MO", mo.StaffId, ReportingManager, selfapproval);
                        }
                    }

                    if (_AE_.ApplicationEntryId != null)
                    {
                        AE.Id = Convert.ToInt32(_AE_.ApplicationEntryId);

                    }

                    AE.StaffId = _AE_.StaffId;
                    AE.ApplicationId = ApplicationId;
                    AE.Reason = _AE_.Reason;
                    AE.FromDate = Convert.ToDateTime(_AE_.FromDate);
                    AE.ToDate = Convert.ToDateTime(_AE_.ToDate);
                    AE.Remarks = _AE_.Remarks;
                    AE.CreatedOn = DateTime.Now;
                    AE.CreatedBy = UserFullName;
                    AE.ModifiedOn = DateTime.Now;
                    AE.ModifiedBy = UserFullName;
                    AE.CancelledOn = Convert.ToDateTime("1900-01-01"); ;
                    AE.CancelledBy = "-";
                    if (AE.Reason == "LA")
                    {
                        AE.TotalDays = _AE_.Total;
                    }
                    else if (AE.Reason == "PO")
                    {
                        AE.TotalDays = _AE_.TotalHours;
                    }
                    else if (AE.Reason == "CO")
                    { AE.TotalDays = "-"; }
                    else if (AE.Reason == "MP")
                    {
                        AE.TotalDays = "-";
                        if (_AE_.FromDate != null && _AE_.ToDate != null)
                        {
                            AE.FromDate = Convert.ToDateTime(_AE_.FromDate);
                            AE.ToDate = Convert.ToDateTime(_AE_.ToDate);
                        }
                        else if (_AE_.ToDate == null)
                        {
                            AE.FromDate = Convert.ToDateTime(_AE_.FromDate);
                            AE.ToDate = Convert.ToDateTime("1900-01-01 00:00:00");
                        }
                        else
                        {
                            AE.FromDate = Convert.ToDateTime("1900-01-01 00:00:00");
                            AE.ToDate = Convert.ToDateTime(_AE_.ToDate);
                        }

                    }
                    else if (AE.Reason == "OD")
                    { AE.TotalDays = _AE_.TotalDays; }
                    else if (AE.Reason == "RH")
                    { AE.TotalDays = "-"; }
                    else if (AE.Reason == "LO")
                    { AE.TotalDays = "-"; }
                    else if (AE.Reason == "MO")
                    { AE.TotalDays = "-"; }
                    context.ApplicationEntry.AddOrUpdate(AE);
                    context.SaveChanges();

                    Trans.Commit();
                }
                catch (Exception)
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }



        public void SaveLeaveApplicationDetails(LeaveApplicationWabco law)
        {
            var lastid = string.Empty;
            if (string.IsNullOrEmpty(law.Id) == true)
            {
                var mr = new MasterRepository();
                lastid = mr.getmaxid("LeaveApplicationWabco", "id", "LA", "", 10, ref lastid);
                law.Id = lastid;
            }

            context.LeaveApplicationWabco.AddOrUpdate(law);
            context.SaveChanges();
        }

        public void SavePermissionOffDetails(PermissionOff co)
        {
            var lastid = string.Empty;
            if (string.IsNullOrEmpty(co.Id) == true)
            {
                var mr = new MasterRepository();
                lastid = mr.getmaxid("PermissionOff", "Id", "PO", "", 10, ref lastid);
                co.Id = lastid;
            }

            context.PermissionOff.AddOrUpdate(co);
            context.SaveChanges();
        }

        public void SaveCOffDetails(CompensatoryOff co)
        {
            var lastid = string.Empty;
            if (string.IsNullOrEmpty(co.Id) == true)
            {
                var mr = new MasterRepository();
                lastid = mr.getmaxid("compensatoryoff", "Id", "CO", "", 10, ref lastid);
                co.Id = lastid;
            }

            context.CompensatoryOff.AddOrUpdate(co);
            context.SaveChanges();
        }

        public void SaveManualPunchDetails(ManualPunch mp)
        {
            //var maxid = string.Empty;
            var lastid = string.Empty;
            //var mp = new ManualPunch();
            if (string.IsNullOrEmpty(mp.Id) == true)
            {
                var mr = new MasterRepository();
                lastid = mr.getmaxid("ManualPunch", "id", "MP", "", 10, ref lastid);
                mp.Id = lastid;
            }

            context.ManualPunch.AddOrUpdate(mp);
            context.SaveChanges();
        }

        public void SaveODApplication(ODApplication oda)
        {
            var lastid = string.Empty;
            if (string.IsNullOrEmpty(oda.Id))
            {
                var mr = new MasterRepository();
                lastid = mr.getmaxid("ODApplication", "id", "OD", "", 10, ref lastid);
                oda.Id = lastid;
            }
            //oda.CreatedOn = DateTime.Now;
            //oda.CreatedBy = "-";
            //oda.ModifiedOn = DateTime.Now;
            //oda.ModifiedBy = "-";

            context.ODApplication.AddOrUpdate(oda);
            context.SaveChanges();
        }

        public void SaveRHApplication(RHApplication rh)
        {
            var lastid = string.Empty;

            if (string.IsNullOrEmpty(rh.Id))
            {
                var mr = new MasterRepository();
                lastid = mr.getmaxid("RHApplication", "id", "RH", "", 10, ref lastid);
                rh.Id = lastid;
            }
            context.RHApplication.AddOrUpdate(rh);
            context.SaveChanges();
        }

        public void SaveLaterOffDetails(LaterOff co)
        {
            var lastid = string.Empty;
            if (string.IsNullOrEmpty(co.Id) == true)
            {
                var mr = new MasterRepository();
                lastid = mr.getmaxid("lateroff", "Id", "LO", "", 10, ref lastid);
                co.Id = lastid;
            }

            context.LaterOff.AddOrUpdate(co);
            context.SaveChanges();
        }

        public void SaveMaintenanceOffDetails(MaintenanceOff co)
        {
            var lastid = string.Empty;
            if (string.IsNullOrEmpty(co.Id) == true)
            {
                var mr = new MasterRepository();
                lastid = mr.getmaxid("maintenanceoff", "Id", "MO", "", 10, ref lastid);
                co.Id = lastid;
            }

            context.MaintenanceOff.AddOrUpdate(co);
            context.SaveChanges();
        }

        public List<RHHolidayList> GetRHList(string StaffId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@StaffId", StaffId); 

            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT convert ( varchar , Id ) as RHId , Name as RHName , RHDate FROM fnGetRestrictedHolidayList( @StaffId )");

            try
            {
                var lst = context.Database.SqlQuery<RHHolidayList>(QryStr.ToString(), sqlParameters).Select(d => new RHHolidayList()
                {
                    RHId = d.RHId,
                    RHName = d.RHName,
                    RHDate = d.RHDate
                }).ToList();

                if (lst == null)
                {
                    return new List<RHHolidayList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<RHHolidayList>();
            }
        }

        public List<ApplicationforcancellationList> GetApplication(string staffId, string applicationType, string fromDate, string toDate)
        {

            SqlParameter[] sqlParameters = new SqlParameter[4];
            sqlParameters[0] = new SqlParameter("@StaffId", staffId);
            sqlParameters[1] = new SqlParameter("@fromDate", fromDate);
            sqlParameters[2] = new SqlParameter("@applicationType", applicationType);
            sqlParameters[3] = new SqlParameter("@toDate", toDate);

            var qryStr = new StringBuilder();

            if (applicationType == "LA")
            {
                qryStr.Clear();
                qryStr.Append("select a.Id as ApplicationId,a.StaffId,b.ApprovalStatusId,c.Name as LeaveTypeName,a.RequestApplicationType as ApplicationShortName,REPLACE( CONVERT ( VARCHAR , a.StartDate, 106 ) , ' ' , '-' ) as FromDate," +
                              "REPLACE ( CONVERT ( VARCHAR , a.EndDate, 106 ) , ' ' , '-' )  as ToDate, a.TotalDays from RequestApplication as a join ApplicationApproval as b on b.ParentId = a.Id join LeaveType as c on c.Id = a.LeaveTypeid " +
                              "where a.IsCancelled= 0 and ApprovalStatusId = 2 and ");
                //qryStr.Append("Select B.Id as ApplicationId , B.StaffId ,C.ApprovalStatusId , A.Name as LeaveTypename , 'LA' as ApplicationShortname ,"+
                //" REPLACE ( CONVERT ( VARCHAR , B.LeaveStartDate, 106 ) , ' ' , '-' )  as  FromDate, REPLACE "+
                //" ( CONVERT ( VARCHAR , B.LeaveEndDate, 106 ) , ' ' , '-' )  as ToDate , B.TotalDays from "+
                //" LeaveApplicationWabco B Inner Join LeaveType A On B.LeaveTypeid = A.Id  inner join   "+
                //" ApplicationApproval C on C.ParentId = B.Id where StaffId = '" + staffId + "' and IsCancelled= 0"+
                //" and ApprovalStatusId = 2 and CONVERT ( datetime, LeaveStartDate )   Between "+
                //" convert(datetime,'" + fromDate + "') AND convert(datetime,'" + toDate + "') AND CONVERT "+
                //" ( datetime, LeaveEndDate ) Between convert(datetime,'" + fromDate + "') AND  convert"+
                //" (datetime,'" + toDate + "')");
            }
            else if (applicationType == "PO")
            {
                qryStr.Clear();
                //qryStr.Append("Select A.Id as ApplicationId ,  A.StaffId ,B.ApprovalStatusId, 'PO' as ApplicationShortname , Convert(Varchar,A.TimeFrom,108)"+
                //    " as FromDate,  Convert(Varchar,A.TimeTo,108) as ToDate , Convert(Varchar,A.Totalhours,108) as TotalHours ,"+
                //    " Convert (Varchar,A.PermissionDate,106) as PermissionDate  from PermissionOff A inner join ApplicationApproval"+
                //    " B on B.ParentId = A.Id where StaffId = '" + staffId + "' and  IsCancelled= 0 and ApprovalStatusId = 2 and"+
                //    " CONVERT ( datetime, TimeFrom ) Between  convert(datetime,'" + fromDate + "') AND convert"+
                //    "(datetime,'" + toDate + "') AND CONVERT ( datetime, TimeTo ) Between convert(datetime,'" + fromDate + "')"+
                //    " AND  convert(datetime,'" + toDate + "')");
                qryStr.Append("Select A.Id as ApplicationId , A.StaffId ,B.ApprovalStatusId, a.RequestApplicationType as ApplicationShortName , Convert(Varchar,A.StartDate,108) "+
                               "as FromDate, Convert(Varchar, A.EndDate, 108) as ToDate, Convert(Varchar, A.Totalhours, 108) as TotalHours,"+
                               "Convert(Varchar, A.StartDate, 106) as PermissionDate  from RequestApplication A inner join ApplicationApproval B on B.ParentId = A.Id " +
                               "where a.IsCancelled= 0 and ApprovalStatusId = 2 and ");
            }
            else if (applicationType == "MP")
            {
                qryStr.Clear();
                //qryStr.Append("Select A.Id as ApplicationId ,  A.StaffId ,B.ApprovalStatusId ,'MP' as ApplicationShortname , A.InDateTime as"+
                //    " InDateTime, A.OutDateTime As OutDateTime, A.PunchType from ManualPunch A inner join ApplicationApproval"+
                //    " B on B.ParentId = A.Id where  StaffId  = '" + staffId + "' and IsCancelled= 0 and ApprovalStatusId = 2 "+
                //    " and CONVERT ( datetime, InDateTime )   Between convert(datetime,'" + fromDate + "')  AND convert"+
                //    " (datetime,'" + toDate + "') AND CONVERT ( datetime, OutDateTime ) Between  convert"+
                //    " (datetime,'" + fromDate + "') AND  convert(datetime,'" + toDate + "')");
                qryStr.Append("Select A.Id as ApplicationId ,  A.StaffId ,B.ApprovalStatusId ,a.RequestApplicationType as ApplicationShortName , A.StartDate as "+
                                "InDateTime, A.EndDate As OutDateTime, A.PunchType from RequestApplication A inner join ApplicationApproval B on B.ParentId = A.Id "+
                                "where a.IsCancelled= 0 and ApprovalStatusId = 2 and ");
            }
            else if (applicationType == "CR")
            {
                qryStr.Clear();
                //qryStr.Append("Select A.Id as ApplicationId ,  A.StaffId ,B.ApprovalStatusId , 'CO' as ApplicationShortname , Replace"+
                //    " (convert(Varchar,A.COffReqDate ,106) ,' ' , '-') as COffReqDate , Replace(convert(Varchar,A.COffAvailDate ,106)"+
                //    " ,' ' , '-') as COffAvailDate , A.Reason from CompensatoryOff A inner join ApplicationApproval B on"+
                //    "  B.ParentId = A.Id where StaffId = '" + staffId + "' and  IsCancelled= 0 and ApprovalStatusId = 2 "+
                //    " and CONVERT ( date, COffReqDate ) Between convert(date,'" + fromDate + "') AND  "+
                //    " convert(date,'" + toDate + "')");
                qryStr.Append("Select A.Id as ApplicationId , A.StaffId , B.ApprovalStatusId  ,a.RequestApplicationType as ApplicationShortName ," +
                              "REPLACE(CONVERT(VARCHAR, A.[StartDate], 106), ' ', '-') as COffReqDate,REPLACE(CONVERT(VARCHAR, A.[EndDate], 106), ' ', '-') as CoffAvailDate, A.Remarks as Reason " +
                              "from RequestApplication as A inner join ApplicationApproval B on B.ParentId = A.Id " +
                              " where a.IsCancelled= 0 and ApprovalStatusId = 2 and ");
            }
            else if (applicationType == "OD")
            {
                qryStr.Clear();
                //qryStr.Append("Select A.Id as ApplicationId , A.StaffId , B.ApprovalStatusId , 'OD' as ApplicationShortname ,"+
                //    " A.ODDuration ,  REPLACE ( CONVERT ( VARCHAR , A.[From], 106 ) , ' ' , '-' )  as  FromDate  ,"+
                //    " REPLACE ( CONVERT ( VARCHAR , A.[To], 106 ) , ' ' , '-' )  as  ToDate  , A.ODReason as Reason "+
                //    "from ODApplication A inner join  ApplicationApproval B on B.ParentId = A.Id where StaffId = '" + staffId + "'"+
                //    " and  IsCancelled= 0  and ApprovalStatusId = 2 and CONVERT ( datetime, [From] )   Between "+
                //    "convert(datetime,'" + fromDate + "')  AND convert(datetime,'" + toDate + "') AND CONVERT"+
                //    " ( datetime, [To] ) Between  convert(datetime,'" + fromDate + "') AND  convert(datetime,'" + toDate + "')");

                qryStr.Append("Select A.Id as ApplicationId , A.StaffId , B.ApprovalStatusId  ,a.RequestApplicationType as ApplicationShortName ,"+
                              "A.ODDuration, REPLACE(CONVERT(VARCHAR, A.[StartDate], 106), ' ', '-') as FromDate,REPLACE(CONVERT(VARCHAR, A.[EndDate], 106), ' ', '-') as ToDate, A.Remarks as Reason "+
                              "from RequestApplication as A inner join ApplicationApproval B on B.ParentId = A.Id " +
                              " where a.IsCancelled= 0 and ApprovalStatusId = 2 and ");
            }

            qryStr.Append("a.StaffId=@StaffId and CONVERT(datetime, a.StartDate)  Between convert(datetime, @fromDate) AND convert(datetime, @toDate) AND CONVERT" +
                            "(datetime, a.EndDate) Between convert(datetime, @fromDate) AND  convert (datetime, @toDate) and a.RequestApplicationType=@applicationType");
            try
            {
                var lst = context.Database.SqlQuery<ApplicationforcancellationList>(qryStr.ToString(), sqlParameters).Select(d => new ApplicationforcancellationList()
                {
                    StaffId = d.StaffId,
                    LeaveType = d.LeaveType,
                    LeaveTypename = d.LeaveTypename,
                    ApplicationId = d.ApplicationId,
                    ApplicationShortname = d.ApplicationShortname,
                    FromDate = d.FromDate,
                    ToDate = d.ToDate,
                    TimeFrom = d.TimeFrom,
                    TimeTo = d.TimeTo,
                    InDateTime = d.InDateTime,
                    OutDateTime = d.OutDateTime,
                    PunchType = d.PunchType,
                    PermissionDate = d.PermissionDate,
                    TotalHours = d.TotalHours,
                    TotalDays =d.TotalDays,
                    COffReqDate = d.COffReqDate,
                    COffAvailDate = d.COffAvailDate,
                    ODDuration = d.ODDuration,
                    Reason = d.Reason
                }).ToList();

                if (lst == null)
                {
                    return new List<ApplicationforcancellationList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception e)
            {
                return new List<ApplicationforcancellationList>();
            }
        }

        public List<ApplicationforcancellationList> GetAllApplication(string staffId, string applicationType, string fromDate, string toDate)
        {
            SqlParameter[] sqlParameters = new SqlParameter[4];
            sqlParameters[0] = new SqlParameter("@StaffId", staffId);
            sqlParameters[1] = new SqlParameter("@fromDate", fromDate);
            sqlParameters[2] = new SqlParameter("@applicationType", applicationType);
            sqlParameters[3] = new SqlParameter("@toDate", toDate);
            var qryStr = new StringBuilder();

            if (applicationType == "LA")
            {
                qryStr.Clear();
                qryStr.Append("select a.Id as ApplicationId,a.StaffId,b.ApprovalStatusId,c.Name as LeaveTypeName,a.RequestApplicationType as ApplicationShortName,REPLACE( CONVERT ( VARCHAR , a.StartDate, 106 ) , ' ' , '-' ) as FromDate," +
                              "REPLACE ( CONVERT ( VARCHAR , a.EndDate, 106 ) , ' ' , '-' )  as ToDate, a.TotalDays from RequestApplication as a join ApplicationApproval as b on b.ParentId = a.Id join LeaveType as c on c.Id = a.LeaveTypeid " +
                              "where a.IsCancelled= 0 and ApprovalStatusId = 1 and ");
            }
            else if (applicationType == "PO")
            {
                qryStr.Clear();
                qryStr.Append("Select A.Id as ApplicationId , A.StaffId ,B.ApprovalStatusId, a.RequestApplicationType as ApplicationShortName , Convert(Varchar,A.StartDate,108) " +
                               "as FromDate, Convert(Varchar, A.EndDate, 108) as ToDate, Convert(Varchar, A.Totalhours, 108) as TotalHours," +
                               "Convert(Varchar, A.StartDate, 106) as PermissionDate  from RequestApplication A inner join ApplicationApproval B on B.ParentId = A.Id " +
                               "where a.IsCancelled= 0 and ApprovalStatusId = 1 and ");
            }
            else if (applicationType == "MP")
            {
                qryStr.Clear();
                qryStr.Append("Select A.Id as ApplicationId ,  A.StaffId ,B.ApprovalStatusId ,a.RequestApplicationType as ApplicationShortName , A.StartDate as " +
                                "InDateTime, A.EndDate As OutDateTime, A.PunchType from RequestApplication A inner join ApplicationApproval B on B.ParentId = A.Id " +
                                "where a.IsCancelled= 0 and ApprovalStatusId = 1 and ");
            }
            else if (applicationType == "CR")
            {
                qryStr.Clear();
                qryStr.Append("Select A.Id as ApplicationId , A.StaffId , B.ApprovalStatusId  ,a.RequestApplicationType as ApplicationShortName ," +
                              "REPLACE(CONVERT(VARCHAR, A.[StartDate], 106), ' ', '-') as COffReqDate,REPLACE(CONVERT(VARCHAR, A.[EndDate], 106), ' ', '-') as CoffAvailDate, A.Remarks as Reason " +
                              "from RequestApplication as A inner join ApplicationApproval B on B.ParentId = A.Id " +
                              " where a.IsCancelled= 0 and ApprovalStatusId = 1 and ");
            }
            else if (applicationType == "OD")
            {
                qryStr.Clear();
                qryStr.Append("Select A.Id as ApplicationId , A.StaffId , B.ApprovalStatusId  ,a.RequestApplicationType as ApplicationShortName ," +
                              "A.ODDuration, REPLACE(CONVERT(VARCHAR, A.[StartDate], 106), ' ', '-') as FromDate,REPLACE(CONVERT(VARCHAR, A.[EndDate], 106), ' ', '-') as ToDate, A.Remarks as Reason " +
                              "from RequestApplication as A inner join ApplicationApproval B on B.ParentId = A.Id " +
                              " where a.IsCancelled= 0 and ApprovalStatusId = 1 and ");
            }

            qryStr.Append("a.StaffId=@StaffId and CONVERT(datetime, a.StartDate)  Between convert(datetime, @fromDate) AND convert(datetime, @toDate) AND CONVERT" +
                            "(datetime, a.EndDate) Between convert(datetime, @fromDate) AND  convert (datetime, @toDate) and a.RequestApplicationType=@applicationType");
            try
            {
                var lst = context.Database.SqlQuery<ApplicationforcancellationList>(qryStr.ToString(), sqlParameters).Select(d => new ApplicationforcancellationList()
                {
                    StaffId = d.StaffId,
                    LeaveType = d.LeaveType,
                    LeaveTypename = d.LeaveTypename,
                    ApplicationId = d.ApplicationId,
                    ApplicationShortname = d.ApplicationShortname,
                    FromDate = d.FromDate,
                    ToDate = d.ToDate,
                    TimeFrom = d.TimeFrom,
                    TimeTo = d.TimeTo,
                    InDateTime = d.InDateTime,
                    OutDateTime = d.OutDateTime,
                    PunchType = d.PunchType,
                    PermissionDate = d.PermissionDate,
                    TotalHours = d.TotalHours,
                    TotalDays = d.TotalDays,
                    COffReqDate = d.COffReqDate,
                    COffAvailDate = d.COffAvailDate,
                    ODDuration = d.ODDuration,
                    Reason = d.Reason
                }).ToList();

                if (lst == null)
                {
                    return new List<ApplicationforcancellationList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception e)
            {
                return new List<ApplicationforcancellationList>();
            }
        }
        public void RemovePunchesFromSmax(string id, string StaffId)
        {
           
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@StaffId", StaffId);
            sqlParameters[1] = new SqlParameter("@Id", id);
            string staffid = string.Empty;
            DateTime fromDate = DateTime.Now;
            DateTime Indatetime = DateTime.Now;
            DateTime Outdatetime = DateTime.Now;
            string punchtype = string.Empty;
          
            // Get the Date value from Manual Punch Table fro deleting the Punch
            var QryStr1 = new StringBuilder();
            QryStr1.Clear();
            QryStr1.Append("Select staffid,StartDate as Indatetime,EndDate as OutDateTime,PunchType from  [RequestApplication] where Id = @Id AND StaffId = @StaffId ");

            try
            {
               var data = context.Database.SqlQuery<Manualpunchforsmax>(QryStr1.ToString(), sqlParameters).Select(d => new Manualpunchforsmax()
                {
                    StaffId = d.StaffId,
                    Indatetime=d.Indatetime,
                    Outdatetime=d.Outdatetime,
                    PunchType=d.PunchType
                }).FirstOrDefault();
               staffid = data.StaffId;
               Indatetime = data.Indatetime;
               Outdatetime = data.Outdatetime;
               punchtype = data.PunchType;
            }
            catch (Exception)
            {
                throw;
            }
            
            if (punchtype == "Out")
            {
                string dateForDelete = string.Empty;
                var day = Outdatetime.Day;
                var monthName = Outdatetime.ToString("MMM");
                var year = Outdatetime.Year;
                dateForDelete = day + "-" + monthName + "-" + year;
                
                var QryStr2 = new StringBuilder();
                QryStr2.Clear();
                QryStr2.Append(" Delete from [SmaxTransaction]  where Tr_ChId = '" + StaffId + "' AND  Convert(Date,Tr_Date)= Convert(Date,'" + dateForDelete + "') AND Tr_IpAddress = '192.168.0.223'  AND Tr_OpName = 'OUT'");
                context.Database.ExecuteSqlCommand(QryStr2.ToString());
            }
            else if (punchtype == "In")
            {
                string dateForDelete = string.Empty;
                var day = Indatetime.Day;
                var monthName = Indatetime.ToString("MMM");
                var year = Indatetime.Year;
                dateForDelete = day + "-" + monthName + "-" + year;

                var QryStr3 = new StringBuilder();
                QryStr3.Clear();
                QryStr3.Append(" Delete from [SmaxTransaction]  where Tr_ChId = '" + StaffId + "' AND  Convert(Date,Tr_Date)= Convert(Date,'" + dateForDelete + "') AND Tr_IpAddress = '192.168.0.223' AND Tr_OpName = 'IN'");
                context.Database.ExecuteSqlCommand(QryStr3.ToString());
            }
            else if (punchtype == "InOut")
            {
                if (Outdatetime.Date > Indatetime.Date)
                {
                    string inDateForDelete = string.Empty;
                    var day = Indatetime.Day;
                    var monthName = Indatetime.ToString("MMM");
                    var year = Indatetime.Year;
                    inDateForDelete = day + "-" + monthName + "-" + year;

                    string outDateForDelete = string.Empty;
                    var day1 = Outdatetime.Day;
                    var monthName1 = Outdatetime.ToString("MMM");
                    var year1 = Outdatetime.Year;
                    outDateForDelete = day1 + "-" + monthName1 + "-" + year1;

                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append(" Delete from [SmaxTransaction]  where Tr_ChId = '" + StaffId + "' AND  Convert(Date,Tr_Date)= Convert(Date,'" + inDateForDelete + "') AND Tr_IpAddress = '192.168.0.223'");
                    context.Database.ExecuteSqlCommand(QryStr.ToString());

                    var QryStr2 = new StringBuilder();
                    QryStr2.Clear();
                    QryStr2.Append(" Delete from [SmaxTransaction]  where Tr_ChId = '" + StaffId + "' AND  Convert(Date,Tr_Date)= Convert(Date,'" + outDateForDelete + "') AND Tr_IpAddress = '192.168.0.223'");
                    context.Database.ExecuteSqlCommand(QryStr2.ToString());
                }
                else
                {
                    string dateForDelete = string.Empty;
                    var day = Indatetime.Day;
                    var monthName = Indatetime.ToString("MMM");
                    var year = Indatetime.Year;
                    dateForDelete = day + "-" + monthName + "-" + year;
                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append(" Delete from [SmaxTransaction]  where Tr_ChId = '" + StaffId + "' AND  Convert(Date,Tr_Date)= Convert(Date,'" + dateForDelete + "') AND Tr_IpAddress = '192.168.0.223'");
                     context.Database.ExecuteSqlCommand(QryStr.ToString());
                }
         
        }
        }

        public string Cancel(string ApplicationId, string ApplicationShortname)
        {
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@ApplicationId", ApplicationId);
            
            string staffId = string.Empty;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            string applicationId = string.Empty;
            string applicationType = string.Empty;
            var queryString = new StringBuilder();
            CommonRepository obj = new CommonRepository();
            queryString.Clear();
            try
            {
                if (ApplicationShortname == "LA")
                {
                    var bl = new CommonRepository();
                    bl.LeaveBalanceHandler(ApplicationId, "CANCEL");

                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("Update LeaveApplicationWabco Set IsCancelled=1 where id =@ApplicationId");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), sqlParameters);
                    //CancelLeaveEmployeeLeaveAccount(id, Type, StaffId, TotalDays);

                   
                  }
                else if (ApplicationShortname == "PO")
                {
                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("Update PermissionOff Set IsCancelled=1 where id =@ApplicationId");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), sqlParameters);
                }

                else if (ApplicationShortname == "CO")
                {
                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("Update CompensatoryOff Set IsCancelled=1 where id =@ApplicationId");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), sqlParameters);
                }
                else if (ApplicationShortname == "MP")
                {
                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("Update ManualPunch Set IsCancelled=1 where id =@ApplicationId");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), sqlParameters);

                    QryStr.Clear();
                    QryStr.Append("select staffid  from ManualPunch where id =@ApplicationId");
                   var rtnstaffid= context.Database.SqlQuery<string>(QryStr.ToString(), sqlParameters).FirstOrDefault();
                   RemovePunchesFromSmax(ApplicationId, rtnstaffid);

                }
                else if (ApplicationShortname == "OD")
                {
                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("Update ODApplication Set IsCancelled=1 where id=@ApplicationId");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), sqlParameters);
                }
                else if (ApplicationShortname == "RH")
                {
                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("Update RHApplication Set IsCancelled=1 where id =@ApplicationId");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), sqlParameters);
                }
                else if (ApplicationShortname == "LO")
                {
                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("Update LaterOff Set IsCancelled=1 where id =@ApplicationId");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), sqlParameters);
                }
                else if (ApplicationShortname == "MO")
                {
                    var QryStr = new StringBuilder();
                    QryStr.Clear();
                    QryStr.Append("Update MaintenanceOff Set IsCancelled=1 where id =@ApplicationId");
                    context.Database.ExecuteSqlCommand(QryStr.ToString(), sqlParameters);
                }
                if (ApplicationId.StartsWith("LA"))
                {

                    queryString.Append(" Select StaffId ,LeaveStartDate as FromDate , LeaveEndDate as ToDate from  [LeaveApplicationWabco] where Id = @ApplicationId ");
                    try
                    {

                        var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), sqlParameters).Select(d => new ACTList()
                        {
                            StaffId = d.StaffId,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate
                        }).FirstOrDefault();

                        staffId = data.StaffId;
                        fromDate = data.FromDate;
                        toDate = data.ToDate;
                        applicationType = "LA";
                        applicationId = ApplicationId;
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                    if (fromDate < currentDate)
                    {
                        if (toDate >= currentDate)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        obj.LogIntoIntoAttendanceControlTable(staffId, fromDate, toDate, applicationType, ApplicationId);
                    }

                }

                else if (ApplicationId.StartsWith("MP"))
                {

                    queryString.Append(" Select StaffId , InDateTime as FromDate , OutDateTime as ToDate from ManualPunch where Id = @ApplicationId ");
                    try
                    {

                        var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), sqlParameters).Select(d => new ACTList()
                        {
                            StaffId = d.StaffId,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate
                        }).FirstOrDefault();

                        staffId = data.StaffId;
                        fromDate = data.FromDate;
                        toDate = data.ToDate;
                        applicationType = "MP";
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                    if (fromDate < currentDate)
                    {
                        if (toDate >= currentDate)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        obj.LogIntoIntoAttendanceControlTable(staffId, fromDate, toDate, applicationType, ApplicationId);
                    }
                }
                else if (ApplicationId.StartsWith("PO"))
                {

                    queryString.Append(" Select StaffId , PermissionDate as FromDate , PermissionDate as ToDate from PermissionOff where Id  = @ApplicationId");
                    try
                    {

                        var data = context.Database.SqlQuery<ACTList>(queryString.ToString(),sqlParameters).Select(d => new ACTList()
                        {
                            StaffId = d.StaffId,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate
                        }).FirstOrDefault();

                        staffId = data.StaffId;
                        fromDate = data.FromDate;
                        toDate = data.ToDate;
                        applicationType = "PO";
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                    if (fromDate < currentDate)
                    {
                        if (toDate >= currentDate)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        obj.LogIntoIntoAttendanceControlTable(staffId, fromDate, toDate, applicationType, ApplicationId);
                    }


                }
                else if (ApplicationId.StartsWith("CO"))
                {

                    queryString.Append(" Select StaffId , COffAvailDate as FromDate , COffAvailDate as ToDate from [COMPENSATORYOFF] where Id = @ApplicationId ");
                    try
                    {

                        var data = context.Database.SqlQuery<ACTList>(queryString.ToString(),sqlParameters).Select(d => new ACTList()
                        {
                            StaffId = d.StaffId,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate
                        }).FirstOrDefault();

                        staffId = data.StaffId;
                        fromDate = data.FromDate;
                        toDate = data.ToDate;
                        applicationType = "CO";
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                    if (fromDate < currentDate)
                    {
                        if (toDate >= currentDate)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        obj.LogIntoIntoAttendanceControlTable(staffId, fromDate, toDate, applicationType, ApplicationId);
                    }


                }

                else if (ApplicationId.StartsWith("OD"))
                {

                 
                    queryString.Append(" Select StaffId , [From] as FromDate , [To] as ToDate from [ODApplication] where Id = @ApplicationId ");

                    try
                    {

                        var data = context.Database.SqlQuery<ACTList>(queryString.ToString(),sqlParameters).Select(d => new ACTList()
                        {
                            StaffId = d.StaffId,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate
                        }).FirstOrDefault();

                        staffId = data.StaffId;
                        fromDate = data.FromDate;
                        toDate = data.ToDate;
                        applicationType = "OD";
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                    if (fromDate < currentDate)
                    {
                        if (toDate >= currentDate)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        obj.LogIntoIntoAttendanceControlTable(staffId, fromDate, toDate, applicationType, ApplicationId);
                    }

                }
                else if (ApplicationId.StartsWith("RH"))
                {

                    queryString.Append(" Select StaffId , ApplicationDate as FromDate , ApplicationDate as ToDate from [RHApplication] where Id = @ApplicationId ");

                    try
                    {

                        var data = context.Database.SqlQuery<ACTList>(queryString.ToString(),sqlParameters).Select(d => new ACTList()
                        {
                            StaffId = d.StaffId,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate
                        }).FirstOrDefault();

                        staffId = data.StaffId;
                        fromDate = data.FromDate;
                        toDate = data.ToDate;
                        applicationType = "RH";
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                    if (fromDate < currentDate)
                    {
                        if (toDate >= currentDate)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        obj.LogIntoIntoAttendanceControlTable(staffId, fromDate, toDate, applicationType, ApplicationId);
                    }


                }
                else if (ApplicationId.StartsWith("OT"))
                {
                  
                    queryString.Append(" Select StaffId , OTDate as FromDate , OTDate as ToDate from [OTApplication] where Id = @ApplicationId ");

                    try
                    {

                        var data = context.Database.SqlQuery<ACTList>(queryString.ToString(),sqlParameters).Select(d => new ACTList()
                        {
                            StaffId = d.StaffId,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate
                        }).FirstOrDefault();

                        staffId = data.StaffId;
                        fromDate = data.FromDate;
                        toDate = data.ToDate;
                        applicationType = "OT";
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                    if (fromDate < currentDate)
                    {
                        if (toDate >= currentDate)
                        {
                            toDate = DateTime.Now.AddDays(-1);
                        }
                        obj.LogIntoIntoAttendanceControlTable(staffId, fromDate, toDate, applicationType, ApplicationId);
                    }

                }

                return "OK";
            }
            catch (Exception)
            {
                return "NOT SAVED";
            }
        }

        public string CancelApplication(string Id)
        {
            string staffId = string.Empty;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            string applicationId = string.Empty;
            string applicationType = string.Empty;
            string punchtype = string.Empty;
            //Get the leave application details based on the Id passed to this function as a parameter.
            RALeaveApplicationRepository repo = new RALeaveApplicationRepository();
            ClassesToSave CTS = new ClassesToSave();
            var Obj = repo.GetRequestApplicationDetails(Id);
            //
            //Check whether the starting date of the leave application is below the current date.
            var IsFutureDate = IsFromDateMoreOrEqualToCurrerntDate(Obj.StartDate, DateTime.Now);
            //
            //If the leave application date is future to the current date.
            if (IsFutureDate == true)
            {
                if (Obj.IsApproved.Equals(true))//If the leave application has been approved then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                    {
                        //Cancel the leave application which is in approved state.
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;

                        //Credit back the leave balance that was deducted.
                        EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
                        ELA.StaffId = Obj.StaffId;
                        ELA.LeaveTypeId = Obj.LeaveTypeId;
                        ELA.TransactionFlag = 1;
                        ELA.TransactionDate = DateTime.Now;
                        ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays);
                        ELA.Narration = "Cancelled the approved leave application.";
                        ELA.RefId = Obj.Id;
                        ELA.LeaveCreditDebitReasonId = 23;
                        //
                        CTS.RA = Obj;
                        CTS.ELA = ELA;
                        repo.CancelApplication(CTS);
                        
                    }
                    else //If the leave application has already been cancelled then...
                    {
                        //throw exception.
                        throw new Exception("You cannot cancel a leave request that is already been cancelled.");
                    }
                }
            }
            else  //If the leave application is a past date then...
            {
                if (Obj.IsApproved.Equals(true)) //If the leave application has been approved then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))//If the leave application has not been cancelled then...
                    {
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;

                        //Credit back the leave balance that was deducted.
                        EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
                        ELA.StaffId = Obj.StaffId;
                        ELA.LeaveTypeId = Obj.LeaveTypeId;
                        ELA.TransactionFlag = 1;
                        ELA.TransactionDate = DateTime.Now;
                        ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays);
                        ELA.Narration = "Cancelled the approved leave application.";
                        ELA.RefId = Obj.Id;
                        ELA.LeaveCreditDebitReasonId = 23;
                        //
                        CTS.RA = Obj;
                        CTS.ELA = ELA;
                        repo.CancelApplication(CTS);
                        var queryString = new StringBuilder();
                        CommonRepository obj = new CommonRepository();
                        queryString.Clear();
                        SqlParameter[] sqlParameter = new SqlParameter[1];
                        sqlParameter[0] = new SqlParameter("@Id", Obj.Id);

                        queryString.Append(" Select StaffId ,StartDate as FromDate , EndDate as ToDate from  RequestApplication where Id = @Id ");
                        try
                        {

                            var data = context.Database.SqlQuery<ACTList>(queryString.ToString(), sqlParameter).Select(d => new ACTList()
                            {
                                StaffId = d.StaffId,
                                FromDate = d.FromDate,
                                ToDate = d.ToDate
                            }).FirstOrDefault();

                            staffId = data.StaffId;
                            fromDate = data.FromDate;
                            toDate = data.ToDate;
                            applicationType = Obj.RequestApplicationType;
                            applicationId = Obj.Id;
                        }
                        catch (Exception err)
                        {
                            throw err;
                        }
                        if (fromDate < currentDate)
                        {
                            if (toDate >= currentDate)
                            {
                                toDate = DateTime.Now.AddDays(-1);
                            }
                            obj.LogIntoIntoAttendanceControlTable(staffId, fromDate, toDate, applicationType, applicationId);
                        }
                    }
                    else  //If the leave application has already been cancelled then...
                    {
                        //throw exception.
                        throw new Exception("You cannot cancel a leave request that is already been cancelled.");
                    }
                }
            }

            return "OK";
        }

        private bool IsFromDateMoreOrEqualToCurrerntDate(DateTime? LeaveStartDate, DateTime? CurrentDate)
        {
            //TimeSpan TS1 = new TimeSpan();
            //TS1 = LeaveStartDate;


            if (LeaveStartDate.Value.Date < CurrentDate.Value.Date)
            {
                return false;
            }
            else if (LeaveStartDate.Value.Date >= CurrentDate.Value.Date)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
