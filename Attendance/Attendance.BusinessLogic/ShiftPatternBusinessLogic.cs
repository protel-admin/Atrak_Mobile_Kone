using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;

namespace Attendance.BusinessLogic
{
    public class ShiftPatternBusinessLogic
    {
        public void SaveInformation(ShiftPattern model)
        {
            var repo = new ShiftPatternRepository();
            repo.SaveInformation(model);
        }

        public List<ShiftPatternList> GetShiftPatternList()
        {
            var repo = new ShiftPatternRepository();
            var lst = repo.GetShiftPatternList();
            return lst;
        }
    }
}
