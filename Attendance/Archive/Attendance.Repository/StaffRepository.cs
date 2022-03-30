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

namespace Attendance.Repository
{
    public class StaffRepository : TrackChangeRepository
    {

        private AttendanceManagementContext context = null;

        public StaffRepository()
        {
            context = new AttendanceManagementContext();
        }

        StringBuilder builder = new StringBuilder();
        string Message = string.Empty;

        //rajesh
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



        public StaffInformation GetStaffMainInformation(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select Id , StaffStatusId , CardCode , " +
                          "FirstName , MiddleName , LastName , " +
                          "ShortName , Gender, SalutationId , IsHidden , CreatedOn , CreatedBy from Staff " +
                          "where id = @staffid");
            try
            {
                var si = context.Database.SqlQuery<StaffInformation>(qryStr.ToString(),new SqlParameter("@staffid", staffid)).Select(d => new StaffInformation()
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
            catch (Exception)
            {
                return new StaffInformation();
            }
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
                        if (data.ShiftName.Contains("WO") || data.ShiftName.Contains("WEEK"))
                        {
                            tbl.ShiftId = "LV0011";
                        }
                        else
                        {
                            ShiftId = context.Shifts.Where(condition => condition.Name.Contains(data.ShiftName) || condition.ShortName.Contains(data.ShiftName)).Select(select => select.Id).FirstOrDefault();
                            tbl.ShiftId = ShiftId ?? throw new Exception("Please Enter Shift Name or Shift Short Name");
                        }
                        tbl.StaffId = data.StaffId;
                        tbl.ShiftFromDate = data.FromDate;
                        tbl.ShiftToDate = data.ToDate;
                        tbl.IsProcessed = false;
                        tbl.CreatedOn = DateTime.Now;
                        tbl.CreatedBy = CreatedBy;

                        context.ShiftsImportData.Add(tbl);
                        context.SaveChanges();
                    }
                    trans.Commit();

                    
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    Message = e.Message;
                }
            }

            string Check = "true";
            Check = context.Settings.Where(condition => condition.Parameter == "BulkShiftsImportData" && condition.IsActive == true).Select(select => select.Value).FirstOrDefault();
            if (Check != null && Check == "1")
            {
                builder = new StringBuilder();
                builder.Append("Exec GetBulkShiftsImportData");
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 0;
                context.Database.ExecuteSqlCommand(builder.ToString());
            }

