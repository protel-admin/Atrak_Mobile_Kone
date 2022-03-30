using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;


namespace Attendance.BusinessLogic
{
    public class ShiftWeeklyPostingBusinessLogic
    {
        public List<ShiftView> GetAllShifts()
        {
            
            using (ShiftWeeklyPostingRepository shiftWeeklyPostingRepository = new ShiftWeeklyPostingRepository())
            {
                return shiftWeeklyPostingRepository.GetAllShifts();
                
            }
        }

        public List<ShiftPostingPatternList> GetShiftPostingPatternList(int PatternId)
        {
            using (ShiftWeeklyPostingRepository shiftWeeklyPostingRepository = new ShiftWeeklyPostingRepository())
            {
                return shiftWeeklyPostingRepository.GetShiftPostingPatternList(PatternId);
            }
        }

        public List<ShiftPatternList> GetShiftPattern()
        {
            using (ShiftWeeklyPostingRepository shiftWeeklyPostingRepository = new ShiftWeeklyPostingRepository())
            {
                return shiftWeeklyPostingRepository.GetShiftPattern();
            }
        }

        public void SavePostingInformation(ShiftWeeklyPosting data)
        {
            using (ShiftWeeklyPostingRepository shiftWeeklyPostingRepository = new ShiftWeeklyPostingRepository())
            {
                shiftWeeklyPostingRepository.SavePostingInformation(data);
            }
        }
    }
}
