using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Attendance.Repository;
using Attendance.Model;

namespace Attendance.BusinessLogic
{
    public class RHEntryBusinessLogic
    {
        public List<RestrictedHolidayList> GetRestrictedHolidayList()
        {
            using (RHEntryRepository rHEntryRepository = new RHEntryRepository())
            {
                return rHEntryRepository.GetRestrictedHolidayList();                
            }
        }

        public void SaveRestrictedHolidayEntry(RestrictedHolidays data)
        {
            using (RHEntryRepository rHEntryRepository = new RHEntryRepository())
            {
               rHEntryRepository.SaveRestrictedHolidayEntry(data);
            }
        }

    }
}
