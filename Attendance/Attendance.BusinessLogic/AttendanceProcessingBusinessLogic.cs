//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Mvc;
//using Attendance.Model;
//using Attendance.Repository;

//namespace Attendance.BusinessLogic
//{
//    public class AttendanceProcessingBusinessLogic
//    {
//        public List<SelectListItem> GetShiftLists()
//        {
//            var repo = new AttendanceProcessingRepository();
//            var lst = repo.GetDurationList();

//            var item = new List<SelectListItem>();

//            item = lst.Select(i => new SelectListItem()
//            {
//                Text = i.Name,
//                Value = i.Id.ToString(),
//                Selected = false
//            }).ToList();

//            return item;
//        }
//        //self
//        public string SaveEmpAttendanceProcess(string StaffList, string FromDate,string ToDate,string CreatedById)
//        {
//            var repo = new AttendanceProcessingRepository();
//            return repo.SaveEmpAttendanceProcess(StaffList, FromDate, ToDate, CreatedById);
//        }
//        public void FromDateShouldBeGreaterthanToDate(DateTime FromDate, DateTime ToDate)
//        {
//            if (FromDate > ToDate)
//            {
//                throw new Exception("Starting date of your Attendance Processing must be greater than the Ending date.");
//            }
//        }
//    }
//}
