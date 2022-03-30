using Attendance.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class RequestApplicationRepository : IDisposable
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
        public RequestApplicationRepository()
        {
            context = new AttendanceManagementContext();
        }
        public string GetUniqueId()
        {
            string uniqueId = string.Empty;
            try
            {
                uniqueId = context.Database.SqlQuery<string>("select convert(varchar,getdate(),112) + " +
           "replace(convert(varchar,getdate(),114),':','')").First();
            }
            catch
            {
                throw;
            }
            return uniqueId;
        }
        public RequestApplication GetRequestApplicationDetails(string Id)
        {
            return context.RequestApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }
        public ApplicationApproval GetApplicationApproval(string parentId, string parentType)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(parentId) && d.ParentType.Equals(parentType)).FirstOrDefault();
        }
        public List<HolidayWorkingListItem> GetAppliedHolidayWorkingList(string StaffId)
        {
            try
            {
                string Qry = string.Empty;
                Qry = @"select top 30 ApplicationDate,Id,StaffId,Name,Department,TransactionDate,InTime,OutTime,Remarks,IsCancelled,
                ApproverStatus1,ApproverStatus2 from vwHolidayWorkingList as v where StaffId=@StaffId
                order by ApplicationDate desc ";
                SqlParameter[] sqlParameters = { new SqlParameter("@StaffId", StaffId) };
                return context.Database.SqlQuery<HolidayWorkingListItem>(Qry, sqlParameters).ToList();

            }
            catch (Exception err)
            {
                throw err;
            }
        }
        public List<HolidayWorkingPendingApprovalListItem> GetPendingHolidayWorkingApprovals(string ReportingManagerId)
        {
            try
            {
                string Qry = string.Empty;
                Qry = @"Select 
                    HolidayWorkingId,StaffId,Name,TransactionDate,InTime,OutTime,HolidayWorkingReason
                    from [HolidayWorkingApproval] Where ApprovalOwner = @ReportingManagerId and ApprovalStatusId = 1
                    Union All 
                    Select 
                    HolidayWorkingId,StaffId,Name,TransactionDate,InTime,OutTime,HolidayWorkingReason
                    from [HolidayWorkingApproval] 
                    Where  Approval2Owner = @ReportingManagerId and Approval2Owner <> ApprovalOwner and Approval2statusId = 1 ";


                SqlParameter[] sqlParameters = { new SqlParameter("@ReportingManagerId", ReportingManagerId) };
                return context.Database.SqlQuery<HolidayWorkingPendingApprovalListItem>(Qry, sqlParameters).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SaveHolidayWorkingDetails(ClassesToSave classesToSave, bool isFinalLevelApproval)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            CommonRepository commonRepository = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    // save to request application table.
                    context.RequestApplication.Add(classesToSave.RA);
                    // save to application approval table.
                    context.ApplicationApproval.Add(classesToSave.AA);

                    if (isFinalLevelApproval == true)
                    {
                        fromDate = Convert.ToDateTime(classesToSave.RA.StartDate);
                        toDate = Convert.ToDateTime(classesToSave.RA.EndDate);
                        if (fromDate.Date < currentDate.Date)
                        {
                            if (toDate.Date >= currentDate.Date)
                            {
                                toDate = DateTime.Now.AddDays(-1);
                            }
                            commonRepository.LogIntoIntoAttendanceControlTable(classesToSave.RA.StaffId, fromDate.Date, toDate.Date, classesToSave.RA.RequestApplicationType, classesToSave.AA.Id);
                        }
                    }
                    context.SaveChanges();
                    Trans.Commit();
                    // save to email send log table.
                    if (classesToSave.ESL != null)
                    {
                        foreach (var l in classesToSave.ESL)
                            if (l.To != "-" && l.To != "")
                                commonRepository.SendEmailMessage(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody);
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException err)
                {
                    Trans.Rollback();
                    throw err;
                }
            }
        }

        public void ApproveHolidayWorking(ClassesToSave classesToSave, bool isFinalLevelApproval)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            CommonRepository commonRepository = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    context.Entry(classesToSave.RA).Property("IsApproved").IsModified = true;
                    context.Entry(classesToSave.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(classesToSave.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(classesToSave.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(classesToSave.AA).Property("Comment").IsModified = true;
                    context.Entry(classesToSave.AA).Property("Approval2statusId").IsModified = true;
                    context.Entry(classesToSave.AA).Property("Approval2By").IsModified = true;
                    context.Entry(classesToSave.AA).Property("Approval2On").IsModified = true;

                    if (isFinalLevelApproval == true)
                    {
                        //Insert into attendance control table
                        fromDate = Convert.ToDateTime(classesToSave.RA.StartDate);
                        toDate = Convert.ToDateTime(classesToSave.RA.EndDate);
                        if (fromDate.Date < currentDate.Date)
                        {
                            if (toDate.Date >= currentDate.Date)
                            {
                                toDate = DateTime.Now.AddDays(-1);
                            }
                            commonRepository.LogIntoIntoAttendanceControlTable(classesToSave.RA.StaffId, fromDate.Date, toDate.Date, classesToSave.RA.RequestApplicationType, classesToSave.AA.Id);
                        }
                    }
                    context.SaveChanges();
                    Trans.Commit();
                    if (classesToSave.ESL != null)
                    {
                        foreach (var l in classesToSave.ESL)
                            if (l.To != "-" && l.To != "" && l.To != null)
                                commonRepository.SendEmailMessage(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody);
                    }
                }
                catch (Exception e)
                {
                    Trans.Rollback();
                    throw e;
                }
            }
        }

        public void RejectHolidayWorkingApplication(ClassesToSave classesToSave)
        {
            CommonRepository commonRepository = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    context.Entry(classesToSave.RA).Property("IsRejected").IsModified = true;

                    //Update the application approval table.
                    context.Entry(classesToSave.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(classesToSave.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(classesToSave.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(classesToSave.AA).Property("Approval2statusId").IsModified = true;
                    context.Entry(classesToSave.AA).Property("Approval2By").IsModified = true;
                    context.Entry(classesToSave.AA).Property("Approval2On").IsModified = true;
                    context.Entry(classesToSave.AA).Property("Comment").IsModified = true;
                    context.SaveChanges();
                    Trans.Commit();
                    foreach (var l in classesToSave.ESL)
                        if (l.To != "-")
                            commonRepository.SendEmailMessage(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody);
                }
                catch (Exception e)
                {
                    Trans.Rollback();
                    throw e;
                }
            }
        }
        public void CancelHolidayWorkingApplication(ClassesToSave CTS, string user)
        {
            CommonRepository commonRepository = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    context.Entry(CTS.RA).Property("IsCancelled").IsModified = true;
                    context.Entry(CTS.RA).Property("CancelledDate").IsModified = true;
                    context.Entry(CTS.RA).Property("CancelledBy").IsModified = true;

                    if (CTS.RA.IsApproved == true)
                    {
                        DateTime fromDate = DateTime.Now;
                        DateTime toDate = DateTime.Now;
                        DateTime currentDate = DateTime.Now;
                        fromDate = Convert.ToDateTime(CTS.RA.StartDate);
                        toDate = Convert.ToDateTime(CTS.RA.EndDate);
                        if (fromDate.Date < currentDate.Date)
                        {
                            if (toDate.Date >= currentDate.Date)
                            {
                                toDate = DateTime.Now.AddDays(-1);
                            }
                            commonRepository.LogIntoIntoAttendanceControlTable(CTS.RA.StaffId, fromDate.Date, toDate.Date, CTS.RA.RequestApplicationType, CTS.RA.Id);
                        }
                    }
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                            if (l.To != "-" && l.To != "")
                                commonRepository.SendEmailMessage(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody);
                    }
                    //save the changes.
                    context.SaveChanges();
                    Trans.Commit();
                }
                catch
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }
        public void SaveShiftExtensionDetails(ClassesToSave classesToSave, bool isFinalLevelApproval)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            CommonRepository commonRepository = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    // save to request application table.
                    context.RequestApplication.Add(classesToSave.RA);
                    // save to application approval table.
                    context.ApplicationApproval.Add(classesToSave.AA);

                    if (isFinalLevelApproval == true)
                    {
                        if (classesToSave.RA.StartDate != null && classesToSave.RA.StartDate != null)
                        {
                            fromDate = Convert.ToDateTime(classesToSave.RA.StartDate);
                            toDate = Convert.ToDateTime(classesToSave.RA.EndDate);
                            if (fromDate.Date < currentDate.Date)
                            {
                                if (toDate.Date >= currentDate.Date)
                                {
                                    toDate = DateTime.Now.AddDays(-1);
                                }
                                commonRepository.LogIntoIntoAttendanceControlTable(classesToSave.RA.StaffId, fromDate.Date, toDate.Date, classesToSave.RA.RequestApplicationType, classesToSave.AA.Id);
                            }
                        }
                    }
                    context.SaveChanges();
                    Trans.Commit();
                    // save to email send log table.
                    if (classesToSave.ESL != null)
                    {
                        foreach (var l in classesToSave.ESL)
                            if (l.To != "-" && l.To != "")
                                commonRepository.SendEmailMessage(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody);
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException err)
                {
                    Trans.Rollback();
                    throw err;
                }
            }
        }

        public void ApproveShiftExtensionApplication(ClassesToSave CTS, bool isFinalLevelApproval)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            CommonRepository commonRepository = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    context.Entry(CTS.RA).Property("IsApproved").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2statusId").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2By").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2On").IsModified = true;

                    if (isFinalLevelApproval == true)
                    {
                        //Insert into attendance control table
                        fromDate = Convert.ToDateTime(CTS.RA.StartDate);
                        toDate = Convert.ToDateTime(CTS.RA.EndDate);
                        if (fromDate.Date < currentDate.Date)
                        {
                            if (toDate.Date >= currentDate.Date)
                            {
                                toDate = DateTime.Now.AddDays(-1);
                            }
                            commonRepository.LogIntoIntoAttendanceControlTable(CTS.RA.StaffId, fromDate.Date, toDate.Date, CTS.RA.RequestApplicationType, CTS.AA.Id);
                        }
                    }
                    context.SaveChanges();
                    Trans.Commit();
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                            if (l.To != "-" && l.To != "")
                                commonRepository.SendEmailMessage(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody);
                    }
                }
                catch (Exception e)
                {
                    Trans.Rollback();
                    throw e;
                }
            }
        }

        public void RejectShiftExtensionApplication(ClassesToSave CTS)
        {
            CommonRepository commonRepository = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    context.Entry(CTS.RA).Property("IsRejected").IsModified = true;

                    //Update the application approval table.
                    context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2statusId").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2By").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2On").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;
                    //
                    context.SaveChanges();
                    Trans.Commit();
                    foreach (var l in CTS.ESL)
                        if (l.To != "-")
                            commonRepository.SendEmailMessage(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody);
                }
                catch (Exception e)
                {
                    Trans.Rollback();
                    throw e;
                }
            }
        }
        public List<ShiftExtenstionListItem> GetAppliedShiftExtenstionList(string StaffId)
        {
            try
            {
                string Qry = string.Empty;
                Qry = @"select top 30 Id,ApplicationDate,StaffId,StaffName,Department,TxnDate,DurationOfHoursExtension,
                    HoursBeforeShift,HoursAfterShift,IsCancelled,CancelledBy,CancelledDate,Remarks,ApprovalOwner
                    ,ApprovalStatus1,Approval2Owner,ApprovalStatus2 
                    from  [ShiftExtensionHistory] as v where StaffId = @StaffId
                    order by ApplicationDate desc";
                return context.Database.SqlQuery<ShiftExtenstionListItem>(Qry, new SqlParameter("@StaffId", StaffId)).ToList();

            }
            catch (Exception err)
            {
                throw err;
            }
        }
        public List<ShiftExtenstionListItem> GetAllShiftExtensionList(string StaffId)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Append(" select top 25  Id,ApplicationDate,StaffId,StaffName,Department,TxnDate,DurationOfHoursExtension," +
                     "HoursBeforeShift, HoursAfterShift, IsCancelled, CancelledBy, CancelledDate, Remarks, ApprovalOwner," +
                     " ApprovalStatus1, Approval2Owner, ApprovalStatus2 from [ShiftExtensionHistory] as v where " +
                     "StaffId = @StaffId order by ApplicationDate desc ");
                var Obj = context.Database.SqlQuery<ShiftExtenstionListItem>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).ToList();
                return Obj;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ShiftExtenstionListItem> GetAppliedShiftExtensionForMyTeam(string StaffId, string AppliedId, string userRole)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                if (userRole == "3" || userRole == "5")
                {
                    Str.Append("select top 25  Id,ApplicationDate,StaffId,StaffName,Department,TxnDate,DurationOfHoursExtension," +
                         "HoursBeforeShift, HoursAfterShift, IsCancelled, CancelledBy, CancelledDate, Remarks, ApprovalOwner," +
                         " ApprovalStatus1, Approval2Owner, ApprovalStatus2 from[ShiftExtensionHistory] as v where staffid=@StaffId" +
                        " and (AppliedBy = @AppliedId or ApprovalOwner = @AppliedId or Approval2Owner = @AppliedId) Order by ApplicationDate Desc ");
                    var Obj = context.Database.SqlQuery<ShiftExtenstionListItem>(Str.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@AppliedId", AppliedId)).ToList();
                    return Obj;
                }
                else
                {
                    Str.Append("select top 25  Id,ApplicationDate,StaffId,StaffName,Department,TxnDate,DurationOfHoursExtension," +
                         "HoursBeforeShift, HoursAfterShift, IsCancelled, CancelledBy, CancelledDate, Remarks, ApprovalOwner," +
                         " ApprovalStatus1, Approval2Owner, ApprovalStatus2 from[ShiftExtensionHistory] as v where staffid=@StaffId " +
                        "and approval1owner=@AppliedId");
                    var Obj = context.Database.SqlQuery<ShiftExtenstionListItem>(Str.ToString(), new SqlParameter("@StaffId", StaffId),
                    new SqlParameter("@AppliedId", AppliedId)).ToList();
                    return Obj;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void CancelShiftExtensionApplication(ClassesToSave CTS, string user)
        {
            CommonRepository commonRepository = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    context.Entry(CTS.RA).Property("IsCancelled").IsModified = true;
                    context.Entry(CTS.RA).Property("CancelledDate").IsModified = true;
                    context.Entry(CTS.RA).Property("CancelledBy").IsModified = true;

                    if (CTS.RA.IsApproved == true)
                    {
                        DateTime fromDate = DateTime.Now;
                        DateTime toDate = DateTime.Now;
                        DateTime currentDate = DateTime.Now;
                        fromDate = Convert.ToDateTime(CTS.RA.StartDate);
                        toDate = Convert.ToDateTime(CTS.RA.EndDate);
                        if (fromDate.Date < currentDate.Date)
                        {
                            if (toDate.Date >= currentDate.Date)
                            {
                                toDate = DateTime.Now.AddDays(-1);
                            }
                            commonRepository.LogIntoIntoAttendanceControlTable(CTS.RA.StaffId, fromDate.Date, toDate.Date, CTS.RA.RequestApplicationType, CTS.RA.Id);
                        }
                    }
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                            if (l.To != "-" && l.To != "")
                                commonRepository.SendEmailMessage(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody);
                    }
                    //save the changes.
                    context.SaveChanges();
                    Trans.Commit();
                }
                catch
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }
        public List<ShiftExtensionPendingApprovalListItem> GetPendingShiftExtensionApprovals(string ReportingManagerId)
        {
            try
            {
                string Qry = string.Empty;
                Qry = @"Select 
                ShiftExtensionId,StaffId,FirstName,ExtensionDate,DurationOfHoursExtension
                ,HoursBeforeShift,HoursAfterShift,ShiftExtensionReason
                from [ShiftExtensionApproval] Where ApprovalOwner = @ReportingManagerId and ApprovalStatusId = 1
                Union All 
                Select 
                ShiftExtensionId,StaffId,FirstName,ExtensionDate,DurationOfHoursExtension
                ,HoursBeforeShift,HoursAfterShift,ShiftExtensionReason
                from [ShiftExtensionApproval] 
                Where  Approval2Owner = @ReportingManagerId and Approval2Owner <> ApprovalOwner and Approval2statusId = 1  ";


                SqlParameter[] sqlParameters = { new SqlParameter("@ReportingManagerId", ReportingManagerId) };
                return context.Database.SqlQuery<ShiftExtensionPendingApprovalListItem>(Qry, sqlParameters).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SaveShiftChangeInformation(ClassesToSave classesToSave, bool isFinalLevelApproval)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            DateTime defaultToDate = DateTime.Now.AddDays(-1);
            DateTime expectedWorkingHours = Convert.ToDateTime("1900-01-01 00:00:00.000");
            CommonRepository commonRepository = new CommonRepository();
            var dates = new List<DateTime>();
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    // save to request application table.
                    context.RequestApplication.Add(classesToSave.RA);
                    // save to application approval table.
                    context.ApplicationApproval.Add(classesToSave.AA);

                    if (isFinalLevelApproval == true)
                    {
                        string ShiftName = string.Empty;
                        string ShiftShortName = string.Empty;
                        DateTime StartTime = DateTime.Now;
                        DateTime EndTime = DateTime.Now;
                        TimeSpan ExpectedWorkingHours;
                        Int64 Id = 0;
                        int IsWeeklyOff = 0;

                        try
                        {
                            if (classesToSave.RA.NewShiftId == "LV0011")
                            {
                                ShiftName = "WO";
                                ShiftShortName = "WO";
                                StartTime = Convert.ToDateTime("1900-01-01 00:00:00.000");
                                EndTime = Convert.ToDateTime("1900-01-01 00:00:00.000");
                                ExpectedWorkingHours = EndTime - StartTime;
                                IsWeeklyOff = 1;
                            }
                            else
                            {
                                var lst = new List<ShiftDetailsForShiftChange>();

                                lst = commonRepository.GetShiftDetailsForShiftChange(classesToSave.RA.NewShiftId).ToList();

                                if (lst != null)
                                {
                                    foreach (var rec in lst)
                                    {
                                        ShiftName = rec.Name;
                                        ShiftShortName = rec.ShortName;
                                        StartTime = rec.StartTime;
                                        EndTime = rec.EndTime;
                                        IsWeeklyOff = 0;
                                    }
                                }

                                ExpectedWorkingHours = EndTime - StartTime;
                                if (EndTime > StartTime)
                                {
                                    ExpectedWorkingHours = EndTime - StartTime;
                                }
                                else
                                {
                                    var EndTime1 = EndTime.AddDays(1);
                                    ExpectedWorkingHours = EndTime1 - StartTime;
                                }
                                var startTime = StartTime.ToString("hh:mm:tt");
                                var endTime = EndTime.ToString("hh:mm:tt");
                            }
                            for (var dt = Convert.ToDateTime(classesToSave.RA.StartDate);
                                dt <= Convert.ToDateTime(classesToSave.RA.EndDate); dt = dt.AddDays(1))
                            {
                                dates.Add(dt);
                            }
                            foreach (var date in dates)
                            {

                                DateTime ShiftOutDate = date;
                                if (EndTime < StartTime)
                                {
                                    ShiftOutDate = date.AddDays(1);
                                }
                                StringBuilder stringBuilder = new StringBuilder();
                                SqlParameter sqlParameter = new SqlParameter("@StaffId", classesToSave.RA.StaffId);
                                stringBuilder.Append("Select Top 1 Id from AttendanceData where StaffId = @StaffId " +
                                "AND CONVERT ( Date , ShiftInDate ) ='" + date.ToString("yyyy-MM-dd") + "' ");

                                try
                                {
                                    Id = context.Database.SqlQuery<Int64>(stringBuilder.ToString(), sqlParameter).FirstOrDefault();
                                }
                                catch (Exception)
                                {
                                    throw;
                                }

                                if (Id != 0)
                                {
                                    StringBuilder stringBuilder2 = new StringBuilder();
                                    stringBuilder2.Append("Update AttendanceData set ShiftId = @NewShiftId , ShiftShortName = @ShiftShortName," +
                                        " ShiftInDate = '" + date.ToString("yyyy-MM-dd") + "', ShiftInTime = '" + StartTime.ToString("yyyy-MM-dd HH:mm:ss:ms") + "' ," +
                                        " ShiftOutDate = '" + ShiftOutDate.ToString("yyyy-MM-dd") + "', IsWeeklyOff = @IsWeeklyOff," +
                                        "ShiftOutTime = '" + EndTime.ToString("yyyy-MM-dd HH:mm:ss:ms") + "', ExpectedWorkingHours = @ExpectedWorkingHours, " +
                                        " [IsProcessed] = 0 where  Id = @Id AND  StaffId = @StaffId");
                                    context.Database.ExecuteSqlCommand(stringBuilder2.ToString(), new SqlParameter("@NewShiftId", classesToSave.RA.NewShiftId),
                                        new SqlParameter("@ShiftShortName", ShiftShortName), new SqlParameter("@IsWeeklyOff", IsWeeklyOff),
                                        new SqlParameter("@ExpectedWorkingHours", ExpectedWorkingHours), new SqlParameter("@Id", Id),
                                        new SqlParameter("@StaffId", classesToSave.RA.StaffId));
                                }

                                else
                                {
                                    var QryStr3 = new StringBuilder();
                                    QryStr3.Append(" Insert into AttendanceData([StaffId],[ShiftId],[ShiftShortName],[ShiftInDate],[ShiftInTime]," +
                                    " [ShiftOutDate],  [ShiftOutTime],[ExpectedWorkingHours],[IsEarlyComing],[IsEarlyComingValid],[IsLateComing]," +
                                   "  [IsLateComingValid],[IsEarlyGoing], [IsEarlyGoingValid],[IsLateGoing],[IsLateGoingValid],[IsOT],[IsOTValid]," +
                                    " [IsManualPunch],[IsSinglePunch],[IsIncorrectPunches],[IsDisputed],[OverRideEarlyComing]," +
                                    " [OverRideLateComing],[OverRideEarlyGoing],[OverRideLateGoing],[OverRideOT],[AttendanceStatus]," +
                                    " [FHStatus],[SHStatus],[AbsentCount],[DayAccount],[IsLeave],[IsLeaveValid],[IsLeaveWithWages],[IsAutoShift]," +
                                    " [IsWeeklyOff],[IsPaidHoliday],[IsProcessed]) VALUES (@StaffId,@NewShiftId,@ShiftShortName," +
                                    " '" + date.ToString("yyyy-MM-dd") + "','" + StartTime.ToString("yyyy-MM-dd HH:mm:ss:ms") + "','" + ShiftOutDate.ToString("yyyy-MM-dd") + "'," +
                                    " '" + EndTime.ToString("yyyy-MM-dd HH:mm:ss:ms") + "' , @ExpectedWorkingHours,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0," +
                                    " 0,0,0,0,'-','-','-',0,0,0,0,0,0,@IsWeeklyOff,0,0)");

                                    context.Database.ExecuteSqlCommand(QryStr3.ToString(), new SqlParameter("@StaffId", classesToSave.RA.StaffId),
                                        new SqlParameter("@NewShiftId", classesToSave.RA.NewShiftId), new SqlParameter("@ShiftShortName", ShiftShortName),
                                        new SqlParameter("@ExpectedWorkingHours", ExpectedWorkingHours), new SqlParameter("@IsWeeklyOff", IsWeeklyOff));
                                }
                                if (classesToSave.RA.StartDate < currentDate)
                                {
                                    if (classesToSave.RA.EndDate >= currentDate)
                                    {
                                        toDate = DateTime.Now.AddDays(-1);
                                    }
                                    commonRepository.LogIntoIntoAttendanceControlTable(classesToSave.RA.StaffId, fromDate, toDate, "SC", classesToSave.RA.Id);
                                }
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
                catch (Exception err)
                {
                    trans.Rollback();
                    throw err;
                }
                context.SaveChanges();
                trans.Commit();
                if (classesToSave.ESL != null)
                {
                    foreach (var l in classesToSave.ESL)
                        if (l.To != "-" && l.To != "")
                            commonRepository.SendEmailMessage(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody);
                }
            }
        }

        public void ApproveShiftChangeApplication(ClassesToSave classesToSave, bool isFinalLevelApproval)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            CommonRepository commonRepository = new CommonRepository();
            var dates = new List<DateTime>();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    context.Entry(classesToSave.RA).Property("IsApproved").IsModified = true;
                    context.Entry(classesToSave.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(classesToSave.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(classesToSave.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(classesToSave.AA).Property("Comment").IsModified = true;
                    context.Entry(classesToSave.AA).Property("Approval2statusId").IsModified = true;
                    context.Entry(classesToSave.AA).Property("Approval2By").IsModified = true;
                    context.Entry(classesToSave.AA).Property("Approval2On").IsModified = true;

                    if (isFinalLevelApproval == true)
                    {
                        string ShiftName = string.Empty;
                        string ShiftShortName = string.Empty;
                        DateTime StartTime = DateTime.Now;
                        DateTime EndTime = DateTime.Now;
                        TimeSpan ExpectedWorkingHours;
                        Int64 Id = 0;
                        int IsWeeklyOff = 0;

                        try
                        {
                            if (classesToSave.RA.NewShiftId == "LV0011")
                            {
                                ShiftName = "WO";
                                ShiftShortName = "WO";
                                StartTime = Convert.ToDateTime("1900-01-01 00:00:00.000");
                                EndTime = Convert.ToDateTime("1900-01-01 00:00:00.000");
                                ExpectedWorkingHours = EndTime - StartTime;
                                IsWeeklyOff = 1;
                            }
                            else
                            {
                                var lst = new List<ShiftDetailsForShiftChange>();

                                lst = commonRepository.GetShiftDetailsForShiftChange(classesToSave.RA.NewShiftId).ToList();

                                if (lst != null)
                                {
                                    foreach (var rec in lst)
                                    {
                                        ShiftName = rec.Name;
                                        ShiftShortName = rec.ShortName;
                                        StartTime = rec.StartTime;
                                        EndTime = rec.EndTime;
                                        IsWeeklyOff = 0;
                                    }
                                }

                                ExpectedWorkingHours = EndTime - StartTime;
                                if (EndTime > StartTime)
                                {
                                    ExpectedWorkingHours = EndTime - StartTime;
                                }
                                else
                                {
                                    var EndTime1 = EndTime.AddDays(1);
                                    ExpectedWorkingHours = EndTime1 - StartTime;
                                }
                                var startTime = StartTime.ToString("hh:mm:tt");
                                var endTime = EndTime.ToString("hh:mm:tt");
                            }
                            for (var dt = Convert.ToDateTime(classesToSave.RA.StartDate);
                                dt <= Convert.ToDateTime(classesToSave.RA.EndDate); dt = dt.AddDays(1))
                            {
                                dates.Add(dt);
                            }
                            foreach (var date in dates)
                            {

                                DateTime ShiftOutDate = date;
                                if (EndTime < StartTime)
                                {
                                    ShiftOutDate = date.AddDays(1);
                                }
                                StringBuilder stringBuilder = new StringBuilder();
                                SqlParameter sqlParameter = new SqlParameter("@StaffId", classesToSave.RA.StaffId);
                                stringBuilder.Append("Select Top 1 Id from AttendanceData where StaffId = @StaffId " +
                                "AND CONVERT ( Date , ShiftInDate ) ='" + date.ToString("yyyy-MM-dd") + "' ");

                                try
                                {
                                    Id = context.Database.SqlQuery<Int64>(stringBuilder.ToString(), sqlParameter).FirstOrDefault();
                                }
                                catch (Exception)
                                {
                                    throw;
                                }

                                if (Id != 0)
                                {
                                    StringBuilder stringBuilder2 = new StringBuilder();
                                    stringBuilder2.Append("Update AttendanceData set ShiftId = @NewShiftId , ShiftShortName = @ShiftShortName," +
                                        " ShiftInDate = '" + date.ToString("yyyy-MM-dd") + "', ShiftInTime = '" + StartTime.ToString("yyyy-MM-dd HH:mm:ss:ms") + "' ," +
                                        " ShiftOutDate = '" + ShiftOutDate.ToString("yyyy-MM-dd") + "', IsWeeklyOff = @IsWeeklyOff," +
                                        "ShiftOutTime = '" + EndTime.ToString("yyyy-MM-dd HH:mm:ss:ms") + "', ExpectedWorkingHours = @ExpectedWorkingHours, " +
                                        " [IsProcessed] = 0 where  Id = @Id AND  StaffId = @StaffId");
                                    context.Database.ExecuteSqlCommand(stringBuilder2.ToString(), new SqlParameter("@NewShiftId", classesToSave.RA.NewShiftId),
                                        new SqlParameter("@ShiftShortName", ShiftShortName), new SqlParameter("@IsWeeklyOff", IsWeeklyOff),
                                        new SqlParameter("@ExpectedWorkingHours", ExpectedWorkingHours), new SqlParameter("@Id", Id),
                                        new SqlParameter("@StaffId", classesToSave.RA.StaffId));
                                }

                                else
                                {
                                    var QryStr3 = new StringBuilder();
                                    QryStr3.Append(" Insert into AttendanceData([StaffId],[ShiftId],[ShiftShortName],[ShiftInDate],[ShiftInTime]," +
                                    " [ShiftOutDate],  [ShiftOutTime],[ExpectedWorkingHours],[IsEarlyComing],[IsEarlyComingValid],[IsLateComing]," +
                                   "  [IsLateComingValid],[IsEarlyGoing], [IsEarlyGoingValid],[IsLateGoing],[IsLateGoingValid],[IsOT],[IsOTValid]," +
                                    " [IsManualPunch],[IsSinglePunch],[IsIncorrectPunches],[IsDisputed],[OverRideEarlyComing]," +
                                    " [OverRideLateComing],[OverRideEarlyGoing],[OverRideLateGoing],[OverRideOT],[AttendanceStatus]," +
                                    " [FHStatus],[SHStatus],[AbsentCount],[DayAccount],[IsLeave],[IsLeaveValid],[IsLeaveWithWages],[IsAutoShift]," +
                                    " [IsWeeklyOff],[IsPaidHoliday],[IsProcessed]) VALUES (@StaffId,@NewShiftId,@ShiftShortName," +
                                    " '" + date.ToString("yyyy-MM-dd") + "','" + StartTime.ToString("yyyy-MM-dd HH:mm:ss:ms") + "','" + ShiftOutDate.ToString("yyyy-MM-dd") + "'," +
                                    " '" + EndTime.ToString("yyyy-MM-dd HH:mm:ss:ms") + "' , @ExpectedWorkingHours,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0," +
                                    " 0,0,0,0,'-','-','-',0,0,0,0,0,0,@IsWeeklyOff,0,0)");


                                    SqlParameter[] sqlParameters = {
                                         new SqlParameter("@StaffId", classesToSave.RA.StaffId),
                                            new SqlParameter("@NewShiftId", classesToSave.RA.NewShiftId),
                                            new SqlParameter("@ShiftShortName", ShiftShortName),
                                            new SqlParameter("@ExpectedWorkingHours", ExpectedWorkingHours),
                                            new SqlParameter("@IsWeeklyOff", IsWeeklyOff)
                                        };
                                    context.Database.ExecuteSqlCommand(QryStr3.ToString(), sqlParameters);
                                }
                                if (classesToSave.RA.StartDate < currentDate)
                                {
                                    if (classesToSave.RA.EndDate >= currentDate)
                                    {
                                        toDate = DateTime.Now.AddDays(-1);
                                    }
                                    commonRepository.LogIntoIntoAttendanceControlTable(classesToSave.RA.StaffId, fromDate, toDate, "SC", "");
                                }
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        if (classesToSave.RA.StartDate != null && classesToSave.RA.EndDate != null)
                        {
                            //Insert into attendance control table
                            fromDate = Convert.ToDateTime(classesToSave.RA.StartDate);
                            toDate = Convert.ToDateTime(classesToSave.RA.EndDate);
                            if (fromDate.Date < currentDate.Date)
                            {
                                if (toDate.Date >= currentDate.Date)
                                {
                                    toDate = DateTime.Now.AddDays(-1);
                                }
                                commonRepository.LogIntoIntoAttendanceControlTable(classesToSave.RA.StaffId, fromDate.Date, toDate.Date, classesToSave.RA.RequestApplicationType, classesToSave.AA.Id);
                            }
                        }
                    }
                    context.SaveChanges();
                    Trans.Commit();
                    if (classesToSave.ESL != null)
                    {
                        foreach (var l in classesToSave.ESL)
                            if (l.To != "-" && l.To != "")
                                commonRepository.SendEmailMessage(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody);
                    }
                }
                catch (Exception e)
                {
                    Trans.Rollback();
                    throw e;
                }
            }
        }
        public void RejectShiftChangeApplication(ClassesToSave CTS)
        {
            CommonRepository commonRepository = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    context.Entry(CTS.RA).Property("IsRejected").IsModified = true;

                    //Update the application approval table.
                    context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2statusId").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2By").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2On").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;
                    //
                    context.SaveChanges();
                    Trans.Commit();
                    foreach (var l in CTS.ESL)
                        if (l.To != "-"  &&string.IsNullOrEmpty(l.To).Equals(false))
                            commonRepository.SendEmailMessage(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody);
                }
                catch (Exception e)
                {
                    Trans.Rollback();
                    throw e;
                }
            }
        }
        public List<ShiftchangeListItemViewModel> GetAppliedShiftChangeList(string staffId)
        {
            try
            {
                string Qry = string.Empty;
                Qry = @"select Top 30 ApplicationId as [Id],StaffId,StaffName as FirstName ,StartDate as FromDate,EndDate as ToDate,
                NewShiftName as ShiftName, Remarks,Approval1StatusName as ApproverStatus1,
                Approval2statusName as ApproverStatus2,IsCancelled from [View_ShiftChangeApplicationHistory] as v where v.StaffId=@StaffId  
                Order by CONVERT(DateTime, StartDate, 101) Desc ";
                SqlParameter[] sqlParameters = { new SqlParameter("@StaffId", staffId) };
                var Obj = context.Database.SqlQuery<ShiftchangeListItemViewModel>(Qry, sqlParameters).ToList();
                return Obj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<WeeklyCalender> GetWeeklyCalenderList(string StaffId)
        {
            try
            {
                string Qry = string.Empty;
                Qry = @"Exec [dbo].[GetWeeklyWorkedHoursCalendar] @StaffId";
                SqlParameter[] sqlParameters = { new SqlParameter("@StaffId", StaffId) };
                return context.Database.SqlQuery<WeeklyCalender>(Qry, sqlParameters).ToList();

            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}

