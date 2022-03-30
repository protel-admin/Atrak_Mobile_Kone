using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;
using System.Configuration;
using Newtonsoft.Json;
using System.Data.Entity.Validation;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

namespace Attendance.Repository
{
   public  class EmployeeImportRepository : IDisposable
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

        public EmployeeImportRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<EmployeeImportResultMesss> ImportStaffDetails(List<EmployeeImportStaff> ei)
        {
            string ResultMessage = String.Empty;

            List<EmployeeImportResultMesss> EM = new List<EmployeeImportResultMesss>();
            EmployeeImportResultMesss EIm = new EmployeeImportResultMesss();
            foreach(var EIS1 in ei)
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        EmployeeImportStaff EIS = new EmployeeImportStaff();

                        EIS.Empcode = EIS1.Empcode;
                        EIS.FirstName = EIS1.FirstName;
                        EIS.MiddleName = EIS1.MiddleName;
                        EIS.LastName = EIS1.LastName;
                        EIS.Gender = EIS1.Gender;
                        EIS.DOB = EIS1.DOB;
                        EIS.Address = EIS1.Address;
                        EIS.City = EIS1.City;
                        EIS.PostalCode = EIS1.PostalCode;
                        EIS.State = EIS1.State;
                        EIS.Country = EIS1.Country;
                        EIS.PANNO = EIS1.PANNO;
                        EIS.BankACNo = EIS1.BankACNo;
                        EIS.BankName = EIS1.BankName;
                        EIS.BankIFSCCode = EIS1.BankIFSCCode;
                        EIS.AadharNo = EIS1.AadharNo;
                        EIS.PassPortNo = EIS1.PassPortNo;
                        EIS.DOJ = EIS1.DOJ;
                        EIS.Designation = EIS1.Designation;
                        EIS.Grade = EIS1.Grade;
                        EIS.Department = EIS1.Department;
                        EIS.Company = EIS1.Company;
                        EIS.Location = EIS1.Location;
                        EIS.Branch = EIS1.Branch;
                        EIS.Division = EIS1.Division;
                        EIS.Category = EIS1.Category;
                        EIS.OfficialMobile = EIS1.OfficialMobile;
                        EIS.PersonalMobile = EIS1.PersonalMobile;
                        EIS.Email = EIS1.Email;                        
                        EIS.CostCentre = EIS1.CostCentre;
                        EIS.District = EIS1.District;
                        EIS.MedicalClaimNumber = EIS1.MedicalClaimNumber;

                        //self
                        EIS.Approval = EIS1.Approval;
                        EIS.Reviewer = EIS1.Reviewer;
                        EIS.ReportingManagerid = EIS1.ReportingManagerid;
                        EIS.Shifttype = EIS1.Shifttype;
                        EIS.Flexishift = EIS1.Flexi;
                        EIS.ShiftPatternId = EIS1.ShiftPattern;
                        EIS.ShiftId = EIS1.General;
                        EIS.Workingpattern = EIS1.Workingpattern;
                        EIS.Workingdaypattern = EIS1.Workingdaypattern;
                        EIS.weeklyoff = EIS1.weeklyoff;
                        
                       // EIS.weeklyoff = EIS1.weeklyoff;
                        EIS.Policygroup = EIS1.Policygroup;
                        EIS.SecurityGroup = EIS1.SecurityGroup;

                        //EIS.OTReviewer = EIS1.OTReviewer;
                        //EIS.OTReportingManager = EIS1.OTReportingManager;
                        EIS.PFNo = EIS1.PFNo;
                        EIS.ESINo = EIS1.ESINo;
                        EIS.PresentAddress = EIS1.PresentAddress;
                        EIS.EmergencyContactPerson = EIS1.EmergencyContactPerson;
                        EIS.EmergencyContactNumber = EIS1.EmergencyContactNumber;


                        Staff sf = new Staff();
                        sf.Id = EIS.Empcode;
                        sf.FirstName = EIS.FirstName;
                        sf.MiddleName = EIS.MiddleName;
                        sf.LastName = EIS.LastName;
                        sf.CreatedOn = DateTime.Now;
                        sf.CreatedBy = "-";
                        sf.StaffStatusId = 1;
                        sf.Gender = EIS.Gender;
                        sf.IsHidden = false;
                        sf.IsSentToSMax = false;
                        if (sf.Gender == "M")
                        {
                            sf.SalutationId=1;
                        }
                        else
                        {

                            sf.SalutationId = 7;
                        }
                        context.Staff.AddOrUpdate(sf);
                        context.SaveChanges();
                        StaffOfficial _SO = new StaffOfficial();

                        _SO.StaffId = EIS.Empcode;
                        _SO.CompanyId = GetCompanyid(EIS.Company);
                        _SO.BranchId = GetBranch(EIS.Branch);
                        _SO.DepartmentId = GetDepartment(EIS.Department);
                        _SO.DivisionId = GetDivision(EIS.Division);

                        _SO.DesignationId = GetDesignation(EIS.Designation);
                        _SO.GradeId = GetGrade(EIS.Grade);
                      //  _SO.DateOfJoining = Convert.ToDateTime(EIS.DOJ);
                        if (!string.IsNullOrEmpty(EIS.DOJ))
                        {
                            DateTime DOJ = DateTime.Parse(EIS.DOJ);
                            _SO.DateOfJoining = DOJ;

                        }

                        //_SO.WeeklyOffId = "WO00000002";
                        _SO.PolicyId = 1;
                        _SO.SecurityGroupId = 2;
                        _SO.HolidayGroupId = 1;
                        _SO.LeaveGroupId = "LG0001";
                        _SO.Phone = EIS.OfficialMobile;
                        _SO.IsConfirmed = false;
                        _SO.Email = EIS.Email;
                        _SO.ExtensionNo = 0;
                        _SO.CategoryId = GetCategory(EIS.Category);
                        _SO.CostCentreId = GetCostCenter(EIS.CostCentre);
                        _SO.LocationId = GetLocation(EIS.Location);
                        _SO.ReportingManager = EIS.ReportingManagerid;
                        //_SO.WorkingDayPatternId = 1;
                        _SO.Canteen = false;
                        _SO.Travel = false;
                        _SO.IsWeeklyOffLocked = true;
                        _SO.IsWorkingDayPatternLocked = true;
                        _SO.IsLeaveGroupLocked = true;
                        _SO.IsHolidayGroupLocked = true;
                        _SO.IsPolicyLocked = true;
                        _SO.SalaryDay = 1;
                        _SO.Interimhike = false; ;
                        _SO.Tenure = 0;
                        
                        _SO.MedicalClaimNumber = EIS.MedicalClaimNumber;
                        _SO.Reviewer = EIS.Reviewer;
                        _SO.PFNo = EIS.PFNo;
                        _SO.ESINo = EIS.ESINo;
                        //Approve Level Validation
                        if (EIS.Approval == "1")
                        {
                            _SO.Reviewer = EIS.Reviewer;
                            _SO.ReportingManager = EIS.Reviewer;
                            _SO.ApproverLevel =Convert.ToInt16(EIS.Approval);
                        }
                        if (EIS.Approval == "2")
                        {
                            _SO.Reviewer = EIS1.Reviewer;
                            _SO.ReportingManager = EIS1.ReportingManagerid;
                            _SO.ApproverLevel =Convert.ToInt16(EIS.Approval);
                        }
                        //ShiftWise Validation                       
                        if (EIS.Shifttype == "Flexi")
                        {
                           _SO.Isflexishift = true;
                           _SO.IsGeneralShift = false;
                           _SO.IsShiftPattern = false;
                           _SO.IsAutoShift = false;
                            //_SO.Flexishift =Convert.ToDateTime(EIS1.Flexi);
                            if (!string.IsNullOrEmpty(EIS.Flexi))
                            {
                                DateTime DOJ = DateTime.Parse(EIS.Flexi);
                                _SO.Flexishift = DOJ;

                            }
                           
                        }
                        if (EIS.Shifttype == "Auto")
                        {
                            _SO.Isflexishift = false;
                            _SO.IsGeneralShift = false;
                            _SO.IsShiftPattern = false;
                            _SO.IsAutoShift = true;
                        }
                        if (EIS1.Shifttype == "ShiftPattern")
                        {
                           _SO.Isflexishift = false;
                           _SO.IsGeneralShift = false;
                           _SO.IsShiftPattern = true;
                           _SO.IsAutoShift = false;
                           //_SO.ShiftPatternId =Convert.ToInt16(EIS1.ShiftPattern);
                           _SO.ShiftPatternId =1;
                        }
                        _SO.ShiftPatternId = 1;
                        //_SO.ShiftPatternId = 1;
                        if (EIS1.Shifttype == "General")
                        {
                            _SO.Isflexishift = false;
                            _SO.IsGeneralShift = true;
                            _SO.IsShiftPattern = false;
                            _SO.IsAutoShift = false;
                            _SO.ShiftId = EIS1.General;
                        }
                        //_SO.ShiftId = EIS1.General;
                        _SO.ShiftId = "SH0003";
                        //weeklyoff leave Pattern
                        if (EIS.Workingpattern == "Workingday pattern")
                        {
                            _SO.IsWorkingDayPatternLocked = true;
                            _SO.IsWeeklyOffLocked = false;
                            _SO.WorkingDayPatternId = 2;
                            if (EIS.Workingdaypattern == "1W1-6")
                            {
                                _SO.WorkingDayPatternId = 1;
                            }
                            if (EIS.Workingdaypattern == "1W2-5")
                            {
                                _SO.WorkingDayPatternId = 2;
                            }
                            if (EIS.Workingdaypattern == "1W3-5.5")
                            {
                                _SO.WorkingDayPatternId = 3;
                            }
                            if (EIS.Workingdaypattern == "000-24")
                            {
                                _SO.WorkingDayPatternId = 4;
                            }
                            if (EIS.Workingdaypattern == "W1W-13")
                            {
                                _SO.WorkingDayPatternId = 5;
                            }
                            
                        }
                       // _SO.WorkingDayPatternId = 2;
                        if (EIS.Workingpattern == "weekly off")
                        {
                            _SO.IsWeeklyOffLocked = true;
                            _SO.IsWorkingDayPatternLocked = false;
                            if (EIS.weeklyoff == "MONDAY-WO")
                            {
                                _SO.WeeklyOffId = "WO00000001";
                            }
                            if (EIS.weeklyoff == "SUNDAY-WO")
                            {
                                _SO.WeeklyOffId = "WO00000002";
                            }
                            if (EIS.weeklyoff == "TUESDAY-WO")
                            {
                                _SO.WeeklyOffId = "WO00000003";
                            }
                            if (EIS.weeklyoff == "WEDNESDAY-WO")
                            {
                                _SO.WeeklyOffId = "WO00000004";
                            }
                            if (EIS.weeklyoff == "THURSDAY-WO")
                            {
                                _SO.WeeklyOffId = "WO00000005";
                            }
                            if (EIS.weeklyoff == "FRIDAY-WO")
                            {
                                _SO.WeeklyOffId = "WO00000006";
                            }
                            if (EIS.weeklyoff == "SATURDAY-WO")
                            {
                                _SO.WeeklyOffId = "WO00000007";
                            }
                            if (EIS.weeklyoff == "Q")
                            {
                                _SO.WeeklyOffId = "WO00000008";
                            }
                        }
                       // _SO.WeeklyOffId = "WO00000001";
                        context.StaffOfficial.AddOrUpdate(_SO);
                        context.SaveChanges();
                        StaffPersonal _SP = new StaffPersonal();

                        _SP.StaffId = EIS.Empcode;
                        _SP.StaffBloodGroup = 1;
                        _SP.StaffMaritalStatus = 1;
                        _SP.Addr = EIS.Address;
                        _SP.Location = EIS.Location;
                        _SP.City = EIS.City;
                        //_SP.District = EIS.di;
                        _SP.State = EIS.State;
                        _SP.Country = EIS.Country;
                        _SP.PostalCode = EIS.PostalCode;
                        _SP.Phone = EIS.PersonalMobile;
                        //_SP.DateOfBirth = DateTime.Parse(EIS.DOB);
                        if (!string.IsNullOrEmpty(EIS.DOB))
                        {
                            DateTime dob = DateTime.Parse(EIS.DOB);
                            _SP.DateOfBirth = dob;

                        }
                       
                         
                        _SP.Email = EIS.Email;
                        //_SP.MarriageDate = model.StaffPerInfo.MarriageDate;
                        _SP.PANNo = EIS.PANNO;
                        _SP.AadharNo = EIS.AadharNo;
                        _SP.PassportNo = EIS.PassPortNo;
                        // _SP.DrivingLicense = model.StaffPerInfo.DrivingLicense;
                        _SP.BankName = EIS.BankName;
                        _SP.BankACNo = EIS.BankACNo;
                        _SP.BankIFSCCode = EIS.BankIFSCCode;
                        // _SP.BankBranch = EIS.ba;

                        _SP.District = EIS.District;
                        _SP.PresentAddress = EIS.PresentAddress;
                        _SP.EmergencyContactPerson = EIS.EmergencyContactPerson;
                        _SP.EmergencyContactNumber = EIS.EmergencyContactNumber;


                        context.StaffPersonal.AddOrUpdate(_SP);
                        context.SaveChanges();

                        
                        trans.Commit();
                        //if(ResultMessage=="")
                        //{
                        //ResultMessage = EIS.Empcode+"-Saved SuccessFully";
                        //}
                        //else{
                        //    ResultMessage=ResultMessage +","+EIS.Empcode+"-Saved SuccessFully";
                        //}
                    }
                    catch (DbEntityValidationException e)
                    {
                        trans.Rollback();
                        foreach (var exp in e.EntityValidationErrors)
                        {
                            foreach (var ex in exp.ValidationErrors)
                            {
                                //string mess;
                                //mess = "ERROR:"+EIS1.Empcode+"-"+ ex.ErrorMessage + "_" + ex.PropertyName + " - ";
                                //if (ResultMessage == "")
                                //{
                                //    ResultMessage = mess;
                                //}
                                //else
                                //{
                                //    ResultMessage = ResultMessage + "," + mess;
                                //}
                                EIm.Staffid = EIS1.Empcode;
                                EIm.MessageVal = ex.ErrorMessage + "_" + ex.PropertyName;
                                EM.Add(EIm);
                            }
                        }
                    }
                    catch (Exception e)
                     {
                        trans.Rollback();
                       // string mess1;
                        //mess1 = "ERROR:"+EIS1.Empcode+ e.InnerException.Data;
                        //if (ResultMessage == "")
                        //        {
                        //            ResultMessage = mess1;
                        //        }
                        //        else
                        //        {
                        //            ResultMessage = ResultMessage + "," + mess1;
                        //        }
                        EIm.Staffid = EIS1.Empcode;
                        EIm.MessageVal = e.InnerException.Data.ToString();
                        EM.Add(EIm);


                    }
                }
                               
            }

            if(EM.Count!=0)
            {
                return EM;
            }
            return EM;
          //  return ResultMessage;

        }

        private List<EmployeeImportResultMesss> GetErrormessage()
        {
 	       var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("select Staffid,MessageVal from StaffImportError ");

            var lst = context.Database.SqlQuery<EmployeeImportResultMesss>(qrystr.ToString()).ToList();
           return lst;
        }
        

        private String GetCompanyid(string Companyid)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@Companyid", Companyid);
            
            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("select id from company where Name = @Companyid");
          
            var lst = context.Database.SqlQuery<string>(qrystr.ToString(),sqlParameter).FirstOrDefault();
            if(lst==null)
            {
                qrystr.Clear();
                qrystr.Append("select top 1 id from company ");
          
                var Topid = context.Database.SqlQuery<string>(qrystr.ToString()).FirstOrDefault();
                return Topid;
            }
            else
            {
                return lst;
            }
           
        }
        private String GetDesignation(string DesignationId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@DesignationId", DesignationId);
            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("select id from designation where Name= @DesignationId");

            var lst = context.Database.SqlQuery<string>(qrystr.ToString(),sqlParameter).FirstOrDefault();
            if (lst == null)
            {
                qrystr.Clear();
                qrystr.Append("select top 1 id from designation ");

                var Topid = context.Database.SqlQuery<string>(qrystr.ToString()).FirstOrDefault();
                return Topid;
            }
            else
            {
                return lst;
            }

        }
        private String GetGrade(string Grade)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@Grade", Grade);
            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("select id from Grade where Name=@Grade");

            var lst = context.Database.SqlQuery<string>(qrystr.ToString(),sqlParameter).FirstOrDefault();
            if (lst == null)
            {
                qrystr.Clear();
                qrystr.Append("select top 1 id from Grade ");

                var Topid = context.Database.SqlQuery<string>(qrystr.ToString()).FirstOrDefault();
                return Topid;
            }
            else
            {
                return lst;
            }

        }
        private String GetDepartment(string DepartId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@DepartId", DepartId);

            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("select id from Department where Name = @DepartId");

            var lst = context.Database.SqlQuery<string>(qrystr.ToString(),sqlParameter).FirstOrDefault();
            if (lst == null)
            {
                qrystr.Clear();
                qrystr.Append("select top 1 id from Department ");

                var Topid = context.Database.SqlQuery<string>(qrystr.ToString()).FirstOrDefault();
                return Topid;
            }
            else
            {
                return lst;
            }

        }
        private String GetCostCenter(string Costcenter)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@Costcenter", Costcenter);
            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("select id from CostCentre where Name= @Costcenter");

            var lst = context.Database.SqlQuery<string>(qrystr.ToString(),sqlParameter).FirstOrDefault();
            if (lst == null)
            {
                qrystr.Clear();
                qrystr.Append("select top 1 id from CostCentre ");

                var Topid = context.Database.SqlQuery<string>(qrystr.ToString()).FirstOrDefault();
                return Topid;
            }
            else
            {
                return lst;
            }

        }
        private String GetCategory(string Category)
        {

            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@Category", Category);
            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("select id from Category where Name=@Category");

            var lst = context.Database.SqlQuery<string>(qrystr.ToString(),sqlParameter).FirstOrDefault();
            if (lst == null)
            {
                qrystr.Clear();
                qrystr.Append("select top 1 id from Category ");

                var Topid = context.Database.SqlQuery<string>(qrystr.ToString()).FirstOrDefault();
                return Topid;
            }
            else
            {
                return lst;
            }

        }

        private String GetDivision(string Division)
        {

            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@Division", Division);
            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("select id from Division where Name =@Division");

            var lst = context.Database.SqlQuery<string>(qrystr.ToString(),sqlParameter).FirstOrDefault();
            if (lst == null)
            {
                qrystr.Clear();
                qrystr.Append("select top 1 id from Division ");

                var Topid = context.Database.SqlQuery<string>(qrystr.ToString()).FirstOrDefault();
                return Topid;
            }
            else
            {
                return lst;
            }

        }
        private String GetBranch(string Branch)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@Branch", Branch);
            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("select id from Branch where Name=@Branch");

            var lst = context.Database.SqlQuery<string>(qrystr.ToString(),sqlParameter).FirstOrDefault();
            if (lst == null)
            {
                qrystr.Clear();
                qrystr.Append("select top 1 id from Branch ");

                var Topid = context.Database.SqlQuery<string>(qrystr.ToString()).FirstOrDefault();
                return Topid;
            }
            else
            {
                return lst;
            }

        }


        private String GetLocation(string Location)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@Location", Location);
            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("select id from Location where Name= @Location");

            var lst = context.Database.SqlQuery<string>(qrystr.ToString(),sqlParameter).FirstOrDefault();
            if (lst == null)
            {
                qrystr.Clear();
                qrystr.Append("select top 1 id from Location ");

                var Topid = context.Database.SqlQuery<string>(qrystr.ToString()).FirstOrDefault();
                return Topid;
            }
            else
            {
                return lst;
            }

        }
    }
}
