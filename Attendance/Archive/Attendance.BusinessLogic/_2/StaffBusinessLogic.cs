using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Attendance.Model;
using Attendance.Repository;


namespace Attendance.BusinessLogic
{
    public class
        StaffBusinessLogic
    {

        public StaffInformation GetStaffMainInformation(string staffid)
        {
            var repo = new StaffRepository();
            var si = repo.GetStaffMainInformation(staffid);
            return si;
        }

        public StaffOfficialInformation GetStaffOfficialInformation(string staffid)
        {
            var repo = new StaffRepository();
            var so = repo.GetStaffOfficialInformation(staffid);
            return so;
        }

        public StaffPersonalInformation GetStaffPersonalInformation(string staffid)
        {
            var repo = new StaffRepository();
            var sp = repo.GetStaffPersonalInformation(staffid);
            return sp;
        }

        //public void SaveStaffInformationToDB(Staff _Sta,  StaffPersonal _SP, StaffOfficial _SO, StaffFamily _SF, StaffEducation _SE)
        public void SaveStaffInformationToDB(Staff _Sta, StaffPersonal _SP, StaffOfficial _SO, EmployeePhoto _EP, bool AddMode)
        {
            var repo = new StaffRepository();
            //repo.SaveStaffInformationToDB(_Sta, _SP, _SO, _SF, _SE);
            repo.SaveStaffInformationToDB(_Sta, _SP, _SO,_EP, AddMode);
        }

        public EmployeePhoto GetEmployeePhoto(string StaffId)
        {
            var repo = new StaffRepository();
            var Data = repo.GetEmployeePhoto(StaffId);

            if(Data!=null)
            {
                return Data;
            }
            else
            {
                return null;
            }
        }

        public List<SelectListItem> GetAllWorkingDayPattern()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllWorkingDayPattern();

            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.PsCode + " - " + d.WorkingPattern,
                Value = d.Id.ToString(),
                Selected = true
            }).ToList();

            return items;
        }

        public List<SelectListItem> GetAllCompanies()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllCompanies();

            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;

        }

        public List<SelectListItem> GetAllBranches()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllBranches();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllDepartments()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllDepartments();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllDivisions()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllDivisions();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllVolumes()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllVolumes();
            var items = new List<SelectListItem>();
            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }


        public List<SelectListItem> GetAllDesignations()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllDesignations();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllGrades()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllGrades();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllCategories()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllCategories();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllCostCentres()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllCostCentres();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllSecurityGroup()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllSecurityGroup();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllLocations()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllLocations();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllStatuses()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllStatuses();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllMaritalStatus()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllMaritalStatus();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllBloodGroup()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllBloodGroup();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }


        public List<SelectListItem> GetAllLeaveGroup()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllLeaveGroup();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllHolidayGroup()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllHolidayGroup();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllWeeklyOffGroup()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllWeeklyOffGroup();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<StaffFamilyInformation> GetStaffFamilyInformation(string staffid)
        {
            var repo = new StaffRepository();
            var lst = repo.GetStaffFamilyInformation(staffid);
            return lst;
        }

        public List<StaffEducationInformation> GetStaffEducationInformation(string staffid)
        {
            var repo = new StaffRepository();
            var lst = repo.GetStaffEducationInformation(staffid);
            return lst;
        }

        public List<SelectListItem> GetSalutation()
        {
            var repo = new StaffRepository();
            var lst = repo.GetSalutation();

            var items = new List<SelectListItem>();
            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id,
                Selected = true
            }).ToList();

            return items;
        }

        public List<SelectListItem> GetPolicyList()
        {
            var repo = new StaffRepository();
            var lst = repo.GetPolicyList();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.name,
                Value = d.id,
                Selected = true
            }).ToList();

            return items;
        }
    }
}
