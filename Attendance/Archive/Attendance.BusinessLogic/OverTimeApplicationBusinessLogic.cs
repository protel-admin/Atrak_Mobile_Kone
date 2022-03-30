using Attendance.Model;
using Attendance.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.BusinessLogic
{
    public class OverTimeApplicationBusinessLogic 
    {
        
        public List<RAOTRequestApplication> GetAppliedOverTimeRequestForMyTeam(string StaffId)
        {
            using (OverTimeRepository overTimeRepository = new OverTimeRepository())
            {
                return overTimeRepository.OTRequestApplication(StaffId);

            }
        }
        public string CancelApplication(string Id)
        {
            using (OverTimeRepository overTimeRepository = new OverTimeRepository())
            {
                return overTimeRepository.CancelApplication(Id);
            }
        }
    }
}
