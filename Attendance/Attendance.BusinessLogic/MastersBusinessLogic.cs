using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class MastersBusinessLogic
    {
        MasterRepository repo = new MasterRepository();
        public List<CompanyList> GetAllCompanies()
        {
            using (var repo = new MasterRepository())
            { 
                var lst = repo.GetAllCompanies();
            return lst;
            }
        }

        public List<SelectListItem> LoadCompany()
        {
            using (var repo = new MasterRepository())
            { 
                try
                {
                    var lst = repo.GetAllCompaniesList();

                    var items = new List<SelectListItem>();

                    items = lst.Select(d => new SelectListItem()
                    {
                        Text = d.Name,
                        Value = d.Id.ToString(),
                        Selected = false
                    }).ToList();

                    if (items == null)
                    {
                        return new List<SelectListItem>();
                    }
                    else
                    {
                        return items;
                    }
                }
                catch (Exception)
                {
                    return new List<SelectListItem>();
                    throw;
                }
            }
        }

        public List<BranchList> GetAllBranches()
        {
            using (var repo = new MasterRepository())
            { 
                var lst = repo.GetAllBranches();
            return lst;
            }
        }

        public List<DepartmentList> GetAllDepartments()
        {
            using (var repo = new MasterRepository())
            { 
                var lst = repo.GetAllDepartments();
            return lst;
            }
        }

        public List<DivisionList> GetAllDivisions()
        {
            using (var repo = new MasterRepository())
            { 
                var lst = repo.GetAllDivisions();
            return lst;
            }
        }

        public List<DesignationList> GetAllDesignations()
        {
            using (var repo = new MasterRepository())
            { 
             var lst = repo.GetAllDesignations();
            return lst;
            }
        }

        public List<GradeList> GetAllGrades()
        {
            using (var repo = new MasterRepository())
            {    
                var lst = repo.GetAllGrades();
            return lst;
            }
        }

        public List<CategoryList> GetAllCategories()
        {
            using (var repo = new MasterRepository())
            { 
                var lst = repo.GetAllCategories();
            return lst;
            }
        }

        public List<CostCentreList> GetAllCostCentre()
        {
            using (var repo = new MasterRepository())
            { 
                var lst = repo.GetAllCostCentre();
            return lst;
            }
        }

        public List<LocationList> GetAllLocation()
        {
            using (var repo = new MasterRepository())
            { 
                var lst = repo.GetAllLocation();
            return lst;
            }
        }
        public String ValidateMasterFieldValue(string tablename, string fieldname, string fieldValue, string operationMode, string IdFieldName = "", string IdFieldValue = "")
        {
            using (var repo = new MasterRepository())
            {  
                var data = repo.ValidateMasterFieldValue(tablename, fieldname, fieldValue, operationMode, IdFieldName, IdFieldValue);
            return data;
            }
        }

        public String ValidateIsActive(int HolidayId)
        {
            using (var repo = new MasterRepository())
            { 
                var data = repo.ValidateIsActive(HolidayId);
            return data;
            }

        }
        public string getmaxid(string tablename, string fieldname, string prefix, string suffix, int totalidlength, ref string lastid)
        {
            using (var repo = new MasterRepository())
            { 
                var maxid = repo.getmaxid(tablename, fieldname, prefix, suffix, totalidlength, ref lastid);
            return maxid;
            }
        }

        public void AddOrUpdateInformation<T>(T newItem, string tableName, string operationMode, string CreatedBy) where T : class
        {
            using (var repo = new MasterRepository())
                repo.AddOrUpdateInformation(newItem, tableName, operationMode, CreatedBy);
        }

        #region Attendance Policy
        public List<AttendancePolicyModel> GetAttendancePolicyBusinessLogic()
        {
            using (var repo = new MasterRepository())
                return repo.GetAttendancePolicyRepository();
        }
        public List<AttendancePolicyModel> GetRuleGroupTxnValuesBusinessLogic(string RuleType, string Description)
        {
            using (var repo = new MasterRepository())
                return repo.GetRuleGroupTxnValuesRepository(RuleType, Description);
        }
        public string SaveRuleGroupTxnBusinessLogic(List<AttendancePolicyModel> lst, string ModifiedBy)
        {
            using (var repo = new MasterRepository())
                return repo.SaveRuleGroupTxnRepository(lst, ModifiedBy);
        }
        #endregion
    }
}
