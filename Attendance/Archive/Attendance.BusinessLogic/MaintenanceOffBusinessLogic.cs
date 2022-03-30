using Attendance.Model;
using Attendance.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.BusinessLogic
{
    public class MaintenanceOffBusinessLogic
    {

        public string GetFinalDate(string StaffId, string FromDate, string ToDate, int Flag)
        {
            using (MaintenanceOffRepository maintenanceOffRepository = new MaintenanceOffRepository())
            {
                return maintenanceOffRepository.GetFinalDate(StaffId, FromDate, ToDate, Flag);

            }
        }
        public string ValidateMaintenanceOff(string StaffId, string FromDate, string ToDate, bool IsFixed)
        {
            using (MaintenanceOffRepository maintenanceOffRepository = new MaintenanceOffRepository())
            {
                return maintenanceOffRepository.ValidateMaintenanceOff(StaffId, FromDate, ToDate, IsFixed);
            }
        }

        public List<MOApplicableYear> GetMOffApplicableYear(string id)
        {
            using (MaintenanceOffRepository maintenanceOffRepository = new MaintenanceOffRepository())
            {
                return maintenanceOffRepository.GetMOffApplicableYear(id);
            }
        }

        public void CancelMOffApplication(string ApplicatinId)
        {
            using (MaintenanceOffRepository maintenanceOffRepository = new MaintenanceOffRepository())
            {
                maintenanceOffRepository.CancelMOffApplication(ApplicatinId);
            }
        }

        public List<MaintenanceOffList> GetAllMaintenanceOff(string id)
        {
            using (MaintenanceOffRepository maintenanceOffRepository = new MaintenanceOffRepository())
            {
                return maintenanceOffRepository.GetAllMaintenanceOff(id);
            }
        }

        public bool CanMOffBeOpened()
        {
            using (MaintenanceOffRepository maintenanceOffRepository = new MaintenanceOffRepository())
            {
                return maintenanceOffRepository.CanMOffBeOpened();
            }
        }

        public string GetFirstLetterGrade(string id)
        {
            using (MaintenanceOffRepository maintenanceOffRepository = new MaintenanceOffRepository())
            {
                return maintenanceOffRepository.GetFirstLetterGrade(id);
            }
        }

        public string SaveCoffInfo(MaintenanceOff MaintenanceOff)
        {
            using (MaintenanceOffRepository maintenanceOffRepository = new MaintenanceOffRepository())
            {
                maintenanceOffRepository.SaveMaintenanceOffInfo(MaintenanceOff);
            }
            return null;
        }

        public string GetLeaveBalBusi(string StaffId)
        {
            using (MaintenanceOffRepository maintenanceOffRepository = new MaintenanceOffRepository())
            {
                return maintenanceOffRepository.GetLeaveBalRep(StaffId);
            }
        }
        public string Getapproval(string StaffId)
        {
            using (MaintenanceOffRepository maintenanceOffRepository = new MaintenanceOffRepository())
            {
                return maintenanceOffRepository.GetApproval(StaffId);
            }
        }

        public void SaveMaintenanceOffInfo(MaintenanceOff co)
        {
            using (MaintenanceOffRepository maintenanceOffRepository = new MaintenanceOffRepository())
            {
                maintenanceOffRepository.SaveMaintenanceOffInfo(co);
            }
        }
    }
}
