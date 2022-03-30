using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class LeaveTypeBusinessLogic
    {
        public List<LeaveView> GetAllLeaveTypes()
        {
            using (LeaveTypeRepository leaveTypeRepository = new LeaveTypeRepository())
            {     
                return leaveTypeRepository.GetLeaveView();
            }
        }

        public List<LeaveTypeMasterDetails> GetAllLeaveTypesFromMaster()
        {
            using (LeaveTypeRepository leaveTypeRepository = new LeaveTypeRepository())
            {
                return leaveTypeRepository.GetAllLeaveTypesFromMaster();
            }
        }
    }
}
