﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using Attendance.Model;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class MasterRepository : TrackChangeRepository
    {
        private AttendanceManagementContext context;

        public MasterRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<CompanyList> GetAllCompanies()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select Id , Name , ShortName , LegalName , Website , RegisterNo , " +
            "TNGSNo , CSTNo , TINNo , ServiceTaxNo , PANNo , PFNo , " +
            "case when IsActive = 1 then 'Yes' else 'No' end as IsActive  , CreatedOn , CreatedBy , ModifiedOn , ModifiedBy " +
            "from Company");
            try
            {
                var lstComp = context.Database.SqlQuery<CompanyList>(qryStr.ToString()).Select(c => new CompanyList()
                {
                    Id = c.Id,
                    Name = c.Name,
                    ShortName = c.ShortName,
                    LegalName = c.LegalName,
                    Website = c.Website,
                    RegisterNo = c.RegisterNo,
                    TNGSNo = c.TNGSNo,
                    CSTNo = c.CSTNo,
                    TINNo = c.TINNo,
                    ServiceTaxNo = c.ServiceTaxNo,
                    PANNo = c.PANNo,
                    PFNo = c.PFNo,
                    IsActive = c.IsActive,
                    CreatedOn = c.CreatedOn,
                    CreatedBy = c.CreatedBy,
                    ModifiedOn = c.ModifiedOn,
                    ModifiedBy = c.ModifiedBy
                }).ToList();
                return lstComp;
            }
            catch (Exception)
            {
                return new List<CompanyList>();
                throw;
            }
        }

        public List<CompanyList> GetAllCompaniesList()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select * from company where isactive = 1");
            try
            {
                var lst = context.Database.SqlQuery<Company>(qryStr.ToString()).Select(d => new CompanyList()
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
            try
            {
                var lstBranch = context.Branch.Select(c => new BranchList()
                {
                    Id = c.Id,
                    Name = c.Name,
                    ShortName = c.ShortName,
                    Address = c.Address,
                    City = c.City,
                    District = c.District,
                    State = c.State,
                    Country = c.Country,
                    PostalCode = c.PostalCode,
                    Phone = c.Phone,
                    Fax = c.Fax,
                    Email = c.Email,
                    CompanyID = c.CompanyID,
                    IsActive = c.IsActive,
                    IsHeadOffice = c.IsHeadOffice,
                    CreatedOn = c.CreatedOn,
                    CreatedBy = c.CreatedBy
                }).ToList();

                if (lstBranch == null)
                {
                    return new List<BranchList>();
                }
                else
                {
                    return lstBranch;
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
            qryStr.Append("select * from Department Order By Name Asc");
            try
            {
                var lstDept = context.Database.SqlQuery<DepartmentList>(qryStr.ToString()).Select(c => new DepartmentList()
                {
                    Id = c.Id,
                    Name = c.Name,
                    ShortName = c.ShortName,
                    Phone = c.Phone,
                    Fax = c.Fax,
                    Email = c.Email,
                    IsActive = c.IsActive,
                    CreatedOn = c.CreatedOn,
                    CreatedBy = c.CreatedBy
                }
                ).ToList();

                if (lstDept == null)
                {
                    return new List<DepartmentList>();
                }
                else
                {
                    return lstDept;
                }
            }
            catch (Exception)
            {
                return new List<DepartmentList>();
            }
        }

        public List<DivisionList> GetAllDivisions()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Id , Name , ShortName , IsActive , CreatedOn ,CreatedBy  from Division Order By Name Asc");
            try
            {
                var lst = context.Database.SqlQuery<DivisionList>(qryStr.ToString()).Select(d => new DivisionList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    IsActive = d.IsActive,
                    CreatedOn = d.CreatedOn,
                    CreatedBy = d.CreatedBy
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

        public List<DesignationList> GetAllDesignations()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Id , Name , ShortName , IsActive  , CreatedOn , CreatedBy from Designation Order By Name Asc");
            try
            {
                var lst =
                    context.Database.SqlQuery<DesignationList>(qryStr.ToString()).Select(d => new DesignationList()
                    {
                        Id = d.Id,
                        Name = d.Name,
                        ShortName = d.ShortName,
                        IsActive = d.IsActive,
                        CreatedOn = d.CreatedOn,
                        CreatedBy = d.CreatedBy
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
            qryStr.Append("Select Id , Name , ShortName , IsActive , CreatedOn , CreatedBy from Grade Order by Name asc");
            try
            {
                var lst = context.Database.SqlQuery<GradeList>(qryStr.ToString()).Select(d => new GradeList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    IsActive = d.IsActive,
                    CreatedOn = d.CreatedOn,
                    CreatedBy = d.CreatedBy
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

        public List<CategoryList> GetAllCategories()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select Id ,PeopleSoftCode, Name , ShortName , IsActive , CreatedOn , CreatedBy from Category Order by Name asc");
            try
            {
                var lst = context.Database.SqlQuery<CategoryList>(qryStr.ToString()).Select(d => new CategoryList()
                {
                    Id = d.Id,
                    PeopleSoftCode = d.PeopleSoftCode,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    IsActive = d.IsActive,
                    CreatedOn = d.CreatedOn,
                    CreatedBy = d.CreatedBy
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

        public List<CostCentreList> GetAllCostCentre()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select Id,PeopleSoftCode, Name , ShortName , IsActive  , CreatedOn , CreatedBy from CostCentre Order By Name asc");
            try
            {
                var lst = context.Database.SqlQuery<CostCentreList>(qryStr.ToString()).Select(d => new CostCentreList()
                {
                    Id = d.Id,
                    PeopleSoftCode = d.PeopleSoftCode,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    IsActive = d.IsActive,
                    CreatedOn = d.CreatedOn,
                    CreatedBy = d.CreatedBy
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

        public List<LocationList> GetAllLocation()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select Id,PeopleSoftCode, Name , ShortName , IsActive  , CreatedOn , CreatedBy from Location Order By Name Asc");
            try
            {
                var lst = context.Database.SqlQuery<LocationList>(qryStr.ToString()).Select(d => new LocationList()
                {
                    Id = d.Id,
                    PeopleSoftCode = d.PeopleSoftCode,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    IsActive = d.IsActive,
                    CreatedOn = d.CreatedOn,
                    CreatedBy = d.CreatedBy
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

        public void SaveCompanyInfo(Company CompInfo)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    string createdBy = string.Empty;
                    string operationMode = string.Empty;
                    string tableName = "COMPANY";
                    if (string.IsNullOrEmpty(CompInfo.Id))
                    {
                        string lastid = string.Empty;
                        string maxid = getmaxid("company", "id", "C", "", 6, ref lastid);
                        CompInfo.Id = maxid;
                        operationMode = "add";
                        createdBy = CompInfo.CreatedBy;
                        AddOrUpdateInformation(CompInfo,  tableName, operationMode, createdBy);
                    }
                    else
                    {
                        operationMode = "edit";
                        createdBy = CompInfo.CreatedBy;
                        AddOrUpdateInformation(CompInfo,  tableName, operationMode, createdBy);
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

        public void AddOrUpdateInformation<T>(T newItem,  string tableName, string operationMode, string CreatedBy) where T : class
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    if (operationMode == "add")
                    {
                        context.Set<T>().Add(newItem);
                        context.SaveChanges();
                    }
                    else
                    {
                        context.Set<T>().AddOrUpdate(newItem);
                        string ActionType = string.Empty;
                        string _ChangeLog = string.Empty;
                        string _PrimaryKeyValue = string.Empty;
                        GetChangeLogString(newItem, context, ref _ChangeLog, ref ActionType, ref _PrimaryKeyValue);
                        context.SaveChanges();
                        if (string.IsNullOrEmpty(_ChangeLog.ToString()) == false)
                        {
                            RecordChangeLog(context, CreatedBy, tableName, _ChangeLog, ActionType, _PrimaryKeyValue);
                        }
                    }
                    trans.Commit();
                }
                catch (Exception err)
                {
                    trans.Rollback();
                    throw err;
                }
            }
        }

        //COMMON FUNCTION
        public string getmaxid(string tablename, string fieldname,
            string prefix, string suffix, int totalidlength, ref string lastid)
        {


            //getting maxid for the given table and field.
            string qryStr = string.Empty;
            string maxid = string.Empty;
            int idlen = totalidlength - prefix.Length - suffix.Length;

            if (string.IsNullOrEmpty(lastid) == true)
            {
                qryStr = "select '" + prefix + "'+right ( '" + stringstuff('0', idlen) + "' + convert ( varchar , convert " +
                    "( int , right ( isnull ( max ( " + fieldname + " ) , 0 ) , " + idlen + " ) ) + 1 )  ,  " + idlen + " ) + '" + suffix + "' " +
                    "from " + tablename + " where 1= 1";

                maxid = context.Database.SqlQuery<string>(qryStr).FirstOrDefault();
            }
            else
            {
                lastid = GetNewMaxId(lastid, prefix, suffix, totalidlength);
                maxid = lastid;
            }

            return maxid;
        }

        public string VisitorPasssgetmaxid(int totalidlength, ref string lastid)
        {
            //getting maxid for the given table and field.
            string qryStr = string.Empty;
            string maxid = string.Empty;
            
            if (string.IsNullOrEmpty(lastid) == true)
            {
                qryStr = "select right ( '0000' + convert ( nvarchar , convert ( int , right ( isnull ( max ( id ) , 0 ) ,4  ) ) + 1 )  , 4 ) + '' from VisitorPass";

                maxid = context.Database.SqlQuery<string>(qryStr).FirstOrDefault();
            }
            else
            {
                lastid = GetNewMaxId(lastid, totalidlength);
                maxid = lastid;
            }
            return maxid;
        }

        public string Id(int totalidlength, ref string lastid)
        {
            //getting maxid for the given table and field.
            string qryStr = string.Empty;
            string Id = string.Empty;
            if (string.IsNullOrEmpty(lastid) == true)
            {
                qryStr = "select max(Id) from VisitorPass";

                Id = context.Database.SqlQuery<string>(qryStr).FirstOrDefault();
            }
            else
            {
                lastid = GetNewMaxId(lastid, totalidlength);
                Id = lastid;
            }
            return Id;
        }

        private string GetNewMaxId(string lastid, int totalidlength)
        {
            var a = string.Empty;
            a = lastid;
            return a;
        }

        public string GetNewMaxId(string LastId, string prefix, string suffix, int totalidlength)
        {
            var a = string.Empty;
            var p = prefix;
            var s = suffix;
            var l = totalidlength;
            a = LastId;

            var idlen = l - p.Length - s.Length;
            if (string.IsNullOrEmpty(s) == false)
                a = a.Substring(0, a.Length - s.Length);

            if (string.IsNullOrEmpty(p) == false)
                a = a.Substring(p.Length, a.Length - p.Length);

            a = stringstuff('0', idlen) + (Convert.ToInt16(a) + 1).ToString();
            a = Strings.Right(a, idlen);
            //a = a.Substring ( a.Length - idlen , a.Length - idlen );
            a = p + a + s;
            return a;
        }

        //COMMON FUNCTION
        public string stringstuff(char c, int repeat)
        {
            //string stuffing function.
            string t = string.Empty;
            for (int i = 0; i < repeat; i += 1)
                t = t + c;
            return t;
        }
        public void ImportFile(UploadControlTable model)
        {
            context.UploadControlTable.AddOrUpdate(model);
            context.SaveChanges();
        }

        public List<UploadControlTabledataList> GetUploadControltableList()
        {
            var query = new StringBuilder();
            query.Append("Select top 10  filename,typeofdata,Uploadedon ,Uploadedby,case when isprocessed=0 then 'Pending' " +
            "when isprocessed=1 and iserror=1 then  'Process Completed With Error' else 'Completed' end as status, " +
            "case when iserror=1 then errormessage else '-' end as Errormessage  from uploadcontroltable order by id desc");
            var Res = context.Database.SqlQuery<UploadControlTabledataList>(query.ToString()).Select(d => new UploadControlTabledataList()
            {
                //Id = d.Id,
                FileName = d.FileName,
                TypeofData = d.TypeofData,
                UploadedBy = d.UploadedBy,
                Uploadedon = d.Uploadedon,
                Status = d.Status,
                ErrorMessage = d.ErrorMessage

            }).ToList();
            return Res;
        }
    }
}
