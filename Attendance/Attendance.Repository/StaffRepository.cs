using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Data.Entity.Validation;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class StaffRepository : TrackChangeRepository,IDisposable
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

        public StaffRepository()
        {
            context = new AttendanceManagementContext();
        }
        StringBuilder builder = new StringBuilder();
        string Message = string.Empty;
        public string GetUniqueId()
        {
            return context.Database.SqlQuery<string>("select convert(varchar,getdate(),112) + replace" +
                "(convert(varchar,getdate(),114),':','')").First();
        }
        public string GetCoffReqPeriodRepository()
        {
            Message = context.Settings.Where(condition => condition.Parameter == "COffReqPeriod").Select(select => select.Value).FirstOrDefault();
            return Message;
        }


 public StaffOfficialInformationForApi GetStaffOfficialInformationForApi(string staffid)
        {


            var newQry = $@"Select StaffId 
                 ,c.Name as CompanyName
                 ,d.Name as DesignationName
                 ,l.Id as LocationId,l.Name as LocationName
                 , dep.Name as DepartmentName
                 , grd.Name as GradeName
                 , div.Name as DivisionName
                 , (select email from StaffOfficial where StaffId = A.ReportingManager ) as ReportingManagerEmailId
                 , dbo.fnGetStaffName(StaffId) as UserFullName
                 , CompanyId , BranchId , DepartmentId , DivisionId ,VolumeId , DesignationId , GradeId 
                 , LeaveGroupId , WeeklyOffId , HolidayGroupId , PolicyId, Canteen, Travel
                 , SalaryDay, IsConfirmed, convert(varchar,ConfirmationDate) as ConfirmationDate 
                 , IsWorkingDayPatternLocked, IsLeaveGroupLocked, IsHolidayGroupLocked, IsWeeklyOffLocked
                 , IsPolicyLocked,  convert(varchar,DateOfJoining) as DateOfJoining 
                 ,convert(varchar,ResignationDate) as ResignationDate ,convert(varchar, DateOfRelieving) as DateOfRelieving 
                 , a.Phone , a.Fax , a.Email , a.ExtensionNo , a.Interimhike , a.Tenure , a.PFNo ,a.ESINo ,CategoryId 
                 ,CostCentreId ,LocationId , SecurityGroupId as UserRoleId , WorkingDayPatternId 
                 , ReportingManager as ReportingManagerId,b.FirstName as ReportingManagerName
                 , a.Approver2 as Reviewer
                 , DomainId  
                 , g.name as UserRole  
                  from staffofficial  a left  join staff b on a.reportingmanager = b.id  
                 JOIN  SecurityGroup g on g.id= a.SecurityGroupId  
                 JOIN  designation d on d.id= a.designationId
                 JOIN location l on l.id=a.locationId
                 JOIN department dep on dep.id= a.departmentId
                 JOIN grade grd on grd.id=a.gradeId
                 JOIN division div on div.id= a.divisionId
                 JOIN company  c on c.id=a.companyId
                  where staffid = '{staffid}'";
            try
            {
                var so =
                   context.Database.SqlQuery<StaffOfficialInformationForApi>(newQry).FirstOrDefault();
                // context.Database.SqlQuery<StaffOfficialInformationForApi>(qryStr.ToString()).FirstOrDefault();
                // context.StaffOfficialInformationForApi.FromSqlRaw(qryStr.ToString()).FirstOrDefault();



                return so;

            }
            catch (Exception e)
            {

                throw e;
            }
        }




        public void SaveRequestApplication(ClassesToSave DataToSave)
        {
            //context.Entry(DataToSave).State = System.Data.Entity.EntityState.Added;

            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    // save to request application table.
                    context.RequestApplication.Add(DataToSave.RA);
                    // save to application approval table.
                    CommonRepository CR = new CommonRepository();
                    context.ApplicationApproval.Add(DataToSave.AA);
                    // save to email send log table.


                    foreach (var l in DataToSave.ESL)
                    {
                        if (l.To != "-")
                            CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                    //context.EmailSendLog.Add(l);


                    // context.Testing.Add(DataToSave.Test);

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

        public string GetTotalDaysLeave(string StaffId, string LeaveStartDurationId, string FromDate, string ToDate, string LeaveEndDurationId, string LeaveTypeId)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("Select dbo.fnGetTotalDaysLeave(@StaffId,@LeaveStartDurationId,@FromDate,@ToDate,@LeaveEndDurationId,'"+ LeaveTypeId + "')");

            try
            {
                var data = context.Database.SqlQuery<string>(QryStr.ToString(),new SqlParameter("@StaffId", StaffId)
                    , new SqlParameter("@LeaveStartDurationId", LeaveStartDurationId), new SqlParameter("@FromDate", FromDate)
                    , new SqlParameter("@ToDate", ToDate), new SqlParameter("@LeaveEndDurationId", LeaveEndDurationId)).FirstOrDefault();

                if (string.IsNullOrEmpty(data) == true)
                {
                    return "EMPTY";
                }
                else
                {
                    return data;
                }
            }
            catch 
            {
                return "ERROR";
            }
        }
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
        #region Bulk Shift Import
        public List<string> GetActiveShiftRepository()
        {
            List<string> list = new List<string>();
            try
            {
                list = context.Shifts.Where(condition => condition.IsActive == true).Select(select => select.Name).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
        public string GetShiftNameRepository(string ShiftName)
        {
            string Valid = "0";
            try
            {
                string validShiftName = context.Shifts.Where(condition => condition.Name.Contains(ShiftName) || condition.ShortName.Contains(ShiftName)).Select(select => select.Id).FirstOrDefault();
                if (string.IsNullOrEmpty(validShiftName).Equals(true))
                {
                    Valid = "0";
                }
                else
                {
                    Valid = validShiftName;
                }
            }
            catch
            {
                Valid = "0";
            }
            return Valid;
        }
        public string SaveBulkShiftsRepository(List<BulkShiftImportModel> model, string CreatedBy)
        {
            using (DbContextTransaction trans = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var data in model)
                    {
                        string ShiftId = string.Empty;
                        ShiftsImportData tbl = new ShiftsImportData();
                        tbl.ShiftId = data.ShiftName;
                        tbl.StaffId = data.StaffId;
                        tbl.ShiftFromDate = Convert.ToDateTime(data.FromDate);
                        tbl.ShiftToDate = Convert.ToDateTime(data.ToDate);
                        tbl.IsProcessed = false;
                        tbl.CreatedOn = DateTime.Now;
                        tbl.CreatedBy = CreatedBy;

                        context.ShiftsImportData.Add(tbl);
                        context.SaveChanges();

                        if (Convert.ToDateTime(data.FromDate).Date < DateTime.Now.Date)
                        {
                            AttendanceControlTable process = new AttendanceControlTable();
                            process.StaffId = data.StaffId;
                            process.FromDate = Convert.ToDateTime(data.FromDate);
                            process.ToDate = DateTime.Now.AddDays(-1);
                            process.IsProcessed = false;
                            process.CreatedOn = DateTime.Now;
                            process.CreatedBy = CreatedBy;
                            process.ApplicationType = "BulkShiftImport";

                            context.AttendanceControlTable.Add(process);
                            context.SaveChanges();
                        }
                    }
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    Message = e.Message;
                }
                string Check = "true";
                Check = context.Settings.Where(condition => condition.Parameter == "BulkShiftsImportData" && condition.IsActive == true).Select(select => select.Value).FirstOrDefault();
                if (Check != null && Check == "1")
                {
                    builder = new StringBuilder();
                    builder.Append("Exec GetBulkShiftsImportData");
                    ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 360;
                    context.Database.ExecuteSqlCommand(builder.ToString());
                }
                Message = "success";
            }
            return Message;
        }
        #endregion
        public StaffInformation GetStaffMainInformation(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Append("Select Id , StaffStatusId , CardCode , FirstName , MiddleName , LastName , " +
                          "ShortName , Gender, SalutationId , IsHidden , CreatedOn , CreatedBy from Staff " +
                          "where id = '"+staffid+"'");
            try
            {
                var si = context.Database.SqlQuery<StaffInformation>(qryStr.ToString()).Select(d => new StaffInformation()
                {
                    PageStaffId = d.Id,
                    Id = d.Id,
                    StaffStatusId = d.StaffStatusId,
                    CardCode = d.CardCode,
                    FirstName = d.FirstName,
                    MiddleName = d.MiddleName,
                    LastName = d.LastName,
                    ShortName = d.ShortName,
                    Gender = d.Gender,
                    SalutationId = d.SalutationId,
                    CreatedOn = d.CreatedOn,
                    CreatedBy = d.CreatedBy,
                    ModifiedOn = d.ModifiedOn,
                    ModifiedBy = d.ModifiedBy,
                    IsHidden = d.IsHidden
                }).FirstOrDefault();

                if (si == null)
                {
                    return new StaffInformation();
                }
                else
                {
                    return si;
                }
            }
            catch 
            {
                return new StaffInformation();
            }
        }

        public StaffOfficialInformation GetStaffOfficialInformation(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Append(" Select  Approver2 ,dbo.fngetstaffname(Approver2) as Approver2Name,StaffId , CompanyId , BranchId , DepartmentId , DivisionId ,VolumeId , " +
                          "DesignationId , GradeId , LeaveGroupId , WeeklyOffId , HolidayGroupId , PolicyId, Canteen, Travel, SalaryDay, IsConfirmed,IsEarlyConfirmation ,IsLateConfirmation, " +
                          "ConfirmationDate, IsWorkingDayPatternLocked, IsLeaveGroupLocked, IsHolidayGroupLocked, " +
                          "IsWeeklyOffLocked, IsPolicyLocked, " + "DateOfJoining , ResignationDate , DateOfRelieving , Phone , Fax , Email , ExtensionNo , Interimhike , Convert(varchar(10),Tenure) as Tenure , PFNo ," +
                          "ESINo ,CategoryId ,CostCentreId ,LocationId , SecurityGroupId , WorkingDayPatternId, ReportingManager ," +
                          "b.FirstName as ReportingManagerName , DomainId ,ShiftId,ShiftPatternId,WorkStationId,ApproverLevel," +
                          " CASE WHEN AutoShift = 1 THEN 'Autoshift' ELSE '0' END AS AutoShift, " +
                          "CASE WHEN GeneralShift = 1 THEN 'Generalshift' ELSE '0' END AS GeneralShift, " +
                          "CASE WHEN ShiftPattern = 1 THEN 'Shiftpattern' ELSE '0' END AS ShiftPattern, " +
                          "CASE WHEN ManualShift = 1 THEN 'ManualShift' ELSE '0' END AS ManualShift, " +
                          "CASE WHEN IsFlexi = 1 THEN 'FlexiShift' ELSE '0' END AS FlexiShift " +
                          " from staffofficial  a left join staff b on a.reportingmanager = b.id where staffid = '"+staffid+"'");
            try
            {
                var so = context.Database.SqlQuery<StaffOfficialInformation>(qryStr.ToString()).FirstOrDefault();

                if (so == null)
                {
                    return new StaffOfficialInformation();
                }
                else
                {
                    return so;
                }
            }
            catch (Exception e)
            {
                return new StaffOfficialInformation();
                throw e;
            }
        }

        public StaffOfficialInformation GetstaffShiftPlanvalue(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            
            qryStr.Append(" Select  Approver2 ,dbo.fngetstaffname(Approver2) as Approver2Name,a.StaffId , CompanyId , BranchId , DepartmentId , DivisionId ,VolumeId ,      " +
                          "  DesignationId, GradeId, LeaveGroupId, ESP.WeeklyOffId, HolidayGroupId, PolicyId, Canteen, Travel, SalaryDay, IsConfirmed,                      " +
                          "  ConfirmationDate, IsLeaveGroupLocked, IsHolidayGroupLocked,                                                         " +
                          "  IsPolicyLocked, DateOfJoining, ResignationDate, DateOfRelieving, Phone, Fax, Email, ExtensionNo, Interimhike, Tenure, PFNo, " +
                          "  ESINo, CategoryId, CostCentreId, LocationId, SecurityGroupId, ReportingManager,                                           " +
                          "  b.FirstName as ReportingManagerName, DomainId, ESP.ShiftId,Esp.PatternId ShiftPatternId, WorkStationId,                                                     " +
                          "  CASE WHEN ESP.IsAutoShift = 1 THEN 'Autoshift' ELSE '0' END AS AutoShift,                                                                      " +
                          "  CASE WHEN ESP.IsGeneralShift = 1 THEN 'Generalshift' ELSE '0' END AS GeneralShift,                                                             " +
                          "  CASE WHEN ESP.IsWeekPattern = 1 THEN 'Shiftpattern' ELSE '0' END AS ShiftPattern,                                                             " +
                          "  CASE WHEN ESP.IsManualShift = 1 THEN 'ManualShift' ELSE '0' END AS ManualShift,                                                                " +
                          "  CASE WHEN ESP.IsFlexiShift = 1 THEN 'FlexiShift' ELSE '0' END AS FlexiShift, ESP.isactive,                                                      " +
                          "  ESP.UseDayPattern as IsWorkingDayPatternLocked,ESP.UseWeeklyOff as IsWeeklyOffLocked, ESP.DayPatternId as WorkingDayPatternId                                                                                                           " +
                          "  from staffofficial a left                                                                                                                      " +
                          "  join staff b on a.reportingmanager = b.id  inner                                                                                               " +
                          "  join EmployeeShiftPlan Esp on a.staffid = Esp.staffid                                                                                          " +
                          "  where a.StaffId = '" + staffid + "' and ESP.isactive = '1'");

            try
            {
                var so = context.Database.SqlQuery<StaffOfficialInformation>(qryStr.ToString()).FirstOrDefault();

                if (so == null)
                {
                    return new StaffOfficialInformation();
                }
                else
                {
                    return so;
                }
            }
            catch (Exception e)
            {
                return new StaffOfficialInformation();
                throw e;
            }
        }

        public StaffPersonalInformation GetStaffPersonalInformation(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select StaffId , Qualification,StaffBloodGroup , StaffMaritalStatus , Addr , " +
                          "Location , City , District , State , Country , PostalCode , " +
                          "Phone , Email , DateOfBirth , MarriageDate , PANNo , AadharNo , " +
                          "PassportNo , DrivingLicense , BankName , BankACNo , BankIFSCCode , " +
                          "BankBranch, FatherName, MotherName, FatherAadharNo, MotherAadharNo, EmergencyContactNo1, " +
                          "EmergencyContactNo2,EmergencyContactPerson1,EmergencyContactPerson2 from staffpersonal where staffid = '"+staffid+"'");

            try
            {
                var sp =
                    context.Database.SqlQuery<StaffPersonalInformation>(qryStr.ToString())
                        .Select(d => new StaffPersonalInformation()
                        {
                            Qualification = d.Qualification,
                            StaffId = d.StaffId,
                            StaffBloodGroup = d.StaffBloodGroup,
                            StaffMaritalStatus = d.StaffMaritalStatus,
                            Addr = d.Addr,
                            Location = d.Location,
                            City = d.City,
                            District = d.District,
                            State = d.State,
                            Country = d.Country,
                            PostalCode = d.PostalCode,
                            Phone = d.Phone,
                            Email = d.Email,
                            DateOfBirth = d.DateOfBirth,
                            MarriageDate = d.MarriageDate,
                            PANNo = d.PANNo,
                            AadharNo = d.AadharNo,
                            PassportNo = d.PassportNo,
                            DrivingLicense = d.DrivingLicense,
                            BankName = d.BankName,
                            BankACNo = d.BankACNo,
                            BankIFSCCode = d.BankIFSCCode,
                            BankBranch = d.BankBranch,
                            FatherName = d.FatherName,
                            MotherName = d.MotherName,
                            FatherAadharNo = d.FatherAadharNo,
                            MotherAadharNo = d.MotherAadharNo,
                            EmergencyContactPerson1 = d.EmergencyContactPerson1,
                            EmergencyContactPerson2 = d.EmergencyContactPerson2,
                            EmergencyContactNo1 = d.EmergencyContactNo1,
                            EmergencyContactNo2 = d.EmergencyContactNo2
                        }).FirstOrDefault();

                if (sp == null)
                {
                    return new StaffPersonalInformation();
                }
                else
                {
                    return sp;
                }
            }
            catch (Exception)
            {
                return new StaffPersonalInformation();
            }
        }

        public bool IsStaffExists(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select count(*) as TotalCount from staff where id =@staffid");
            var count = context.Database.SqlQuery<int>(qryStr.ToString(), new SqlParameter("@staffid", staffid));
            var count1 = Convert.ToInt32(count);

            if (count1 == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public EmployeePhoto GetEmployeePhoto(string StaffId)
        {
            try
            {
                return context.EmployeePhoto.First(d => d.StaffId == StaffId);
            }
            catch
            {
                return null;
            }
        }

        public string SaveStaffInformationToDB(Staff _Sta, StaffPersonal _SP, StaffOfficial _SO, EmployeePhoto _EP, List<AdditionalFieldValue> _AF, EmployeeShiftPlan _ESP, AttendanceControlTable _ACT, bool AddMode)
        {
            string baseuri = string.Empty;
            string requesturi = string.Empty;
            // Int64 Cardno = 0;
            string json = string.Empty;
            string considerStaffPersonal = string.Empty;
            string synchDataToSmax = string.Empty;
            string NameOfSmaxDataBase = string.Empty;
            considerStaffPersonal = ConfigurationManager.AppSettings["ConsiderStaffPersonal"].ToString();
            synchDataToSmax = ConfigurationManager.AppSettings["SynchDataToSmax"].ToString().Trim();
            baseuri = ConfigurationManager.AppSettings["BASEURI"];
            requesturi = ConfigurationManager.AppSettings["REQUESTURI"];
            NameOfSmaxDataBase = ConfigurationManager.AppSettings["NameOfSmaxDataBase"].ToString().Trim();
            var EmailContent = string.Empty;
            var RepName = string.Empty;
            var Repmail = string.Empty;
            var StaffDepname = string.Empty;
            var StaffDesgname = string.Empty;
            CommonRepository rep = new CommonRepository();
            //SMaxData smaxdata = null;
            //List<SMaxData> _lst_ = null;

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    if (_Sta.StaffStatusId != 1)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append("Update AspNetUsers set IsActive=0 where StaffId=@StaffId;");
                        context.Database.ExecuteSqlCommand(builder.ToString(), new SqlParameter("@StaffId", _Sta.Id));
                    }

                    if (_Sta.StaffStatusId == 7)
                    {
                        SqlParameter[] Param = new SqlParameter[2];
                        Param[0] = new SqlParameter("@StaffId", _SO.StaffId);
                        Param[1] = new SqlParameter("@DOR", _SO.DateOfRelieving ?? DateTime.Now);

                        builder = new StringBuilder();
                        builder.Append("Delete from AttendanceData Where StaffId=@StaffId and Convert(date,ShiftInDate)>@DOR");
                        context.Database.ExecuteSqlCommand(builder.ToString(), Param);
                    }

                    if (AddMode == true) //if addition mode then...
                    {
                        //Cardno = Convert.ToInt64(Getmaxid());
                        //Cardno = Cardno + 1;
                        context.Staff.AddOrUpdate(_Sta);
                        context.StaffPersonal.AddOrUpdate(_SP);
                        context.StaffOfficial.AddOrUpdate(_SO);
                        context.SaveChanges();
                        if (_EP != null)
                        {
                            context.EmployeePhoto.AddOrUpdate(_EP);
                            context.SaveChanges();
                        }
                        if (_AF != null)
                        {
                            foreach (var AF in _AF)
                            {
                                if (context.AdditionalFieldValue.Where(d => d.Staffid == AF.Staffid && d.AddfId == AF.AddfId).Count() == 0)
                                {
                                    context.AdditionalFieldValue.AddOrUpdate(AF);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    var qrystr = new StringBuilder();
                                    qrystr.Append("update AdditionalFieldValue set ActualValue=@AFActualValue where " +
                                        "Staffid = @AFStaffid and AddfId = @AFAddfId");
                                    context.Database.ExecuteSqlCommand(qrystr.ToString(), new SqlParameter("@AFActualValue", AF.ActualValue)
                                        , new SqlParameter("@AFStaffid", AF.Staffid), new SqlParameter("@AFAddfId", AF.AddfId));
                                }
                            }
                        }

                        RepName = rep.GetStaffName(_SO.ReportingManager);
                        Repmail = rep.GetEmailIdOfEmployee(_SO.ReportingManager);
                        StaffDepname = rep.GetMasterName(_SO.StaffId, "DP");
                        StaffDesgname = rep.GetMasterName(_SO.StaffId, "DG");

                        if (_SO.IsConfirmed.Equals(false) && _SO.CategoryId!= "CT0003" && _SO.VolumeId!= "VO0002")
                        {
                            var qryString1 = new StringBuilder();
                            qryString1.Clear();
                            qryString1.Append("Exec DBO.spNewJoineesLeaveCredits @StaId");
                            context.Database.ExecuteSqlCommand(qryString1.ToString(), new SqlParameter("@StaId", _Sta.Id));
                        }
                        //if (!string.IsNullOrEmpty(Repmail))
                        //{
                        //    EmailContent = "<html><head><title></title></head><body><p>Dear " + RepName + ",</p><p>The below employee has joined in your department and details are as follows.</p><p>&nbsp; Staffid: " + _Sta.Id + "</p><p>&nbsp; Staff Name: " + _Sta.FirstName + _Sta.MiddleName + _Sta.LastName + "</p><p>&nbsp; Department: " + StaffDepname + "</p><p>&nbsp; Designation: " + StaffDesgname + "</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p>Auto Generated Mail-LMS</p></body></html>";
                        //    rep.SendEmailMessage("", Repmail, "", "", "Subject", EmailContent);
                        //}
                        //if (_ESP != null && Convert.ToString(_ESP.StartDate) != "01-01-0001 00:00:00") //Save Information For EMPShiftPlane
                        //{
                        //    var qrystr = new StringBuilder();
                        //    int GetOLDESPShiftDetails = 0;
                        //    GetOLDESPShiftDetails = context.EmployeeShiftPlan.Where(d => d.StaffId == _ESP.StaffId && d.IsActive == true).Select(a => a.Id).FirstOrDefault();
                        //    if (GetOLDESPShiftDetails != 0)
                        //    {
                        //        qrystr.Append("update EmployeeShiftPlan set IsActive=0 where Id = '" + GetOLDESPShiftDetails + "'");
                        //        context.Database.ExecuteSqlCommand(qrystr.ToString());

                        //    }
                        //    context.EmployeeShiftPlan.Add(_ESP);
                        //    context.SaveChanges();

                        //}

                        //if (_ACT != null && _ACT.ApplicationId != null && _ACT.ApplicationId != "" || _ACT.StaffId != null && _ACT.StaffId != "" || _ACT.FromDate != null || _ACT.ToDate != null || _ACT.CreatedOn != null)
                        //{
                        //    context.AttendanceControlTable.Add(_ACT);
                        //    context.SaveChanges();
                        //}
                    }
                    else
                    {
                        context.Staff.AddOrUpdate(_Sta);
                        string ActionType = string.Empty;
                        string _ChangeLog = string.Empty;
                        string _PrimaryKeyValue = string.Empty;
                        GetChangeLogString(_Sta, context, ref _ChangeLog, ref ActionType, ref _PrimaryKeyValue);

                        context.SaveChanges();
                        if (string.IsNullOrEmpty(_ChangeLog.ToString()) == false)
                        {
                            RecordChangeLog(context, _Sta.ModifiedBy, "STAFFMAIN", _ChangeLog, ActionType, _PrimaryKeyValue);
                        }

                        if (considerStaffPersonal == "Yes")
                        {
                            context.StaffPersonal.AddOrUpdate(_SP);
                            string ActionType1 = string.Empty;
                            string _ChangeLog1 = string.Empty;
                            string _PrimaryKeyValue1 = string.Empty;
                            GetChangeLogString(_SP, context, ref _ChangeLog1, ref ActionType1, ref _PrimaryKeyValue1);
                            context.SaveChanges();
                            if (string.IsNullOrEmpty(_ChangeLog1.ToString()) == false)
                            {
                                RecordChangeLog(context, _Sta.ModifiedBy, "STAFFPERSONAL", _ChangeLog1, ActionType1, _PrimaryKeyValue1);
                            }
                        }

                        context.StaffOfficial.AddOrUpdate(_SO);
                        string ActionType2 = string.Empty;
                        string _ChangeLog2 = string.Empty;
                        string _PrimaryKeyValue2 = string.Empty;
                        GetChangeLogString(_SO, context, ref _ChangeLog2, ref ActionType2, ref _PrimaryKeyValue2);
                        context.SaveChanges();
                        if (string.IsNullOrEmpty(_ChangeLog2.ToString()) == false)
                        {
                            RecordChangeLog(context, _Sta.ModifiedBy, "STAFFOFFICIAL", _ChangeLog2, ActionType2, _PrimaryKeyValue2);
                        }
                        if (_AF != null)
                        {
                            foreach (var AF in _AF)
                            {
                                if (context.AdditionalFieldValue.Where(d => d.Staffid == AF.Staffid && d.AddfId == AF.AddfId).Count() == 0)
                                {
                                    context.AdditionalFieldValue.AddOrUpdate(AF);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    var qrystr = new StringBuilder();
                                    qrystr.Clear();
                                    qrystr.Append("update AdditionalFieldValue set ActualValue=@AFActualValue where " +
                                        "Staffid = @AFStaffid and AddfId = @AFAddfId");
                                    context.Database.ExecuteSqlCommand(qrystr.ToString(), new SqlParameter("@AFActualValue", AF.ActualValue)
                                        , new SqlParameter("@AFStaffid", AF.Staffid), new SqlParameter("@AFAddfId", AF.AddfId));
                                }
                            }
                        }

                        if (_EP != null)
                        {
                            context.EmployeePhoto.AddOrUpdate(_EP);
                            context.SaveChanges();
                        }
                        //if (_ESP != null && Convert.ToString(_ESP.StartDate) != "01-01-0001 00:00:00") //Save Information For EMPlevePlane
                        //{
                        //    var qrystr = new StringBuilder();
                        //    int GetOLDESPShiftDetails = 0;
                        //    GetOLDESPShiftDetails = context.EmployeeShiftPlan.Where(d => d.StaffId == _ESP.StaffId && d.IsActive == true).Select(a => a.Id).FirstOrDefault();
                        //    if (GetOLDESPShiftDetails != 0)
                        //    {
                        //        qrystr.Append("update EmployeeShiftPlan set IsActive=0 where Id = '" + GetOLDESPShiftDetails + "'");
                        //        context.Database.ExecuteSqlCommand(qrystr.ToString());

                        //    }
                        //    context.EmployeeShiftPlan.Add(_ESP);
                        //    context.SaveChanges();

                        //}

                        //if (_ACT != null && _ACT.ApplicationId != null && _ACT.ApplicationId != "" || _ACT.StaffId != null && _ACT.StaffId != "" || _ACT.FromDate != null || _ACT.ToDate != null || _ACT.CreatedOn != null)
                        //{
                        //    context.AttendanceControlTable.Add(_ACT);
                        //    context.SaveChanges();
                        //}
                    }
                    if (_Sta.StaffStatusId != 1 && _Sta.StaffStatusId != 2)
                    {
                        builder = new StringBuilder();
                        SqlParameter[] sqlParameters = new SqlParameter[2];
                        sqlParameters[0] = new SqlParameter("@StaffId", _SO.StaffId);
                        sqlParameters[1] = new SqlParameter("@DOR", _SO.DateOfRelieving ?? DateTime.Now);

                        builder.Clear();
                        builder.Append("Delete from AtrakUserDetails Where StaffId = @StaffId");
                        context.Database.ExecuteSqlCommand(builder.ToString(), new SqlParameter("@StaffId", _SO.StaffId));

                        builder.Clear();
                        builder.Append("Delete from AtrakUserDetails Where StaffId = @StaffId");
                        context.Database.ExecuteSqlCommand(builder.ToString(), new SqlParameter("@StaffId", _SO.StaffId));

                        builder.Clear();
                        builder.Append("Delete from AttendanceData Where StaffId = @StaffId And Convert(Date,ShiftInDate) " +
                            " > Convert(Date , @DOR)");
                        context.Database.ExecuteSqlCommand(builder.ToString(), sqlParameters);

                        builder.Clear();
                        builder.Append("Update EmployeeShiftPlan Set IsActive = 0 Where StaffId = @StaffId And IsActive = 1 ");
                        context.Database.ExecuteSqlCommand(builder.ToString(), new SqlParameter("@StaffId", _SO.StaffId));
                    }
                    trans.Commit();
                    Message = "success";
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var error in e.EntityValidationErrors)
                    {
                        foreach (var er in error.ValidationErrors)
                        {
                            if (Message.ToString() != "")
                            {
                                Message = Message + "~" + er.ErrorMessage;
                            }
                            else
                            {
                                Message = er.ErrorMessage;
                            }
                        }
                    }
                    trans.Rollback();
                    return Message;
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
                return Message;
            }

            ////if (synchDataToSmax == "YES")
            ////{
            ////    if (AddMode == true) //if addition mode then...
            ////    {
            ////        //if (IsStaffExists(_Sta.Id) == true) //if the staffid is already existing then...
            ////        //{
            ////        //    //throw error.
            ////        //    throw new Exception("Staff id already exists. Try entering a new Staff id.");
            ////        //}

            ////        Cardno = Convert.ToInt64(Getmaxid());
            ////        Cardno = Cardno + 1;
            ////    }
            ////    else
            ////    {
            ////        var qryStr = new StringBuilder();
            ////        qryStr.Clear();
            ////        qryStr.Append("select ch_cardno from '" + NameOfSmaxDataBase + "' where  ch_empid = @parameter1");
            ////        decimal count = context.Database.SqlQuery<decimal>(qryStr.ToString(), new SqlParameter("@parameter1", _Sta.Id)).FirstOrDefault();

            ////        if (count != 0)
            ////        {
            ////            Cardno = Convert.ToInt64(count);
            ////        }
            ////    }

            ////    // Assigning values to smaxdata 
            ////    _lst_ = new List<SMaxData>();
            ////    smaxdata = new SMaxData();

            ////    smaxdata.StaffId = _SO.StaffId;
            ////    smaxdata.FName = _Sta.FirstName;
            ////    smaxdata.LName = _Sta.LastName;
            ////    smaxdata.ShortName = _Sta.ShortName;
            ////    if (string.IsNullOrEmpty((_SP.DateOfBirth).ToString()) == false)
            ////    {

            ////        smaxdata.DOB = Convert.ToDateTime(_SP.DateOfBirth);
            ////    }
            ////    else
            ////    {
            ////        smaxdata.DOB = null;
            ////    }

            ////    if (string.IsNullOrEmpty(_Sta.Gender.ToString()) == true)
            ////    {
            ////        smaxdata.Gender = "M";
            ////    }
            ////    else
            ////    {
            ////        smaxdata.Gender = _Sta.Gender.ToString();
            ////    }

            ////    if (smaxdata.Gender == "M")
            ////    {
            ////        smaxdata.Title = "Mr";
            ////    }
            ////    else
            ////    {
            ////        smaxdata.Title = "Miss";
            ////    }

            ////    if (string.IsNullOrEmpty(_SO.Phone) == true)
            ////    {
            ////        smaxdata.Phone = null;
            ////    }
            ////    else
            ////    {
            ////        smaxdata.Phone = _SO.Phone.ToString();
            ////    }

            ////    if (string.IsNullOrEmpty(_SO.Email) == true)
            ////    {
            ////        smaxdata.Phone = null;
            ////    }
            ////    else
            ////    {
            ////        smaxdata.Phone = _SO.Email.ToString();
            ////    }

            ////    if (_Sta.StaffStatusId != 1)
            ////    {
            ////        smaxdata.Status = "HotList";
            ////    }
            ////    else
            ////    {
            ////        smaxdata.Status = "ACTIVE";
            ////    }


            ////    if (string.IsNullOrEmpty((_SO.DateOfJoining).ToString()) == false)
            ////    {

            ////        smaxdata.DOJ = _SO.DateOfJoining;
            ////    }
            ////    else
            ////    {
            ////        smaxdata.DOJ = null;
            ////    }

            ////    smaxdata.Cardnumber = Cardno.ToString();
            ////    smaxdata.DOS = null;
            ////    smaxdata.Company = Getmastervalue(_SO.CompanyId, "C");
            ////    smaxdata.Company = smaxdata.Company.Replace("\"", "");
            ////    smaxdata.Location = Getmastervalue(_SO.LocationId, "L");
            ////    smaxdata.Branch = Getmastervalue(_SO.BranchId, "BR");
            ////    smaxdata.Department = Getmastervalue(_SO.DepartmentId, "DP");
            ////    smaxdata.Division = Getmastervalue(_SO.DivisionId, "DV");
            ////    smaxdata.Designation = Getmastervalue(_SO.DesignationId, "DG");
            ////    smaxdata.Grade = Getmastervalue(_SO.GradeId, "GD");
            ////    smaxdata.Plant = "DEFAULT";

            ////    _lst_.Add(smaxdata);

            ////    json = JsonConvert.SerializeObject(_lst_);

            ////    //calling API method
            ////    // SendDataToSmax(baseuri, requesturi, json).Wait();
            ////}
        }
        public string SaveStaffInformationRequest(Staff _Sta, StaffPersonal _SP, StaffOfficial _SO, EmployeePhoto _EP, List<AdditionalFieldValue> _AF, EmployeeShiftPlan _ESP, AttendanceControlTable _ACT, string Loggedinuser)
        {
            string BaseAddress = string.Empty;
            string MAILFORREP = string.Empty;
            string requesturi = string.Empty;
            string Message = string.Empty;
            string Content = string.Empty;
            string MailMess = string.Empty;
            BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"];
            MAILFORREP = ConfigurationManager.AppSettings["StaffEDITREP"];
            string CC = string.Empty;


            var mr = new MasterRepository();
            var Cm = new CommonRepository();
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    string Staff = string.Empty;
                    string Staffpersonal = string.Empty;
                    string StaffOfficial = string.Empty;
                    string AdditionalField = string.Empty;
                    string AttendanceControlTable = string.Empty;

                    if (_EP != null)
                    {
                        context.EmployeePhoto.AddOrUpdate(_EP);
                        context.SaveChanges();
                    }
                    var qryStr = new StringBuilder();
                    qryStr.Clear();
                    context.Staff.AddOrUpdate(_Sta);
                    GetStaffEditValues(_Sta, context, ref Staff);
                    context.StaffPersonal.AddOrUpdate(_SP);
                    GetStaffEditValues(_SP, context, ref Staffpersonal);
                    //_SO.DateOfJoining = DateTime.ParseExact(_SO.DateOfJoining, "yyyy-MM-dd HH:mm:ss", null);//.ToString("yyyy-MM-dd HH:mm:ss");
                    context.StaffOfficial.AddOrUpdate(_SO);
                    GetStaffEditValues(_SO, context, ref StaffOfficial);
                    context.AttendanceControlTable.Add(_ACT);
                    GetStaffEditValues(_ACT, context, ref AttendanceControlTable);

                    foreach (var AF in _AF)
                    {
                        if (context.AdditionalFieldValue.Where(d => d.Staffid == AF.Staffid && d.AddfId == AF.AddfId).Count() == 0)
                        {
                            AdditionalFieldValue AFT = new AdditionalFieldValue();
                            AFT.Staffid = AF.Staffid;
                            AFT.AddfId = AF.AddfId;
                            context.AdditionalFieldValue.AddOrUpdate(AFT);
                            context.SaveChanges();
                        }
                    }
                    if (_ESP != null) //Save Information For EMPlevePlane
                    {
                        var qrystr = new StringBuilder();
                        int GetOLDESPShiftDetails = 0;
                        GetOLDESPShiftDetails = context.EmployeeShiftPlan.Where(d => d.StaffId == _ESP.StaffId && d.IsActive == true).Select(a => a.Id).FirstOrDefault();
                        if (GetOLDESPShiftDetails != 0)
                        {
                            qrystr.Append("update EmployeeShiftPlan set IsActive=0 where Id = @GetOLDESPShiftDetails");
                            context.Database.ExecuteSqlCommand(qrystr.ToString(),new SqlParameter("@GetOLDESPShiftDetails", GetOLDESPShiftDetails));
                        }
                        context.EmployeeShiftPlan.Add(_ESP);
                        context.SaveChanges();
                    }
                    //////////GetLogForAdditionalFiled(_AF, ref AdditionalField);
                    string lastid = string.Empty;
                    string Maxid = mr.getmaxid("StaffEditRequest", "id", "SR", "", 10, ref lastid);
                    if (Staff != "" || Staffpersonal != "" || StaffOfficial != "" || AdditionalField != "")
                    {
                        if (Staff != "")
                        {
                            var St = Staff.Split('|');
                            string dataValue = string.Empty;
                            Content = "<p><b>StaffMain Information:</b></p><table border=\"1\" style=\" font-size:9pt;\"><thead><tr><td></td><td>Old Value</td><td>New Value</td></tr></thead><tbody>";
                            foreach (var s1 in St)
                            {
                                if (!string.IsNullOrEmpty(s1))
                                {
                                    var data = s1.Split('=');

                                    var value1 = data[0];
                                    var value2 = data[1].Split('>')[0];
                                    var value3 = data[1].Split('>')[1];

                                    var val2 = GetStaffMasterName("Staff", value1, data[1].Split('>')[0]);
                                    if (val2 != "")
                                    {
                                        value2 = val2;
                                    }
                                    var val3 = GetStaffMasterName("Staff", value1, data[1].Split('>')[1]);
                                    if (val3 != "")
                                    {
                                        value3 = val3;
                                    }
                                    Content = Content + "<tr><td>" + value1 + "</td><td>" + value2 + "</td><td>" + value3 + "</td></tr>";
                                }

                            }

                            if (AdditionalField != "")
                            {
                                var AF = AdditionalField.Split('|');

                                foreach (var s1 in AF)
                                {
                                    if (!string.IsNullOrEmpty(s1))
                                    {
                                        string TableName = string.Empty;
                                        string ColumnName = string.Empty;
                                        var data = s1.Split('=');

                                        var value1 = data[0];
                                        GetTableAdditionfield(value1, ref TableName, ref ColumnName);
                                        if (TableName == "Staff")
                                        {
                                            var value2 = data[1].Split('>')[0];
                                            var value3 = data[1].Split('>')[1];
                                            Content = Content + "<tr><td>" + ColumnName + "</td><td>" + value2 + "</td><td>" + value3 + "</td></tr>";
                                        }
                                    }
                                }
                            }
                            Content = Content + "</tr></tbody></table>";

                        }
                        else
                        {

                            if (AdditionalField != "")
                            {
                                string Content1 = string.Empty;

                                var AF = AdditionalField.Split('|');

                                foreach (var s1 in AF)
                                {
                                    if (!string.IsNullOrEmpty(s1))
                                    {
                                        string TableName = string.Empty;
                                        string ColumnName = string.Empty;

                                        var data = s1.Split('=');

                                        var value1 = data[0];
                                        GetTableAdditionfield(value1, ref TableName, ref ColumnName);
                                        if (TableName == "Staff")
                                        {
                                            var value2 = data[1].Split('>')[0];
                                            var value3 = data[1].Split('>')[1];
                                            Content1 = Content1 + "<tr><td>" + ColumnName + "</td><td>" + value2 + "</td><td>" + value3 + "</td></tr>";
                                        }
                                    }

                                }
                                if (Content1 != "")
                                {
                                    Content = "<p><b>StaffMain Information:</b></p><table border=\"1\" style=\" font-size:9pt;\"><thead><tr><td></td><td>Old Value</td><td>New Value</td></tr></thead><tbody>";
                                    Content = Content + Content1 + "</tr></tbody></table>";
                                }
                            }

                        }
                        if (StaffOfficial != "")
                        {
                            var St = StaffOfficial.Split('|');
                            string dataValue = string.Empty;

                            Content = Content + "<p><b>StaffOfficial Information:</b></p><table border=\"1\" style=\" font-size:9pt;\"><thead><tr><td></td><td>Old Value</td><td>New Value</td></tr></thead><tbody>";
                            foreach (var s1 in St)
                            {
                                if (!string.IsNullOrEmpty(s1))
                                {
                                    var data = s1.Split('=');
                                    var value1 = data[0];
                                    var value2 = data[1].Split('>')[0];
                                    var value3 = data[1].Split('>')[1];
                                    if (value1 == "ReportingManager" && MAILFORREP == "YES")
                                    {
                                        CC = Cm.GetEmailIdOfEmployee(value2);
                                    }

                                    var val2 = GetStaffMasterName("Staffofficial", value1, data[1].Split('>')[0]);
                                    if (val2 != "")
                                    {
                                        value2 = val2;
                                    }
                                    var val3 = GetStaffMasterName("Staffofficial", value1, data[1].Split('>')[1]);
                                    if (val3 != "")
                                    {
                                        value3 = val3;
                                    }
                                    try
                                    {
                                        string valueRed = DateTime.ParseExact(value2, "dd-MM-yyyy HH:mm:ss", null).ToString("dd-MM-yyyy");
                                        value2 = valueRed;
                                    }
                                    catch (Exception e) { throw e; }

                                    try
                                    {
                                        string valueRed = DateTime.ParseExact(value3, "dd-MM-yyyy HH:mm:ss", null).ToString("dd-MM-yyyy");
                                        value3 = valueRed;
                                    }
                                    catch (Exception e) { throw e; }
                                    Content = Content + "<tr><td>" + value1 + "</td><td>" + value2 + "</td><td>" + value3 + "</td></tr>";
                                }
                            }
                            if (AdditionalField != "")
                            {
                                var AF = AdditionalField.Split('|');
                                foreach (var s1 in AF)
                                {
                                    if (!string.IsNullOrEmpty(s1))
                                    {
                                        string TableName = string.Empty;
                                        string ColumnName = string.Empty;
                                        var data = s1.Split('=');

                                        var value1 = data[0];
                                        GetTableAdditionfield(value1, ref TableName, ref ColumnName);
                                        if (TableName == "StaffOffcial")
                                        {
                                            var value2 = data[1].Split('>')[0];
                                            var value3 = data[1].Split('>')[1];
                                            Content = Content + "<tr><td>" + ColumnName + "</td><td>" + value2 + "</td><td>" + value3 + "</td></tr>";
                                        }
                                    }
                                }
                            }
                            Content = Content + "</tr></tbody></table>";
                        }
                        else
                        {
                            if (AdditionalField != "")
                            {
                                string Content1 = string.Empty;
                                var AF = AdditionalField.Split('|');
                                foreach (var s1 in AF)
                                {
                                    if (!string.IsNullOrEmpty(s1))
                                    {
                                        string TableName = string.Empty;
                                        string ColumnName = string.Empty;
                                        var data = s1.Split('=');
                                        var value1 = data[0];
                                        GetTableAdditionfield(value1, ref TableName, ref ColumnName);
                                        if (TableName == "StaffOffcial")
                                        {
                                            var value2 = data[1].Split('>')[0];
                                            var value3 = data[1].Split('>')[1];
                                            Content1 = Content1 + "<tr><td>" + ColumnName + "</td><td>" + value2 + "</td><td>" + value3 + "</td></tr>";
                                        }
                                    }
                                }
                                if (Content1 != "")
                                {
                                    Content = Content + "<p><b>StaffOfficial Information:</b></p><table border=\"1\" style=\" font-size:9pt;\"><thead><tr><td></td><td>Old Value</td><td>New Value</td></tr></thead><tbody>";
                                    Content = Content + Content1 + "</tr></tbody></table>";
                                }
                            }
                        }
                        if (Staffpersonal != "")
                        {
                            var St = Staffpersonal.Split('|');
                            string dataValue = string.Empty;
                            Content = Content + "<p><b>StaffPersonal Information:</b></p><table border=\"1\" style=\" font-size:9pt;\"><thead><tr><td></td><td>Old Value</td><td>New Value</td></tr></thead><tbody>";
                            foreach (var s1 in St)
                            {
                                if (!string.IsNullOrEmpty(s1))
                                {
                                    var data = s1.Split('=');

                                    var value1 = data[0];
                                    var value2 = data[1].Split('>')[0];
                                    var value3 = data[1].Split('>')[1];
                                    var val2 = GetStaffMasterName("StaffPersonal", value1, data[1].Split('>')[0]);
                                    if (val2 != "")
                                    {
                                        value2 = val2;
                                    }
                                    var val3 = GetStaffMasterName("StaffPersonal", value1, data[1].Split('>')[1]);
                                    if (val3 != "")
                                    {
                                        value3 = val3;
                                    }
                                    try
                                    {
                                        string valueRed = DateTime.ParseExact(value2, "dd-MM-yyyy HH:mm:ss", null).ToString("dd-MM-yyyy");
                                        value2 = valueRed;
                                    }
                                    catch (Exception e) { throw e; }
                                    try
                                    {
                                        string valueRed = DateTime.ParseExact(value3, "dd-MM-yyyy HH:mm:ss", null).ToString("dd-MM-yyyy");
                                        value3 = valueRed;
                                    }
                                    catch (Exception e) { throw e; }
                                    Content = Content + "<tr><td>" + value1 + "</td><td>" + value2 + "</td><td>" + value3 + "</td></tr>";
                                }
                            }
                            if (AdditionalField != "")
                            {
                                var AF = AdditionalField.Split('|');

                                foreach (var s1 in AF)
                                {
                                    if (!string.IsNullOrEmpty(s1))
                                    {
                                        string TableName = string.Empty;
                                        string ColumnName = string.Empty;
                                        var data = s1.Split('=');

                                        var value1 = data[0];
                                        GetTableAdditionfield(value1, ref TableName, ref ColumnName);
                                        if (TableName == "StaffPersonal")
                                        {
                                            var value2 = data[1].Split('>')[0];
                                            var value3 = data[1].Split('>')[1];
                                            Content = Content + "<tr><td>" + ColumnName + "</td><td>" + value2 + "</td><td>" + value3 + "</td></tr>";
                                        }
                                    }
                                }
                            }
                            Content = Content + "</tr></tbody></table>";
                        }
                        else
                        {
                            if (AdditionalField != "")
                            {
                                string Content1 = string.Empty;
                                var AF = AdditionalField.Split('|');
                                foreach (var s1 in AF)
                                {
                                    if (!string.IsNullOrEmpty(s1))
                                    {
                                        string TableName = string.Empty;
                                        string ColumnName = string.Empty;

                                        var data = s1.Split('=');

                                        var value1 = data[0];
                                        GetTableAdditionfield(value1, ref TableName, ref ColumnName);
                                        if (TableName == "StaffPersonal")
                                        {
                                            var value2 = data[1].Split('>')[0];
                                            var value3 = data[1].Split('>')[1];
                                            Content1 = Content1 + "<tr><td>" + ColumnName + "</td><td>" + value2 + "</td><td>" + value3 + "</td></tr>";
                                        }
                                    }
                                }
                                if (Content1 != "")
                                {
                                    Content = Content + "<p><b>StaffPersonal Information:</b></p><table border=\"1\" style=\" font-size:9pt;\"><thead><tr><td></td><td>Old Value</td><td>New Value</td></tr></thead><tbody>";
                                    Content = Content + Content1 + "</tr></tbody></table>";
                                }
                            }
                        }
                        Content = Content.Replace("True", "YES");
                        Content = Content.Replace("False", "NO");
                        Staff = Staff.Replace("True", "1");
                        Staff = Staff.Replace("False", "0");
                        Staffpersonal = Staffpersonal.Replace("True", "1");
                        Staffpersonal = Staffpersonal.Replace("False", "0");
                        StaffOfficial = StaffOfficial.Replace("True", "1");
                        StaffOfficial = StaffOfficial.Replace("False", "0");
                        qryStr.Append("insert into StaffEditRequest values('" + Maxid + "','" + _Sta.Id + "','" + Staff + "','" + StaffOfficial + "','" + Staffpersonal + "',getdate(),'" + Loggedinuser + "','" + AdditionalField + "')");
                        context.Database.ExecuteSqlCommand(qryStr.ToString());
                        //Content = Content.Replace("-","-->");
                        //string HRSTAFFID = string.Empty;
                        qryStr.Clear();
                        qryStr.Append("select Value from Settings where parameter='HRAPPROVER'");
                        var HRSTAFFID = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
                        string Staffname = Cm.GetStaffName(_Sta.Id);
                        string ModifiedStaffname = Cm.GetStaffName(Loggedinuser);
                        string HRMailid = Cm.GetEmailIdOfEmployee(HRSTAFFID);
                        string FromMailid = Cm.GetEmailIdOfEmployee(Loggedinuser);
                        //MailMess = "<html><body><p>Hi,</p><p>&nbsp; &nbsp; &nbsp;The&nbsp;following  details of  " + Staffname + "&nbsp; are modified by  " + ModifiedStaffname + " </p>" + Content + "<br>Your attention is needed to either approve or reject this application.</p><p><a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + HRSTAFFID + "&ApplicationApprovalId=" + Maxid + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + HRSTAFFID + "&ApplicationApprovalId=" + Maxid + "&Approve=false\">Reject</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p>Atrak - Auto Generated Mail</p></body></html>";
                        MailMess = "<html><body><p>Hi,<br />Following below are the personal/official details modified by employee- " + Staffname + " - " + Loggedinuser + ". .<br />Please approve or reject the changes.</p><p>" + Content + "<br><a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + HRSTAFFID + "&ApplicationApprovalId=" + Maxid + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + HRSTAFFID + "&ApplicationApprovalId=" + Maxid + "&Approve=false\">Reject</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">Atrak - Auto Generated Mail</p></body></html>";
                        Cm.SendEmailMessage(FromMailid, HRMailid, CC, "", "Approval Requisition for tha changes made in Staff Details  by empcode" + Loggedinuser + "", MailMess);
                        Cm.SaveIntoApplicationApproval(Maxid, "SR", Loggedinuser, HRSTAFFID, false);
                        Message = "Request sent, It will get effected after the HR approval.";
                    }
                    else
                    {
                        Message = "There is no changes is done to send a Request";
                    }
                    if (_Sta.StaffStatusId != 1 && _Sta.StaffStatusId != 2)
                    {
                        builder = new StringBuilder();
                        SqlParameter[] sqlParameters = new SqlParameter[2];
                        sqlParameters[0] = new SqlParameter("@StaffId", _SO.StaffId);
                        sqlParameters[1] = new SqlParameter("@DOR", _SO.DateOfRelieving ?? DateTime.Now);

                        builder.Clear();
                        builder.Append("Delete from AtrakUserDetails Where StaffId = @StaffId");
                        context.Database.ExecuteSqlCommand(builder.ToString(), new SqlParameter("@StaffId", _SO.StaffId));

                        builder.Clear();
                        builder.Append("Delete from AtrakUserDetails Where StaffId = @StaffId");
                        context.Database.ExecuteSqlCommand(builder.ToString(), new SqlParameter("@StaffId", _SO.StaffId));

                        builder.Clear();
                        builder.Append("Delete from AttendanceData Where StaffId = @StaffId And Convert(Date,ShiftInDate) " +
                            " > Convert(Date , @DOR)");
                        context.Database.ExecuteSqlCommand(builder.ToString(), sqlParameters);

                        builder.Clear();
                        builder.Append("Update EmployeeShiftPlan Set IsActive = 0 Where StaffId = @StaffId And IsActive = 1 ");
                        context.Database.ExecuteSqlCommand(builder.ToString(), new SqlParameter("@StaffId", _SO.StaffId));
                    }
                    trans.Commit();
                    return Message;
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    Message = e.InnerException.Message;
                    return Message;
                }
            }
        }

        public void GetTableAdditionfield(string value1, ref string TableName, ref string ColumnName)
        {

            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("select ScreenName as TableName,ColumnName from AdditionalField where id=@value1");
            var data = context.Database.SqlQuery<AdditionalFieldModel>(qrystr.ToString(),new SqlParameter("@value1", value1)).FirstOrDefault();

            ColumnName = data.ColumnName;
            TableName = data.TableName;

        }
        public void GetLogForAdditionalFiled(List<AdditionalFieldValue> AF, ref string EditableAdditionalField)
        {
            StringBuilder _EditStaffValues = new StringBuilder();
            foreach (var dt in AF)
            {
                var OldValue = context.AdditionalFieldValue.Where(d => d.Staffid == dt.Staffid && d.AddfId == dt.AddfId).Select(d => d.ActualValue).FirstOrDefault();
                var NewValue = dt.ActualValue;
                if (OldValue != NewValue)
                {
                    _EditStaffValues.Append(string.Format("{0}={1}>{2}|", dt.AddfId, OldValue, NewValue));
                }
            }

            EditableAdditionalField = _EditStaffValues.ToString();

        }
        public string GetStaffMasterName(string Tablename, string columnname, string value)
        {
            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("select dbo.fnGetMasterNameforStaff (@Tablename,@columnname,@value)");
            var data = context.Database.SqlQuery<string>(qrystr.ToString(), new SqlParameter("@Tablename", Tablename)
                , new SqlParameter("@columnname", columnname), new SqlParameter("@value", value)).FirstOrDefault();
            return data;
        }

        public void GetStaffEditValues<T>(T _ClassObject, AttendanceManagementContext context, ref string EditableStaffValues) where T : class
        {
            StringBuilder _EditStaffValues = new StringBuilder();
            string keyName = string.Empty;


            foreach (var entry in context.ChangeTracker.Entries<T>())
            {
                if (entry.State == System.Data.Entity.EntityState.Modified)
                {
                    string FieldName = string.Empty;
                    string OldValue = string.Empty;
                    string NewValue = string.Empty;
                    string DataType = string.Empty;

                    keyName = entry.Entity.GetType().GetProperties().Single(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0).Name;

                    foreach (var name in entry.OriginalValues.PropertyNames)
                    {
                        FieldName = string.Empty;
                        OldValue = string.Empty;
                        NewValue = string.Empty;
                        DataType = string.Empty;

                        FieldName = name;

                        if (entry.OriginalValues[name] != null)
                            OldValue = entry.OriginalValues[name].ToString();

                        if (entry.CurrentValues[name] != null)
                            NewValue = entry.CurrentValues[name].ToString();



                        if (!OldValue.Equals(NewValue) && FieldName != "ModifiedOn" && FieldName != "ModifiedBy")
                        {
                            _EditStaffValues.Append(string.Format("{0}={1}>{2}|", FieldName, OldValue, NewValue));
                        }
                    }
                }
            }
            EditableStaffValues = _EditStaffValues.ToString();
        }

        private string Getmastervalue(string Id, string Tag)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select dbo.[fnGetMastName] (@Id,@Tag)");
            var Name = context.Database.SqlQuery<string>(qryStr.ToString(),new SqlParameter("@Id", Id),
                 new SqlParameter("@Tag", Tag)).FirstOrDefault();
            return Name;
        }

        //private decimal Getmaxid()
        //{

        //    var qryStr = new StringBuilder();
        //    qryStr.Clear();
        //    qryStr.Append("select max(ch_cardno) from MPTSmaxV2.dbo.smx_cardholder");
        //    decimal Maxid = context.Database.SqlQuery<decimal>(qryStr.ToString()).FirstOrDefault();
        //    return Maxid;
        //}

        //public static async Task SendDataToSmax(string BaseURI, string RequestURI, string JSonString)
        //{

        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            client.BaseAddress = new Uri(BaseURI);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        //            HttpResponseMessage response = client.PostAsJsonAsync(RequestURI, JSonString).Result;
        //            try
        //            {
        //                response.EnsureSuccessStatusCode();
        //                // Handle success
        //            }
        //            catch (HttpRequestException)
        //            {
        //                // Handle failure
        //            }

        //        }
        //        catch (Exception)
        //        {
        //            throw new Exception("There was an error while sending the data to SMAX .");
        //        }

        //    }
        //}

        public void SaveStaffFamilyInformation(List<StaffFamilyInformation> sf, string staffid)
        {
            var familylist = GetFamilyFromDB(staffid);
            var mr = new MasterRepository();
            var StaffFamily = new StaffFamily();
            var lastid = string.Empty;
            if (sf != null)
            {
                foreach (var f in sf)
                {
                    StaffFamily = new StaffFamily();
                    if (string.IsNullOrEmpty(f.Id) == true)
                    {
                        lastid = mr.getmaxid("StaffFamily", "id", "SF", "", 10, ref lastid);
                        f.Id = lastid;
                    }
                    StaffFamily.Id = f.Id;
                    StaffFamily.StaffId = f.StaffId;
                    StaffFamily.RelatedAs = f.RelatedAs;
                    StaffFamily.Name = f.Name;
                    StaffFamily.Age = f.Age;

                    context.StaffFamily.AddOrUpdate(StaffFamily);
                    context.SaveChanges();

                    try
                    {
                        familylist.Remove(familylist.Find(d => d.Id.Equals(f.Id)));
                    }
                    catch { }
                }
            }

            //remove excess family member in the list.
            if (familylist.Count > 0)
            {
                foreach (var f in familylist)
                {
                    DeleteFamilyMember(f.Id);
                }
            }
        }

        public void DeleteFamilyMember(string id)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("delete from stafffamily where id = @id");

            context.Database.ExecuteSqlCommand(qryStr.ToString(),new SqlParameter("@id", id));
        }

        public List<StaffFamilyInformation> GetFamilyFromDB(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("select id , staffid , relatedas , name , age from stafffamily where staffid = @staffid");

            try
            {
                var lst = context.Database.SqlQuery<StaffFamilyInformation>(qryStr.ToString(), new SqlParameter("@staffid", staffid))
                    .Select(d => new StaffFamilyInformation()
                    {
                        Id = d.Id,
                        StaffId = d.StaffId,
                        RelatedAs = d.RelatedAs,
                        Name = d.Name,
                        Age = d.Age
                    }).ToList();

                return lst;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SaveEducationInformation(List<StaffEducationInformation> se, string staffid)
        {
            var educationlist = GetEducationFromDB(staffid);
            var mr = new MasterRepository();
            var StaffEducation = new StaffEducation();
            var lastid = string.Empty;

            if (se != null)
            {
                foreach (var e in se)
                {

                    if (string.IsNullOrEmpty(e.Id) == true)
                    {
                        lastid = mr.getmaxid("staffeducation", "Id", "SE", "", 10, ref lastid);
                        e.Id = lastid;
                    }

                    StaffEducation = new StaffEducation();
                    StaffEducation.Id = e.Id;
                    StaffEducation.StaffId = e.StaffId;
                    StaffEducation.CourseName = e.CourseName;
                    StaffEducation.University = e.University;
                    StaffEducation.Completed = e.Completed;
                    StaffEducation.CompletionYear = e.CompletionYear;
                    StaffEducation.Percentage = e.Percentage;
                    StaffEducation.Grade = e.Grade;

                    context.StaffEducation.AddOrUpdate(StaffEducation);
                    context.SaveChanges();

                    try
                    {
                        educationlist.Remove(educationlist.Find(d => d.Id.Equals(e.Id)));
                    }
                    catch { }
                }
            }

            //remove excess family member in the list.
            if (educationlist.Count > 0)
            {
                foreach (var e in educationlist)
                {
                    DeleteEducation(e.Id);
                }
            }
        }

        public void DeleteEducation(string id)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("delete from staffeducation where id = @id");
            context.Database.ExecuteSqlCommand(qryStr.ToString(), new SqlParameter("@id", id));
        }

        public List<StaffEducationInformation> GetEducationFromDB(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select id , staffid , coursename , university , " +
                          "completed, completionyear , percentage , " +
                          "grade from staffeducation where staffid = @staffid");

            try
            {
                var lst =
                context.Database.SqlQuery<StaffEducationInformation>(qryStr.ToString(), new SqlParameter("@staffid", staffid))
                        .Select(d => new StaffEducationInformation()
                        {
                            Id = d.Id,
                            StaffId = d.StaffId,
                            CourseName = d.CourseName,
                            University = d.University,
                            Completed = d.Completed,
                            CompletionYear = d.CompletionYear,
                            Percentage = d.Percentage,
                            Grade = d.Grade
                        }).ToList();

                return lst;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public string GetLevelOfApprovalfromSettings()
        {
            string Result = string.Empty;
            try
            {
                Result = context.Settings.Where(condition => condition.Parameter == "LevelOfApproval" && condition.IsActive == true).Select(select => select.Value).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
            return Result;
        }

        public List<WorkingDayPatternList> GetAllWorkingDayPattern()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select 0 as Id ,'-- Select WorkingDayPattern --' as  PatternDesc Union All Select Id, " +
                "PatternDesc from workingdaypattern Where IsActive = 1");

            try
            {
                var lst = context.Database.SqlQuery<WorkingDayPatternList>(qryStr.ToString()).Select(d => new WorkingDayPatternList()
                {
                    Id = d.Id,
                    PsCode = d.PsCode,
                    PatternDesc = d.PatternDesc,
                    WorkingPattern = d.WorkingPattern,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<WorkingDayPatternList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<WorkingDayPatternList>();
            }
        }

        public List<CompanyList> GetAllCompanies()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select Id , Name from company Where IsActive =1 Order By Name Asc");

            try
            {
                var lst = context.Database.SqlQuery<CompanyList>(qryStr.ToString()).Select(d => new CompanyList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<CompanyList>();
                }
                else
                {
                    return lst;
                }

            }
            catch (Exception)
            {
                return new List<CompanyList>();
            }
        }

        public List<BranchList> GetAllBranches()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select Id , Name from branch Where IsActive = 1 Order By Name Asc");

            try
            {
                var lst = context.Database.SqlQuery<BranchList>(qryStr.ToString()).Select(d => new BranchList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<BranchList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<BranchList>();
            }
        }

        public List<DepartmentList> GetAllDepartments()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select '0' as Id , '-- Select Department --' as Name Union All Select Id , Name from " +
                "Department Where IsActive = 1 Order By Name Asc");

            try
            {
                var lst = context.Database.SqlQuery<DepartmentList>(qryStr.ToString()).Select(d => new DepartmentList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    Phone = d.Phone,
                    Fax = d.Fax,
                    Email = d.Email,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<DepartmentList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<DepartmentList>();
                throw;
            }
        }

        public List<DivisionList> GetAllDivisions()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select '0' as Id,'-- Select the Division --' as Name union all select Id , Name from " +
                "Division Where IsActive=1 Order By Name Asc");

            try
            {
                var lst = context.Database.SqlQuery<DivisionList>(qryStr.ToString()).Select(d => new DivisionList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<DivisionList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<DivisionList>();
            }
        }

        public List<VolumeList> GetAllVolumes()
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append("select '0' as Id,'-- Select the Volume --' as Name union all Select Id,Name from " +
                "[Volume] where IsActive = 1 Order By Name Asc");
            try
            {
                var lst = context.Database.SqlQuery<VolumeList>(queryString.ToString()).Select(r => new VolumeList()
                {
                    Id = r.Id,
                    Name = r.Name,
                    ShortName = r.ShortName
                }).ToList();

                if (lst == null)
                {
                    return new List<VolumeList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<VolumeList>();
            }
        }

        public List<DesignationList> GetAllDesignations()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select '0' as Id,'-- Select the Designation --' as Name union all select Id , Name " +
                "from [Designation] where IsActive=1 order by Name Asc");
            try
            {
                var lst = context.Database.SqlQuery<DesignationList>(qryStr.ToString()).Select(d => new DesignationList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<DesignationList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<DesignationList>();
            }
        }

        public List<GradeList> GetAllGrades()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select '0' as Id,'-- Select the Grade --' as Name union all select Id , Name from " +
                "Grade where IsActive=1 Order By Name Asc");

            try
            {
                var lst = context.Database.SqlQuery<GradeList>(qryStr.ToString()).Select(d => new GradeList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<GradeList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<GradeList>();
            }
        }

        public List<StaffStatusList> GetAllStatuses()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select '0' as Id , '-- Select StaffStatus --' as Name Union All Select Id , Name from " +
                "StaffStatus Where IsActive = 1 Order By Name Asc ");

            try
            {
                var lst = context.Database.SqlQuery<StaffStatusList>(qryStr.ToString()).Select(d => new StaffStatusList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<StaffStatusList>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<StaffStatusList>();
            }
        }

        public List<MaritalStatus> GetAllMaritalStatus()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("select '0' as Id,'-- Select the Marital Status --' as Name union all select Id , Name " +
                "from maritalstatus");

            try
            {
                var lst = context.Database.SqlQuery<MaritalStatus>(qryStr.ToString()).Select(d => new MaritalStatus()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<MaritalStatus>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<MaritalStatus>();
            }
        }

        public List<BloodGroup> GetAllBloodGroup()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("select '0' as Id,'-- Select the Blood Group --' as Name union all select Id , Name from BloodGroup");

            try
            {
                var lst = context.Database.SqlQuery<BloodGroup>(qryStr.ToString()).Select(d => new BloodGroup()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<BloodGroup>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<BloodGroup>();
            }
        }

        public List<LeaveGroupList> GetAllLeaveGroup()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select '0' as Id,'-- Select the Leave Group --' as Name union all select Id , Name from leavegroup where IsActive=1");

            try
            {
                var lst = context.Database.SqlQuery<LeaveGroupList>(qryStr.ToString()).Select(d => new LeaveGroupList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<LeaveGroupList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LeaveGroupList>();
            }
        }

        public List<HolidayGroupList> GetAllHolidayGroup()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select '0' as Id,'-- Select the Holiday Group --' as Name union all select Convert ( varchar, Id) as Id , Name from holidayzone where Isactive=1");

            try
            {
                var lst = context.Database.SqlQuery<HolidayGroupList>(qryStr.ToString()).Select(d => new HolidayGroupList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<HolidayGroupList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<HolidayGroupList>();
            }
        }

        public List<WeeklyOffList> GetAllWeeklyOffGroup()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select '0' as Id , '-- Select WeeklyOffGroup --' as Name Union All Select Id , Name from weeklyoffs Where IsActive = 1");

            try
            {
                var lst = context.Database.SqlQuery<WeeklyOffList>(qryStr.ToString()).Select(d => new WeeklyOffList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<WeeklyOffList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<WeeklyOffList>();
            }
        }

        public List<StaffFamilyInformation> GetStaffFamilyInformation(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Id , StaffId , RelatedAs , Name , Age from stafffamily where staffid = '"+staffid+"'");

            try
            {
                var lst =
                context.Database.SqlQuery<StaffFamilyInformation>(qryStr.ToString(), new SqlParameter("@staffid", staffid))
                        .Select(d => new StaffFamilyInformation()
                        {
                            Id = d.Id,
                            StaffId = d.StaffId,
                            RelatedAs = d.RelatedAs,
                            Name = d.Name,
                            Age = d.Age
                        }).ToList();

                if (lst == null)
                {
                    return new List<StaffFamilyInformation>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<StaffFamilyInformation>();
            }
        }

        public List<StaffEducationInformation> GetStaffEducationInformation(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("select Id , StaffId , CourseName , University , " +
                          "Completed , CompletionYear , Percentage , " +
                          "Grade from staffeducation where staffid = '" + staffid + "'");
            try
            {
                var lst =
                    context.Database.SqlQuery<StaffEducationInformation>(qryStr.ToString())
                        .Select(d => new StaffEducationInformation()
                        {
                            Id = d.Id,
                            StaffId = d.StaffId,
                            CourseName = d.CourseName,
                            University = d.University,
                            Completed = d.Completed,
                            CompletionYear = d.CompletionYear,
                            Percentage = d.Percentage,
                            Grade = d.Grade
                        }).ToList();

                if (lst == null)
                {
                    return new List<StaffEducationInformation>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<StaffEducationInformation>();
            }
        }

        public List<SalutationList> GetSalutation()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select '0' as Id,'-- Select the Title --' as Name union all  select Convert( varchar, Id) as Id," +
                " Name from salutation where IsActive = 1");

            try
            {
                var lst = context.Database.SqlQuery<SalutationList>(qryStr.ToString()).Select(d => new SalutationList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<SalutationList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<SalutationList>();
            }
        }

        public List<RuleGroupList> GetPolicyList()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select '0' as Id,'-- Select the Policy Group --' as Name union all select " +
                "Convert ( varchar, id ) as id , Name from RuleGroup where isactive = 1");

            try
            {
                var lst = context.Database.SqlQuery<RuleGroupList>(qryStr.ToString()).Select(d => new RuleGroupList()
                {
                    id = d.id,
                    name = d.name
                }).ToList();

                if (lst == null)
                {
                    return new List<RuleGroupList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<RuleGroupList>();
            }
        }

        public List<ShiftView> GetshiftList()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select id,Name from Shifts where IsActive=1");

            try
            {
                var lst = context.Database.SqlQuery<ShiftView>(qryStr.ToString()).Select(d => new ShiftView()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<ShiftView>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<ShiftView>();
            }
        }

        public List<ShiftView> Getshiftpattern()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select CONVERT(varchar(10),id) as id,Name from shiftpattern where IsActive=1 ");

            try
            {
                var lst = context.Database.SqlQuery<ShiftView>(qryStr.ToString()).Select(d => new ShiftView()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<ShiftView>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<ShiftView>();
            }
        }

        public List<ShiftView> GetWorkStationList()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select Convert ( varchar, id ) as id ,Name from WorkStation where IsActive = 1 " +
                "Union All Select '0' as Id ,'-- Select WorkStation --' as Name");

            try
            {
                var lst = context.Database.SqlQuery<ShiftView>(qryStr.ToString()).Select(d => new ShiftView()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<ShiftView>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<ShiftView>();
            }
        }

        public List<CategoryList> GetAllCategories()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select '0' as Id,'-- Select the Category --' as Name union all select Id , Name " +
                "from Category where IsActive=1");

            try
            {
                var lst = context.Database.SqlQuery<CategoryList>(qryStr.ToString()).Select(d => new CategoryList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<CategoryList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<CategoryList>();
            }
        }

        public List<CostCentreList> GetAllCostCentres()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select '0' as Id,'-- Select the Cost Centre --' as Name union all select Id , " +
                "Name from CostCentre where IsActive=1");

            try
            {
                var lst = context.Database.SqlQuery<CostCentreList>(qryStr.ToString()).Select(d => new CostCentreList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    IsActive = d.IsActive
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
            catch (Exception)
            {
                return new List<CostCentreList>();
            }
        }

        public List<LocationList> GetAllLocations()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select Id , Name from [Location] where IsActive=1 Order By Name Asc");

            try
            {
                var lst = context.Database.SqlQuery<LocationList>(qryStr.ToString()).Select(d => new LocationList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<LocationList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LocationList>();
            }
        }

        public List<SecurityGroupList> GetAllSecurityGroup()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select '0' as Id,'-- Select the Access Level --' as Name union all SELECT Id , " +
                "Name FROM [SecurityGroup] where IsActive=1");

            try
            {
                var lst = context.Database.SqlQuery<SecurityGroupList>(qryStr.ToString()).Select(d => new SecurityGroupList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<SecurityGroupList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<SecurityGroupList>();
            }
        }

        public void UpdateStaffInformation(string ApplicationApprovalId)
        {
            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("select * from StaffEditRequest where Requestid=@ApplicationApprovalId");
            var lst = context.Database.SqlQuery<StaffEditRequest>(qrystr.ToString()
                ,new SqlParameter("@ApplicationApprovalId", ApplicationApprovalId)).FirstOrDefault();

            var staffid = lst.UserId;
            var Staff = lst.Staff;
            var StaffOfficial = lst.StaffOfficial;
            var StaffPersonal = lst.StaffPersonal;
            var Additionalfield = lst.AdditionalFieldValue;
            if (Staff != "")
            {
                qrystr.Clear();
                var st = Staff.Split('|');
                string dataValue = string.Empty;

                foreach (var s1 in st)
                {
                    if (!string.IsNullOrEmpty(s1))
                    {
                        var data = s1.Split('=');
                        var value1 = data[0];
                        var value2 = data[1].Split('>')[1];
                        if (!string.IsNullOrEmpty(dataValue))
                        {
                            dataValue = dataValue + "," + value1 + "=" + "'" + value2 + "'";
                        }
                        else
                        {
                            dataValue = value1 + "=" + "'" + value2 + "'";
                        }
                    }
                }
                qrystr.Append("update staff set " + dataValue + "  where id= @staffid");
                context.Database.ExecuteSqlCommand(qrystr.ToString(), new SqlParameter("@staffid", staffid));
            }
            if (StaffOfficial != "")
            {

                qrystr.Clear();
                var SO = StaffOfficial.Split('|');

                string dataValue = string.Empty;

                foreach (var s1 in SO)
                {
                    if (!string.IsNullOrEmpty(s1))
                    {
                        var data = s1.Split('=');
                        var value1 = data[0];
                        var value2 = data[1].Split('>')[1];
                        if (!string.IsNullOrEmpty(dataValue))
                        {
                            try
                            {
                                string valueRed = DateTime.ParseExact(value2, "dd-MM-yyyy HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                                dataValue = dataValue + "," + value1 + "=" + "'" + valueRed + "'";
                            }
                            catch 
                            {
                                dataValue = dataValue + "," + value1 + "=" + "'" + value2 + "'";
                            }
                        }
                        else
                        {
                            try
                            {
                                string valueRed = DateTime.ParseExact(value2, "dd-MM-yyyy HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                                dataValue = value1 + "=" + "'" + valueRed + "'";
                            }
                            catch 
                            {
                                dataValue = value1 + "=" + "'" + value2 + "'";
                            }
                        }
                    }
                }
                qrystr.Append("update Staffofficial set " + dataValue + "  where staffid=@staffid");
                context.Database.ExecuteSqlCommand(qrystr.ToString(), new SqlParameter("@staffid", staffid));

            }
            if (StaffPersonal != "")
            {
                qrystr.Clear();
                var SP = StaffPersonal.Split('|');
                string dataValue = string.Empty;

                foreach (var s1 in SP)
                {
                    if (!string.IsNullOrEmpty(s1))
                    {
                        var data = s1.Split('=');
                        var value1 = data[0];
                        var value2 = data[1].Split('>')[1];
                        if (!string.IsNullOrEmpty(dataValue))
                        {
                            try
                            {
                                string valueRed = DateTime.ParseExact(value2, "dd-MM-yyyy HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                                dataValue = dataValue + "," + value1 + "=" + "'" + valueRed + "'";
                            }
                            catch 
                            {
                                dataValue = dataValue + "," + value1 + "=" + "'" + value2 + "'";
                            }
                        }
                        else
                        {
                            try
                            {
                                string valueRed = DateTime.ParseExact(value2, "dd-MM-yyyy HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                                dataValue = value1 + "=" + "'" + valueRed + "'";
                            }
                            catch 
                            {
                                dataValue = value1 + "=" + "'" + value2 + "'";
                            }
                        }
                    }
                }
                qrystr.Append("update staffpersonal set " + dataValue + "  where staffid=@staffid");
                context.Database.ExecuteSqlCommand(qrystr.ToString(),new SqlParameter("@staffid", staffid));

            }
            if (Additionalfield != "")
            {
                qrystr.Clear();
                var SP = Additionalfield.Split('|');
                string dataValue = string.Empty;

                foreach (var s1 in SP)
                {
                    if (!string.IsNullOrEmpty(s1))
                    {
                        var data = s1.Split('=');
                        var value1 = data[0];
                        var value2 = data[1].Split('>')[1];

                        qrystr.Append("update AdditionalFieldValue set ActualValue=@value2 where staffid=staffid and AddfId=value1");
                        context.Database.ExecuteSqlCommand(qrystr.ToString(), new SqlParameter("@value2", value2), 
                            new SqlParameter("@value1", value1), new SqlParameter("@staffid", staffid));
                    }

                }

            }
        }

        public List<StaffEditReqModel> GetStaffEditRequest(string Staffid)
        {
            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("select RequestId,UserId as Staffid,Staff,StaffOfficial,StaffPersonal,upper ( replace ( convert ( varchar , Createdon , 106 ) , ' ' , '-' ) ) as Createdon,case ");
            qrystr.Append("ApprovalStatusId when 1 then 'Pending' when 2 then 'Approved' else 'Rejected' end  as Status,upper ( replace ( convert ( varchar , ApprovedOn , 106 ) , ' ' , '-' ) )  as ApprovedOn from StaffEditRequest A ");
            qrystr.Append("join ApplicationApproval B on A.RequestId=B.ParentId where A.Userid=@Staffid");
            var lst = context.Database.SqlQuery<StaffEditReqModel>(qrystr.ToString(), new SqlParameter("@staffid", Staffid)).ToList();
            List<StaffEditReqModel> list = new List<StaffEditReqModel>();
            if (lst != null)
            {
                foreach (var data in lst)
                {
                    StaffEditReqModel dt = new StaffEditReqModel();
                    dt.Requestid = data.Requestid;
                    dt.Staffid = data.Staffid;
                    dt.createdon = data.createdon;
                    dt.Status = data.Status;
                    dt.Approvedon = data.Approvedon;
                    dt.Requestid = data.Requestid;
                    if (data.Staff != "")
                    {
                        var Content = string.Empty;
                        var St = data.Staff.Split('|');
                        string dataValue = string.Empty;

                        Content = Content + "<table border=\"0\" style=\" font-size:6pt;\"><thead><tr><td></td><td>Old Value</td><td>New Value</td></tr></thead><tbody>";
                        foreach (var s1 in St)
                        {
                            if (!string.IsNullOrEmpty(s1))
                            {
                                var data1 = s1.Split('=');

                                var value1 = data1[0];

                                var value2 = data1[1].Split('>')[0];
                                var value3 = data1[1].Split('>')[1];

                                var val2 = GetStaffMasterName("Staff", value1, data1[1].Split('>')[0]);
                                if (val2 != "")
                                {
                                    value2 = val2;
                                }
                                var val3 = GetStaffMasterName("Staff", value1, data1[1].Split('>')[1]);
                                if (val3 != "")
                                {
                                    value3 = val3;
                                }
                                try
                                {
                                    string valueRed = DateTime.ParseExact(value2, "dd-MM-yyyy HH:mm:ss", null).ToString("dd-MM-yyyy");
                                    value2 = valueRed;
                                }
                                catch (Exception e) { throw e; }

                                try
                                {
                                    string valueRed = DateTime.ParseExact(value3, "dd-MM-yyyy HH:mm:ss", null).ToString("dd-MM-yyyy");
                                    value3 = valueRed;
                                }
                                catch (Exception e) { throw e; }

                                Content = Content + "<tr><td>" + value1 + "</td><td>" + value2 + "</td><td>" + value3 + "</td></tr>";

                            }
                        }
                        Content = Content + "</tr></tbody></table>";
                        dt.Staff = Content;
                    }
                    if (data.Staffofficial != "")
                    {
                        var Content = string.Empty;
                        var St = data.Staffofficial.Split('|');
                        string dataValue = string.Empty;

                        Content = Content + "<table border=\"0\" style=\" font-size:6pt;\"><thead><tr><td></td><td>Old Value</td><td>New Value</td></tr></thead><tbody>";
                        foreach (var s1 in St)
                        {
                            if (!string.IsNullOrEmpty(s1))
                            {
                                var data1 = s1.Split('=');

                                var value1 = data1[0];

                                var value2 = data1[1].Split('>')[0];
                                var value3 = data1[1].Split('>')[1];

                                var val2 = GetStaffMasterName("Staffofficial", value1, data1[1].Split('>')[0]);
                                if (val2 != "")
                                {
                                    value2 = val2;
                                }
                                var val3 = GetStaffMasterName("Staffofficial", value1, data1[1].Split('>')[1]);
                                if (val3 != "")
                                {
                                    value3 = val3;
                                }
                                try
                                {
                                    string valueRed = DateTime.ParseExact(value2, "dd-MM-yyyy HH:mm:ss", null).ToString("dd-MM-yyyy");
                                    value2 = valueRed;
                                }
                                catch (Exception e) { throw e; }

                                try
                                {
                                    string valueRed = DateTime.ParseExact(value3, "dd-MM-yyyy HH:mm:ss", null).ToString("dd-MM-yyyy");
                                    value3 = valueRed;
                                }
                                catch (Exception e) { throw e; }

                                Content = Content + "<tr><td>" + value1 + "</td><td>" + value2 + "</td><td>" + value3 + "</td></tr>";
                            }
                        }
                        Content = Content + "</tr></tbody></table>";
                        dt.Staffofficial = Content;
                    }
                    if (data.StaffPersonal != "")
                    {
                        var Content = string.Empty;
                        var St = data.StaffPersonal.Split('|');
                        string dataValue = string.Empty;

                        Content = Content + "<table border=\"0\" style=\" font-size:6pt;\"><thead><tr><td></td><td>Old Value</td><td>New Value</td></tr></thead><tbody>";
                        foreach (var s1 in St)
                        {
                            if (!string.IsNullOrEmpty(s1))
                            {
                                var data1 = s1.Split('=');

                                var value1 = data1[0];

                                var value2 = data1[1].Split('>')[0];
                                var value3 = data1[1].Split('>')[1];

                                var val2 = GetStaffMasterName("StaffPersonal", value1, data1[1].Split('>')[0]);
                                if (val2 != "")
                                {
                                    value2 = val2;
                                }
                                var val3 = GetStaffMasterName("StaffPersonal", value1, data1[1].Split('>')[1]);
                                if (val3 != "")
                                {
                                    value3 = val3;
                                }
                                try
                                {
                                    string valueRed = DateTime.ParseExact(value2, "dd-MM-yyyy HH:mm:ss", null).ToString("dd-MM-yyyy");
                                    value2 = valueRed;
                                }
                                catch (Exception e) { throw e; }

                                try
                                {
                                    string valueRed = DateTime.ParseExact(value3, "dd-MM-yyyy HH:mm:ss", null).ToString("dd-MM-yyyy");
                                    value3 = valueRed;
                                }
                                catch (Exception e) { throw e; }

                                Content = Content + "<tr><td>" + value1 + "</td><td>" + value2 + "</td><td>" + value3 + "</td></tr>";
                            }
                        }
                        Content = Content + "</tr></tbody></table>";
                        dt.StaffPersonal = Content;
                    }
                    list.Add(dt);
                }
            }

            return list;
        }

        public void SaveAdditionalField(AdditionalFieldModel objSt, string LoggedinStaffid)
        {

            AdditionalField af = new AdditionalField();
            af.ScreenName = objSt.TableName;
            af.ColumnName = objSt.ColumnName;
            af.Access = objSt.Access;
            af.Type = objSt.Type;
            af.CreatedOn = DateTime.Now;
            af.Createdby = LoggedinStaffid;
            context.AdditionalField.AddOrUpdate(af);
            context.SaveChanges();
        }

        public List<AdditionalFieldModel> GetAdditionalFileds()
        {
            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("Select * from AdditionalField");
            var data = context.Database.SqlQuery<AdditionalField>(qrystr.ToString()).ToList();
            if (data != null)
            {
                List<AdditionalFieldModel> lst = new List<AdditionalFieldModel>();
                foreach (var dt in data)
                {
                    AdditionalFieldModel af = new AdditionalFieldModel();
                    af.ColumnName = dt.ColumnName;
                    af.TableName = dt.ScreenName;
                    af.Id = dt.Id;
                    af.Access = dt.Access;
                    af.Type = dt.Type;
                    lst.Add(af);
                }
                return lst;
            }
            else
            {
                return new List<AdditionalFieldModel>();
            }
        }

        public List<AdditionalFieldModel> GetAdditionalFiledsValues(string staffid)
        {
            var qrystr = new StringBuilder();
            qrystr.Append("Select B.Id,A.Staffid,B.ColumnName,B.ScreenName as TableName,B.[Type],B.Access," +
                "A.ActualValue as Value from AdditionalFieldValue A join AdditionalField B on A.AddfId=B.Id" +
                " where staffid= '" + staffid + "'");
            var data = context.Database.SqlQuery<AdditionalFieldModel>(qrystr.ToString()).ToList();
            if (data != null)
            {
                return data;
            }
            else
            {
                return new List<AdditionalFieldModel>();
            }
        }

        #region Bulk Masters Import
        public string GetMasterTableValueRepository(string MasterTable, string Name)
        {
            try
            {
                builder = new StringBuilder();
                builder.Append("Select Convert(varchar(30),Id) from " + MasterTable + " where Name=@Name");
                Message = context.Database.SqlQuery<string>(builder.ToString(), new SqlParameter("@Name", Name)).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
            return Message;
        }
        public string UpdateMastersRepository(List<BulkMasterImportModel> list, string MasterTable)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    string UpdateColumn = string.Empty;

                    foreach (var data in list)
                    {
                        SqlParameter[] Param = new SqlParameter[2];
                        Param[0] = new SqlParameter("@value", data.MasterValue);
                        Param[1] = new SqlParameter("@StaffId", data.StaffId);

                        UpdateColumn = MasterTable + "Id";
                        builder = new StringBuilder();
                        builder.Append("Update StaffOfficial set " + UpdateColumn + "=@value where StaffId= @StaffId");
                        context.Database.ExecuteSqlCommand(builder.ToString(), Param);
                    }
                    trans.Commit();
                    Message = "success";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }
            return Message;
        }
        #endregion

        #region Staff Removing Process
        public int ValidateStaffIdRepository(string StaffId)
        {
            int count = 0;
            try
            {
                builder = new StringBuilder();
                builder.Append("Select Count(Id) from Staff where Id=@StaffId and StaffStatusId=1");
                count = context.Database.SqlQuery<int>(builder.ToString(), new SqlParameter("@StaffId", StaffId)).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
            return count;
        }
        public string UpdateBulkEmployeeRelieving(List<RemoveStaffModel> List)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    //string GetReaderDB = context.Settings.Where(condition => condition.Parameter == "SmaxConnection" && condition.IsActive == true).Select(select => select.Value).FirstOrDefault();
                    foreach (var data in List)
                    {
                        SqlParameter[] Param = new SqlParameter[4];
                        Param[0] = new SqlParameter("@StaffId", data.StaffId);
                        Param[1] = new SqlParameter("@Status", data.Status);
                        Param[2] = new SqlParameter("@ResignationDate", data.ResignationDate ?? data.RelievingDate);
                        Param[3] = new SqlParameter("@RelievingDate", data.RelievingDate ?? data.ResignationDate);

                        builder = new StringBuilder();
                        builder.Append("Update Staff set StaffStatusId=@Status where Id=@StaffId;Update StaffOfficial set ResignationDate=@ResignationDate,DateOfRelieving=@RelievingDate where StaffId=@StaffId");
                        context.Database.ExecuteSqlCommand(builder.ToString(), Param);

                        //HotlistProcessRepository(data.StaffId, "Hotlist", GetReaderDB);
                    }
                    ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 360;
                    trans.Commit();
                    Message = "success";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
                return Message;
            }
        }
        public void HotlistProcessRepository(string StaffId, string Status, string GetReaderDB)
        {
            try
            {
                SqlParameter[] Param = new SqlParameter[2];
                Param[0] = new SqlParameter("@StaffId", StaffId);
                Param[1] = new SqlParameter("@Status", Status);

                StringBuilder builder = new StringBuilder();
                builder.Append("Exec " + GetReaderDB + ".[dbo].[sp_EmployeesStatus] @StaffId,@Status");
                context.Database.ExecuteSqlCommand(builder.ToString(), Param);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}
