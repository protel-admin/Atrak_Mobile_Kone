using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;
using System.Data.SqlClient;
using System.Data.Entity.Migrations;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class RACoffRequestApplicationRepository:IDisposable
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
        public RACoffRequestApplicationRepository()
        {
            context = new AttendanceManagementContext();
        }
        string Message = string.Empty;
        public List<RACoffRequestApplication> GetAppliedCoffRequest(string StaffId)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Append(" SELECT ");
                Str.Append(" Id, ");
                Str.Append(" StaffId, ");
                Str.Append(" Remarks, ");
                Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,StartDate,106),' ','-')) AS StartDate, ");
                Str.Append(" UPPER(REPLACE(CONVERT(VARCHAR,EndDate,106),' ','-')) AS EndDate, ");
                Str.Append(" TotalDays, ");
                Str.Append(" CASE  ");
                Str.Append(" WHEN IsCancelled = 0	and IsApproved = 0	and IsRejected = 0 THEN 'PENDING' ");
                Str.Append(" WHEN IsCancelled = 0	and IsApproved = 1	and IsRejected = 0 THEN 'APPROVED' ");
                Str.Append(" WHEN IsCancelled = 0	and IsApproved = 0	and IsRejected = 1 THEN 'REJECTED' ");
                Str.Append(" WHEN IsCancelled = 1	and IsApproved = 0	and IsRejected = 0 THEN 'CANCELLED' ");
                Str.Append(" WHEN IsCancelled = 1	and IsApproved = 1	and IsRejected = 0 THEN 'APPROVED BUT CANCELLED' ");
                Str.Append(" WHEN IsCancelled = 1	and IsApproved = 0	and IsRejected = 1 THEN 'REJECTED BUT CANCELLED' ");
                Str.Append(" END as [Status] ");
                Str.Append(" FROM RequestApplication A WHERE RequestApplicationType = 'CO' AND StaffId = @StaffId order by ApplicationDate desc ");

                //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
                var Obj = context.Database.SqlQuery<RACoffRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).Select(d => new RACoffRequestApplication
                {
                    Id = d.Id,
                    StaffId = d.StaffId,
                    Remarks = d.Remarks,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    TotalDays = d.TotalDays,
                    Status = d.Status
                }).ToList();
                return Obj;
            }
            catch 
            {
                return new List<RACoffRequestApplication>();
            }
        }

        public LeaveTypeAndBalance GetLeaveTypeAndBalance(string StaffId)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select leavetypeid,CAST(LEAVEBALANCE as varchar) as LeaveBalance, LeaveName from leavebalance where StaffId='" + StaffId + "' and IsActive='1' and LeavetypeId='LV0005'");
            try
            {
                var data = context.Database.SqlQuery<LeaveTypeAndBalance>(QryStr.ToString()).Select(d => new LeaveTypeAndBalance()
                {
                    LeaveBalance = d.LeaveBalance,
                    LeaveTypeId = d.LeaveTypeId,
                    LeaveName = d.LeaveName
                }).FirstOrDefault();

                if (data == null)
                {
                    return new LeaveTypeAndBalance();
                }
                else
                {
                    //if (string.IsNullOrEmpty(data.) == true)
                    //{
                    //    throw new Exception("Employee does not exists.");
                    //}
                    return data;
                }
            }
            catch (Exception e)
            {
                throw e;
                //return new IndividualLeaveCreditDebit_EmpDetails();
            }
        }

        public List<PermissionType> GetPermissionTypes()
        {
            var Obj = context.PermissionType.Where(d => d.IsActive.Equals(true)).ToList();
            return Obj;
        }

        public string GetUniqueId()
        {
            return context.Database.SqlQuery<string>("select convert(varchar,getdate(),112) + replace(convert(varchar,getdate(),114),':','')").First();
        }

        public void SaveRequestApplication(ClassesToSave DataToSave)
        {
            //context.Entry(DataToSave).State = System.Data.Entity.EntityState.Added;
            CommonRepository CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    // save to request application table.
                    context.RequestApplication.Add(DataToSave.RA);
                    // save to application approval table.
                    context.ApplicationApproval.Add(DataToSave.AA);
                    // save to email send log table.
                    if (DataToSave.ESL != null)
                    {
                        //context.Testing.Add(DataToSave.Test);
                        foreach (var l in DataToSave.ESL)
                            //context.EmailSendLog.Add(l);
                            if (l.To != "-")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                    context.SaveChanges();
                    Trans.Commit();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException err)
                {
                    Trans.Rollback();
                    throw err;
                }
            }
        }


