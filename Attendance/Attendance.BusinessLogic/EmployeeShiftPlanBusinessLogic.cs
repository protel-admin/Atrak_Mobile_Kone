using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;
using Newtonsoft.Json;

namespace Attendance.BusinessLogic
{
    public class EmployeeShiftPlanBusinessLogic
    {
        public StaffView GetStaffInformation(string StaffId)
        {
            using (var repo = new EmployeeShiftPlanRepository())
            { 
                var data = repo.GetStaffInformation(StaffId);
            return data;
            }
        }

        public List<ShiftView> GetShifts()
        {
            using (var repo = new EmployeeShiftPlanRepository())
            { 
            var lst = repo.GetShifts();
            return lst;
            }
        }

        public List<ShiftPatternList> GetShiftPattern()
        {
            using (var repo = new EmployeeShiftPlanRepository())
            {
                var lst = repo.GetShiftPattern();
                return lst;
            }
        }

        public List<ShiftPatternList> GetShiftPatternForWeeklyPosting()
        {
            using (var repo = new EmployeeShiftPlanRepository())
            {
                var lst = repo.GetShiftPatternForWeeklyPosting();
                return lst;
            }
        }
        public List<WorkingDayPatternList> GetWorkingDayPattern()
        {
            using (var repo = new EmployeeShiftPlanRepository())
            {
                var lst = repo.GetWorkingDayPattern();
                return lst;
            }
        }

        public List<WeeklyOffList> GetWeeklyOffs()
        {
            using (var repo = new EmployeeShiftPlanRepository())
            { 
                var lst = repo.GetWeeklyOffs();
            return lst;
            }
        }

        public List<PermanantShiftChangeList> GetShiftChangeList()
        {
            using (var repo = new EmployeeShiftPlanRepository())
            { 
                var lst = repo.GetShiftChangeList();
            return lst;
            }
        }

        public void SaveInformation(EmployeeShiftPlan data ,AttendanceControlTable act)
        {
            using (var repo = new EmployeeShiftPlanRepository())
                repo.SaveInformation(data, act);
        }
        public string LoadDataBasedOnRandomStaff(string staffid, string stafflist, bool includetermination, string beginning, string ending)
        {
            using (var repo = new EmployeeShiftPlanRepository())
            { 
                var lst = repo.LoadDataBasedOnRandomStaff(staffid, stafflist, includetermination, beginning, ending);
            var str = ConvertToJsonString(lst);
            return str;
            }
        }
        public string ConvertToJsonString(List<StaffList> lst)
        {
            var jsonstring = new StringBuilder();
            var a = "";
            foreach (var v in lst)
            {
                jsonstring.Append(JsonConvert.SerializeObject(new
                {
                    staffid = v.StaffId,
                    staffname = v.StaffName,
                    department = v.Department,
                    location = v.Location,
                    volume = v.Volume,
                }));

                jsonstring.Append(",");
            }

            if (jsonstring.Length > 0)
            {
                //jsonstring.remo
                a = jsonstring.ToString();
                a = a.Substring(0, a.Length - 1);
            }

            return a;
        }
    }
}
