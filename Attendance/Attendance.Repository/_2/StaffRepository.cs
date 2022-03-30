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
using System.Configuration;

namespace Attendance.Repository
{
    public class StaffRepository : TrackChangeRepository
    {

        private AttendanceManagementContext context = null;

        public StaffRepository()
        {
            context = new AttendanceManagementContext();
        }

        public StaffInformation GetStaffMainInformation(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select Id , StaffStatusId , CardCode , " +
                          "FirstName , MiddleName , LastName , " +
                          "ShortName , Gender, SalutationId , IsHidden , CreatedOn , CreatedBy from Staff " +
                          "where id = '" + staffid + "'");
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
            catch (Exception)
            {
                return new StaffInformation();
            }
        }

        public StaffOfficialInformation GetStaffOfficialInformation(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append(" Select StaffId , CompanyId , BranchId , DepartmentId , DivisionId ,VolumeId , " +
                          "DesignationId , GradeId , LeaveGroupId , WeeklyOffId , HolidayGroupId , PolicyId, Canteen, Travel, SalaryDay, IsConfirmed, " +
                          "ConfirmationDate, IsWorkingDayPatternLocked, IsLeaveGroupLocked, IsHolidayGroupLocked, " +
                          "IsWeeklyOffLocked, IsPolicyLocked, " + "DateOfJoining , ResignationDate , DateOfRelieving , Phone , Fax , Email , ExtensionNo , Interimhike , Tenure , PFNo ," +
                          "ESINo ,CategoryId ,CostCentreId ,LocationId , SecurityGroupId , WorkingDayPatternId , ReportingManager ," +
                          "b.FirstName as ReportingManagerName , DomainId " +
                          "from staffofficial  a left join staff b on a.reportingmanager = b.id where staffid = '" + staffid + "'");

            try
            {
                var so =
                context.Database.SqlQuery<StaffOfficialInformation>(qryStr.ToString())
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
                            Phone = d.Phone,
                            Fax = d.Fax,
                            Email = d.Email,
                            ExtensionNo = d.ExtensionNo,
                            WorkingDayPatternId = d.WorkingDayPatternId,
                            ReportingManager = d.ReportingManager,
                            ReportingManagerName = d.ReportingManagerName,
                            IsConfirmed = d.IsConfirmed,
                            ConfirmationDate = d.ConfirmationDate,
                            IsWorkingDayPatternLocked = d.IsWorkingDayPatternLocked,
                            IsLeaveGroupLocked = d.IsLeaveGroupLocked,
                            IsHolidayGroupLocked = d.IsHolidayGroupLocked,
                            IsWeeklyOffLocked = d.IsWeeklyOffLocked,
                            IsPolicyLocked = d.IsPolicyLocked,
                            DomainId = d.DomainId
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
            catch (Exception)
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
                          "PassportNo , DrivingLicense , BankName , BankACNo , BankIFSCCode , " +
                          "BankBranch from staffpersonal where staffid = '" + staffid + "'");

            try
            {
                var sp =
                    context.Database.SqlQuery<StaffPersonalInformation>(qryStr.ToString())
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
                            BankBranch = d.BankBranch
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
            qryStr.Append("select count(*) as TotalCount from staff where id ='" + staffid + "'");
            var count = context.Database.SqlQuery<int>(qryStr.ToString());
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

        //public void SaveStaffInformationToDB(Staff _Sta,  StaffPersonal _SP, StaffOfficial _SO, StaffFamily _SF, StaffEducation _SE)
        public void SaveStaffInformationToDB(Staff _Sta, StaffPersonal _SP, StaffOfficial _SO, EmployeePhoto _EP, bool AddMode)
        {
            string baseuri = string.Empty;
            string requesturi = string.Empty;
            Int64 Cardno = 0;
            string json = string.Empty;
            string considerStaffPersonal = string.Empty;
            considerStaffPersonal = ConfigurationManager.AppSettings["ConsiderStaffPersonal"].ToString();

            baseuri = ConfigurationManager.AppSettings["BASEURI"];
            requesturi = ConfigurationManager.AppSettings["REQUESTURI"];

            SMaxData smaxdata = null;
            List<SMaxData> _lst_ = null;

            using (var trans = context.Database.BeginTransaction())
            {

                try
                {
                    if (AddMode == true) //if addition mode then...
                    {
                        Cardno = Convert.ToInt64(Getmaxid());
                        Cardno = Cardno + 1;
                        context.Staff.AddOrUpdate(_Sta);
                        context.StaffPersonal.AddOrUpdate(_SP);
                        context.StaffOfficial.AddOrUpdate(_SO);
                        context.SaveChanges();
                        if (_EP != null)
                        {
                            context.EmployeePhoto.AddOrUpdate(_EP);
                            context.SaveChanges();
                        }

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
                    
                        if (_EP != null)
                        {
                            context.EmployeePhoto.AddOrUpdate(_EP);
                            context.SaveChanges();
                        }
                    }


                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }

            if (AddMode == true) //if addition mode then...
            {
                //if (IsStaffExists(_Sta.Id) == true) //if the staffid is already existing then...
                //{
                //    //throw error.
                //    throw new Exception("Staff id already exists. Try entering a new Staff id.");
                //}

                Cardno = Convert.ToInt64(Getmaxid());
                Cardno = Cardno + 1;
            }
            else
            {
                var qryStr = new StringBuilder();
                qryStr.Clear();
                qryStr.Append("select ch_cardno from MPTSmaxV2.dbo.smx_cardholder where  ch_empid = @parameter1");
                decimal count = context.Database.SqlQuery<decimal>(qryStr.ToString(), new SqlParameter("@parameter1", _Sta.Id)).FirstOrDefault();

                if (count != 0)
                {
                    Cardno = Convert.ToInt64(count);
                }
            }

            // Assigning values to smaxdata 
            _lst_ = new List<SMaxData>();
            smaxdata = new SMaxData();

            smaxdata.StaffId = _SO.StaffId;
            smaxdata.FName = _Sta.FirstName;
            smaxdata.LName = _Sta.LastName;
            smaxdata.ShortName = _Sta.ShortName;
            if (string.IsNullOrEmpty((_SP.DateOfBirth).ToString()) == false)
            {

                smaxdata.DOB = Convert.ToDateTime(_SP.DateOfBirth);
            }
            else
            {
                smaxdata.DOB = null;
            }

            if (string.IsNullOrEmpty(_Sta.Gender.ToString()) == true)
            {
                smaxdata.Gender = "M";
            }
            else
            {
                smaxdata.Gender = _Sta.Gender.ToString();
            }

            if (smaxdata.Gender == "M")
            {
                smaxdata.Title = "Mr";
            }
            else
            {
                smaxdata.Title = "Miss";
            }

            if (string.IsNullOrEmpty(_SO.Phone) == true)
            {
                smaxdata.Phone = null;
            }
            else
            {
                smaxdata.Phone = _SO.Phone.ToString();
            }

            if (string.IsNullOrEmpty(_SO.Email) == true)
            {
                smaxdata.Phone = null;
            }
            else
            {
                smaxdata.Phone = _SO.Email.ToString();
            }

            if (_Sta.StaffStatusId != 1)
            {
                smaxdata.Status = "HotList";
            }
            else
            {
                smaxdata.Status = "ACTIVE";
            }


            if (string.IsNullOrEmpty((_SO.DateOfJoining).ToString()) == false)
            {

                smaxdata.DOJ = _SO.DateOfJoining;
            }
            else
            {
                smaxdata.DOJ = null;
            }

            smaxdata.Cardnumber = Cardno.ToString();
            smaxdata.DOS = null;
            smaxdata.Company = Getmastervalue(_SO.CompanyId, "C");
            smaxdata.Company = smaxdata.Company.Replace("\"", "");
            smaxdata.Location = Getmastervalue(_SO.LocationId, "L");
            smaxdata.Branch = Getmastervalue(_SO.BranchId, "BR");
            smaxdata.Department = Getmastervalue(_SO.DepartmentId, "DP");
            smaxdata.Division = Getmastervalue(_SO.DivisionId, "DV");
            smaxdata.Designation = Getmastervalue(_SO.DesignationId, "DG");
            smaxdata.Grade = Getmastervalue(_SO.GradeId, "GD");
            smaxdata.Plant = "DEFAULT";

            _lst_.Add(smaxdata);

            json = JsonConvert.SerializeObject(_lst_);

            //calling API method
         //   SendDataToSmax(baseuri, requesturi, json).Wait();


        }

        private string Getmastervalue(string Id, string Tag)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select dbo.[fnGetMastName] ('" + Id + "','" + Tag + "')");
            var Name = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault();
            return Name;
        }

        private decimal Getmaxid()
        {

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select max(ch_cardno) from MPTSmaxV2.dbo.smx_cardholder");
            decimal Maxid = context.Database.SqlQuery<decimal>(qryStr.ToString()).FirstOrDefault();
            return Maxid;
        }



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
            qryStr.Append("delete from stafffamily where id = '" + id + "'");

            context.Database.ExecuteSqlCommand(qryStr.ToString());
        }

        public List<StaffFamilyInformation> GetFamilyFromDB(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("select id , staffid , relatedas , name , age from stafffamily where staffid = '" + staffid + "'");

            try
            {
                var lst = context.Database.SqlQuery<StaffFamilyInformation>(qryStr.ToString())
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
            qryStr.Append("delete from staffeducation where id = '" + id + "'");
            context.Database.ExecuteSqlCommand(qryStr.ToString());
        }

        public List<StaffEducationInformation> GetEducationFromDB(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select id , staffid , coursename , university , " +
                          "completed, completionyear , percentage , " +
                          "grade from staffeducation where staffid = '" + staffid + "'");

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
            qryStr.Append("select Id , PsCode , WorkingPattern from workingdaypattern");

            try
            {
                var lst = context.Database.SqlQuery<WorkingDayPatternList>(qryStr.ToString()).Select(d => new WorkingDayPatternList()
                {
                    Id = d.Id,
                    PsCode = d.PsCode,
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
            qryStr.Append("select Id , Name from company");

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
            qryStr.Append("select Id , Name from branch");

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
            qryStr.Append("select Id , Name from Department Order By Name Asc");

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
            qryStr.Append("select Id , Name from Division Order By Name Asc");


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
            qryStr.Append("select Id , Name from [Designation] order by Name Asc");
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
            qryStr.Append("select Id , Name from Grade Order By Name Asc");

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
            qryStr.Append("select Id , Name from StaffStatus Order By Name Asc");

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

            qryStr.Append("select Id , Name from maritalstatus");

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
            qryStr.Append("select Id , Name from leavegroup");

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
            qryStr.Append("select Convert ( varchar, Id) as Id , Name from holidayzone");

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
            qryStr.Append("select Id , Name from weeklyoffs");

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
            qryStr.Append("select Id , Name from Category");

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
            qryStr.Append("select Id , Name from CostCentre");

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
            qryStr.Append("select Id , Name from [Location] Order By Name Asc");

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
            qryStr.Append("SELECT Id , Name FROM [SecurityGroup]");

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
    }
}
