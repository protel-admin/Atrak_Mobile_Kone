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
    public class StaffBusinessLogic
    {
        StaffRepository repo = new StaffRepository();


        //rajesh TO BE ADDED IN THE WEB SOURCE MAY 16 2020
        public StaffOfficialInformationForApi GetStaffOfficialInformationForApi(string staffid)
        {
            var repo = new StaffRepository();
            var so = repo.GetStaffOfficialInformationForApi(staffid);
            //var photoData = await GetEmployeePhotoAsync(staffid);
            //if (photoData != null && photoData.EmpPhoto.Length > 0)
            //    so.PhotoB64String = Convert.ToBase64String(photoData.EmpPhoto);
            //else
            //    so.PhotoB64String = string.Empty;
            return so;
        }

        public StaffInformation GetStaffMainInformation(string staffid)
        {
            using (StaffRepository staffRepository = new StaffRepository())
            {
                return staffRepository.GetStaffMainInformation(staffid);                
            }
        }

        public string SaveBulkShiftsBusinessLogic(List<BulkShiftImportModel> model, string CreatedBy)
        {
            using (StaffRepository staffRepository = new StaffRepository())
            {
                return staffRepository.SaveBulkShiftsRepository(model, CreatedBy);
            }
        }

        public StaffOfficialInformation GetStaffOfficialInformation(string staffid)
        {
            using (StaffRepository staffRepository = new StaffRepository())
            {
                return staffRepository.GetStaffOfficialInformation(staffid);
            }
        }

        public StaffPersonalInformation GetStaffPersonalInformation(string staffid)
        {
            using (StaffRepository staffRepository = new StaffRepository())
            {
                return staffRepository.GetStaffPersonalInformation(staffid);
            }
        }

        //public void SaveStaffInformationToDB(Staff _Sta,  StaffPersonal _SP, StaffOfficial _SO, StaffFamily _SF, StaffEducation _SE)
        public void SaveStaffInformationToDB(Staff _Sta, StaffPersonal _SP, StaffOfficial _SO, EmployeePhoto _EP, List<AdditionalFieldValue>  _AF, bool AddMode)
        {
            using (StaffRepository staffRepository = new StaffRepository())
            {
                staffRepository.SaveStaffInformationToDB(_Sta, _SP, _SO, _EP, _AF, AddMode);
            }
        }

        public EmployeePhoto GetEmployeePhoto(string StaffId)
        {
            EmployeePhoto employeePhoto = new EmployeePhoto();
            using (StaffRepository staffRepository = new StaffRepository())
            {
                employeePhoto =  staffRepository.GetEmployeePhoto(StaffId);
            }

            if(employeePhoto != null)
            {
                return employeePhoto;
            }
            else
            {
                return null;
            }
        }

        //Rajesh TO DO add to websource Aug 7
        public async Task<string> GetEmployeePhotoAsync(string StaffId)
        {
            var repo = new StaffRepository();
            var photoData = repo.GetEmployeePhoto(StaffId);
            string photoString= string.Empty; 
            if (photoData != null && photoData.EmpPhoto.Length > 0)
                photoString = Convert.ToBase64String(photoData.EmpPhoto);

            return photoString;
        }
        //Rajesh TO DO add to websource Aug 7
        public byte[] GetEmployeePhotoBytesAsync(string StaffId)
        {
            var repo = new StaffRepository();
            var photoData = repo.GetEmployeePhoto(StaffId);


            if (photoData != null)
                return photoData.EmpPhoto;
            else
                return null;
        }

        public List<SelectListItem> GetAllWorkingDayPattern()
        {
            List<WorkingDayPatternList> workingDayPatternLists = new List<WorkingDayPatternList>(); 
            using (StaffRepository staffRepository = new StaffRepository())
            {
                workingDayPatternLists = staffRepository.GetAllWorkingDayPattern();
            }
            var items = new List<SelectListItem>();

            items = workingDayPatternLists.Select(d => new SelectListItem()
            {
                Text = d.PatternDesc,
                Value = d.Id.ToString(),
                Selected = true
            }).ToList();

            return items;
        }

        public List<SelectListItem> GetAllCompanies()
        {
            List<CompanyList> companies = new List<CompanyList>();
            using (StaffRepository staffRepository = new StaffRepository())
            {
                companies = staffRepository.GetAllCompanies();
            }
            var items = new List<SelectListItem>();

            items = companies.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;

        }

        public List<SelectListItem> GetAllBranches()
        {
            List<BranchList> branches = new List<BranchList>();
            using (StaffRepository staffRepository = new StaffRepository())
            {
                branches = staffRepository.GetAllBranches();
            }
            var items = new List<SelectListItem>();

            items = branches.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllDepartments()
        {
            List<DepartmentList> departments = new List<DepartmentList>();
            using (StaffRepository staffRepository = new StaffRepository())
            {
                departments = staffRepository.GetAllDepartments();
            }
            var items = new List<SelectListItem>();

            items = departments.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllDivisions()
        {
            List<DivisionList> divisions = new List<DivisionList>();
            using (StaffRepository staffRepository = new StaffRepository())
            {
                divisions = staffRepository.GetAllDivisions();
            }
            var items = new List<SelectListItem>();

            items = divisions.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllVolumes()
        {
            List<VolumeList> volumes = new List<VolumeList>();

            using (StaffRepository staffRepository = new StaffRepository())
            {
                volumes = staffRepository.GetAllVolumes();
            }
            var items = new List<SelectListItem>();
            items = volumes.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }


        public List<SelectListItem> GetAllDesignations()
        {
            List<DesignationList> designations = new List<DesignationList>();

            using (StaffRepository staffRepository = new StaffRepository())
            {
                designations = staffRepository.GetAllDesignations();
            }
            var items = new List<SelectListItem>();

            items = designations.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllGrades()
        {
            List<GradeList> grades = new List<GradeList>();

            using (StaffRepository staffRepository = new StaffRepository())
            {
                grades = staffRepository.GetAllGrades();
            }
            var items = new List<SelectListItem>();

            items = grades.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllCategories()
        {
            List<CategoryList> categories = new List<CategoryList>();

            using (StaffRepository staffRepository = new StaffRepository())
            {
                categories = staffRepository.GetAllCategories();
            }
            var items = new List<SelectListItem>();

            items = categories.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllCostCentres()
        {
            List<CostCentreList> costCentres = new List<CostCentreList>();

            using (StaffRepository staffRepository = new StaffRepository())
            {
                costCentres = staffRepository.GetAllCostCentres();
            }
            var items = new List<SelectListItem>();

            items = costCentres.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllSecurityGroup()
        {
            List<SecurityGroupList> securityGroups = new List<SecurityGroupList>();

            using (StaffRepository staffRepository = new StaffRepository())
            {
                securityGroups = staffRepository.GetAllSecurityGroup();
            }
            var items = new List<SelectListItem>();

            items = securityGroups.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllLocations(string role, string Location)
        {
            List<LocationList> locations = new List<LocationList>();

            using (StaffRepository staffRepository = new StaffRepository())
            {
                locations = staffRepository.GetAllLocations(role, Location);
            }
            var items = new List<SelectListItem>();

            items = locations.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllStatuses()
        {
            List<StaffStatusList> staffStatuses = new List<StaffStatusList>();

            using (StaffRepository staffRepository = new StaffRepository())
            {
                staffStatuses = staffRepository.GetAllStatuses();
            }
            var items = new List<SelectListItem>();

            items = staffStatuses.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllMaritalStatus()
        {
            List<MaritalStatus> maritalStatuses = new List<MaritalStatus>();

            using (StaffRepository staffRepository = new StaffRepository())
            {
                maritalStatuses = staffRepository.GetAllMaritalStatus();
            }
            var items = new List<SelectListItem>();

            items = maritalStatuses.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllBloodGroup()
        {
            List<BloodGroup> bloodGroups = new List<BloodGroup>();

            using (StaffRepository staffRepository = new StaffRepository())
            {
                bloodGroups = staffRepository.GetAllBloodGroup();
            }
            var items = new List<SelectListItem>();

            items = bloodGroups.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }


        public List<SelectListItem> GetAllLeaveGroup()
        {
            List<LeaveGroupList> leaveGroups = new List<LeaveGroupList>();

            using (StaffRepository staffRepository = new StaffRepository())
            {
                leaveGroups = staffRepository.GetAllLeaveGroup();
            }
            var items = new List<SelectListItem>();

            items = leaveGroups.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllHolidayGroup()
        {
            List<HolidayGroupList> holidayGroups = new List<HolidayGroupList>();

            using (StaffRepository staffRepository = new StaffRepository())
            {
                holidayGroups = staffRepository.GetAllHolidayGroup();
            }
            var items = new List<SelectListItem>();

            items = holidayGroups.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<SelectListItem> GetAllWeeklyOffGroup()
        {
            List<WeeklyOffList> weeklyOffs = new List<WeeklyOffList>();

            using (StaffRepository staffRepository = new StaffRepository())
            {
                weeklyOffs = staffRepository.GetAllWeeklyOffGroup();
            }
            var items = new List<SelectListItem>();

            items = weeklyOffs.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public List<StaffFamilyInformation> GetStaffFamilyInformation(string staffid)
        {

            using (StaffRepository staffRepository = new StaffRepository())
            {
                return staffRepository.GetStaffFamilyInformation(staffid);
            }
        }

        public List<StaffEducationInformation> GetStaffEducationInformation(string staffid)
        {
            using (StaffRepository staffRepository = new StaffRepository())
            {
                return staffRepository.GetStaffEducationInformation(staffid);
            }
        }

        public List<SelectListItem> GetSalutation()
        {
            List<SalutationList> salutations = new List<SalutationList>();

            using (StaffRepository staffRepository = new StaffRepository())
            {
                salutations = staffRepository.GetSalutation();
            }

            var items = new List<SelectListItem>();
            items = salutations.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id,
                Selected = true
            }).ToList();

            return items;
        }

        public List<SelectListItem> GetPolicyList()
        {
            List<RuleGroupList> ruleGroups = new List<RuleGroupList>();

            using (StaffRepository staffRepository = new StaffRepository())
            {
                ruleGroups = staffRepository.GetPolicyList();
            }
            var items = new List<SelectListItem>();

            items = ruleGroups.Select(d => new SelectListItem()
            {
                Text = d.name,
                Value = d.id,
                Selected = true
            }).ToList();

            return items;
        }

        public string SaveStaffInformationRequest(Staff _Sta, StaffPersonal _SP, StaffOfficial _SO, EmployeePhoto _EP,List<AdditionalFieldValue> _AF, string loggedInUserStaffId)
        {
            using (StaffRepository staffRepository = new StaffRepository())
            {
                return staffRepository.SaveStaffInformationRequest(_Sta, _SP, _SO, _EP, _AF, loggedInUserStaffId);
            }
        }

        public List<StaffEditReqModel> GetStaffEditRequest(string Staffid)
        {
            using (StaffRepository staffRepository = new StaffRepository())
            {
                return staffRepository.GetStaffEditRequest(Staffid);
            }
        }

        
        public void SaveAdditionalField(AdditionalFieldModel objSt, string LoggedinStaffid)
        {
            using (StaffRepository staffRepository = new StaffRepository())
            {
                staffRepository.SaveAdditionalField(objSt, LoggedinStaffid);
            }
        }

        public List<AdditionalFieldModel> GetAdditionalFileds()
        {
            using (StaffRepository staffRepository = new StaffRepository())
            {
                return staffRepository.GetAdditionalFileds();
            }
        }

        public List<AdditionalFieldModel> GetAdditionalFiledsValues(string staffid)
        {
            using (StaffRepository staffRepository = new StaffRepository())
            {
                return staffRepository.GetAdditionalFiledsValues(staffid);
            }
        }

        /*self*/
        public List<SelectListItem> GetshiftList()
        {
            List<ShiftView> shifts = new List<ShiftView>();
            using (StaffRepository staffRepository = new StaffRepository())
            {
                shifts = staffRepository.GetshiftList();
            }
            var items = new List<SelectListItem>();

            items = shifts.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id,
                Selected = true
            }).ToList();

            return items;
        }

        public List<SelectListItem> Getshiftpattern()
        {
            List<ShiftView> shifts = new List<ShiftView>();
            using (StaffRepository staffRepository = new StaffRepository())
            {
                shifts = staffRepository.Getshiftpattern();
            }
            var items = new List<SelectListItem>();

            items = shifts.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id,
                Selected = true
            }).ToList();

            return items;
        }
        public string BulkSaveStaffInformationToDB(List<EmployeeImportModel> list)
        {
            using (StaffRepository staffRepository = new StaffRepository())
            {
                return staffRepository.BulkSaveStaffInformationToDB(list);
            }
        }
    }
}
