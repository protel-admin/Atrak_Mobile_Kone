﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;
using System.Data.SqlClient;
using System.Data.Entity.Migrations;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class RACoffCreditApplicationRepository : IDisposable
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
        public RACoffCreditApplicationRepository()
        {
            context = new AttendanceManagementContext();
        }

        public int GetCompOffLapsePeriod(string LocationId, string StaffId)
        {
            var qryStr = new StringBuilder();
            SqlParameter[] Params = new SqlParameter[2];
            Params[0] = new SqlParameter("@LocationId", LocationId);
            Params[1] = new SqlParameter("@StaffId", StaffId);
            qryStr.Append(" Select Top 1 Coalesce(A.Value,30) from [RuleGroupTxn] A Inner Join [Rule] B On A.ruleid = B.id Inner Join [StaffOfficial] C " +
                          "On A.rulegroupid = C.PolicyId and A.LocationID = C.LocationId Where B.name = 'LapsePeriodForCompOff' " +
                          "and  C.LocationId = @LocationId and C.staffId = @StaffId ");
            try
            {
                var lapsePeriod = context.Database.SqlQuery<int>(qryStr.ToString(), Params).FirstOrDefault();
                return lapsePeriod;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string ValidateCoffCreditApplication(string StaffId, string FromDate, string ToDate)
        {
            StringBuilder builder = new StringBuilder();
            string Message = string.Empty;
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@StaffId", StaffId);
                param[1] = new SqlParameter("@COffFromDate", FromDate);
                param[2] = new SqlParameter("@COffToDate", ToDate);
                builder = new StringBuilder();
                builder.Append("Select [dbo].[ValidateCOFF_Credit_Application] (@StaffId,@COffFromDate,@COffToDate)");
                Message = context.Database.SqlQuery<string>(builder.ToString(), param).FirstOrDefault();
                if (!Message.Equals("OK."))
                {
                    throw new Exception(Message);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Message;
        }


        public List<RACoffCreditRequestApplication> GetAppliedCoffCreditRequest(string StaffId, string AppliedId, string userRole)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Append("select top 30 COffId as Id, StaffId, Name as StaffName, WorkedDate as StartDate, COffReason as Remarks, Approval1StatusName as Status1," +
                    "Approval2statusName as Status2, IsCancelled as IsCancelled from View_COff_Credit_ApplicationHistory where staffid = @StaffId " +
                    "Order by CONVERT(DateTime, WorkedDate, 101) Desc");
                var Obj = context.Database.SqlQuery<RACoffCreditRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).ToList();
                return Obj;
            }
            catch
            {
                return new List<RACoffCreditRequestApplication>();
            }
        }

        public List<RACoffCreditRequestApplication> GetAppliedCoffCreditRequestForMyTeam(string StaffId, string AppliedBy, string userRole)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                //Str.Append(" select COffId as Id,StaffId,FirstName as StaffName,COffReason as Remarks,COffReqDate as StartDate,COffAvailDate as EndDate,convert(varchar(10),TotalDays) as TotalDays,AppliedBy, Approval1StatusName as Status1, Approval2statusName as Status2, IsCancelled as IsCancelled, Approval1Owner, Approval2Owner from vwCOffApproval where staffid = @StaffId  ");

                if (userRole == "3" || userRole == "5")
                {
                    Str.Append(" select Top 30 COffId as Id,StaffId,Name as StaffName,COffReason as Remarks,WorkedDate as StartDate," +
                        " WorkedDate as EndDate, convert(varchar(10), TotalDays) as TotalDays, AppliedBy, Approval1StatusName " +
                        "as Status1, Approval2statusName as Status2, IsCancelled as IsCancelled, ApprovalOwner as Approval1Owner," +
                        " Approval2Owner from[View_COff_Credit_ApplicationHistory] where staffid = @StaffId Order by" +
                        " Convert(Date,WorkedDate) desc ");
                }
                else
                {
                    Str.Append("select Top 30 COffId as Id,StaffId,Name as StaffName,COffReason as Remarks,WorkedDate as StartDate," +
                        " WorkedDate as EndDate, convert(varchar(10), TotalDays) as TotalDays, AppliedBy, Approval1StatusName " +
                        "as Status1, Approval2statusName as Status2, IsCancelled as IsCancelled, ApprovalOwner as Approval1Owner," +
                        " Approval2Owner from[View_COff_Credit_ApplicationHistory] where staffid = @StaffId and(AppliedBy = @AppliedBy" +
                        " or ApprovalOwner = @AppliedBy or Approval2Owner =@AppliedBy) Order by Convert(Date,WorkedDate) desc");
                }
                var Obj = context.Database.SqlQuery<RACoffCreditRequestApplication>(Str.ToString(),
                    new SqlParameter("@StaffId", StaffId), new SqlParameter("@AppliedBy", AppliedBy)).ToList();
                return Obj;
            }
            catch
            {
                return new List<RACoffCreditRequestApplication>();
            }
        }

        //To get the C-Off credit application which are mapped under me for approval
        public List<RACoffCreditRequestApplication> GetCoffCreditRequestMappedUnderMe(string loggedInUser)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("select Top 30 COffId as Id,StaffId,Name as StaffName,COffReason as Remarks,WorkedDate as StartDate, WorkedDate as EndDate, convert(varchar(10), TotalDays) as TotalDays, AppliedBy, Approval1StatusName as Status1, Approval2statusName as Status2, IsCancelled as IsCancelled, ApprovalOwner as Approval1Owner, Approval2Owner from [View_COff_Credit_ApplicationHistory] where ApprovalOwner = @LoggedInUser or Approval2Owner = @LoggedInUser) Order by Convert(Date,WorkedDate) desc");
                var Obj = context.Database.SqlQuery<RACoffCreditRequestApplication>(stringBuilder.ToString(),
                    new SqlParameter("@LoggedInUser", loggedInUser)).ToList();
                return Obj;
            }
            catch
            {
                return new List<RACoffCreditRequestApplication>();
            }
        }

        //Coff Avaling SelfApplication List
        public List<RACoffCreditRequestApplication> RenderAppliedCompAvailingList(string StaffId, string AppliedBy, string userRole)
        {
            try
            {
                StringBuilder Str = new StringBuilder();

                if (userRole == "3" || userRole == "5")
                {
                    Str.Append(" select COffId as Id,StaffId,FirstName as StaffName,COffReason as Remarks,COffReqDate as StartDate,COffAvailDate as EndDate,convert(varchar(10),TotalDays) as TotalDays,AppliedBy, Approval1StatusName as Status1, Approval2statusName as Status2, IsCancelled as IsCancelled, Approval1Owner, Approval2Owner from vwCOffApproval where staffid = @StaffId  ");
                }
                else
                {
                    Str.Append(" select COffId as Id,StaffId,FirstName as StaffName,COffReason as Remarks,COffReqDate as StartDate,COffAvailDate as EndDate,convert(varchar(10),TotalDays) as TotalDays,AppliedBy, Approval1StatusName as Status1, Approval2statusName as Status2, IsCancelled as IsCancelled, Approval1Owner, Approval2Owner from vwCOffApproval where staffid = @StaffId and(AppliedBy = '" + AppliedBy + "' or Approval1Owner = '" + AppliedBy + "' or Approval2Owner = '" + AppliedBy + "') ");
                }
                var Obj = context.Database.SqlQuery<RACoffCreditRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).ToList();
                return Obj;
            }
            catch
            {
                return new List<RACoffCreditRequestApplication>();
            }
        }

        //Coff Avaling TeamApplication List
        public List<RACoffCreditRequestApplication> RenderAppliedCompAvailingListMyteam(string StaffId, string AppliedBy, string userRole)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                if (userRole == "3" || userRole == "5")
                {
                    Str.Append(" select COffId as Id,StaffId,FirstName as StaffName,COffReason as Remarks,COffReqDate as StartDate,COffAvailDate as EndDate,convert(varchar(10),TotalDays) as TotalDays,AppliedBy, Approval1StatusName as Status1, Approval2statusName as Status2, IsCancelled as IsCancelled, Approval1Owner, Approval2Owner from vwCOffApproval where staffid = @StaffId  ");
                }
                else
                {
                    Str.Append(" select COffId as Id,StaffId,FirstName as StaffName,COffReason as Remarks,COffReqDate as StartDate,COffAvailDate as EndDate,convert(varchar(10),TotalDays) as TotalDays,AppliedBy, Approval1StatusName as Status1, Approval2statusName as Status2, IsCancelled as IsCancelled, Approval1Owner, Approval2Owner from vwCOffApproval where staffid = @StaffId and(AppliedBy = '" + AppliedBy + "' or Approval1Owner = '" + AppliedBy + "' or Approval2Owner = '" + AppliedBy + "') ");
                }
                //var Obj = context.RequestApplication.Where(d => d.RequestApplicationType.Equals("LA")).ToList();
                var Obj = context.Database.SqlQuery<RACoffCreditRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@AppliedId", AppliedBy)).ToList();
                return Obj;
            }
            catch
            {
                return new List<RACoffCreditRequestApplication>();
            }
        }


        public List<RACoffCreditRequestApplication> GetAllCoffList(string StaffId, string AppliedBy, string userRole)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                if (userRole == "3" || userRole == "5")
                {
                    Str.Append("  select Top 30 COffId as Id,StaffId,Name as StaffName,COffReason as Remarks,WorkedDate as StartDate," +
                        " WorkedDate as EndDate, convert(varchar(10), TotalDays) as TotalDays, AppliedBy, Approval1StatusName " +
                        "as Status1, Approval2statusName as Status2, IsCancelled as IsCancelled, ApprovalOwner as Approval1Owner," +
                        " Approval2Owner from[View_COff_Credit_ApplicationHistory] where staffid = @StaffId  " +
                        "Order by Convert(Date,WorkedDate) desc");
                }
                else
                {
                    Str.Append("select Top 30 COffId as Id,StaffId,Name as StaffName,COffReason as Remarks,WorkedDate as StartDate," +
                        " WorkedDate as EndDate, convert(varchar(10), TotalDays) as TotalDays, AppliedBy, Approval1StatusName " +
                        "as Status1, Approval2statusName as Status2, IsCancelled as IsCancelled, ApprovalOwner as Approval1Owner," +
                        " Approval2Owner from[View_COff_Credit_ApplicationHistory] where staffid = @StaffId and(AppliedBy = '" + AppliedBy + "' or " +
                        "Approval1Owner = '" + AppliedBy + "' or Approval2Owner = '" + AppliedBy + "' Order by Convert(Date,WorkedDate) desc) ");
                }
                var Obj = context.Database.SqlQuery<RACoffCreditRequestApplication>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).ToList();
                return Obj;
            }
            catch
            {
                return new List<RACoffCreditRequestApplication>();
            }
        }

        public List<CoffReqDates> GetAllOTDates(string Staffid, string FromDate, string ToDate)
        {
            var qrystr = new StringBuilder();
            qrystr.Clear();

            qrystr.Append("EXEC [DBO].[GETCOFFDATES] '" + Staffid + "', '" + FromDate + "', '" + ToDate + "'");
            var data = context.Database.SqlQuery<CoffReqDates>(qrystr.ToString()).ToList();
            if (data != null)
            {
                return data;
            }
            else
            {
                List<CoffReqDates> CRD = new List<CoffReqDates>();
                return CRD;
            }
        }


        public string GetUniqueId()
        {
            return context.Database.SqlQuery<string>("select convert(varchar,getdate(),112) + replace(convert(varchar,getdate(),114),':','')").First();
        }

        public List<COffReqAvailModel> GetWorkedDatesForCompOffCreditRequest(string StaffId)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Append("Select StaffId , Replace(Convert(Varchar,StartDate,106),' ','-') as WorkedDate," +
                    " cast (TotalDays as decimal(10,2)) as Balance,Replace(Convert(Varchar,ExpiryDate,106),' ','-') as ExpiryDate " +
                    " From RequestApplication Where StaffId = @StaffId And IsCancelled = 0 and IsApproved = 1 and IsRejected = 0 and " +
                    " RequestApplicationType = 'CR' and Convert(Date,ExpiryDate) >= Convert(Date,GetDate()) and WorkedDate" +
                    " not in (Select WorkedDate From RequestApplication Where StaffId = @StaffId And RequestApplicationType = 'CO'" +
                    " and IsCancelled = 0 and IsRejected = 0) Order by StartDate Asc");

                var Obj = context.Database.SqlQuery<COffReqAvailModel>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).Select(d => new COffReqAvailModel
                {
                    StaffId = d.StaffId,
                    WorkedDate = d.WorkedDate,
                    Balance = d.Balance,
                    ExpiryDate = d.ExpiryDate,

                }).ToList();
                return Obj;
            }
            catch
            {
                return new List<COffReqAvailModel>();
            }
        }
        public void SaveRequestApplication(ClassesToSave DataToSave, bool isFinalLevelApproval)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            CommonRepository CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    // save to request application table.
                    context.RequestApplication.Add(DataToSave.RA);
                    // save to application approval table.
                    context.ApplicationApproval.Add(DataToSave.AA);
                    if (isFinalLevelApproval == true)
                    {
                        if (DataToSave.ELA != null)
                        {
                            context.EmployeeLeaveAccount.Add(DataToSave.ELA);
                        }
                        if (DataToSave.RA.StartDate != null && DataToSave.RA.EndDate != null)
                        {
                            //Insert into attendance control table
                            fromDate = Convert.ToDateTime(DataToSave.RA.StartDate);
                            toDate = Convert.ToDateTime(DataToSave.RA.EndDate);
                            if (fromDate.Date < currentDate.Date)
                            {
                                if (toDate.Date >= currentDate.Date)
                                {
                                    toDate = DateTime.Now.AddDays(-1);
                                }
                                CR.LogIntoIntoAttendanceControlTable(DataToSave.RA.StaffId, fromDate.Date, toDate.Date, DataToSave.RA.RequestApplicationType, DataToSave.AA.Id);
                            }
                        }
                    }
                    context.SaveChanges();
                    Trans.Commit();
                    if (DataToSave.ESL != null)
                    {
                        foreach (var l in DataToSave.ESL)
                            if (string.IsNullOrEmpty(l.To).Equals(false) && l.To != "-")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
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
            var CR = new CommonRepository();
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
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;
                    context.SaveChanges();
                    Trans.Commit();
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                            if (string.IsNullOrEmpty(l.To).Equals(false) && l.To != "-")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                }
                catch
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }

        public void ApproveApplication(ClassesToSave CTS, string ReportingManagerId, bool isFinalLevelApproval)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            CommonRepository CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    context.Entry(CTS.RA).Property("IsApproved").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2statusId").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2By").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2On").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;

                    if (isFinalLevelApproval == true)
                    {
                        if (CTS.ELA != null)
                        {
                            //Insert into employee leave account.
                            context.EmployeeLeaveAccount.Add(CTS.ELA);
                        }
                        if (CTS.RA.StartDate != null && CTS.RA.EndDate != null)
                        {
                            fromDate = Convert.ToDateTime(CTS.RA.StartDate);
                            toDate = Convert.ToDateTime(CTS.RA.EndDate);
                            if (fromDate.Date < currentDate.Date)
                            {
                                if (toDate.Date >= currentDate.Date)
                                {
                                    toDate = DateTime.Now.AddDays(-1);
                                }
                                CR.LogIntoIntoAttendanceControlTable(CTS.RA.StaffId, fromDate.Date, toDate.Date, CTS.RA.RequestApplicationType, CTS.AA.Id);
                            }
                        }
                        context.SaveChanges();
                        Trans.Commit();
                        if (CTS.ESL != null)
                        {
                            foreach (var l in CTS.ESL)
                                if (string.IsNullOrEmpty(l.To).Equals(false) && l.To != "-")
                                    CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                        }
                    }
                }
                catch (Exception e)
                {
                    Trans.Rollback();
                    throw e;
                }
            }
        }

        public void CancelApplication(ClassesToSave CTS, string user)
        {
            var CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    context.Entry(CTS.RA).Property("IsCancelled").IsModified = true;
                    context.Entry(CTS.RA).Property("CancelledDate").IsModified = true;
                    context.Entry(CTS.RA).Property("CancelledBy").IsModified = true;

                    if (CTS.ELA != null)
                    {
                        //Insert into employee leave account.
                        context.EmployeeLeaveAccount.Add(CTS.ELA);
                    }
                    context.SaveChanges();
                    Trans.Commit();
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                            if (string.IsNullOrEmpty(l.To).Equals(false) && l.To != "-")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                }
                catch
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }

        public RequestApplication GetRequestApplicationDetails(string Id)
        {
            return context.RequestApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }

        public ApplicationApproval GetApplicationApproval(string ParentId)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(ParentId) && d.ParentType.Equals("CR")).FirstOrDefault();
        }
        //Coff Availling Application
        public ApplicationApproval GetApplicationApprovalForCoffAvailing(string ParentId)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(ParentId) && d.ParentType.Equals("CO")).FirstOrDefault();
        }

        public List<COffReqAvailModel> GetCompOffRequestList(string StaffId)
        {
            try
            {
                StringBuilder Str = new StringBuilder();
                Str.Append("Select StaffId , Replace(Convert(Varchar,StartDate,106),'','-') as WorkedDate," +
                    " TotalDays as Balance,Replace(Convert(Varchar,ExpiryDate,106),'','-') as ExpiryDate " +
                    " From RequestApplication Where StaffId = @StaffId And IsCancelled = 0 and IsApproved = 1 And " +
                    " RequestApplicationType = 'CR' And Convert(Date,ExpiryDate) >= Convert(Date,GetDate()) And WorkedDate" +
                    " not in (Select WorkedDate From RequestApplication Where StaffId = @StaffId And RequestApplicationType = 'CO'" +
                    " And IsCancelled = 0 And IsRejected = 0) Order by StartDate Asc");

                var Obj = context.Database.SqlQuery<COffReqAvailModel>(Str.ToString(), new SqlParameter("@StaffId", StaffId)).Select(d => new COffReqAvailModel
                {
                    StaffId = d.StaffId,
                    WorkedDate = d.WorkedDate,
                    Balance = d.Balance,
                    ExpiryDate = d.ExpiryDate,

                }).ToList();
                return Obj;
            }
            catch 
            {
                return new List<COffReqAvailModel>();
            }
        }

        public bool CheckIsCompOffAvailed(string StaffId, DateTime WorkedDate)
        {
            bool isCompOffAvailed = false;
            string applicationId = string.Empty;
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("Select Top 1 Id From RequestApplication Where StaffId = @StaffId And Convert(Date,WorkedDate) " +
                " = Convert(Date,@WorkedDate) And IsCancelled = 0 And IsRejected = 0");
            applicationId = context.Database.SqlQuery<string>(queryString.ToString(), new SqlParameter("@StaffId", StaffId),
                new SqlParameter("@WorkedDate", WorkedDate)).FirstOrDefault();
            if (string.IsNullOrEmpty(applicationId).Equals(true))
            {
                isCompOffAvailed = true;
            }
            return isCompOffAvailed;
        }
    }
}