            Message = "success";
            return Message;
        }

        public StaffOfficialInformation GetStaffOfficialInformation(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append(" Select StaffId , CompanyId , BranchId , DepartmentId , DivisionId ,VolumeId , " +
                          " DesignationId , GradeId , LeaveGroupId , WeeklyOffId , HolidayGroupId , PolicyId, Canteen, Travel, SalaryDay, IsConfirmed, " +
                          " ConfirmationDate, IsWorkingDayPatternLocked, IsLeaveGroupLocked, IsHolidayGroupLocked, " +
                          " IsWeeklyOffLocked, IsPolicyLocked, " + "DateOfJoining , ResignationDate , DateOfRelieving , Phone , Fax , Email , ExtensionNo , Interimhike , Tenure , PFNo ," +
                          " ESINo ,CategoryId ,CostCentreId ,LocationId , SecurityGroupId , WorkingDayPatternId , ReportingManager ,ApproverLevel, " +
                          " b.FirstName as ReportingManagerName , DomainId , a.MedicalClaimNumber, ShiftId, ShiftPatternId, left (convert (varchar , Flexishift , 114 ) , 8 ) as FlexishiftsTime, " +
                          " CASE WHEN IsAutoShift = 1 THEN 'Autoshift' ELSE '0' END AS IsAutoShift,CASE  WHEN Isflexishift = 1 THEN 'flexishift' ELSE '0' END AS Isflexishift, CASE WHEN IsGeneralShift = 1 THEN 'Generalshift' ELSE '0' END AS IsGeneralShift, CASE WHEN IsShiftPattern = 1 THEN 'Shiftpattern' ELSE '0' END AS IsShiftPattern, " +
                          " Case when ApproverLevel = 1 then 'SingleLevel' else '0' end as SingleLevel, Case when ApproverLevel = 2 then 'MultipleLevel' else '0' end as MultipleLevel, " +
                          " Case when IsWorkingDayPatternLocked = 1 then 'IsWorkingDayPatternLocked' else '0' end as WorkingDayPattern, Case when IsWeeklyOffLocked = 1 then 'IsWeeklyOffLocked' else '0' end as WeeklyOffLocked, " +
                          " [dbo].[fnGetStaffName](a.Reviewer) as ReviewerName, a.Reviewer as Reviewer ,a.IsOTEligible , a.IsMobileAppEligible " +
                          " from staffofficial  a left join staff b on a.reportingmanager = b.id where staffid = @staffid");

            try
            {
                var so =
                context.Database.SqlQuery<StaffOfficialInformation>(qryStr.ToString(),new SqlParameter("@staffid", staffid))
                        .Select(d => new StaffOfficialInformation()
                        {
                            StaffId = d.StaffId,
                            CompanyId = d.CompanyId,
                            BranchId = d.BranchId,
                            DepartmentId = d.DepartmentId,
                            DivisionId = d.DivisionId,
                            VolumeId = d.VolumeId,
                            DesignationId = d.DesignationId,
                            GradeId = d.GradeId,
                            CategoryId = d.CategoryId,
                            CostCentreId = d.CostCentreId,
                            LocationId = d.LocationId,
                            SecurityGroupId = d.SecurityGroupId,
                            Interimhike = d.Interimhike,
                            Tenure = d.Tenure,
                            PFNo = d.PFNo,
                            ESINo = d.ESINo,
                            LeaveGroupId = d.LeaveGroupId,
                            WeeklyOffId = d.WeeklyOffId,
                            HolidayGroupId = d.HolidayGroupId,
                            PolicyId = d.PolicyId,
                            Canteen = d.Canteen,
                            Travel = d.Travel,
                            SalaryDay = d.SalaryDay,
                            DateOfJoining = d.DateOfJoining,
                            ResignationDate = d.ResignationDate,
                            DateOfRelieving = d.DateOfRelieving,
                            MedicalClaimNumber = d.MedicalClaimNumber,
                            Phone = d.Phone,
                            Fax = d.Fax,
                            Email = d.Email,
                            ExtensionNo = d.ExtensionNo,
                            WorkingDayPatternId = d.WorkingDayPatternId,
                            ReportingManager = d.ReportingManager,
                            ReportingManagerName = d.ReportingManagerName,
                            Reviewer = d.Reviewer,
                            ReviewerName = d.ReviewerName,
                            //OTReviewer = d.OTReviewer,
                            //OTReviewerName = d.OTReviewerName,
                            //OTReportingManager = d.OTReportingManager,
                            //OTReportingManagerName = d.OTReportingManagerName,
                            IsConfirmed = d.IsConfirmed,
                            ConfirmationDate = d.ConfirmationDate,
                            IsLeaveGroupLocked = d.IsLeaveGroupLocked,
                            IsHolidayGroupLocked = d.IsHolidayGroupLocked,                           
                            IsPolicyLocked = d.IsPolicyLocked,
                            DomainId = d.DomainId,
                            IsAutoShift = d.IsAutoShift,
                            IsGeneralShift = d.IsGeneralShift,
                            Isflexishift = d.Isflexishift,
                            IsShiftPattern = d.IsShiftPattern,
                            ShiftId = d.ShiftId,
                            ShiftPatternId = d.ShiftPatternId,
                            FlexishiftsTime =d.FlexishiftsTime,
                            SingleLevel = d.SingleLevel,
                            MultipleLevel = d.MultipleLevel,
                            IsWorkingDayPatternLocked = d.IsWorkingDayPatternLocked,
                            WorkingDayPattern = d.WorkingDayPattern,
                            IsWeeklyOffLocked = d.IsWeeklyOffLocked,
                            WeeklyOffLocked = d.WeeklyOffLocked,
                            IsOTEligible = d.IsOTEligible  ,
                            IsMobileAppEligible = d.IsMobileAppEligible

                        }).FirstOrDefault();

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
                throw;
            }
        }

        public StaffPersonalInformation GetStaffPersonalInformation(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select StaffId , StaffBloodGroup , StaffMaritalStatus , Addr , " +
                          "Location , City , District , State , Country , PostalCode , " +
                          "Phone , Email , DateOfBirth , MarriageDate , PANNo , AadharNo , " +
                          "PassportNo , DrivingLicense , BankName , BankACNo , BankIFSCCode , EmergencyContactNumber, EmergencyContactPerson, PresentAddress,  " +
                          "BankBranch from staffpersonal where staffid = @staffid");

            try
            {
                var sp =
                    context.Database.SqlQuery<StaffPersonalInformation>(qryStr.ToString(),new SqlParameter("@staffid", staffid))
                        .Select(d => new StaffPersonalInformation()
                        {
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
                            EmergencyContactNumber = d.EmergencyContactNumber,
                            EmergencyContactPerson = d.EmergencyContactPerson,
                            PresentAddress = d.PresentAddress
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
            qryStr.Append("select count(*) as TotalCount from staff where id = @staffid");
            var count = context.Database.SqlQuery<int>(qryStr.ToString(),new SqlParameter("@staffid", staffid));
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

        public void SaveStaffInformationToDB(Staff _Sta, StaffPersonal _SP, StaffOfficial _SO, EmployeePhoto _EP, List<AdditionalFieldValue> _AF, bool AddMode)
        {
            string baseuri = string.Empty;
            string requesturi = string.Empty;
            string json = string.Empty;
            string considerStaffPersonal = string.Empty;
            string synchDataToSmax = string.Empty;
            string NameOfSmaxDataBase = string.Empty;
            considerStaffPersonal = ConfigurationManager.AppSettings["ConsiderStaffPersonal"].ToString();
            synchDataToSmax = ConfigurationManager.AppSettings["SynchDataToSmax"].ToString().Trim();
            baseuri = ConfigurationManager.AppSettings["BASEURI"];
            requesturi = ConfigurationManager.AppSettings["REQUESTURI"];
            NameOfSmaxDataBase = ConfigurationManager.AppSettings["NameOfSmaxDataBase"].ToString().Trim();
            string IsMailTriggerEnabledForNewJoinee = ConfigurationManager.AppSettings["SendMailTriggerToReportingManagerForNewJoinee"].ToString().Trim();
            var EmailContent = string.Empty;
            var RepName = string.Empty;
            var Repmail = string.Empty;
            var StaffDepname = string.Empty;
            var StaffDesgname = string.Empty;
            CommonRepository rep = new CommonRepository();

            using (var trans = context.Database.BeginTransaction())
            {

                try
                {
                    if (AddMode == true) //if addition mode then...
                    {
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
                                    SqlParameter[] sqlParameter = new SqlParameter[3];
                                    sqlParameter[0] = new SqlParameter("@ActualValue", AF.ActualValue);
                                    sqlParameter[1] = new SqlParameter("@Staffid", AF.Staffid ?? "");
                                    sqlParameter[2] = new SqlParameter("@AddfId", AF.AddfId);


                                    var qrystr = new StringBuilder();
                                    qrystr.Clear();
                                    qrystr.Append("update AdditionalFieldValue set ActualValue= @ActualValue where Staffid = @Staffid  and AddfId = @AddfId ");
                                    context.Database.ExecuteSqlCommand(qrystr.ToString(),sqlParameter);
                                }
                            }
                        }

                        if (IsMailTriggerEnabledForNewJoinee.Equals("Yes"))
                        {
                            RepName = rep.GetStaffName(_SO.ReportingManager);
                            Repmail = rep.GetEmailIdOfEmployee(_SO.ReportingManager);
                            StaffDepname = rep.GetMasterName(_SO.StaffId, "DP");
                            StaffDesgname = rep.GetMasterName(_SO.StaffId, "DG");
                            if (!string.IsNullOrEmpty(Repmail))
                            {
                                EmailContent = "<html><head><title></title></head><body><p>Dear " + RepName + ",</p><p>The below employee has joined in your department and details are as follows.</p><p>&nbsp; Staffid: " + _Sta.Id + "</p><p>&nbsp; Staff Name: " + _Sta.FirstName + _Sta.MiddleName + _Sta.LastName + "</p><p>&nbsp; Department: " + StaffDepname + "</p><p>&nbsp; Designation: " + StaffDesgname + "</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p>Auto Generated Mail-LMS</p></body></html>";
                                rep.SendEmailMessage("", Repmail, "", "", "Subject", EmailContent);
                            }
                        }

                        //var qryString1 = new StringBuilder();
                        //qryString1.Clear();
                        //qryString1.Append("Exec DBO.spNewJoineesLeaveCredits '" + _Sta.Id + "'");
                        //context.Database.ExecuteSqlCommand(qryString1.ToString());

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
                                    SqlParameter[] sqlParameter = new SqlParameter[3];
                                    sqlParameter[0] = new SqlParameter("@ActualValue", AF.ActualValue);
                                    sqlParameter[1] = new SqlParameter("@Staffid", AF.Staffid ?? "");
                                    sqlParameter[2] = new SqlParameter("@AddfId", AF.AddfId);
                                    var qrystr = new StringBuilder();
                                    qrystr.Clear();
                                    qrystr.Append("update AdditionalFieldValue set ActualValue=@ActualValue where Staffid = @Staffid and AddfId = @AddfId ");
                                    context.Database.ExecuteSqlCommand(qrystr.ToString(), sqlParameter);

                                }

                            }
                        }

                        if (_EP != null)
                        {
                            context.EmployeePhoto.AddOrUpdate(_EP);
                            context.SaveChanges();
                        }
                    }
                    trans.Commit();
                }
                catch(Exception e)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }


        public string SaveStaffInformationRequest(Staff _Sta, StaffPersonal _SP, StaffOfficial _SO, EmployeePhoto _EP, List<AdditionalFieldValue> _AF, string Loggedinuser)
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
                    context.StaffOfficial.AddOrUpdate(_SO);
                    GetStaffEditValues(_SO, context, ref StaffOfficial);


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
                                    catch { }

                                    try
                                    {
                                        string valueRed = DateTime.ParseExact(value3, "dd-MM-yyyy HH:mm:ss", null).ToString("dd-MM-yyyy");
                                        value3 = valueRed;
                                    }
                                    catch { }

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
                                    catch
                                    {

                                    }

                                    try
                                    {
                                        string valueRed = DateTime.ParseExact(value3, "dd-MM-yyyy HH:mm:ss", null).ToString("dd-MM-yyyy");
                                        value3 = valueRed;
                                    }
                                    catch
                                    {

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

                        qryStr.Clear();
                        qryStr.Append("Select Value from Settings where parameter='HRAPPROVER'");
                        var HRSTAFFID = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
                        var reportingmanager = string.Empty;
                        string ReportingManagerEmailId = string.Empty;
                        var aa = new CommonRepository();
                        string Staffname = Cm.GetStaffName(_Sta.Id);
                        reportingmanager = aa.GetReportingManager(Loggedinuser);
                        string ModifiedStaffname = Cm.GetStaffName(Loggedinuser);
                        //string FromMailid = Cm.GetEmailIdOfEmployee(Loggedinuser);
                        string FromMailid = Cm.GetEmailFromAdd();
                        if (!string.IsNullOrEmpty(reportingmanager).Equals(true))
                        {
                            ReportingManagerEmailId = aa.GetEmailIdOfEmployee(reportingmanager);
                        }
                        else
                        {
                            throw new ApplicationException("Your reporting manager not yet configured so you cannot make any changes.");
                        }
                        string mailToBeSentForStaffEditRequest = string.Empty;
                        try
                        {
                            mailToBeSentForStaffEditRequest = ConfigurationManager.AppSettings["MailToBeSentForStaffEditRequest"].ToString();
                        }
                        catch
                        {
                            throw new ApplicationException("The value of the mail to be sent for staff edit request is empty.");
                        }

                        if (mailToBeSentForStaffEditRequest.Equals("ReportingManager"))
                        {
                            string reportingManagerName = Cm.GetStaffName(reportingmanager);
                            MailMess = "<html><body><p>Hi " + reportingManagerName + ",<br />Following below are the personal/official details modified by employee" + Staffname + " - (" + Loggedinuser + ")<br />Kindly approve or reject the changes.</p><p style=\"font-family:tahoma; font-size:9pt;\">" + Content + "<br>Your attention is required for this application.</p><p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">10.114.76.61:8011</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">Atrak - Auto Generated Mail</p></body></html>";
                            Cm.SendEmailMessage(FromMailid, ReportingManagerEmailId, CC, "", "Approval Requisition for the changes made in Staff Details by " + Staffname + " (" + Loggedinuser + ")", MailMess);
                            Cm.SaveIntoApplicationApproval(Maxid, "SR", Loggedinuser, reportingmanager, false);
                            Message = "Request sent, It will get effected after the ReportingManager approval.";
                        }
                        else if (mailToBeSentForStaffEditRequest.Equals("OneAboveManager"))
                        {
                            string oneAboveReportingManager = string.Empty;
                            string oneAboveReportingManagerilId = string.Empty;
                            string oneAboveReportingManagerName = string.Empty;
                            oneAboveReportingManager = aa.GetReportingManager(reportingmanager);
                            oneAboveReportingManagerName = Cm.GetStaffName(oneAboveReportingManager);
                            if (!string.IsNullOrEmpty(oneAboveReportingManager).Equals(true))
                            {
                                oneAboveReportingManagerilId = Cm.GetEmailIdOfEmployee(oneAboveReportingManager);
                            }
                            else
                            {
                                oneAboveReportingManager = reportingmanager;
                                oneAboveReportingManagerilId = Cm.GetEmailIdOfEmployee(reportingmanager);
                            }

                            MailMess = "<html><body><p>Hi " + oneAboveReportingManagerName + ",<br />Following below are the personal/official details modified by employee- " + Staffname + " - " + Loggedinuser + ". .<br />Kindly approve or reject the changes.</p><p>" + Content + "<br><a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + oneAboveReportingManager + "&ApplicationApprovalId=" + Maxid + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + oneAboveReportingManager + "&ApplicationApprovalId=" + Maxid + "&Approve=false\">Reject</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">Atrak - Auto Generated Mail</p></body></html>";
                            Cm.SendEmailMessage(FromMailid, oneAboveReportingManagerilId, CC, "", "Approval Requisition for the changes made in Staff Details by " + Staffname + " (" + Loggedinuser + ")", MailMess);
                            Cm.SaveIntoApplicationApproval(Maxid, "SR", Loggedinuser, oneAboveReportingManager, false);
                            Message = "Request sent, It will get effected after the one above manager approval.";
                        }
                        else
                        {
                            string HRMailid = Cm.GetEmailIdOfEmployee(HRSTAFFID);
                            string HRName = string.Empty;
                            HRName = Cm.GetStaffName(HRSTAFFID);
                            MailMess = "<html><body><p>Hi " + HRName + ",<br />Following below are the personal/official details modified by employee- " + Staffname + " - " + Loggedinuser + ". .<br />Kindly approve or reject the changes.</p><p>" + Content + "<br><a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + HRSTAFFID + "&ApplicationApprovalId=" + Maxid + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + HRSTAFFID + "&ApplicationApprovalId=" + Maxid + "&Approve=false\">Reject</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">Atrak - Auto Generated Mail</p></body></html>";
                            Cm.SendEmailMessage(FromMailid, ReportingManagerEmailId, CC, "", "Approval Requisition for the changes made in Staff Details by " + Staffname + " (" + Loggedinuser + ")", MailMess);
                            Cm.SaveIntoApplicationApproval(Maxid, "SR", Loggedinuser, HRSTAFFID, false);
                            Message = "Request sent, It will get effected after the HR approval.";
                        }
                    }
                    else
                    {
                        Message = "There is no changes is done to send a Request";
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
            qrystr.Append("select ScreenName as TableName,ColumnName from AdditionalField where id= @value1");
            var data = context.Database.SqlQuery<AdditionalFieldModel>(qrystr.ToString(), new SqlParameter("@value1", value1)).FirstOrDefault();

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
            SqlParameter[] sqlParameter = new SqlParameter[3];
            sqlParameter[0] = new SqlParameter("@Tablename", Tablename);
            sqlParameter[1] = new SqlParameter("@columnname", columnname);
            sqlParameter[2] = new SqlParameter("@value", value);

            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("select dbo.fnGetMasterNameforStaff ( @Tablename,@columnname,@value)");
            var data = context.Database.SqlQuery<string>(qrystr.ToString(),sqlParameter).FirstOrDefault();
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
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@Id", Id);
            sqlParameter[1] = new SqlParameter("@Tag", Tag);


            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select dbo.[fnGetMastName] (@Id,@Tag)");
            var Name = context.Database.SqlQuery<string>(qryStr.ToString(), sqlParameter).FirstOrDefault();
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

            context.Database.ExecuteSqlCommand(qryStr.ToString(),new SqlParameter("@id",id));
        }

        public List<StaffFamilyInformation> GetFamilyFromDB(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("select id , staffid , relatedas , name , age from stafffamily where staffid = @staffid");

            try
            {
                var lst = context.Database.SqlQuery<StaffFamilyInformation>(qryStr.ToString(),new SqlParameter("@staffid", staffid))
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
            context.Database.ExecuteSqlCommand(qryStr.ToString(),new SqlParameter("@id", id));
        }

        public List<StaffEducationInformation> GetEducationFromDB(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select id , staffid , coursename , university , " +
                          "completed, completionyear , percentage , " +
                          "grade from staffeducation where staffid = @staffid");

            try
            {
                var lst =
                context.Database.SqlQuery<StaffEducationInformation>(qryStr.ToString(),new SqlParameter("@staffid", staffid))
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

        public List<WorkingDayPatternList> GetAllWorkingDayPattern()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select Id , PatternDesc  from WorkingDayPattern Where IsActive = 1");

            try
            {
                var lst = context.Database.SqlQuery<WorkingDayPatternList>(qryStr.ToString()).Select(d => new WorkingDayPatternList()
                {
                    Id = d.Id,
                    PatternDesc = d.PatternDesc ,
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
            qryStr.Append("select Id , Name from company Where IsActive = 1");

            try
            {
                var lst = context.Database.SqlQuery<CompanyList>(qryStr.ToString()).Select(d => new CompanyList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    LegalName = d.LegalName,
                    Website = d.Website,
                    RegisterNo = d.RegisterNo,
                    TNGSNo = d.TNGSNo,
                    CSTNo = d.CSTNo,
                    TINNo = d.TINNo,
                    ServiceTaxNo = d.ServiceTaxNo,
                    PANNo = d.PANNo,
                    PFNo = d.PFNo,
                    IsActive = d.IsActive
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
            qryStr.Append("select Id , Name from branch Where IsActive = 1");

            try
            {
                var lst = context.Database.SqlQuery<BranchList>(qryStr.ToString()).Select(d => new BranchList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    Address = d.Address,
                    City = d.City,
                    District = d.District,
                    State = d.State,
                    Country = d.Country,
                    PostalCode = d.PostalCode,
                    Phone = d.Phone,
                    Fax = d.Fax,
                    Email = d.Email,
                    IsHeadOffice = d.IsHeadOffice,
                    IsActive = d.IsActive,
                    CompanyID = d.CompanyID
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
            qryStr.Append("select Id , Name from Department Where IsActive = 1 Order By Name Asc");

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
            qryStr.Append("select Id , Name from Division Where IsActive = 1 Order By Name Asc");


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
            queryString.Append("Select Id,Name,ShortName from [Volume] where IsActive = 1 Order By Name Asc");
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
            qryStr.Append("select Id , Name from [Designation] Where IsActive = 1 Order by Name Asc");
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
            qryStr.Append("select Id , Name from Grade Where IsActive = 1 Order By Name Asc");

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
            qryStr.Append("select Id , Name from StaffStatus Where IsActive = 1 Order By Name Asc");

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
            catch (Exception)
            {
                return new List<StaffStatusList>();
            }
        }

        public List<MaritalStatus> GetAllMaritalStatus()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("select Id , Name from maritalstatus Where IsActive = 1");

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

            qryStr.Append("select Id , Name from BloodGroup");

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
            qryStr.Append("select Id , Name from leavegroup Where IsActive = 1");

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
            qryStr.Append("select Convert ( varchar, Id) as Id , Name from holidayzone Where IsActive = 1");

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
            qryStr.Append("select Id , Name from weeklyoffs Where IsActive = 1");

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
            qryStr.Append("select Id , StaffId , RelatedAs , Name , Age from stafffamily where staffid = @parameter1");

            try
            {
                var lst =
                context.Database.SqlQuery<StaffFamilyInformation>(qryStr.ToString(), new SqlParameter("@parameter1", staffid))
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
                          "Grade from staffeducation where staffid = @parameter1");
            try
            {
                var lst =
                    context.Database.SqlQuery<StaffEducationInformation>(qryStr.ToString(), new SqlParameter("@parameter1", staffid))
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
            qryStr.Append("select Convert( varchar, Id) as Id, Name from salutation where IsActive = 1");

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
            qryStr.Append("select Convert ( varchar, id ) as id , Name from RuleGroup where isactive = 1");

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

        public List<CategoryList> GetAllCategories()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Id , Name from Category Where IsActive = 1");

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
            qryStr.Append("select Id , Name from CostCentre Where IsActive = 1");

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

        public List<LocationList> GetAllLocations(string role, string Location)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            List<LocationList> locations = new List<LocationList>();

            if (role == "3" || role == "5")
            {
                qryStr.Append("select Id , Name from [Location] Where IsActive = 1 Order By Name Asc");
                locations = context.Database.SqlQuery<LocationList>(qryStr.ToString()).ToList();
            }
            else
            {
                qryStr.Append("select Id , Name from [Location] where id = @Location");
                locations = context.Database.SqlQuery<LocationList>(qryStr.ToString(),new SqlParameter("@Location", Location)).ToList();
            }

            try
            {
                var lst = locations.Select(d => new LocationList()
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
            qryStr.Append("SELECT Id , Name FROM [SecurityGroup] Where IsActive = 1");

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
            var lst = context.Database.SqlQuery<StaffEditRequest>(qrystr.ToString(),new SqlParameter(@ApplicationApprovalId, ApplicationApprovalId)).FirstOrDefault();

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
                SqlParameter[] sqlParameter = new SqlParameter[2];
                sqlParameter[0] = new SqlParameter("@dataValue", dataValue);
                sqlParameter[1] = new SqlParameter("@staffid", staffid);

                qrystr.Append("update staff set @dataValue where id= @staffid");
                context.Database.ExecuteSqlCommand(qrystr.ToString(),sqlParameter);


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
                            catch (Exception e)
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
                            catch (Exception e)
                            {
                                dataValue = value1 + "=" + "'" + value2 + "'";
                            }

                        }
                    }
                }
                SqlParameter[] sqlParameter = new SqlParameter[2];
                sqlParameter[0] = new SqlParameter("@dataValue", dataValue);
                sqlParameter[1] = new SqlParameter("@staffid", staffid);
                qrystr.Append("update Staffofficial set @dataValue where staffid=@staffid");
                context.Database.ExecuteSqlCommand(qrystr.ToString(),sqlParameter);


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
                            catch (Exception e)
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
                            catch (Exception e)
                            {
                                dataValue = value1 + "=" + "'" + value2 + "'";
                            }
                        }
                    }

                }
                SqlParameter[] sqlParameter = new SqlParameter[2];
                sqlParameter[0] = new SqlParameter("@dataValue", dataValue);
                sqlParameter[1] = new SqlParameter("@staffid", staffid);

                qrystr.Append("update staffpersonal set @dataValue where staffid=@staffid");
                context.Database.ExecuteSqlCommand(qrystr.ToString(),sqlParameter);

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

                        SqlParameter[] sqlParameter = new SqlParameter[3];
                        sqlParameter[0] = new SqlParameter("@value2", value2);
                        sqlParameter[1] = new SqlParameter("@staffid", staffid);
                        sqlParameter[2] = new SqlParameter("@value1", value1);

                        qrystr.Append("update AdditionalFieldValue set ActualValue= @value2 where staffid= @staffid and AddfId=@value1");
                        context.Database.ExecuteSqlCommand(qrystr.ToString(),sqlParameter);
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
            var lst = context.Database.SqlQuery<StaffEditReqModel>(qrystr.ToString(),new SqlParameter("@Staffid", Staffid)).ToList();
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
                                catch (Exception e) { }

                                try
                                {
                                    string valueRed = DateTime.ParseExact(value3, "dd-MM-yyyy HH:mm:ss", null).ToString("dd-MM-yyyy");
                                    value3 = valueRed;
                                }
                                catch (Exception e) { }

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
                                catch (Exception e) { }

                                try
                                {
                                    string valueRed = DateTime.ParseExact(value3, "dd-MM-yyyy HH:mm:ss", null).ToString("dd-MM-yyyy");
                                    value3 = valueRed;
                                }
                                catch (Exception e) { }

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
                                catch (Exception e) { }

                                try
                                {
                                    string valueRed = DateTime.ParseExact(value3, "dd-MM-yyyy HH:mm:ss", null).ToString("dd-MM-yyyy");
                                    value3 = valueRed;
                                }
                                catch (Exception e) { }

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
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@staffid", staffid ?? "");

            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("Select B.Id,A.Staffid,B.ColumnName,B.ScreenName as TableName,B.[Type],B.Access,A.ActualValue as Value from AdditionalFieldValue A join AdditionalField B on A.AddfId=B.Id where staffid= @staffid");
            var data = context.Database.SqlQuery<AdditionalFieldModel>(qrystr.ToString(), sqlParameter).ToList();
            if (data != null)
            {

                return data;
            }
            else
            {
                return new List<AdditionalFieldModel>();
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
            catch (Exception e)
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
            catch (Exception e)
            {
                return new List<ShiftView>();
            }
        }
        public string BulkSaveStaffInformationToDB(List<EmployeeImportModel> list)
        {
            using (DbContextTransaction trans = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (EmployeeImportModel data in list)
                    {
                        Staff staff = new Staff();
                        staff.Id = data.StaffId;
                        staff.StaffStatusId = 1;
                        staff.SalutationId = 1;
                        staff.PeopleSoftCode = "-";
                        staff.FirstName = data.StaffName;
                        staff.LastName = "-";
                        staff.Gender = data.Gender;
                        staff.CreatedOn = DateTime.Now;
                        staff.IsSentToSMax = false;
                        staff.IsHidden = false;
                        staff.IsAttached = false;

                        context.Staff.AddOrUpdate(staff);
                        context.SaveChanges();

                        StaffOfficial official = new StaffOfficial();
                        official.StaffId = data.StaffId;
                        official.CompanyId = "C00001";
                        official.LocationId = data.LocationId;
                        official.BranchId = data.BranchId;
                        official.DepartmentId = data.DepartmentId;
                        official.DivisionId = "DV0001";
                        official.DesignationId = data.DesignationId;
                        official.GradeId = "GD0001";
                        official.LeaveGroupId = data.LeaveGroupId;
                        official.HolidayGroupId = data.HolidayZoneId;
                        official.PolicyId = data.PolicyId;
                        official.CategoryId = data.CategoryId;
                        official.CostCentreId = data.CostCentreId;
                        official.SecurityGroupId = data.SecurityGroupId;
                        official.DateOfJoining = data.DOJ;
                        official.IsConfirmed = false;
                        official.Email = data.OfficialEmail;
                        official.ExtensionNo = 0;
                        official.WorkingDayPatternId = data.WorkingDayPatternId;
                        official.ReportingManager = data.ReportingManager;
                        official.ApproverLevel = data.ApproverLevel;
                        if (data.ApproverLevel == 1)
                        {
                            official.Reviewer = data.ReportingManager;
                        }
                        else
                        {
                            official.Reviewer = data.Reviewer;
                        }
                        official.Canteen = false;
                        official.Travel = false;
                        official.IsWorkingDayPatternLocked=false;
                        official.IsLeaveGroupLocked = false;
                        official.IsHolidayGroupLocked = false;
                        official.IsWeeklyOffLocked = false;
                        official.IsPolicyLocked = false;
                        official.SalaryDay = 1;
                        official.Interimhike = false;
                        official.IsAutoShift = false;
                        official.IsGeneralShift = true;
                        official.IsShiftPattern = false;
                        official.Isflexishift = false;
                        official.ShiftId = data.ShiftId;
                        official.ShiftPatternId = 1;

                        context.StaffOfficial.AddOrUpdate(official);
                        context.SaveChanges();

                        StaffPersonal personal = new StaffPersonal();
                        personal.StaffId = data.StaffId;
                        personal.StaffBloodGroup = data.BloodGroupId;
                        personal.StaffMaritalStatus = data.MaritalStatusId;
                        personal.DateOfBirth = data.DOB;
                        personal.MarriageDate = data.MarriageDate;

                        context.StaffPersonal.AddOrUpdate(personal);
                        context.SaveChanges();
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
    }
}
