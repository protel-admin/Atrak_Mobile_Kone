using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
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

        public string SaveInformation(List<OTBulkUpload> OTP, string LogedInUser)
        {
            var ReportingManager = string.Empty;
            var repo = new CommonRepository();
            using (var trans = context.Database.BeginTransaction())
            {
                foreach (var lst in OTP)
                {
                    try
                    {
                        DateTime TxnDate = Convert.ToDateTime(lst.TransactionDate);
                        StringBuilder QryStr = new StringBuilder();
                        QryStr.Clear();
                        QryStr.Append("Update Attendancedata set ApprovedExtraHours = '" + lst.ApprovedExtraHours + "' , ConsiderExtraHoursFor = '" + lst.ConsiderExtraHoursFor + "' , ExtraHoursApprovedOn = Convert(varchar,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',23)  , ExtraHoursApprovedBy = '" + LogedInUser + "' where StaffId = '" + lst.StaffId + "' and Shiftindate = Convert(varchar,'" + TxnDate.ToString("yyyy-MM-dd HH:mm:ss") + "',23) ");
                        context.Database.ExecuteSqlCommand(QryStr.ToString());
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw e;
                    }
                }
                trans.Commit();
            }
            return "OK";
        }

        public void SaveInformation(ClassesToSaveforOT ota, string SolidLine)
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

                    if (ota.AA.ApprovalOwner == ota.AA.Approval2Owner)
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
                        Message = Message + "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + "has the following validation errors:";
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Message = Message + "- Property: \"" + ve.PropertyName + "\", Error: \"" + ve.ErrorMessage + "\"";
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

        public void SaveOTInformation(OTApplication ota, string LogedInUser)
        {
            var DottedLine = string.Empty;
           // var selfapproval = true;
            var repo = new CommonRepository();

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    if (string.IsNullOrEmpty(ota.Id) == true)
                    {
                        //
                        SaveOTApplication(ota);
                        //
                        DottedLine = repo.GetReportingManager(ota.StaffId);
                        //
                        repo.SaveIntoApplicationApproval(ota.Id, "OT", ota.StaffId, DottedLine, true);

                        UpdateOTInAttendance(ota.StaffId, Convert.ToDateTime(ota.OTDate).ToString("dd-MMM-yyyy"));
                    }
                    else
                    {
                        //
                        SaveOTApplication(ota);
                    }

                    trans.Commit();

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
            var qryStr = new StringBuilder();
            qryStr.Append("update attendancedata set IsOTValid = 1 where staffid = '" + StaffId + "' and ShiftinDate = '" + ShiftDate + "'");
            context.Database.ExecuteSqlCommand(qryStr.ToString());
        }

        public void SaveOTApplication(OTApplication ota)
        {
            var lastid = string.Empty;
            if (string.IsNullOrEmpty(ota.Id))
            {
                var mr = new MasterRepository();
                lastid = mr.getmaxid("OTApplication", "id", "OT", "", 10, ref lastid);
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
            qryStr.Append("SELECT Id,StaffId,FirstName,OTDate,OTTime,OTDuration,OTReason,CONVERT ( VARCHAR ,[StatusId] ) AS [StatusId] ,Status ,CreatedOn ,CreatedBy FROM [vwOTRequestList]");
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
                        StatusId = d.StatusId,
                        Status = d.Status,
                        CreatedOn = d.CreatedOn,
                        CreatedBy = d.CreatedBy
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
    }
}
