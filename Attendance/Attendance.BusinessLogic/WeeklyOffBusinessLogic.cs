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
            var repo = new WeeklyOffRepository();
            var lst = repo.GetAllWeeklyOffs();
            return lst;
        }

        public void SaveWeeklyOffInfo(WeeklyOffs wo)
        {
            var repo = new WeeklyOffRepository();
            repo.SaveWeeklyOffInfo(wo);
        }
    }
}
