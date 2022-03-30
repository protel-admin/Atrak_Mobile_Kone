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
        public List<CompanyList> GetAllCompanies()
        {
            using (MasterRepository masterRepository = new MasterRepository())
            {
                return masterRepository.GetAllCompanies();
            }
        }

        public List<SelectListItem> LoadCompany()
        {
            var repo = new MasterRepository();
            try
            {
                List<CompanyList> companies = new List<CompanyList>();
                using (MasterRepository masterRepository = new MasterRepository())
                {
                    companies = masterRepository.GetAllCompaniesList();
                }

                var items = new List<SelectListItem>();

                items = companies.Select(d => new SelectListItem()
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

        public List<BranchList> GetAllBranches()
        {
            using (MasterRepository masterRepository = new MasterRepository())
            {
                return masterRepository.GetAllBranches();
            }
        }

        public List<DepartmentList> GetAllDepartments()
        {
            using (MasterRepository masterRepository = new MasterRepository())
            {
                return masterRepository.GetAllDepartments();
            }
        }

        public List<DivisionList> GetAllDivisions()
        {
            using (MasterRepository masterRepository = new MasterRepository())
            {
                return masterRepository.GetAllDivisions();
            }
        }

        public List<DesignationList> GetAllDesignations()
        {
            using (MasterRepository masterRepository = new MasterRepository())
            {
                return masterRepository.GetAllDesignations();
            }
        }

        public List<GradeList> GetAllGrades()
        {
            using (MasterRepository masterRepository = new MasterRepository())
            {
                return masterRepository.GetAllGrades();
            }
        }

        public List<CategoryList> GetAllCategories()
        {
            using (MasterRepository masterRepository = new MasterRepository())
            {
                return masterRepository.GetAllCategories();
            }
        }

        public List<CostCentreList> GetAllCostCentre()
        {
            using (MasterRepository masterRepository = new MasterRepository())
            {
                return masterRepository.GetAllCostCentre();
            }
        }

        public List<LocationList> GetAllLocation()
        {
            using (MasterRepository masterRepository = new MasterRepository())
            {
                return masterRepository.GetAllLocation();
            }
        }

        public string getmaxid(string tablename, string fieldname, string prefix, string suffix, int totalidlength, ref string lastid)
        {
            using (MasterRepository masterRepository = new MasterRepository())
        {
                return masterRepository.getmaxid(tablename, fieldname, prefix, suffix, totalidlength, ref lastid);
            }
        }

        public void AddOrUpdateInformation<T>(T newItem, string tableName, string operationMode, string CreatedBy) where T : class
        {
            using (MasterRepository masterRepository = new MasterRepository())
            {
                masterRepository.AddOrUpdateInformation(newItem, tableName, operationMode, CreatedBy);
            }
        }
        public List<UploadControlTabledataList> GetUploadControltableList()
        {
            using (MasterRepository masterRepository = new MasterRepository())
            {
                return masterRepository.GetUploadControltableList();
            }
        }
        public void ImportFile(UploadControlTable model)
        {
            using (MasterRepository masterRepository = new MasterRepository())
            {
                masterRepository.ImportFile(model);
            }
        }
    }
}
