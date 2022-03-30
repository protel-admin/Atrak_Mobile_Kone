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
            using (ShiftPatternRepository shiftPatternRepository = new ShiftPatternRepository())
            {
                shiftPatternRepository.SaveInformation(model);
            }
        }

        public List<ShiftPatternList> GetShiftPatternList()
        {
            using (ShiftPatternRepository shiftPatternRepository = new ShiftPatternRepository())
            {
                return shiftPatternRepository.GetShiftPatternList();
            }
        }
    }
}
