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
        ShiftChangeViewRepository repo = new ShiftChangeViewRepository();

        public List<SelectListItem> LoadFilterList(string companyid, string branchid, string departmentid,
                       string divisionid, string designationid, string gradeid,
                       string categoryid, string costcentreid, string locationid, string volumeid,string shortname)
        {
            var repo = new StaffDrillDownRepository();
            var lst = repo.LoadFilterList(companyid, branchid, departmentid,
                         divisionid, designationid, gradeid,
                         categoryid, costcentreid, locationid,volumeid, shortname);
            var item = new List<SelectListItem>();
            item = lst.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id,
                Selected = false
            }).ToList();
            return item;
        }
        public List<DropDownStrModel> GetShiftShortNameBusinessLogic()
        {
            return repo.GetShiftShortNameRepository();
        }

        public List<Dates> AllStaffShiftCalendar(string StaffId, string Fromdate, String Todate)
        {
            var repo = new ShiftChangeViewRepository();
            var lst = repo.AllStaffShiftCalendar(StaffId,Fromdate,Todate);
            return lst;
        }

        public List<StaffList> LoadReportingmangerwisestaff(string Staffid)
        {
            var repo = new ShiftChangeViewRepository();
            var lst = repo.LoadReportingmangerwisestaff(Staffid);
            return lst;
        }

        public List<StaffList> LoadDepartmentwise(string Departmentid, string staffid)
        {
            var repo = new ShiftChangeViewRepository();
            var lst = repo.LoadDepartmentwise(Departmentid,staffid);
            return lst;
        }

        public List<ShiftviewList> LoadShifts()
        {
            var repo = new ShiftChangeViewRepository();
            var lst = repo.LoadShifts();
            return lst;
        }

        public String UpdateAttendanceshift(string O_Staffid, string O_Shiftdate, string N_Shiftid, string N_Shiftshortname)
        {
            var repo = new ShiftChangeViewRepository();
            var lst = repo.UpdateAttendanceshift(O_Staffid, O_Shiftdate, N_Shiftid, N_Shiftshortname);
            return lst;
        }

        public List<SelectListItem> GetDepartmentList(string staffid)
        {

            var vrepo = new ShiftChangeViewRepository();
            var lst = vrepo.GetDepartmentList(staffid);
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

        public void UpdateAssignShifts(string staffid, string fromdate, string todate, string stafflist, string createdBy)
        {
            var vrepo = new ShiftChangeViewRepository();
            vrepo.UpdateAssignShifts(staffid, fromdate, todate, stafflist, createdBy);
        }

        public string UpdateAttendanceshiftGrid(List<ShiftChangeDetailViewModel> model, string UserId, string CreatedBy)
        {
            var vrepo = new ShiftChangeViewRepository();
            return vrepo.UpdateAttendanceshiftGrid(model, UserId, CreatedBy);
        }
    }
}
