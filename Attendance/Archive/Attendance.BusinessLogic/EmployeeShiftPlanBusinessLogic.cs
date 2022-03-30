using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class EmployeeShiftPlanBusinessLogic
    {
        public StaffView GetStaffInformation(string StaffId)
        {
            using (EmployeeShiftPlanRepository employeeShiftPlanRepository = new EmployeeShiftPlanRepository())
            {                
                return employeeShiftPlanRepository.GetStaffInformation(StaffId);            
            }
        }

        public List<ShiftView> GetShifts()
        {
            using (EmployeeShiftPlanRepository employeeShiftPlanRepository = new EmployeeShiftPlanRepository())
            {
                return employeeShiftPlanRepository.GetShifts();
            }
        }

        public List<ShiftPatternList> GetShiftPattern()
        {
            using (EmployeeShiftPlanRepository employeeShiftPlanRepository = new EmployeeShiftPlanRepository())
            {
                return employeeShiftPlanRepository.GetShiftPattern();
            }
        }

        public List<ShiftPatternList> GetShiftPatternForWeeklyPosting()
        {
            using (EmployeeShiftPlanRepository employeeShiftPlanRepository = new EmployeeShiftPlanRepository())
            {
                return employeeShiftPlanRepository.GetShiftPatternForWeeklyPosting();
            }
        }
        public List<WorkingDayPatternList> GetWorkingDayPattern()
        {
            using (EmployeeShiftPlanRepository employeeShiftPlanRepository = new EmployeeShiftPlanRepository())
            {
                return employeeShiftPlanRepository.GetWorkingDayPattern();
            }
        }

        public List<WeeklyOffList> GetWeeklyOffs()
        {
            using (EmployeeShiftPlanRepository employeeShiftPlanRepository = new EmployeeShiftPlanRepository())
            {
                return employeeShiftPlanRepository.GetWeeklyOffs();
            }
        }

        public List<PermanantShiftChangeList> GetShiftChangeList()
        {
            using (EmployeeShiftPlanRepository employeeShiftPlanRepository = new EmployeeShiftPlanRepository())
            {
                return employeeShiftPlanRepository.GetShiftChangeList();
            }
        }

        public void SaveInformation(EmployeeShiftPlan data ,AttendanceControlTable act)
        {
            using (EmployeeShiftPlanRepository employeeShiftPlanRepository = new EmployeeShiftPlanRepository())
            {
                employeeShiftPlanRepository.SaveInformation(data , act);
            }
        }
    }
}
