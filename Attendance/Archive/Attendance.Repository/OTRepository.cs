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
using System.Security;
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
        public List<CostCentreList> GetCostCentre()
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT ID , NAME FROM COSTCENTRE WHERE ISACTIVE = 1");

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

        //public List<AttendanceDataView> GetOT(string StaffId, string fromdate, string todate, string CostCentreId)
        //{

        //    var qryStr = new StringBuilder();
        //    qryStr.Clear();
        //    qryStr.Append(" Exec  [dbo].[GetOTForApproval]  '" + StaffId + "','" + fromdate + "','" + todate + "'");
        //    //qryStr.Append("SELECT  STAFFID ,NAME as FirstName , TXNDATE as ShiftInDate , ActualInTime As InTime,  ActualOutTime as OutTime,ACTUALOTTIME As ActualOTTime FROM fnGetOT ( '" + StaffId + "','" + fromdate + "','" + todate + "')");

        //    try
        //    {
        //        var lstGrp = context.Database.SqlQuery<AttendanceDataView>(qryStr.ToString())
        //                .Select(d => new AttendanceDataView()
        //                {
        //                    //Id = d.Id,
        //                    StaffId = d.StaffId,
        //                    FirstName = d.FirstName,
        //                    ShiftInDate = d.TXNDATE,
        //                    ActualOTTime = d.ActualOTTime,
        //                    ShiftShortName = d.ShiftShortName,
        //                    ShiftInTime = d.ShiftInTime,
        //                    ShiftOutTime = d.ShiftOutTime,
        //                    InTime = d.ActualInTime,
        //                    OutTime = d.ActualOutTime
        //                }).ToList();

        //        if (lstGrp == null)
        //        {
        //            return new List<AttendanceDataView>();
        //        }
        //        else
        //        {
        //            if (lstGrp.Count == 0)
        //            {
        //                throw new Exception("No over time entries were found.");
        //            }
        //            else
        //            {
        //                return lstGrp;
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        return new List<AttendanceDataView>();
        //        throw;
        //    }
        //}

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
            SqlParameter[] sqlParameter = new SqlParameter[3];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            sqlParameter[1] = new SqlParameter("@FromDate", FromDate);
            sqlParameter[2] = new SqlParameter("@ToDate", ToDate);

            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select dbo.fnValidateOverTimeApplication(@StaffId,@FromDate,@ToDate)");

            try
            {
                var str = context.Database.SqlQuery<string>(QryStr.ToString(),sqlParameter).FirstOrDefault();
                return str;
            }
            catch (Exception err)
            {
                return "ERROR!" + err.Message;
            }
        }



        public void SaveOTApplicationEntry(string Staffid,string FromDate, string ToDate, string Createdby)
        {

            SqlParameter[] sqlParameter = new SqlParameter[4];
            sqlParameter[0] = new SqlParameter("@FromDate", FromDate);
            sqlParameter[1] = new SqlParameter("@ToDate", ToDate);
            sqlParameter[2] = new SqlParameter("@Createdby", Createdby);
            

            string output = Staffid.Remove(Staffid.Length - 1, 1);

            foreach (string id in output.Split(',').ToList())       
                {
                sqlParameter[3] = new SqlParameter("@id", id);
                                var  QRY = new StringBuilder();
                        QRY.Clear();
                        QRY.Append("insert into OTApplicationEntry values(@id,dbo.fngetstaffname(@id),@FromDate,@ToDate,GetDate(),@Createdby) ");
                        using (var trans = context.Database.BeginTransaction())
                        {
                            try
                            {

                                var str = context.Database.ExecuteSqlCommand(QRY.ToString(),sqlParameter);
                

                                trans.Commit();
                   
                            }
               
                            catch (Exception )
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
                            StaffName=d.StaffName,
                            FromDate=d.FromDate,
                            ToDate=d.ToDate,
                            CreatedOn=d.CreatedOn
                                                       
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
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT DBO.FNGETSTAFFNAME(STAFFID) AS StaffName , DEPTNAME AS DepartmentName FROM STAFFVIEW WHERE STAFFID = @StaffId");

            var data = context.Database.SqlQuery<EmpData>(qryStr.ToString(),sqlParameter).FirstOrDefault();

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
        public List<AttendanceDataViewNewModel> GetOTRepository(int Flag, string StaffList, string fromdate, string todate)
        {
            var qryStr = new StringBuilder();
            qryStr.Append("EXEC [dbo].[spGetOTHoursV1] @Flag ,@StaffList, @FromDate ,@ToDate");
            try
            {
                context.Database.CommandTimeout = 0;
                var lstGrp = context.Database.SqlQuery<AttendanceDataViewNewModel>(qryStr.ToString(), new SqlParameter("@Flag", Flag), new SqlParameter("@StaffList", StaffList), new SqlParameter("@FromDate", fromdate), new SqlParameter("@ToDate", todate))
                        .Select(d => new AttendanceDataViewNewModel()
                        {
                            Id = d.Id,
                            StaffId = d.StaffId,
                            Name = d.Name,
                            Department = d.Department,
                            TxnDate = d.TxnDate,
                            ShiftName = d.ShiftName,
                            ActualIn = d.ActualIn,
                            ActualOut = d.ActualOut,
                            TotalHoursWorked = d.TotalHoursWorked,
                            Txn_DayName = d.Txn_DayName,
                            AccountedOTTime = d.AccountedOTTime,
                            ActualOTTime = d.ActualOTTime,
                            OTDuration = d.OTDuration,
                            IsOTExceed = d.IsOTExceed,
                            QuarterStart = d.QuarterStart,
                            QuarterEnd = d.QuarterEnd,
                            TotalOTInQuarter = d.TotalOTInQuarter,
                            IsOTTimeProcessed = d.IsOTTimeProcessed
                        }).ToList();

                if (lstGrp == null)
                {
                    return new List<AttendanceDataViewNewModel>();
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
                return new List<AttendanceDataViewNewModel>();
            }
        }
        public void UpdateAttendanceData(string StaffId, string ShiftInDate, RequestApplication RA, ApplicationApproval AA)
        {
            //ApplicationEscalationHistory AEH = new ApplicationEscalationHistory();
            //ApplicationEmailEscalateHistory AEEH = new ApplicationEmailEscalateHistory();
            //AEH.IsActive = true;
            //AEH.RecordDateTime = DateTime.Now;
            //AEH.ActionDateTime = DateTime.Now;
            //AEH.ApprovalStatusId = 3;
            //AEH.ApplicationId = RA.Id;
            //AEH.Comments = "Application rejected on Initial stage by" + RA.AppliedBy;
            //AEEH.IsActive = true;
            //AEEH.RecordDateTime = DateTime.Now;
            //AEEH.ActionDateTime = DateTime.Now;
            //AEEH.IsMailSent = false;
            //AEEH.ApplicationId = RA.Id;
            //AEEH.Comments = "Application rejected on Initial stage by" + RA.AppliedBy;

            //using (var Trans = context.Database.BeginTransaction())
            //{
            //    try
            //    {
            //        context.RequestApplication.Add(RA);
            //        context.ApplicationApproval.Add(AA);
            //        context.ApplicationEscalationHistory.Add(AEH);
            //        context.ApplicationEmailEscalateHistory.Add(AEEH);
            //        context.SaveChanges();
            //        Trans.Commit();
            //    }
            //    catch (Exception e)
            //    {
            //        Trans.Rollback();
            //        throw e;
            //    }
            //}
            var QRY = new StringBuilder();
            QRY.Append("update AttendanceData set IsOT = 0 where StaffId=@StaffId and convert(date,ShiftInDate) = Convert(date,@ShiftInDate) ");
            try
            {
                var str = context.Database.ExecuteSqlCommand(QRY.ToString(), new SqlParameter("@StaffId", StaffId), new SqlParameter("@ShiftInDate", ShiftInDate));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SubmitRequestApplication(ClassesToSave DataToSave)
        {
            //DateTime fromDate = DateTime.Now;
            //DateTime toDate = DateTime.Now;
            //DateTime currentDate = DateTime.Now;
            //CommonRepository CR = new CommonRepository();
            ////context.Entry(DataToSave).State = System.Data.Entity.EntityState.Added;

            //using (var Trans = context.Database.BeginTransaction())
            //{
            //    try
            //    {
            //        // save to request application table.
            //        DataToSave.RA.IsApproved = false;
            //        context.RequestApplication.Add(DataToSave.RA);
            //        context.SaveChanges();
            //        Trans.Commit();
            //    }
            //    catch (System.Data.Entity.Validation.DbEntityValidationException err)
            //    {
            //        Trans.Rollback();
            //        throw err;
            //    }
            //}

            //ApplicationEscalationHistory AEH = new ApplicationEscalationHistory();
            //ApplicationEmailEscalateHistory AEEH = new ApplicationEmailEscalateHistory();
            //AEH.IsActive = true;
            //AEH.RecordDateTime = DateTime.Now;
            //AEH.ActionDateTime = DateTime.Now;
            //AEH.ApprovalStatusId = 1;
            //AEH.ApplicationId = DataToSave.RA.Id;
            //AEH.Comments = "Application Applied for approval by" + DataToSave.RA.AppliedBy;
            //AEEH.IsActive = true;
            //AEEH.RecordDateTime = DateTime.Now;
            //AEEH.ActionDateTime = DateTime.Now;
            //AEEH.IsMailSent = false;
            //AEEH.ApplicationId = DataToSave.RA.Id;
            //AEEH.Comments = "Application Applied for approval by" + DataToSave.RA.AppliedBy;
            //DataToSave.AA.ForwardCounter = 1;
            //DataToSave.AA.ApprovalStatusId = 1;

            //if (DataToSave.RA.StaffId != DataToSave.RA.AppliedBy)
            //{
            //    try
            //    {
            //        var lst = new List<HierarchyFlow>();
            //        try
            //        {
            //            lst = context.HierarchyFlow.Where(x => x.RootEmpId == DataToSave.RA.StaffId && x.IsActive == true && x.IsOTApprover == true).ToList();
            //        }
            //        catch (Exception e)
            //        {
            //            lst = null;
            //        }
            //        if (lst != null)
            //        {
            //            if (lst.Count == 3)
            //            {
            //                using (var Trans = context.Database.BeginTransaction())
            //                {
            //                    try
            //                    {
            //                        AEH.ReviewOwnerId = lst[1].EmpId;
            //                        AEEH.ReviewOwnerId = lst[1].EmpId;
            //                        DataToSave.AA.ApprovalOwner = lst[1].EmpId;
            //                        context.ApplicationEscalationHistory.Add(AEH);
            //                        context.ApplicationEmailEscalateHistory.Add(AEEH);
            //                        context.ApplicationApproval.Add(DataToSave.AA);
            //                        context.SaveChanges();
            //                        Trans.Commit();
            //                    }
            //                    catch (System.Data.Entity.Validation.DbEntityValidationException err)
            //                    {
            //                        Trans.Rollback();
            //                        throw err;
            //                    }
            //                }
            //            }
            //            else if (lst.Count == 2)
            //            {
            //                using (var Trans = context.Database.BeginTransaction())
            //                {
            //                    try
            //                    {
            //                        AEH.ReviewOwnerId = lst[0].EmpId;
            //                        AEEH.ReviewOwnerId = lst[0].EmpId;
            //                        DataToSave.AA.ApprovalOwner = lst[0].EmpId;
            //                        //context.ApplicationEscalationHistory.Add(AEH);
            //                        //context.ApplicationEmailEscalateHistory.Add(AEEH);
            //                        context.ApplicationApproval.Add(DataToSave.AA);
            //                        context.SaveChanges();
            //                        Trans.Commit();
            //                    }
            //                    catch (System.Data.Entity.Validation.DbEntityValidationException err)
            //                    {
            //                        Trans.Rollback();
            //                        throw err;
            //                    }
            //                }
            //            }
            //            else if (lst.Count == 1)
            //            {
            //                if (lst[0].EmpId == DataToSave.RA.AppliedBy)
            //                {
            //                    using (var Trans = context.Database.BeginTransaction())
            //                    {
            //                        try
            //                        {
            //                            AEH.ReviewOwnerId = lst[0].EmpId;
            //                            AEEH.ReviewOwnerId = lst[0].EmpId;
            //                            DataToSave.AA.ApprovalOwner = lst[0].EmpId;
            //                            //context.ApplicationEscalationHistory.Add(AEH);
            //                            //context.ApplicationEmailEscalateHistory.Add(AEEH);
            //                            context.ApplicationApproval.Add(DataToSave.AA);
            //                            context.SaveChanges();
            //                            Trans.Commit();
            //                        }
            //                        catch (System.Data.Entity.Validation.DbEntityValidationException err)
            //                        {
            //                            Trans.Rollback();
            //                            throw err;
            //                        }
            //                    }

            //                    var queryString = new StringBuilder();
            //                    queryString.Clear();
            //                    queryString.Append("exec [DBO].[HandleApprovedRejectedApplicationsWEB] '" + DataToSave.RA.Id + "','" + DataToSave.RA.AppliedBy + "','2'");
            //                    var data = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();
            //                }
            //                else
            //                {
            //                    using (var Trans = context.Database.BeginTransaction())
            //                    {
            //                        try
            //                        {
            //                            AEH.ReviewOwnerId = lst[0].EmpId;
            //                            AEEH.ReviewOwnerId = lst[0].EmpId;
            //                            DataToSave.AA.ApprovalOwner = lst[0].EmpId;
            //                            //context.ApplicationEscalationHistory.Add(AEH);
            //                            //context.ApplicationEmailEscalateHistory.Add(AEEH);
            //                            context.ApplicationApproval.Add(DataToSave.AA);
            //                            context.SaveChanges();
            //                            Trans.Commit();
            //                        }
            //                        catch (System.Data.Entity.Validation.DbEntityValidationException err)
            //                        {
            //                            Trans.Rollback();
            //                            throw err;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {

            //        }
            //    }
            //    catch (System.Data.Entity.Validation.DbEntityValidationException err)
            //    {
            //        throw err;
            //    }
            //}
        }
        public string SaveOT_COFF_DetailsRepository(List<OTData> lst, string CreatedBy,string IsOTorCoff)
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
    }
}

          
        








    

    


