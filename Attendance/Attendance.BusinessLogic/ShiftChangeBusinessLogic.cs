using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic {
    public class ShiftChangeBusinessLogic {

        public List<ShiftChangeList> GetShiftChangeList(string StaffId, string ShiftChangeAccess)
        {
            using (var repo = new ShiftChangeRepository())
            { 
                var lst = repo.GetShiftChangeList(StaffId, ShiftChangeAccess);
            return lst;
            }
        }

        //public void SaveShiftChange(ShiftChangeApplication sca)
        //{
        //    var repo = new ShiftChangeRepository ( );
        //    repo.SaveShiftChange(sca);
        //}

        public void SaveShiftChangeInformation(CustomAttendanceData cattdada, string loggedInUserRole)
        {
            using (var repo = new ShiftChangeRepository())
                repo.SaveShiftChangeInformation(cattdada, loggedInUserRole);
        }

        public List<OldShift> GetOldShifts(string staffid, string fromdate, string todate)
        {
            using (var repo = new ShiftChangeRepository())
            { 
                var lst = repo.GetOldShifts(staffid, fromdate, todate);
            return lst;
            }
        }

        public string ShiftUserRule(string Txtstaffid, string SesstaffId)
        {
            using (var repo = new ShiftChangeRepository())
            { 
                var lst = repo.ShiftUserRule(Txtstaffid, SesstaffId);
            return lst;
            }
        } 

        public List<SelectListItem> GetShiftList( )
        {
            using (var repo = new ShiftChangeRepository())
            { 
                var lst = repo.GetShiftList();

            var item = new List<SelectListItem> ( );

            item = lst.Select ( i => new SelectListItem ( ) {
                Text = i.ShiftOutTime,
                Value = i.Id.ToString(),
                Selected = false
            } ).ToList ( ); 
            //item.Add(new SelectListItem(){Selected = true,Text = "-- Select Shift --",Value = "0"});
            return item;
            }
        }

        public List<SelectListItem> GetWeeklyoff()
        {using (var repo = new ShiftChangeRepository())
            {
                var lst = repo.GetWeeklyoff();
                var item = new List<SelectListItem>();
                item = lst.Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString(), Selected = false }).ToList();
                return item;
            }
        }
        public string SaveShiftExtensionDetails(ShiftExtensionAndDoubleShift se)
        {
            using (ShiftsRepository repo = new ShiftsRepository())
                return repo.SaveShiftExtensionDetails(se);
        }
    }
}
