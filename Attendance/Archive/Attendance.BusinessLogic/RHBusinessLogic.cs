using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;

namespace Attendance.BusinessLogic
{
    public class RHBusinessLogic
    {
        public void CancelRH(string ApplId)
        {
            using (RHRepository rHRepository = new RHRepository())
            {
                rHRepository.CancelRH(ApplId);
            }
        }

        public void SaveRHApplication(RHApplication rha)
        {
            using (RHRepository rHRepository = new RHRepository())
            {
                rHRepository.SaveRHApplication(rha);
            }
        }

        public List<RHHistory> GetRHApplicationHistory(string StaffId)
        {
            using (RHRepository rHRepository = new RHRepository())
            {
                return rHRepository.GetRHApplicationHistory(StaffId);
            }
        }

        public List<RestrictedHolidayList> GetRestrictedHolidays(string CompanyId, string StaffId)
        {
            using (RHRepository rHRepository = new RHRepository())
            {
                return rHRepository.GetRestrictedHolidays(CompanyId, StaffId);
            }
        }

        public string ValidateRHApplication(string StaffId)
        {
            using (RHRepository rHRepository = new RHRepository())
            {
                return rHRepository.ValidateRHApplication(StaffId);
            }
        }
    }
}
