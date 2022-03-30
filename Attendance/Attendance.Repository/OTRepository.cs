using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class OTRepository : IDisposable
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
        public OTRepository()
        {

            context = new AttendanceManagementContext();
        }
        StringBuilder builder = new StringBuilder();
        public List<CostCentreList> GetCategory()
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT ID , NAME FROM Category WHERE ISACTIVE = 1");

            try
            {
                var lst = context.Database.SqlQuery<CostCentreList>(QryStr.ToString()).Select(d => new CostCentreList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<CostCentreList>();
                }
                else
                {
                    return lst;
                }
            }
            catch
            {
                return new List<CostCentreList>();
            }
        }

        public List<AttendanceDataView> GetOT(string StaffId, string fromdate, string todate, string CategoryId)
        {

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT STAFFID ,NAME as FirstName , TXNDATE as ShiftInDate, ShiftShortName ,ShiftInTime,ShiftOutTime, ActualInTime As InTime,  ActualOutTime as OutTime,ACTUALOTTIME As ActualOTTime FROM fnGetOT ( '" + StaffId + "','" + fromdate + "','" + todate + "','" + CategoryId + "')");

            try
            {
                var lstGrp = context.Database.SqlQuery<AttendanceDataView>(qryStr.ToString())
                        .Select(d => new AttendanceDataView()
                        {
                            //Id = d.Id,
                            STAFFID = d.STAFFID,
                            FirstName = d.FirstName,
                            ShiftInDate = d.ShiftInDate,
                            ActualOTTime = d.ActualOTTime,
                            ShiftShortName = d.ShiftShortName,
                            ShiftInTime = d.ShiftInTime,
                            ShiftOutTime = d.ShiftOutTime,
                            InTime = d.InTime,
                            OutTime = d.OutTime,
                           // NoOfEmpCount = d.StaffId.Count()
                        }).ToList();               
                if (lstGrp == null)
                {
                    return new List<AttendanceDataView>();
                }
                else
                {
                    if (lstGrp.Count == 0)
                    {
                        throw new Exception("No over time entries were found.");
                    }
                    else
                    {
                        return lstGrp;
                    }
                }

            }
            catch (Exception e)
            {
                return new List<AttendanceDataView>();
                throw e;
            }
        }

        public void SaveInformation(OTApplication ota, string ReportingManagerId)
        {
            var ReportingManager = string.Empty;
            bool selfapproval = false;
            var repo = new CommonRepository();

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    if (string.IsNullOrEmpty(ota.Id) == true)
                    {
                        //Save the information into OTApplication table
                        SaveOTApplication(ota);

                        //Get the reporting manager for the employee
                        ReportingManager = repo.GetReportingManager(ReportingManagerId);

                        if (string.IsNullOrEmpty(ReportingManager) == true)
                        {
                            // Set the employee as reporting manager and approval type to auto
                            ReportingManager = ota.StaffId;
                            selfapproval = true;
                        }
                        //Save the application in Application approval
                        repo.SaveIntoApplicationApproval(ota.Id, "OT", ota.StaffId, ReportingManager, selfapproval);
                    }
                    else
                    {
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

        public void SaveOTApplication(OTApplication ota)
        {
            var lastid = string.Empty;
            if (string.IsNullOrEmpty(ota.Id))
            {
                var mr = new MasterRepository();
                lastid = mr.getmaxid("OTApplication", "id", "OT", "", 10, ref lastid);
                ota.Id = lastid;
                ota.CreatedOn = DateTime.Now;
                ota.CreatedBy = "-";
                ota.ModifiedOn = DateTime.Now;
                ota.ModifiedBy = "-";
            }
            else
            {
                ota.ModifiedOn = DateTime.Now;
                ota.ModifiedBy = "-";
            }

            context.OTApplication.AddOrUpdate(ota);
            context.SaveChanges();
        }


        public string ValidateApplication(string StaffId, string FromDate, string ToDate)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select dbo.fnValidateOverTimeApplication('" + StaffId + "','" + FromDate + "','" + ToDate + "')");

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



        public void SaveOTApplicationEntry(string Staffid, string FromDate, string ToDate, string Createdby)
        {

            string output = Staffid.Remove(Staffid.Length - 1, 1);

            foreach (string id in output.Split(',').ToList())
            {
                var QRY = new StringBuilder();
                QRY.Clear();
                QRY.Append("insert into OTApplicationEntry values('" + id + "',dbo.fngetstaffname('" + id + "'),'" + FromDate + "','" + ToDate + "',GetDate(),'" + Createdby + "') ");
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {

                        var str = context.Database.ExecuteSqlCommand(QRY.ToString());


                        trans.Commit();

                    }

                    catch (Exception)
                    {
                        trans.Rollback();

                        throw;
                    }
                }

            }


        }

        public List<OTApplicationEntryList> GetAllOT()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT Id,staffid,staffname,convert(varchar(12) ,convert(datetime,fromdate,101))as fromdate,convert(varchar(12) ,convert(datetime,todate,101)) as todate,convert(varchar(12) ,convert(datetime,createdon,101)) as createdon  from OTApplicationEntry order by fromdate desc");

            try
            {
                var lstGrp =
                    context.Database.SqlQuery<OTApplicationEntryList>(qryStr.ToString())
                        .Select(d => new OTApplicationEntryList()
                        {
                            Id = d.Id,
                            StaffId = d.StaffId,
                            StaffName = d.StaffName,
                            FromDate = d.FromDate,
                            ToDate = d.ToDate,
                            CreatedOn = d.CreatedOn

                        }).ToList();

                if (lstGrp == null)
                {
                    return new List<OTApplicationEntryList>();
                }
                else
                {
                    if (lstGrp.Count == 0)
                    {
                        throw new Exception("No over time entries were found.");
                    }
                    else
                    {
                        return lstGrp;
                    }
                }

            }
            catch (Exception)
            {
                return new List<OTApplicationEntryList>();
                throw;
            }
        }

        public EmpData GetEmployeeDetails(string StaffId)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT DBO.FNGETSTAFFNAME(STAFFID) AS StaffName , DEPTNAME AS DepartmentName FROM STAFFVIEW WHERE STAFFID = '" + StaffId + "'");

            var data = context.Database.SqlQuery<EmpData>(qryStr.ToString()).FirstOrDefault();

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
        #region OT and Coff Approval (Extra hours Approvals)
        public string SaveOT_COFF_DetailsRepository(List<OTData> lst, string CreatedBy, string IsOTorCoff)
        {
            string Result = string.Empty;
            using (DbContextTransaction trans = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var data in lst)
                    {
                        SqlParameter[] Param = new SqlParameter[5];
                        Param[0] = new SqlParameter("@StaffId", data.StaffId);
                        Param[1] = new SqlParameter("@TotalOTHours", data.TotalOTHours);
                        Param[2] = new SqlParameter("@OTDate", data.OTDate);
                        Param[3] = new SqlParameter("@CreatedBy", CreatedBy);
                        Param[4] = new SqlParameter("@IsOTorCoff", IsOTorCoff);

                        builder.Append("Update Attendancedata set ApprovedExtraHours = @TotalOTHours ,ConsiderExtraHoursFor= @IsOTorCoff, ExtraHoursApprovedOn = getdate()  , ExtraHoursApprovedBy = @CreatedBy  where StaffId = @StaffId and Convert(date,Shiftindate) = Convert(date,@OTDate)");
                        context.Database.ExecuteSqlCommand(builder.ToString(), Param);

                        //if (IsOTorCoff == "COFF")
                        //{
                        //    RACoffCreditApplicationRepository BL = new RACoffCreditApplicationRepository();
                        //    RequestApplication reqData = new RequestApplication();

                        //    double seconds = TimeSpan.Parse(data.TotalOTHours).TotalSeconds;

                        //    if (seconds >= 14400 && seconds <= 27324)
                        //    {
                        //        reqData.TotalDays = Convert.ToDecimal("0.5");
                        //    }
                        //    else if (seconds >= 27324 && seconds <= 43199)
                        //    {
                        //        reqData.TotalDays = Convert.ToDecimal("1.00");
                        //    }
                        //    else if (seconds >= 43200 && seconds <= 57599)
                        //    {
                        //        reqData.TotalDays = Convert.ToDecimal("1.50");
                        //    }
                        //    else if (seconds >= 57600 && seconds <= 72000)
                        //    {
                        //        reqData.TotalDays = Convert.ToDecimal("2.00");
                        //    }

                        //    reqData.Id = BL.GetUniqueId();
                        //    reqData.StaffId = data.StaffId;
                        //    reqData.LeaveTypeId = "LV0005";
                        //    reqData.LeaveStartDurationId = 0;
                        //    reqData.StartDate = Convert.ToDateTime(data.OTDate);
                        //    reqData.EndDate = Convert.ToDateTime(data.OTDate);
                        //    reqData.LeaveEndDurationId = 0;
                        //    reqData.Remarks = "Coff based on Extra hours worked";
                        //    reqData.ApplicationDate = DateTime.Now;
                        //    reqData.AppliedBy = CreatedBy;
                        //    reqData.IsCancelled = false;
                        //    reqData.IsApproved = true;
                        //    reqData.IsRejected = false;
                        //    reqData.RequestApplicationType = "CR";
                        //    reqData.IsCancelApprovalRequired = false;
                        //    reqData.IsCancelApproved = false;
                        //    reqData.IsCancelRejected = false;
                        //    reqData.ExpiryDate = DateTime.Now;
                        //    reqData.WorkedDate = DateTime.Now;
                        //    context.RequestApplication.Add(reqData);
                        //    context.SaveChanges();

                        //    ApplicationApproval apprData = new ApplicationApproval();
                        //    apprData.Id = BL.GetUniqueId();
                        //    apprData.ParentId = reqData.Id;
                        //    apprData.ApprovalStatusId = 2;
                        //    apprData.ApprovedBy = CreatedBy;
                        //    apprData.ApprovedOn = reqData.ApplicationDate;
                        //    apprData.Comment = "APPROVED THE COFF REQUEST";
                        //    apprData.ApprovalOwner = CreatedBy;
                        //    apprData.ParentType = "CR";
                        //    apprData.ForwardCounter = 1;
                        //    apprData.ApplicationDate = reqData.ApplicationDate;
                        //    apprData.Approval2statusId = 2;
                        //    apprData.Approval2Owner = CreatedBy;
                        //    apprData.Approval2On = reqData.ApplicationDate;
                        //    apprData.Approval2By = CreatedBy;
                        //    context.ApplicationApproval.Add(apprData);
                        //    context.SaveChanges();

                        //    EmployeeLeaveAccount leaveData = new EmployeeLeaveAccount();
                        //    leaveData.StaffId = data.StaffId;
                        //    leaveData.LeaveTypeId = "LV0005";
                        //    leaveData.TransactionFlag = 1;
                        //    leaveData.TransactionDate = reqData.ApplicationDate;
                        //    leaveData.LeaveCount = Convert.ToDecimal(reqData.TotalDays);
                        //    leaveData.Narration = "Coff based on Extra hours worked";
                        //    leaveData.LeaveCreditDebitReasonId = 28;
                        //    leaveData.TransctionBy = CreatedBy;
                        //    leaveData.Year = DateTime.Now.Year;
                        //    leaveData.Month = DateTime.Now.Month;
                        //    context.EmployeeLeaveAccount.Add(leaveData);
                        //    context.SaveChanges();

                        //    ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 360;
                        //}
                    }

                    trans.Commit();
                    Result = "OK";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }
            return Result;
        }
        #endregion
    }
}
















