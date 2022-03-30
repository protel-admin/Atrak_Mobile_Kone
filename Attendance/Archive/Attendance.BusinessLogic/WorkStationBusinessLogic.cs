using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
   public  class WorkStationBusinessLogic
    {

       public List<WorkStationList> GetAllWorkStation()
        {
            using (WorkStationRepository workStationRepository = new WorkStationRepository())
            {
                return workStationRepository.GetAllWorkStation();
            }
        }

       public void saveWorkstationHistory(WorkstationAllocation wa)
       {
            using (WorkStationRepository workStationRepository = new WorkStationRepository())
            {
                workStationRepository.saveWorkstationHistory(wa);
            }
         
       }
       
    }
}
