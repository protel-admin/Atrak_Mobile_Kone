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
    public class AttendanceProcessingBusinessLogic
    {
        public List<SelectListItem> GetShiftLists()
        {
            List<ShiftList1> shifts = new List<ShiftList1>();
            using (AttendanceProcessingRepository attendanceProcessingRepository = new AttendanceProcessingRepository())
            {
                shifts = attendanceProcessingRepository.GetDurationList();
            }
            var item = new List<SelectListItem>();

            item = shifts.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString(),
                Selected = false
            }).ToList();

            return item;
        }

        public void ProcessAttendance(string StaffId, string FromDate, string ToDate)
        {
            using (AttendanceProcessingRepository attendanceProcessingRepository = new AttendanceProcessingRepository())
            {
                attendanceProcessingRepository.ProcessAttendance(StaffId, FromDate, ToDate);
            }
        }
        public string ProcessBacklogAttendance()
        {
            using (AttendanceProcessingRepository attendanceProcessingRepository = new AttendanceProcessingRepository())
            {
                return attendanceProcessingRepository.ProcessBacklogAttendance();
            }
        }
    }
}
