﻿using System;
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
        StaffRepository staffObj = new StaffRepository();
        RACoffAvailingApplicationRepository repo = new RACoffAvailingApplicationRepository();
       
        //rajesh TO BE ADDED IN THE WEB SOURCE MAY 16 2020
        public StaffOfficialInformationForApi GetStaffOfficialInformationForApi(string staffid)
        {
            using (var repo = new StaffRepository())
            { 
                var so = repo.GetStaffOfficialInformationForApi(staffid);
            //var photoData = await GetEmployeePhotoAsync(staffid);
            //if (photoData != null && photoData.EmpPhoto.Length > 0)
            //    so.PhotoB64String = Convert.ToBase64String(photoData.EmpPhoto);
            //else
            //    so.PhotoB64String = string.Empty;
            return so;
            }
        }

        public byte[] GetEmployeePhotoBytesAsync(string StaffId)
        {
            using (var repo = new StaffRepository())
            { 
                var photoData = repo.GetEmployeePhoto(StaffId);


            if (photoData != null)
                return photoData.EmpPhoto;
            else
                return null;
            }
        }
        public async Task<string> GetEmployeePhotoAsync(string StaffId)
        {
            using (var repo = new StaffRepository())
            { 
                var photoData = repo.GetEmployeePhoto(StaffId);
            string photoString = string.Empty;
            if (photoData != null && photoData.EmpPhoto.Length > 0)
                photoString = Convert.ToBase64String(photoData.EmpPhoto);
            return photoString;
            }
        }


        public string GetTotalDaysLeave(string StaffId, string LeaveStartDurationId, string FromDate, string ToDate, string LeaveEndDurationId, string LeaveTypeId)
        {
            using (StaffRepository staffObj = new StaffRepository())
                return staffObj.GetTotalDaysLeave(StaffId, LeaveStartDurationId, FromDate, ToDate, LeaveEndDurationId, LeaveTypeId);
        }
        #region Bulk Shift Import
        public string GetShiftNameBusinessLogic(string ShiftName)
        {
           using( StaffRepository staffObj = new StaffRepository())
            return staffObj.GetShiftNameRepository(ShiftName);
        }
        public string SaveBulkShiftsBusinessLogic(List<BulkShiftImportModel> model, string CreatedBy)
        {
            using (StaffRepository staffObj = new StaffRepository())
                return staffObj.SaveBulkShiftsRepository(model, CreatedBy);
        }
        public List<string> GetActiveShiftBusinessLogic()
        {
            using (StaffRepository staffObj = new StaffRepository())
                return staffObj.GetActiveShiftRepository();
        }
        #endregion
        public string ValidateCoffAvailing(string StaffId, string COffFromDate, string COffToDate, decimal TotalDays, string COffReqDate)
        {
          using(  RACoffAvailingApplicationRepository repo = new RACoffAvailingApplicationRepository())
            return repo.ValidateCoffAvailing(StaffId, COffFromDate, COffToDate, TotalDays, COffReqDate);
        }
        public string GetCoffReqPeriodBusinessLogic()
        {
            using (StaffRepository staffObj = new StaffRepository())
                return staffObj.GetCoffReqPeriodRepository();
        }
        public string GetUniqueId()
        {

            using (var repo = new RACoffRequestApplicationRepository())
                return repo.GetUniqueId();
        }
        public void SaveRequestApplication(ClassesToSave DataToSave)
        {
            using (RACoffRequestApplicationRepository repo = new RACoffRequestApplicationRepository())
                repo.SaveRequestApplication(DataToSave);
        }
        public List<SelectListItem> GetDurationListBusinessLogic()
        {
            var lst = staffObj.GetDurationListRepository();

            var item = new List<SelectListItem>();

            item = lst.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString(),
                Selected = false
            }).ToList();

            return item;
        }
        public StaffInformation GetStaffMainInformation(string staffid)
        {
            using (var repo = new StaffRepository())
            { 
                var si = repo.GetStaffMainInformation(staffid);
            return si;
            }
        }

        public StaffOfficialInformation GetStaffOfficialInformation(string staffid)
        {
            using (var repo = new StaffRepository())
            { 
                var so = repo.GetStaffOfficialInformation(staffid);
            return so;
            }
        }


        public StaffOfficialInformation GetstaffShiftPlanvalue(string staffid)
        {
            using (var repo = new StaffRepository())
            { 
                var so = repo.GetstaffShiftPlanvalue(staffid);
            return so;
            }
        }

        public StaffPersonalInformation GetStaffPersonalInformation(string staffid)
        {
            using (var repo = new StaffRepository())
            { 
                var sp = repo.GetStaffPersonalInformation(staffid);
            return sp;
            }
        }

        public string SaveStaffInformationToDB(Staff _Sta, StaffPersonal _SP, StaffOfficial _SO, EmployeePhoto _EP, List<AdditionalFieldValue> _AF, EmployeeShiftPlan _ESP, AttendanceControlTable _ACT, bool AddMode)
        {
            using (StaffRepository staffObj = new StaffRepository())
                return staffObj.SaveStaffInformationToDB(_Sta, _SP, _SO, _EP, _AF, _ESP, _ACT, AddMode);
        }

        public EmployeePhoto GetEmployeePhoto(string StaffId)
        {
            var repo = new StaffRepository();
            var Data = repo.GetEmployeePhoto(StaffId);

            if (Data != null)
            {
                return Data;
            }
            else
            {
                return null;
            }
        }
        public string GetLevelOfApprovalfromSettings()
        {
            using (StaffRepository staffObj = new StaffRepository())
                return staffObj.GetLevelOfApprovalfromSettings();
        }
        public List<SelectListItem> GetAllWorkingDayPattern()
        {
            var repo = new StaffRepository();
            var lst = repo.GetAllWorkingDayPattern();

            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.PsCode + d.PatternDesc,
                Value = d.Id.ToString(),
                Selected = true
            }).ToList();

            return items;
        }

        public List<SelectListItem> GetAllCompanies()
        {
            using (var repo = new StaffRepository())
            { 
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
        }

        public List<SelectListItem> GetAllBranches()
        {
            using (var repo = new StaffRepository())
            { 
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
        }

        public List<SelectListItem> GetAllDepartments()
        {
            using (var repo = new StaffRepository())
            { 
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
        }

        public List<SelectListItem> GetAllDivisions()
        {
            using (var repo = new StaffRepository())
            { 
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
        }

        public List<SelectListItem> GetAllVolumes()
        {
            using (var repo = new StaffRepository())
            { 
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
        }


        public List<SelectListItem> GetAllDesignations()
        {
            using (var repo = new StaffRepository())
            { 
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
        }

        public List<SelectListItem> GetAllGrades()
        {
            using (var repo = new StaffRepository())
            { 
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
        }

        public List<SelectListItem> GetAllCategories()
        {
            using (var repo = new StaffRepository())
            { 
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
        }

        public List<SelectListItem> GetAllCostCentres()
        {
            using (var repo = new StaffRepository())
            { 
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
        }

        public List<SelectListItem> GetAllSecurityGroup()
        {
          using(  var repo = new StaffRepository())
                { 
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
        }

        public List<SelectListItem> GetAllLocations()
        {
            using (var repo = new StaffRepository())
            { 
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
        }

        public List<SelectListItem> GetAllStatuses()
        {
            using (var repo = new StaffRepository())
            { 
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
        }

        public List<SelectListItem> GetAllMaritalStatus()
        {using (var repo = new StaffRepository())
            {
                var lst = repo.GetAllMaritalStatus();
                var items = new List<SelectListItem>();
                items = lst.Select(d => new SelectListItem() { Text = d.Name, Value = d.Id.ToString(), Selected = false }).ToList();
                return items;
            }
        }

        public List<SelectListItem> GetAllBloodGroup()
        {
            using (var repo = new StaffRepository())
            { 
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
        }


        public List<SelectListItem> GetAllLeaveGroup()
        {
            using (var repo = new StaffRepository())
            {    
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
        }

        public List<SelectListItem> GetAllHolidayGroup()
        {
            using (var repo = new StaffRepository())
            { 
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
        }

        public List<SelectListItem> GetAllWeeklyOffGroup()
        {
            using (var repo = new StaffRepository())
            { 
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
        }

        public List<StaffFamilyInformation> GetStaffFamilyInformation(string staffid)
        {
            using (var repo = new StaffRepository())
            { 
                var lst = repo.GetStaffFamilyInformation(staffid);
            return lst;
            }
        }

        public List<StaffEducationInformation> GetStaffEducationInformation(string staffid)
        {
            using (var repo = new StaffRepository())
            { 
                var lst = repo.GetStaffEducationInformation(staffid);
            return lst;
            }
        }

        public List<SelectListItem> GetSalutation()
        {
            using (var repo = new StaffRepository())
            {
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
        }

        public List<SelectListItem> GetPolicyList()
        {
            using (var repo = new StaffRepository())
            { 
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
        public List<SelectListItem> GetshiftList()
        {using (var repo = new StaffRepository())
            {
                var lst = repo.GetshiftList();
                var items = new List<SelectListItem>();
                items = lst.Select(d => new SelectListItem() { Text = d.Name, Value = d.Id, Selected = true }).ToList();
                return items;
            }
        }

        public List<SelectListItem> Getshiftpattern()
        {
            using (var repo = new StaffRepository())
            { 
                var lst = repo.Getshiftpattern();
            var items = new List<SelectListItem>();

            items = lst.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id,
                Selected = true
            }).ToList();

            return items;
        }
        }
        public List<SelectListItem> GetWorkStationList()
        {using (var repo = new StaffRepository())
            {
                var lst = repo.GetWorkStationList();
                var items = new List<SelectListItem>();
                items = lst.Select(d => new SelectListItem() { Text = d.Name, Value = d.Id, Selected = true }).ToList();
                return items;
            }
        }

        public string SaveStaffInformationRequest(Staff _Sta, StaffPersonal _SP, StaffOfficial _SO, EmployeePhoto _EP, List<AdditionalFieldValue> _AF, EmployeeShiftPlan _ESP, AttendanceControlTable _ACT, string loggedInUserStaffId)
        {
            using (var repo = new StaffRepository())
                return repo.SaveStaffInformationRequest(_Sta, _SP, _SO, _EP, _AF, _ESP, _ACT, loggedInUserStaffId);
        }
        public List<StaffEditReqModel> GetStaffEditRequest(string Staffid)
        {
            using (var repo = new StaffRepository())
                return repo.GetStaffEditRequest(Staffid);
        }
        public void SaveAdditionalField(AdditionalFieldModel objSt, string LoggedinStaffid)
        {
            using (var repo = new StaffRepository())
                repo.SaveAdditionalField(objSt, LoggedinStaffid);
        }
        public List<AdditionalFieldModel> GetAdditionalFileds()
        {
            using (var repo = new StaffRepository())
                return repo.GetAdditionalFileds();
        }
        public List<AdditionalFieldModel> GetAdditionalFiledsValues(string staffid)
        {
            using(var repo = new StaffRepository())
            return repo.GetAdditionalFiledsValues(staffid);
        }
        #region Bulk Masters Import
        public string GetMasterTableValueBusinessLogic(string MasterTable, string Name)
        {
            using (StaffRepository staffObj = new StaffRepository())
                return staffObj.GetMasterTableValueRepository(MasterTable, Name);
        }
        public string UpdateMastersBusinessLogic(List<BulkMasterImportModel> list, string MasterTable)
        {
            using (StaffRepository staffObj = new StaffRepository())
                return staffObj.UpdateMastersRepository(list, MasterTable);
        }
        #endregion

        #region Staff Removing Process
        public int ValidateStaffIdBusinessLogic(string StaffId)
        {
            using (StaffRepository staffObj = new StaffRepository())
                return staffObj.ValidateStaffIdRepository(StaffId);
        }
        public string UpdateBulkEmployeeRelieving(List<RemoveStaffModel> List)
        {
            using (StaffRepository staffObj = new StaffRepository())
                return staffObj.UpdateBulkEmployeeRelieving(List);
        }
        #endregion

    }
}
