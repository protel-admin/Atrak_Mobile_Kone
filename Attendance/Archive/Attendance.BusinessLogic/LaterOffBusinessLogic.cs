using Attendance.Model;
using Attendance.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.BusinessLogic
{
    public class LaterOffBusinessLogic
    {
        //public List<LaterOffList> GetAllLaterOff()
        //{
        //    var repo = new LaterOffRepository();
        //    var lst = repo.GetAllLaterOff();
        //    return lst;
        //}

        public void CancelLaterOff(string ApplId)
        {
            using (LaterOffRepository laterOffRepository = new LaterOffRepository())
            {
                laterOffRepository.CancelLaterOff(ApplId);
            }
        }

        public string SaveCoffInfo(LaterOff LaterOff)
        {
            using (LaterOffRepository laterOffRepository = new LaterOffRepository())
            {
                laterOffRepository.SaveLaterOffInfo(LaterOff);
            }
            return null;
        }

        public string Getapproval(string Id)
        {
            using (CommonRepository commonRepository = new CommonRepository())
            {
                return commonRepository.GetApprovalStatus(Id);
            }
        }

        public List<ValidLaterOffDates> GetValidLaterOffDates(string StaffId)
        {
            using (LaterOffRepository laterOffRepository = new LaterOffRepository())
            {
                return laterOffRepository.GetValidLaterOffDates(StaffId);
            }
        }

        public List<LaterOffList> GetAllLaterOff(string staffid)
        {
            using (LaterOffRepository laterOffRepository = new LaterOffRepository())
            {
                return laterOffRepository.GetAllLaterOff(staffid);
            }
        }
        public string ValidateApplication(string StaffId, string FromDate, string ToDate)
        {
            using (LaterOffRepository laterOffRepository = new LaterOffRepository())
            {
                return laterOffRepository.ValidateApplication(StaffId, FromDate, ToDate);
            }
        }
        public void SaveLaterOffInfo(LaterOff co)
        {
            using (LaterOffRepository laterOffRepository = new LaterOffRepository())
            {
                laterOffRepository.SaveLaterOffInfo(co);
            }
        }
    }
}
