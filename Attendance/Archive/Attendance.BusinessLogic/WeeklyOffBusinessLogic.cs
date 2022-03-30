using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class WeeklyOffBusinessLogic
    {
        public List<WeeklyOffList> GetAllWeeklyOffs()
        {
            using (WeeklyOffRepository weeklyOffRepository = new WeeklyOffRepository())
            {
                return weeklyOffRepository.GetAllWeeklyOffs();
            }
        }

        public void SaveWeeklyOffInfo(WeeklyOffs wo)
        {
            using (WeeklyOffRepository weeklyOffRepository = new WeeklyOffRepository())
            {
                weeklyOffRepository.SaveWeeklyOffInfo(wo);
            }
        }
    }
}