public void SaveRequestApplication(ClassesToSave DataToSave, bool IsFinalLevelApproval)
        {
            //CommonRepository commonRepository = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    // save to request application table.
                    context.RequestApplication.Add(DataToSave.RA);
                    // save to application approval table.
                    context.ApplicationApproval.Add(DataToSave.AA);
                    // save to email send log table.

                    if (IsFinalLevelApproval == true)
                    {
                        context.EmployeeLeaveAccount.Add(DataToSave.ELA);
                        if (DataToSave.RA.StartDate.Date < DateTime.Now.Date)
                        {
                           DateTime fromDate = Convert.ToDateTime(DataToSave.RA.StartDate);
                            DateTime toDate = Convert.ToDateTime(DataToSave.RA.EndDate);
                            if (fromDate.Date < DateTime.Now.Date)
                            {
                                if (toDate.Date >= DateTime.Now.Date)
                                {
                                    toDate = DateTime.Now.AddDays(-1);
                                }
                                using (CommonRepository commonRepository = new CommonRepository())
                                {
                                    commonRepository.LogIntoIntoAttendanceControlTable(DataToSave.RA.StaffId, fromDate, toDate,
                                        DataToSave.RA.RequestApplicationType, DataToSave.RA.Id);
                                }
                            }
                        }
                    }
                    context.SaveChanges();
                    Trans.Commit();
                    if (DataToSave.ESL != null)
                    {
                        foreach (var l in DataToSave.ESL)
                        {
                            if (l.To != "-")
                                using (CommonRepository commonRepository = new CommonRepository())
                                {
                                    commonRepository.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                                }
                        }
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException err)
                {
                    Trans.Rollback();
                    throw err;
                }
            }
        }



        public void RejectApplication(ClassesToSave CTS)
        {
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    context.Entry(CTS.RA).Property("IsRejected").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;
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

        public void ApproveApplication(ClassesToSave CTS)
        {
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
                    //Insert into employee leave account.
                    //context.EmployeeLeaveAccount.Add(CTS.ELA);
                    //
                    context.SaveChanges();
                    Trans.Commit();
                }
                catch (Exception e)
                {
                    Trans.Rollback();
                    throw e;
                }
            }
        }

        public void CancelApplication(ClassesToSave CTS)
        {
            if (CTS.ELA == null)
            {
                //context.Entry(CTS.RA).State = System.Data.Entity.EntityState.Modified;
                CTS.RA.CancelledBy = CTS.RA.StaffId;
                context.Entry(CTS.RA).Property("IsCancelled").IsModified = true;
                context.Entry(CTS.RA).Property("CancelledDate").IsModified = true;
                context.Entry(CTS.RA).Property("CancelledBy").IsModified = true;
                context.SaveChanges();
            }
            else
            {
                using (var Trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        CTS.RA.CancelledBy = CTS.RA.StaffId;
                        context.Entry(CTS.RA).Property("IsCancelled").IsModified = true;
                        context.Entry(CTS.RA).Property("CancelledDate").IsModified = true;
                        context.Entry(CTS.RA).Property("CancelledBy").IsModified = true;
                        if (CTS.ELA != null)
                        {
                            //set the Employee Leave Account class to add.
                            context.EmployeeLeaveAccount.Add(CTS.ELA);
                        }
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
        }

        public RequestApplication GetRequestApplicationDetails(string Id)
        {
            return context.RequestApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }

        public ApplicationApproval GetApplicationApproval(string ParentId)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(ParentId) && d.ParentType.Equals("CO")).FirstOrDefault();
        }
        //Self
        public List<LeaveDuration> GetDurationListRepository()
        {
            var qryStr = new StringBuilder();
            qryStr.Append("select Id , Name , isactive from leaveduration where isactive = 1");
            try
            {
                var lst = context.Database.SqlQuery<LeaveDuration>(qryStr.ToString()).Select(d => new LeaveDuration()
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<LeaveDuration>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LeaveDuration>();
            }
        }
        public string GetCoffReqPeriodRepository()
        {
            Message = context.Settings.Where(condition => condition.Parameter == "COffReqPeriod").Select(select => select.Value).FirstOrDefault();
            return Message;
        }
        public string GetCoffReqPeriodRepository(string StaffId)
        {
            var qryStr = new StringBuilder();
            qryStr.Append(" Select Value From [RuleGroupTxn] Where RuleId in (Select Id from[Rule] Where Name = 'LapsePeriodForCompOff') And " +
                          " RuleGroupId in (Select PolicyId from StaffOfficial Where StaffId = @StaffId ) and LocationId= (Select LocationId from StaffOfficial Where StaffId =@StaffId )  ");
            try
            {
                var lst = context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@StaffId", StaffId)).FirstOrDefault();
                if (lst == null)
                {
                    return lst;
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            //Message = context.Settings.Where(condition => condition.Parameter == "COffReqPeriod").Select(select => select.Value).FirstOrDefault();
            //return Message;
        }
        public int GetDesignationRank(string StaffId)
        {
            var query = new StringBuilder();
            query.Clear();
            query.Append("Select peopleSoftCode from Designation where Id in (Select DesignationId from StaffOfficial where StaffId = '" + StaffId + "')");
            int designationRank = Convert.ToInt16(context.Database.SqlQuery<string>(query.ToString()).FirstOrDefault());
            return designationRank;
        }
        public string ValidateCOFFCreditApplication(string StaffId, string FromDate)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("Select dbo.ValidateCOFF_Credit_Application('" + StaffId + "','" + FromDate + "','" + FromDate + "')");

            try
            {
                var str = context.Database.SqlQuery<string>(QryStr.ToString()).FirstOrDefault();
                return str;
            }
            catch (Exception err)
            {
                return "ERROR!" + err.Message;
            }
        }
    }
}
