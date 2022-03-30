using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic {
    public class StaffDrillDownBusinessLogic {

        public List<SelectListItem> LoadFilterList(string companyid , string branchid , string departmentid , 
                        string divisionid , string designationid , string gradeid , 
                        string categoryid , string costcentreid , string locationid,string volumeid,
                        string shortname, string role, string LocationId)
        {
            List<FilterList> filters = new List<FilterList>();
            
            using (StaffDrillDownRepository staffDrillDownRepository = new StaffDrillDownRepository())
            {
                filters = staffDrillDownRepository.LoadFilterList(companyid, branchid, departmentid,
                         divisionid, designationid, gradeid,
                         categoryid, costcentreid, locationid, volumeid, shortname, role, LocationId);
            }
            var item = new List<SelectListItem>();
            item = filters.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id,
                Selected = false
            }).ToList();
            return item;
        }

        public List<SelectListItem> LoadCompanyWiseDepartment(string CompanyId)
        {
            List<DepartmentList> departments = new List<DepartmentList>();
            using (StaffDrillDownRepository staffDrillDownRepository = new StaffDrillDownRepository())
            {
                departments = staffDrillDownRepository.LoadCompanyWiseDepartment(CompanyId);
            }

            var item = new List<SelectListItem>();

            item = departments.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id,
                Selected = false
            }).ToList();

            if (item.Count > 0)
                item[0].Selected = true;

            return item;

        }

        public List<SelectListItem> GetCompanyList()
        {
            List<CompanyList> companies = new List<CompanyList>();
            using (StaffDrillDownRepository staffDrillDownRepository = new StaffDrillDownRepository())
            {
                companies = staffDrillDownRepository.GetCompanyList();
            }
            var item = new List<SelectListItem>();

            item = companies.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id,
                Selected = false
            }).ToList();

            if (item.Count > 0)
                item[0].Selected = true;

            return item;
        }

        public List<SelectListItem> GetBranchList( )
        {
            List<BranchList> branches = new List<BranchList>();
            using (StaffDrillDownRepository staffDrillDownRepository = new StaffDrillDownRepository())
            {
                branches = staffDrillDownRepository.GetBranchList();
            }
            var item = new List<SelectListItem> ( );

            item = branches.Select(i => new SelectListItem()
            {
                Text =  i.Name,
                Value =  i.Id,
                Selected = false
            }).ToList();

            if ( item.Count > 0 )
                item [ 0 ].Selected = true;

            return item;
        }

        public List<SelectListItem> GetDepartmentList( )
        {
            List<DepartmentList> departments = new List<DepartmentList>();
            using (StaffDrillDownRepository staffDrillDownRepository = new StaffDrillDownRepository())
            {
                departments = staffDrillDownRepository.GetDepartmentList();
            }
            var item = new List<SelectListItem> ( );

            item = departments.Select ( i => new SelectListItem ( ) {
                Text = i.Name ,
                Value = i.Id ,
                Selected = false
            } ).ToList ( );

            if ( item.Count > 0 )
                item [ 0 ].Selected = true;

            return item;
        }

        public List<SelectListItem> GetDivisionList( )
        {
            List<DivisionList> divisions = new List<DivisionList>();
            using (StaffDrillDownRepository staffDrillDownRepository = new StaffDrillDownRepository())
            {
                divisions = staffDrillDownRepository.GetDivisionList();
            }
            var item = new List<SelectListItem> ( );

            item = divisions.Select ( i => new SelectListItem ( ) {
                Text = i.Name ,
                Value = i.Id ,
                Selected = false
            } ).ToList ( );

            if ( item.Count > 0 )
                item [ 0 ].Selected = true;

            return item;
            
        }

        public List<SelectListItem> GetDesignationList( )
        {
            List<DesignationList> designations = new List<DesignationList>();
            using (StaffDrillDownRepository staffDrillDownRepository = new StaffDrillDownRepository())
            {
                designations = staffDrillDownRepository.GetDsignationList();
            }
            var item = new List<SelectListItem> ( );

            item = designations.Select ( i => new SelectListItem ( ) {
                Text = i.Name ,
                Value = i.Id ,
                Selected = false
            } ).ToList ( );

            if ( item.Count > 0 )
                item [ 0 ].Selected = true;

            return item;
            
        }

        public List<SelectListItem> GetGradeList( )
        {
            List<GradeList> grades = new List<GradeList>();
            using (StaffDrillDownRepository staffDrillDownRepository = new StaffDrillDownRepository())
            {
                grades = staffDrillDownRepository.GetGradeList();
            }
            var item = new List<SelectListItem> ( );

            item = grades.Select ( i => new SelectListItem ( ) {
                Text = i.Name ,
                Value = i.Id ,
                Selected = false
            } ).ToList ( );

            if ( item.Count > 0 )
                item [ 0 ].Selected = true;
            
            return item;
            
        }

        public List<SelectListItem> GetCategoryList()
        {
            List<CategoryList> categories = new List<CategoryList>();
            using (StaffDrillDownRepository staffDrillDownRepository = new StaffDrillDownRepository())
            {
                categories = staffDrillDownRepository.GetCategoryList();
            }
            var item = new List<SelectListItem>();

            item = categories.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id,
                Selected = false
            }).ToList();

            if (item.Count > 0)
                item[0].Selected = true;

            return item;
        }

        public List<SelectListItem> GetCostCentreList()
        {
            List<CostCentreList> costCentres = new List<CostCentreList>();
            using (StaffDrillDownRepository staffDrillDownRepository = new StaffDrillDownRepository())
            {
                costCentres = staffDrillDownRepository.GetCostCentreList();
            }
            var item = new List<SelectListItem>();

            item = costCentres.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id,
                Selected = false
            }).ToList();

            if (item.Count > 0)
                item[0].Selected = true;

            return item;
        }

        public List<SelectListItem> GetLocationList()
        {
            List<LocationList> locations = new List<LocationList>();
            using (StaffDrillDownRepository staffDrillDownRepository = new StaffDrillDownRepository())
            {
                locations = staffDrillDownRepository.GetLocationList();
            }
            var item = new List<SelectListItem>();

            item = locations.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id,
                Selected = false
            }).ToList();

            if (item.Count > 0)
                item[0].Selected = true;

            return item;
        }
        public List<SelectListItem> GetVolumeList()
        {
            List<VolumeList> volumes = new List<VolumeList>();
            using (StaffDrillDownRepository staffDrillDownRepository = new StaffDrillDownRepository())
            {
                volumes = staffDrillDownRepository.GetVolumeList();
            }
            var item = new List<SelectListItem>();

            item = volumes.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id,
                Selected = false
            }).ToList();

            if (item.Count > 0)
                item[0].Selected = true;

            return item;
        }

        public List<SelectListItem> GetEmployeeGroupList()
        {
            List<EmployeeGroupView> employeeGroups = new List<EmployeeGroupView>();
            using (StaffDrillDownRepository staffDrillDownRepository = new StaffDrillDownRepository())
            {
                employeeGroups = staffDrillDownRepository.GetEmployeeGroupList();
            }
            var item = new List<SelectListItem>();

            item = employeeGroups.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id,
                Selected = false
            }).ToList();

            if (item.Count > 0)
                item[0].Selected = true;

            return item;
        }

        public List<SelectListItem> GetShiftList(string role, string Location)
        {
            List<ShiftView> shiftViews = new List<ShiftView>();
            using (StaffDrillDownRepository staffDrillDownRepository = new StaffDrillDownRepository())
            {
                shiftViews = staffDrillDownRepository.GetShiftList(role, Location);
            }
            var item = new List<SelectListItem>();

            item = shiftViews.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id,
                Selected = false
            }).ToList();

            if (item.Count > 0)
                item[0].Selected = true;

            return item;
        }
    }
}
