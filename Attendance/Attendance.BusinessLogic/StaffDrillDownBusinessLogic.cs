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
        StaffDrillDownRepository vrepo = new StaffDrillDownRepository();
        public List<DropDownModel> GetStaffStatusBusinessLogic()
        {
            using (StaffDrillDownRepository vrepo = new StaffDrillDownRepository())
                return vrepo.GetStaffStatusRepository();
        }
        public List<SelectListItem> LoadFilterList(string companyid , string branchid , string departmentid , 
                        string divisionid , string designationid , string gradeid , 
                        string categoryid , string costcentreid , string locationid,string volumeid,
                        string shortname)
        {
            using (var repo = new StaffDrillDownRepository())
            { 
                var lst = repo.LoadFilterList(companyid, branchid, departmentid,
                             divisionid, designationid, gradeid,
                             categoryid, costcentreid, locationid, volumeid, shortname);
            var item = new List<SelectListItem>();
            item = lst.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id,
                Selected = false
            }).ToList();
            return item;
            }
        }

        public List<SelectListItem> LoadCompanyWiseDepartment(string CompanyId)
        {
            using (var vrepo = new StaffDrillDownRepository())
            { 
                var lst = vrepo.LoadCompanyWiseDepartment(CompanyId);

            var item = new List<SelectListItem>();

            item = lst.Select(i => new SelectListItem()
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

        public List<SelectListItem> GetCompanyList()
        {
            using (var vrepo = new StaffDrillDownRepository())
            { 
                var lst = vrepo.GetCompanyList();
            var item = new List<SelectListItem>();

            item = lst.Select(i => new SelectListItem()
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

        public List<SelectListItem> GetBranchList( )
        {
            using (var vrepo = new StaffDrillDownRepository())
            { 
                var lst = vrepo.GetBranchList();
            var item = new List<SelectListItem> ( );

            item = lst.Select(i => new SelectListItem()
            {
                Text =  i.Name,
                Value =  i.Id,
                Selected = false
            }).ToList();

            if ( item.Count > 0 )
                item [ 0 ].Selected = true;

            return item;
            }
        }

        public List<SelectListItem> GetDepartmentList( )
        {
            using (var vrepo = new StaffDrillDownRepository())
            { 
                var lst = vrepo.GetDepartmentList();
            var item = new List<SelectListItem> ( );

            item = lst.Select ( i => new SelectListItem ( ) {
                Text = i.Name ,
                Value = i.Id ,
                Selected = false
            } ).ToList ( );

            if ( item.Count > 0 )
                item [ 0 ].Selected = true;

            return item;
            }
        }

        public List<SelectListItem> GetDivisionList( )
        {
            using (var vrepo = new StaffDrillDownRepository())
            { 
                var lst = vrepo.GetDivisionList();
            var item = new List<SelectListItem> ( );

            item = lst.Select ( i => new SelectListItem ( ) {
                Text = i.Name ,
                Value = i.Id ,
                Selected = false
            } ).ToList ( );

            if ( item.Count > 0 )
                item [ 0 ].Selected = true;

            return item;
            }
        }

        public List<SelectListItem> GetDesignationList( )
        {
            using (var vrepo = new StaffDrillDownRepository())
            { 
                var lst = vrepo.GetDsignationList();
            var item = new List<SelectListItem> ( );

            item = lst.Select ( i => new SelectListItem ( ) {
                Text = i.Name ,
                Value = i.Id ,
                Selected = false
            } ).ToList ( );

            if ( item.Count > 0 )
                item [ 0 ].Selected = true;

            return item;
            }

        }

        public List<SelectListItem> GetGradeList( )
        {
            using (var vrepo = new StaffDrillDownRepository())
            { 
                var lst = vrepo.GetGradeList();
            var item = new List<SelectListItem> ( );

            item = lst.Select ( i => new SelectListItem ( ) {
                Text = i.Name ,
                Value = i.Id ,
                Selected = false
            } ).ToList ( );

            if ( item.Count > 0 )
                item [ 0 ].Selected = true;
            
            return item;
            }

        }

        public List<SelectListItem> GetCategoryList()
        {
            using (var vrepo = new StaffDrillDownRepository())
            {
                var lst = vrepo.GetCategoryList();
            var item = new List<SelectListItem>();

            item = lst.Select(i => new SelectListItem()
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

        public List<SelectListItem> GetCostCentreList()
        {
            var vrepo = new StaffDrillDownRepository();
            var lst = vrepo.GetCostCentreList();
            var item = new List<SelectListItem>();

            item = lst.Select(i => new SelectListItem()
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
            var vrepo = new StaffDrillDownRepository();
            var lst = vrepo.GetVolumeList();
            var item = new List<SelectListItem>();

            item = lst.Select(i => new SelectListItem()
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
            using (var vrepo = new StaffDrillDownRepository())
            { 
                var lst = vrepo.GetVolumeList();
            var item = new List<SelectListItem>();

            item = lst.Select(i => new SelectListItem()
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

        public List<SelectListItem> GetEmployeeGroupList()
        {
            using (var vrepo = new StaffDrillDownRepository())
            { 
                var lst = vrepo.GetEmployeeGroupList();
            var item = new List<SelectListItem>();

            item = lst.Select(i => new SelectListItem()
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

        public List<SelectListItem> GetShiftList()
        {
            using (var vrepo = new StaffDrillDownRepository())
            { 
                var lst = vrepo.GetShiftList();
            var item = new List<SelectListItem>();

            item = lst.Select(i => new SelectListItem()
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
}
