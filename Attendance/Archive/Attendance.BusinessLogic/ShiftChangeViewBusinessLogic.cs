using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class ShiftChangeViewBusinessLogic
    {

        public List<SelectListItem> LoadFilterList(string companyid, string branchid, string departmentid,
                       string divisionid, string designationid, string gradeid,
                       string categoryid, string costcentreid, string locationid, string volumeid, string shortname, string role, string LocationId)
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


        public List<Dates> AllStaffShiftCalendar(string StaffId, string Fromdate, String Todate)
        {
            using (ShiftChangeViewRepository shiftChangeViewRepository = new ShiftChangeViewRepository())
            {
                return shiftChangeViewRepository.AllStaffShiftCalendar(StaffId, Fromdate, Todate);
            }
        }

        public List<StaffList> LoadReportingmangerwisestaff(string Staffid)
        {
            using (ShiftChangeViewRepository shiftChangeViewRepository = new ShiftChangeViewRepository())
            {
                return shiftChangeViewRepository.LoadReportingmangerwisestaff(Staffid);
            }
        }

        public List<StaffList> LoadDepartmentwise(string Departmentid, string staffid)
        {
            using (ShiftChangeViewRepository shiftChangeViewRepository = new ShiftChangeViewRepository())
            {
                return shiftChangeViewRepository.LoadDepartmentwise(Departmentid, staffid);
            }
        }

        public List<ShiftviewList> LoadShifts()
        {
            using (ShiftChangeViewRepository shiftChangeViewRepository = new ShiftChangeViewRepository())
            {
                return shiftChangeViewRepository.LoadShifts();
            }
        }

        public String UpdateAttendanceshift(string O_Staffid, string O_Shiftdate, string N_Shiftid, string N_Shiftshortname)
        {
            using (ShiftChangeViewRepository shiftChangeViewRepository = new ShiftChangeViewRepository())
            {
                return shiftChangeViewRepository.UpdateAttendanceshift(O_Staffid, O_Shiftdate, N_Shiftid, N_Shiftshortname);
            }
        }

        public List<SelectListItem> GetDepartmentList(string staffid)
        {
            List<DepartmentList> departments = new List<DepartmentList>();
            using (ShiftChangeViewRepository shiftChangeViewRepository = new ShiftChangeViewRepository())
            {
                departments = shiftChangeViewRepository.GetDepartmentList(staffid);
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

        public void UpdateAssignShifts(string staffid, string fromdate, string todate, string stafflist, string createdBy)
        {
            using (ShiftChangeViewRepository shiftChangeViewRepository = new ShiftChangeViewRepository())
            {
                shiftChangeViewRepository.UpdateAssignShifts(staffid, fromdate, todate, stafflist, createdBy);
            }
        }

        public string UpdateAttendanceshiftGrid(List<ShiftChangeDetailViewModel> model, string UserID)
        {
            using (ShiftChangeViewRepository shiftChangeViewRepository = new ShiftChangeViewRepository())
            {
                return shiftChangeViewRepository.UpdateAttendanceshiftGrid(model, UserID);
            }
        }

        public List<ShiftView> GetAllShifts()
        {
            using (ShiftChangeViewRepository shiftChangeViewRepository = new ShiftChangeViewRepository())
            {
                return shiftChangeViewRepository.GetAllShifts();
            }
        }
        public List<ShiftView> GetAllShifts(string role, string Location)
        {
            using (ShiftsRepository shiftsRepository = new ShiftsRepository())
            {
                return shiftsRepository.GetAllShifts(role,Location);
            }
        }
    }
}
