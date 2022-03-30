using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

namespace Attendance.Repository {
    public class OTApplicationRepository : IDisposable
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

        public OTApplicationRepository()
        {
            context = new AttendanceManagementContext();
        }

        public string SaveInformation(List<OTBulkUpload> OTP,string LogedInUser)
        {            
            var ReportingManager = string.Empty;
            var repo = new CommonRepository();
            SqlParameter[] sqlParameter = new SqlParameter[4];
            
            using (var trans = context.Database.BeginTransaction())
            {
                foreach (var lst in OTP)
                {
                    sqlParameter[0] = new SqlParameter("@ApprovedExtraHours", lst.ApprovedExtraHours);
                    sqlParameter[1] = new SqlParameter("@ConsiderExtraHoursFor", lst.ConsiderExtraHoursFor);
                    sqlParameter[2] = new SqlParameter("@LoggedInUser", LogedInUser);
                    sqlParameter[3] = new SqlParameter("@StaffId", lst.StaffId);

                    try
                    {
                        DateTime TxnDate = Convert.ToDateTime(lst.TransactionDate);
                        StringBuilder QryStr = new StringBuilder();
                        QryStr.Clear();
                        QryStr.Append("Update Attendancedata set ApprovedExtraHours = @ApprovedExtraHours , ConsiderExtraHoursFor = @ConsiderExtraHoursFor , ExtraHoursApprovedOn = Convert(varchar,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',23)  , ExtraHoursApprovedBy = @LoggedInUser where StaffId = @StaffId and Shiftindate = Convert(varchar,'" + TxnDate.ToString("yyyy-MM-dd HH:mm:ss") + "',23) ");
                        context.Database.ExecuteSqlCommand(QryStr.ToString(),sqlParameter);
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                trans.Commit();

            }
            return "OK";
        }

        public OTApplication GetOTApplicationDetails(string Id)
        {
            return context.OTApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }

        public ApplicationApproval GetApplicationApproval(string Id)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(Id)).FirstOrDefault();
        }

        public RequestApplication GetRequestApplicationDetails(string Id)
        {
            return context.RequestApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }

        public void SaveInformation(ClassesToSaveforOT ota , string SolidLine)
        {
            var DottedLine = string.Empty;
            var repo = new CommonRepository();

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                        
                        //SaveOTApplication(ota.OTP);
                        context.OTApplication.Add(ota.OTP);
                        context.ApplicationApproval.Add(ota.AA);

                    if (ota.AA.ApprovalOwner == ota.AA.ReviewerOwner)
                    {
                        UpdateOTInAttendance(ota.OTP.StaffId, Convert.ToDateTime(ota.OTP.OTDate).ToString("dd-MMM-yyyy"));
                    }

                    foreach (var l in ota.ESL)
                        //context.EmailSendLog.Add(l);
                        if (l.To != "-")
                            repo.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    context.SaveChanges();
                    trans.Commit();

                }
                catch (DbEntityValidationException e)
                {
                    string Message = "";
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Message=Message+"Entity of type "+eve.Entry.Entity.GetType().Name +" in state "+eve.Entry.State +"has the following validation errors:";
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Message = Message + "- Property: \"" + ve.PropertyName + "\", Error: \""+ve.ErrorMessage+"\"";
                        }
                    }
                    throw;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public void UpdateOTInAttendance(string StaffId, string ShiftDate)
        {
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            sqlParameter[1] = new SqlParameter("@ShiftDate", ShiftDate);
            var qryStr = new StringBuilder();
            qryStr.Append("update attendancedata set IsOTValid = 1 where staffid = @StaffId and ShiftinDate = @ShiftDate");
            context.Database.ExecuteSqlCommand(qryStr.ToString(), sqlParameter);
        }

        public void SaveOTApplication(OTApplication ota)
        {
            var lastid = string.Empty;
            if (string.IsNullOrEmpty(ota.Id))
            {
                var mr = new MasterRepository();
                lastid = mr.getmaxid( "OTApplication" , "id" , "OT" , "" , 10 , ref lastid );
                ota.Id = lastid;
                ota.InTime = ota.OTDuration;
                ota.OutTime = ota.OTDuration;
                ota.CreatedOn = DateTime.Now;
                ota.CreatedBy = ota.CreatedBy;
                ota.ModifiedOn = DateTime.Now;
                ota.ModifiedBy = ota.ModifiedBy;
            }
            else
            {
                ota.ModifiedOn = DateTime.Now;
                ota.ModifiedBy = ota.ModifiedBy;
            }

            context.OTApplication.AddOrUpdate(ota);
            context.SaveChanges();
        }



        public List<OTApplicationList> LoadOTApplications()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append( "SELECT Id,StaffId,FirstName,OTDate,OTTime,OTDuration,OTReason,CONVERT ( VARCHAR ,[StatusId] ) AS [StatusId] ,Status ,CreatedOn ,CreatedBy FROM [vwOTRequestList]" );
            try
            {
                var lst =
                    context.Database.SqlQuery<OTApplicationList>(qryStr.ToString()).Select(d => new OTApplicationList()
                    {
                        Id = d.Id,
                        StaffId = d.StaffId,
                        FirstName = d.FirstName,
                        OTDate = d.OTDate,
                        OTTime = d.OTTime,
                        OTDuration = d.OTDuration,
                        OTReason = d.OTReason,
                        StatusId = d.StatusId ,
                        Status = d.Status,
                        CreatedOn = d.CreatedOn,
                        CreatedBy =  d.CreatedBy
                    }).ToList();

                if (lst == null)
                {
                    return new List<OTApplicationList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<OTApplicationList>();
            }
        }

        public void ApproveApplication(ClassesToSaveforOT CTS, string ReportingManagerId, string StaffId)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            CommonRepository CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {

                    //Update the application approval table.
                    //context.Entry(CTS.AA).State = System.Data.Entity.EntityState.Modified;
                    context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("ReviewerstatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ReviewedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ReviewedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;
                    UpdateOTInAttendance(CTS.OTP.StaffId, Convert.ToDateTime(CTS.OTP.OTDate).ToString("dd-MMM-yyyy"));
                    foreach (var l in CTS.ESL)
                        //context.EmailSendLog.Add(l);
                        if (l.To != "-")
                            CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    context.SaveChanges();

                    //Insert into attendance control table
                    fromDate = Convert.ToDateTime(CTS.AA.ApplicationDate);
                    //toDate = Convert.ToDateTime(CTS.RA.EndDate);
                    //if (CTS.AA.ApprovalOwner == ReportingManagerId)
                    //{
                        if (fromDate <= currentDate)
                        {

                            CR.LogIntoIntoAttendanceControlTable(StaffId, fromDate, toDate, CTS.AA.ParentType, CTS.AA.Id);
                        }
                    //}
                    //
                    Trans.Commit();
                }
                catch (Exception e)
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }

        public void RejectApplication(ClassesToSave CTS , string StaffId)
        {
            var CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //context.Entry(CTS.AA).State = System.Data.Entity.EntityState.Modified;
                    context.Entry(CTS.AA).Property("ReviewerstatusId").IsModified = true; 
                    context.Entry(CTS.AA).Property("ReviewedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ReviewedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;  
                    context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;

                    //UpdateOTInAttendance(StaffId, Convert.ToDateTime(CTS.AA.ApplicationDate).ToString("dd-MMM-yyyy"));

                    foreach (var l in CTS.ESL)
                        //context.EmailSendLog.Add(l);
                        if (l.To != "-")
                            CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    //
                    context.SaveChanges();
                    Trans.Commit();
                }
                catch(Exception e)
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }

    }
}
