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
            using (ShiftChangeRepository shiftChangeRepository = new ShiftChangeRepository())
            {
                return shiftChangeRepository.GetShiftChangeList(StaffId, ShiftChangeAccess);
            }
        }

        //public void SaveShiftChange(ShiftChangeApplication sca)
        //{
        //    var repo = new ShiftChangeRepository ( );
        //    repo.SaveShiftChange(sca);
        //}

        public void SaveShiftChangeInformation(CustomAttendanceData cattdada)
        {
            using (ShiftChangeRepository shiftChangeRepository = new ShiftChangeRepository())
            {
                shiftChangeRepository.SaveShiftChangeInformation(cattdada);
            }
        }

        public List<OldShift> GetOldShifts(string staffid, string fromdate, string todate)
        {
            using (ShiftChangeRepository shiftChangeRepository = new ShiftChangeRepository())
            {
                return shiftChangeRepository.GetOldShifts(staffid, fromdate, todate);
            }
        }

        public string ShiftUserRule(string Txtstaffid, string SesstaffId)
        {
            using (ShiftChangeRepository shiftChangeRepository = new ShiftChangeRepository())
            {
                return shiftChangeRepository.ShiftUserRule(Txtstaffid, SesstaffId);
            }
        } 

        public List<SelectListItem> GetShiftList( )
        {
            List<OldShift> oldShifts = new List<OldShift>();

            using (ShiftChangeRepository shiftChangeRepository = new ShiftChangeRepository())
            {
                oldShifts =  shiftChangeRepository.GetShiftList();
            }

            var item = new List<SelectListItem> ( );

            item = oldShifts.Select ( i => new SelectListItem ( ) {
                Text = i.ShortName,
                Value = i.Id.ToString(),
                Selected = false
            } ).ToList ( ); 
            item.Add(new SelectListItem(){Selected = true,Text = "-- Select Shift --",Value = "0"});
            return item;
        }

        public List<SelectListItem> GetWeeklyoff()
        {

            List<WeeklyOffList> weeklyOffLists = new List<WeeklyOffList>();

            using (ShiftChangeRepository shiftChangeRepository = new ShiftChangeRepository())
            {
                weeklyOffLists = shiftChangeRepository.GetWeeklyoff();
            }

            var item = new List<SelectListItem>();

            item = weeklyOffLists.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString(),
                Selected = false
            }).ToList();
            //item.Add(new SelectListItem() { Selected = true, Text = "-- Select WeeklyOff --", Value = "0" });

            return item;
        }

        //public string VaidateApplicationForPayPeriod (string FromDate , string ToDate)
        //{
        //    string validationMessage = string.Empty;
        //    var repo = new CommonRepository();
        //    validationMessage = repo.ValidateApplicationForPayDate(Convert.ToDateTime(FromDate) , Convert.ToDate);
        //    return validationMessage;
        //}
        public string CheckIsOTorCompOffApproved(string StaffId, string ApplicationDate)
        {
            string validationMessage = string.Empty;
            using (CommonRepository commonRepository = new CommonRepository())
            {
                validationMessage = commonRepository.CheckIsOTorCompOffApproved(StaffId, ApplicationDate);
        }
            return validationMessage;
        }

        public string SaveShiftExtensionDetails(ShiftExtensionAndDoubleShift se)
        {
            using (ShiftsRepository shiftsRepository = new ShiftsRepository())
            {
                return shiftsRepository.SaveShiftExtensionDetails(se);
            }
        }
    }
}
