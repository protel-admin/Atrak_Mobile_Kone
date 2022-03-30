using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class VisitorPassListBusinessLogic
    {
        public List<VisitAppointment> GetAppointmentList(string StaffId)
        {
            using (VisitorPassRepository visitorPassRepository = new VisitorPassRepository())
            {
                return visitorPassRepository.GetAppointmentList(StaffId);
            }
        }
    }
}
